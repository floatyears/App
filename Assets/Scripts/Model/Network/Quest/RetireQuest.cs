using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class RetireQuest: ProtoManager {
	// req && rsp
	private bbproto.ReqRetireQuest reqRetireQuest;
	private bbproto.RspRetireQuest rspRetireQuest;
	// state for req
	// data
	private uint questId;
	private bool gameOver;
	
	public RetireQuest() {
	}
	
	~RetireQuest () {
	}
	
	public static void SendRequest(DataListener callBack, uint questid, bool gameover = false) {

		RetireQuest retireQuest = new RetireQuest();

		retireQuest.questId = questid;
		retireQuest.gameOver = gameover;

		retireQuest.OnRequest(null, callBack);
	}

	//Property: request server parameters
	//    public uint FriendUid { get { return friendUid; } set { friendUid = value; } }
	
	
	//make request packet==>TODO rename to request
	public override bool MakePacket() {
		Proto = Protocol.RETIRE_QUEST;
		reqType = typeof(ReqRetireQuest);
		rspType = typeof(RspRetireQuest);
		
		reqRetireQuest = new ReqRetireQuest();
		reqRetireQuest.header = new ProtoHeader();
		reqRetireQuest.header.apiVer = Protocol.API_VERSION;
		reqRetireQuest.header.userId = DataCenter.Instance.UserInfo.UserId;

		//request params
		reqRetireQuest.questId = this.questId;
		reqRetireQuest.isGameOver = (this.gameOver ? 1 : 0);
		
		ErrorMsg err = SerializeData(reqRetireQuest); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

