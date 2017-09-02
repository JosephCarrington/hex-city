﻿using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Grids2.Examples.Algorithms.PathFinding
{
	public enum NeighborSetup
	{
		Orthogonal,
		Diagonal,
		OrthogonalAndDiagonal
	}

	public class PathFindingRectGrid : GridBehaviour<GridPoint2, TileCell>
	{
		[Header("Path Parameters")]
		public PathMode pathMode;
		public NeighborSetup neighborSetup;
		public SpriteCell pathPrefab;

		[Header("Scene Setup")]
		public GameObject pathRoot;
		//public Texture2D perlinNoiseTexture;

		[Header("Colors")]
		public Gradient weightGradient;

		public Color blockedColor = Color.black;
		public Color pathColor = Utils.DefaultColors[6];
		public Color startColor = Utils.DefaultColors[5];
		public Color goalColor = Utils.DefaultColors[7];

		private GridPoint2 start;
		private GridPoint2 goal;
		private bool selectStart = true; //otherwise, select goal

		private Grid2<WalkableCell> walkableGrid;

		public override void InitGrid()
		{
			walkableGrid = (Grid2<WalkableCell>)Grid.CloneStructure<WalkableCell>();
			//var imageGrid = GridUtils.ImageToGreyScaleGrid(perlinNoiseTexture);

			foreach (var gridPoint2 in walkableGrid.Points)
			{
				walkableGrid[gridPoint2] = Grid[gridPoint2].GetComponent<WalkableCell>();
				walkableGrid[gridPoint2].IsWalkable = true;

				var movementCost =
					+ Mathf.PerlinNoise(gridPoint2.X * .1f, gridPoint2.Y * .1f) / 2
					+ Mathf.PerlinNoise(gridPoint2.X * .2f, gridPoint2.Y * .2f) / 4
					+ Mathf.PerlinNoise(gridPoint2.X * .4f, gridPoint2.Y * .4f) / 8
					+ Mathf.PerlinNoise(gridPoint2.X * .8f, gridPoint2.Y * .8f) / 16
					; //1 + Random.value*2;

				walkableGrid[gridPoint2].MovementCost = movementCost;
			}

			var min = walkableGrid.Cells.Min(c => c.MovementCost);
			var max = walkableGrid.Cells.Max(c => c.MovementCost);

			foreach (var walkableCell in walkableGrid.Cells)
			{
				if (min != max)
					walkableCell.MovementCost = 1 * (walkableCell.MovementCost - min) / (max - min);

				walkableCell.Color = weightGradient.Evaluate(walkableCell.MovementCost);
			}

			start = walkableGrid.First().point;
			goal = walkableGrid.Last().point;

			UpdatePath();
		}

		/// <summary>
		/// Returns the Euclidean distance (in world units)
		/// between the given grid points
		/// </summary>
		private float EuclideanDistance(GridPoint2 p, GridPoint2 q)
		{
			float dX = GridMap.GridToWorld(p).x - GridMap.GridToWorld(q).x;
			float dY = GridMap.GridToWorld(p).y - GridMap.GridToWorld(q).y;

			float distance = Mathf.Sqrt(dX * dX + dY * dY);

			return distance;
		}

		public void OnLeftClick(GridPoint2 clickedPoint)
		{
			if (clickedPoint == start || clickedPoint == goal) return;

			ToggleCellWalkability(clickedPoint);
			UpdatePath();
		}

		public void OnRightClick(GridPoint2 clickedPoint)
		{
			if (!walkableGrid[clickedPoint].IsWalkable) return;

			SetStartOrGoal(clickedPoint);
			UpdatePath();
		}

		private void ToggleCellWalkability(GridPoint2 clickedPoint)
		{
			walkableGrid[clickedPoint].IsWalkable = !walkableGrid[clickedPoint].IsWalkable;

			var color = walkableGrid[clickedPoint].IsWalkable
				? weightGradient.Evaluate(walkableGrid[clickedPoint].MovementCost)
				: blockedColor;
			walkableGrid[clickedPoint].Color = color;
		}

		private void SetStartOrGoal(GridPoint2 clickedPoint)
		{
			if (selectStart && clickedPoint != goal)
			{
				start = clickedPoint;
				selectStart = false;
			}
			else if (clickedPoint != start)
			{
				goal = clickedPoint;
				selectStart = true;
			}
		}

		public IEnumerable<GridPoint2> GetGridPath()
		{
			//We use the original grid here, and not the
			//copy, to preserve neighbor relationships. Therefore, we
			//have to cast the cell in the lambda expression below.
			var path = Grids2.Algorithms.AStar(
				walkableGrid,
				start,
				goal,
				(p, q) => RectPoint.ManhattanNorm(p - q),
				c => walkableGrid[c].IsWalkable,
				GetNeighbors,
				(p, q) => 1);

			return path;
		}

		public IEnumerable<GridPoint2> GetEuclideanPath()
		{
			var path = Grids2.Algorithms.AStar(
				walkableGrid,
				start,
				goal,
				EuclideanDistance,
				c => walkableGrid[c].IsWalkable,
				GetNeighbors,
				EuclideanDistance);

			return path;
		}

		public IEnumerable<GridPoint2> GetWeightedPath()
		{
			//We use the original grid here, and not the
			//copy, to preserve neighbor relationships. Therefore, we
			//have to cast the cell in the lambda expression below.
			var path = Grids2.Algorithms.AStar(
				walkableGrid,
				start,
				goal,
				(p, q) => RectPoint.ManhattanNorm(p - q) * WalkableCell.MinCost,
				c => walkableGrid[c].IsWalkable,
				GetNeighbors,
				GetMovementCost);

			return path;
		}

		/// <summary>
		/// Gets the cost of moving between the cells at the given grid points,
		/// asuming cells are neighbors.
		/// </summary>
		private float GetMovementCost(GridPoint2 p1, GridPoint2 p2)
		{
			return (walkableGrid[p1].MovementCost +
					walkableGrid[p2].MovementCost) / 2;
		}

		private void UpdatePath()
		{
			if (Application.isPlaying)
			{
				pathRoot.transform.DestroyChildren();
			}
			else
			{
				pathRoot.transform.DestroyChildrenImmediate();
			}

			IEnumerable<GridPoint2> path = null;

			switch (pathMode)
			{
				case PathMode.GridPath:
					path = GetGridPath();
					break;
				case PathMode.EuclideanPath:
					path = GetEuclideanPath();
					break;
				case PathMode.WeightedPath:
					path = GetWeightedPath();
					break;
			}

			if (path == null)
			{
				//then there is no path between the start and goal.
				return;
			}

			foreach (var point in path)
			{
				var pathNode = Instantiate(pathPrefab);

				pathNode.transform.parent = pathRoot.transform;
				pathNode.transform.localScale = Vector3.one * 0.5f;
				pathNode.transform.localPosition = GridMap.GridToWorld(point);

				if (point == start)
				{
					pathNode.Color = startColor;
				}
				else if (point == goal)
				{
					pathNode.Color = goalColor;
				}
				else
				{
					pathNode.Color = pathColor;
				}
			}
		}

		private IEnumerable<GridPoint2> GetNeighbors(GridPoint2 point)
		{
			switch(neighborSetup)
			{
				case NeighborSetup.Orthogonal:
					return RectPoint.GetOrthogonalNeighbors(point);
				case NeighborSetup.Diagonal:
					return RectPoint.GetDiagonalNeighbors(point);
				default:
				case NeighborSetup.OrthogonalAndDiagonal:
					return RectPoint.GetOrthogonalAndDiagonalNeighbors(point);
			}

		}
	}
}