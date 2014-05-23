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
	private AudioManager(){}

	//AudioSource Cache
	private Dictionary< int, AudioSource> audioPlayerCache = new Dictionary<int, AudioSource>();

	AudioSource prevBackground = null;

	/// <summary>
	/// background audio close
	/// </summary>
	private bool isCloseBackground = false;

	/// <summary>
	/// sound audio close
	/// </summary>
	private bool isCloseSound = false;

	/// <summary>
	/// Closes the background.
	/// </summary>
	/// <param name="close"> true is close background.  flase is play background</param>
	public void CloseBackground(bool close) {
		isCloseBackground = close;
		if (isCloseBackground) {
			prevBackground.Pause ();
		} else {
			prevBackground.Play ();	
		}
	}

	/// <summary>
	/// Closes the background.
	/// </summary>
	/// <param name="close"> true is close background.  flase is play background</param>
	public void CloseSound(bool close) {
		isCloseSound = close;
	}
	

	public void PlayBackgroundAudio (AudioEnum audioEnum) {
		if (!IsBackgroundAuido(audioEnum)) {
			return;	
		}
		if (prevBackground != null) {
			prevBackground.Stop ();	
		}

		prevBackground = Play (audioEnum);

		if (isCloseBackground) {
			prevBackground.Pause();
		}
	}

	bool IsBackgroundAuido (AudioEnum audio) {
		bool back = false;
		switch (audio) {
		case AudioEnum.music_boss_battle:
		case AudioEnum.music_dungeon:
		case AudioEnum.music_enemy_battle:
		case AudioEnum.music_home:
		case AudioEnum.music_victory:
			back = true;
			break;
		}
		return back;
	}

	bool CheckAudio(AudioEnum audioEnum) {
		if (audioEnum == AudioEnum.sound_walk && CheckAudioIsPlay(AudioEnum.sound_walk_hurt)) {
			return false;
		}

		if (audioEnum == AudioEnum.sound_walk_hurt) {
			if(CheckAudioIsPlay(AudioEnum.sound_walk) && CheckAudioIsPlay(AudioEnum.sound_walk_hurt)) {
				return false;
			}
		}

		return true;
	}

	bool CheckAudioIsPlay(AudioEnum audioEnum) {
		int id = (int)audioEnum;
		AudioSource audio = null;
		if (!audioPlayerCache.TryGetValue (id, out audio)) {
			return false;
		}

		if (audio.isPlaying) {
			return true;	
		} else {
			return false;	
		}
	}
	
	public void PlayAudio(AudioEnum audioEnum){
		if (IsBackgroundAuido (audioEnum) || isCloseSound) {
			return;	
		}

		if (!CheckAudio (audioEnum)) {
			return;	
		}

		Play (audioEnum);
	}

	AudioSource Play(AudioEnum audioEnum) {
		int audioID = (int)audioEnum;
		if ( ConfigAudio.audioList == null ) {
			Debug.LogError("ERROR: ConfigAudio.audioList==null");
		}
		AudioConfigItem configItem = ConfigAudio.audioList.Find( item=>item.id == audioID );
		if(configItem == default( AudioConfigItem ) )	
			return null;
		
		if( !audioPlayerCache.ContainsKey( audioID )){
			//			Debug.Log( string.Format( "At present, NOT EXIST a audioPlayer Cache with the ID [{0}]. ADD IT", audioID) );
			GameObject go = new GameObject();
			go.name = string.Format( "_{0}", audioEnum.ToString() );
			AudioSource source = go.AddComponent< AudioSource >();
			AudioClip clip = Resources.Load( configItem.resourcePath ) as AudioClip;
			source.clip = clip;
			audioPlayerCache.Add( audioID, source );
			SetPlayType( configItem.type, source);
		}
		
		if( audioPlayerCache[ audioID ] == null ){
			Debug.LogError( string.Format( "The audioPlayer's GameObject with the ID [{0}] is NULL", audioID) );
			return null;
		}	
		AudioSource audioSource = audioPlayerCache [audioID];
		audioSource.Play ();
		return audioSource;
	}

	public void PauseAudio( AudioEnum audioEnum ) {
		int audioID = (int)audioEnum;
		if( !audioPlayerCache.ContainsKey( audioID ) ){
			Debug.LogError( string.Format( "At present, NOT EXIST a audioPlayer Cache with the ID [{0}]. CAN'T PAUSE it", audioID) );
			return;
		}

		if( audioPlayerCache[ audioID ] == null ){
			Debug.LogError( string.Format( "The audioPlayer's GameObject with the ID [{0}] is NULL", audioID) );
			return;
		}

		if( audioPlayerCache[ audioID ].isPlaying )	
			audioPlayerCache[ audioID ].Pause();
	}



	public void StopAudio( AudioEnum audioEnum ) {
		int audioID = (int)audioEnum;
		if( !audioPlayerCache.ContainsKey( audioID )){
			Debug.LogError( string.Format( "At present, NOT EXIST a audioPlayer Cache with the ID [{0}]. CAN'T STOP it", audioID) );
			return;
		}
		
		if( audioPlayerCache[ audioID ] == null ){
			Debug.LogError( string.Format( "The audioPlayer's GameObject with the ID [{0}] is NULL", audioID) );
			return;
		}
		
		if( audioPlayerCache[ audioID ].isPlaying )	
			audioPlayerCache[ audioID ].Stop();
	}

	void SetPlayType(EPlayType type, AudioSource source){
		switch (type){
			case EPlayType.LOOP : 
				source.loop = true;
				break;
			case EPlayType.ONCE : 
				source.loop = false;
				break;
			default:
				break;
		}
	}

}
