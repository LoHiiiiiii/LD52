using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class WigglerAct : Act {

	[SerializeField] int baseWiggles;
	[SerializeField] int maxDifficulty;
	[SerializeField] int maxWiggles;
	[SerializeField] float time;
	[Space]
	[SerializeField] SpriteRenderer wiggler;
	[SerializeField] SpriteRenderer rightArrow;
	[SerializeField] SpriteRenderer leftArrow;
	[SerializeField] TMP_Text counter;
	[SerializeField] Image bar;
	[Space]
	[SerializeField] Sprite wigglerDefault;
	[SerializeField] Sprite wigglerTwist;
	[SerializeField] Sprite bigArrow;
	[SerializeField] Sprite smallArrow;

	bool active;
	bool ended;
	bool nextIsRight;
	int lastX;
	int remainingWiggles;
	float startTime;

	Action<ActState, Action> Finish;

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		ended = false;
		lastX = 0;
		remainingWiggles = baseWiggles + Mathf.FloorToInt((maxWiggles - baseWiggles) / maxDifficulty * difficulty);
		HandleCounter();
		nextIsRight = Random.value > 0.5;
		wiggler.sprite = wigglerDefault;
		HandleArrow();
		this.Finish = Finish;
		startTime = Time.time;
		active = true;
	}

	public override void EndAct(ActState state) {
		if (!active)
			active = false;
		Finish(state, () => {
			StopShake();
			gameObject.SetActive(false);
		} );
	}

	void Update() {
		if (!active || ended) return;
		float spentTime = Time.time - startTime;
		if (spentTime > time) {
			StartCoroutine(EndRoutine(ActState.Fail));
			return;
		}
		bar.fillAmount = (time - spentTime) / time;
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active) return;
		if (lastX != x) {
			Shake(wiggler.transform, 0.1f, 0.5f);
			if (x == 1) {
				wiggler.sprite = wigglerTwist;
				wiggler.flipX = true;
			} else {
				wiggler.flipX = false;
				if (x == -1) {
					wiggler.sprite = wigglerTwist;
				} else {
					wiggler.sprite = wigglerDefault;
				}
			}
			if (nextIsRight && x == 1) {
				HandleScore();
			} else if (!nextIsRight && x == -1) {
				HandleScore();
			}

			lastX = x;
		}

	}

	void HandleScore() {
		if (ended) return;
		remainingWiggles--;
		nextIsRight = !nextIsRight;
		HandleCounter();
		HandleArrow();
		if (remainingWiggles == 0) {
			StartCoroutine(EndRoutine(ActState.Success));
		}
	}

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		rightArrow.sprite = smallArrow;
		leftArrow.sprite = smallArrow;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

	void HandleCounter() {
		counter.text = remainingWiggles.ToString();
	}

	void HandleArrow() {
		rightArrow.sprite = nextIsRight ? bigArrow : smallArrow;
		leftArrow.sprite = nextIsRight ? smallArrow : bigArrow;
	}

}
