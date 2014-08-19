using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class UploadStat: ProtoManager {
	// req && rsp
	private bbproto.ReqUploadStat reqUploadStat;
	private bbproto.RspUploadStat rspUploadStat;
	// state for req
	// data
	public List<EventData> events;
	
	public UploadStat() {
	}
	
	~UploadStat () {
	}
	
	public static void SendRequest(DataListener callBack, List<EventData> events) {
		
		UploadStat req = new UploadStat();
		req.events = new List<EventData> ();
		req.events.AddRange( events );
		req.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.UPLOAD_STAT;
		reqType = typeof(ReqUploadStat);
		rspType = typeof(RspUploadStat);
		
		reqUploadStat = new ReqUploadStat();
		reqUploadStat.header = new ProtoHeader();
		reqUploadStat.header.apiVer = Protocol.API_VERSION;
		reqUploadStat.header.userId = DataCenter.Instance.UserInfo != null ? DataCenter.Instance.UserInfo.UserId : 0;
		
		//request params
		reqUploadStat.events.AddRange( this.events );
		
		ErrorMsg err = SerializeData(reqUploadStat); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

