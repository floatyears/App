using UnityEngine;
using System.Collections;
using bbproto;

public class QuestController : ControllerBase {

	private static QuestController instance;

	public static QuestController Instance{
		get{
			if(instance == null)
				instance = new QuestController();
			return instance;
		}
	}

	public void ClearQuest(ClearQuestParam questParam, NetCallback callback){
		ReqClearQuest reqClearQuest = new ReqClearQuest();
		reqClearQuest.header = new ProtoHeader();
		reqClearQuest.header.apiVer = ServerConfig.API_VERSION;
		
		if (DataCenter.Instance.UserData.UserInfo != null)
			reqClearQuest.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		reqClearQuest.questId = questParam.questID;
		reqClearQuest.getMoney = questParam.getMoney;
		reqClearQuest.getUnit.AddRange(questParam.getUnit);
		reqClearQuest.hitGrid.AddRange(questParam.hitGrid);

		reqClearQuest.copyType = questParam.copyType;
		reqClearQuest.totalHp = questParam.totalHp;
		reqClearQuest.leftHp = questParam.leftHp;

		HttpRequestManager.Instance.SendHttpRequest (reqClearQuest, data => {

			RspClearQuest rspClearQuest = data as RspClearQuest;
			
			
			DataCenter.Instance.UserData.UserInfo.staminaNow = rspClearQuest.staminaNow;
			DataCenter.Instance.UserData.UserInfo.staminaRecover = rspClearQuest.staminaRecover;
			
			TRspClearQuest cq = new TRspClearQuest();
			rspClearQuest = data as RspClearQuest;
			if (rspClearQuest.header.code != 0) { 
				Debug.LogError("Response info is error : " + rspClearQuest.header.error + " header code : " + rspClearQuest.header.code);
				callback(cq);
				return;
			}
			
			cq.rank = rspClearQuest.rank;
			cq.exp = rspClearQuest.exp;
			cq.money = rspClearQuest.money;
			cq.gotMoney = rspClearQuest.gotMoney;
			cq.friendPoint = rspClearQuest.friendPoint;
			cq.staminaNow = rspClearQuest.staminaNow;
			cq.staminaMax = rspClearQuest.staminaMax;
			cq.staminaRecover = rspClearQuest.staminaRecover;
			
			cq.gotExp = rspClearQuest.gotExp;
			cq.gotStone = rspClearQuest.gotStone;
			cq.gotFriendPoint = rspClearQuest.gotFriendPoint;
			cq.curStar = rspClearQuest.curStar;

			foreach (UserUnit uu in rspClearQuest.gotUnit) {
				DataCenter.Instance.UnitData.UserUnitList.AddMyUnit(uu);
				uu.userID = DataCenter.Instance.UserData.UserInfo.userId;
				cq.gotUnit.Add(uu);
			}
			callback(cq);
			LogHelper.Log("rspClearQuest code:{0}, error:{1}", rspClearQuest.header.code, rspClearQuest.header.error);
		}, ProtocolNameEnum.RspClearQuest);
	}

	public void GetQuestColors(NetCallback callBack, uint questid, int count=0) {
		ReqGetQuestColors reqGetQuestColors = new ReqGetQuestColors();
		reqGetQuestColors.header = new ProtoHeader();
		reqGetQuestColors.header.apiVer = ServerConfig.API_VERSION;
		reqGetQuestColors.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqGetQuestColors.questId = questid;
		if ( count > 0 )
			reqGetQuestColors.count = count;
		HttpRequestManager.Instance.SendHttpRequest (reqGetQuestColors, callBack, ProtocolNameEnum.RspGetQuestColors);
	}

	public void RedoQuest(NetCallback callBack, uint questid, int floor) {
		ReqRedoQuest reqRedoQuest = new ReqRedoQuest();
		reqRedoQuest.header = new ProtoHeader();
		reqRedoQuest.header.apiVer = ServerConfig.API_VERSION;
		reqRedoQuest.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqRedoQuest.questId = questid;
		reqRedoQuest.floor = floor;

		HttpRequestManager.Instance.SendHttpRequest (reqRedoQuest, data =>{
			RspRedoQuest rrq = data as RspRedoQuest;
			if (rrq == null) {
				return;	
			}
			
			if (rrq.header.code != 0) {
				//			Debug.LogError("rrq.header.code : " + rrq.header.code + rrq.header.error);
				TipsManager.Instance.ShowTipsLabel(rrq.header.code.ToString() , " : ", rrq.header.error);
				return;
			}
			
			callBack(data);
		}, ProtocolNameEnum.RspRedoQuest);
	}

	public void ResumeQuest(NetCallback callBack, uint questid) {
		ReqResumeQuest reqResumeQuest = new ReqResumeQuest();
		reqResumeQuest.header = new ProtoHeader();
		reqResumeQuest.header.apiVer = ServerConfig.API_VERSION;
		reqResumeQuest.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqResumeQuest.questId = questid;

		HttpRequestManager.Instance.SendHttpRequest (reqResumeQuest, callBack, ProtocolNameEnum.RspResumeQuest);
	}

	public void RetireQuest(NetCallback callBack, uint questid, bool gameover = false) {
		ReqRetireQuest reqRetireQuest = new ReqRetireQuest();
		reqRetireQuest.header = new ProtoHeader();
		reqRetireQuest.header.apiVer = ServerConfig.API_VERSION;
		reqRetireQuest.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqRetireQuest.questId = questid;
		reqRetireQuest.isGameOver = (gameover ? 1 : 0);

		HttpRequestManager.Instance.SendHttpRequest (reqRetireQuest, callBack, ProtocolNameEnum.RspRetireQuest);
	}

	public void StartQuest(StartQuestParam questParam, NetCallback callback) {
		ReqStartQuest reqStartQuest = new ReqStartQuest();
		reqStartQuest.header = new ProtoHeader();
		reqStartQuest.header.apiVer = ServerConfig.API_VERSION;
		
		if (DataCenter.Instance.UserData.UserInfo != null)
			reqStartQuest.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		
		reqStartQuest.stageId = questParam.stageId;
		reqStartQuest.questId = questParam.questId;
		if(questParam.helperUserUnit != null)
			reqStartQuest.helperUserId = questParam.helperUserUnit.userId;
		reqStartQuest.currentParty = questParam.currPartyId;
		reqStartQuest.restartNew = questParam.startNew;
		//		TUserUnit userunit = DataCenter.Instance.UnitData.UserUnitList.Get(questParam.helperUserId, questParam.helperUniqueId);
		//		Debug.LogError ("userunit : " + userunit);
		//        if (userunit != null)
		if(questParam.helperUserUnit != null)
			reqStartQuest.helperUnit = questParam.helperUserUnit.UserUnit;
		reqStartQuest.isUserGuide = questParam.isUserGuide;

		reqStartQuest.copyType = questParam.copyType;  //普通副本 or 精英副本

		HttpRequestManager.Instance.SendHttpRequest (reqStartQuest, callback, ProtocolNameEnum.RspStartQuest);
	}


	public void AcceptStarBonus(NetCallback callBack, uint stageId, ECopyType copyType) {
		ReqAcceptStarBonus req = new ReqAcceptStarBonus();

		req.header = new ProtoHeader();
		req.header.apiVer = ServerConfig.API_VERSION;
		req.header.userId = DataCenter.Instance.UserData.UserInfo.userId;

		//request params
		req.copyType = copyType;
		req.stageId = stageId;

		Debug.Log("req.copyType:"+copyType+" req.stageId:"+req.stageId);

		HttpRequestManager.Instance.SendHttpRequest (req, callBack, ProtocolNameEnum.RspAcceptStarBonus);
	}
}
