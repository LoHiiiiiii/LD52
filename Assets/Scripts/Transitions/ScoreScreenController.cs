using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ScoreScreenController : MonoBehaviour, IInputTarget {

	[SerializeField] TMP_Text scoreMessage;
	[SerializeField] GameObject[] indicators;

	int index;
	Action ScoreFinished;

	bool lastEscape;
	bool lastAction;

	public void ShowScore(int score, Action ScoreFinished) {
		scoreMessage.text = $"score: {score}";
		index = 0;
		lastEscape = true;
		lastAction = true;
		HandleIndicator();
		gameObject.SetActive(true);
		this.ScoreFinished = ScoreFinished;
	}


	void HandleIndicator() {
		for (int i = 0; i < indicators.Length; ++i) indicators[i].SetActive(i == index);
	}

	public void UseInput(int x, int y, bool action, bool escape) {
		if ((action && !lastAction) || (escape && !lastEscape)) {
			gameObject.SetActive(false);
			ScoreFinished();
		}
		lastAction = action;
		lastEscape = escape;
	}
}
