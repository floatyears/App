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

		Debug.Log("SelectInfoUILogic.ReceiveShowBriefRquest(), receive command, to show unit brief Info...");

		BriefUnitInfo briefInfo = msg as BriefUnitInfo;
		lastMsgFrom = briefInfo.tag;

		Debug.LogError("SelectInfoUILogic.ReceiveShowBriefRquest(), lastMsgFrom is " + lastMsgFrom);

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshPanelView", briefInfo.data);

		ExcuteCallback(cbdArgs);
	}


	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbd = data as CallBackDispatcherArgs;

		switch (cbd.funcName){
			case "Choose" : 
				SendBackChooseMsg();
				break;
			case "ViewDetailInfo" : 
				MsgCenter.Instance.Invoke(CommandEnum.ShowFocusUnitDetail, null);
				break;

			default:
				break;
		}
	}
	
	void SendBackChooseMsg(){
		if(lastMsgFrom == null){
			Debug.LogError("SelectInfoUILogic.SendBackChooseMsg(), lastMsgFrom is NULL, return!!!");
			return;
		}

		if(lastMsgFrom == "PartyItem"){
			MsgCenter.Instance.Invoke(CommandEnum.EnsureFocusOnPartyItem, null);
			MsgCenter.Instance.Invoke(CommandEnum.ActivateMyUnitDragPanelState, true);
		}

		if(lastMsgFrom == "MyUnitItem")
			MsgCenter.Instance.Invoke(CommandEnum.EnsureSubmitUnitToParty, null);

		lastMsgFrom = null;

		Debug.LogError("SelectInfoUILogic.SendBackChooseMsg(), End...");
	}



}
