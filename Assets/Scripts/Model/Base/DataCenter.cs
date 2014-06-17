using UnityEngine;
using System.Collections.Generic;
using bbproto;

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
	LoginInfo,
	EventStageList,

	//new add
	CityListInfo,

    User            = 1000,
    UnitPartyInfo   = 1001,
    FriendCount     = 1002,
    
    UIInsConfig     = 2000,
    MapConfig       = 2001,

    /// temp
    TempEffect      = 10000, 
    HaveCard,
    InEventGacha,
    ItemObject,

	CityInfo,
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
		supportFriendManager = new SupportFriendManager ();
	}

	private static GameState _gameState = GameState.Normal;
	public static GameState gameState {
		set { _gameState = value; ConfigBattleUseData.Instance.gameState = (byte)_gameState; }
		get { return _gameState; }
	}

	public static TEvolveStart evolveInfo = null;

    public const int maxEnergyPoint = 20;
    public const int posStart = 0;
    public const int posEnd = 5;
    public const int minNeedCard = 2;
    public const int maxNeedCard = 5;
    public const int maxFriendLimit = 200;
    public const int maxUnitLimit = 400;
    public const int friendExpansionStone = 1;
    public const int unitExpansionStone = 1;
    public const int staminaRecoverStone = 1;
    public const int friendGachaFriendPoint = 200;
    public const int rareGachaStone = 5;
    public const int eventGachaStone = 5;
    public const int maxGachaPerTime = 9;
	public const int friendPos = 4;

//	private static StartQuestParam startQuestInfo;
//	public static StartQuestParam StartQuestInfo{
//		set { startQuestInfo = value; }
//		get { return startQuestInfo; }
//	}

//	private TFriendInfo battleFriend;
//	public TFriendInfo BattleFriend {
//		set { battleFriend = value; }
//		get { return battleFriend; }
//	}

    public TUserInfo UserInfo { 
        get { return getData(ModelEnum.UserInfo) as TUserInfo; } 
        set { setData(ModelEnum.UserInfo, value); } 
    }
    public TAccountInfo AccountInfo {
        get { return getData(ModelEnum.AccountInfo) as TAccountInfo; }
        set { setData(ModelEnum.AccountInfo, value); }
    }

	public SupportFriendManager supportFriendManager;

    public List<TFriendInfo> SupportFriends {
		get { return supportFriendManager.GetSupportFriend(); }
		set { supportFriendManager.AddSupportFriend(value); }
    }

    public TFriendList FriendList { 
        get { return getData(ModelEnum.FriendList) as TFriendList; }
        set { setData(ModelEnum.FriendList, value); } 
    }

	public List<TStageInfo> EventStageList { 
		get { return getData(ModelEnum.EventStageList) as List<TStageInfo>; }
		set { setData(ModelEnum.EventStageList, value); } 
	}

	public TQuestClearInfo QuestClearInfo {
		get { return getData(ModelEnum.QuestClearInfo) as TQuestClearInfo; }
		set { setData(ModelEnum.QuestClearInfo, value); }
	}

    public bool InEventGacha {
        get {
            bool ret = true;
            if (getData(ModelEnum.InEventGacha) == null){
                setData(ModelEnum.InEventGacha, true);
                ret = true;
            }
            else {
                ret = (bool)getData(ModelEnum.InEventGacha);
            }
            return ret;
        }
        set {
            setData(ModelEnum.InEventGacha, value);
        }
    }

    public int FriendCount {
        get {
            int ret = 0;
            if (getData(ModelEnum.FriendCount) != null){
                ret = (int)getData(ModelEnum.FriendCount);
            }
            else{
                List<TFriendInfo> supporters = SupportFriends;
                if (supporters != null){
                    for (int i = 0; i < supporters.Count; i++){
                        if (supporters[i].FriendState == EFriendState.ISFRIEND){
                            ret += 1;
                        }
                    }
                    LogHelper.Log("total friends from supporters = {0}", ret);
                }
                setData(ModelEnum.FriendCount, ret);
            }
            return ret;
        }
        set {
            setData(ModelEnum.FriendCount, value);
        }
    }
    public TPartyInfo PartyInfo { 
        get { return getData(ModelEnum.PartyInfo) as TPartyInfo; }
        set { setData(ModelEnum.PartyInfo, value); }
    }

	public TUnitCatalog CatalogInfo { 
		get { return getData(ModelEnum.UnitCatalogInfo) as TUnitCatalog; }
		set { setData(ModelEnum.UnitCatalogInfo, value); }
	}

	public TNoticeInfo NoticeInfo { 
		get { return getData(ModelEnum.NoticeInfo) as TNoticeInfo; }
		set { setData(ModelEnum.NoticeInfo, value); }
	}

	public TLoginInfo LoginInfo { 
		get { return getData(ModelEnum.LoginInfo) as TLoginInfo; }
		set { setData(ModelEnum.LoginInfo, value); }
	}


	/// <summary>
	/// store operate befoure account info
	/// </summary>
	public TUserInfo oldAccountInfo = null;

	/// <summary>
	/// store befoure levelup's level
	/// </summary>
	public TUserUnit oldUserUnitInfo = null;

    //TODO: reconstruct myUnitList
//    public UserUnitList MyUnitList { 
//        get { 
//            UserUnitList ret = getData(ModelEnum.MyUnitList) as UserUnitList;
//            if (ret == null) {
//                ret = new UserUnitList();
//                setData(ModelEnum.MyUnitList, ret);
//            }
//            return ret; 
//        }
//        set { setData(ModelEnum.MyUnitList, value); } 
//    }

    public UserUnitList UserUnitList {
        get { 
            UserUnitList ret = getData(ModelEnum.UserUnitList) as UserUnitList;
            if (ret == null) {
                ret = new UserUnitList();
                setData(ModelEnum.UserUnitList, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UserUnitList, value); } 
    }

    // unit configs table(come from config file: ) e.g.<hp, hpLevelConfigList>
    public Dictionary<int,TPowerTableInfo> UnitValue {
        get { 
            Dictionary<int,TPowerTableInfo> ret = getData(ModelEnum.UnitValue) as Dictionary<int, TPowerTableInfo>;
            if (ret == null) {
                ret = new Dictionary<int,TPowerTableInfo>();
                setData(ModelEnum.UnitValue, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitValue, value); } 
    }

    public Dictionary<int, SkillBaseInfo> Skill {
        get { 
            Dictionary<int, SkillBaseInfo> ret = getData(ModelEnum.Skill) as Dictionary<int, SkillBaseInfo>;
            if (ret == null) {
                ret = new Dictionary<int, SkillBaseInfo>();
                setData(ModelEnum.Skill, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.Skill, value); } 
    }

	public Dictionary<string, SkillBaseInfo> AllSkill {
		get { 
			Dictionary<string, SkillBaseInfo> ret = getData(ModelEnum.AllSkill) as Dictionary<string, SkillBaseInfo>;
			if (ret == null) {
				ret = new Dictionary<string, SkillBaseInfo>();
				setData(ModelEnum.AllSkill, ret);
			}
			return ret; 
		}
		set { setData(ModelEnum.AllSkill, value); } 
	}

	public SkillBaseInfo GetSkill(string userUnitID, int skillID, SkillType skillType) {
		if (skillID == 0) {

			return null;
		}
		SkillBaseInfo skill = null;
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

	
	 string GetSkillID (string userUnitID, int skillID) {
		return userUnitID + "_" + skillID;
	}


    private Dictionary<uint, TUnitInfo>  UnitInfo {
        get { 
            Dictionary<uint, TUnitInfo> ret = getData(ModelEnum.UnitInfo) as Dictionary<uint, TUnitInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, TUnitInfo>();
                setData(ModelEnum.UnitInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitInfo, value); } 
    }

    public Dictionary<uint, TEnemyInfo> EnemyInfo {
        get { 
            Dictionary<uint, TEnemyInfo> ret = getData(ModelEnum.EnemyInfo) as Dictionary<uint, TEnemyInfo>;
            if (ret == null) {
                ret = new Dictionary<uint, TEnemyInfo>();
                setData(ModelEnum.EnemyInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.EnemyInfo, value); } 
    }

    public Dictionary<int, UnitBaseInfo> UnitBaseInfo {
        get { 
            Dictionary<int, UnitBaseInfo> ret = getData(ModelEnum.UnitBaseInfo) as Dictionary<int, UnitBaseInfo>;
            if (ret == null) {
                ret = new Dictionary<int, UnitBaseInfo>();
                setData(ModelEnum.UnitBaseInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitBaseInfo, value); } 
    }

	//new add by Lynn
	public List<TCityInfo> CityListInfo{
		get { 
			List<TCityInfo> ret = getData(ModelEnum.CityListInfo) as List<TCityInfo>;
			if (ret == null) {
				ret = new List<TCityInfo>();
				setData(ModelEnum.CityListInfo, ret);
			}
			return ret; 
		}
		set { setData(ModelEnum.CityListInfo, value); } 
	}

	public Dictionary<uint, TCityInfo> CityInfo {
		get {
			Dictionary<uint, TCityInfo> ret = getData(ModelEnum.CityInfo) as Dictionary<uint, TCityInfo>;
			if (ret == null) {
				ret = new Dictionary<uint, TCityInfo>();
				setData(ModelEnum.CityInfo, ret);
			}
			return ret;
		}
		set { setData(ModelEnum.UnitBaseInfo, value); }
	}

	//-------new add


	//-------end new add

    public Dictionary<uint, TrapBase> TrapInfo {
        get { 
            Dictionary<uint, TrapBase> ret = getData(ModelEnum.TrapInfo) as Dictionary<uint, TrapBase>;
            if (ret == null) {
                ret = new Dictionary<uint, TrapBase>();
                setData(ModelEnum.TrapInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.TrapInfo, value); } 
    }

    public UnitBaseInfo FriendBaseInfo {
        get {
            UnitBaseInfo ret = getData(ModelEnum.FriendBaseInfo) as UnitBaseInfo;

            if (ret == null) {
                ret = UnitBaseInfo[195];
                setData(ModelEnum.FriendBaseInfo, ret);
            }
            return ret;
        }
        set { setData(ModelEnum.FriendBaseInfo, value); } 
    }

    public Dictionary<string, Object> TempEffect {
        get { 
			Dictionary<string, Object> ret = getData(ModelEnum.TempEffect) as Dictionary<string, Object>;
            if (ret == null) {
				ret = new Dictionary<string, Object>();
                setData(ModelEnum.TempEffect, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.TempEffect, value); } 
    } 

    public List<int> HaveCard {
        get {
            List<int> ret = getData(ModelEnum.HaveCard) as List<int>;
            if (ret == null) {
                ret = new List<int>() {111,185,161,101,122,195};
                setData(ModelEnum.HaveCard, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.HaveCard, value); }
    }

    public GameObject ItemObject {
        get {
            GameObject ret = getData(ModelEnum.ItemObject) as GameObject; 
            if (ret == null) {
                ret = ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
                setData(ModelEnum.ItemObject, ret);
            }
            return ret;
        }
        set { setData(ModelEnum.ItemObject, value); } 
    }

//	public Dictionary<string, string> effectPath = new Dictionary<string, string> ();

    public Object GetEffect(AttackInfo ai) {
		int type = ai.AttackType;
		int attackRange = ai.AttackRange;
		string name = type + "" + attackRange;
        Object obj = null;
		if (!TempEffect.TryGetValue(name, out obj)) {
            string path = GetEffectPath(type,attackRange);
			//never use the raw interface! Use ResourceManager instead
			obj = ResourceManager.Instance.LoadLocalAsset(path);
           // obj = ResourceManager.Instance.LoadLocalAsset(path);
			TempEffect.Add(name, obj);
        }
        return obj;
    }

	public Object GetMapEffect(string name) {
		Object obj = null;
		if (!TempEffect.TryGetValue(name, out obj)) {
			string path = EffectPath.Instance.GetEffectPath(name);
			//never use the raw interface! Use ResourceManager instead
			obj = ResourceManager.Instance.LoadLocalAsset(path);
//			Debug.LOGR
		//	obj = ResourceManager.Instance.LoadLocalAsset(path);
			TempEffect.Add(name, obj);
		}
		return obj;
	}
    
    public string GetEffectPath(int type,int attackRange) {
        string path = string.Empty;
        switch (type) {
        case 1:
			if(attackRange == 0) {
				path = "Effect/BOOM";
			}else  {
				path = "Effect/firerain";
			}
            break;
        case 2:
            path = "Effect/daoguang";
//			path = "Effect/Ice2";
            break;
        case 3:
            path = "Effect/zhua";
//			path = "Effect/jiufeng";
            break;
        case 8:
            path = "Effect/card_effect";
            break;
        default:
            break;
        }
        return path;
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
            FriendList = new TFriendList(friendList);
        }
        else {
            FriendList.RefreshFriendList(friendList);
        }
    }
    
    // return UserCost of curr Rank.
    public int UserCost {
        get {
            return GetUnitValue(TPowerTableInfo.UserCostMax, UserInfo.Rank); 
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

        TPowerTableInfo pti = UnitValue[type];
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

    public TUnitInfo GetUnitInfo(uint unitID) {
        if (UnitInfo.ContainsKey(unitID)) {
            TUnitInfo tui = UnitInfo[unitID];
            return tui;
        }
        else {
			TUnitInfo tui = DGTools.LoadUnitInfoProtobuf(unitID);
			if(tui == null) {
				Debug.LogError("uintid : " + unitID + " is invalid");
				return null;
			}
			UnitInfo.Add(tui.ID,tui);
			return tui;
        }
    }

	//PS: GetStageInfo() only used for story stage ( event stage cannot use)
	public TStageInfo GetStageInfo (uint stageID) {
		uint cityId = stageID/10;
		TCityInfo cityInfo = GetCityInfo(cityId);
		for(int i=0; i < cityInfo.Stages.Count; i++) {
			if (stageID==cityInfo.Stages[i].ID)
				return cityInfo.Stages[i];
		}

		return null;
	}

	public TCityInfo GetCityInfo (uint cityID) {
		if (CityInfo.ContainsKey(cityID)) {
			TCityInfo tui = CityInfo[cityID];
			return tui;
		}
		else {
			TCityInfo tui = DGTools.LoadCityInfo(cityID);
			if(tui == null) {
				Debug.LogError("city id : " + cityID + " is invalid");
				return null;
			}
			//CityInfo.Add(tui.ID,tui);
			CityInfo.Add(cityID,tui);
			return tui;
		}
	}

	public List<TCityInfo> GetCityListInfo(){
		if(CityListInfo.Count == 0){
//			Debug.Log("DataCenter.GetCityListInfo(), CityListInfo is NULL");
			CityListInfo = DGTools.LoadCityList();
		}
//		Debug.Log("DataCenter.GetCityListInfo(), CityListInfo count is : " + CityListInfo.Count);
		return CityListInfo;
	}

	public TFriendInfo GetSupporterInfo(uint friendUid){
		foreach (var item in SupportFriends) {
			if (item.UserId == friendUid) {
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
        return AccountInfo.FriendPoint / GetFriendGachaNeedPoints();
    }

    public int GetRareGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableRareGachaTimes(){
        if (GetRareGachaNeedStones() == 0)
            return 0;
        return AccountInfo.Stone / GetRareGachaNeedStones();
    }

    public int GetEventGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableEventGachaTimes(){
        if (!InEventGacha)
            return 0;
        if (GetEventGachaNeedStones() == 0)
            return 0;
//        LogHelper.Log("GetAvailableEventGachaTimes(), InEventGacha, AccountInfo.Stone / GetEventGachaNeedStones()");
//        return 0;
        return AccountInfo.Stone / GetEventGachaNeedStones();
    }

    
    private void setData(ModelEnum modelType, object modelData) {
        ModelManager.Instance.SetData(modelType, modelData);
    }
    
    private object getData(ModelEnum modelType) {
        ErrorMsg errMsg = new ErrorMsg();
        return ModelManager.Instance.GetData(modelType, errMsg);
    }
	
	public const uint AVATAR_ATLAS_COUNT = 3;
	public const uint AVATAR_ATLAS_CAPACITY = 100;
	private Dictionary<uint, UIAtlas> avatarAtalsDic = new Dictionary<uint, UIAtlas>();
	public UIAtlas GetAvatarAtlas(uint unitID){
		if(avatarAtalsDic.Count == 0){
//			Debug.LogError("DataCenter :: avatarAtalsDic is empty, loading...");
			LoadAvatarAtlas();
		}
		uint index = unitID/AVATAR_ATLAS_CAPACITY;
		if(avatarAtalsDic[ index ] == null){
			Debug.LogError("AvatarAtlas_" + index + " is NOT Found, Please Check it....");
			return null;
		}
		return avatarAtalsDic[ index ];
	}

	private bool LoadAvatarAtlas(){
		bool successful = false;
		for (uint i = 0; i < AVATAR_ATLAS_COUNT; i++){
			string sourcePath = string.Format("Atlas/AvatarAtlas_{0}", i);
			//never use the raw interface! Use ResourceManager instead
			GameObject source = ResourceManager.Instance.LoadLocalAsset(sourcePath) as GameObject;
			//GameObject source = ResourceManager.Instance.LoadLocalAsset(sourcePath) as GameObject;
			UIAtlas atlas = source.GetComponent<UIAtlas>();
			if(atlas == null){ Debug.LogError("LoadAvatarAtlas(), atlas is NULL"); continue; }
			avatarAtalsDic.Add(i, atlas);
		}
		successful = (avatarAtalsDic.Count == AVATAR_ATLAS_COUNT) ? true : false;
//		Debug.Log("DataCenter.LoadAvatarAtlas(), successful is : " + successful);
		return successful;
	}


}