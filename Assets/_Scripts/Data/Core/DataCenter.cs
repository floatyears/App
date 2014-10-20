using UnityEngine;

using bbproto;
using LitJson;
using System.Collections.Generic;

public enum ModelEnum {
	ViewData,

	//new add

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
		userData = new UserDataModel ();
		unitData = new UnitDataModel ();
		questData = new QuestDataModel ();
		commonData = new CommonDataModel ();
		friendData = new FriendDataModel ();
		battleData = new BattleDataModel ();
		//      ConfigUnitInfo cui = new ConfigUnitInfo();
		//		Debug.LogWarning ("InitData ConfigSkill");
//		supportFriendManager = new SupportFriendDataModel ();
		
		ResourceManager.Instance.LoadLocalAsset (PathConfig.UIInsConfigPath, o => {
			new UIConfigData ((o as TextAsset).text);

		});
		ResourceManager.Instance.LoadLocalAsset(PathConfig.DragPanelConfigPath,o=>{
			new DragPanelData((o as TextAsset).text);
		});

		ResourceManager.Instance.LoadLocalAsset (PathConfig.TaskConfigPath, o => {
			taskAndAchieveData = new TaskAndAchieveModel();
			taskAndAchieveData.Init((o as TextAsset).text);
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

	

//	private static GameState _gameState = GameState.Normal;
//	public static GameState gameState {
//		set { _gameState = value; BattleConfigData.Instance.gameState = (byte)_gameState; }
//		get { return _gameState; }
//	}

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
			userData = value;
		}
	}

	private UnitDataModel unitData;
	
	public UnitDataModel UnitData{
		get{
			return unitData;
		}
		set{
			unitData = value;
		}
	}

	private QuestDataModel questData;
	public QuestDataModel QuestData{
		get{
			return questData;
		}
		set{
			questData = value;
		}
	}

	private CommonDataModel commonData;
	public CommonDataModel CommonData{
		get{
			return commonData;
		}
		set{
			commonData = value;
		}
	}

//	public SupportFriendDataModel supportFriendManager;

//    public List<FriendInfo> SupportFriends {
//		get { return supportFriendManager.GetSupportFriend(); }
//		set { supportFriendManager.AddSupportFriend(value); }
//    }

	private FriendDataModel friendData;
    public FriendDataModel FriendData { 
		get{
			return friendData;
		}
		set{
			friendData = value;
		}
    }

	private BattleDataModel battleData;
	public BattleDataModel BattleData{
		get{
			return battleData;
		}
		set{
			battleData = value;
		}
	}

	private TaskAndAchieveModel taskAndAchieveData;
	public TaskAndAchieveModel TaskAndAchieveData{
		get{
			return taskAndAchieveData;
		}set{
			taskAndAchieveData = value;
		}
	}

	/// <summary>
	/// store operate befoure account info
	/// </summary>
	public UserInfo oldAccountInfo = null;
   
	
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
    
    // return UserCost of curr Rank.
    public int UserCost {
        get {
            return unitData.GetUnitValue(PowerTable.UserCostMax, userData.UserInfo.rank); 
        }
    }
    
    /// <summary>
    /// Gets the unit value.  1 =  exp. 2 = attack. 3 = hp. 4 = rankCost
    /// </summary>
    /// <returns>The unit value.</returns>
    /// <param name="type">Type.</param>
    /// <param name="level">Level.</param>
    
   

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


	//PS: GetStageInfo() only used for story stage ( event stage cannot use)


    
    public int GetFriendGachaNeedPoints(){
        return DataCenter.friendGachaFriendPoint;
    }
    
    public int GetAvailableFriendGachaTimes(){
        if (GetFriendGachaNeedPoints() == 0)
            return 0;
		return  userData.AccountInfo.friendPoint / GetFriendGachaNeedPoints();
    }

    public int GetRareGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableOneGachaTimes(){
        if (GetRareGachaNeedStones() == 0)
            return 0;
		return  userData.AccountInfo.stone / GetRareGachaNeedStones();
    }

    public int GetEventGachaNeedStones(){
        return DataCenter.rareGachaStone;
    }
    
    public int GetAvailableNineGachaTimes(){
//        if (!InEventGacha)
//            return 0;
        if (GetEventGachaNeedStones() == 0)
            return 0;
		return  userData.AccountInfo.stone / GetRareGachaNeedStones();
    }

	private List<AudioConfigItem> configAudioList;
	public List<AudioConfigItem> ConfigAudioList{
		get{
			return configAudioList;//GetData(ModelEnum.AudioList) as List<AudioConfigItem>;
		}
		set{
			configAudioList = value;
		}
	}

	private List<UserUnit> configViewData;
	public List<UserUnit> ConfigViewData{
		get{
			return configViewData;//GetData(ModelEnum.ViewData) as List<UserUnit>;
		}
		set{
			configViewData = value;
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