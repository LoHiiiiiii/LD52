using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] InputHandler handler;
	[SerializeField] MenuController menu;
	[SerializeField] ActManager actManager;
	[SerializeField] SwapController swap;
	[SerializeField] MessageScreenController messageScreen;
	[SerializeField] ScoreScreenController scoreScreen;
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
		actManager.StartAct(score, ActFinished, swap.Transition);
	}

	public void EndGame() {
		handler.SetTarget(scoreScreen);
		scoreScreen.ShowScore(score, () => { swap.Transition(); GotoMenu(); });
		if (score > PlayerPrefs.GetInt("hiscore", 0)) {
			PlayerPrefs.SetInt("hiscore", score);
		}
		score = 0;
	}

	public void ActFinished(ActState state) {
		swap.Transition();

		switch (state) {
			case ActState.Success:
				score++;
				messageScreen.Succeed(GotoNextAct);
				break;
			case ActState.Fail: 
				lives--;
				messageScreen.Fail(lives, () => {
					if (lives == 0) {
						swap.Transition();
						EndGame();
						return;
					} else
						GotoNextAct();
				});
				break;
			case ActState.Interrupt:
				EndGame();
				break;
		}
	}
}
