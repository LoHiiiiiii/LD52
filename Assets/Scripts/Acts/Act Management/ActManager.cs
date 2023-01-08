using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActManager : MonoBehaviour, IInputTarget {

	[SerializeField] InputHandler inputHandler;
	[SerializeField] MessageScreenController messageHandler;
	[Space]
	[SerializeField] int actRepeatGap;
	[SerializeField] Act[] allActs;
	[SerializeField] Act sleepAct;

	Act currentAct;
	Queue<Act> preventionQueue = new Queue<Act>();

	public bool StartAct(int difficulty, Action<ActState, Action> ActEnded, Action<Action> Transition) {
		if (currentAct != null) return false;

		while (preventionQueue.Count > actRepeatGap && preventionQueue.Count > 0) {
			preventionQueue.Dequeue();
		}

		if (difficulty > 3 && !SpookyManager.Instance.FirstGame && (!SpookyManager.Instance.NapBeaten || (Random.value <= 0.35f && !preventionQueue.Contains(sleepAct)))) {
			currentAct = sleepAct;
			preventionQueue.Enqueue(sleepAct);
		} else {
			var chosenActs = allActs.Where(act => !preventionQueue.Contains(act)).ToArray();

			if (chosenActs.Length == 0) return false;
			currentAct = chosenActs[Random.Range(0, chosenActs.Length)];
			preventionQueue.Enqueue(currentAct);
		}

		Action BeginAct = () => {
			inputHandler.SetTarget(this);
			currentAct.BeginAct(difficulty,
				(ActState state, Action Ended) => {
					currentAct = null;
					ActEnded(state, Ended);
				}
			);
		};

		if (currentAct.tutorial != null) {
			messageHandler.ShowMessage(currentAct.tutorial, (Action A) => {
				Transition(() => {
					A();
					BeginAct();
				});

			});
		} else BeginAct();
		return true;
	}

	public void UseInput(int x, int y, bool action, bool escape) {
		if (escape) {
			InterruptAct();
		} else if (currentAct != null) {
			currentAct.UseInput(x, y, action, escape);
		}
	}

	public void InterruptAct() {
		if (currentAct == null) return;
		currentAct.EndAct(ActState.Interrupt);
		currentAct = null;
	}
}
