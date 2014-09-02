using UnityEngine;
using System.Collections;

public class FriendUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
//		Debug.LogError("FriendUnitSortPanelView InitBtns 1");
		base.InitBtns();
//		Debug.LogError("FriendUnitSortPanelView InitBtns 2");
		UIButton btn;
//		Debug.LogError("FriendUnitSortPanelView InitBtns 3");
		UILabel label;
//		Debug.LogError("FriendUnitSortPanelView InitBtns 4");
		btn = FindChild<UIButton>("RulePanel/Button_Sort_Rank");
//		Debug.LogError("FriendUnitSortPanelView InitBtns 5");
		label = btn.GetComponentInChildren<UILabel>();
//		Debug.LogError("FriendUnitSortPanelView InitBtns 6");
		label.text = TextCenter.GetText("Btn_SortRule_Rank");
//		Debug.LogError("FriendUnitSortPanelView InitBtns 7");
		sortRuleSelectDic.Add(btn, SortRule.Rank);
//		Debug.LogError("FriendUnitSortPanelView InitBtns 8");
		AddEventListener();
	}
}
