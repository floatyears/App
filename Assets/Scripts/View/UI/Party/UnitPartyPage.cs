using UnityEngine;
using System.Collections;

public class UnitPartyPage : PartyPageLogic{

	public UnitPartyPage(string uiName) : base(uiName){}

	public override void CreatUI(){
		base.CreatUI();
		EnableIndexDisplay();
	}

	private void EnableIndexDisplay(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableLabelLeft", null);
		ExcuteCallback(cbdArgs);
	}



}
