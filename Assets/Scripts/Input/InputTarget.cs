using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputTarget {
	void UseInput(int x, int y, bool action, bool escape);
}