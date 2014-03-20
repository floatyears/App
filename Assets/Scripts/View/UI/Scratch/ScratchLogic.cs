using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ScratchLogic : ConcreteComponent {
	
	public ScratchLogic(string uiName):base(uiName) {}

    public override void CreatUI () {
//        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd1");
        base.CreatUI ();
//        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd2");
    }

    public override void ShowUI () {
//        LogHelper.Log("ScratchDecoratorUnity ShowUI Decddddddd1");
		base.ShowUI ();
//        LogHelper.Log("ScratchDecoratorUnity ShowUI Decddddddd2");
	}
	
	public override void HideUI () {
		base.HideUI ();
	}

    public override void DestoryUI () {
        base.DestoryUI ();
    }

    public override void Callback(object data)
    {
        base.Callback(data);
        
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        
        switch (cbdArgs.funcName)
        {
        case "OpenFriendGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenFriendGachaWindow, cbdArgs);
            break;
        case "OpenRareGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenRareGachaWindow, cbdArgs);
            break;
        case "OpenEventGachaWindow": 
            CallBackDispatcherHelper.DispatchCallBack(OpenEventGachaWindow, cbdArgs);
            break;
        default:
            break;
        }
    }


    private void DoFriendGacha(object args){
        LogHelper.Log("DoFriendGacha() start");
        int gachaCount = (int)args;
        Gacha.SendRequest(OnRspGacha, 2, gachaCount);
    }
    
    public void OnRspGacha(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("OnRspGacha() begin");
        LogHelper.Log(data);
        bbproto.RspGacha rsp = data as bbproto.RspGacha;
        
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("RspGacha code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        DataCenter.Instance.AccountInfo.FriendPoint = rsp.friendPoint;
        DataCenter.Instance.AccountInfo.Stone = rsp.stone;
        
        // TODO do evolve start over;
        LogHelper.Log("OnRspGacha() finished, friendPoint {0}, stone {1}"
                      ,  DataCenter.Instance.AccountInfo.FriendPoint, DataCenter.Instance.AccountInfo.Stone);
        
        // record
        List<UserUnit> unitList = rsp.unitList;
        
        LogHelper.LogError("before gacha, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);
        // delete unit;
        DataCenter.Instance.MyUnitList.AddMyUnitList(unitList);
        DataCenter.Instance.UserUnitList.AddMyUnitList(unitList);
        
        LogHelper.LogError("after gacha, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);
    }


    private void OpenFriendGachaWindow(object args){
        LogHelper.Log("OnFriendGacha() start");


//        if (DataCenter.Instance.UserInfo.StaminaNow >= DataCenter.Instance.UserInfo.StaminaMax) {
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.NoNeedToBuy));
//            return;
//        }
//        else if (DataCenter.Instance.AccountInfo.Stone < DataCenter.staminaRecoverStone){
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetBuyFailMsgWindowParams(BuyType.StaminaRecover, BuyFailType.StoneNotEnough));
//            return;
//        }
//        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaMsgWindowParams());
    }

    private void OpenRareGachaWindow(object args){
        LogHelper.Log("OnRareGacha() start");
    }

    private void OpenEventGachaWindow(object args){
        LogHelper.Log("OnEventGacha() start");
    }
}