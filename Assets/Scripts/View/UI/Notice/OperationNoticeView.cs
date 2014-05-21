using UnityEngine;
using System.Collections;

public class OperationNoticeView : UIComponentUnity {

	public GameObject contentObj;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();
	}
	
	public override void HideUI() {
		base.HideUI();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private void InitUI(){
		//		sortBtn = transform.FindChild("Button_Sort").GetComponent<UIButton>();
		//		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		//sortRuleLabel = transform.FindChild("Label_Sort_Rule").GetComponent<UILabel>();
		
		//curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		//sortRuleLabel.text = curSortRule.ToString();
	}
}
