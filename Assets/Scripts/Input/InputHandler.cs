using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	IInputTarget currentTarget;

	private void Update() {
		HandleInput();
	}

	void HandleInput() {
		if (currentTarget == null) return;

		int x = 0, y = 0;
		if (Input.GetKey(KeyCode.LeftArrow)) x--;
		if (Input.GetKey(KeyCode.RightArrow)) x++;
		if (Input.GetKey(KeyCode.DownArrow)) y--;
		if (Input.GetKey(KeyCode.UpArrow)) y++;

		bool action = Input.GetKey(KeyCode.Space);
		bool escape = Input.GetKey(KeyCode.Escape);

		currentTarget.UseInput(x, y, action, escape);
	}

	public void SetTarget(IInputTarget target) {
		currentTarget = target;
	}
}
