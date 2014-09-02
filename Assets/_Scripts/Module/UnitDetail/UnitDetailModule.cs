using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDetailModule : ModuleBase {
	public UnitDetailModule(UIConfigItem config):base(  config) {
		CreateUI<UnitDetailView> ();
	}
	
	public override void InitUI () {
		base.InitUI ();
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



	void AddMsgCmd () {
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.AddListener (CommandEnum.LevelUp, LevelUpFunc);
	}
	
	void RmvMsgCmd () {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, CallBackUnitData);
		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUp, LevelUpFunc);
	}

	void CallBackUnitData (object data) {
//		Debug.LogError ("CallBackUnitData : " + data);
		view.CallbackView (data);
	}

	void LevelUpFunc(object data) {
		view.CallbackView (data);
	}
}
