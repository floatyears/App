using UnityEngine;
using System.Collections;
using bbproto;

public class StartQuestParam {
    public uint stageId;
    public uint questId;
    public uint helperUserId;
    public uint helperUniqueId;
    public int currPartyId;
}

public class StartQuest: ProtoManager {
    private bbproto.ReqStartQuest reqStartQuest;
    private bbproto.RspStartQuest rsp;
    private StartQuestParam questParam;

    public StartQuest() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
    }

    ~StartQuest() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
    }


    public StartQuestParam Param {
        set { questParam = value;}
    }
    public override bool MakePacket() {
//		LogHelper.Log ("StartQuest.MakePacket()...");

        Proto = Protocol.START_QUEST;
        reqType = typeof(ReqStartQuest);
        rspType = typeof(RspStartQuest);

        reqStartQuest = new ReqStartQuest();
        reqStartQuest.header = new ProtoHeader();
        reqStartQuest.header.apiVer = Protocol.API_VERSION;

        if (DataCenter.Instance.UserInfo != null)
            reqStartQuest.header.userId = DataCenter.Instance.UserInfo.UserId;


        reqStartQuest.stageId = questParam.stageId;
        reqStartQuest.questId = questParam.questId;
        reqStartQuest.helperUserId = questParam.helperUserId;
        reqStartQuest.currentParty = 0;//questParam.currPartyId;
        TUserUnit userunit = DataCenter.Instance.UserUnitList.GetMyUnit(questParam.helperUniqueId);
        if (userunit != null)
            reqStartQuest.helperUnit = userunit.Object;

        LogHelper.Log("helperUserId:{0} currParty:{1} userunit:{2}", reqStartQuest.helperUserId, reqStartQuest.currentParty, userunit);

        ErrorMsg err = SerializeData(reqStartQuest); // save to Data for send out
		
        return err.Code == (int)ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) {
            return;
        }

    }
    protected override void OnResponseEnd(object data) {
        if (data == null) {
            Debug.LogError("OnResponseEnd(), data == null");
            return;
        }
        //        Debug.LogError("Login Success : " + Time.realtimeSinceStartup);
        //        Debug.LogError("data=" + data);
        
        bbproto.RspStartQuest rsp = data as bbproto.RspStartQuest;
        errMsg.SetErrorMsg(rsp.header.code);
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            return;
        }
        if (rsp == null) {
            //                errMsg.SetErrorMsg(ErrorCode.ILLEGAL_PARAM, ErrorMsgType.RSP_AUTHUSER_NULL);
            LogHelper.Log("RspChangeParty OnResponseEnd() response rsp == null");
            return;
        }

        TQuestDungeonData tqdd = null;
        if (rsp.header.code == 0 && rsp.dungeonData != null) {
            
            DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
            DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
            
            LogHelper.Log("rsp code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            
            tqdd = new TQuestDungeonData(rsp.dungeonData);
            
            ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
        }
        
        if (data == null || tqdd == null) {
            Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
            //TODO: show failed window for user to retry
            return;
        }
    }

    protected override void OnReceiveCommand(object data) {
        questParam = data as StartQuestParam;
        if (questParam == null) {
            LogHelper.Log("StartQuest: Invalid param data.");
            return;
        }

        LogHelper.Log("OnReceiveCommand(StartQuest): stageId:{0} questId:{1} helperUserId:{2} helperUniqueId:{3} currParty:{4}",
			questParam.stageId, questParam.questId, questParam.helperUserId, questParam.helperUniqueId, questParam.currPartyId);

        Send(); //send request to server
    }
}
