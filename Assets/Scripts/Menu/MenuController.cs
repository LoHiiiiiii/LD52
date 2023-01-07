using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour, IInputTarget {

	[SerializeField] InputHandler handler;
	[SerializeField] GameController gameController;
	[SerializeField] TMP_Text hiscore;
	[SerializeField] GameObject[] indicators;

	int index = 0;
	int lastY = 0;

	void OnEnable() {
		if (index > MaxIndex()) index = MaxIndex();
		HandleIndicator();
		handler.SetTarget(this);
		int score = PlayerPrefs.GetInt("hiscore", 0);
		if (score > 0) {
			hiscore.gameObject.SetActive(true);
			hiscore.text = $"Hiscore: {score}";
		} else {
			hiscore.gameObject.SetActive(false);
		}
	}

	public void UseInput(int x, int y, bool action, bool escape) {
		if (action) {
			Press(index);
		}

		if (escape) {
			index = 1;
			return;
		}
		if (y != lastY) {
			if (y > 0 && index != 0) index--;
			if (y < 0 && index != MaxIndex()) index++;
		}
		lastY = y;
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
				gameController.BeginGame();
				break;
			case 1:
				Application.Quit();
				break;
			case 2: break;
		}
	}
}
