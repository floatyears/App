using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class GetPremiumHelper: ProtoManager {
    // req && rsp
    private bbproto.ReqGetPremiumHelper reqGetPremiumHelper;
    private bbproto.RspGetPremiumHelper rspGetPremiumHelper;
    
	private EUnitRace race;
	private EUnitType type;
	private int level;

    public GetPremiumHelper() {
    }
    
    ~GetPremiumHelper() {
    }
    
    public static void SendRequest(DataListener callback, EUnitRace race, EUnitType type, int level) {
        GetPremiumHelper req = new GetPremiumHelper();
		req.race = race;
		req.type = type;
		req.level = level;
		req.OnRequest(null, callback);
    }
    
    public override bool MakePacket() {
		Proto = Protocol.GET_PREMIUM_HELPER;
        reqType = typeof(ReqGetPremiumHelper);
        rspType = typeof(RspGetPremiumHelper);
        
        //request params
		reqGetPremiumHelper = new ReqGetPremiumHelper();
		reqGetPremiumHelper.header = new ProtoHeader();
		reqGetPremiumHelper.header.apiVer = Protocol.API_VERSION;
		reqGetPremiumHelper.header.userId = DataCenter.Instance.UserInfo.UserId;

		reqGetPremiumHelper.race = this.race;
		reqGetPremiumHelper.type = this.type;
		reqGetPremiumHelper.level = this.level;
        
        ErrorMsg err = SerializeData(reqGetPremiumHelper); // save to Data for send out
        
        return (err.Code == (int)ErrorCode.SUCCESS);
    }
}

