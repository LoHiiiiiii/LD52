using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour, IInputTarget {

	[SerializeField] TMP_Text hiscore;
	[SerializeField] TMP_Text title;
	[SerializeField] GameObject[] indicators;
	[SerializeField] TMP_Text truth;
	[SerializeField] GameObject knowledge;
	[SerializeField] GameObject rightIndicator;

	public event Action StartPressed;
	public event Action TruthPressed;

	string normalName = "Solvate Rush";
	string spookyName = "soul haRveSt";

	int index;
	int lastY;
	bool lastEscape;
	bool lastAction;
	bool active;

	bool inRight;

	void OnEnable() {
		index = 0;
		lastY = 0;
		lastEscape = true;
		lastAction = true;
		HandleIndicator();
		active = true;
		knowledge.SetActive(!SpookyManager.Instance.MenuKnowledge && SpookyManager.Instance.SpookUnlocked);
		int score = PlayerPrefs.GetInt("hiscore", 0);
		if (score > 0) {
			hiscore.gameObject.SetActive(true);
			hiscore.text = $"Hiscore: {score}";
		} else {
			hiscore.gameObject.SetActive(false);
		}
		var count = SpookyManager.Instance.GetKnowledgeCount();
		truth.gameObject.SetActive(count > 0);
		truth.text =  $"Truth {count}/3";
		title.text = count == 3 ? spookyName : normalName;
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

		if (!inRight && x == 1 && SpookyManager.Instance.SpookUnlocked && !SpookyManager.Instance.MenuKnowledge) {
			inRight = true;
		}

		if (inRight && x == -1) inRight = false;

		lastY = y;
		lastEscape = escape;
		lastAction = action;
		HandleIndicator();
	}

	void HandleIndicator() {
		for (int i = 0; i < indicators.Length; ++i) indicators[i].SetActive(i == index && !inRight);
		rightIndicator.SetActive(inRight);
		knowledge.transform.localScale = (inRight) ? Vector3.one : Vector3.one / 10;
	}

	int MaxIndex() {
		return truth.gameObject.activeInHierarchy ? 2 : 1;
	}

	void Press(int index) {
		if (inRight) {
			inRight = false;
			HandleIndicator();
			SpookyManager.Instance.MenuKnowledge = true;
			var count = SpookyManager.Instance.GetKnowledgeCount();
			truth.gameObject.SetActive(count > 0);
			truth.text = $"Truth {count}/3";
			knowledge.SetActive(false);
		} else
			switch (index) {
				case 0:
					active = false;
					StartPressed?.Invoke();
					break;
				case 1:
					Application.Quit();
					break;
				case 2:
					if (SpookyManager.Instance.GetKnowledgeCount() == 3) {
						active = false;
						TruthPressed?.Invoke();
					} else {

					}
					break;
			}
	}
}
