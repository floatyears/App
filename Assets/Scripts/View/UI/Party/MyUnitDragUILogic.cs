using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitDragUILogic : ConcreteComponent {

	private bool pickableState;
	private TUserUnit currentPickedUnit;

	public MyUnitDragUILogic(string uiName):base(uiName) {}

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
		CallBackDeliver cbd = new CallBackDeliver("activate", null);
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

		CallBackDeliver cbd = new CallBackDeliver("activate", null);
		ExcuteCallback(cbd);

		LogHelper.Log("MyUnitDragUILogic.ActivatePickableState(), " +
			"Receive Msg, change current pickableState to : " + pickableState);
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDeliver cbd = data as CallBackDeliver;

		switch (cbd.callBackName){
			case "ItemClick" : 
				TUserUnit tuu = cbd.callBackContent as TUserUnit;
				RspUnitPickFromView( tuu );
				break;
			case "ClickReject" :
				SendRejectMessage();
				break;
			default:
				break;
		}

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

	void SendRejectMessage(){
	
		Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");

		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}




}
