using System;
using UnityEngine;

public abstract class Act : MonoBehaviour, IInputTarget {

	public int minDifficulty;
	public int maxDifficulty;

	public abstract void BeginAct(Action<bool> Finish);
	public abstract void EndAct();
	public abstract void UseInput(int x, int y, bool action, bool escape);
}