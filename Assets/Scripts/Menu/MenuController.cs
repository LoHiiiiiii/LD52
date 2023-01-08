using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour, IInputTarget {
	
	[SerializeField] TMP_Text hiscore;
	[SerializeField] TMP_Text title;
	[SerializeField] GameObject[] indicators;

	public event Action StartPressed;

	string normalName = "Solvate Rush";

	int index;
	int lastY;
	bool lastEscape;
	bool lastAction;
	bool active;

	void OnEnable() {
		index = 0;
		lastY = 0;
		title.text = normalName;
		lastEscape = true;
		lastAction = true;
		HandleIndicator();
		active = true;
		int score = PlayerPrefs.GetInt("hiscore", 0);
		if (score > 0) {
			hiscore.gameObject.SetActive(true);
			hiscore.text = $"Hiscore: {score}";
		} else {
			hiscore.gameObject.SetActive(false);
		}
	}

	public void UseInput(int x, int y, bool action, bool escape) {
		if (!active) return;
		if (action && !lastAction) {
			Press(index);
		}

		if (escape && !lastEscape) {
			index = 1;
		} else if (y != lastY) {
			if (y > 0 && index != 0) index--;
			if (y < 0 && index != MaxIndex()) index++;
		}
		lastY = y;
		lastEscape = escape;
		lastAction = action;
		HandleIndicator();
	}

	void HandleIndicator() {
		for (int i = 0; i < indicators.Length; ++i) indicators[i].SetActive(i == index);
	}

	int MaxIndex() {
		return 1;
	}

	void Press(int index) {
		switch (index) {
			case 0:
				active = false;
				StartPressed?.Invoke();
				break;
			case 1:
				Application.Quit();
				break;
			case 2: break;
		}
	}
}
