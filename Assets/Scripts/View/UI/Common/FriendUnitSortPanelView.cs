using UnityEngine;
using System.Collections;

public class FriendUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
		base.InitBtns();
		UIButton btn;
		
		btn = FindChild<UIButton>("RulePanel/Button_Sort_Rank");
		sortRuleSelectDic.Add(btn, SortRule.Rank);
                
		AddEventListener();
	}
}
