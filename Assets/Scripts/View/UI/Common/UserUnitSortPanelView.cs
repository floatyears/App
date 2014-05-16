using UnityEngine;
using System.Collections;

public class UserUnitSortPanelView : SortPanelView {
	protected override void InitBtns(){
		base.InitBtns();
		UIButton btn;

		btn = FindChild<UIButton>("Button_Sort_GetTime");
		sortRuleSelectDic.Add(btn, SortRule.GetTime);

		AddEventListener();

//		this.gameObject.SetActive(false);
	}

}
