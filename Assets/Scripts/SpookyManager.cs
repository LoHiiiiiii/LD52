using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyManager : MonoBehaviour {

	[SerializeField] VoiceLine[] spookyInstructions;

	public static SpookyManager Instance { get; set; }
	public bool Spooky { get; set; }
	public bool FirstGame { get; set; }
	public bool SpookUnlocked { get; set; }

	public bool NapBeaten { get; set; }
	public bool NapKnowledge { get; set; }

	public bool HiscoreKnowledge { get; set; }
	public bool MenuKnowledge { get; set; }

	bool spookForced;

	public int GetKnowledgeCount() {
		int count = 0;
		if (NapKnowledge) count++;
		if (MenuKnowledge) count++;
		if (HiscoreKnowledge) count++;
		return count;
	}

	private void Start() {
		if (Instance != null) { Debug.Log("Multiple spooks"); Destroy(gameObject); }
		Instance = this;
	}

	public VoiceLine GetSpookyInstructions(VoiceLine defaultLine) {
		if (NapBeaten && !spookForced) {
			spookForced = true;
			SpookUnlocked = true;
			return spookyInstructions[Random.Range(0, spookyInstructions.Length)];
		} else if (NapBeaten && Random.value <= 0.25f) {
			return spookyInstructions[Random.Range(0, spookyInstructions.Length)];
		}
		return defaultLine;
	}
}
