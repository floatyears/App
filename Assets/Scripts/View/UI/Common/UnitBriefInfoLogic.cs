using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBriefInfoLogic : ConcreteComponent {

	public UnitBriefInfoLogic(string uiName):base(uiName) {}

	string lastMsgFrom;

	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitBriefInfo, ReceiveShowBriefRquest);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitBriefInfo, ReceiveShowBriefRquest);
        }

        void ReceiveShowBriefRquest(object msg){

		//Debug.Log("UnitBriefInfoLogic.ReceiveShowBriefRquest(), receive command, to show unit brief Info...");

		BriefUnitInfo briefInfo = msg as BriefUnitInfo;
		lastMsgFrom = briefInfo.tag;

		//Debug.LogError("UnitBriefInfoLogic.ReceiveShowBriefRquest(), lastMsgFrom is " + lastMsgFrom);

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshPanelView", briefInfo.data);

		ExcuteCallback(cbdArgs);
	}


	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "Choose" : 
				CallBackDispatcherHelper.DispatchCallBack(SendBackChooseMsg, cbdArgs);
				break;
			case "ViewDetailInfo" : 
				MsgCenter.Instance.Invoke(CommandEnum.ShowFocusUnitDetail, null);
				break;
			default:
				break;
		}
	}
	
	void SendBackChooseMsg(object args){
		if(lastMsgFrom == null){
			Debug.LogError("UnitBriefInfoLogic.SendBackChooseMsg(), lastMsgFrom is NULL, return!!!");
			return;
		}

		if(lastMsgFrom == "PartyItem"){
			Debug.Log("UnitBriefInfoLogic.SendBackChooseMsg(), receive choose button click, activate the function of partying...");
			MsgCenter.Instance.Invoke(CommandEnum.EnsureFocusOnPartyItem, null);
			MsgCenter.Instance.Invoke(CommandEnum.ActivateMyUnitDragPanelState, true);
		}

		if(lastMsgFrom == "MyUnitItem"){
			Debug.Log("UnitBriefInfoLogic.SendBackChooseMsg(), receive choose button click, message to party panel replace item...");
			MsgCenter.Instance.Invoke(CommandEnum.EnsureSubmitUnitToParty, null);
		}

		lastMsgFrom = null;

//		Debug.LogError("UnitBriefInfoLogic.SendBackChooseMsg(), End...");
	}



}
