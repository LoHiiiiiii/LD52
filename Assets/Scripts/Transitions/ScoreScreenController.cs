using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ScoreScreenController : MonoBehaviour, IInputTarget {

	[SerializeField] TMP_Text scoreMessage;
	[SerializeField] GameObject knowledge;
	[SerializeField] GameObject[] indicators;

	int index;
	Action<Action> ScoreFinished;

	bool lastEscape;
	bool lastAction;
	bool active;

	public void ShowScore(int score, Action<Action> ScoreFinished) {
		scoreMessage.text = $"score: {score}";
		knowledge.gameObject.SetActive(score >= 20 && SpookyManager.Instance.SpookUnlocked && !SpookyManager.Instance.HiscoreKnowledge);
		index = 0;
		lastEscape = true;
		lastAction = true;
		active = true;
		HandleIndicator();
		gameObject.SetActive(true);
		this.ScoreFinished = ScoreFinished;
	}


	void HandleIndicator() {
		for (int i = 0; i < indicators.Length; ++i) indicators[i].SetActive(i == index);
	}

	public void UseInput(int x, int y, bool action, bool escape) {
		if (!active) return;
		if ((action && !lastAction) || (escape && !lastEscape)) {

			if (index == 0) {
				active = false;
				ScoreFinished(() => {
					gameObject.SetActive(false);
				});
			} else {
				knowledge.gameObject.SetActive(false);
				SpookyManager.Instance.HiscoreKnowledge = true;
				index = 0;
				HandleIndicator();

			}
		}
		if (x == -1 && knowledge.activeInHierarchy) {
			index = 1;
			HandleIndicator();
		}

		if (x == 1 && index == 1) {
			index = 0;
			HandleIndicator();
		}
		lastAction = action;
		lastEscape = escape;
	}
}
