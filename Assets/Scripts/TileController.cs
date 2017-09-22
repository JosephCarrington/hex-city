using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids2;
namespace HexCity {
	[RequireComponent(typeof(SpriteRenderer))]
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

		private bool collected;
		public bool Collected {
			get {
				return collected;
			}
			set {
				collected = value;
				UpdatePresentation ();
			}
		}

		public int age = 0;
		public void UpdatePresentation() {
			if (!collected) {
				gameObject.GetComponent<SpriteRenderer> ().enabled = true;
				gameObject.GetComponent<SpriteRenderer> ().sprite = gameObject.GetComponentInParent<SpriteLibrary> ().GetSprite (type, stage);
			} else {
				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}

		public void IncreaseStage() {
			Stage = (GameController.TileStage)((int)stage + 1);

		}
	}
}