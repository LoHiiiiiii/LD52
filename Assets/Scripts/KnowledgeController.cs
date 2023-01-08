using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnowledgeController : MonoBehaviour {
	[SerializeField] Sprite[] knowledgeSprites;
	[SerializeField] float updateTime;

	int index;
	float lastUpdate;

	public static KnowledgeController Instance { get; set; }

	public event Action<Sprite> KnowledgeUpdated;

	private void Awake() {
		Instance = this;
	}

	private void Update() {
		if (Time.time > updateTime + lastUpdate) {
			index = (index + 1) % knowledgeSprites.Length;
			KnowledgeUpdated?.Invoke(knowledgeSprites[index]);
			lastUpdate = Time.time;
		}
	}

}
