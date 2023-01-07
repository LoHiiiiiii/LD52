using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class MessageScreenController : MonoBehaviour {
	[SerializeField] GameObject regularHolder;
	[SerializeField] GameObject failHolder;

	[SerializeField] TMP_Text regularMessage;
	[SerializeField] TMP_Text failMessage;
	[SerializeField] HeartController[] hearts;

	[SerializeField] VoiceLine[] successVoiceLines;
	[SerializeField] VoiceLine[] failVoiceLines;

	public void Fail(int lives, Action FailFinished) {
		var chosen = failVoiceLines[Random.Range(0, failVoiceLines.Length)];
		failMessage.text = chosen.text;
		failHolder.SetActive(true);
		for (int i = 0; i < hearts.Length; ++i) {
			if (i <= lives) hearts[i].ShowHeart();
			else hearts[i].HideHeart();
		}
		StartCoroutine(HeartRoutine(hearts[lives], FailFinished, chosen.audio));
	}

	IEnumerator HeartRoutine(HeartController heart, Action FailFinished, SoundHolder soundHolder) {
		yield return new WaitForSeconds(1f);
		var sound = AudioMaster.Instance.Play(soundHolder);
		if (sound != null) {
			while (sound.isPlaying) {
				yield return null;
			}
		}

		yield return new WaitForSeconds(0.5f);
		heart.Break();
		yield return new WaitForSeconds(2f);
		FailFinished();
		failHolder.SetActive(false);
	}

	public void Succeed(Action SuccessFinished) {
		var chosen = successVoiceLines[Random.Range(0, successVoiceLines.Length)];
		ShowMessage(chosen, SuccessFinished);
	}

	public void ShowMessage(VoiceLine voiceLine, Action MessageFinished) {
		regularHolder.SetActive(true);
		regularMessage.text = voiceLine.text;
		StartCoroutine(MessageRoutine(voiceLine.audio, MessageFinished));
	}

	IEnumerator MessageRoutine(SoundHolder soundHolder, Action MessageFinished) {
		yield return new WaitForSeconds(0.8f);
		var sound = AudioMaster.Instance.Play(soundHolder);
		if (sound != null) {
			while (sound.isPlaying) {
				yield return null;
			}
		}
		yield return new WaitForSeconds(0.5f);
		regularHolder.SetActive(false);
		MessageFinished();
	}
}
