using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RepeatAct : Act {

	[SerializeField] int basePresses;
	[SerializeField] int pressesPerDifficulty;
	[SerializeField] float time;
	[Space]
	[SerializeField] GameObject[] icons;
	[SerializeField] Image bar;

	bool active;
	bool ended;
	bool lastAction;
	int lastX;
	int lastY;
	int remainingPresses;
	float startTime;

	int currentIndex = 0;

	Action<ActState, Action> Finish;
	
	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		ended = false;
		lastAction = false;
		lastX = 0;
		lastY = 0;
		remainingPresses = basePresses + difficulty * pressesPerDifficulty;
		currentIndex = 0;
		ChooseNewIndex();
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
			Fail();
			return;
		}
		bar.fillAmount = (time - spentTime) / time;
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active || ended) return;

		if (lastX != x && x != 0) {
			if (currentIndex == 1) {
				if (x == -1) HandleScore();
				else Fail();
			} else if (currentIndex == 2) {
				if (x == 1) HandleScore();
				else Fail();
			} else {
				Fail();
			}
		} else if (lastY != y && y != 0) {
			if (currentIndex == 3) {
				if (y == -1) HandleScore();
				else Fail();
			} else if (currentIndex == 4) {
				if (y == 1) HandleScore();
				else Fail();
			} else {
				Fail();
			}
		} else if (action && !lastAction) {
			if (currentIndex == 0) HandleScore();
			else Fail();
		}

		lastX = x;
		lastY = y;
		lastAction = action;
	}

	void HandleScore() {
		if (ended) return;
		remainingPresses--;
		if (remainingPresses == 0) {
			foreach (var icon in icons) icon.SetActive(false);
			StartCoroutine(EndRoutine(ActState.Success));
		} else {
			ChooseNewIndex();
		}
	}

	void ChooseNewIndex() {
		var newIndex = Random.Range(0, icons.Length - 1);
		if (newIndex >= currentIndex) newIndex++;
		currentIndex = newIndex;
		for (int i = 0; i < icons.Length; ++i) icons[i].SetActive(currentIndex == i);
	}

	void Fail() {
		Shake(icons[currentIndex].transform, 0.3f, 0.8f);
		StartCoroutine(EndRoutine(ActState.Fail));
	}

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

}
