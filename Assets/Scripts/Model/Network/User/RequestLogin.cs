using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class RequestLoginToServer {
	public static void Login(){
		INetBase netBase = new AuthUser();
		netBase.OnRequest(null, LoginSuccess);
	}
	
	static RspAuthUser rspAuthUser;
	public static void LoginSuccess(object data) {
		if (data != null) {
			rspAuthUser = data as bbproto.RspAuthUser;
			if (rspAuthUser == null) {
				Debug.LogError("authUser response rspAuthUser == null");
				return;
			}
			
			if (rspAuthUser.header.code != 0) {
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspAuthUser.header.code);
				Debug.LogError("rspAuthUser return code: "+rspAuthUser.header.code+" error:" + rspAuthUser.header.error);
				return;
			}
			
			uint userId = rspAuthUser.user.userId;
			
			if (rspAuthUser.isNewUser == 1) {
				LogHelper.Log("New user registeed, save userid:" + userId);
				GameDataStore.Instance.StoreData(GameDataStore.USER_ID, rspAuthUser.user.userId);
			}
			
			//TODO: update localtime with servertime
			//localTime = rspAuthUser.serverTime
			
			//save to GlobalData
			GameTimer.GetInstance().InitDateTime(rspAuthUser.serverTime);
			
			if (rspAuthUser.account != null) {
				DataCenter.Instance.AccountInfo = new TAccountInfo(rspAuthUser.account);
			}
			
			if (rspAuthUser.user != null) {
				Debug.Log("authUser response userId:" + rspAuthUser.user.userId);
				
				DataCenter.Instance.UserInfo = new TUserInfo(rspAuthUser.user);
				if (rspAuthUser.evolveType != null) {
					DataCenter.Instance.UserInfo.EvolveType = rspAuthUser.evolveType;
				}
			}
			else {
				Debug.LogError("authUser response rspAuthUser.user == null");
			}
			
			if (rspAuthUser.friends != null) {
				List<TFriendInfo> supportFriends = new List<TFriendInfo>();
				foreach (FriendInfo fi in rspAuthUser.friends) {
					TFriendInfo tfi = new TFriendInfo(fi);
					supportFriends.Add(tfi);
					DataCenter.Instance.UserUnitList.Add(tfi.UserId, tfi.UserUnit.ID, tfi.UserUnit);
				}
				DataCenter.Instance.SupportFriends = supportFriends;
			}
			else {
				Debug.LogError("rsp.friends==null");
			}
			
			DataCenter.Instance.EventStageList = new List<TStageInfo>();
			if (rspAuthUser.eventList != null) {
				foreach (StageInfo stage in rspAuthUser.eventList) {
					TStageInfo tsi = new TStageInfo(stage);
					DataCenter.Instance.EventStageList.Add(tsi);
				}
			}
			
			if (rspAuthUser.unitList != null) {
				foreach (UserUnit unit in rspAuthUser.unitList) {
					//					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId,unit));
					DataCenter.Instance.UserUnitList.Add(userId, unit.uniqueId, TUserUnit.GetUserUnit(userId, unit));
				}
				LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
			}
			
			if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
				DataCenter.Instance.PartyInfo = new TPartyInfo(rspAuthUser.party);
				//TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.PartyInfo.CurrentParty
				ModelManager.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.PartyInfo.CurrentParty);
			}
			
			if (rspAuthUser.questClear != null) {
				DataCenter.Instance.QuestClearInfo = new TQuestClearInfo(rspAuthUser.questClear);
			}
			
			DataCenter.Instance.CatalogInfo = new TUnitCatalog(rspAuthUser.meetUnitFlag, rspAuthUser.haveUnitFlag);
			
			if( rspAuthUser.notice != null){
				DataCenter.Instance.NoticeInfo = new TNoticeInfo(rspAuthUser.notice);
			}
			
			if( rspAuthUser.login != null){
				DataCenter.Instance.LoginInfo = new TLoginInfo(rspAuthUser.login);
			}
			NoviceGuideStepEntityManager.InitGuideStage(rspAuthUser.userGuideStep);
			
			
		}
	}
}