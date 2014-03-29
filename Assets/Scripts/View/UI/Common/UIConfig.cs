using System;
using UnityEngine;
using LitJson;
using System.Collections.Generic;

public class UIConfig
{
	public static int PartyMaxCount = 5;
	public static string TextNickNameInputDefault = "change name here";
	public static float uisliderSpeed = 0.2f;
	public static string emptyLabelTextFormat = "---";
	public static string stageDragPanelItemPath = "Stage/StageDragPanelItem";
	public static string questDragPanelItemPath = "Prefabs/UI/Quest/QuestItem";
	public static string PartyUnitViewItemPath = "Prefabs/UI/Friend/UnitItem";

	public static string Lab_T_Rank = "Rank:";
	public static string Lab_V_Rank = "12";
	public static string Lab_V_PlayerName = "Orca Chen";
	public static string Lab_V_Money = "12345";

	public static string Lab_T_NextExp = "Next Exp:";
	public static string Lab_T_CurTotalExp = "Total Exp:";
	
	public static string Lab_V_NextExp = "5555";
	public static string Lab_V_CurTotalExp = "8888";

	public static string Lab_V_CurStamina = "24";
	public static string Lab_T_Line = "/";
	public static string Lab_V_TotalStamina = "48";

	public static string Lab_V_ChipNum = "1";

	public static string Lab_V_UIName = "Test";

	public static string Lab_V_TotalEnegy = "24";
	public static string Lab_V_CurEnegy = "12";

	public static string Lab_V_ScrollViewTitleInfo = "This is the Story Scene";

	public static string DragUIObjectPath = "Prefabs/Scroller";
	public const string sharePath = "UI/Share/";
	public const string questPath = "UI/Quest/";
	public const string friendPath = "UI/Friend/";
	public const string scratchPath = "UI/Scratch/";
	public const string shopPath = "UI/Shop/";
	public const string othersPath = "UI/Others/";
	public const string unitPath = "UI/Units/";

	public const string UIInsConfigPath = "Config/UIInsConfig";
	public const string menuBackgroundName = "MenuBg";
	public const string topBackgroundName = "PlayerInfoBar";
	public const string menuBottomName = "MenuBottom";
	public const string sceneInfoBarName = "SceneInfoBar";
	public const string TipsBarName = "TipsBar";

	public const string questWindowName = "QuestWindow";
    public const string loadingWindowName = "Loading";
	public const string friendWindowName = "FriendWindow";
	public const string scratchWindowName = "ScratchWindow";
    public const string gachaWindowName = "GachaWindow";
	public const string shopWindowName = "ShopWindow";
	public const string othersWindowName = "OthersWindow";
	public const string unitsWindowName = "UnitsWindow";

	public const string partyDragPanelName = "PartyDragPanel";
	public const string partyInfoPanelName = "PartyInfoPanel";
	public const string partyPagePanelName = "PartyPagePanel";

	public const string partyWindowName = "PartyWindow";
	public const string catalogWindowName = "CatalogWindow";
	public const string levelUpWindowName = "LevelUpWindow";
	public const string sellWindowName = "SellWindow";
	public const string unitListWindowName = "UnitListWindow";
	public const string evolveWindowName = "EvolveWindow";
	public const string unitDisplay = "UnitDisplay";
	public const string evolveFriend = "EvolveFriend";
	public const string questSelectWindowName = "QuestSelectWindow";
	public const string friendSelectWindowName = "FriendSelectWindow";
	public const string applyWindowName = "ApplyWindow";
	public const string friendListWindowName = "FriendListWindow";

	public const string userIDWindowName = "UserIDWindow";
	public const string informationWindowName = "InformationWindow";
	public const string unitDetailPanelName = "UnitDetailPanel";
	public const string searchMainWindowName = "SearchMainWindow";
	public const string searchInfoWindowName = "SearchInfoWindow";

	public const string levelUpInfoPanelName = "LevelUpInfoPanel";
	public const string levelUpReadyPanelName = "LevelUpReadyPanel";
	public const string levelUpMaterialWindowName = "LevelUpMaterialWindow";
	public const string levelUpFriendWindowName = "LevelUpFriendWindow";
	public const string levelUpBasePanelName = "LevelUpBasePanel";
	public const string commonNoteWindowName = "CommonNoteWindow";
	public const string unitBriefInfoWindowName = "UnitBriefInfoWindow";
	public const string userBriefInfoWindowName = "UserBriefInfoWindow";
	public const string applyMessageWindowName = "ApplyMessageWindow";
	public const string acceptApplyMessageWindowName = "AcceptApplyMessageWindow";
	public const string selectRoleWindowName = "SelectRoleWindow";
	public const string screenMaskName = "ScreenMask";
	public const string itemCounterBarName = "ItemCounterBar";

	public const float playerInfoBox_X = 160f;
	public const float playerInfoBox_Y = -50f;

	public const float longPressedTimeCount = 0.5f;
	public const int partyTotalCount = 5;

	public const int otherMusicSettingIndex = 1;
	public const string otherMusicSettingName = "Music";

	

}


public class UIIns : JsonOriginData
{

	private Dictionary<string,UIInsConfig> uiInsData = new Dictionary<string, UIInsConfig>(); 

	public UIIns(string info) :base(info)
	{
		//init data and fill the dicitionay
		DeserializeData();

		// release Useless memory
		jsonData = null;
		info = null;
	}

	public UIInsConfig GetData(string uiName)
	{
		UIInsConfig ins = null;

		if (uiInsData.TryGetValue(uiName, out ins))
		{
			return ins;
		}

		return ins;
	}

	public override object DeserializeData()
	{
		base.DeserializeData();

		UIInsConfig ins;

		for (int i = 0; i < jsonData.Count; i++)
		{
//            Debug.LogError("json config DeserializeData uiName " + (string)jsonData [i] ["uiName"]);
			ins = new UIInsConfig();
			ins.uiName = (string)jsonData [i] ["uiName"];
			ins.resourcePath = (string)jsonData [i] ["resoucePath"] + ins.uiName;
			ins.localPosition.x = (int)jsonData [i] ["positionx"];
			ins.localPosition.y = (int)jsonData [i] ["positiony"];
			ins.localPosition.z = (int)jsonData [i] ["positionz"];
			byte parent = (byte)((int)jsonData [i] ["parent"]);
			ins.parent = GetParentTrans(parent);
			uiInsData.Add(ins.uiName, ins);
//			Debug.LogError(ins.uiName);
		}

		return uiInsData;
	}

	public override ErrorMsg SerializeData(object instance)
	{
		return base.SerializeData(instance);
	}

	Transform GetParentTrans(byte parentEnum)
	{
		ViewManager vm = ViewManager.Instance;

		UIParentEnum uipe = (UIParentEnum)parentEnum;
		Transform trans = null;
		switch (uipe)
		{
			case UIParentEnum.Bottom:
				trans = vm.BottomPanel.transform;
				break;
			case UIParentEnum.Center:
				trans = vm.CenterPanel.transform;
				break;
			case UIParentEnum.Top:
				trans = vm.TopPanel.transform;
				break;
			case UIParentEnum.BottomNoPanel:
				trans = vm.ParentPanel.transform;
				break;
			default:
				break;
		}

		return trans;
	}
}

public class SkillJsonConfig : JsonOriginData {
	public Dictionary<string,string> data = new Dictionary<string, string> ();
	public SkillJsonConfig(string info) : base (info) {
		DeserializeData();

		jsonData = null;
		info = null;
	}

	public override object DeserializeData () {
		data = JsonMapper.ToObject< Dictionary<string,string> > (originData);
//		foreach (var item in data) {
//			Debug.LogError(item.Key + "  " + item.Value);
//		}
		return data;
	}

	public string GetClassName (int id) {
		string name = string.Empty;
		string key = id.ToString ();
		data.TryGetValue (key, out name);
		return name;
	}
}

public class UIInsConfig
{
	public string uiName = string.Empty;
	public string resourcePath = string.Empty;
	public Transform parent = null;
	public Vector3 localPosition = Vector3.zero;
}


