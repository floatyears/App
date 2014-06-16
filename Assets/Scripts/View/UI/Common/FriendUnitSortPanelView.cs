using UnityEngine;
using System.Collections;

public class FriendUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
		base.InitBtns();
		UIButton btn;
		UILabel label;
		btn = FindChild<UIButton>("RulePanel/Button_Sort_Rank");
		label = btn.GetComponentInChildren<UILabel>();
		label.text = TextCenter.GetText("Btn_SortRule_Rank");
		sortRuleSelectDic.Add(btn, SortRule.Rank);
                
		AddEventListener();
	}
}
