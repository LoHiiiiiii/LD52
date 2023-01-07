using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActManager : MonoBehaviour {

	[SerializeField] Act[] allActs;

	Act currentAct;

	public bool StartAct(int difficulty, Action<bool> ActEnded) {
		if (currentAct != null) return false;

		var chosenActs = allActs.Where(act => act.minDifficulty <= difficulty && act.maxDifficulty >= difficulty) as Act[];

		if (chosenActs.Length == 0) return false;
		currentAct = chosenActs[Random.Range(0, chosenActs.Length)];

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
