using System;
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
	bool nextIsRight;
	int lastX;
	int remainingWiggles;
	float startTime;

	Action<bool> Finish;

	public override void BeginAct(int difficulty, Action<bool> Finish) {
		gameObject.SetActive(true);
		lastX = 0;
		remainingWiggles = baseWiggles + Mathf.FloorToInt((maxWiggles-baseWiggles)/maxDifficulty * difficulty);
		HandleCounter();
		nextIsRight = Random.value > 0.5;
		wiggler.sprite = wigglerDefault;
		HandleArrow();
		this.Finish = Finish;
		startTime = Time.time;
		active = true;
		Debug.Log("Begin");
	}

	public override void EndAct() {
		active = false;
		gameObject.SetActive(false);
		Debug.Log("End");
	}

	void Update() {
		if (!active) return;
		float spentTime = Time.time - startTime;
		if (spentTime > time) {
			EndAct();
			Finish(false);
			return;
		}
		bar.fillAmount = (time - spentTime) / time;
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (lastX != x) {
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
		remainingWiggles--;
		if (remainingWiggles == 0) {
			EndAct();
			Finish(true);
			return;
		}
		nextIsRight = !nextIsRight;
		HandleCounter();
		HandleArrow();
	}

	void HandleCounter() {
		counter.text = remainingWiggles.ToString();
	}

	void HandleArrow() {
		rightArrow.sprite = nextIsRight ? bigArrow : smallArrow;
		leftArrow.sprite = nextIsRight ? smallArrow : bigArrow;
	}

}
