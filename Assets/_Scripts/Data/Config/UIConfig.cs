using System;
using UnityEngine;
using LitJson;
using System.Collections.Generic;

public class UIConfig{

	public const string SPR_NAME_BORDER_FIRE = "avatar_border_fire";
	public const string SPR_NAME_BORDER_WATER = "avatar_border_water";
	public const string SPR_NAME_BORDER_WIND = "avatar_border_wind";
	public const string SPR_NAME_BORDER_LIGHT = "avatar_border_light";
	public const string SPR_NAME_BORDER_DARK = "avatar_border_dark";
	public const string SPR_NAME_BORDER_NONE = "avatar_border_none";

	public const string SPR_NAME_BG_FIRE = "avatar_bg_fire";
	public const string SPR_NAME_BG_WATER = "avatar_bg_water";
	public const string SPR_NAME_BG_WIND = "avatar_bg_wind";
	public const string SPR_NAME_BG_LIGHT = "avatar_bg_light";
	public const string SPR_NAME_BG_DARK = "avatar_bg_dark";
	public const string SPR_NAME_BG_NONE = "avatar_bg_none";

	public const string SPR_NAME_BASEBOARD_HELPER = "helper_item";
	public const string SPR_NAME_BASEBOARD_FRIEND = "friend_item";

	public const string SPR_NAME_PAGE_INDEX_PREFIX = "page_index_";

	public static int PartyMaxCount = 5;

	public const float playerInfoBox_X = 160f;
	public const float playerInfoBox_Y = -50f;

	public const float longPressedTimeCount = 0.5f;
	public const int partyTotalCount = 5;

	public const int otherMusicSettingIndex = 1;
}


public class UIConfigData
{

	public UIConfigData(string info)
	{
		JsonData jsonData = JsonMapper.ToObject (info);
		
		Dictionary<ModuleEnum,UIConfigItem> uiInsData = new Dictionary<ModuleEnum, UIConfigItem> ();
		
		UIConfigItem ins;
		
		for (int i = 0; i < jsonData.Count; i++){

			//            Debug.LogError("json config DeserializeData uiName " + (string)jsonData [i] ["uiName"]);
			ins = new UIConfigItem();
			try{
				ins.moduleName = (ModuleEnum)Enum.Parse(typeof(ModuleEnum), jsonData[i] ["name"].ToString());
				Debug.Log("module name: " + ins.moduleName);
			}catch(ArgumentException){
			Debug.LogError("ModuleEnum Convert Err: [[[---"+ jsonData[i] ["name"]+ "---]]] is not a member of the ModuleEnum");
				continue;
			}
			
			ins.resourcePath = (string)jsonData[i]["path"];
			if(jsonData[i] ["x"].IsDouble) {
				double data = (double)jsonData[i]["x"];
				ins.localPosition.x = (float)data;
			} else{
				ins.localPosition.x = (int)jsonData[i] ["x"];
			}
			
			if(jsonData[i]["y"].IsDouble) {
			double data = (double)jsonData[i] ["y"];
				ins.localPosition.y = (float)data;
			} else{
			ins.localPosition.y = (int)jsonData[i]["y"];
			}
			
			if(jsonData[i]["z"].IsDouble) {
			double data = (double)jsonData[i] ["z"];
				ins.localPosition.z = (float)data;
			} else{
			ins.localPosition.z = (int)jsonData[i] ["z"];
			}
			byte parent = (byte)((int)jsonData[i] ["parent"]);
			ins.parent = GetParentTrans(parent);
			ins.group = (ModuleGroup)(int)jsonData[i]["group"];
			uiInsData.Add(ins.moduleName, ins);
			//			Debug.LogError(ins.uiName);
		}
		
		DataCenter.Instance.SetData (ModelEnum.UIInsConfig, uiInsData);
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
			case UIParentEnum.PopUp:
				trans = vm.PopupPanel.transform;
				break;
			default:
				break;
		}

		return trans;
	}
}

public class DragPanelData{

	public DragPanelData(string info){
		JsonData jsonData = JsonMapper.ToObject (info);

		Dictionary<string,DragPanelConfigItem> uiInsData = new Dictionary<string, DragPanelConfigItem> ();

		DragPanelConfigItem ins;

		for (int i = 0; i < jsonData.Count; i++){

			ins = new DragPanelConfigItem ();

			ins.configName = (string)jsonData[i]["configName"];
			ins.cellHeight = (int)jsonData[i]["cellHeight"];
			ins.cellWidth = (int)jsonData[i]["cellWidth"];

			JsonData clip = jsonData[i]["clipRange"];
			ins.clipRange = new Vector4((int)clip[0],(int)clip[1],(int)clip[2],(int)clip[3]);
			ins.depth = (int)jsonData[i]["depth"];
			ins.gridArrage = (UIGrid.Arrangement)Enum.Parse(typeof(UIGrid.Arrangement),jsonData[i]["gridArrange"].ToString());
			ins.maxPerLine = (int)jsonData[i]["maxPerLine"];

			JsonData pos = jsonData[i]["position"];
			ins.position = new Vector3((int)pos[0],(int)pos[1],(int)pos[2]);

			JsonData scrollpos = jsonData[i]["scrollBarPosition"];
			ins.scrollBarPosition = new Vector3((int)scrollpos[0],(int)scrollpos[1],(int)scrollpos[2]);

			JsonData scrolllocal = jsonData[i]["scrollerLocalPos"];
			ins.scrollerLocalPos = new Vector3((int)scrolllocal[0],(int)scrolllocal[1],(int)scrolllocal[2]);
			ins.scrollMovement = (UIScrollView.Movement)Enum.Parse(typeof(UIScrollView.Movement),jsonData[i]["scrollMovement"].ToString());

			uiInsData.Add(ins.configName,ins);
		}

		DataCenter.Instance.SetData (ModelEnum.DragPanelConfig, uiInsData);
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

		return data;
	}

	public string GetClassName (int id) {
		string name = string.Empty;
		string key = id.ToString ();
		data.TryGetValue (key, out name);
		return name;
	}
}

public class UIConfigItem
{
	public ModuleEnum moduleName = ModuleEnum.None;
	public string resourcePath = string.Empty;
	public Transform parent = null;
	public Vector3 localPosition = Vector3.zero;
	public ModuleGroup group = ModuleGroup.DEFAULT;
}

public class DragPanelConfigItem{
	public string configName;
	public Vector3 scrollerLocalPos;
	public Vector3 position;
	public Vector4 clipRange;
	public UIGrid.Arrangement gridArrage;
	public UIScrollView.Movement scrollMovement;
	public Vector3 scrollBarPosition;
	public int cellWidth;
	public int cellHeight;
	public int maxPerLine;
	public int depth;
}


