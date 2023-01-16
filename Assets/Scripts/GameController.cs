using System;
using System.Collections;
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
	[SerializeField] MusicController musicController;
	[SerializeField] VoiceLine controlMessage;
	[SerializeField] VoiceLine[] truthLines;
	[SerializeField] GameObject aura;
	[SerializeField] SoundHolder endSound;

	int score;
	int lives;

	public void Start() {
		menu.StartPressed += BeginGame;
		menu.TruthPressed += TruthEnding;
		StartCoroutine(LateStart());
	}

	IEnumerator LateStart() {
		yield return null;
		GotoMenu();
	}

	void GotoMenu() {
		var count = SpookyManager.Instance.GetKnowledgeCount();

		if (count < 2) {
			musicController.PlayMenu();
			swap.SwapRandom();
		} else {
			musicController.PlaySpooky();
			swap.SwapSpooky();
		}
		menu.gameObject.SetActive(true);
		handler.SetTarget(menu);
	}

	public void BeginGame() {
		score = 0;
		lives = maxLives;
		var message = SpookyManager.Instance.GetSpookyInstructions(controlMessage);
		if (message != controlMessage) musicController.StopMusic();
		cameraController.CameraTransition(() => {
			menu.gameObject.SetActive(false);
			messageScreen.ShowMessage(message, (Action A) => {
				musicController.StopMusic();
				cameraController.CameraTransition(() => {
					A();
					musicController.PlayGame();
					GotoNextAct();
				});
			});
		});
	}

	void GotoNextAct() {
		swap.SwapRandom();
		actManager.StartAct(score, ActFinished, cameraController.CameraTransition);
	}

	public void EndGame() {
		SpookyManager.Instance.FirstGame = false;
		musicController.StopMusic();
		handler.SetTarget(scoreScreen);
		scoreScreen.ShowScore(score, (Action A) => {
			cameraController.CameraTransition(() => {
				A();
				GotoMenu();
			});
		});
		if (score > PlayerPrefs.GetInt("hiscore", 0)) {
			PlayerPrefs.SetInt("hiscore", score);
		}
		score = 0;
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

	void TruthEnding() {
		cameraController.CameraTransition(() => {
			musicController.StopMusic();
			menu.gameObject.SetActive(false);
			ShowTruth(0);
		});
	}

	void ShowTruth(int index) {
		if (truthLines.Length <= index) {
			StartCoroutine(Final());
		} else {
			messageScreen.ShowMessage(truthLines[index], (Action A) => {
				swap.Transition();
				A();
				ShowTruth(index + 1);
			});
		}
	}

	IEnumerator Final() {
		aura.SetActive(false);
		var sound = AudioMaster.Instance.Play(endSound);
		while (sound.isPlaying) {
			yield return null;
		}
		Application.Quit();
	}
}
