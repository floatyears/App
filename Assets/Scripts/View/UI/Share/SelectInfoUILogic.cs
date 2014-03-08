using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectInfoUILogic : ConcreteComponent {

	public SelectInfoUILogic(string uiName):base(uiName) {}

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

        void ReceiveShowBriefRquest(object data){

		Debug.Log("SelectInfoUILogic.ReceiveShowBriefRquest(), receive command, to show unit brief Info...");

		BriefUnitInfo briefInfo = data as BriefUnitInfo;
		lastMsgFrom = briefInfo.tag;

		Debug.LogError("SelectInfoUILogic.ReceiveShowBriefRquest(), lastMsgFrom is " + lastMsgFrom);
		CallBackDeliver cbd = new CallBackDeliver("ShowUnitBrief", briefInfo.data);

		ExcuteCallback( cbd );
	}


	public override void Callback(object data){
		base.Callback(data);

		CallBackDeliver cbd = data as CallBackDeliver;

		switch (cbd.callBackName){
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
