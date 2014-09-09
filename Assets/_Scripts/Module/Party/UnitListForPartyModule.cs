using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitListForPartyModule : ModuleBase{
	PartyUnitItem currentPickedUnit;
//	List<UnitItemViewInfo> onPartyViewItemList = new List<UnitItemViewInfo>();
	List<TUserUnit> partyDataList = new List<TUserUnit>();

	public UnitListForPartyModule(UIConfigItem config):base(  config){
//		CreateUI<un
	}

	public override void ShowUI(){
		base.ShowUI();
//		GetOnPartyViewItemList();
//		CreateUnitList();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
//		DestoryUnitList();
	}

//    public override void ResetUIState(bool clear) {
//        if (!clear){
//            return;
//        }
//        base.ResetUIState(clear);
//        DestoryUnitList();
//        GetOnPartyViewItemList();
//        CreateUnitList();
//
//    }

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
//		MsgCenter.Instance.AddListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
//		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
//		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
//		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);
	}


	//Get the dragPanel's viewItem list
	void GetOnPartyViewItemList(){
//		onPartyViewItemList.Clear();
		partyDataList.Clear();
//		List<TUserUnit> tuuList = new List<TUserUnit>();
		partyDataList.AddRange(DataCenter.Instance.UserUnitList.GetAllMyUnit());

//		for (int i = 0; i < tuuList.Count; i++){
//			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(tuuList [i]);
////			onPartyViewItemList.Add(viewItem);
//		}
	
	}


	void ActivateItem(object data){
		string tag = data as string;
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		view.CallbackView("activate");
	}
	
	void ActivatePickableState(object data){
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("Activate", null);
		view.CallbackView("Activate");
//		bool state = (bool)data;
//		foreach (var item in onPartyViewItemList){
//			if (item.IsParty){
//				Dictionary<string,object> args = new Dictionary<string, object>();
//				args.Add("enable", false);
//				item.RefreshStates(args);
//			} 
//			else{
//				Dictionary<string,object> args = new Dictionary<string, object>();
//				args.Add("enable", true);
//				item.RefreshStates(args);
//			}
//		}
//
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("Activate", onPartyViewItemList);
//		ExcuteCallback(cbd);

	}

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (data[0].ToString()){
			case "ClickItem": 
				CallbackRspUnitPickFromView(data[1]);
				break;
			case "PressItem": 
				ViewUnitDetailInfo(data[1]);
				break;
			case "ClickReject":
				CallBackSendRejectMessage(data[1]);
				break;
			default:
				break;
		}
	}

	void CallbackRspUnitPickFromView(object args){
		PartyUnitItem position = (PartyUnitItem)args;
		RspUnitPickFromView(position);
	}

	void ViewUnitDetailInfo(object args){
		int position = (int)args;
//		TUserUnit unitInfo = onPartyViewItemList [position - 1].DataItem;
		TUserUnit unitInfo = partyDataList[position - 1];
		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule,"unit",unitInfo);
	}

	void RspUnitPickFromView(PartyUnitItem itemView){
		currentPickedUnit = itemView;
		TUserUnit tup = partyDataList.Find(a=>a.Equals(itemView.UserUnit));
		if(tup ==default(TUserUnit)) {
			SubmitPickedUnitToParty(true);
		} else{
			SubmitPickedUnitToParty(false);
		}

	}
	
	void SubmitPickedUnitToParty(bool temp){
//		if (currentPickedUnit.UserUnit == null){
//			LogHelper.LogError("PartyUnitsLogicSubmitPickedUnitToParty(), currentPickedUnit is Null, return!!!");
//			return;
//		}
		Debug.LogError("SubmitPickedUnitToParty");
		currentPickedUnit.IsParty = temp;
		currentPickedUnit.IsEnable = !temp;
//		Dictionary<string, object> newArgs = new Dictionary<string, object>();
//		newArgs.Add("enable", false);
//		newArgs.Add("party", true);
//		currentPickedUnit.RefreshStates(newArgs);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", onPartyViewItemList);

		//LogHelper.Log("PartyUnitsLogicSubmitPickedUnitToParty(), sure to message PartyPage to change party member!");
		MsgCenter.Instance.Invoke(CommandEnum.ReplacePartyFocusItem, currentPickedUnit.UserUnit);
	}

	void CallBackSendRejectMessage(object args){
		//Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");
		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}

//	void RefreshOnPartyUnitList(object msg){
//		GetOnPartyViewItemList();
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", onPartyViewItemList);
//		ExcuteCallback(cbdArgs);
//	}
	
	void CreateUnitList(){
		//Debug.LogError("CreateUnitList(), partyUnitItemViewList count is " + partyUnitItemViewList.Count);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", partyDataList);
		Debug.LogError("CreateUnitList : " + view);
		view.CallbackView("CreateDragView", partyDataList);
	}

	void DestoryUnitList(){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", onPartyViewItemList);
//		ExcuteCallback(cbdArgs);
	}
}
