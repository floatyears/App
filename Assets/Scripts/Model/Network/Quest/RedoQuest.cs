using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class RedoQuest: ProtoManager {
	// req && rsp
	private bbproto.ReqRedoQuest reqRedoQuest;
	private bbproto.RspRedoQuest rspRedoQuest;
	// data
	private uint questId;
	private int floorNo;
	
	public RedoQuest() {
	}
	
	~RedoQuest () {
	}
	
	public void SendRequest(DataListener callBack, uint questid, int floor) {
		RedoQuest redoQuest = new RedoQuest();

		redoQuest.questId = questid;
		redoQuest.floorNo = floor;

		redoQuest.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.REDO_QUEST;
		reqType = typeof(ReqRedoQuest);
		rspType = typeof(RspRedoQuest);
		
		reqRedoQuest = new ReqRedoQuest();
		reqRedoQuest.header = new ProtoHeader();
		reqRedoQuest.header.apiVer = Protocol.API_VERSION;
		reqRedoQuest.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqRedoQuest.questId = this.questId;
		reqRedoQuest.floor = this.floorNo;
		
		ErrorMsg err = SerializeData(reqRedoQuest); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}


