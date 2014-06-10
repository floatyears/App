using UnityEngine;
using System.Collections;
using bbproto;

public class StartQuestParam {
    public uint stageId;
    public uint questId;
	public TFriendInfo helperUserUnit;
    public int currPartyId;
	public int startNew;
	public int isUserGuide;
}

public class StartQuest: ProtoManager {
    private bbproto.ReqStartQuest reqStartQuest;
    private bbproto.RspStartQuest rspStartQuest;
    private StartQuestParam questParam;

    public StartQuest() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
    }

    ~StartQuest() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
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
		reqStartQuest.helperUserId = questParam.helperUserUnit.UserId;
        reqStartQuest.currentParty = questParam.currPartyId;
		reqStartQuest.restartNew = questParam.startNew;
//		TUserUnit userunit = DataCenter.Instance.UserUnitList.Get(questParam.helperUserId, questParam.helperUniqueId);
//		Debug.LogError ("userunit : " + userunit);
//        if (userunit != null)
		reqStartQuest.helperUnit = questParam.helperUserUnit.UserUnit.Object;
		reqStartQuest.isUserGuide = questParam.isUserGuide;

//        LogHelper.Log("helperUserId:{0} currParty:{1} userunit:{2}", reqStartQuest.helperUserId, reqStartQuest.currentParty, userunit);

		

        ErrorMsg err = SerializeData(reqStartQuest); // save to Data for send out
		
        return err.Code == ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) {
            return;
        }

    }

    protected override void OnReceiveCommand(object data) {
        questParam = data as StartQuestParam;
        if (questParam == null) {
            LogHelper.Log("StartQuest: Invalid param data.");
            return;
        }

//        LogHelper.Log("OnReceiveCommand(StartQuest): stageId:{0} questId:{1} helperUserId:{2} helperUniqueId:{3} currParty:{4}",
//			questParam.stageId, questParam.questId, questParam.helperUserId, questParam.helperUniqueId, questParam.currPartyId);

        Send(); //send request to server
    }
}
