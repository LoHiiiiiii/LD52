using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ScoreScreenController : MonoBehaviour, IInputTarget {

	[SerializeField] TMP_Text scoreMessage;
	[SerializeField] GameObject[] indicators;

	int index;
	Action<Action> ScoreFinished;

	bool lastEscape;
	bool lastAction;
	bool active;

	public void ShowScore(int score, Action<Action> ScoreFinished) {
		scoreMessage.text = $"score: {score}";
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

			ScoreFinished(() => {
				active = true;
				gameObject.SetActive(false);
			});
		}
		lastAction = action;
		lastEscape = escape;
	}
}
