using UnityEngine;
using System.Collections;

public class PartyInfoPanel : UIComponentUnity {

	protected UILabel hpLabel;
	protected UILabel curCostLabel;
	protected UILabel maxCostLabel;

	protected UILabel fireLabel;
	protected UILabel lightLabel;
	protected UILabel darkLabel;
	protected UILabel waterLabel;
	protected UILabel windLabel;
	protected UILabel wuLabel;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		FindUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		SetUIElement();
	}

	public override void HideUI(){
		base.HideUI();
		ResetUIElement();
	}

	void FindUIElement(){

	}

	void SetUIElement(){

	}

	void ResetUIElement(){
		
	}

}
