using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyUnitsLogic : ConcreteComponent {

	bool pickableState;
	TUserUnit currentPickedUnit;
	
	List<PartyUnitItemView> partyUnitItemViewList = new List<PartyUnitItemView>();

	public PartyUnitsLogic(string uiName):base(uiName){

	}

	public override void ShowUI(){
	
		base.ShowUI();

		GetUnitItemViewList();
		CreateUnitList();

		AddCmdListener();
	
	}

	public override void HideUI(){
		base.HideUI();
		//ResetPickableState();
		RmvCmdListener();

		DestoryUnitList();
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

	void GetUnitItemViewList(){
		List<TUserUnit> userUnitList = new List<TUserUnit>();
		partyUnitItemViewList.Clear();
		userUnitList.AddRange(GlobalData.myUnitList.GetAll().Values);
		long msNow = TimeHelper.MillionSecondsNow();
		for(int i = 0; i < userUnitList.Count; i++){
			PartyUnitItemView viewItem = PartyUnitItemView.Create(userUnitList[ i ]);
			partyUnitItemViewList.Add(viewItem);
			//Debug.LogError(".....viewItem isEnable : " + viewItem.IsEnable);

		}
		LogHelper.Log("loop end {0}",  TimeHelper.MillionSecondsNow() - msNow);
		//Debug.LogError("GetUnitItemViewList(), ViewList count : " + partyUnitItemViewList.Count);

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
		//pickableState = state;
		foreach (var item in partyUnitItemViewList){
			if(item.IsParty){
				Dictionary<string,object> args = new Dictionary<string, object>();
				args.Add("enable", false);
				item.RefreshStates(args);
			}
			else{
				Dictionary<string,object> args = new Dictionary<string, object>();
				args.Add("enable", true);
				item.RefreshStates(args);
			}
		}

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", partyUnitItemViewList);
		ExcuteCallback(cbd);

//		LogHelper.Log("PartyUnitsLogic.ActivatePickableState(), " +
//			"Receive Msg, change current pickableState to : " + pickableState);
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
//		TUserUnit tuu = args as TUserUnit;
		int position = (int)args;
		RspUnitPickFromView( partyUnitItemViewList[ position - 1 ] );
	}

	void RspUnitPickFromView(PartyUnitItemView itemView){
		if( !itemView.IsEnable ){
			LogHelper.LogError(string.Format("PartyUnitsLogic.RspUnitPickFromView(), " +
			                                 "pickable state is : {0}, do nothing!", itemView.IsEnable));
			return;
		}

		BriefUnitInfo briefInfo = new BriefUnitInfo("MyUnitItem", itemView.DataItem );

		currentPickedUnit = itemView.DataItem;

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
		GetUnitItemViewList();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}
	
	void CreateUnitList(){
		Debug.LogError("CreateUnitList(), partyUnitItemViewList count is " + partyUnitItemViewList.Count);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}

	void DestoryUnitList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}
}
