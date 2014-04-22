using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortPanelView : UIComponentUnity {
	UIButton openBtn;
	UIButton closeBtn;
	protected Dictionary<UIButton, SortRule> sortRuleSelectDic = new Dictionary<UIButton, SortRule>() ;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitBtns();
	}

	public override void ShowUI(){
		base.ShowUI();
		ActivateSortRuleWindow(false);
	}

	protected virtual void InitBtns(){
		openBtn = FindChild<UIButton>("Button_Open");
		UIEventListener.Get(openBtn.gameObject).onClick = OpenSortRulePanel;

		closeBtn = FindChild<UIButton>("Button_Close");
		UIEventListener.Get(closeBtn.gameObject).onClick = CloseSortRulePanel;

		UIButton btn;
		btn = FindChild<UIButton>("Button_Sort_HP");
		sortRuleSelectDic.Add(btn, SortRule.HP);

		btn = FindChild<UIButton>("Button_Sort_ATK");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_Sort_Race");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_Sort_Attribute");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_Sort_ID");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_Sort_Fav");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_AddPoint");
		sortRuleSelectDic.Add(btn, SortRule.Attack);
	}

	protected void SelectSortRule(GameObject item){
		UIButton btn = item.transform.GetComponent<UIButton>();
		SortRule targetRule = sortRuleSelectDic[ btn ];
		MsgCenter.Instance.Invoke(CommandEnum.SortByRule, targetRule);
		ActivateSortRuleWindow(false);
	}

	protected void ActivateSortRuleWindow(bool isActive){
		gameObject.SetActive(isActive);
	}

	private void OpenSortRulePanel(GameObject btn){
		ActivateSortRuleWindow(true);
	}

	private void CloseSortRulePanel(GameObject btn){
		ActivateSortRuleWindow(false);
	}

	protected void AddEventListener(){
		foreach (var item in sortRuleSelectDic){
			UIEventListener.Get(item.Key.gameObject).onClick = SelectSortRule;
		}
	}
}
