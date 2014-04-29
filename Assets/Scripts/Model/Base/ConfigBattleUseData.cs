#region
// leiliang
// use to store enter battle need data. befoure enten battle. init data from disk or server. dont konw battle is contine or a new.
#endregion

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using bbproto;

public class ConfigBattleUseData {
	private static ConfigBattleUseData instance;

	public static ConfigBattleUseData Instance {
		get {
			if(instance == null) {
				instance = new ConfigBattleUseData ();
			}
			return instance;
		}
	}

	private ConfigBattleUseData () {

	}
	
	public int colorIndex;
	
	public Coordinate roleInitCoordinate;

	public TQuestDungeonData questDungeonData;
	
	public TQuestInfo currentQuestInfo;

	public TStageInfo currentStageInfo;

	public TFriendInfo BattleFriend;

	private StoreBattleData _storeBattleData;
	public StoreBattleData storeBattleData {
		get { return _storeBattleData; }
	}

	public void ResetFromServer(TQuestDungeonData tdd) {
		InitStoreBattleData ();

		roleInitCoordinate = _storeBattleData.roleCoordinate;
		colorIndex = 0;
		questDungeonData = tdd;
		WriteFriend ();
		WriteQuestInfo ();
		WriteStageInfo ();
		WriteQuestDungeonData ();
		GameDataStore.Instance.StoreDataNoEncrypt (GameDataStore.battleStore, isBattle);
	}

	void InitStoreBattleData() {
		_storeBattleData = new StoreBattleData ();
		_storeBattleData.sp = DataCenter.maxEnergyPoint;
		_storeBattleData.hp = DataCenter.Instance.PartyInfo.CurrentParty.GetInitBlood ();
		_storeBattleData.roleCoordinate.x = MapConfig.characterInitCoorX;
		_storeBattleData.roleCoordinate.y = MapConfig.characterInitCoorY;
	}

	public void ResetFromDisk(StoreBattleData sbd, TQuestDungeonData tdd) {
		questDungeonData = tdd;
		roleInitCoordinate = sbd.roleCoordinate;
		if (sbd.colorIndex > 5) {
			colorIndex = sbd.colorIndex - 5;	
		} else {
			colorIndex = 0;
		}

		ReadFriend ();
		ReadQuestDungeonData ();
		ReadQuestInfo ();
		ReadStageInfo ();
	}

	public void ClearData () {
		GameDataStore.Instance.DeleteInfo (GameDataStore.battleStore);
	}

	private const string floderPath = "/Battle/";

	public const string isBattle = "/true";
	public const string friendFileName = "/Friend";
	public const string questDungeonDataName = "/DungeonData";
	public const string questInfoName = "/Quest";
	public const string stageInfoName = "/Stage";

	string GetPath (string path) {
		return Application.persistentDataPath + path;
	}

	public void EnterFight(bool isBoos) {
		_storeBattleData.isBattle = isBoos ? 2 : 1;	// 2 == battle boss, 1 == battle enemy;
	}

	public void ExitFight () {
		_storeBattleData.isBattle = 0;
	}
	
	public bool hasBattleData () {
		string value = GameDataStore.Instance.GetDataNoEncrypt (GameDataStore.battleStore);
		if (value == isBattle) {
			return true;		
		} else {
			return false;	
		}
	}

	public void StoreRuntimeData () {

	}



	//stage
	public void WriteStageInfo() {
		byte[] stage = ProtobufSerializer.SerializeToBytes<StageInfo> (currentStageInfo.stageInfo);
		WriteToFile (stage, stageInfoName);
	}

	void ReadStageInfo() {
		byte[] friend = ReadFile (stageInfoName);
		StageInfo qi = ProtobufSerializer.ParseFormBytes<StageInfo> (friend);
		currentStageInfo = new TStageInfo (qi);
	}
	//end

	//quest info
	public void WriteQuestInfo() {
		byte[] quest = ProtobufSerializer.SerializeToBytes<QuestInfo> (currentQuestInfo.questInfo);
		WriteToFile (quest, questInfoName);
	}

	void ReadQuestInfo() {
		byte[] friend = ReadFile (questInfoName);
		QuestInfo qi = ProtobufSerializer.ParseFormBytes<QuestInfo> (friend);
		currentQuestInfo = new TQuestInfo (qi);
	}
	//end

	//dungeonData
	public void WriteQuestDungeonData () {
		byte[] tdd = ProtobufSerializer.SerializeToBytes<QuestDungeonData> (questDungeonData.Instance);
		WriteToFile (tdd, questDungeonDataName);
	}

	void ReadQuestDungeonData() {
		byte[] friend = ReadFile (questDungeonDataName);
		QuestDungeonData qdd = ProtobufSerializer.ParseFormBytes<QuestDungeonData> (friend);
		questDungeonData = new TQuestDungeonData (qdd);
	}
	//end

	//friend
	public void WriteFriend() {
		byte[] friend = ProtobufSerializer.SerializeToBytes<FriendInfo>(BattleFriend.Instance);
		WriteToFile (friend, friendFileName);
	}

	void ReadFriend() {
		byte[] friend = ReadFile (friendFileName);
		FriendInfo fi = ProtobufSerializer.ParseFormBytes<FriendInfo> (friend);
		BattleFriend = new TFriendInfo (fi);
	}
	//end 

	void DeleteAndWrite(string fileName){
		if (File.Exists (fileName)) {
			File.Delete(fileName);
		}
	}

	void WriteToFile(byte[] data, string fileName){
		string path = GetPath (fileName);
		DeleteAndWrite (path);
		try {
			FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
			fs.Write(data, 0, data.Length);
			fs.Close();
			fs.Dispose();
		} catch (System.Exception ex) {
			Debug.LogError("WriteToFile exception : " + ex.Message);
		}
	}

	byte[] ReadFile(string fileName) {
		string path = GetPath (fileName);
		byte[] data = null;
		try {
			FileStream fs = new FileStream (path, FileMode.Open, FileAccess.Read);
			byte[] readData = new byte[fs.Length];
			fs.Read (readData, 0, (int)fs.Length);
			fs.Close ();
			fs.Dispose ();
			data = readData;
		} catch (System.Exception ex) {
			Debug.LogError ("ReadFile exception : " + ex.Message);
		}

		return data;
	}
}
