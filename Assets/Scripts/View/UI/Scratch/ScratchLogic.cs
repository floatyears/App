using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public enum GachaFailedType {
    FriendGachaPointNotEnough = 1,
    FriendGachaUnitCountReachedMax,
    RareGachaStoneNotEnough,
    RareGachaUnitCountReachedMax,
    EventGachaStoneNotEnough,
    EventGachaUnitCountReachedMax,
}

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

    MsgWindowParams GetFriendGachaFailedMsgWindowParams(GachaFailedType failedType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("FriendGacha");
//        string content1 = TextCenter.Instace.GetCurrentText("FriendGachaDescription");
        
//        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableFriendGachaTimes());
//        LogHelper.Log("GetFriendGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
//        string content2 = TextCenter.Instace.GetCurrentText("FriendGachaStatus", DataCenter.friendGachaFriendPoint, 
//                                                            maxGachaTimes, maxGachaTimes * DataCenter.friendGachaFriendPoint,
//                                                            DataCenter.Instance.AccountInfo.FriendPoint);
//        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
//        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
//        msgWindowParam.btnParams[0].callback = CallbackFriendGacha;
//        msgWindowParam.btnParams[1].args = 1;
//        msgWindowParam.btnParams[0].callback = CallbackFriendGacha;
//        msgWindowParam.btnParams[1].args = DataCenter.Instance.GetAvailableFriendGachaTimes();

        return msgWindowParam;
    }

    MsgWindowParams GetFriendGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("FriendGacha");
        string content1 = TextCenter.Instace.GetCurrentText("FriendGachaDescription");

        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableFriendGachaTimes());
        LogHelper.Log("GetFriendGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.Instace.GetCurrentText("FriendGachaStatus", DataCenter.friendGachaFriendPoint, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.friendGachaFriendPoint,
                                                            DataCenter.Instance.AccountInfo.FriendPoint);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackFriendGacha;
        msgWindowParam.btnParams[1].args = 1;
        msgWindowParam.btnParams[0].callback = CallbackFriendGacha;
        msgWindowParam.btnParams[1].args = DataCenter.Instance.GetAvailableFriendGachaTimes();

        return msgWindowParam;
    }

    private void CallbackFriendGacha(object args){
        LogHelper.Log("CallbackFriendGacha() start");
        int gachaCount = (int)args;
        Gacha.SendRequest(OnRspGacha, 2, gachaCount);
    }
    
    private void OpenFriendGachaWindow(object args){
        LogHelper.Log("OnFriendGacha() start");


        if (DataCenter.Instance.GetAvailableFriendGachaTimes() < 1) {
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendGachaFailedMsgWindowParams(GachaFailedType.FriendGachaPointNotEnough));
            return;
        }
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