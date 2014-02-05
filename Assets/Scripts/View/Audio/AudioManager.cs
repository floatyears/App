using UnityEngine;
using System.Collections;

public class AudioManager {

	private AudioManager() {}

	private AudioManager instance;
	public AudioManager Instance {
		get {
			if( instance == null )
				instance = new AudioManager();
			return instance;
		}
	}

}
