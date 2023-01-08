using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	[SerializeField] float duration;
	[SerializeField] float maxOrthoMultiplier;
	[SerializeField] AnimationCurve transitionCurve;
	[SerializeField] Transform cameraCanvas;
	[SerializeField] Transform aura;

	float unscaledSize;
	float baseSize;

	private void Awake() {
		unscaledSize = Camera.main.orthographicSize;
		baseSize = Camera.main.orthographicSize * Mathf.Max(1, (1920f / 1080f) / Camera.main.aspect);
		Camera.main.orthographicSize = baseSize;
		aura.transform.localScale = Vector3.one * Camera.main.orthographicSize / unscaledSize;
	}


	public void CameraTransition(Action Midpoint) {
		StartCoroutine(TransitionRoutine(duration, maxOrthoMultiplier, Midpoint));
	}

	IEnumerator TransitionRoutine(float duration, float max, Action Midpoint) {
		float a = 1;
		while (a > 0) {
			SetSize(a, max);
			a -= Time.deltaTime / duration;
			yield return null;
		}
		Midpoint();
		a = 0;
		while (a < 1) {
			SetSize(a, max);
			a += Time.deltaTime / duration;
			yield return null;
		}
		SetSize(1, max);
	}

	void SetSize(float lerp, float max) {
		Camera.main.orthographicSize = Mathf.Lerp(max, 1, transitionCurve.Evaluate(lerp)) * baseSize;
		cameraCanvas.localScale = Vector3.one * baseSize / Camera.main.orthographicSize;
		aura.localScale = Vector3.one * Camera.main.orthographicSize / unscaledSize;
	}

}
