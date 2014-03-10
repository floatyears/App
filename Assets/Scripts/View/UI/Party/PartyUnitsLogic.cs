using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyUnitsLogic : ConcreteComponent {

	private bool pickableState;
	private TUserUnit currentPickedUnit;

	public PartyUnitsLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		ResetPickableState();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
	}
	
	void ActivateItem(object data){
		string tag = data as string;
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);
	}

	void ResetPickableState(){
		LogHelper.Log("MyUnitDragUILogic.ResetDragUIState(), Start...");
		pickableState = false;
		LogHelper.Log("MyUnitDragUILogic.ResetDragUIState(), End...CanPick : " + pickableState);
	}
	
	void ActivatePickableState(object data){
		bool state = (bool)data;

		pickableState = state;

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);

		LogHelper.Log("MyUnitDragUILogic.ActivatePickableState(), " +
			"Receive Msg, change current pickableState to : " + pickableState);
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "ItemClick" : 
				CallBackDispatcherHelper.DispatchCallBack(CallbackRspUnitPickFromView, cbdArgs);
				break;
			case "ClickReject" :
				CallBackDispatcherHelper.DispatchCallBack(CallBackSendRejectMessage, cbdArgs);
				break;
			default:
				break;
		}

	}

	void CallbackRspUnitPickFromView(object args){
		TUserUnit tuu = args as TUserUnit;
		RspUnitPickFromView( tuu );
	}

	void RspUnitPickFromView(TUserUnit tuu){
		if( !pickableState ){
			LogHelper.LogError(string.Format("MyUnitDragUILogic.RspUnitPickFromView(), " +
				"pickable state is : {0}, do nothing!", pickableState));
			return;
		}

		BriefUnitInfo briefInfo = new BriefUnitInfo("MyUnitItem", tuu );

		currentPickedUnit = tuu;

		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);

		LogHelper.LogError("MyUnitDragUILogic.RspUnitPickFromView(), End...");
	}
	
	void SubmitPickedUnitToParty( object data){
		if(currentPickedUnit == null){
			Debug.LogError("MyUnitDragPanel.SubmitPickedUnitToParty(), currentPickedUnit is Null, return!!!");
			return;
		}

		MsgCenter.Instance.Invoke(CommandEnum.ReplacePartyFocusItem, currentPickedUnit);
	}

	void CallBackSendRejectMessage(object args){
	
		Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");

		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}




}
