using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActInputHandler : IInputTarget {

	public Act CurrentAct { get; set; }

	public void UseInput(int x, int y, bool action, bool escape) {
		CurrentAct.UseInput(x, y, action, escape);
	}
}
