using System;
using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] InputHandler handler;
	[SerializeField] MenuController menu;
	[SerializeField] ActManager actManager;
	[SerializeField] SwapController swap;
	[SerializeField] MessageScreenController messageScreen;
	[SerializeField] ScoreScreenController scoreScreen;
	[SerializeField] CameraController cameraController;
	[SerializeField] int maxLives;

	int score;
	int lives;

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
		cameraController.CameraTransition(() => {
			menu.gameObject.SetActive(false);
			GotoNextAct();
		});
	}

	void GotoNextAct() {
		swap.SwapRandom();
		actManager.StartAct(score, ActFinished, cameraController.CameraTransition);
	}

	public void EndGame() {
		cameraController.CameraTransition(() => {
			scoreScreen.ShowScore(score, (Action A) => {
				A();
				swap.Transition();
				GotoMenu();
			});
			if (score > PlayerPrefs.GetInt("hiscore", 0)) {
				PlayerPrefs.SetInt("hiscore", score);
			}
			score = 0;
		});
	}

	public void ActFinished(ActState state, Action TransitionDone) {
		cameraController.CameraTransition(() => {
			TransitionDone();

			switch (state) {
				case ActState.Success:
					score++;
					messageScreen.Succeed((Action A) => {
						cameraController.CameraTransition(() => {
							A();
							GotoNextAct();
						});
					});
					break;
				case ActState.Fail:
					lives--;
					messageScreen.Fail(lives, (Action A) => {
						cameraController.CameraTransition(() => {
							A();
							if (lives == 0) {
								swap.Transition();
								EndGame();
								return;
							} else
								GotoNextAct();
						}); 
					});
					break;
				case ActState.Interrupt:
					EndGame();
					break;
			}
		});
	}
}
