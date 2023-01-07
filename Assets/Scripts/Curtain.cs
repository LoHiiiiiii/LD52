using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curtain : MonoBehaviour {
	[SerializeField] Image curtain;
	[SerializeField] float duration;

	Coroutine fadeOutRoutine;

	public void FadeOut(Color color) {
		curtain.color = color;
		if (fadeOutRoutine != null) StopCoroutine(fadeOutRoutine);
		fadeOutRoutine = StartCoroutine(FadeOutRoutine(duration));
	}

	IEnumerator FadeOutRoutine(float duration) {
		float a = 1;
		while (a > 0 && duration > 0) {
			curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, a);
			a -= Time.deltaTime / duration;
			yield return null;
		}
		curtain.color = new Color(curtain.color.r, curtain.color.g, curtain.color.b, 0);
		fadeOutRoutine = null;
	}
}
