using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class GetServerTime: ProtoManager {
	// req && rsp
	private bbproto.ReqGetServerTime reqGetServerTime;
	private bbproto.RspGetServerTime rspGetServerTime;
	// state for req
	// data

	public GetServerTime() {
	}
	
	~GetServerTime () {
	}
	
	public static void SendRequest(DataListener callBack, uint questid, bool gameover=false) {

		GetServerTime req = new GetServerTime();
		req.OnRequest(null, callBack);
	}

	public override bool MakePacket() {
		Proto = Protocol.GET_SERVER_TIME;
		reqType = typeof(ReqGetServerTime);
		rspType = typeof(RspGetServerTime);
		
		reqGetServerTime = new ReqGetServerTime();
		reqGetServerTime.header = new ProtoHeader();
		reqGetServerTime.header.apiVer = Protocol.API_VERSION;
		reqGetServerTime.header.userId = DataCenter.Instance.UserInfo.UserId;

		//request params

		ErrorMsg err = SerializeData(reqGetServerTime); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

