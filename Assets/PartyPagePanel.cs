using UnityEngine;
using System.Collections;

public class PartyPagePanel : UIComponentUnity {

	UILabel curPartyIndexLabel;
	UILabel partyCountLabel;
	UILabel curPartyBigLabel;
	UIButton leftButton;
	UIButton rightButton;

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	void FindUIElement(){
//		curPartyIndexLabel = FindChild<UILabel>("")
	}

	void SetUIElement(){

	}

	void ResetUIElement(){

	}
}
