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
		ExcuteCallback (data);
	}

	void AddMsgCmd () {

		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.AddListener (CommandEnum.LevelUp, LevelUpFunc);
	}
	
	void RmvMsgCmd () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUp, LevelUpFunc);
	}
        
	void LevelUpFunc(object data) {
		ExcuteCallback (data);
	}
}
