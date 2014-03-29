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
//		MsgCenter.Instance.Invoke(CommandEnum.CancelSelectunitbrief, null);
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitBriefInfo, ReceiveShowBriefRquest);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitBriefInfo, ReceiveShowBriefRquest);
        }

	void ReceiveShowBriefRquest(object msg){
		BriefUnitInfo briefInfo = msg as BriefUnitInfo;
		lastMsgFrom = briefInfo.tag;
//		Debug.LogError ("lastMsgFrom : " + lastMsgFrom);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshPanelView", briefInfo.data);

		ExcuteCallback(cbdArgs);
	}


	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
//		Debug.LogError ("lastmsg : " + lastMsgFrom + " cbdArgs.funcName : " + cbdArgs.funcName);
		switch (cbdArgs.funcName){
			case "Choose" : 
				CallBackDispatcherHelper.DispatchCallBack(SendBackChooseMsg, cbdArgs);
				break;
			case "ViewDetailInfo" : 
				MsgCenter.Instance.Invoke(CommandEnum.ShowFocusUnitDetail, null);
				break;
			case UnitBriefInfoView.CancelCommand:
				lastMsgFrom = UnitBriefInfoView.CancelCommand;
				CallBackDispatcherHelper.DispatchCallBack(SendBackChooseMsg, cbdArgs);
				break;
			case UnitBriefInfoView.EnsureCommand:
				CallBackDispatcherHelper.DispatchCallBack(SendBackChooseMsg, cbdArgs);
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

		if (lastMsgFrom == "PartyItem") {
			Debug.Log ("UnitBriefInfoLogic.SendBackChooseMsg(), receive choose button click, activate the function of partying...");
			MsgCenter.Instance.Invoke (CommandEnum.EnsureFocusOnPartyItem, null);
			MsgCenter.Instance.Invoke (CommandEnum.ActivateMyUnitDragPanelState, true);
		} else if (lastMsgFrom == "MyUnitItem") {
			Debug.Log ("UnitBriefInfoLogic.SendBackChooseMsg(), receive choose button click, message to party panel replace item...");
			MsgCenter.Instance.Invoke (CommandEnum.EnsureSubmitUnitToParty, null);
		} else if (lastMsgFrom == UnitBriefInfoView.EnsureCommand) {
			MsgCenter.Instance.Invoke (CommandEnum.EnsureSubmitUnitToParty, true);
		} else if (lastMsgFrom == UnitBriefInfoView.CancelCommand) {
			MsgCenter.Instance.Invoke (CommandEnum.EnsureSubmitUnitToParty, false);
		}

		lastMsgFrom = null;
	}



}

