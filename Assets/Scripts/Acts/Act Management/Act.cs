using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Act : MonoBehaviour, IInputTarget {
	public VoiceLine tutorial;

	public abstract void BeginAct(int difficulty, Action<ActState, Action> Finish);
	public abstract void EndAct(ActState state);
	public abstract void UseInput(int x, int y, bool action, bool escape);

	Coroutine shakeRoutine;
	float shakeRatio;
	Vector3 start;
	Transform target;

	protected void Shake(Transform t, float duration, float amount) {
		if (!gameObject.activeInHierarchy) return;
		if (shakeRoutine != null) StopShake();
		shakeRoutine = StartCoroutine(ShakeRoutine(t, duration, amount));
	}

	protected void StopShake() {
		if (shakeRoutine == null) return;
		shakeRatio = 0;
		target.localPosition = start;
		target = null;
		StopCoroutine(shakeRoutine);
		shakeRoutine = null;
	}

	IEnumerator ShakeRoutine(Transform t, float duration, float amount) {
		target = t;
		start = t.localPosition;
		shakeRatio = 1;
		while (shakeRatio > 0) {
			shakeRatio -= Time.deltaTime / duration;
			target.localPosition = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.right * amount * shakeRatio * shakeRatio + start;
			yield return null;
		}
		StopShake();
	}

}