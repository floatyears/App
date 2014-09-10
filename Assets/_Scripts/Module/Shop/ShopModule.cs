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
    public override void OnReceiveMessages(params object[] data){
//        base.OnReceiveMessages(data);
//        
//        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (data[0].ToString()){
	        case "DoFriendExpansion": 
	           	OnFriendExpansion();
	            break;
	        case "DoStaminaRecover": 
	            OnStaminaRecover();
	            break;
	        case "DoUnitExpansion": 
	           	OnUnitExpansion();
	            break;
	        default:
	            break;
        }
    }

    void CallbackFriendExpansion(object data){
//        MsgCenter.Instance.Invoke(CommandEnum.FriendExpansion);
		FriendMaxExpand.SendRequest(OnRspFriendExpansion);
		Umeng.GA.Buy ("FriendExpansion", 1, DataCenter.friendExpansionStone);
    }

    void OnFriendExpansion(){
//        LogHelper.Log("start OnFriendExpansion()");
        if (DataCenter.Instance.UserInfo.FriendMax >= DataCenter.maxFriendLimit) {
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.FriendExpansion, BuyFailType.NoNeedToBuy));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FriendExpansionFailed"),TextCenter.GetText("FriendCountLimitReachedMax"),TextCenter.GetText("OK"));
            return;
        }
        else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.friendExpansionStone){
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.FriendExpansion, BuyFailType.StoneNotEnough));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FriendExpansionFailed"),TextCenter.GetText("FriendExpandStonesNotEnough"),TextCenter.GetText("OK"));
            return;
        }
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendExpansionMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FriendExpand"),
		                                   new string[2]{ TextCenter.GetText("FriendExpansionConfirm", DataCenter.friendExpansionStone, DataCenter.friendsExpandCount), TextCenter.GetText("FriendExpansionInfo", DataCenter.Instance.FriendCount,DataCenter.Instance.UserInfo.FriendMax) },
										   TextCenter.GetText("DoFriendExpand"),TextCenter.GetText("CANCEL"),CallbackFriendExpansion);

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
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuySuccessWindowParams(BuyType.FriendExpansion));
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("FriendExpansionFinish"), TextCenter.GetText ("FriendExpansionResult", DataCenter.Instance.UserInfo.FriendMax),TextCenter.GetText("OK"));

    }
	
    void OnStaminaRecover(){
        if (DataCenter.Instance.UserInfo.StaminaNow >= DataCenter.Instance.UserInfo.StaminaMax) {
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.NoNeedToBuy));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaRecoverFailed"),TextCenter.GetText("StaminaStillFull"),TextCenter.GetText("OK"));
            return;
        } else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.staminaRecoverStone){
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.StoneNotEnough));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaRecoverFailed"),TextCenter.GetText("StaminaRecoverStonesNotEnough"),TextCenter.GetText("OK"));
            return;
        }
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaMsgWindowParams());
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("StaminaRecover"), TextCenter.GetText ("StaminaRecoverConfirm", DataCenter.staminaRecoverStone), TextCenter.GetText ("DoStaminaRecover"), TextCenter.GetText ("CANCEL"), CallbackStaminaRecover);
    }

    void CallbackStaminaRecover(object args){
		RestoreStamina.SendRequest(OnRspStartminaRecover);

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
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("StaminaRecoverFinish"), TextCenter.GetText ("StaminaRecoverResult", DataCenter.Instance.UserInfo.StaminaNow),TextCenter.GetText("OK"));
    }



    void OnUnitExpansion(){

        if (DataCenter.Instance.UserInfo.UnitMax >= DataCenter.maxUnitLimit) {
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("UnitExpansionFailed"),TextCenter.GetText("UnitCountLimitReachedMax"),TextCenter.GetText("OK"));
            return;
        }
        else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.unitExpansionStone){
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("UnitExpansionFailed"),TextCenter.GetText("UnitExpandStonesNotEnough"),TextCenter.GetText("OK"));
            return;
        }
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("UnitExpand"), new string[2] {
						TextCenter.GetText ("UnitExpansionConfirm", DataCenter.unitExpansionStone),
						TextCenter.GetText ("UnitExpansionInfo", DataCenter.Instance.UserUnitList.GetAllMyUnit ().Count, DataCenter.Instance.UserInfo.UnitMax)}, 
						TextCenter.GetText ("DoUnitExpansion"),TextCenter.GetText("CANCEL"), CallbackUnitpansion);

    }

    void CallbackUnitpansion(object args){
		Umeng.GA.Buy ("UnitExpansion",1, DataCenter.unitExpansionStone);

		UnitMaxExpand.SendRequest(OnRspUnitExpansion);
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
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuySuccessWindowParams(BuyType.UnitExpansion));
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("UnitExpansionFinish"), TextCenter.GetText ("UnitExpansionResult", DataCenter.Instance.UserInfo.UnitMax),TextCenter.GetText("OK"));
    }

}
