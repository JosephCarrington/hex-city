using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids2;
namespace HexCity {
	public class TileController : TileCell {
		private GameController.TileType type;
		private GameController.TileStage stage;
		public GameController.TileType Type {
			get { 
				return type;
			}
			set {
				type = value;
				UpdatePresentation ();
			}
		}
			
		public GameController.TileStage Stage {
			get {
				return stage;
			}
			set {
				stage = value;
				UpdatePresentation ();
			}
		}

		public int age = 0;
		private bool collected = false;
		public bool Collected {
			get {
				return collected;
			}
			set {
				collected = value;
				UpdatePresentation ();
			}
		}

		public void UpdatePresentation() {
			if (Collected) { 
				gameObject.GetComponent<SpriteRenderer> ().sprite = null;
			} else {
				gameObject.GetComponent<SpriteRenderer> ().sprite = gameObject.GetComponentInParent<SpriteLibrary> ().GetSprite (type, stage);
			}
		}

		public void IncreaseStage() {
			stage = (GameController.TileStage)((int)stage + 1);
		}
	}
}