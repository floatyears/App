using UnityEngine;
using System.Collections;
using System.Text;

public class GameSingleDataStore {
	private static GameSingleDataStore instance;
	public static GameSingleDataStore Instance {
		get {
			if(instance == null) {
				instance = new GameSingleDataStore();
			}
			return instance;
		}
	}

	public void StoreSingleData(string key, object value) {
		string info = value.ToString ();
		info = AES.Encrypt (info);
		PlayerPrefs.SetString (key, info);
	}

	public string GetSingleData(string key) {
		string info = string.Empty;
		if (PlayerPrefs.HasKey (key)) {
			info = PlayerPrefs.GetString (key);

			info = AES.Decrypt (info);

		} 
		return info;
	}

	public const string XXXXXX = "XXXXX";
}
