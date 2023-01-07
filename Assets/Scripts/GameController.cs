using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] MenuController menu;
	[SerializeField] ActManager actManager;
	[SerializeField] SwapController swap;
	[SerializeField] int maxLives;

	int score = 0;
	int lives;

	public void Start() {
		swap.SwapRandom();
	}

	public void BeginGame() {
		lives = maxLives;
		menu.gameObject.SetActive(false);
		swap.SwapRandom();
		actManager.StartAct(score, ActFinished);
	}

	public void EndGame() {
		if (score > PlayerPrefs.GetInt("hiscore", 0)) {
			PlayerPrefs.SetInt("hiscore", score);
		}
		score = 0;
		swap.SwapRandom();
		menu.gameObject.SetActive(true);
	}

	public void ActFinished(bool succesful) {
		swap.SwapRandom();
		if (succesful) {
			score++;
			actManager.StartAct(score, ActFinished);
		} else {
			lives--;
			if (lives == 0) EndGame();
			else {
				actManager.StartAct(score, ActFinished);
			}
		}
	}
}
