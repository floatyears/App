using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager {

	private AudioSource musicSource;
	private AudioSource soundSource;
	private bool canPlaySound;
	private bool canPlayMusic;

	private Dictionary< string, AudioClip > audioClipCollection = new  Dictionary<string, AudioClip>();

	AudioClip bgMusicClip;

	private AudioManager() {
		InitAudioSource();
		canPlaySound = true;
		canPlayMusic = true;
	}

	private static AudioManager instance;
	public static AudioManager Instance {
		get {
			if( instance == null )
				instance = new AudioManager();
			return instance;
		}
	}

	private void InitAudioSource() {
		musicSource = GameObject.Find("Audio/Music").GetComponent<AudioSource>();
		soundSource = GameObject.Find("Audio/Sound").GetComponent< AudioSource >();

		bgMusicClip = Resources.Load("Audio/bgMusic") as AudioClip;

		musicSource.clip = bgMusicClip;
		musicSource.loop = true;
	}
	private void InitAudioClip(){

	}

	public void OnMusic() {
		if(!canPlayMusic)
			return;

		if(musicSource.isPlaying)
			return;
			musicSource.Play();
	}

	public void OffMusic() {
		if( musicSource.isPlaying )
			musicSource.Pause();
	}




	public void PlaySound(string soundName) {
		if(!canPlaySound) 
			return;
		if(!audioClipCollection.ContainsKey(soundName))
			return;
			
		AudioClip clip = audioClipCollection[soundName];
		if(clip == null){
			Debug.LogError("Clip is Null");
			return;
		}
		soundSource.clip = clip;
		soundSource.Play();
	}
}
