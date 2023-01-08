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
	[SerializeField] VoiceLine[] spookySuccessLines;
	[SerializeField] VoiceLine[] failVoiceLines;
	[SerializeField] VoiceLine[] spookyFailLines;

	public void Fail(int lives, Action<Action> FailFinished) {
		VoiceLine chosen;
		if (SpookyManager.Instance.SpookUnlocked && Random.value <= 0.125f) {
			chosen = spookyFailLines[Random.Range(0, spookyFailLines.Length)];
		} else {
			chosen = failVoiceLines[Random.Range(0, failVoiceLines.Length)];
		}
		failMessage.text = chosen.text;
		failHolder.SetActive(true);
		for (int i = 0; i < hearts.Length; ++i) {
			if (i <= lives) hearts[i].ShowHeart();
			else hearts[i].HideHeart();
		}
		StartCoroutine(HeartRoutine(hearts[lives], FailFinished, chosen.audio));
	}

	IEnumerator HeartRoutine(HeartController heart, Action<Action> FailFinished, SoundHolder soundHolder) {
		yield return new WaitForSeconds(1f);
		var sound = AudioMaster.Instance.Play(soundHolder);
		if (sound != null) {
			while (sound.isPlaying) {
				yield return null;
			}
		}

		yield return new WaitForSeconds(0.5f);
		heart.Break();
		yield return new WaitForSeconds(1.5f);
		FailFinished(() => failHolder.SetActive(false));
	}

	public void Succeed(Action<Action> SuccessFinished) {
		if (SpookyManager.Instance.SpookUnlocked && Random.value <= 0.125f) {
			var chosen = spookySuccessLines[Random.Range(0, spookySuccessLines.Length)];
			ShowMessage(chosen, SuccessFinished);
		}
		else {
			var chosen = successVoiceLines[Random.Range(0, successVoiceLines.Length)];
			ShowMessage(chosen, SuccessFinished);
		}
	}

	public void ShowMessage(VoiceLine voiceLine, Action<Action> MessageFinished) {
		regularHolder.SetActive(true);
		regularMessage.text = voiceLine.text;
		StartCoroutine(MessageRoutine(voiceLine.audio, MessageFinished));
	}

	IEnumerator MessageRoutine(SoundHolder soundHolder, Action<Action> MessageFinished) {
		yield return new WaitForSeconds(0.8f);
		var sound = AudioMaster.Instance.Play(soundHolder);
		if (sound != null) {
			while (sound.isPlaying) {
				yield return null;
			}
		}
		yield return new WaitForSeconds(0.5f);
		MessageFinished(() => regularHolder.SetActive(false));
	}
}
