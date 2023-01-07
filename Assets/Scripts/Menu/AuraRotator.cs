using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraRotator : MonoBehaviour {

	[SerializeField] float secondsToRound = 0;

	void Update() {
		transform.Rotate(0, 0, 360/secondsToRound * Time.deltaTime);
	}
}
