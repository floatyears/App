using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpMaterialWindow : UIComponentUnity {
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private Dictionary<GameObject, UnitBaseInfo> materialItemInfo = new Dictionary<GameObject, UnitBaseInfo>();
	List<UnitInfo> materialUnitInfoList = new List<UnitInfo>();



	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		//this.gameObject.SetActive(false);
		MsgCenter.Instance.AddListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}
	
	public override void HideUI() {
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUpPanelFocus, FocusOnPanel);
	}
	
	void InitUI(){
		CreateDragPanel();
	}
	
	void FocusOnPanel(object data) {
		string message = (string)data;
		if(message == "Tab_Material" ){
			this.gameObject.SetActive(true);
		}
		else if(message == "Tab_Base"){
			this.gameObject.SetActive(true);
		}
		else{
			this.gameObject.SetActive(false);
		}
	}

	void CreateDragPanel() {
		GameObject materialItem = 
			Resources.Load("Prefabs/UI/Friend/UnitItem") as GameObject;
		dragPanel = new DragPanel("MaterialScroller", materialItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(GlobalData.HaveCard.Count);
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject target = dragPanel.ScrollItem [i];
			UITexture tex = target.GetComponentInChildren<UITexture>();
			UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [GlobalData.HaveCard [i]];
			tex.mainTexture = Resources.Load(ubi.GetHeadPath) as Texture2D;
			UIEventListenerCustom ulc = UIEventListenerCustom.Get(target);
			ulc.onClick = PickMaterial;
			ulc.LongPress = LongPressPickMaterial;
			materialItemInfo.Add(target, ubi);
		}
		InitDragPanelArgs();
		dragPanel.RootObject.SetScrollView(dragPanelArgs);
	}

	void LongPressPickMaterial(GameObject go){
		UnitBaseInfo ubi = materialItemInfo [go];
		MsgCenter.Instance.Invoke(CommandEnum.EnterUnitInfo, ubi);
	}
	
	void PickMaterial(GameObject go){
		//Debug.LogError("Pick Material");
		int index = dragPanel.ScrollItem.IndexOf( go );
		UnitInfo pickedUnitInfo = materialUnitInfoList[ index ];
		MsgCenter.Instance.Invoke( CommandEnum.TransmitMaterialUnitInfo, pickedUnitInfo );
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",	transform);
		dragPanelArgs.Add("scrollerScale",	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-45 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",		3);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -340, 0));
		dragPanelArgs.Add("cellWidth", 		120);
		dragPanelArgs.Add("cellHeight",		120);
	}
}

public class ConfigOwnedUnitInfo{
	public static List<UnitInfo> ownedUnitInfo = new List<UnitInfo>();
	public ConfigOwnedUnitInfo(){
		Config();
	}
	
	void Config(){
		//Debug.Log("Start to Config the data of stage");

//		UnitInfo tempUnitInfo = new UnitInfo();
//
//		tempUnitInfo.id = 1;
//		tempUnitInfo.name = "Zeus";
//		tempUnitInfo.type = EUnitType.UFIRE;
//		tempUnitInfo.race = 0;
//		tempUnitInfo.skill1 = 1;
//		tempUnitInfo.skill2 = 2;
//		tempUnitInfo.leaderSkill = 3;
//		tempUnitInfo.activeSkill = 4;
//		tempUnitInfo.passiveSkill = 5;
//		tempUnitInfo.maxLevel = 50;
//		tempUnitInfo.rare = 3;
//		tempUnitInfo.expType = 1;
//		tempUnitInfo.cost = 8;
//		tempUnitInfo.levelUpValue = 329;
//
//		//ownedUnitInfo.Add();
	


	}
}



