using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListView : UIComponentUnity {
	private SortRule curSortRule;
	private DragPanel dragPanel;
	private UIButton sortBtn;
	private UILabel sortRuleLabel;
	private List<TUserUnit> myUnitDataList = new List<TUserUnit>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUIElement();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		CreateDragPanel();
		SortUnitByCurRule();
		RefreshItemCounter();
		ShowUIAnimation();
	}
	
	public override void HideUI (){
		base.HideUI ();
		dragPanel.DestoryUI();
	}

	private void InitUIElement(){
		sortBtn = transform.FindChild("Button_Sort").GetComponent<UIButton>();
		sortRuleLabel = transform.FindChild("Button_Sort/Label_Rule").GetComponent<UILabel>();
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
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
		transform.localPosition = new Vector3(-1000, 0 , 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}

	private void ClickSortBtn(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		SortUnitByCurRule();
	}

	private List<TUserUnit> GetUnitList(){
		if(DataCenter.Instance.MyUnitList.GetAll() == null){
			return null;
		}
		List<TUserUnit> unitList = new List<TUserUnit>();
		unitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		//Debug.Log("MyUnitListView.GetUnitList(), unitList count is : " + unitList.Count);
		return unitList;
	}

	private void SortUnitByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, myUnitDataList);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitItem muv = dragPanel.ScrollItem[ i ].GetComponent<MyUnitItem>();
			muv.UserUnit = myUnitDataList[ i ];
			muv.CurrentSortRule = curSortRule;
		}
	}

	private void RefreshItemCounter(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.MyUnitList.Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}
}
