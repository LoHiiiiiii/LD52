using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

	[SerializeField] float soundDuration;
	[SerializeField] SoundHolder menu;
	[SerializeField] SoundHolder game;
	[SerializeField] SoundHolder spooky;

	AudioSource menuSource;
	AudioSource gameSource;
	AudioSource spookySource;

	AudioSource target;

	public void Start() {
		menuSource = AudioMaster.Instance.Play(menu);
		gameSource = AudioMaster.Instance.Play(game);
		spookySource = AudioMaster.Instance.Play(spooky);
	}

	private void Update() {
		UpdateVolume(menuSource);
		UpdateVolume(gameSource);
		UpdateVolume(spookySource);
	}

	void UpdateVolume(AudioSource source) {
		if (target != source) source.volume = Mathf.MoveTowards(source.volume, 0, Time.deltaTime / soundDuration);
		else source.volume = Mathf.MoveTowards(source.volume, 1, Time.deltaTime / soundDuration);
	}

	public void PlayMenu() => target = menuSource;
	public void PlayGame() => target = gameSource;
	public void PlaySpooky() => target = spookySource;
	public void StopMusic() => target = null;

}
