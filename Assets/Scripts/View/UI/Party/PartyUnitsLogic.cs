using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyUnitsLogic : ConcreteComponent {

	bool pickableState;
	TUserUnit currentPickedUnit;
	List<TUserUnit> playerPossessUnitList = new List<TUserUnit>();

	List<PartyUnitItemView> viewList = new List<PartyUnitItemView>();

	public PartyUnitsLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		GetUnitList();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		ResetPickableState();
		ClearUnitList();
		RmvCmdListener();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshUnitList);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshUnitList);
	}

	void GetUnitList(){
		playerPossessUnitList.AddRange(GlobalData.myUnitList.GetAll().Values);
		Debug.LogError("GetUnitList(), UnitList Count : " + playerPossessUnitList.Count);
	}

	void ClearUnitList(){
		playerPossessUnitList.Clear();
		Debug.LogError("ClearUnitList(), UnitList Count : " + playerPossessUnitList.Count);
	}

	void ActivateItem(object data){
		string tag = data as string;
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);
	}

	void ResetPickableState(){
		LogHelper.Log("PartyUnitsLogic.ResetDragUIState(), Start...");
		pickableState = false;
		LogHelper.Log("PartyUnitsLogic.ResetDragUIState(), End...CanPick : " + pickableState);
	}
	
	void ActivatePickableState(object data){
		bool state = (bool)data;

		pickableState = state;

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);

		LogHelper.Log("PartyUnitsLogic.ActivatePickableState(), " +
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
			LogHelper.LogError(string.Format("PartyUnitsLogic.RspUnitPickFromView(), " +
				"pickable state is : {0}, do nothing!", pickableState));
			return;
		}

		BriefUnitInfo briefInfo = new BriefUnitInfo("MyUnitItem", tuu );

		currentPickedUnit = tuu;

		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);

		LogHelper.LogError("PartyUnitsLogic.RspUnitPickFromView(), End...");
	}
	
	void SubmitPickedUnitToParty( object data){
		if(currentPickedUnit == null){
			LogHelper.LogError("PartyUnitsLogicSubmitPickedUnitToParty(), currentPickedUnit is Null, return!!!");
			return;
		}

		LogHelper.Log("PartyUnitsLogicSubmitPickedUnitToParty(), sure to message PartyParge to change party member!");
		MsgCenter.Instance.Invoke(CommandEnum.ReplacePartyFocusItem, currentPickedUnit);
	}

	void CallBackSendRejectMessage(object args){
		Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");
		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}

	void RefreshUnitList(object msg){
		Debug.LogError("RefreshUnitList(), Callback the list data to view...");
		pickableState = false;
		//Callback the list data to view
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", playerPossessUnitList);
		ExcuteCallback(cbdArgs);
		Debug.LogError("RefreshUnitList(), End...");
	}


}
