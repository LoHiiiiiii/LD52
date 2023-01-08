using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DodgeAct : Act {


	[SerializeField] int lanes;
	[SerializeField] float laneSize;
	[SerializeField] float baseSpeed;
	[SerializeField] int baseSpikes;
	[SerializeField] int maxDifficulty;
	[SerializeField] float maxSpeed;
	[SerializeField] int maxSpikes;
	[SerializeField] float time;
	[Space]
	[SerializeField] Transform dodger;
	[SerializeField] Transform[] spikes;

	int currentLane;

	bool active;
	bool ended;
	int lastY;
	float startTime;
	float dudeX;

	Action<ActState, Action> Finish;

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		dudeX = dodger.position.x;
		ended = false;
		lastY = 0;
		currentLane = Mathf.FloorToInt(lanes / 2f);
		this.Finish = Finish;
		startTime = Time.time;
		active = true;
	}

	public override void EndAct(ActState state) {
		if (!active)
			active = false;
		Finish(state, () => {
			StopShake();
			gameObject.SetActive(false);
		});
	}

	void Update() {
		if (!active) return;
		HandleDodger();
		if (ended) return;
		float spentTime = Time.time - startTime;
		if (spentTime > time) {
			StartCoroutine(EndRoutine(ActState.Success));
			return;
		}
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active) return;
		if (lastY != y) {
			currentLane = Mathf.Clamp(currentLane + y, 0, lanes-1);
			lastY = y;
		}
	}

	void HandleDodger() {
		dodger.transform.position = Vector3.MoveTowards(dodger.transform.position, 
			new Vector3(dodger.position.x, GetLaneY(currentLane), 0),
			30 * Time.deltaTime);
	}

	float GetLaneY(int lane) {
		return (lane - Mathf.FloorToInt(lanes / 2f)) * laneSize;
	}
	

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

}
