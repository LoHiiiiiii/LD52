using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwapController : MonoBehaviour {
	[SerializeField] PostProcessProfile colorSwap;
	[SerializeField] SwapProfile[] profiles;
	[SerializeField] Curtain curtain;
	[SerializeField] SwapProfile spookyProfile;

	[System.Serializable]
	struct SwapProfile {
		public Color primary;
		public Color secondary;
		public Color tertiary;
	}
	public int CurrentSwapIndex { get; private set; }
	bool spooky;

	private void Awake() {
		CurrentSwapIndex = profiles.Length; //Any index is possible at start;
	}

	public void SwapRandom() {
		int index = Random.Range(0, profiles.Length - 1);
		if (index >= CurrentSwapIndex) index++;

		if (colorSwap == null || index >= profiles.Length) return;
		spooky = false;
		CurrentSwapIndex = index;
		SetProfile(profiles[CurrentSwapIndex]);
		Transition();
	}

	public void SwapSpooky() {
		SetProfile(spookyProfile);
		spooky = true;
		Transition();
	}

	public void Transition() {
		if (curtain != null) {
			curtain.FadeOut(spooky ? spookyProfile.tertiary : profiles[CurrentSwapIndex].secondary);
		}
	}

	void SetProfile(SwapProfile profile) {

		var settings = colorSwap.GetSetting<ColorSwap>();
		settings.redSwap.value = profile.primary;
		settings.greenSwap.value = profile.secondary;
		settings.blueSwap.value = profile.tertiary;
		settings.restSwap.value = profile.tertiary;
	}

}
