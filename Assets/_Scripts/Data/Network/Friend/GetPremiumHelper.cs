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
	private int premiumKind;

    public GetPremiumHelper() {
    }
    
    ~GetPremiumHelper() {
    }
    
	//premiumKind: 1= LevelUp 2= evolve 3=battle
	public static void SendRequest(DataListener callback, EUnitRace race, EUnitType type, int level, int premiumKind) {
        GetPremiumHelper req = new GetPremiumHelper();
		req.race = race;
		req.type = type;
		req.level = level;
		req.premiumKind = premiumKind;
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
		reqGetPremiumHelper.premiumKind = this.premiumKind;
        
        ErrorMsg err = SerializeData(reqGetPremiumHelper); // save to Data for send out
        
        return (err.Code == (int)ErrorCode.SUCCESS);
    }
}

