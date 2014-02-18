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
		materialUnitInfoList = ConfigOwnedUnitInfo.ownedUnitInfo;
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
//		dragPanel.AddItem( ConfigOwnedUnitInfo.ownedUnitInfo.Count );

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject target = dragPanel.ScrollItem [i];

			UITexture tex = target.GetComponentInChildren<UITexture>();
			UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [GlobalData.HaveCard [i]];

			tex.mainTexture = Resources.Load(ubi.GetHeadPath) as Texture2D;
//			string texPath = "Avatar/role0" + materialUnitInfoList[ i ].id.ToString();
//			Debug.Log(texPath);
//			tex.mainTexture = Resources.Load( texPath ) as Texture2D;

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

		UnitInfo tempUnitInfo = new UnitInfo();

		tempUnitInfo.id = 11;
		tempUnitInfo.name = "Zeus";
		tempUnitInfo.type = EUnitType.UFIRE;
//		tempUnitInfo.
		tempUnitInfo.skill1 = 1;
		tempUnitInfo.skill2 = 2;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 8;
		tempUnitInfo.levelUpValue = 329;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 12;
		tempUnitInfo.name = "Darchro";
		tempUnitInfo.type = EUnitType.UDARK;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 18;
		tempUnitInfo.levelUpValue = 117;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 13;
		tempUnitInfo.name = "Boush";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 14;
		tempUnitInfo.name = "Azwraith";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 15;
		tempUnitInfo.name = "Rigwarl";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 16;
		tempUnitInfo.name = "Jah'rakal";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 17;
		tempUnitInfo.name = "Yurnero";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 18;
		tempUnitInfo.name = "Nortrom";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 19;
		tempUnitInfo.name = "Ulfsaar";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 20;
		tempUnitInfo.name = "Tiny";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 21;
		tempUnitInfo.name = "Chen";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 22;
		tempUnitInfo.name = "Furion";
		tempUnitInfo.type = EUnitType.ULIGHT;
		//tempUnitInfo.
		tempUnitInfo.skill1 = 3;
		tempUnitInfo.skill2 = 4;
		tempUnitInfo.leaderSkill = 3;
		tempUnitInfo.activeSkill = 4;
		tempUnitInfo.passiveSkill = 5;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.rare = 3;
		tempUnitInfo.expType = 1;
		tempUnitInfo.cost = 3;
		tempUnitInfo.levelUpValue = 92;
		ownedUnitInfo.Add(tempUnitInfo);

		Debug.Log("Config OwnedUnitInfo, List Count : " + ownedUnitInfo.Count);
	}
}



