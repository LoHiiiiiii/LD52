using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwapController : MonoBehaviour {
	[SerializeField] PostProcessProfile colorSwap;
	[SerializeField] SwapProfile[] profiles;
	[SerializeField] Curtain curtain;

	[System.Serializable]
	struct SwapProfile {
		public Color primary;
		public Color secondary;
		public Color tertiary;
	}
	public int CurrentSwapIndex { get; private set; }

	private void Awake() {
		CurrentSwapIndex = profiles.Length; //Any index is possible at start;
	}
	
	public void SwapRandom() {
		int index = Random.Range(0, profiles.Length - 1);
		if (index >= CurrentSwapIndex) index++;

		SwapColors(index);
		Transition();
	}

	public void Transition() {
		if (curtain != null) {
			curtain.FadeOut(profiles[CurrentSwapIndex].secondary);
		}
	}

	void SwapColors(int index) {
		if (colorSwap == null || index >= profiles.Length) return;
		CurrentSwapIndex = index;
		var settings = colorSwap.GetSetting<ColorSwap>();
		settings.redSwap.value = profiles[CurrentSwapIndex].primary;
		settings.greenSwap.value = profiles[CurrentSwapIndex].secondary;
		settings.blueSwap.value = profiles[CurrentSwapIndex].tertiary;
		settings.restSwap.value = profiles[CurrentSwapIndex].tertiary;
	}

}
