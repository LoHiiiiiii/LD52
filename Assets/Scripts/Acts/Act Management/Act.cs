using System;
using UnityEngine;

public abstract class Act : MonoBehaviour {

	public int minDifficulty;
	public int maxDifficulty;

	public abstract void BeginAct(Action<bool> Finish);
	public abstract void EndAct();
}