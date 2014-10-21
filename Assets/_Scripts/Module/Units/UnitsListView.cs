using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitsListView : ViewBase {
	private UILabel volumeInfo;

//	private GameObject OkBtn;
	
	private int currentContentIndex;

	private SortRule curSortRule;
	private DragPanel dragPanel;

	private Transform content;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.MyUnitListView);//DEFAULT_SORT_RULE;
		volumeInfo = FindChild<UILabel> ("VolumeInfo");

//		OkBtn = FindChild ("OkBtn");
//		UIEventListenerCustom.Get (OkBtn).onClick = ClickOK;

//		FindChild<UILabel>("Title").text = TextCenter.GetText("Btn_JumpScene_UnitList");
		content = FindChild<Transform>("Content");

		FindChild<UILabel> ("1/Label").text = TextCenter.GetText ("UnitListTab1");
		FindChild<UILabel> ("2/Label").text = TextCenter.GetText ("UnitListTab2");

		dragPanel = new DragPanel("MyUnitsListDragPanel","Prefabs/UI/Units/UnitsListCardItem",typeof(UnitsListCardItemView), content);
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","unit_list");

		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);

		SortUnitByCurRule();

		volumeInfo.text = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ().Count + "/" + DataCenter.Instance.UserData.UserInfo.unitMax;
	}
	
	public override void HideUI (){
		base.HideUI ();

		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);

		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);

	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f));
		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){

		List<UserUnit> myUnitDataList = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.MyUnitListView);

		dragPanel.SetData<UserUnit> (myUnitDataList);
	}


	void ClickOK(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.UnitsListModule);
	}


	/// <summary>
	/// Shows the tab info. this function is used in UnitListUI's prefab.
	/// </summary>
	/// <param name="data">Data.</param>
	public void ShowTabInfo(object data){
		UIToggle toggle = UIToggle.GetActiveToggle (4);
		
		if (toggle != null) {
			int i;
			int.TryParse(UIToggle.GetActiveToggle (4).name,out i);
			if(i == 2){
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FunctionNotOpenTitle"),TextCenter.GetText("FunctionNotOpenContent"),TextCenter.GetText("OK"));
				transform.FindChild("1").SendMessage("OnClick");
				return;
			}
			if (currentContentIndex != i) {
				currentContentIndex = i;
				SortUnitByCurRule();
			}
		}
	}
	
}
