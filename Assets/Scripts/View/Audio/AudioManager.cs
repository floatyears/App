using UnityEngine;
using System.Collections;

public class AudioManager {

	AudioSource bgMusicSource;
	AudioClip bgMusicClip;

	private AudioManager() {
		InitAudioSource();
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
		bgMusicSource = GameObject.Find("Main").GetComponent<AudioSource>();
		bgMusicClip = Resources.Load("Audios/bgMusic") as AudioClip;

		bgMusicSource.clip = bgMusicClip;
		bgMusicSource.loop = true;
	}

	public void OnMusic() {
		if( !bgMusicSource.isPlaying )
			bgMusicSource.Play();
	}

	public void OffMusic() {
		if( bgMusicSource.isPlaying )
			bgMusicSource.Pause();
	}

}
