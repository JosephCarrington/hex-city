using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexCity{
	public class SpriteLibrary : MonoBehaviour {

		// Use this for initialization
		public Sprite[] blueSprites, greenSprites, greySprites, purpleSprites, redSprites, yellowSprites;
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public Sprite GetSprite(GameController.TileType type, GameController.TileStage stage) {
			int stageToGet = (int)stage;
			Sprite spriteToReturn = null;
			switch (type) {
			case GameController.TileType.Blue:
				spriteToReturn = blueSprites[stageToGet];
				break;
			case GameController.TileType.Green:
				spriteToReturn = greenSprites[stageToGet];
				break;
			case GameController.TileType.Grey:
				spriteToReturn = greySprites[stageToGet];
				break;
			case GameController.TileType.Purple:
				spriteToReturn = purpleSprites[stageToGet];
				break;
			case GameController.TileType.Red:
				spriteToReturn = redSprites[stageToGet];
				break;
			case GameController.TileType.Yellow:
				spriteToReturn = yellowSprites[stageToGet];
				break;
			}

			return spriteToReturn;

		}
	}
}
