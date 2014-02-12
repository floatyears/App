using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager {
	private static AudioManager instance;
	public static AudioManager Instance {
		get {
			if( instance == null )
				instance = new AudioManager();
			return instance;
		}
	}

	private AudioManager() {}

	public void PlayAudio(AudioEnum audioEnum){
		int audioID = (int)audioEnum;
		GameObject audioPlayer = new GameObject();
		AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();
		AudioClip audioClip = new AudioClip();
	
		AudioConfigItem configItem = ConfigAudio.audioList.Find( item=>item.id == audioID );
		if(configItem == default(AudioConfigItem))	return;
		audioClip = Resources.Load(configItem.resourcePath) as AudioClip;
		if(audioClip == null)	return;

		audioPlayer.name = "_" + audioEnum.ToString();
		audioSource.clip = audioClip;
		audioSource.Play();
	}

}
