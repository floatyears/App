using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListView : UIComponentUnity {
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private List<TUserUnit> myUnitDataList = new List<TUserUnit>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUIElement();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		AddCmdListener();
		CreateDragPanel();
		SortUnitByCurRule();
		RefreshItemCounter();
		ShowUIAnimation();
	}
	
	public override void HideUI (){
		base.HideUI ();
		dragPanel.DestoryUI();
		RmvCmdListener();
	}

	private void InitUIElement(){
		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.MyUnitListView);//DEFAULT_SORT_RULE;
	}

	private void CreateDragPanel(){
		myUnitDataList = GetUnitList();
		dragPanel = new DragPanel("DragPanel", MyUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(myUnitDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.UnitListDragPanelArgs, transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitItem.Inject(dragPanel.ScrollItem[ i ]).Init(myUnitDataList[ i ]);
		}
	}

	private void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -473 , 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private List<TUserUnit> GetUnitList(){
		List<TUserUnit> myUnitList = DataCenter.Instance.UserUnitList.GetAllMyUnit ();
		if(myUnitList == null){
			return null;
		}
		return myUnitList;
	}

	private void SortUnitByCurRule(){
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.MyUnitListView);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitItem muv = dragPanel.ScrollItem[ i ].GetComponent<MyUnitItem>();
			muv.UserUnit = myUnitDataList[ i ];
			muv.CurrentSortRule = curSortRule;
		}
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
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
