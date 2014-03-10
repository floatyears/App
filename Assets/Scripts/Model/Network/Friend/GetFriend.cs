using UnityEngine;
using System.Collections;
using bbproto;


public class GetFriend: ProtoManager {
	private bbproto.ReqGetFriend reqGetFriend;
	private bbproto.RspGetFriend rspGetFriend;
	private bool bGetHelper = false;
	private bool bGetFriend = false;

	public GetFriend() {}
	~GetFriend() {}


	//Property: request server parameters
	public bool ToGetHelper { get { return bGetHelper; } set { bGetHelper = value; } }
	public bool ToGetFriend { get { return bGetFriend; } set { bGetFriend = value; } }


	//make request packet
	public override bool MakePacket () {

		Proto = Protocol.GET_FRIEND;
		reqType = typeof(ReqGetFriend);
		rspType = typeof(RspGetFriend);

		reqGetFriend = new ReqGetFriend ();
		reqGetFriend.header = new ProtoHeader ();
		reqGetFriend.header.apiVer = Protocol.API_VERSION;
		reqGetFriend.header.userId = GlobalData.userInfo.UserId;

		//request params
		reqGetFriend.getFriend = ToGetFriend;
		reqGetFriend.getHelper = ToGetHelper;

		ErrorMsg err = SerializeData (reqGetFriend); // save to Data for send out
		
		return (err.Code == ErrorCode.Succeed);
	}

}

