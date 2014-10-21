using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSortView : ViewBase {
	private UIButton sortBtn;
	private UILabel sortBtnLabel;
	private UISprite sortBtnMask;
	private bool isShow = false;
	private GameObject sortRuleSelectPanel;
	SortRule targetRule = SortRule.None;
	protected Dictionary<UIButton, SortRule> sortRuleSelectDic = new Dictionary<UIButton, SortRule>() ;

	SortRuleByUI srui;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitBtns();
	}

	public override void ShowUI(){
		base.ShowUI();

		if (viewData != null && viewData.ContainsKey("from")) {
			switch((string)viewData["from"]){
			case "unit_list":
				srui = SortRuleByUI.MyUnitListView;
				break;
			case "level_up":
				srui = SortRuleByUI.UnitLevelupAndEvolveView;
				break;
			case "party":
				srui = SortRuleByUI.PartyView;
				break;
			case "sell_unit":
				srui = SortRuleByUI.SellView;
				break;
			}
		}
	}

	public override void DestoryUI () {
		AddEventListener (false);
		base.DestoryUI ();

	}

	void HideSortView(object data) {
		bool enable = (bool)data;
		gameObject.SetActive (enable);
	}

	protected virtual void InitBtns(){
		sortRuleSelectPanel = transform.FindChild("RulePanel").gameObject;
		sortBtn = FindChild<UIButton>("Button_Sort");
		sortBtnLabel = FindChild<UILabel>("Button_Sort/Label");
		sortBtnLabel.text = TextCenter.GetText("Btn_SORT");
		sortBtnMask = FindChild<UISprite>("Button_Sort/Mask");
		UIEventListenerCustom.Get(sortBtn.gameObject).onClick = ClickSortBtn;

		UIButton btn;
		UILabel label;
		btn = FindChild<UIButton>("RulePanel/Button_Sort_HP");
		label = FindChild<UILabel>("RulePanel/Button_Sort_HP/Label");
		label.text = TextCenter.GetText("Btn_SortRule_HP");
		sortRuleSelectDic.Add(btn, SortRule.HP);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Atk");
		label = FindChild<UILabel>("RulePanel/Button_Sort_Atk/Label");
		label.text = TextCenter.GetText("Btn_SortRule_Atk");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Race");
		label = FindChild<UILabel>("RulePanel/Button_Sort_Race/Label");
		label.text = TextCenter.GetText("Btn_SortRule_Race");
		sortRuleSelectDic.Add(btn, SortRule.Race);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Attribute");
		label = FindChild<UILabel>("RulePanel/Button_Sort_Attribute/Label");
		label.text = TextCenter.GetText("Btn_SortRule_Attribute");
		sortRuleSelectDic.Add(btn, SortRule.Attribute);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_ID");
		label = FindChild<UILabel>("RulePanel/Button_Sort_ID/Label");
		label.text = TextCenter.GetText("Btn_SortRule_ID");
		sortRuleSelectDic.Add(btn, SortRule.ID);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Fav");
		label = FindChild<UILabel>("RulePanel/Button_Sort_Fav/Label");
		label.text = TextCenter.GetText("Btn_SortRule_Fav");
		sortRuleSelectDic.Add(btn, SortRule.Fav);

//		btn = FindChild<UIButton>("RulePanel/Button_Sort_AddPoint");
//		label = btn.GetComponentInChildren<UILabel>();
//		label.text = TextCenter.GetText("Btn_SortRule_AddPoint");
//		sortRuleSelectDic.Add(btn, SortRule.AddPoint);
		AddEventListener (true);
	}

	private void ClickSortBtn(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		ActivateSortRuleWindow();
	}

	protected void SelectSortRule(GameObject item){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );

		//Debug.Log("SortPanelView.SelectSortRule(), click item : " + item.name);
		UIButton btn = item.transform.GetComponent<UIButton>();
		targetRule = sortRuleSelectDic[ btn ];
		MsgCenter.Instance.Invoke(CommandEnum.SortByRule, targetRule);
		ActivateSortRuleWindow();
	}

	protected void ActivateSortRuleWindow(){
		if (sortRuleSelectPanel.GetComponent<iTween> () != null) {
			Debug.Log("the object is tweening!");
			return;	
		}

		SortRule sr = SortUnitTool.GetSortRule (srui);
		foreach (var item in sortRuleSelectDic) {
			if(item.Value == sr){
				item.Key.defaultColor = new Color(0.88f,0.78f,0.59f,1.0f);
			}else {
				item.Key.defaultColor = Color.white;
			}
		}

		sortBtnMask.enabled = true;
//		sortBtn.isEnabled = false;

		if(isShow){
			iTween.MoveTo(sortRuleSelectPanel, iTween.Hash("x", 0, "time", 0.4f, "islocal", true, "oncomplete",  "ActivateSortBtn", "oncompletetarget", gameObject));
			isShow = false;
			//Debug.Log("isShow : " + isShow);
		}
		else{
			iTween.MoveTo(sortRuleSelectPanel, iTween.Hash("x", -215, "time", 0.4f, "islocal", true, "oncomplete", "ActivateSortBtn", "oncompletetarget", gameObject));
			isShow = true;
			//Debug.Log("isShow : " + isShow);
		}	
//		ModuleManager.SendMessage(ModuleEnum.MaskModule, "block", new BlockerMaskParams(BlockerReason.SortWindow, isShow));
		InputManager.Instance.SetBlockWithinModule (ModuleEnum.UnitSortModule, isShow);
	}
	
	public void ActivateSortBtn(){
		sortBtnMask.enabled = false;
		sortBtn.isEnabled = true;
	}

	protected void AddEventListener(bool isAdd){
		foreach (var item in sortRuleSelectDic){
			if(isAdd)
				UIEventListenerCustom.Get(item.Key.gameObject).onClick += SelectSortRule;
			else
				UIEventListenerCustom.Get(item.Key.gameObject).onClick -= SelectSortRule;
		}
	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);
			Debug.Log("sort ui pos: " + transform.localPosition);
//			transform.localPosition = new Vector3(1000, -567, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		}else{
//			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}
}
