using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortPanelView : UIComponentUnity {
	UIButton closeBtn;
	protected Dictionary<UIButton, SortRule> sortRuleSelectDic = new Dictionary<UIButton, SortRule>() ;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitBtns();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		ActivateSortRuleWindow(false);
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	public override void DestoryUI(){
		base.DestoryUI();
		RmvCmdListener();
	}

	protected virtual void InitBtns(){

		closeBtn = FindChild<UIButton>("Button_Close");
		UIEventListener.Get(closeBtn.gameObject).onClick = CloseSortRulePanel;

		UIButton btn;
		btn = FindChild<UIButton>("Button_Sort_HP");
		sortRuleSelectDic.Add(btn, SortRule.HP);

		btn = FindChild<UIButton>("Button_Sort_Atk");
		sortRuleSelectDic.Add(btn, SortRule.Attack);

		btn = FindChild<UIButton>("Button_Sort_Race");
		sortRuleSelectDic.Add(btn, SortRule.Race);

		btn = FindChild<UIButton>("Button_Sort_Attribute");
		sortRuleSelectDic.Add(btn, SortRule.Attribute);

		btn = FindChild<UIButton>("Button_Sort_ID");
		sortRuleSelectDic.Add(btn, SortRule.ID);

		btn = FindChild<UIButton>("Button_Sort_Fav");
		sortRuleSelectDic.Add(btn, SortRule.Fav);

		btn = FindChild<UIButton>("Button_Sort_AddPoint");
		sortRuleSelectDic.Add(btn, SortRule.AddPoint);
	}

	protected void SelectSortRule(GameObject item){
		//Debug.Log("SortPanelView.SelectSortRule(), click item : " + item.name);
		UIButton btn = item.transform.GetComponent<UIButton>();
		SortRule targetRule = sortRuleSelectDic[ btn ];
		//curSortRuleLabel.text = targetRule.ToString();
		MsgCenter.Instance.Invoke(CommandEnum.SortByRule, targetRule);
		ActivateSortRuleWindow(false);
	}

	protected void ActivateSortRuleWindow(object msg){
		bool isActive = (bool)msg;
		gameObject.SetActive(isActive);
		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.SortWindow, isActive));
	}

	private void OpenSortRulePanel(object msg){
		ActivateSortRuleWindow(true);
	}

	private void CloseSortRulePanel(GameObject btn){
		ActivateSortRuleWindow(false);
		MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.SortWindow, false));
	}

	protected void AddEventListener(){
		foreach (var item in sortRuleSelectDic){
			UIEventListener.Get(item.Key.gameObject).onClick = SelectSortRule;
		}
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.OpenSortRuleWindow, ActivateSortRuleWindow);
	}

	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.OpenSortRuleWindow, ActivateSortRuleWindow);
	}
}
