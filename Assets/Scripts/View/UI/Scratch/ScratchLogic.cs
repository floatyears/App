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
    public GachaType gachaType;
    public int totalChances = 1;
    public List<uint> blankList = new List<uint>();
    public List<uint> unitList = new List<uint>();
    public List<uint> newUnitIdList = new List<uint>();
}

public class ScratchLogic : ConcreteComponent {
	private GachaType gachaType;
    private int gachaCount;

	public ScratchLogic(string uiName):base(uiName) {}

    public override void CreatUI () {
        base.CreatUI ();
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

    public override void CallbackView(object data) {
		base.CallbackView (data);
        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
        switch (cbdArgs.funcName) {
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

    GachaWindowInfo GetGachaWindowInfo(GachaType gachaType, int gachaCount, List<uint> unitList, List<uint> blankList, List<uint> newUnitIdList){
        GachaWindowInfo info = new GachaWindowInfo();
        info.gachaType = gachaType;
        info.blankList = blankList;
        LogHelper.Log("GetGachaWindowInfo() blank count {0}", blankList.Count);
        info.unitList = unitList;
        info.newUnitIdList = newUnitIdList;
        foreach (var item in unitList) {
            LogHelper.Log("GetGachaWindowInfo() gacha item uint {0}", item);
        }
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
            ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code,ClickOK);
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
        
        LogHelper.LogError("before gacha, userUnitList count {0}", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
        // delete unit;

        List<uint> newUnitIdList = DataCenter.Instance.UserUnitList.FirstGetUnits(unitList);
//        DataCenter.Instance.MyUnitList.AddMyUnitList(unitList);
        DataCenter.Instance.UserUnitList.AddMyUnitList(unitList);

		//update catalog
		foreach (UserUnit unit in unitList) {
			DataCenter.Instance.CatalogInfo.AddHaveUnit( unit.unitId );
		}
        
        LogHelper.LogError("after gacha, userUnitList count {0}", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);

//        SceneEnum nextScene = SceneEnum.FriendScratch;
//        if (gachaType == GachaType.FriendGacha){
//            nextScene = SceneEnum.FriendScratch;
//        }
//        else if (gachaType == GachaType.RareGacha){
//            nextScene = SceneEnum.RareScratch;
//        }
//        else if (gachaType == GachaType.EventGacha){
//            nextScene = SceneEnum.EventScratch;
//        }
//        else {
//            return;
//        }
//        UIManager.Instance.ChangeScene(nextScene);

        LogHelper.Log("MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow");
        MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow, GetGachaWindowInfo(gachaType, gachaCount, rsp.unitUniqueId, blankList, newUnitIdList));
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);

//		TouchEventBlocker.Instance.SetState(BlockerReason.Connecting, false);
    }
    
    MsgWindowParams GetGachaFailedMsgWindowParams(GachaFailedType failedType){
        MsgWindowParams msgWindowParam = new MsgWindowParams();
		List<TUserUnit> myUnitList = DataCenter.Instance.UserUnitList.GetAllMyUnit ();
        switch (failedType) {
        case GachaFailedType.FriendGachaPointNotEnough:
            msgWindowParam.titleText = TextCenter.GetText("FriendGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("GachaFriendPointNotEnough", DataCenter.friendGachaFriendPoint);
            break;
        case GachaFailedType.FriendGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.GetText("FriendGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("UnitCountReachedMax",
               myUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        case GachaFailedType.RareGachaStoneNotEnough:
            msgWindowParam.titleText = TextCenter.GetText("RareGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("RareGachaStoneNotEnough", DataCenter.rareGachaStone);
            break;
        case GachaFailedType.RareGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.GetText("RareGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("UnitCountReachedMax",
       			myUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        case GachaFailedType.EventGachaNotOpen:
            msgWindowParam.titleText = TextCenter.GetText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("EventGachaNotOpen");
            break;
        case GachaFailedType.EventGachaStoneNotEnough:
            msgWindowParam.titleText = TextCenter.GetText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("EventGachaStoneNotEnough", DataCenter.eventGachaStone);
            break;
        case GachaFailedType.EventGachaUnitCountReachedMax:
            msgWindowParam.titleText = TextCenter.GetText("EventGachaFailed");
            msgWindowParam.contentText = TextCenter.GetText("UnitCountReachedMax",
       			myUnitList.Count, DataCenter.Instance.UserInfo.UnitMax);
            break;
        default:
            break;
        }
        msgWindowParam.btnParam = new BtnParam();
        msgWindowParam.btnParam.text = TextCenter.GetText("Back");
//		msgWindowParam.btnParam.callback = ClickOK;

        return msgWindowParam;
    }

	private void ClickOK(object data){
		Debug.Log ("scratch: ");
		UIManager.Instance.ChangeScene (SceneEnum.Scratch);
	}

    MsgWindowParams GetFriendGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.inputEnable = true;
        msgWindowParam.titleText = TextCenter.GetText("FriendGacha");
        string content1 = TextCenter.GetText("FriendGachaDescription");

        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableFriendGachaTimes());
        LogHelper.Log("GetFriendGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.GetText("FriendGachaStatus", DataCenter.friendGachaFriendPoint, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.friendGachaFriendPoint,
                                                            DataCenter.Instance.AccountInfo.FriendPoint);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackFriendGacha;
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.GetText("ConfirmOneFriendGacha");
        msgWindowParam.btnParams[1].callback = CallbackFriendGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.GetText("ConfirmMaxRareGacha", msgWindowParam.btnParams[1].args);

        return msgWindowParam;
    }

    MsgWindowParams GetRareGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.inputEnable = true;

        msgWindowParam.titleText = TextCenter.GetText("RareGacha");
        string content1 = TextCenter.GetText("RareGachaDescription");
        
        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableRareGachaTimes());
        LogHelper.Log("GetRareGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.GetText("RareGachaStatus", DataCenter.rareGachaStone, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.rareGachaStone,
                                                            DataCenter.Instance.AccountInfo.Stone);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackRareGacha;
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.GetText("ConfirmOneRareGacha");
        msgWindowParam.btnParams[1].callback = CallbackRareGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.GetText("ConfirmMaxRareGacha", msgWindowParam.btnParams[1].args);
        
        return msgWindowParam;
    }

    MsgWindowParams GetEventGachaMsgWindowParams(){
        MsgWindowParams msgWindowParam = new MsgWindowParams();

        msgWindowParam.inputEnable = true;
        msgWindowParam.titleText = TextCenter.GetText("EventGacha");
        string content1 = TextCenter.GetText("EventGachaDescription");
        
        int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableEventGachaTimes());
        LogHelper.Log("GetEventGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
        string content2 = TextCenter.GetText("EventGachaStatus", DataCenter.eventGachaStone, 
                                                            maxGachaTimes, maxGachaTimes * DataCenter.eventGachaStone,
                                                            DataCenter.Instance.AccountInfo.Stone);
        msgWindowParam.contentTexts = new string[2]{content1, content2}; 
        msgWindowParam.btnParams = new BtnParam[2]{new BtnParam(), new BtnParam()};
        msgWindowParam.btnParams[0].callback = CallbackEventGacha;
        msgWindowParam.btnParams[0].args = 1;
        msgWindowParam.btnParams[0].text = TextCenter.GetText("ConfirmOneEventGacha");
        msgWindowParam.btnParams[1].callback = CallbackEventGacha;
        msgWindowParam.btnParams[1].args = maxGachaTimes;
        msgWindowParam.btnParams[1].text = TextCenter.GetText("ConfirmMaxEventGacha", msgWindowParam.btnParams[1].args);
        
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
        else if (DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.FriendGachaUnitCountReachedMax));
            return;
        }
        SceneEnum nextScene = SceneEnum.FriendScratch;
        UIManager.Instance.ChangeScene(nextScene);
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
        else if (DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.RareGachaUnitCountReachedMax));
            return;
        }
        SceneEnum nextScene = SceneEnum.RareScratch;
        UIManager.Instance.ChangeScene(nextScene);
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetRareGachaMsgWindowParams());

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
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
		else if (DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaUnitCountReachedMax));
            return;
        }
        // TODO eventGacha
        SceneEnum nextScene = SceneEnum.EventScratch;
        UIManager.Instance.ChangeScene(nextScene);
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetEventGachaMsgWindowParams());
    }
}