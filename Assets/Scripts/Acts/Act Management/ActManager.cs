using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActManager : MonoBehaviour {

	[SerializeField] int actRepeatGap;
	[SerializeField] Act[] allActs;

	Act currentAct;
	Queue<Act> preventionQueue = new Queue<Act>();


	public bool StartAct(int difficulty, Action<bool> ActEnded) {
		if (currentAct != null) return false;

		while (preventionQueue.Count > actRepeatGap && preventionQueue.Count > 0) {
			preventionQueue.Dequeue();
		} 

		var chosenActs = allActs.Where(act => act.minDifficulty <= difficulty && act.maxDifficulty >= difficulty && !preventionQueue.Contains(act)) as Act[];

		if (chosenActs.Length == 0) return false;
		currentAct = chosenActs[Random.Range(0, chosenActs.Length)];
		preventionQueue.Enqueue(currentAct);

		currentAct.BeginAct(
			(bool successful) => {
				currentAct = null;
				ActEnded(successful);
			}
		);
		return true;
	}

	public void InterruptAct() {
		if (currentAct != null) return;
		currentAct.EndAct();
		currentAct = null;
	}
}
