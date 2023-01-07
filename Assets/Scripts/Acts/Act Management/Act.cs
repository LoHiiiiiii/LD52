using System;
using UnityEngine;

public abstract class Act : MonoBehaviour, IInputTarget {

	public abstract void BeginAct(int difficulty, Action<bool> Finish);
	public abstract void EndAct();
	public abstract void UseInput(int x, int y, bool action, bool escape);
}