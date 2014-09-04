using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;


public class UserguideEvoUnit: ProtoManager {
	// req && rsp
	private bbproto.ReqUserGuideEvolveUnit reqAddEvolveUnit;
	private bbproto.RspUserGuideEvolveUnit rspBonusList;
	// state for req
	// data
	public List<int> bonusId;
	public  uint unitId;
	
	public UserguideEvoUnit() {
	}
	
	~UserguideEvoUnit () {
	}
	
	public static void SendRequest(DataListener callBack, uint unitId) {
		
		UserguideEvoUnit req = new UserguideEvoUnit();
		req.unitId = unitId;
		req.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.USERGUIDE_EVOLVE_UNIT;
		reqType = typeof(ReqUserGuideEvolveUnit);
		rspType = typeof(RspUserGuideEvolveUnit);
		
		reqAddEvolveUnit = new ReqUserGuideEvolveUnit();
		reqAddEvolveUnit.header = new ProtoHeader();
		reqAddEvolveUnit.header.apiVer = Protocol.API_VERSION;
		reqAddEvolveUnit.header.userId = DataCenter.Instance.UserInfo.UserId;

		//request params
		reqAddEvolveUnit.unitId = this.unitId;

		ErrorMsg err = SerializeData(reqAddEvolveUnit); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}

