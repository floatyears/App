using UnityEngine;

using bbproto;
using LitJson;
using System.Collections.Generic;

public enum ModelEnum {
    UserInfo = 100,
    AccountInfo,
    SupportFriends,
    FriendList,
    PartyInfo,
    MyUnitList,
    UserUnitList,
    UnitValue,
    Skill,
	AllSkill,
    UnitInfo,
    EnemyInfo,
    UnitBaseInfo,
    TrapInfo,
    FriendBaseInfo,
	QuestClearInfo,
	UnitCatalogInfo,
	NoticeInfo,
	HelperInfo,
	LoginInfo,
	EventStageList,
	AudioList,
	ViewData,

	//new add
	CityListInfo,

    User            = 1000,
    UnitPartyInfo   = 1001,
    FriendCount     = 1002,
    
    UIInsConfig     = 2000,
    MapConfig       = 2001,

    /// temp

    HaveCard,
    InEventGacha,
    ItemObject,

	CityInfo,
	DragPanelConfig,
}

/// <summary>
/// game state, tag current game in any state.
/// </summary>
public enum GameState : byte {
	Normal = 0,

	Evolve = 1,
}

public class DataCenter {
    public static DataCenter Instance {
        get {
            if (instance == null) {
                instance = new DataCenter();
            }
            return instance;
        } 
    }
    private static DataCenter instance;
    private DataCenter() { 

	}
	private Dictionary<ModelEnum, object> modelDataDic = new Dictionary<ModelEnum, object>();
	/// <summary>
	/// get the data to use
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="modelType">Model type.</param>
	/// <param name="erroMsg">Erro message.</param>
	private object GetData(ModelEnum modelType) {
		
		if (!modelDataDic.ContainsKey(modelType)) {
			Debug.LogError("required key [[[---" + modelType + "---]]] not exist in ModelManager");
			return null;
		}
		return modelDataDic[modelType];
	}
	
	public void Init() {
		//      ConfigUnitInfo cui = new ConfigUnitInfo();
		//		Debug.LogWarning ("InitData ConfigSkill");
		supportFriendManager = new SupportFriendManager ();
		
		ResourceManager.Instance.LoadLocalAsset (PathConfig.UIInsConfigPath, o => {
			new UIConfigData ((o as TextAsset).text);

		});
		ResourceManager.Instance.LoadLocalAsset(PathConfig.DragPanelConfigPath,o=>{
			new DragPanelData((o as TextAsset).text);
		});
		new ConfigSkill();
		//      ConfigEnermy ce = new ConfigEnermy();
		new ConfigUnitBaseInfo();
		new ConfigTrap();
		
//		ConfigFriendList configFriendList = new ConfigFriendList();
		new ConfigAudio();
		//      ConfigStage stage = new ConfigStage();
		new ConfigViewData();
		//		ConfigNoteMessage noteMsgConfig = new ConfigNoteMessage();
	}
	
	/// <summary>
	/// Adds the data.
	/// </summary>
	/// <param name="modelType">Model type.</param>
	/// <param name="model">Model.</param>
	public void SetData(ModelEnum modelType, object model) {
		if (modelDataDic.ContainsKey(modelType)) {
			modelDataDic[modelType] = model;
		} else {
			modelDataDic.Add(modelType, model);	
		}
	}

	

	private static GameState _gameState = GameState.Normal;
	public static GameState gameState {
		set { _gameState = value; BattleConfigData.Instance.gameState = (byte)_gameState; }
		get { return _gameState; }
	}

	public static UnitDataModel evolveInfo = null;

    public const int maxEnergyPoint = 20;
    public const int posStart = 0;
    public const int posEnd = 5;
    public const int minNeedCard = 2;
    public const int maxNeedCard = 5;
    public const int maxFriendLimit = 200;

    public const int maxUnitLimit = 400;

	public const int friendsExpandCount = 5; //每次扩充好友数量

	//商城花费
	public const int redoQuestStone = 60;
	public const int resumeQuestStone = 60;

	public const int friendExpansionStone = 60;
    public const int unitExpansionStone = 60;
    public const int staminaRecoverStone = 60;
    
    public const int rareGachaStone = 300;
    public const int eventGachaStone = 300;
    public const int maxGachaPerTime = 9;
	public const int friendGachaFriendPoint = 200;

	public const int friendPos = 4;

	private UserDataModel userData;

	public UserDataModel UserData{
		get{
			return userData;
		}
		set{
			UserData = value;
		}
	}

    public UserInfo UserInfo { 
        get { return GetData(ModelEnum.UserInfo) as UserInfo; } 
        set { 
			SetData(ModelEnum.UserInfo, value);
			value.Init();
		} 
    }


    public AccountInfo AccountInfo {
        get { return GetData(ModelEnum.AccountInfo) as AccountInfo; }
        set { SetData(ModelEnum.AccountInfo, value); }
    }

	public SupportFriendManager supportFriendManager;

    public List<FriendInfo> SupportFriends {
		get { return supportFriendManager.GetSupportFriend(); }
		set { supportFriendManager.AddSupportFriend(value); }
    }

    public FriendDataModel FriendList { 
        get { return GetData(ModelEnum.FriendList) as FriendDataModel; }
        set { SetData(ModelEnum.FriendList, value); } 
    }

	public List<StageInfo> EventStageList { 
		get { return GetData(ModelEnum.EventStageList) as List<StageInfo>; }
		set { SetData(ModelEnum.EventStageList, value); } 
	}

	public QuestClearInfo QuestClearInfo {
		get { return GetData(ModelEnum.QuestClearInfo) as QuestClearInfo; }
		set { SetData(ModelEnum.QuestClearInfo, value); }
	}

    public bool InEventGacha {
        get {
            bool ret = true;
            if (GetData(ModelEnum.InEventGacha) == null){
                SetData(ModelEnum.InEventGacha, true);
                ret = true;
            } else {
                ret = (bool)GetData(ModelEnum.InEventGacha);
            }
            return ret;
        }
        set {
            SetData(ModelEnum.InEventGacha, value);
        }
    }

    public int FriendCount {
        get {
            int ret = 0;
            if (GetData(ModelEnum.FriendCount) != null){
                ret = (int)GetData(ModelEnum.FriendCount);
            }
            else{
                List<FriendInfo> supporters = SupportFriends;
                if (supporters != null){
                    for (int i = 0; i < supporters.Count; i++){
                        if (supporters[i].friendState == EFriendState.ISFRIEND){
                            ret += 1;
                        }
                    }
                    LogHelper.Log("total friends from supporters = {0}", ret);
                }
                SetData(ModelEnum.FriendCount, ret);
            }
            return ret;
        }
        set {
            SetData(ModelEnum.FriendCount, value);
        }
    }
    public PartyInfo PartyInfo { 
        get { return GetData(ModelEnum.PartyInfo) as PartyInfo; }
        set { 
			SetData(ModelEnum.PartyInfo, value);
			value.assignParty();
		}
    }

	public UnitCatalogInfo CatalogInfo { 
		get { return GetData(ModelEnum.UnitCatalogInfo) as UnitCatalogInfo; }
		set { SetData(ModelEnum.UnitCatalogInfo, value); }
	}

	public NoticeInfo NoticeInfo { 
		get { return GetData(ModelEnum.NoticeInfo) as NoticeInfo; }
		set { SetData(ModelEnum.NoticeInfo, value); }
	}

	public StatHelperCount HelperCount{
		get { return GetData(ModelEnum.HelperInfo) as StatHelperCount; }
		set { SetData(ModelEnum.HelperInfo, value); }
	}


	public LoginInfo LoginInfo { 
		get { return GetData(ModelEnum.LoginInfo) as LoginInfo; }
		set { SetData(ModelEnum.LoginInfo, value); }
	}


	/// <summary>
	/// store operate befoure account info
	/// </summary>
	public UserInfo oldAccountInfo = null;

	/// <summary>
	/// store befoure levelup's level
	/// </summary>
	public UserUnit oldUserUnitInfo = null;

	/// <summary>
	/// store levelup's materials
	/// </summary>
	public List<UserUnit> levelUpMaterials = new List<UserUnit> ();

	/// <summary>
	/// store levelup helper info.
	/// </summary>
	public UserUnit levelUpFriend = null;

    public UserUnitList UserUnitList {
        get { 
            UserUnitList ret = GetData(ModelEnum.UserUnitList) as UserUnitList;
            if (ret == null) {
                ret = new UserUnitList();
                SetData(ModelEnum.UserUnitList, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.UserUnitList, value); } 
    }

    // unit configs table(come from config file: ) e.g.<hp, hpLevelConfigList>
    public Dictionary<int,PowerTable> UnitValue {
        get { 
            Dictionary<int,PowerTable> ret = GetData(ModelEnum.UnitValue) as Dictionary<int, PowerTable>;
            if (ret == null) {
                ret = new Dictionary<int,PowerTable>();
                SetData(ModelEnum.UnitValue, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.UnitValue, value); } 
    }

    public Dictionary<int, SkillBase> Skill {
        get { 
            Dictionary<int, SkillBase> ret = GetData(ModelEnum.Skill) as Dictionary<int, SkillBase>;
            if (ret == null) {
                ret = new Dictionary<int, SkillBase>();
                SetData(ModelEnum.Skill, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.Skill, value); } 
    }

	public Dictionary<string, SkillBase> AllSkill {
		get { 
			Dictionary<string, SkillBase> ret = GetData(ModelEnum.AllSkill) as Dictionary<string, SkillBase>;
			if (ret == null) {
				ret = new Dictionary<string, SkillBase>();
				SetData(ModelEnum.AllSkill, ret);
			}
			return ret; 
		}
		set { SetData(ModelEnum.AllSkill, value); } 
	}

	public SkillBase GetSkill(string userUnitID, int skillID, SkillType skillType) {
		if (skillID == 0) {

			return null;
		}
		SkillBase skill = null;
		string skillUserID = GetSkillID (userUnitID, skillID);

		if (!AllSkill.TryGetValue (skillUserID, out skill)) {
			skill = DGTools.LoadSkill(skillID, skillType);
			if(skill == null) {
				Debug.LogError("load skill faile. not have this skill config ! " + userUnitID + " skillID : " + skillID) ;
				return null;
			}
			AllSkill.Add(skillUserID, skill);
		}

		return skill;
	}

	
	public string GetSkillID (string userUnitID, int skillID) {
		return userUnitID + "_" + skillID;
	}


    private Dictionary<uint, UnitInfo>  UnitInfo {
        get { 
            Dictionary<uint, UnitInfo> ret = GetData(ModelEnum.UnitInfo) as Dictionary<uint, UnitInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, UnitInfo>();
                SetData(ModelEnum.UnitInfo, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.UnitInfo, value); } 
    }

    public Dictionary<uint, EnemyInfo> EnemyInfo {
        get { 
            Dictionary<uint, EnemyInfo> ret = GetData(ModelEnum.EnemyInfo) as Dictionary<uint, EnemyInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, EnemyInfo>();
                SetData(ModelEnum.EnemyInfo, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.EnemyInfo, value); } 
    }

//    public Dictionary<int, UnitBaseInfo> UnitBaseInfo {
//        get { 
//            Dictionary<int, UnitBaseInfo> ret = getData(ModelEnum.UnitBaseInfo) as Dictionary<int, UnitBaseInfo>;
//            if (ret == null) {
//                ret = new Dictionary<int, UnitBaseInfo>();
//                setData(ModelEnum.UnitBaseInfo, ret);
//            }
//            return ret; 
//        }
//        set { setData(ModelEnum.UnitBaseInfo, value); } 
//    }

	//new add by Lynn
	public List<CityInfo> CityListInfo{
		get { 
			List<CityInfo> ret = GetData(ModelEnum.CityListInfo) as List<CityInfo>;
			if (ret == null) {
				ret = new List<CityInfo>();
				SetData(ModelEnum.CityListInfo, ret);
			}
			return ret; 
		}
		set { SetData(ModelEnum.CityListInfo, value); } 
	}

	public Dictionary<uint, CityInfo> CityInfo {
		get {
			Dictionary<uint, CityInfo> ret = GetData(ModelEnum.CityInfo) as Dictionary<uint, CityInfo>;
			if (ret == null) {
				ret = new Dictionary<uint, CityInfo>();
				SetData(ModelEnum.CityInfo, ret);
			}
			return ret;
		}
		set { SetData(ModelEnum.UnitBaseInfo, value); }
	}

	//-------new add


	//-------end new add

    public Dictionary<uint, TrapBase> TrapInfo {
        get { 
            Dictionary<uint, TrapBase> ret = GetData(ModelEnum.TrapInfo) as Dictionary<uint, TrapBase>;
            if (ret == null) {
                ret = new Dictionary<uint, TrapBase>();
                SetData(ModelEnum.TrapInfo, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.TrapInfo, value); } 
    }

//    public UnitBaseInfo FriendBaseInfo {
//        get {
//            UnitBaseInfo ret = getData(ModelEnum.FriendBaseInfo) as UnitBaseInfo;
//
//            if (ret == null) {
//                ret = UnitBaseInfo[195];
//                setData(ModelEnum.FriendBaseInfo, ret);
//            }
//            return ret;
//        }
//        set { setData(ModelEnum.FriendBaseInfo, value); } 
//    }

    public List<int> HaveCard {
        get {
            List<int> ret = GetData(ModelEnum.HaveCard) as List<int>;
            if (ret == null) {
                ret = new List<int>() {111,185,161,101,122,195};
                SetData(ModelEnum.HaveCard, ret);
            }
            return ret; 
        }
        set { SetData(ModelEnum.HaveCard, value); }
    }

    public GameObject ItemObject {
        get {
            GameObject ret = GetData(ModelEnum.ItemObject) as GameObject; 
            if (ret == null) {
				ret = ResourceManager.Instance.LoadLocalAsset("Prefabs/Item/FriendScrollerItem",null) as GameObject;
                SetData(ModelEnum.ItemObject, ret);
            }
            return ret;
        }
        set { SetData(ModelEnum.ItemObject, value); } 
    }

    public void RefreshUserInfo(TRspClearQuest clearQuest) {
        if (clearQuest == null) {
            return; 
        }
        UserInfo.RefreshUserInfo(clearQuest);
        AccountInfo.RefreshAcountInfo(clearQuest);
    }

    public void SetFriendList(FriendList friendList){
		if (friendList == null){
			return;
		}
        if (FriendList == null){
            FriendList = new FriendDataModel(friendList);
        }
        else {
            FriendList.RefreshFriendList(friendList);
        }
    }
    
    // return UserCost of curr Rank.
    public int UserCost {
        get {
            return GetUnitValue(PowerTable.UserCostMax, UserInfo.rank); 
        }
    }
    
    /// <summary>
    /// Gets the unit value.  1 =  exp. 2 = attack. 3 = hp. 4 = rankCost
    /// </summary>
    /// <returns>The unit value.</returns>
    /// <param name="type">Type.</param>
    /// <param name="level">Level.</param>
    
    public int GetUnitValue(int type, int level) {
		if ( !UnitValue.ContainsKey(type)) {
			Debug.LogError("FATAL ERROR: GetUnitValue() :: type:"+type+" not exists in UnitValue.");
			return 0;
		}

        PowerTable pti = UnitValue[type];
        return pti.GetValue(level);
    }

//    public int GetUnitValueTotal(int type, int level) {
//        TPowerTableInfo pti = UnitValue[type];
//        int totalValue = 0;
//
//        for (int i=1; i<=level; i++) {
//			totalValue += pti.GetValue(i);
//		}
//
//        return totalValue;
//    }

    public UnitInfo GetUnitInfo(uint unitID) {
        if (UnitInfo.ContainsKey(unitID)) {
            UnitInfo tui = UnitInfo[unitID];
            return tui;
        }
        else {
			UnitInfo tui = DGTools.LoadUnitInfoProtobuf(unitID);
			if(tui == null) {
				Debug.LogError("uintid : " + unitID + " is invalid");
				return null;
			}
			UnitInfo.Add(tui.id,tui);
			return tui;
        }
    }

	//PS: GetStageInfo() only used for story stage ( event stage cannot use)
	public StageInfo GetStageInfo (uint stageID) {
		uint cityId = stageID/10;
		CityInfo cityInfo = GetCityInfo(cityId);
		for(int i=0; i < cityInfo.stages.Count; i++) {
			if (stageID==cityInfo.stages[i].id)
				return cityInfo.stages[i];
		}

		return null;
	}

	public CityInfo GetCityInfo (uint cityID) {
		if (CityInfo.ContainsKey(cityID)) {
			CityInfo tui = CityInfo[cityID];
			return tui;
		}
		else {
			CityInfo tui = DGTools.LoadCityInfo(cityID);
			if(tui == null) {
				Debug.LogError("city id : " + cityID + " is invalid");
				return null;
			}
			//CityInfo.Add(tui.ID,tui);
			CityInfo.Add(cityID,tui);
			return tui;
		}
	}

	public List<CityInfo> GetCityListInfo(){
		if(CityListInfo.Count == 0){
//			Debug.Log("DataCenter.GetCityListInfo(), CityListInfo is NULL");
			CityListInfo = DGTools.LoadCityList();
		}
//		Debug.Log("DataCenter.GetCityListInfo(), CityListInfo count is : " + CityListInfo.Count);
		return CityListInfo;
	}

	public FriendInfo GetSupporterInfo(uint friendUid){
		foreach (var item in SupportFriends) {
			if (item.userId == friendUid) {
				return item;
			}
		}
		return null;
	}
    
    public int GetFriendGachaNeedPoints(){
        return DataCenter.friendGachaFriendPoint;
    }
    
    public int GetAvailableFriendGachaTimes(){
        if (GetFriendGachaNeedPoints() == 0)
            return 0;
        return AccountInfo.friendPoint / GetFriendGachaNeedPoints();
    }

    public int GetRareGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableRareGachaTimes(){
        if (GetRareGachaNeedStones() == 0)
            return 0;
        return AccountInfo.stone / GetRareGachaNeedStones();
    }

    public int GetEventGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableEventGachaTimes(){
        if (!InEventGacha)
            return 0;
        if (GetEventGachaNeedStones() == 0)
            return 0;
        return AccountInfo.stone / GetEventGachaNeedStones();
    }

	public List<AudioConfigItem> ConfigAudioList{
		get{
			return GetData(ModelEnum.AudioList) as List<AudioConfigItem>;
		}
	}

	public List<UserUnit> ConfigViewData{
		get{
			return GetData(ModelEnum.ViewData) as List<UserUnit>;
		}
	}

	public UIConfigItem GetConfigUIItem(ModuleEnum name){
		Dictionary<ModuleEnum, UIConfigItem> uiData = GetData (ModelEnum.UIInsConfig) as Dictionary<ModuleEnum, UIConfigItem>;
		if(uiData.ContainsKey(name)){
			return uiData[name];
		}else{
			Debug.LogError("No UIConfig Item: [[[---" + name + "---]]]");
			return null;
		}
	}

	public DragPanelConfigItem GetConfigDragPanelItem(string name){
		Dictionary<string, DragPanelConfigItem> dragData = GetData (ModelEnum.DragPanelConfig) as Dictionary<string, DragPanelConfigItem>;
		if(dragData.ContainsKey(name)){
			Debug.Log("drag panel: " + name);
			return dragData[name];
		}else{
			Debug.LogError("No DragConfig Item: [[[---" + name + "---]]]");
			return null;
		}
	}

}