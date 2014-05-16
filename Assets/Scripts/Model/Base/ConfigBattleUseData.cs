﻿#region
// leiliang
// use to store enter battle need data. befoure enten battle. init data from disk or server. dont konw battle is contine or a new.
using System;


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

	private ConfigBattleUseData () { }
	
	public Coordinate roleInitCoordinate;

	public TQuestDungeonData questDungeonData;
	
	public TQuestInfo currentQuestInfo;

	public TStageInfo currentStageInfo;

	public TFriendInfo BattleFriend;

	private AttackInfo _posionAttack = null;
	public AttackInfo posionAttack {
		get { return _posionAttack; }
		set { _posionAttack = value;
			AttackInfoProto aip = _posionAttack == null ? null : _posionAttack.Instance;
			WriteBuff<AttackInfoProto> (posionAttackName, aip); 
		}
	}

	private AttackInfo _reduceHurtAttack = null;
	public AttackInfo reduceHurtAttack {
		get { return _reduceHurtAttack; }
		set { _reduceHurtAttack = value; 
			AttackInfoProto aip = _reduceHurtAttack == null ? null : _reduceHurtAttack.Instance;
			WriteBuff<AttackInfoProto> (reduceHurtName, aip); 
		}
	}

	private AttackInfo _reduceDefenseAttack = null;
	public AttackInfo reduceDefenseAttack {
		get { return _reduceDefenseAttack; }
		set { _reduceDefenseAttack = value;
			AttackInfoProto aip = _reduceDefenseAttack == null ? null : _reduceDefenseAttack.Instance;
			WriteBuff<AttackInfoProto>(reduceDefenseName, aip); 
		}
	}

	private AttackInfo _strengthenAttack = null;
	public AttackInfo strengthenAttack {
		get { return _strengthenAttack; }
		set { _strengthenAttack = value; 
			AttackInfoProto aip = _strengthenAttack == null ? null : _strengthenAttack.Instance;
			WriteBuff<AttackInfoProto>(strengthenAttackName, aip); 
		}
	}

	private TrapPosion _trapPoison = null;
	public TrapPosion trapPoison {
		get { return _trapPoison; }
		set { _trapPoison = value;
			TrapInfo ti = _trapPoison == null ? null :  _trapPoison.GetTrap;
			WriteBuff<TrapInfo>(trapPoisonName, ti);
		}
	}

	private EnvironmentTrap _trapEnvironment = null;
	public EnvironmentTrap trapEnvironment {
		get { return _trapEnvironment; }
		set { _trapEnvironment = value; 
			TrapInfo ti = _trapEnvironment == null ? null :  _trapEnvironment.GetTrap;
			WriteBuff<TrapInfo>(trapEnvironmentName, ti);
		}
	}


	private TStoreBattleData _storeBattleData;
	public TStoreBattleData storeBattleData {
		get { return _storeBattleData; }
	}

	public void ResetFromServer(TQuestDungeonData tdd) {
		InitStoreBattleData ();
		roleInitCoordinate = new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);//
		_storeBattleData.roleCoordinate = roleInitCoordinate;
		_storeBattleData.colorIndex = 0;
		questDungeonData = tdd;
		WriteFriend ();
		WriteQuestInfo ();
		WriteStageInfo ();
		WriteQuestDungeonData ();
	}

	void InitStoreBattleData() {
		StoreBattleData sbd = new StoreBattleData ();
		_storeBattleData = new TStoreBattleData (sbd);
		sbd.sp = DataCenter.maxEnergyPoint;
		sbd.hp = DataCenter.Instance.PartyInfo.CurrentParty.GetInitBlood ();
		sbd.xCoordinate = MapConfig.characterInitCoorX;
		sbd.yCoordinate = MapConfig.characterInitCoorY;
	}

	public void ResetFromDisk() {
		ReadFriend ();
		ReadQuestDungeonData ();
		ReadQuestInfo ();
		ReadStageInfo ();
		ReadAllBuff ();
		ReadRuntimeData ();
		roleInitCoordinate = _storeBattleData.roleCoordinate;
		//reset color index.
		if (_storeBattleData.colorIndex > 5) {
			_storeBattleData.colorIndex -= 5;	
		} else {
			_storeBattleData.colorIndex  = 0;
		}
	}

	public void StoreMapData (List<TClearQuestParam> data) {
		if(data != null)
			_storeBattleData.questData = data;
		WriteAllBuff ();
		StoreRuntimData ();
	}

	
	void ReadRuntimeData () {
		byte[] runtimeData = ReadFile (storeBattleName);
		StoreBattleData qi = ProtobufSerializer.ParseFormBytes<StoreBattleData> (runtimeData);
//		Debug.LogError ("ReadRuntimeData : " + qi.sp + " hp : " + qi.hp); 
		_storeBattleData = new TStoreBattleData (qi);
	}

	void StoreRuntimData () {
		byte[] battleData = ProtobufSerializer.SerializeToBytes<StoreBattleData> (_storeBattleData.instance);
		WriteToFile (battleData, storeBattleName);
	}

	public void StoreData () {
		GameDataStore.Instance.StoreDataNoEncrypt (GameDataStore.battleStore, isBattle);
	}

	public void ClearData () {
		GameDataStore.Instance.DeleteInfo (GameDataStore.battleStore);
	}

	public bool hasBattleData () {
		string value = GameDataStore.Instance.GetDataNoEncrypt (GameDataStore.battleStore);
		if (string.IsNullOrEmpty (value)) {
			return false;
		} else {
			return true;
		}
	}

	public void ClearActiveSkill() {
		posionAttack = null;
		reduceHurtAttack = null;
		reduceDefenseAttack = null;
		strengthenAttack = null;
	}

	private const string floderPath = "/Battle/";
	public const string isBattle = "/true";
	public const string friendFileName = "/Friend";
	public const string questDungeonDataName = "/DungeonData";
	public const string questInfoName = "/Quest";
	public const string stageInfoName = "/Stage";
	public const string storeBattleName = "/StoreBattle";
	public const string posionAttackName = "/Posion";
	public const string reduceHurtName = "/ReduceHurt";
	public const string reduceDefenseName = "/ReduceDefense";
	public const string strengthenAttackName = "/StrengthenAttack";
	public const string trapPoisonName = "/TrapPoison";
	public const string trapEnvironmentName = "/TrapEnvironment";

	string GetPath (string path) {
		return Application.persistentDataPath + path;
	}

	public void EnterFight(bool isBoos) {
		_storeBattleData.isBattle = isBoos ? 2 : 1;	// 2 == battle boss, 1 == battle enemy;
	}

	public void ExitFight () {
		_storeBattleData.isBattle = 0;
	}

	void WriteAllBuff() {
		AttackInfoProto attack = null;
		if (posionAttack != null) {
			attack = posionAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (posionAttackName, attack);
//		Debug.LogError ("write poison attack : " + posionAttack);
		if (reduceHurtAttack != null) {
			attack = reduceHurtAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceHurtName, attack);
		if (reduceDefenseAttack != null) {
			attack = reduceDefenseAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceDefenseName, attack);
		if (strengthenAttack != null) {
			attack = strengthenAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (strengthenAttackName, attack);
	}

	void ReadAllBuff() {
		_posionAttack = ReadBuff<AttackInfo, AttackInfoProto> (posionAttackName);
		_reduceHurtAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceHurtName);
		_reduceDefenseAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceDefenseName);
		_strengthenAttack = ReadBuff<AttackInfo, AttackInfoProto> (strengthenAttackName);
	}

	void WriteBuff<T>(string name, T buff) where T : ProtoBuf.IExtensible {
		if (string.IsNullOrEmpty (name)) {
			return;	
		}
	
		if (buff == null) {
			string path = GetPath (name);
			try {
				File.Delete(path);
			} catch (System.Exception ex) {
				Debug.LogError("WriteBuff ex : " + ex.Message);
			}
			return;
		}

		byte[] attack = ProtobufSerializer.SerializeToBytes<T> (buff);
		WriteToFile (attack, name);
	}

	T ReadBuff<T,T1> (string name) where T : ProtobufDataBase where T1 : ProtoBuf.IExtensible {
		if (string.IsNullOrEmpty (name)) {
			return null;	
		}
		string path = GetPath (name);
		if (!File.Exists (path)) {
			return null;	
		}

		byte[] attackInfo = ReadFile (name);
		T1 aip = ProtobufSerializer.ParseFormBytes<T1> (attackInfo);
		T t = Activator.CreateInstance(typeof(T), aip) as T;
		Debug.LogError ("t : " + t);
		return t;
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
