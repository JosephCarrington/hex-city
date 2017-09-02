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
			StartCoroutine(SearchForMatches ());
		}

		private int minTilesForMatch = 3;
		bool clearedTiles = false;
		GridPoint2? currentTile;
		private IGrid<GridPoint2, TileController> testBoard;
		private List<GridPoint2> collector = new List<GridPoint2>();
		private IEnumerator SearchForMatches() {
			collector.Clear ();
			clearedTiles = false;
			currentTile = null;
			testBoard = Grid.CloneStructure (p => Grid[p].GetRequiredComponent<TileController>());

			foreach (PointCellPair<GridPoint2, TileController> p in testBoard) {
				TestTile (p.point);
				if (collector.Count >= minTilesForMatch) {
					GridPoint2? oldestTilePoint = GetNewTileTarget (collector);
					foreach (GridPoint2 pointToClear in collector) {
						GameObject tileClone = GameObject.Instantiate(Grid[pointToClear].gameObject, Grid[pointToClear].transform.position, Quaternion.identity);
						if (pointToClear == oldestTilePoint) {
							GameObject upgradedTile = Grid [pointToClear].gameObject;
							upgradedTile.GetComponent<TileController> ().IncreaseStage ();
							upgradedTile.transform.localScale = Vector3.zero;
							upgradedTile.transform.DOScale (Vector3.one, 1f).SetEase (Ease.OutElastic);

						} else {
							Grid [pointToClear].GetComponent<TileController> ().Collected = true;
						}
						tileClone.transform.DOMove(Grid[oldestTilePoint.Value].transform.position, 0.1f).SetDelay(Random.value * 0.25f).SetEase(Ease.OutCubic).OnComplete(() => {
							Destroy(tileClone);
						});
					}
					clearedTiles = true;
				}
				currentTile = null;
				collector.Clear ();
//				yield return null;
			}

			if (clearedTiles) {
				yield return StartCoroutine (Settle ());
			}
		}

		private GridPoint2? firstEmpty;
		private List<GridPoint2> fallingBlocks = new List<GridPoint2> ();
		private IEnumerator Settle() {
//			fallingBlocks.Clear ();
//			foreach (var p in dataGrid) {
//				if (p.cell.collected && !firstEmpty.HasValue) {
//					firstEmpty = p.point;
//				} else if (firstEmpty.HasValue && !p.cell.collected) {
//					fallingBlocks.Add (p.point);
////					TileController firstController = dataGrid [new GridPoint2 (p.point.X, firstEmpty.Value)].gameObject.GetComponent<TileController> ();
////					TileController thisController = Grid[p.point].GetComponent<TileController> ();
////					firstController.stage = thisController.stage;
////					firstController.age = thisController.age;
////					firstController.type = thisController.type;
//					if (dataGrid.Contains(new GridPoint2(p.point.X, firstEmpty.Value.Y))) {
//						Transform myT = dataGrid [new GridPoint2 (p.point.X, firstEmpty.Value.Y)].transform;
//						dataGrid [new GridPoint2 (p.point.X, firstEmpty.Value.Y)] = dataGrid [p.point];
////						dataGrid [p.point].transform.DOMove (, 0.25f).SetEase (Ease.InOutQuad);
////					dataGrid [new GridPoint2 (p.point.X, firstEmpty.Value)].GetComponent<SpriteRenderer> ().sprite = p.cell.GetComponent<SpriteRenderer> ().sprite;
//
//						firstEmpty = new GridPoint2 (firstEmpty.Value.X, firstEmpty.Value.Y + 1);
//					}
//			
//
//				}
//			}
//			if (fallingBlocks.Count > 0) {
//				foreach (GridPoint2 fallingBlock in fallingBlocks) {
//					Transform currentTranform = dataGrid [fallingBlock].transform;
////					currentTranform.DOMove(new Vector3(currentTranform.position.x, dataGrid[fallingBlock].last
//				}
//			}

			fallingBlocks.Clear ();
			foreach (var p in Grid) {
				TileController thisController = Grid [p.point].GetComponent<TileController> ();
				if (thisController.Collected && !firstEmpty.HasValue) {
					// If we dont have an empty cell for this column yet, and this cell is marked as empty
					firstEmpty = p.point;
				}else if (firstEmpty.HasValue && thisController.Collected){
					// We have an empty cell below this cell, and this cell is marked as collected
					// Add this cell to fallingBLocks, so that it can be animated
					fallingBlocks.Add(p.point);
					// copy this cell to the first empty cell we found
					CopyCellToCell(p.point, firstEmpty.Value);
					// Mark the copied over cell to not have been collected, so we don't trip over it later
//					MarkCellUncollected(firstEmpty.Value);
					// Finally change firstEmpty to this cell
					firstEmpty = p.point;
				}

			}
			yield return null;

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

		private void CopyCellToCell(GridPoint2 a, GridPoint2 b) {
			TileController aController = Grid [a].GetComponent<TileController> (),
			bController = Grid [b].GetComponent<TileController> ();
			bController.age = aController.age;
			bController.Type = aController.Type;
			bController.Stage = aController.Stage;
		}

		private void MarkCellUncollected(GridPoint2 cell) {
			Grid [cell].GetComponent<TileController>().Collected = false;
		}
	}
}
