using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids2;
namespace HexCity {
	public class TileController : TileCell {
		public GameController.TileType type;
		public Sprite[] stageSprites;
		public GameController.TileStage stage = GameController.TileStage.Empty;
		public int age = 0;
		public bool collected = false;

		public void Hide() {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}

		public void Show() {
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		}

		public void IncreaseStage() {
			stage = (GameController.TileStage)((int)stage + 1);
			gameObject.GetComponent<SpriteRenderer> ().sprite = stageSprites [(int)stage];
		}
	}
}