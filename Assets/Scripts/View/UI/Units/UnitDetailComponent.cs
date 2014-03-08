using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailComponent : ConcreteComponent {
	public UnitDetailComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		AddMsgCmd();
        }
	   
	public override void HideUI () {
		base.HideUI ();
		RmvMsgCmd();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void CallBackUnitData ( object data) {
//		IUICallback caller = viewComponent as IUICallback;
//		caller.Callback( data );
		ExcuteCallback (null);
	}

	void AddMsgCmd () {
		//MsgCenter.Instance.AddListener(CommandEnum.LevelUp , LevelUp);
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
	}
	
	void RmvMsgCmd () {
		//MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp , LevelUp);
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
        }
        
        
}
