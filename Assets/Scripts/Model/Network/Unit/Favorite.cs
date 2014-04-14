
using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class UnitFavorite: ProtoManager {
	// req && rsp
	private bbproto.ReqUnitFavorite reqUnitFavorite;
	private bbproto.RspUnitFavorite rspUnitFavorite;
	// data
	private uint uniqueId;
	private EFavoriteAction action;
	
	public UnitFavorite() {
	}
	
	~UnitFavorite () {
	}
	
	public static void SendRequest(DataListener callBack, uint uniqueid, EFavoriteAction action) {
		UnitFavorite retireQuest = new UnitFavorite();
		retireQuest.uniqueId = uniqueid;
		retireQuest.action = action;
		retireQuest.OnRequest(null, callBack);
	}
	
	public override bool MakePacket() {
		Proto = Protocol.UNIT_FAVORITE;
		reqType = typeof(ReqUnitFavorite);
		rspType = typeof(RspUnitFavorite);
		
		reqUnitFavorite = new ReqUnitFavorite();
		reqUnitFavorite.header = new ProtoHeader();
		reqUnitFavorite.header.apiVer = Protocol.API_VERSION;
		reqUnitFavorite.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqUnitFavorite.unitUniqueId = this.uniqueId;
		reqUnitFavorite.action = this.action;
		
		ErrorMsg err = SerializeData(reqUnitFavorite); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}


