using UnityEngine;
using System.Collections;

public class PartyPartyPage : PartyPageLogic{

	public PartyPartyPage(string uiName) : base(uiName){}

	public override void CreatUI(){
		base.CreatUI();
		EnableIndexDisplay();
	}

	private void EnableIndexDisplay(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableLabelLeft", null);
		ExcuteCallback(cbdArgs);
	}

}
