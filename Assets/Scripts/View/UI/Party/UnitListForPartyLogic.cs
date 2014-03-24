using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitListForPartyLogic : ConcreteComponent{

	UnitItemViewInfo currentPickedUnit;
	List<UnitItemViewInfo> onPartyViewItemList = new List<UnitItemViewInfo>();
	public UnitListForPartyLogic(string uiName):base(uiName){}

	public override void ShowUI(){
		base.ShowUI();
		GetOnPartyViewItemList();
		CreateUnitList();
		AddCmdListener();
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
		DestoryUnitList();
	}

	void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);
	}

	void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);
	}


	//Get the dragPanel's viewItem list
	void GetOnPartyViewItemList(){
		onPartyViewItemList.Clear();
		List<TUserUnit> tuuList = new List<TUserUnit>();
		tuuList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);

		for (int i = 0; i < tuuList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(tuuList [i]);
			onPartyViewItemList.Add(viewItem);
		}
	
	}


	void ActivateItem(object data){
		string tag = data as string;
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);
	}
	
	void ActivatePickableState(object data){
		bool state = (bool)data;
		foreach (var item in onPartyViewItemList){
			if (item.IsParty){
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

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("Activate", onPartyViewItemList);
		ExcuteCallback(cbd);

	}

	public override void Callback(object data)
	{
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName)
		{
			case "ClickItem": 
				CallBackDispatcherHelper.DispatchCallBack(CallbackRspUnitPickFromView, cbdArgs);
				break;
			case "PressItem": 
				CallBackDispatcherHelper.DispatchCallBack(ViewUnitDetailInfo, cbdArgs);
				break;
			case "ClickReject":
				CallBackDispatcherHelper.DispatchCallBack(CallBackSendRejectMessage, cbdArgs);
				break;
			default:
				break;
		}
	}

	void CallbackRspUnitPickFromView(object args)
	{
		int position = (int)args;
		RspUnitPickFromView(onPartyViewItemList [position - 1]);
	}

	void ViewUnitDetailInfo(object args)
	{
		int position = (int)args;
		TUserUnit unitInfo = onPartyViewItemList [position - 1].DataItem;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);	

	}

	void RspUnitPickFromView(UnitItemViewInfo itemView)
	{
		if (!itemView.IsEnable)
		{
			LogHelper.LogError(string.Format("PartyUnitsLogic.RspUnitPickFromView(), " +
				"pickable state is : {0}, do nothing!", itemView.IsEnable));
			return;
		}

		BriefUnitInfo briefInfo = new BriefUnitInfo("MyUnitItem", itemView.DataItem);

		currentPickedUnit = itemView;

		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);

		//LogHelper.LogError("PartyUnitsLogic.RspUnitPickFromView(), End...");
	}
	
	void SubmitPickedUnitToParty(object data){
		if (currentPickedUnit.DataItem == null){
			LogHelper.LogError("PartyUnitsLogicSubmitPickedUnitToParty(), currentPickedUnit is Null, return!!!");
			return;
		}

		Dictionary<string, object> newArgs = new Dictionary<string, object>();
		newArgs.Add("enable", false);
		newArgs.Add("party", true);
		currentPickedUnit.RefreshStates(newArgs);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", onPartyViewItemList);

		//LogHelper.Log("PartyUnitsLogicSubmitPickedUnitToParty(), sure to message PartyPage to change party member!");
		MsgCenter.Instance.Invoke(CommandEnum.ReplacePartyFocusItem, currentPickedUnit.DataItem);
	}

	void CallBackSendRejectMessage(object args){
		//Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");
		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}

	void RefreshOnPartyUnitList(object msg){
		GetOnPartyViewItemList();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", onPartyViewItemList);
		ExcuteCallback(cbdArgs);
	}
	
	void CreateUnitList(){
		//Debug.LogError("CreateUnitList(), partyUnitItemViewList count is " + partyUnitItemViewList.Count);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", onPartyViewItemList);
		ExcuteCallback(cbdArgs);
	}

	void DestoryUnitList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", onPartyViewItemList);
		ExcuteCallback(cbdArgs);
	}
}
