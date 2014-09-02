using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuyType{
    FriendExpansion = 1,
    StaminaRecover = 2,
    UnitExpansion   = 3,
}

public enum BuyFailType {
    StoneNotEnough = 1,
    NoNeedToBuy    = 2,
}

public class ShopModule : ModuleBase {
	
	public ShopModule(UIConfigItem config):base(  config) {
		CreateUI<ShopView> ();
	}
	
	public override void InitUI () {

		base.InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
        AddListener();
	}
	
	public override void HideUI () {
		base.HideUI ();
        RemoveListener();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

    public override void OnReceiveMessages(object data){
        base.OnReceiveMessages(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName){
        case "DoFriendExpansion": 
            CallBackDispatcherHelper.DispatchCallBack(OnFriendExpansion, cbdArgs);
            break;
        case "DoStaminaRecover": 
            CallBackDispatcherHelper.DispatchCallBack(OnStaminaRecover, cbdArgs);
            break;
        case "DoUnitExpansion": 
            CallBackDispatcherHelper.DispatchCallBack(OnUnitExpansion, cbdArgs);
            break;
        default:
            break;
        }
    }

    void AddListener(){
        MsgCenter.Instance.AddListener(CommandEnum.FriendExpansion, DoFriendExpansion);
        MsgCenter.Instance.AddListener(CommandEnum.StaminaRecover, DoStaminaRecover);          
        MsgCenter.Instance.AddListener(CommandEnum.UnitExpansion, DoUnitExpansion);
    }

    void RemoveListener(){
        MsgCenter.Instance.RemoveListener(CommandEnum.FriendExpansion, DoFriendExpansion);
        MsgCenter.Instance.RemoveListener(CommandEnum.StaminaRecover, DoStaminaRecover);          
        MsgCenter.Instance.RemoveListener(CommandEnum.UnitExpansion, DoUnitExpansion);
    }


    MsgWindowParams GetFriendExpansionMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.titleText = TextCenter.GetText("FriendExpand");

		string context1 = TextCenter.GetText("FriendExpansionConfirm", DataCenter.friendExpansionStone, DataCenter.friendsExpandCount);
        string context2 = TextCenter.GetText("FriendExpansionInfo", DataCenter.Instance.FriendCount,
                                                          DataCenter.Instance.UserInfo.FriendMax);

        msgWindowParam.contentTexts = new string[2]{ context1, context2 };
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackFriendExpansion;
        msgWindowParam.btnParams[0].text = TextCenter.GetText("DoFriendExpand");
        return msgWindowParam;
    }

    MsgWindowParams GetBuySuccessWindowParams(BuyType buyType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        
        string title = "";
        string content = "";
        
        switch (buyType) {
        case BuyType.FriendExpansion:
			title = TextCenter.GetText("FriendExpansionFinish");
            content = TextCenter.GetText("FriendExpansionResult", DataCenter.Instance.UserInfo.FriendMax);
            break;
        case BuyType.StaminaRecover:
			title = TextCenter.GetText("StaminaRecoverFinish");
			content = TextCenter.GetText("StaminaRecoverResult", DataCenter.Instance.UserInfo.StaminaNow);
            break;
        case BuyType.UnitExpansion:
			title = TextCenter.GetText("UnitExpansionFinish");
            content = TextCenter.GetText("UnitExpansionResult", DataCenter.Instance.UserInfo.UnitMax);
            break;
        default:
            break;
        }
        msgWindowParam.titleText = title;
        
        msgWindowParam.contentText = content;
        msgWindowParam.btnParam = new BtnParam();
        return msgWindowParam;
    }

    MsgWindowParams GetFriendExpansionBuyFailParams(BuyFailType failType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.GetText("FriendExpansionFailed");
        string content = failType == BuyFailType.NoNeedToBuy ? "FriendCountLimitReachedMax" : "FriendExpandStonesNotEnough";
        msgWindowParam.contentText = TextCenter.GetText(content);;
        msgWindowParam.btnParam = new BtnParam();
        return msgWindowParam;
    }
 
    MsgWindowParams GetStaminaRecoverBuyFailParams(BuyFailType failType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.GetText("StaminaRecoverFailed");
        string content = failType == BuyFailType.NoNeedToBuy ? "StaminaStillFull" : "StaminaRecoverStonesNotEnough";
        msgWindowParam.contentText = TextCenter.GetText(content);;
        msgWindowParam.btnParam = new BtnParam();
        return msgWindowParam;
    }

    MsgWindowParams GetUnitExpansionBuyFailParams(BuyFailType failType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.GetText("UnitExpansionFailed");
        string content = failType == BuyFailType.NoNeedToBuy ? "UnitCountLimitReachedMax" : "UnitExpandStonesNotEnough";
        msgWindowParam.contentText = TextCenter.GetText(content);;
        msgWindowParam.btnParam = new BtnParam();
        return msgWindowParam;
    }

    MsgWindowParams GetBuyFailMsgWindowParams(BuyType buyType, BuyFailType failType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        Dictionary<BuyType, MsgWindowParams> paramsDict = new Dictionary<BuyType, MsgWindowParams>();
        paramsDict.Add(BuyType.FriendExpansion, GetFriendExpansionBuyFailParams(failType));
        paramsDict.Add(BuyType.StaminaRecover, GetStaminaRecoverBuyFailParams(failType));
        paramsDict.Add(BuyType.UnitExpansion, GetUnitExpansionBuyFailParams(failType));
        if (paramsDict.ContainsKey(buyType)){
            return paramsDict[buyType];
        }
//	//-------------------------
//		msgWindowParam.fullScreenClick = true;
        return msgWindowParam;
    }

    void CallbackFriendExpansion(object args){
        MsgCenter.Instance.Invoke(CommandEnum.FriendExpansion);
		Umeng.GA.Buy ("FriendExpansion", 1, DataCenter.friendExpansionStone);
    }

    void OnFriendExpansion(object args){
//        LogHelper.Log("start OnFriendExpansion()");
        if (DataCenter.Instance.UserInfo.FriendMax >= DataCenter.maxFriendLimit) {
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.FriendExpansion, BuyFailType.NoNeedToBuy));
            return;
        }
        else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.friendExpansionStone){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.FriendExpansion, BuyFailType.StoneNotEnough));
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendExpansionMsgWindowParams());

    }

    void DoFriendExpansion(object args){
        FriendMaxExpand.SendRequest(OnRspFriendExpansion);
    }

    void OnRspFriendExpansion(object data){
        if (data == null)
            return;
        
//        LogHelper.Log("OnRspFriendExpansion begin");
        LogHelper.Log(data);
        bbproto.RspFriendMaxExpand rsp = data as bbproto.RspFriendMaxExpand;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS)
        {
            LogHelper.Log("OnRspFriendExpansion code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
            return;
        }
        
        DataCenter.Instance.UserInfo.FriendMax = rsp.friendMax;
        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips);
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuySuccessWindowParams(BuyType.FriendExpansion));

    }

    void OnStaminaRecover(object args){
        if (DataCenter.Instance.UserInfo.StaminaNow >= DataCenter.Instance.UserInfo.StaminaMax) {
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.NoNeedToBuy));
            return;
        } else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.staminaRecoverStone){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.StoneNotEnough));
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaMsgWindowParams());
    }

    MsgWindowParams GetStaminaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        
        msgWindowParam.titleText = TextCenter.GetText("StaminaRecover");

		msgWindowParam.contentText = TextCenter.GetText("StaminaRecoverConfirm", DataCenter.staminaRecoverStone);
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackStaminaRecover;
        msgWindowParam.btnParams[0].text = TextCenter.GetText("DoStaminaRecover");
        return msgWindowParam;
    }
    
    void DoStaminaRecover(object args){
        RestoreStamina.SendRequest(OnRspStartminaRecover);
    }

    void CallbackStaminaRecover(object args){
        MsgCenter.Instance.Invoke(CommandEnum.StaminaRecover);

		Umeng.GA.Buy ("StaminaRecover", 1, DataCenter.staminaRecoverStone);
    }

    void OnRspStartminaRecover(object data){
        if (data == null)
            return;
        
        LogHelper.Log("OnRspStartminaRecover() begin");
        LogHelper.Log(data);
        bbproto.RspRestoreStamina rsp = data as bbproto.RspRestoreStamina;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("OnRspStartminaRecover code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
            return;
        }

        LogHelper.Log("OnRspStartminaRecover StaminaNow:{0}", rsp.staminaNow);

        DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
        DataCenter.Instance.UserInfo.StaminaMax = rsp.staminaMax;
        DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;

        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        MsgCenter.Instance.Invoke(CommandEnum.SyncStamina, null);
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuySuccessWindowParams(BuyType.StaminaRecover));
    }

    void OnUnitExpansion(object args){
//        LogHelper.Log("start OnUnitExpansion()");

        if (DataCenter.Instance.UserInfo.UnitMax >= DataCenter.maxUnitLimit) {
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.UnitExpansion, BuyFailType.NoNeedToBuy));
            return;
        }
        else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.unitExpansionStone){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.UnitExpansion, BuyFailType.StoneNotEnough));
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitExpansionMsgWindowParams());

    }

    MsgWindowParams GetUnitExpansionMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        
        msgWindowParam.titleText = TextCenter.GetText("UnitExpand");
        
		string context1 = TextCenter.GetText("UnitExpansionConfirm", DataCenter.unitExpansionStone);
        string context2 = TextCenter.GetText("UnitExpansionInfo", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,
                                                            DataCenter.Instance.UserInfo.UnitMax);
        
        msgWindowParam.contentTexts = new string[2]{ context1, context2 };
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackUnitpansion;
		msgWindowParam.btnParams[0].text = TextCenter.GetText("DoUnitExpansion");
        return msgWindowParam;
    }

    void DoUnitExpansion(object args){
        UnitMaxExpand.SendRequest(OnRspUnitExpansion);
    }

    void CallbackUnitpansion(object args){
		Umeng.GA.Buy ("UnitExpansion",1, DataCenter.unitExpansionStone);

        MsgCenter.Instance.Invoke(CommandEnum.UnitExpansion);
    }

    void OnRspUnitExpansion(object data){
        if (data == null)
            return;
        
        LogHelper.Log("OnRspUnitExpansion() begin");
        LogHelper.Log(data);
        bbproto.RspUnitMaxExpand rsp = data as bbproto.RspUnitMaxExpand;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS)
        {
            LogHelper.Log("RspUnitMaxExpand code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
            return;
        }
        
        DataCenter.Instance.UserInfo.UnitMax = rsp.unitMax;
        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuySuccessWindowParams(BuyType.UnitExpansion));
    }

}
