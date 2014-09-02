using UnityEngine;
using System.Collections;

public class UnitPartyPage : PartyPageModule{

	public UnitPartyPage(UIConfigItem config) : base(  config){}

	public override void InitUI(){
		base.InitUI();
		EnableIndexDisplay();
	}

	private void EnableIndexDisplay(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableLabelLeft", null);
		view.CallbackView(cbdArgs);
	}



}
