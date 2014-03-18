using UnityEngine;
using System.Collections;

public class QuestPartyPage : PartyPageLogic{

	public QuestPartyPage(string uiName) : base(uiName){}
	
	public override void CreatUI(){
		base.CreatUI();
		EnableFriendDisplay();
	}
	
	private void EnableFriendDisplay(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableItemLeft", null);
		ExcuteCallback(cbdArgs);
	}

}
