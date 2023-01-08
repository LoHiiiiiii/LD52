using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knowledge : MonoBehaviour {

	SpriteRenderer rend;
	Image image;

	void Start() {
		rend = GetComponent<SpriteRenderer>();
		image = GetComponent<Image>();
		KnowledgeController.Instance.KnowledgeUpdated += UpdateSprite;
	}

	void UpdateSprite(Sprite sprite) {
		if (rend != null) rend.sprite = sprite;
		if (image != null) image.sprite = sprite;
	}
}
