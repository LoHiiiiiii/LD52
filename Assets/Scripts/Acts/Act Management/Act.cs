using System;
using UnityEngine;

public abstract class Act : MonoBehaviour, IInputTarget {
	public VoiceLine tutorial;

	public abstract void BeginAct(int difficulty, Action<ActState> Finish);
	public abstract void EndAct(ActState state);
	public abstract void UseInput(int x, int y, bool action, bool escape);
}