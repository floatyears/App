using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortPanelView : UIComponentUnity {
	private UIButton sortBtn;
	private UISprite sortBtnMask;
	private bool isShow = false;
	private GameObject sortRuleSelectPanel;

	protected Dictionary<UIButton, SortRule> sortRuleSelectDic = new Dictionary<UIButton, SortRule>() ;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitBtns();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	protected virtual void InitBtns(){
		sortRuleSelectPanel = transform.FindChild("RulePanel").gameObject;
		sortBtn = FindChild<UIButton>("Button_Sort");
		sortBtnMask = FindChild<UISprite>("Button_Sort/Mask");
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;

		UIButton btn;
		btn = FindChild<UIButton>("RulePanel/Button_Sort_HP");
		sortRuleSelectDic.Add(btn, SortRule.HP);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Atk");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Race");
		sortRuleSelectDic.Add(btn, SortRule.Race);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Attribute");
		sortRuleSelectDic.Add(btn, SortRule.Attribute);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_ID");
		sortRuleSelectDic.Add(btn, SortRule.ID);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_Fav");
		sortRuleSelectDic.Add(btn, SortRule.Fav);

		btn = FindChild<UIButton>("RulePanel/Button_Sort_AddPoint");
		sortRuleSelectDic.Add(btn, SortRule.AddPoint);
	}

	private void ClickSortBtn(GameObject btn){
		ActivateSortRuleWindow();
	}

	protected void SelectSortRule(GameObject item){
		//Debug.Log("SortPanelView.SelectSortRule(), click item : " + item.name);
		UIButton btn = item.transform.GetComponent<UIButton>();
		SortRule targetRule = sortRuleSelectDic[ btn ];
		MsgCenter.Instance.Invoke(CommandEnum.SortByRule, targetRule);
		ActivateSortRuleWindow();
	}

	protected void ActivateSortRuleWindow(){
		sortBtnMask.enabled = true;
		sortBtn.isEnabled = false;
		if(sortBtn.gameObject.layer ==  0){
			sortBtn.gameObject.layer =  GameLayer.blocker;
		}
		else if(sortBtn.gameObject.layer == GameLayer.blocker){
			sortBtn.gameObject.layer = 0;
		}

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
		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.SortWindow, isShow));
	}

	private void OpenSortRulePanel(object msg){
		ActivateSortRuleWindow();
	}

	public void ActivateSortBtn(){
		sortBtnMask.enabled = false;
		sortBtn.isEnabled = true;
	}

	protected void AddEventListener(){
		foreach (var item in sortRuleSelectDic){
			UIEventListener.Get(item.Key.gameObject).onClick = SelectSortRule;
		}
	}
}
