#region
// leiliang
// use to store enter battle need data. befoure enten battle. init data from disk or server. dont konw battle is contine or a new.
using System;

#endregion

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using bbproto;

public class BattleConfigData {

	public const byte cardPoolSingle = 5;
	public const byte cardCollectionCount = 5;
	public const byte cardSep = 13;
	public const byte cardDepth = 3;
	public static Vector3 cardPoolInitPosition = new Vector3(-255f,275f,0f);
	public int[] cardTypeID = new int[4] {1,2,3,7};

	private static BattleConfigData instance;

	public static BattleConfigData Instance {
		get {
			if(instance == null) {
				instance = new BattleConfigData ();
			}
			return instance;
		}
	}

	private BattleConfigData () { 
	
	}
	
//	public Coordinate roleInitCoordinate;

	public QuestDungeonData questDungeonData;
	
	public QuestInfo currentQuestInfo;

	private StageInfo _currentStageInfo;
	public StageInfo currentStageInfo {
		set { _currentStageInfo = value; }// Debug.LogError("currentStageInfo : " + value + " id : " + value.ID) ; }
		get { return _currentStageInfo; }
	}

	public FriendInfo BattleFriend;

	public int gotFriendPoint;  //After ClearQuest response from server, gotFriendPoint will be assigned.

	public bool NotDeadEnemy = false;

	private UnitParty _party;
	public UnitParty party {
		get { return _party; }
		set {
			_party = value;
			WriteBuff<UnitParty>(unitPartyName, _party);
		}
	}

	private AttackInfoProto _posionAttack = null;
	public AttackInfoProto posionAttack {
		get { return _posionAttack; }
		set { _posionAttack = value;
			WriteBuff<AttackInfoProto> (posionAttackName, _posionAttack);
		}
	}

	private AttackInfoProto _reduceHurtAttack = null;
	public AttackInfoProto reduceHurtAttack {
		get { return _reduceHurtAttack; }
		set { _reduceHurtAttack = value; 
			WriteBuff<AttackInfoProto> (reduceHurtName, _reduceHurtAttack); 
		}
	}

	private AttackInfoProto _reduceDefenseAttack = null;
	public AttackInfoProto reduceDefenseAttack {
		get { return _reduceDefenseAttack; }
		set { _reduceDefenseAttack = value;
//			Debug.LogError(aip.skillID);
			WriteBuff<AttackInfoProto>(reduceDefenseName, _reduceDefenseAttack); 
		}
	}

	private AttackInfoProto _strengthenAttack = null;
	public AttackInfoProto strengthenAttack {
		get { return _strengthenAttack; }
		set { _strengthenAttack = value; 
//			Debug.LogError(aip.skillID);
			WriteBuff<AttackInfoProto>(strengthenAttackName, _strengthenAttack); 
		}
	}

	private TrapPosion _trapPoison = null;
	public TrapPosion trapPoison {
		get { return _trapPoison; }
		set { _trapPoison = value;
			WriteBuff<TrapPosion>(trapPoisonName, _trapPoison);
		}
	}

	private EnvironmentTrap _trapEnvironment = null;
	public EnvironmentTrap trapEnvironment {
		get { return _trapEnvironment; }
		set { _trapEnvironment = value; 
			WriteBuff<EnvironmentTrap>(trapEnvironmentName, _trapEnvironment);
		}
	}

	private StoreBattleData _storeBattleData;
	public StoreBattleData storeBattleData {
		get { return _storeBattleData; }
	}

	private UnitDataModel _evolveInfo;
	public UnitDataModel evolveInfo {
		set {
			_evolveInfo = value;
		}
	}
	
	public void ResetFromServer(QuestDungeonData tdd) {
		_storeBattleData = new StoreBattleData ();
		_storeBattleData.sp = DataCenter.maxEnergyPoint;
		_storeBattleData.hp = -1;//DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetInitBlood ();
		_storeBattleData.xCoordinate = MapConfig.characterInitCoorX;
		_storeBattleData.yCoordinate = MapConfig.characterInitCoorY;

		_storeBattleData.roleCoordinate = new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
		_storeBattleData.colorIndex = 0;
		questDungeonData = tdd;

		WriteToFile<FriendInfo> (BattleFriend, friendFileName);

		WriteToFile<QuestInfo> (currentQuestInfo, questInfoName);

		WriteToFile<StageInfo> (currentStageInfo, stageInfoName);

		WriteToFile<QuestDungeonData> (questDungeonData,questDungeonDataName);
	}

	public void ResetFromDisk() {
		BattleFriend = ReadFile<FriendInfo> (friendFileName);

		questDungeonData = ReadFile<QuestDungeonData> (questDungeonDataName);
		questDungeonData.assignData ();
//		questDungeonData.boss.

		currentQuestInfo = ReadFile<QuestInfo>(questInfoName);

		currentStageInfo = ReadFile<StageInfo>(stageInfoName);

		_storeBattleData = ReadFile<StoreBattleData> (storeBattleName);

		ReadAllBuff ();
		if (_storeBattleData.colorIndex > 5) {
			_storeBattleData.colorIndex -= 5;	
		} else {
			_storeBattleData.colorIndex  = 0;
		}
	}

	public void StoreMapData () {
		WriteAllBuff ();
//		StoreRuntimData ();
		WriteToFile<StoreBattleData> (_storeBattleData,storeBattleName);
	}

	public void StoreData (uint questID) {
		int id = (int)questID;
		GameDataPersistence.Instance.StoreIntDatNoEncypt (GameDataPersistence.battleStore, id);
//		Debug.LogError ("StoreData : " + id);
	}

	public void ClearData () {
		GameDataPersistence.Instance.StoreIntDatNoEncypt (GameDataPersistence.battleStore, 0);
//		Debug.LogError ("ClearData : " + GameDataStore.Instance.GetIntDataNoEncypt (GameDataStore.battleStore));
	}

	public int hasBattleData () {
		return GameDataPersistence.Instance.GetIntDataNoEncypt (GameDataPersistence.battleStore);
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
	public const string unitPartyName = "/UnitParty";

	public const string gameStateName = "GameState";

	public void Init(){
		questDungeonData.currentFloor = _storeBattleData.questData.Count > 0 ? _storeBattleData.questData.Count - 1 : 0;

	}

	string GetPath (string path) {
		return Application.persistentDataPath + path;
	}

	void WriteAllBuff() {
		if (posionAttack != null) {
			WriteBuff<AttackInfoProto> (posionAttackName, posionAttack);
		}
		if (reduceHurtAttack != null) {
			WriteBuff<AttackInfoProto> (reduceHurtName, reduceHurtAttack);
		}
		if (reduceDefenseAttack != null) {
			WriteBuff<AttackInfoProto> (reduceDefenseName, reduceDefenseAttack);
		}
		if (strengthenAttack != null) {
			WriteBuff<AttackInfoProto> (strengthenAttackName, strengthenAttack);
		}
	}

	void ReadAllBuff() {
		_posionAttack = ReadFile<AttackInfoProto> (posionAttackName);
		_reduceHurtAttack = ReadFile<AttackInfoProto> (reduceHurtName);
		_reduceDefenseAttack = ReadFile<AttackInfoProto> (reduceDefenseName);
		_strengthenAttack = ReadFile<AttackInfoProto> (strengthenAttackName);
		_party = ReadFile<UnitParty> (unitPartyName);
		_trapPoison = ReadFile<TrapPosion> (trapPoisonName);
		_trapEnvironment = ReadFile<EnvironmentTrap> ( trapEnvironmentName);


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
//		Debug.LogError (" WriteBuff<T> : " + name);
//		byte[] attack = ProtobufSerializer.SerializeToBytes<T> (buff);
		WriteToFile<T> (buff, name);
	}

	//end 

	void DeleteAndWrite(string fileName){
		if (File.Exists (fileName)) {
			File.Delete(fileName);
		}
	}

	void WriteToFile<T>(T data, string fileName){
		if(data == null)
			return;
		byte[] bytes = ProtobufSerializer.SerializeToBytes<T> (data);

		string path = GetPath (fileName);
		DeleteAndWrite (path);
		try {
			FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
			fs.Write(bytes, 0, bytes.Length);
			fs.Close();
			fs.Dispose();
//			Debug.LogError("write to file success : " + fileName);
		} catch (System.Exception ex) {
			Debug.LogError("WriteToFile exception : " + ex.Message);
		}
	}

	T ReadFile<T>(string fileName) {
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

		if (data == null) {
			return default(T);	
		}
		return ProtobufSerializer.ParseFormBytes<T> (data);

	}

	public int GetMapID () {
		if (currentStageInfo == null || NoviceGuideStepEntityManager.isInNoviceGuide()) {
			return 3;
		} else {
			int stageID = ((int)currentStageInfo.id) % 10;
			if (BattleConfigData.Instance.currentStageInfo.cityId == 1) {	
				return stageID == 1 ? 7 : -- stageID;
			} else {
				return stageID;
			}
		}
	}

	public void ResetRoleCoordinate(){
		_storeBattleData.roleCoordinate = new Coordinate(2,0);
	}

	public void InitRoleCoordinate(Coordinate coor){
		_storeBattleData.roleCoordinate = coor;

	}

	public void RefreshCurrentFloor(RspRedoQuest rrq){
		storeBattleData.questData.RemoveAt (storeBattleData.questData.Count - 1);
		storeBattleData.questData.Add (new ClearQuestParam ());
		int floor = questDungeonData.currentFloor;
		List<QuestGrid> reQuestGrid = rrq.dungeonData.Floors[floor];
		questDungeonData.Floors [floor] = reQuestGrid;
		questDungeonData.Boss = rrq.dungeonData.Boss;
	}

	
	public int ResumeColorIndex(){
		int i = questDungeonData.Colors [storeBattleData.colorIndex];
		storeBattleData.colorIndex++;
//		Debug.Log ("index: " + storeBattleData.colorIndex);
		return i;
	}
}
