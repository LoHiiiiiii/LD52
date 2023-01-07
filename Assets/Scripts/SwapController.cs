using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwapController : MonoBehaviour {
	[SerializeField] PostProcessProfile colorSwap;
	[SerializeField] SwapProfile[] profiles;
	
	[System.Serializable]
	struct SwapProfile {
		public Color primary;
		public Color secondary;
		public Color tertiary;
	}

	int currentSwapIndex;

	private void Awake() {
		currentSwapIndex = profiles.Length; //Any index is possible at start;
		SwapRandom();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) SwapRandom();
	}

	public void SwapRandom() {
		int index = Random.Range(0, profiles.Length - 1);
		if (index >= currentSwapIndex) index++;

		SwapColors(index);
	}

	void SwapColors(int index) {
		if (colorSwap == null || index >= profiles.Length) return;
		currentSwapIndex = index;
		var settings = colorSwap.GetSetting<ColorSwap>();
		settings.redSwap.value = profiles[currentSwapIndex].primary;
		settings.greenSwap.value = profiles[currentSwapIndex].secondary;
		settings.blueSwap.value = profiles[currentSwapIndex].tertiary;
		settings.restSwap.value = profiles[currentSwapIndex].tertiary;
	}

}
