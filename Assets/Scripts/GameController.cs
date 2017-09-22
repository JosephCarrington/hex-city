using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids2;
using DG.Tweening;

namespace HexCity {
	public class GameController : GridBehaviour<GridPoint2, TileCell> {

		// Use this for initialization
		public enum TileType {
			Blue,
			Grey,
			Green,
			Purple,
			Red,
			Yellow,
			NONE
		}

		public int totalTypes = 6;

		public enum TileStage {
			Empty,
			Zoned,
			Suburban,
			Urban,
			Max
		}
			
		public override void InitGrid() {
			DOTween.Init ();
			Random.InitState (1);

			foreach(var point in Grid) {
				point.cell.gameObject.name = point.point.ToString ();
				int randomType = Random.Range (0, totalTypes);
				TileType newType = TileType.Blue;
				switch (randomType) {
				case 1: 
					newType = TileType.Grey;
					break;
				case 2: 
					newType = TileType.Green;
					break;
				case 3: 
					newType = TileType.Purple;
					break;
				case 4: 
					newType = TileType.Red;
					break;
				case 5: 
					newType = TileType.Yellow;
					break;
				}
				TileController thisController = point.cell.GetComponent<TileController> ();
				thisController.Type = newType;
				thisController.Stage = TileStage.Empty;
			}
		}

		public void LogClick(GridPoint2 point) {
			
		}

		private int minTilesForMatch = 3;
		bool clearedTiles = false;
		GridPoint2? currentTile;
		private IGrid<GridPoint2, TileController> testBoard;
		private List<GridPoint2> collector = new List<GridPoint2>();
		public void SearchForMatches() {
			collector.Clear ();
			clearedTiles = false;
			currentTile = null;
			testBoard = Grid.CloneStructure (p => Grid[p].GetRequiredComponent<TileController>());

			foreach (PointCellPair<GridPoint2, TileController> p in testBoard) {
				TestTile (p.point);
				if (collector.Count >= minTilesForMatch) {
					GridPoint2? oldestTilePoint = GetNewTileTarget (collector);
					foreach (GridPoint2 pointToClear in collector) {
						if (pointToClear == oldestTilePoint) {
							GameObject upgradedTile = Grid [pointToClear].gameObject;
							upgradedTile.GetComponent<TileController> ().IncreaseStage ();
							upgradedTile.transform.localScale = Vector3.zero;
							upgradedTile.transform.DOScale (Vector3.one, 1f).SetEase (Ease.OutElastic);
						} else {
							ClearTile (pointToClear);
						}
					}
					clearedTiles = true;
				}
				currentTile = null;
				collector.Clear ();
			}

			if (clearedTiles) {
			}
		}

		private void ClearTile(GridPoint2 gp) {
			Grid [gp].GetComponent<TileController> ().Collected = true;
		}
		private void TestTile(GridPoint2 p) {
			if (testBoard [p] == null) {
				return;
			}


			if (currentTile == null) {
				currentTile = p;
				testBoard [p] = null;
				collector.Add (currentTile.Value);
			}
			// Tile doesn't match. Skip
			else if (Grid[currentTile.Value].GetComponent<TileController>().Type != testBoard [p].Type || Grid[currentTile.Value].GetComponent<TileController>().Stage != testBoard [p].Stage) {
				return;
			}
			//Tile matches
			else {
				collector.Add (p);
				testBoard [p] = null;
			}
			foreach (GridPoint2 neighborPoint in PointyHexPoint.GetOrthogonalNeighbors (p).In (testBoard)) {
				TestTile (neighborPoint);
			}
		}

		private GridPoint2?[] firstXEmpty;
		public void Settle() {
			InitClearedArray ();
			foreach (var p in Grid) {
				TileController tileController = p.cell.GetComponent<TileController> ();
				if (!firstXEmpty [p.point.X].HasValue && tileController.Collected) {
					// There is not am empty cell below me, and I am empty
					firstXEmpty [p.point.X] = p.point;
				}
				else if (firstXEmpty [p.point.X].HasValue && !tileController.Collected) {
					// There is an empty cell below me, and I am not empty
					TileController newController = Grid [firstXEmpty [p.point.X].Value].gameObject.GetComponent<TileController> ();
					newController.age = tileController.age;
					newController.Type = tileController.Type;
					newController.Stage = tileController.Stage;
					newController.Collected = false;
					ClearTile (p.point);
					// And then increment the firstXEmtpy's point's Y
					GridPoint2 newPoint = new GridPoint2 (p.point.X, firstXEmpty [p.point.X].Value.Y + 1);
					firstXEmpty [p.point.X] = newPoint;
				}
			}
		}

		private void InitClearedArray() {
			firstXEmpty = new GridPoint2?[Grid.Bounds.Extreme.X];
			for (int x = 0; x < Grid.Bounds.Extreme.X; x++) {
				firstXEmpty [x] = null;
			}
		}

			
		private GridPoint2? GetNewTileTarget(List<GridPoint2> points) {
			int oldestAge = -1;
			GridPoint2? oldestTile = null;
			foreach (GridPoint2 p in points) {
				int myAge = GetAgeForPoint (p);
				if (myAge > oldestAge) {
					oldestAge = myAge;
					oldestTile = p;
				}
			}
			return oldestTile;
		}

		private int GetAgeForPoint(GridPoint2 p) {
			return Grid[p].GetComponent<TileController>().age;
		}
			
	}
}
