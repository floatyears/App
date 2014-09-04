using UnityEngine;
using System.Collections;
using System.Text;

public class GameDataPersistence {
	private static GameDataPersistence instance;
	public static GameDataPersistence Instance {
		get {
			if(instance == null) {
				instance = new GameDataPersistence();
			}
			return instance;
		}
	}

	public void StoreData(string key, object value) {
		string info = value.ToString ();
		info = AES.Encrypt (info);
		PlayerPrefs.SetString (key, info);
	}

	public string GetData(string key) {
		string info = string.Empty;
		if (PlayerPrefs.HasKey (key)) {
			info = PlayerPrefs.GetString (key);

			info = AES.Decrypt (info);
		} 
		return info;
	}

	public void StoreIntDatNoEncypt(string key, int data) {
		PlayerPrefs.SetInt (key, data);
	}

	public int GetIntDataNoEncypt(string key) {
		return PlayerPrefs.GetInt (key);
	}

	public void StoreDataNoEncrypt(string key, object value) {
		string info = value.ToString ();
		PlayerPrefs.SetString (key, info);
	}

	public string GetDataNoEncrypt(string key) {
		string info = string.Empty;
		if (PlayerPrefs.HasKey (key)) {
			info = PlayerPrefs.GetString (key);
		} 
		return info;
	}

	public void DeleteInfo(string key) {
		if (PlayerPrefs.HasKey (key)) {
			PlayerPrefs.DeleteKey(key);
		} 
	}

	public bool HasInfo(string key) {
		if (PlayerPrefs.HasKey (key)) {
			return true;
		} else {
			return false;	
		}
	}

	public int GetInt(string key) {
		string data = GetData(key);
		if (data.Length == 0)
			return 0;
		return System.Convert.ToInt32(data);
	}

	public uint GetUInt(string key) {
		string data = GetData(key);
		if (data.Length == 0)
			return 0;
		return System.Convert.ToUInt32 (data);
	}

	/// <summary>
	/// data key list
	/// </summary>

	public const string USER_ID = "userid";
	public const string UUID = "uuid";

	public const string battleStore = "storeBattleData";
}
