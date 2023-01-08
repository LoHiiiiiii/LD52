using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EatAct : Act {


	[SerializeField] int lanes;
	[SerializeField] float laneSize;
	[SerializeField] float baseSpeed;
	[SerializeField] float baseGrow;
	[SerializeField] int maxDifficulty;
	[SerializeField] float maxGrow;
	[SerializeField] float maxSpeed;
	[SerializeField] int baseApplesToEat;
	[SerializeField] int applePerDifficulty;
	[Space]
	[SerializeField] Transform dude;
	[SerializeField] Transform apple;
	[SerializeField] SoundHolder nom;

	int currentLane;

	bool active;
	bool ended;
	int lastY;
	float dudeX;
	float appleX;
	int appleCount;
	int targetApples;
	float growDuration;
	float moveSpeed;

	Action<ActState, Action> Finish;

	void Start() {
		dudeX = dude.position.x;
		appleX = apple.position.x;
	}

	public override void BeginAct(int difficulty, Action<ActState, Action> Finish) {
		gameObject.SetActive(true);
		apple.gameObject.SetActive(false);
		ended = false;
		lastY = 0;
		appleCount = 0;
		currentLane = Mathf.FloorToInt(lanes / 2f);
		this.Finish = Finish;
		active = true;
		moveSpeed = Mathf.Lerp(baseSpeed, maxSpeed, difficulty / maxDifficulty);
		growDuration = Mathf.Lerp(baseGrow, maxGrow, difficulty / maxDifficulty);
		targetApples = baseApplesToEat + applePerDifficulty * difficulty;
		StartCoroutine(AppleRoutine());
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
	}

	public override void UseInput(int x, int y, bool action, bool escape) {
		if (!active) return;
		if (lastY != y) {
			currentLane = Mathf.Clamp(currentLane + y, 0, lanes-1);
			lastY = y;
		}
	}

	void HandleDodger() {
		dude.transform.position = Vector3.MoveTowards(dude.transform.position, 
			new Vector3(dude.position.x, GetLaneY(currentLane), 0),
			35 * Time.deltaTime);
	}

	float GetLaneY(int lane) {
		return (lane - Mathf.FloorToInt(lanes / 2f)) * laneSize;
	}

	IEnumerator AppleRoutine() {
		yield return new WaitForSeconds(0.5f);
		int appleLane = Random.Range(0, lanes);
		while (!ended) {
			apple.gameObject.SetActive(true);
			int newLane = Random.Range(0, lanes - 1);
			if (newLane >= appleLane) newLane++;
			apple.position = new Vector3(appleX, GetLaneY(newLane));
			appleLane = newLane;

			float a = 0;
			while (a < 1) {
				apple.localScale = a * Vector3.one;
				a += Time.deltaTime / growDuration;
				yield return null;
			}

			apple.localScale = Vector3.one;

			float targetX = dudeX - 1;

			while (true) {
				apple.position += Vector3.left * Time.deltaTime * moveSpeed;
				if (!ended) {
					if (Physics2D.OverlapCircle(apple.position, 0.5f)) {
						a = 1;
						appleCount++;
						AudioMaster.Instance.Play(nom);
						while (a > 0) {
							apple.localScale = a * Vector3.one;
							a -= Time.deltaTime / 0.2f;
							yield return null;
						}
						apple.localScale = Vector3.zero;
						if (appleCount >= targetApples) {
							StartCoroutine(EndRoutine(ActState.Success));
						}
						break;
					};
					if (!ended && apple.position.x <= targetX) {
						StartCoroutine(EndRoutine(ActState.Fail));
					}
				}
				yield return null;
			}
			if (ended) break;
		}
	}

	IEnumerator EndRoutine(ActState state) {
		ended = true;
		yield return new WaitForSeconds(1f);
		EndAct(state);
	}

}
