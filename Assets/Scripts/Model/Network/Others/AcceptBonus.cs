using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class AcceptBonus: ProtoManager {
	// req && rsp
	private bbproto.ReqAcceptBonus reqAcceptBonus;
	private bbproto.RspAcceptBonus rspAcceptBonus;
	// state for req
	// data
	public List<int> bonusId;
	
	public AcceptBonus() {
	}
	
	~AcceptBonus () {
	}
	
	public static void SendRequest(DataListener callBack, List<int> bonusId) {
		
		AcceptBonus req = new AcceptBonus();
		req.bonusId.AddRange( bonusId );
		req.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.ACCEPT_BONUS;
		reqType = typeof(ReqAcceptBonus);
		rspType = typeof(RspAcceptBonus);
		
		reqAcceptBonus = new ReqAcceptBonus();
		reqAcceptBonus.header = new ProtoHeader();
		reqAcceptBonus.header.apiVer = Protocol.API_VERSION;
		reqAcceptBonus.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqAcceptBonus.bonusId.AddRange( this.bonusId );
		
		ErrorMsg err = SerializeData(reqAcceptBonus); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

