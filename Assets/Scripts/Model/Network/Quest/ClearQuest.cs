using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ClearQuestParam {
    public uint questId;
    public int getMoney;
    public List<uint> getUnit = new List<uint>();
    public List<uint> hitGrid = new List<uint>();
}

public class TRspClearQuest {
    public int			rank;
    public int			exp;
    public int			money;			
    public int			friendPoint;
    public int			staminaNow;	
    public int			staminaMax;		
    public uint			staminaRecover;	
    public int			gotMoney;
    public int			gotExp;	
    public int			gotChip;
    public int			gotFriendPoint;
    public List<TUserUnit>		gotUnit;
}

public class ClearQuest: ProtoManager {
    private bbproto.ReqClearQuest reqClearQuest;
    private bbproto.RspClearQuest rspClearQuest;
    private ClearQuestParam questParam;

    public ClearQuest() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqClearQuest, OnReceiveCommand);
    }

    ~ClearQuest() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqClearQuest, OnReceiveCommand);
    }

    public ClearQuestParam Param {
        set { questParam = value;}
    }

    public override bool MakePacket() {
//		LogHelper.Log ("ClearQuest.MakePacket()...");

        Proto = Protocol.CLEAR_QUEST;
        reqType = typeof(ReqClearQuest);
        rspType = typeof(RspClearQuest);

        reqClearQuest = new ReqClearQuest();
        reqClearQuest.header = new ProtoHeader();
        reqClearQuest.header.apiVer = Protocol.API_VERSION;

        if (DataCenter.Instance.UserInfo != null)
            reqClearQuest.header.userId = DataCenter.Instance.UserInfo.UserId;


        reqClearQuest.questId = questParam.questId;
        reqClearQuest.getMoney = questParam.getMoney;
//		reqClearQuest.getUnit = new List<uint>();
        reqClearQuest.getUnit.AddRange(questParam.getUnit);
//		reqClearQuest.hitGrid = new List<uint>();
        reqClearQuest.hitGrid.AddRange(questParam.hitGrid);


        ErrorMsg err = SerializeData(reqClearQuest); // save to Data for send out
		
        return err.Code == (int)ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) {
            return;
        }

        if (InstanceObj != null) {
            rspClearQuest = InstanceObj as bbproto.RspClearQuest;
            //		LogHelper.Log("reponse userId:"+rspClearQuest.user.userId);
			
            DataCenter.Instance.UserInfo.StaminaNow = rspClearQuest.staminaNow;
            DataCenter.Instance.UserInfo.StaminaRecover = rspClearQuest.staminaRecover;

        }

        LogHelper.Log("rspClearQuest code:{0}, error:{1}", rspClearQuest.header.code, rspClearQuest.header.error);

    }

    protected override void OnResponseEnd(object data) {
        if (data != null) {
            TRspClearQuest cq = new TRspClearQuest();
            rspClearQuest = data as RspClearQuest;
            if (rspClearQuest.header.code != 0) { 
                Debug.LogError("Response info is error : " + rspClearQuest.header.error + " header code : " + rspClearQuest.header.code);
                base.OnResponseEnd(null);
                return;
            }
            Debug.LogError("RspClearQuest : " + rspClearQuest.rank + "  rspClearQuest.exp : " + rspClearQuest.exp + " rspClearQuest.money : " + rspClearQuest.money);
            cq.rank = rspClearQuest.rank;
            cq.exp = rspClearQuest.exp;
            cq.money = rspClearQuest.money;
            cq.friendPoint = rspClearQuest.friendPoint;
            cq.staminaNow = rspClearQuest.staminaNow;
            cq.staminaMax = rspClearQuest.staminaMax;
            cq.staminaRecover = rspClearQuest.staminaRecover;
			
            cq.gotExp = rspClearQuest.gotExp;
            cq.gotChip = rspClearQuest.gotChip;
            cq.gotFriendPoint = rspClearQuest.gotFriendPoint;
            foreach (UserUnit uu in rspClearQuest.gotUnit) {

                TUserUnit tuu = new TUserUnit(uu);
                Debug.LogError("tuu : " + tuu.ID);
                cq.gotUnit.Add(tuu);
            }

            base.OnResponseEnd(cq);

        }
        else {
            base.OnResponseEnd(null);
        }
    }

    protected override void OnReceiveCommand(object data) {
        questParam = data as ClearQuestParam;
//		Debug.LogError ("clear quest : " + questParam);
        if (questParam == null) {
            LogHelper.Log("ClearQuest: Invalid param data.");
            return;
        }
//		Debug.LogError ("OnReceiveCommand");
        LogHelper.Log("OnReceiveCommand(ClearQuest): questId:{0} getMoney:{1} getUnit.count:{2} hitGrid.count{3}",
		               questParam.questId, questParam.getMoney, questParam.getUnit.Count, questParam.hitGrid.Count);

        Send(); //send request to server
    }
}

