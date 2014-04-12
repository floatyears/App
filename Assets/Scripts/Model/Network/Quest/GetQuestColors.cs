using System;
using UnityEngine;
using System.Collections;
using bbproto;


public class GetQuestColors: ProtoManager {
	// req && rsp
	private bbproto.ReqGetQuestColors reqGetQuestColors;
	private bbproto.RspGetQuestColors rspGetQuestColors;
	// state for req
	// data
	private uint questId;
	private bool colorCount;
	
	public GetQuestColors() {
	}
	
	~GetQuestColors () {
	}
	
	public static void SendRequest(DataListener callBack, uint questid, int count=0) {

		GetQuestColors getQuestColors = new GetQuestColors();

		getQuestColors.questId = questid;
		getQuestColors.colorCount = count;

		getQuestColors.OnRequest(null, callBack);

	}
	
	//Property: request server parameters
	//    public uint FriendUid { get { return friendUid; } set { friendUid = value; } }
	
	
	//make request packet==>TODO rename to request
	public override bool MakePacket() {
		Proto = Protocol.GET_QUEST_COLORS;
		reqType = typeof(ReqGetQuestColors);
		rspType = typeof(RspGetQuestColors);
		
		reqGetQuestColors = new ReqGetQuestColors();
		reqGetQuestColors.header = new ProtoHeader();
		reqGetQuestColors.header.apiVer = Protocol.API_VERSION;
		reqGetQuestColors.header.userId = DataCenter.Instance.UserInfo.UserId;
		
		//request params
		reqGetQuestColors.questId = this.questId;
		if ( colorCount > 0 )
			reqGetQuestColors.count = colorCount;
		
		ErrorMsg err = SerializeData(reqGetQuestColors); // save to Data for send out
		
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
	
}


