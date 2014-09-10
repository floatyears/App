using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class FinishUserGuide: ProtoManager {
	// req && rsp
	private bbproto.ReqFinishUserGuide reqFinishUserGuide;
	private bbproto.RspFinishUserGuide rspFinishUserGuide;
	// data
	private int step;
	
	public FinishUserGuide() {
	}
	
	~FinishUserGuide () {
	}
	
	public static void SendRequest(DataListener callBack, int step) {
		FinishUserGuide req = new FinishUserGuide();
		
		req.step = step;

		req.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.FINISH_USER_GUIDE;
		reqType = typeof(ReqFinishUserGuide);
		rspType = typeof(RspFinishUserGuide);
		
		reqFinishUserGuide = new ReqFinishUserGuide();
		reqFinishUserGuide.header = new ProtoHeader();
		reqFinishUserGuide.header.apiVer = Protocol.API_VERSION;
		reqFinishUserGuide.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqFinishUserGuide.step = this.step;

		ErrorMsg err = SerializeData(reqFinishUserGuide); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}


