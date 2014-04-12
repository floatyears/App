using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListView : UIComponentUnity {
	private GameObject dragItem;
	private DragPanel dragPanel;
	private UIButton sortBtn;
	private UILabel sortRuleLabel;
	private SortRule curSortRule;
	private List<TUserUnit> memberList = new List<TUserUnit>();
	private Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUIElement();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUIAnimation();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs callArgs = data as CallBackDispatcherArgs;
		switch (callArgs.funcName){
			case "CreateDragPanelView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragPanel, callArgs);
				break;
			case "DestoryDragPanelView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragPanel, callArgs);
				break;
			default:
				break;
		}
	}

	void InitUIElement(){
		InitDragPanelArgs();

		memberList = GetUnitList();

		sortBtn = transform.FindChild("Button_Sort").GetComponent<UIButton>();
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		sortRuleLabel = transform.FindChild("Button_Sort/Label_Rule").GetComponent<UILabel>();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		sortRuleLabel.text = curSortRule.ToString();
	}

	void CreateDragPanel(object args){
		List<TUserUnit> data = args as List<TUserUnit>;
		dragPanel = new DragPanel("DragPanel", MyUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(data.Count);
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitView.Inject(dragPanel.ScrollItem[ i ]).Init(data[ i ]);
		}

	}

	void DestoryDragPanel(object args){
		dragPanel.DestoryUI();
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", transform);
		dragPanelArgs.Add("scrollerScale", Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos", 220 * Vector3.up);
		dragPanelArgs.Add("position", Vector3.zero);
		dragPanelArgs.Add("clipRange", new Vector4(0, -210, 640, 600));
		dragPanelArgs.Add("gridArrange", UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine", 4);
		dragPanelArgs.Add("scrollBarPosition", new Vector3(-320, -540, 0));
		dragPanelArgs.Add("cellWidth", 140);
		dragPanelArgs.Add("cellHeight", 140);
	}

	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, 0 , 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}

	private void ClickSortBtn(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, memberList);
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitView puv = dragPanel.ScrollItem[ i ].GetComponent<MyUnitView>();
			puv.UserUnit = memberList[ i ];//before
			puv.CurrentSortRule = curSortRule;//after
		}
	}

	private List<TUserUnit> GetUnitList(){
		if(DataCenter.Instance.MyUnitList.GetAll() == null){
			return null;
		}
		
		List<TUserUnit> unitList = new List<TUserUnit>();
		unitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		//Debug.LogError("GetUnitList(), unitList count : " + unitList.Count);
		return unitList;
	}
	
}
