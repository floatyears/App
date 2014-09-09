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

public class ScratchModule : ModuleBase {
	private GachaType gachaType;
    private int gachaCount;

	public ScratchModule(UIConfigItem config):base(  config) {
		CreateUI<ScratchView> ();
	}

    public override void OnReceiveMessages(params object[] data) {
//		base.OnReceiveMessages (data);
//        CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (data[0].ToString()) {
	        case "OpenFriendGachaWindow": 
	            OpenFriendGachaWindow();
	            break;
	        case "OpenRareGachaWindow": 
	            OpenRareGachaWindow();
	            break;
	        case "OpenEventGachaWindow": 
	            OpenEventGachaWindow();
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
        
		if(rsp.unitList.Count == 0) {
//			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow("No Card Return",ClickOK);

//			Debug.LogError("server return data is null");
//			UIManager.Instance.GoBackToPrevScene();
			TipsManager.Instance.ShowTipsLabel("server return data is null");
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
        DataCenter.Instance.UserUnitList.AddMyUnitList(unitList);
        
        LogHelper.LogError("after gacha, userUnitList count {0}", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);

        LogHelper.Log("MsgCenter.Instance.Invoke(CommandEnum.EnterGachaWindow");
		ModuleManager.Instance.HideModule (ModuleEnum.ScratchModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.GachaModule, "type", gachaType, "chances", gachaCount, "unit", rsp.unitUniqueId, "blank", blankList, "new", newUnitIdList);// GetGachaWindowInfo (gachaType, gachaCount, rsp.unitUniqueId, blankList, newUnitIdList));
//		ModuleManager.SendMessage(ModuleEnum.GachaModule,
        MsgCenter.Instance.Invoke(CommandEnum.SyncChips, null);

//		TouchEventBlocker.Instance.SetState(BlockerReason.Connecting, false);
    }
    
   

	private void ClickOK(object data){
		Debug.Log ("scratch: ");
		ModuleManager.Instance.ShowModule (ModuleEnum.ScratchModule);
	}
	
    private void CallbackFriendGacha(object args){
        LogHelper.Log("CallbackFriendGacha() start");
        gachaType = GachaType.FriendGacha;
        gachaCount = (int)args;
		Umeng.GA.Event ("Gacha1",gachaCount+"");
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
//		TouchEventBlocker.Instance.SetState(BlockerReason.Connecting, true);
    }

    private void CallbackRareGacha(object args){
        LogHelper.Log("CallbackRareGacha() start");
        gachaType = GachaType.RareGacha;
        gachaCount = (int)args;
		Umeng.GA.Event ("Gacha2",gachaCount+"");
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
    }

    private void CallbackEventGacha(object args){
        gachaType = GachaType.EventGacha;
//		Debug.LogError ("gachaType");
        gachaCount = (int)args;
		Umeng.GA.Event ("Gacha3",gachaCount+"");
//		Debug.LogError ((int)gachaType + " gachaCount : " + gachaCount);
        Gacha.SendRequest(OnRspGacha, (int)gachaType, gachaCount);
    }

    private void OpenFriendGachaWindow(){
        LogHelper.Log("OnFriendGacha() start");

        if (DataCenter.Instance.GetAvailableFriendGachaTimes() < 1) {
            LogHelper.Log("OnFriendGacha() friend point not enough");
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.FriendGachaPointNotEnough));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FriendGachaFailed"),TextCenter.GetText("GachaFriendPointNotEnough", DataCenter.friendGachaFriendPoint),TextCenter.GetText("Back"));
            return;
        }
//        else if (DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.FriendGachaUnitCountReachedMax));
//            return;
//        }
        ModuleEnum nextScene = ModuleEnum.FriendScratchModule;
//        ModuleManger.Instance.ShowModule(nextScene);
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetFriendGachaMsgWindowParams());

		int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableFriendGachaTimes());
		LogHelper.Log("GetFriendGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
		string content2 = TextCenter.GetText("FriendGachaStatus", DataCenter.friendGachaFriendPoint, 
		                                     maxGachaTimes, maxGachaTimes * DataCenter.friendGachaFriendPoint,
		                                     DataCenter.Instance.AccountInfo.FriendPoint);
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("FriendGacha"), new string[2]{TextCenter.GetText ("FriendGachaDescription"),content2}, 
					TextCenter.GetText ("ConfirmOneFriendGacha"), 
					TextCenter.GetText ("ConfirmMaxRareGacha", maxGachaTimes),
					CallbackFriendGacha, CallbackFriendGacha, 1, maxGachaTimes);
    }
	
    private void OpenRareGachaWindow(){
        LogHelper.Log("OnRareGacha() start");
        if (DataCenter.Instance.GetAvailableRareGachaTimes() < 1) {
            LogHelper.Log("OpenRareGachaWindow() stone not enough");
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.RareGachaStoneNotEnough));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("RareGachaFailed"),TextCenter.GetText("RareGachaStoneNotEnough", DataCenter.rareGachaStone),TextCenter.GetText("Back"));
            return;
        }
//        else if (DataCenter.Instance.UserUnitList.GetAllMyUnit().Count > DataCenter.Instance.UserInfo.UnitMax){
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.RareGachaUnitCountReachedMax));
//            return;
//        }
		ModuleEnum nextScene = ModuleEnum.RareScratchModule;
//        ModuleManger.Instance.ShowModule(nextScene);
//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetRareGachaMsgWindowParams());
		int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableRareGachaTimes());
		LogHelper.Log("GetRareGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
		string content2 = TextCenter.GetText("RareGachaStatus", DataCenter.rareGachaStone, 
		                                     maxGachaTimes, maxGachaTimes * DataCenter.rareGachaStone,
		                                     DataCenter.Instance.AccountInfo.Stone);
		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("RareGacha"), new string[2] {TextCenter.GetText ("RareGachaDescription"),content2}, 
				TextCenter.GetText ("ConfirmOneRareGacha"), 
				TextCenter.GetText ("ConfirmMaxRareGacha", maxGachaTimes), 
				CallbackRareGacha, CallbackRareGacha, 1, maxGachaTimes);

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
    }

    private void OpenEventGachaWindow(){
        LogHelper.Log("OnEventGacha() start");
        if (!DataCenter.Instance.InEventGacha) {
            LogHelper.Log("OpenEventGachaWindow()  not in EventGacha");
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaNotOpen));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("EventGachaFailed"),TextCenter.GetText("EventGachaNotOpen"),TextCenter.GetText("Back"));
            return;
        }
        else if (DataCenter.Instance.GetAvailableEventGachaTimes() < 1) {
            LogHelper.Log("OpenEventGachaWindow() stone not enough");
//            MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow,
//                                      GetGachaFailedMsgWindowParams(GachaFailedType.EventGachaStoneNotEnough));
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("EventGachaFailed"),TextCenter.GetText("EventGachaStoneNotEnough", DataCenter.eventGachaStone),TextCenter.GetText("Back"));
            return;
        }
        // TODO eventGacha
//		ModuleEnum nextScene = ModuleEnum.EventScratchModule;
//        ModuleManger.Instance.ShowModule(nextScene);
		int maxGachaTimes = Mathf.Min(DataCenter.maxGachaPerTime, DataCenter.Instance.GetAvailableEventGachaTimes());
		LogHelper.Log("GetEventGachaMsgWindowParams() maxGachaTimes {0}", maxGachaTimes);
		string content2 = TextCenter.GetText("EventGachaStatus", DataCenter.eventGachaStone, 
		                                     maxGachaTimes, maxGachaTimes * DataCenter.eventGachaStone,
		                                     DataCenter.Instance.AccountInfo.Stone);

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("EventGacha"), new string[2] {TextCenter.GetText ("EventGachaDescription"),content2}, 
				TextCenter.GetText ("ConfirmOneEventGacha"), 
				TextCenter.GetText ("ConfirmMaxEventGacha", maxGachaTimes), 
				CallbackEventGacha, CallbackEventGacha, 1, maxGachaTimes);
    }
}