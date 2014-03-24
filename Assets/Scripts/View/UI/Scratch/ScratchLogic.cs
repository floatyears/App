using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public enum GachaFailedType {
    FriendGachaPointNotEnough = 1,
    FriendGachaUnitCountReachedMax,
    RareGachaStoneNotEnough,
    RareGachaUnitCountReachedMax,
    EventGachaNotOpen,
    EventGachaStoneNotEnough,
    EventGachaUnitCountReachedMax,
}

public enum GachaType{
    FriendGacha = 1,
    RareGacha = 2,
    EventGacha = 3,
}

public class GachaWindowInfo{
    public int totalChances = 1;
    public List<uint> blankList = new List<uint>();
    public List<uint> unitList = new List<uint>();
}

public class ScratchLogic : ConcreteComponent {
	private GachaType gachaType;
    private int gachaCount;

	public ScratchLogic(string uiName):base(uiName) {}

    public override void CreatUI () {
//        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd1");
        base.CreatUI ();
//        LogHelper.Log("ScratchDecoratorUnity CreatUI Decddddddd2");
    }

    public override void ShowUI () {
		base.ShowUI ();
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

    GachaWindowInfo GetGachaWindowInfo(GachaType gachaType, int gachaCount, List<uint> unitList, List<uint> blankList){
        GachaWindowInfo info = new GachaWindowInfo();
        info.blankList = blankList;
        LogHelper.Log("GetGachaWindowInfo() blank count {0}", blankList.Count);
        info.unitList = unitList;
        info.totalChances = gachaCount;
        return info;
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
        List<uint> blankList = rsp.blankUnitId;
        List<UserUnit> unitList = rsp.unitList;
        
        LogHelper.LogError("before gacha, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);
        // delete unit;

        DataCenter.Instance.MyUnitList.AddMyUnitList(unitList);
        DataCenter.Instance.UserUnitList.AddMyUnitList(unitList);
        
        LogHelper.LogError("after gacha, userUnitList count {0}", DataCenter.Instance.MyUnitList.GetAll().Count);

        SceneEnum nextScene = SceneEnum.FriendScratch;
        if (gachaType == GachaType.FriendGacha){
            nextScene = SceneEnum.FriendScratch;
        }
        else if (gachaType == GachaType.RareGacha){
            nextScene = SceneEnum.RareScratch;
        }
        else if (gachaType == GachaType.EventGacha){
            nextScene = SceneEnum.EventScratch;
        }
        else {
            return;
        }
        UIManager.Instance.ChangeScene(nextScene);

        LogHelper.Log("MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow");
        MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow, GetGachaWindowInfo(gachaType, gachaCount, rsp.unitUniqueId, blankList));
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);

//		TouchEventBlocker.Instance.SetState(BlockerReason.Connecting, false);
    }
    
    MsgWindowParams GetGachaFailedMsgWindowParams(GachaFailedType failedType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        switch (failedType) {
        case GachaFailedType.FriendGachaPointNotEnough:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("FriendGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("GachaFriendPointNotEnough", DataCenter.friendGachaFriendPoint);
            break;
        case GachaFailedType.FriendGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("FriendGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("UnitCountReachedMax",
                DataCenter.Instance.MyUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        case GachaFailedType.RareGachaStoneNotEnough:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RareGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("RareGachaStoneNotEnough", DataCenter.rareGachaStone);
            break;
        case GachaFailedType.RareGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RareGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("UnitCountReachedMax",
                DataCenter.Instance.MyUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        case GachaFailedType.EventGachaNotOpen:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("EventGachaNotOpen");
            break;
        case GachaFailedType.EventGachaStoneNotEnough:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("EventGachaStoneNotEnough", DataCenter.eventGachaStone);
            break;
        case GachaFailedType.EventGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.Instace.GetCurrentText("UnitCountReachedMax",
                DataCenter.Instance.MyUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        default:
            break;
        }
        msgWindowParam.btnParam = new BtnParam();
        msgWindowParam.btnParam.text = TextCenter.Instace.GetCurrentText("Back");

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
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.Instace.GetCurrentText("ConfirmOneFriendGacha");
        msgWindowParam.btnParams[1].callback = CallbackFriendGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.Instace.GetCurrentText("ConfirmMaxRareGacha", msgWindowParam.btnParams[1].args);

        return msgWindowParam;
    }

    MsgWindowParams GetRareGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        
        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("RareGacha");
        string content1 = TextCenter.Instace.GetCurrentText("RareGachaDescription");
        
        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableRareGachaTimes());
        LogHelper.Log("GetRareGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.Instace.GetCurrentText("RareGachaStatus", DataCenter.rareGachaStone, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.rareGachaStone,
                                                            DataCenter.Instance.AccountInfo.Stone);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackRareGacha;
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.Instace.GetCurrentText("ConfirmOneRareGacha");
        msgWindowParam.btnParams[1].callback = CallbackRareGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.Instace.GetCurrentText("ConfirmMaxRareGacha", msgWindowParam.btnParams[1].args);
        
        return msgWindowParam;
    }

    MsgWindowParams GetEventGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
        
        msgWindowParam.titleText = TextCenter.Instace.GetCurrentText("EventGacha");
        string content1 = TextCenter.Instace.GetCurrentText("EventGachaDescription");
        
        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableEventGachaTimes());
        LogHelper.Log("GetEventGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.Instace.GetCurrentText("EventGachaStatus", DataCenter.eventGachaStone, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.eventGachaStone,
                                                            DataCenter.Instance.AccountInfo.Stone);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackEventGacha;
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.Instace.GetCurrentText("ConfirmOneEventGacha");
        msgWindowParam.btnParams[1].callback = CallbackEventGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.Instace.GetCurrentText("ConfirmMaxEventGacha", msgWindowParam.btnParams[1].args);
        
        return msgWindowParam;
    }
    private void CallbackFriendGacha(object args){
        LogHelper.Log("CallbackFriendGacha() start");
        gachaType = GachaType.FriendGacha;
        gachaCount = (int)args;
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
//		TouchEventBlocker.Instance.SetState(BlockerReason.Connecting, true);
    }

    private void CallbackRareGacha(object args){
        LogHelper.Log("CallbackRareGacha() start");
        gachaType = GachaType.RareGacha;
        gachaCount = (int)args;
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
    }

    private void CallbackEventGacha(object args){
        gachaType = GachaType.EventGacha;
        gachaCount = (int)args;
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
    }
    
    private void OpenFriendGachaWindow(object args){
        LogHelper.Log("OnFriendGacha() start");

        if (DataCenter.Instance.GetAvailableFriendGachaTimes() < 1) {
            LogHelper.Log("OnFriendGacha() friend point not enough");
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.FriendGachaPointNotEnough));
            return;
        }
        else if (DataCenter.Instance.MyUnitList.Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.FriendGachaUnitCountReachedMax));
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendGachaMsgWindowParams());
    }

    private void OpenRareGachaWindow(object args){
        LogHelper.Log("OnRareGacha() start");
        if (DataCenter.Instance.GetAvailableRareGachaTimes() < 1) {
            LogHelper.Log("OpenRareGachaWindow() stone not enough");
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.RareGachaStoneNotEnough));
            return;
        }
        else if (DataCenter.Instance.MyUnitList.Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.RareGachaUnitCountReachedMax));
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetRareGachaMsgWindowParams());
    }

    private void OpenEventGachaWindow(object args){
        LogHelper.Log("OnEventGacha() start");
        if (!DataCenter.Instance.InEventGacha) {
            LogHelper.Log("OpenEventGachaWindow()  not in EventGacha");
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaNotOpen));
            return;
        }
        else if (DataCenter.Instance.GetAvailableEventGachaTimes() < 1) {
            LogHelper.Log("OpenEventGachaWindow() stone not enough");
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaStoneNotEnough));
            return;
        }
        else if (DataCenter.Instance.MyUnitList.Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaUnitCountReachedMax));
            return;
        }
        // TODO eventGacha
        MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetEventGachaMsgWindowParams());
    }
}