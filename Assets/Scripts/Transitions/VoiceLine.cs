using UnityEngine;

[CreateAssetMenu(fileName = "Voice Line")]
public class VoiceLine : ScriptableObject {
	public string text;
	public SoundHolder audio;
}
