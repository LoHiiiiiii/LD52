using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SprintAct : Act {

	[SerializeField] int basePresses;
	[SerializeField] int pressesPerDifficulty;
	[SerializeField] float time;
	[Space]
	[SerializeField] Transform sprinterHolder;
	[SerializeField] SpriteRenderer spinterRenderer;
	[SerializeField] Image bar;
	[SerializeField] Transform flagHolder;
	[Space]
	[SerializeField] Sprite sprintDefault;
	[SerializeField] Sprite sprintForward;

	bool active;
	bool ended;
	bool lastAction;
	int remainingPresses;
	int maxPresses;
	float startTime;
	Vector3 sprinterStartPos;

	Action<ActState, Action> Finish;

	private void Awake() {
		sprinterStartPos = sprinterHolder.position;
	}

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		ended = false;
		lastAction = false;
		sprinterHolder.position = sprinterStartPos;
		maxPresses = basePresses + difficulty * pressesPerDifficulty;
		remainingPresses = maxPresses;
		spinterRenderer.sprite = sprintDefault;
		this.Finish = Finish;
		startTime = Time.time;
		active = true;
	}

	public override void EndAct(ActState state) {
		if (!active) return;
		active = false;
		Finish(state, () => {
			StopShake();
			gameObject.SetActive(false);
		});
	}

	void Update() {
		if (!active || ended) return;
		float spentTime = Time.time - startTime;
		if (spentTime > time) {
			spinterRenderer.sprite = sprintDefault;
			StartCoroutine(EndRoutine(ActState.Fail));
			return;
		}
		bar.fillAmount = (time - spentTime) / time;
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active || ended) return;
		if (action && !lastAction) {
			Shake(spinterRenderer.transform, 0.1f, 0.3f);
			spinterRenderer.sprite = sprintForward;
			HandleScore();
		}

		if (!action && lastAction) {
			spinterRenderer.sprite = sprintDefault;
		}
		lastAction = action;

	}

	void HandleScore() {
		if (ended) return;
		remainingPresses--;
		sprinterHolder.position = Vector3.Lerp(flagHolder.position, sprinterStartPos, remainingPresses / (float)maxPresses);
		if (remainingPresses == 0) {
			StartCoroutine(EndRoutine(ActState.Success));
		}
	}

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

}
