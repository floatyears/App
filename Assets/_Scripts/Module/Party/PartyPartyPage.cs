using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyPartyPage : PartyPageModule{
	private int currentFoucsPosition;
	public PartyPartyPage(UIConfigItem config) : base(  config){
//		CreateUI<
		SetFocusPostion(0);
	}

	public override void InitUI(){
		base.InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		AddCommandListener();
	}

	public override void HideUI(){
		base.HideUI();
		RemoveCommandListener();
//		DestoryUI();
		base.DestoryUI ();
	}

    public override void RefreshCurrentParty(){
        // do refresh in resetUIState
    }

    
//    public override void ResetUIState(bool clear) {
//        if (!clear){
//            return;
//        }
//        base.ResetUIState(clear);
//        RefreshCurrentPartyInfo("current");
//    }

	public override void OnReceiveMessages(params object[] data){
//		base.OnReceiveMessages(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		
		switch (data[0].ToString()){
			case "ClickItem": 
				FocusOnPositionFromView(data[1]);
				break;
			default:
				break;
		}
	}

	private void EnableIndexDisplay(){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("EnableLabelLeft", null);
		view.CallbackView("EnableLabelLeft");
	}
	
	void SetFocusPostion(int position) {
		currentFoucsPosition = position;
	}

	void FocusOnPositionFromView(object args) {
//		if (UIManager.Instance.baseScene.CurrentScene != ModuleEnum.Party) return;
				
		int position = (int)args;
		SetFocusPostion(position);
		LogHelper.LogError("currentFoucsPosition is : " + currentFoucsPosition);
//		TUserUnit tuu = null;
		List<TUserUnit> temp = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit ();
//		Debug.LogError ("temp.count : " + temp.Count + " position - 1 : " + (position - 1));
//		if (temp[ position - 1 ] == null) {
//			CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("LightCurSprite", currentFoucsPosition);
		view.CallbackView("LightCurSprite", currentFoucsPosition);
//			MsgCenter.Instance.Invoke(CommandEnum.ActivateMyUnitDragPanelState, true);
//		}
//		else {
//			tuu = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[ position - 1 ];
//			BriefUnitInfo briefInfo = new BriefUnitInfo("PartyItem", tuu);
//			MsgCenter.Instance.Invoke(CommandEnum.ShowUnitBriefInfo, briefInfo);
			MsgCenter.Instance.Invoke(CommandEnum.ActivateMyUnitDragPanelState, true);
//		}

	}

	void RejectCurrentFocusPartyMember(object msg) {
		LogHelper.Log("PartyPageUILogic.RejectCurrentFocusPartyMember(), Receive message from PartyDragPanel...");
		Debug.LogError("msg : " + msg);
		
		if (currentFoucsPosition == 1) {
			LogHelper.Log("RejectCurrentFocusPartyMember(), current focus is leader, can't reject, return...");
			return;
		}

		List<TUserUnit> tuu = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit();
		if (tuu[currentFoucsPosition - 1]==null)
			return;

		Debug.LogError ("currentFoucsPosition : " + currentFoucsPosition + " tuu : " + tuu.Count);
		uint focusUnitUniqueId = tuu[currentFoucsPosition - 1].ID;
	
//		DataCenter.Instance.PartyInfo.ChangeParty(currentFoucsPosition - 1, 0); 

//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClearItem", currentFoucsPosition);
		view.CallbackView("ClearItem", currentFoucsPosition);
		
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
	}

	void EnsureFocusOnCurrentPick(object msg) {
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("LightCurSprite", currentFoucsPosition);
		view.CallbackView("LightCurSprite", currentFoucsPosition);
	}

	void ShowFocusUnitDetail(object data) {
		if (currentFoucsPosition == 0)   return;
		
		TUserUnit targetUnit = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[currentFoucsPosition - 1];
		ModuleManger.Instance.ShowModule(ModuleEnum.UnitDetailModule);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, targetUnit);
	}

	MsgWindowParams GetPartyCostLimitMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("CostLimit");
		msgParams.contentText = TextCenter.GetText("CostLimitText");
		msgParams.btnParam = new BtnParam();
		msgParams.btnParam.text = TextCenter.GetText("I Know");
		return msgParams;
	}

	void ReplaceFocusPartyItem(object data) {
//		LogHelper.Log("PartyPageUILogic.ReplaceFocusPartyItem(), Start...");
		
		TUserUnit newPartyUnit = data as TUserUnit;
		uint uniqueId = newPartyUnit.ID;
		
		Dictionary<string,object> replaceArgsDic = new Dictionary<string, object>();
		replaceArgsDic.Add("position", currentFoucsPosition);
		replaceArgsDic.Add("unit", newPartyUnit);
		
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ReplaceItemView", replaceArgsDic);
		view.CallbackView("ReplaceItemView", replaceArgsDic);
	
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, DataCenter.Instance.PartyInfo.CurrentParty);
		
	}
	
	void AddCommandListener() {
		MsgCenter.Instance.AddListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.AddListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.AddListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
		MsgCenter.Instance.AddListener(CommandEnum.RejectPartyPageFocusItem, RejectCurrentFocusPartyMember);
	}

	void RemoveCommandListener() {
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFocusUnitDetail, ShowFocusUnitDetail);
		MsgCenter.Instance.RemoveListener(CommandEnum.ReplacePartyFocusItem, ReplaceFocusPartyItem);
		MsgCenter.Instance.RemoveListener(CommandEnum.EnsureFocusOnPartyItem, EnsureFocusOnCurrentPick);
		MsgCenter.Instance.RemoveListener(CommandEnum.RejectPartyPageFocusItem, RejectCurrentFocusPartyMember); 
	}

}
