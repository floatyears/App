using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class ResumeQuest: ProtoManager {
	// req && rsp
	private bbproto.ReqResumeQuest reqResumeQuest;
	private bbproto.RspResumeQuest rspResumeQuest;
	// data
	private uint questId;

	public ResumeQuest() {
	}
	
	~ResumeQuest () {
	}
	
	public static void SendRequest(DataListener callBack, uint questid) {

		ResumeQuest resumeQuest = new ResumeQuest();

		resumeQuest.questId = questid;

		resumeQuest.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.RESUME_QUEST;
		reqType = typeof(ReqResumeQuest);
		rspType = typeof(RspResumeQuest);
		
		reqResumeQuest = new ReqResumeQuest();
		reqResumeQuest.header = new ProtoHeader();
		reqResumeQuest.header.apiVer = Protocol.API_VERSION;
		reqResumeQuest.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqResumeQuest.questId = this.questId;

		ErrorMsg err = SerializeData(reqResumeQuest); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

