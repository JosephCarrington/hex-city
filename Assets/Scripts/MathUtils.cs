using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexCity {
	public class MathUtils : MonoBehaviour {
		public static float GetBetweenValue(float min, float max, float current) {
			return(current - min) / (max - min);
		}
	}
}