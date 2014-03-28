using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReceptionLogic : ConcreteComponent{

	public ReceptionLogic( string uiName ) : base( uiName ) {}
	public override void ShowUI(){
		base.ShowUI();
		EnableRefuseFriend();
        }
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void CallbackView(object data){
		base.CallbackView(data);
	}

	void AddCommandListener(){
	}
	
	void RemoveCommandListener(){
	}

	void EnableRefuseFriend(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableRefuseButton", null);
		ExcuteCallback(cbdArgs);

        }
}

