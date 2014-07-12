using UnityEngine;
using System.Collections;

public class UserUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
//		Debug.LogError("UserUnitSortPanelView init btns 1");
		base.InitBtns();
		UIButton btn;
		UILabel label;
		btn = FindChild<UIButton>("RulePanel/Button_Sort_GetTime");
		label = btn.GetComponentInChildren<UILabel>();
		label.text = TextCenter.GetText("Btn_SortRule_GetTime");
		sortRuleSelectDic.Add(btn, SortRule.GetTime);

		AddEventListener();

//		Debug.LogError("UserUnitSortPanelView init btns 2");
	}

}
