using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class MyUnitsListView : ViewBase {
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private List<UserUnit> myUnitDataList = new List<UserUnit>();

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitUIElement();
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","unit_list");
		ModuleManager.Instance.ShowModule (ModuleEnum.ItemCounterModule,"from","unit_list");

		AddCmdListener();
		CreateDragPanel();
		SortUnitByCurRule();
		RefreshItemCounter();
		ShowUIAnimation();
	}
	
	public override void HideUI (){
		base.HideUI ();

		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);

		dragPanel.DestoryUI();
		RmvCmdListener();
	}

	private void InitUIElement(){
		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.MyUnitListView);//DEFAULT_SORT_RULE;
	}

	private void CreateDragPanel(){
		myUnitDataList = GetUnitList();
		dragPanel = new DragPanel("MyUnitsListDragPanel","Prefabs/UI/UnitItem/MyUnitPrefab",typeof(MyUnitItem), transform);
//		dragPanel.CreatUI();

	}

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -473 , 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private List<UserUnit> GetUnitList(){
		List<UserUnit> myUnitList = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit ();
		if(myUnitList == null){
			return null;
		}
		return myUnitList;
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.MyUnitListView);

		dragPanel.SetData<UserUnit> (myUnitDataList, curSortRule);
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
		countArgs.Add("posy", -740);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
}
