using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitDragUILogic : ConcreteComponent {

	public MyUnitDragUILogic(string uiName):base(uiName) {}

	public override void ShowUI(){
		base.ShowUI();

		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();

		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.NoticeFuncParty, ActivateItem);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.NoticeFuncParty, ActivateItem);
	}
	
	void ActivateItem(object data){
		CallBackDeliver cbd = new CallBackDeliver("activate",null);
		ExcuteCallback(cbd);
	}

}
