using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids2;
using DG.Tweening;

namespace HexCity {
	public class GameController : GridBehaviour<GridPoint2, TileCell> {
		public float speedScale = 1f;
		public int seed = 1;

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

		public enum GameState {
			Idle,
			Collecting,
			Falling,
			Interacting
		}

		private GameState state = GameState.Idle;
			
		public override void InitGrid() {
			DOTween.Init ();
//			Random.InitState (seed);

			foreach(var point in Grid) {
				point.cell.gameObject.name = point.point.ToString ();
				TileController thisController = point.cell.GetComponent<TileController> ();
				thisController.Type = GetRandomTileType ();
				thisController.Stage = TileStage.Empty;
			}
		}
			
		private int minTilesForMatch = 3;
		bool clearedTiles = false;
		GridPoint2? currentTile;
		private IGrid<GridPoint2, TileController> testBoard;
		private List<GridPoint2> collector = new List<GridPoint2>();
		public IEnumerator SearchForMatches() {
			state = GameState.Collecting;
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
							upgradedTile.transform.DOScale (Vector3.one, 1f * speedScale).SetEase (Ease.OutElastic);
						} else {
							GameObject clone = GameObject.Instantiate (Grid [pointToClear].gameObject);
							clone.transform.DOMove (Grid [oldestTilePoint.Value].transform.position, 0.25f * speedScale).SetEase (Ease.OutSine).OnComplete(() => Destroy(clone));
							ClearTile (pointToClear);
						}
					}
					clearedTiles = true;
				}
				currentTile = null;
				collector.Clear ();
			}
			yield return new WaitForSeconds (0.25f * speedScale);
			if (clearedTiles) {
				yield return StartCoroutine(Settle ());
			} else {
				state = GameState.Idle;
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

		public IEnumerator Settle() {
			state = GameState.Falling;
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
					CopyTileToTile (tileController, newController);
					Grid [firstXEmpty [p.point.X].Value].transform.DOMove (p.cell.transform.position, 0.25f * speedScale).From ().SetEase(Ease.InQuad);
					newController.Collected = false;
					ClearTile (p.point);
					// And then increment the firstXEmtpy's point's Y
					GridPoint2 newPoint = new GridPoint2 (p.point.X, firstXEmpty [p.point.X].Value.Y + 1);
					firstXEmpty [p.point.X] = newPoint;
				}
			}
			yield return new WaitForSeconds (1f * speedScale);
			AddNewTilesAtTop ();
			yield return new WaitForSeconds (1f * speedScale);
			yield return StartCoroutine(SearchForMatches ());
		}

		private void InitClearedArray() {
			firstXEmpty = new GridPoint2?[Grid.Bounds.Extreme.X];
			for (int x = 0; x < Grid.Bounds.Extreme.X; x++) {
				firstXEmpty [x] = null;
			}
		}

		private List<GameObject> fallingTiles;
		private void AddNewTilesAtTop() {
			fallingTiles = new List<GameObject> ();
			for (int x = 0; x < Grid.Bounds.Extreme.X; x++) {
				GridPoint2? firstClearedTile = GetFirstClearedTileInColumn (x);
				while (firstClearedTile.HasValue) {
					TileController tileController = Grid [firstClearedTile.Value].GetComponent<TileController> ();
					tileController.Stage = TileStage.Empty;
					tileController.Type = GetRandomTileType ();
					tileController.Collected = false;
					Grid [firstClearedTile.Value].transform.DOScale (Vector3.zero, ((0.25f + Random.value) * speedScale)).From ().SetEase(Ease.OutElastic);
					firstClearedTile = GetFirstClearedTileInColumn (x);
				}
			}
		}
			
		private TileType GetRandomTileType() {
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
			return newType;
		}

		private GridPoint2[] GetTilesInColumn(int col) {
			GridPoint2[] points = new GridPoint2[Grid.Bounds.Extreme.Y];
			int x = 0;
			foreach (var p in Grid) {
				if (p.point.X == col) {
					points [x] = p.point;
					x++;
				}
			}
			return points;
		}

		private GridPoint2 GetTopTileForColumn(int col) {
			GridPoint2[] points = GetTilesInColumn (col);

			GridPoint2 topPoint = new GridPoint2(col, -999);
			foreach (var p in points) {
				if (p.X > topPoint.X) {
					topPoint = p;
				}
			}

			return topPoint;
		}

		private GridPoint2? GetFirstClearedTileInColumn(int col) {
			GridPoint2[] points = GetTilesInColumn (col);
			GridPoint2? firstClearedTile = null;
			foreach (var p in points) {
				if (!firstClearedTile.HasValue) {
					// firstClearedTile has not been set
					if (TileIsCollected (p)) {
						// this tile has been collected
						firstClearedTile = p;
					}
				} else {
					// firstClearedTile is set
					if (TileIsCollected (p)) {
						// this tile has been collected
						if(p.Y < firstClearedTile.Value.Y) {
							firstClearedTile = p;
						}
					}
				}
			}

			return firstClearedTile;
		}

		private bool TileIsCollected(GridPoint2 p) {
			return Grid [p].GetComponent<TileController> ().Collected;
		}
		private GridPoint2? GetNewTileTarget(List<GridPoint2> points) {
			int youngestAge = 999999;
			GridPoint2? oldestTile = null;
			foreach (GridPoint2 p in points) {
				int myAge = GetAgeForPoint (p);
				if (myAge < youngestAge) {
					youngestAge = myAge;
					oldestTile = p;
				}
			}
			return oldestTile;
		}

		private int GetAgeForPoint(GridPoint2 p) {
			return Grid[p].GetComponent<TileController>().age;
		}


		public void StartDeselect(GridPoint2 p) {
			StartCoroutine(DeselectCenterTile(p));
		}
		public IEnumerator DeselectCenterTile(GridPoint2 p) {
			UnhighlightTile (selectedCenterTile);
			foreach (GameObject sprite in neighborSprites) {
				// Copy each tile to the tile it overlaps in grid
				GridPoint2 overlappedTile = GridMap.WorldToGridToDiscrete(sprite.transform.position - transform.position);
				if (Grid.Contains (overlappedTile)) {
					Grid [overlappedTile].GetComponent<TileController> ().Hide ();


					sprite.transform.DOMove (Grid [overlappedTile].transform.position, 0.25f * speedScale).SetEase (Ease.OutBack);
					sprite.GetComponent<SpriteRenderer> ().DOColor (new Color (1, 1, 1, 0), 0.1f * speedScale);
					yield return new WaitForSeconds (0.05f * speedScale);
					CopyTileToTile (sprite.GetComponent<TileController> (), Grid [overlappedTile].GetComponent<TileController> ());
					// Make it young again
					Grid [overlappedTile].GetComponent<TileController> ().age = -1;
				}

			}
			Destroy (neighborSpriteContainer);
			initialAngle = null;
			IncreaseAgeForAllTiles ();
			StartCoroutine(SearchForMatches ());
		}

		GridPoint2 selectedCenterTile;
		GridPoint2[] selectedNeighborTiles;
		GameObject[] neighborSprites;
		GameObject neighborSpriteContainer;
		public void SelectCenterTileForMovement(GridPoint2 p) {
			if (state != GameState.Idle) {
				return;
			}
			StructList<GridPoint2> neighbors = PointyHexPoint.GetOrthogonalNeighbors (p).In (Grid).ToStructList();
			if (neighbors.Count == 6) {
				state = GameState.Interacting;
				// point is surrounde dby neighbors, and is as such can be rotated around
				selectedCenterTile = p;
				selectedNeighborTiles = new GridPoint2[6];
				HighlightTile (p);
				int x = 0;
				foreach (GridPoint2 np in neighbors) {
					selectedNeighborTiles [x] = np;
					x++;
				}
				neighborSprites = new GameObject[6];
				neighborSpriteContainer = new GameObject ();
				neighborSpriteContainer.name = "Tile Rotator";
				neighborSpriteContainer.transform.position = Grid [p].transform.position;
				x = 0;
				foreach (GridPoint2 np in selectedNeighborTiles) {
					neighborSprites [x] = GameObject.Instantiate (Grid [np].gameObject, gameObject.transform);
					CopyTileToTile (Grid [np].gameObject.GetComponent<TileController> (), neighborSprites [x].GetComponent<TileController> ());
					neighborSprites [x].transform.parent = neighborSpriteContainer.transform;
					Grid [np].GetComponent<TileController> ().Hide ();
					neighborSprites[x].transform.DOScale(Vector3.one * 1.2f, 0.25f * speedScale).SetEase(Ease.OutSine).SetDelay(x * 0.025f * speedScale);
					x++;
				}
			}
		}

		private void HighlightTile(GridPoint2 p) {
			Grid [p].gameObject.transform.DOScale (Vector3.one * 1.2f, 0.25f * speedScale).SetEase(Ease.OutSine);
		}

		private void HighlightTile(GridPoint2 p, float delay) {
			Grid [p].gameObject.transform.DOScale (Vector3.one * 1.2f, 0.25f * speedScale).SetEase(Ease.OutSine).SetDelay(delay);
		}

		private void UnhighlightTile(GridPoint2 p) {
			Grid [p].gameObject.transform.DOKill (true);
			Grid [p].gameObject.transform.DOScale (Vector3.one, 0.1f * speedScale);
		}

		float? initialAngle;
		public void Update() {
			if(Input.GetKeyDown(KeyCode.Q)) {
				Debug.Break ();
			}
			float minDistanceToStartMovement = 1f;
			if (state == GameState.Interacting) {
				Vector3 centerPos = Grid [selectedCenterTile].gameObject.transform.position;
				Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Debug.DrawLine (centerPos, mousePos);
				if (Vector2.Distance (centerPos, mousePos) > minDistanceToStartMovement) {
					if (!initialAngle.HasValue) {
						initialAngle = Vector2.SignedAngle (Vector2.up, Grid [selectedCenterTile].gameObject.transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition));
					}
					float currentAngle = Vector2.SignedAngle (Vector2.up, Grid [selectedCenterTile].gameObject.transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition));
					float angle = currentAngle - initialAngle.Value;
					neighborSpriteContainer.transform.rotation = Quaternion.AngleAxis (angle, neighborSpriteContainer.transform.forward);
					foreach (GameObject sprite in neighborSprites) {
						sprite.transform.rotation = Quaternion.Euler (Vector3.zero);
					}
//					print (Mathf.Round (angle / 60f));
				}
			}
		}

		private void IncreaseAgeForAllTiles() {
			foreach (var p in Grid) {
				p.cell.GetComponent<TileController> ().age++;
			}
		}

		private void CopyTileToTile(TileController a, TileController b) {
			
			b.Stage = a.Stage;
			b.Type = a.Type;
			b.age = a.age;
			b.UpdatePresentation ();
		}
	}
}
