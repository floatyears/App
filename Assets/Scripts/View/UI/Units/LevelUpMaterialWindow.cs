using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpMaterialWindow : UIComponentUnity {
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();

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
//			UIEventListenerCustom ulc = UIEventListenerCustom.Get(target);
//			ulc.onClick = PickMaterial;
//			ulc.LongPress = LongPressPickMaterial;
//			materialItemInfo.Add(target, ubi);
		}
		InitDragPanelArgs();
		dragPanel.RootObject.SetScrollView(dragPanelArgs);
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
