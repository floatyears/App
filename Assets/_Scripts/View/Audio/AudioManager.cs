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
	private AudioManager(){
		Debug.Log ("data store: " + GameDataStore.Instance.GetIntDataNoEncypt (soundName));
		isCloseSound = GameDataStore.Instance.GetIntDataNoEncypt (soundName)  == 0 ? false : true;
		isCloseBackground = GameDataStore.Instance.GetIntDataNoEncypt (bgmName) == 0 ? false : true;
	}

	public const string soundName = "sound";
	public const string bgmName = "bgm";

	//AudioSource Cache
	private Dictionary< int, AudioSource> audioPlayerCache = new Dictionary<int, AudioSource>();

	AudioSource prevBackground = null;

	AudioSource currentSoundAudio = null;

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
	public void StopBackgroundMusic(bool close) {
		isCloseBackground = close;
		if (isCloseBackground) {
			if(prevBackground)
				prevBackground.Pause ();
		} else {
			if(prevBackground)
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
		if (!IsBackgroundAuido(audioEnum) || isCloseBackground) {
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

//	public void StopBackground() {
//
//	}

	bool IsBackgroundAuido (AudioEnum audio) {
		bool back = false;
		switch (audio) {
		case AudioEnum.music_boss_battle:
		case AudioEnum.music_dungeon:
//		case AudioEnum.music_enemy_battle:
		case AudioEnum.music_home:
//		case AudioEnum.music_victory:
			back = true;
			break;
		}
		return back;
	}

	bool CheckAudio(AudioEnum audioEnum) {
		if (audioEnum == AudioEnum.sound_walk && CheckAudioIsPlay(AudioEnum.sound_enemy_attack)) {
			return false;
		}

		if (audioEnum == AudioEnum.sound_enemy_attack) {
			if(CheckAudioIsPlay(AudioEnum.sound_walk) && CheckAudioIsPlay(AudioEnum.sound_enemy_attack)) {
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

	AudioEnum currentEnum = AudioEnum.None;
	public AudioEnum GetPlayAuioInfo() {
		if (currentSoundAudio != null && currentSoundAudio.isPlaying) {
			return currentEnum;	
		} else {
			return AudioEnum.None;	
		}
	}
	
	public void PlayAudio(AudioEnum audioEnum){
		if (IsBackgroundAuido (audioEnum) || isCloseSound) {
			return;
		}

		if (!CheckAudio (audioEnum)) {
			return;	
		}

		currentEnum = audioEnum;

		currentSoundAudio = Play (audioEnum);
	}

	AudioSource Play(AudioEnum audioEnum) {
		int audioID = (int)audioEnum;
//		if ( ConfigAudio.audioList == null ) {
//			new ConfigAudio();
//			Debug.LogError("ERROR: ConfigAudio.audioList==null");
//		}
		AudioConfigItem configItem = DataCenter.Instance.ConfigAudioList.Find( item=>item.id == audioID );
		if(configItem == default( AudioConfigItem ) )	
			return null;
		
		if( !audioPlayerCache.ContainsKey( audioID )){
			//			Debug.Log( string.Format( "At present, NOT EXIST a audioPlayer Cache with the ID [{0}]. ADD IT", audioID) );
			GameObject go = new GameObject();
			go.name = string.Format( "_{0}", audioEnum.ToString() );
			AudioSource source = go.AddComponent< AudioSource >();
			ResourceManager.Instance.LoadLocalAsset( configItem.resourcePath ,o =>{
				source.clip = o as AudioClip;;
				audioPlayerCache.Add( audioID, source );
				SetPlayType( configItem.type, source);
			});

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
