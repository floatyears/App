using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class GetBonusList: ProtoManager {
	// req && rsp
	private bbproto.ReqBonusList reqBonusList;
	private bbproto.RspBonusList rspBonusList;
	// state for req
	// data
	public List<int> bonusId;
	
	public GetBonusList() {
	}
	
	~GetBonusList () {
	}
	
	public static void SendRequest(DataListener callBack) {
		
		GetBonusList req = new GetBonusList();
		req.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.GET_BONUS_LIST;
		reqType = typeof(ReqBonusList);
		rspType = typeof(RspBonusList);
		
		reqBonusList = new ReqBonusList();
		reqBonusList.header = new ProtoHeader();
		reqBonusList.header.apiVer = Protocol.API_VERSION;
		reqBonusList.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params

		ErrorMsg err = SerializeData(reqBonusList); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

