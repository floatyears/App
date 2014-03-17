using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyUnitsLogic : ConcreteComponent
{

	UnitItemViewInfo currentPickedUnit;
	
	List<UnitItemViewInfo> partyUnitItemViewList = new List<UnitItemViewInfo>();

	public PartyUnitsLogic(string uiName):base(uiName)
	{
	}

	public override void ShowUI()
	{
		base.ShowUI();

		GetUnitItemViewList();
		CreateUnitList();
		AddCmdListener();
	}

	public override void HideUI()
	{
		base.HideUI();

		RmvCmdListener();
		DestoryUnitList();
	}

	void AddCmdListener()
	{
		MsgCenter.Instance.AddListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshUnitList);
	}

	void RmvCmdListener()
	{
		MsgCenter.Instance.RemoveListener(CommandEnum.ActivateMyUnitDragPanelState, ActivatePickableState);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureSubmitUnitToParty, SubmitPickedUnitToParty);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshUnitList);
	}

	void GetUnitItemViewList()
	{
		List<TUserUnit> userUnitList = new List<TUserUnit>();
		partyUnitItemViewList.Clear();
		userUnitList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
//		long msNow = TimeHelper.MillionSecondsNow();
		for (int i = 0; i < userUnitList.Count; i++)
		{
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(userUnitList [i]);
			partyUnitItemViewList.Add(viewItem);
			//Debug.LogError(".....viewItem isEnable : " + viewItem.IsEnable);

		}
		//LogHelper.Log("loop end {0}",  TimeHelper.MillionSecondsNow() - msNow);
		//Debug.LogError("GetUnitItemViewList(), ViewList count : " + partyUnitItemViewList.Count);

	}


	void ActivateItem(object data)
	{
		string tag = data as string;
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("activate", null);
		ExcuteCallback(cbd);
	}
	
	void ActivatePickableState(object data)
	{
		bool state = (bool)data;
		foreach (var item in partyUnitItemViewList)
		{
			if (item.IsParty)
			{
				Dictionary<string,object> args = new Dictionary<string, object>();
				args.Add("enable", false);
				item.RefreshStates(args);
			} else
			{
				Dictionary<string,object> args = new Dictionary<string, object>();
				args.Add("enable", true);
				item.RefreshStates(args);
			}
		}

		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("Activate", partyUnitItemViewList);
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
		RspUnitPickFromView(partyUnitItemViewList [position - 1]);
	}

	void ViewUnitDetailInfo(object args)
	{
		int position = (int)args;
		TUserUnit unitInfo = partyUnitItemViewList [position - 1].DataItem;
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
	
	void SubmitPickedUnitToParty(object data)
	{
		if (currentPickedUnit.DataItem == null)
		{
			LogHelper.LogError("PartyUnitsLogicSubmitPickedUnitToParty(), currentPickedUnit is Null, return!!!");
			return;
		}

		Dictionary<string, object> newArgs = new Dictionary<string, object>();
		newArgs.Add("enable", false);
		newArgs.Add("party", true);
		currentPickedUnit.RefreshStates(newArgs);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", partyUnitItemViewList);

		//LogHelper.Log("PartyUnitsLogicSubmitPickedUnitToParty(), sure to message PartyPage to change party member!");
		MsgCenter.Instance.Invoke(CommandEnum.ReplacePartyFocusItem, currentPickedUnit.DataItem);
	}

	void CallBackSendRejectMessage(object args)
	{
		//Debug.Log("MyUnitDragPanel.SendRejectMessage(), send the message that reject party current foucs member to PartyPage...");
		MsgCenter.Instance.Invoke(CommandEnum.RejectPartyPageFocusItem, null);
	}

	void RefreshUnitList(object msg)
	{
		GetUnitItemViewList();
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefreshDragList", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}
	
	void CreateUnitList()
	{
		//Debug.LogError("CreateUnitList(), partyUnitItemViewList count is " + partyUnitItemViewList.Count);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}

	void DestoryUnitList()
	{
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", partyUnitItemViewList);
		ExcuteCallback(cbdArgs);
	}
}
