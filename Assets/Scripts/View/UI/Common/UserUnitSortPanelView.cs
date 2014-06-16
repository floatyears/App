using UnityEngine;
using System.Collections;

public class UserUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
		base.InitBtns();
		UIButton btn;
		UILabel label;
		btn = FindChild<UIButton>("RulePanel/Button_Sort_GetTime");
		label = btn.GetComponentInChildren<UILabel>();
		label.text = TextCenter.GetText("Btn_SortRule_GetTime");
		sortRuleSelectDic.Add(btn, SortRule.GetTime);

		AddEventListener();
	}

}
