using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class SleepAct : Act {

	[SerializeField] float time;
	[Space]
	[SerializeField] SpriteRenderer sleeper;
	[Space]
	[SerializeField] Sprite sleeperDefault;
	[SerializeField] Sprite sleeperFail;

	bool active;
	bool ended;
	float startTime;

	Action<ActState, Action> Finish;

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		sleeper.sprite = sleeperDefault;
		this.Finish = Finish;
		startTime = Time.time;
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
		if (spentTime > time) {
			ended = true;
			EndAct(ActState.Success);
			return;
		}
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active || ended) return;
		if (x != 0 || y != 0 || action) {
			StartCoroutine(FailRoutine());
		}
	}


	IEnumerator FailRoutine() {
		ended = true;
		sleeper.sprite = sleeperFail;
		Shake(sleeper.transform, 0.5f, 0.7f);
		yield return new WaitForSeconds(1f);
		EndAct(ActState.Fail);
	}

}
