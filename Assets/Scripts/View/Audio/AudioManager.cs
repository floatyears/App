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
	
	public void PlayAudio(AudioEnum audioEnum){
		int audioID = (int)audioEnum;
		AudioConfigItem configItem = ConfigAudio.audioList.Find( item=>item.id == audioID );
		if(configItem == default( AudioConfigItem ) )	
			return;

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
			return;
		}	
		audioPlayerCache[ audioID ].Play();
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
