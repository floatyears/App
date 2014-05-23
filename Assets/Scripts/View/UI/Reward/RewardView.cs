using UnityEngine;
using System.Collections;

public class RewardView : UIComponentUnity {

	private DragPanel ItemList;

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

	}
}
