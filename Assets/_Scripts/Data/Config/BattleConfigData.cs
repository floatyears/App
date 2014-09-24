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

	public const byte startCardID = 0;
	public const byte endCardID = 4;
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
	
	public Coordinate roleInitCoordinate;

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
			UnitParty up = _party == null ? null : _party;
			WriteBuff<UnitParty>(unitPartyName, up);
		}
	}

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
//			Debug.LogError(aip.skillID);
			WriteBuff<AttackInfoProto>(reduceDefenseName, aip); 
		}
	}

	private AttackInfo _strengthenAttack = null;
	public AttackInfo strengthenAttack {
		get { return _strengthenAttack; }
		set { _strengthenAttack = value; 
			AttackInfoProto aip = _strengthenAttack == null ? null : _strengthenAttack.Instance;
//			Debug.LogError(aip.skillID);
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
		_storeBattleData.hp = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetInitBlood ();
		_storeBattleData.xCoordinate = MapConfig.characterInitCoorX;
		_storeBattleData.yCoordinate = MapConfig.characterInitCoorY;

		_storeBattleData.roleCoordinate = roleInitCoordinate= new Coordinate (MapConfig.characterInitCoorX, MapConfig.characterInitCoorY);
		_storeBattleData.colorIndex = 0;
		questDungeonData = tdd;

		WriteToFile<FriendInfo> (BattleFriend, friendFileName);

		WriteToFile<QuestInfo> (currentQuestInfo, questInfoName);

		WriteToFile<StageInfo> (currentStageInfo, stageInfoName);

		WriteToFile<QuestDungeonData> (questDungeonData,questDungeonDataName);
	}

	public void ResetFromDisk() {
		BattleFriend = ReadFile<FriendInfo> (friendFileName);

		questDungeonData= ReadFile<QuestDungeonData> (questDungeonDataName);
		questDungeonData.assignData ();

		currentQuestInfo = ReadFile<QuestInfo>(questInfoName);

		currentStageInfo = ReadFile<StageInfo>(stageInfoName);

		_storeBattleData = ReadFile<StoreBattleData> (storeBattleName);

		ReadAllBuff ();
		roleInitCoordinate = _storeBattleData.roleCoordinate;
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

//	public void StoreQuestDungeonData(QuestDungeonData tqdd) {
//		questDungeonData = tqdd;
//
////		WriteQuestDungeonData ();
//		WriteToFile<QuestDungeonData> (questDungeonData,questDungeonDataName);
//	}

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
		AttackInfoProto attack = null;
		if (posionAttack != null) {
			attack = posionAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (posionAttackName, attack);
		attack = null;

		if (reduceHurtAttack != null) {
			attack = reduceHurtAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceHurtName, attack);
		attack = null;

		if (reduceDefenseAttack != null) {
			attack = reduceDefenseAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (reduceDefenseName, attack);
		attack = null;

		if (strengthenAttack != null) {
			attack = strengthenAttack.Instance;
		}
		WriteBuff<AttackInfoProto> (strengthenAttackName, attack);
		attack = null;
	}

	void ReadAllBuff() {
		_posionAttack = ReadBuff<AttackInfo, AttackInfoProto> (posionAttackName);
		_reduceHurtAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceHurtName);
		_reduceDefenseAttack = ReadBuff<AttackInfo, AttackInfoProto> (reduceDefenseName);
		_strengthenAttack = ReadBuff<AttackInfo, AttackInfoProto> (strengthenAttackName);
//		_trapPoison = ReadBuff<TrapPosion, TrapInfo> (trapPoisonName);
//		_trapEnvironment = ReadBuff<EnvironmentTrap, TrapInfo> (trapEnvironmentName);
		_party = ReadBuff<UnitParty, UnitParty> (unitPartyName);

		if (File.Exists (trapPoisonName)) {
			_trapPoison = Activator.CreateInstance(typeof(TrapPosion), ReadFile<TrapInfo> (trapPoisonName)) as TrapPosion;
		}
		if (File.Exists (trapEnvironmentName)) {
			_trapEnvironment = Activator.CreateInstance(typeof(EnvironmentTrap), ReadFile<TrapInfo> ( trapEnvironmentName)) as EnvironmentTrap;
		}


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

	T ReadBuff<T,T1> (string name) where T : class where T1 : ProtoBuf.IExtensible {
		if (string.IsNullOrEmpty (name)) {
			return default(T);	
		}
		string path = GetPath (name);
		if (!File.Exists (path)) {
			return default(T);	
		}

		T1 aip = ReadFile<T1> (name);
		T t = Activator.CreateInstance(typeof(T), aip) as T;
//		Debug.LogError ("t : " + t);
		return t;
	}


	
	//stage
	public void WriteStageInfo() {

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
		_storeBattleData.roleCoordinate = roleInitCoordinate;
	}

	public void InitRoleCoordinate(Coordinate coor){
		roleInitCoordinate = coor;
		_storeBattleData.roleCoordinate = coor;

	}

	public void RefreshCurrentFloor(RspRedoQuest rrq){
		storeBattleData.questData.RemoveAt (storeBattleData.questData.Count - 1);
		ClearQuestParam cq = new ClearQuestParam ();
		storeBattleData.questData.Add (cq);
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
