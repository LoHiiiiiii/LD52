using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ShootAct : Act {

	[SerializeField] float time;
	[SerializeField] float baseRotateTime;
	[SerializeField] float minRotateTime;
	[SerializeField] float maxDifficulty;
	[SerializeField] float maxSize;
	[SerializeField] float minSize;
	[SerializeField] AnimationCurve curve;
	[Space]
	[SerializeField] Transform gun;
	[SerializeField] Transform shotTarget;
	[SerializeField] SoundHolder shoot;
	[SerializeField] SoundHolder hitSound;

	bool active;
	bool ended;
	float startTime;
	float rotateTime;

	Action<ActState, Action> Finish;

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		this.Finish = Finish;
		startTime = Time.time;
		shotTarget.localScale = Vector3.one * Mathf.Lerp(maxSize, minSize, difficulty / (float)maxDifficulty);
		rotateTime = Mathf.Lerp(baseRotateTime, minRotateTime, difficulty / (float)maxDifficulty);
		active = true;
		ended = false;
	}

	public override void EndAct(ActState state) {
		if (!active) return;


		Finish(state, () => {
			StopShake();
			active = false;
			gameObject.SetActive(false);
		});
	}

	void Update() {
		if (!active || ended) return;
		float spentTime = Time.time - startTime;
		gun.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-75, 75, curve.Evaluate(spentTime / rotateTime)));
		if (spentTime > time) {
			StartCoroutine(EndRoutine(ActState.Fail));
			return;
		}
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active || ended) return;
		if (action) {
			Shoot();
		}
	}

	void Shoot() {
		AudioMaster.Instance.Play(shoot);
		var hit = Physics2D.Raycast(gun.position, gun.right, 100);
		if (hit) {
			AudioMaster.Instance.Play(hitSound);
			Shake(shotTarget, 0.8f, 0.5f);
		} else Shake(gun, 0.2f, 0.3f);
		StartCoroutine(EndRoutine(hit ? ActState.Success : ActState.Fail));
	}

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

}
