using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] InputHandler handler;
	[SerializeField] MenuController menu;
	[SerializeField] ActManager actManager;
	[SerializeField] SwapController swap;
	[SerializeField] int maxLives;

	int score;
	int lives;

	private void Awake() {
		Camera.main.orthographicSize = Camera.main.orthographicSize * Mathf.Max(1, (1920f/1080f) / Camera.main.aspect);
	}

	public void Start() {
		menu.StartPressed += BeginGame;
		GotoMenu();
	}

	void GotoMenu() {
		swap.SwapRandom();
		menu.gameObject.SetActive(true);
		handler.SetTarget(menu);
	}

	public void BeginGame() {
		score = 0;
		lives = maxLives;
		menu.gameObject.SetActive(false);
		GotoNextAct();
	}

	void GotoNextAct() {
		swap.SwapRandom();
		actManager.StartAct(score, ActFinished);
	}

	public void EndGame() {
		if (score > PlayerPrefs.GetInt("hiscore", 0)) {
			PlayerPrefs.SetInt("hiscore", score);
		}
		score = 0;
		GotoMenu();
	}

	public void ActFinished(bool succesful) {
		if (succesful) {
			score++;
		} else {
			lives--;
			if (lives == 0) {
				EndGame();
				return;
			}
		}
		GotoNextAct();
	}
}
