using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendController : ControllerBase {

	private static FriendController instance;
	
	public static FriendController Instance{
		get{
			if(instance == null)
				instance = new FriendController();
			return instance;
		}
	}

	public void AddFriend(NetCallback callBack, uint friendUid) {
		
		ReqAddFriend reqAddFriend = new ReqAddFriend();
		reqAddFriend.header = new ProtoHeader();
		reqAddFriend.header.apiVer = ServerConfig.API_VERSION;
		reqAddFriend.header.userId = DataCenter.Instance.UserInfo.userId;

		reqAddFriend.friendUid = friendUid;

		HttpRequestManager.Instance.SendHttpRequest (reqAddFriend, callBack, ProtocolNameEnum.RspAddFriend);
	
	}


	public void AcceptFriend(NetCallback callBack, uint friendUid) {

		ReqAcceptFriend reqAcceptFriend = new ReqAcceptFriend();
		reqAcceptFriend.header = new bbproto.ProtoHeader();
		reqAcceptFriend.header.apiVer = ServerConfig.API_VERSION;
		reqAcceptFriend.header.userId = DataCenter.Instance.UserInfo.userId;

		reqAcceptFriend.friendUid = friendUid;

		HttpRequestManager.Instance.SendHttpRequest (reqAcceptFriend,callBack , ProtocolNameEnum.RspAcceptFriend);

	}
	
	public void DelFriend(NetCallback callBack, List<uint> friendUids){


		ReqDelFriend reqDelFriend = new ReqDelFriend();
		reqDelFriend.header = new ProtoHeader();
		reqDelFriend.header.apiVer = ServerConfig.API_VERSION;
		reqDelFriend.header.userId = DataCenter.Instance.UserInfo.userId;
		
		//request params
		reqDelFriend.friendUid.Clear();
		reqDelFriend.friendUid.AddRange (friendUids);
		HttpRequestManager.Instance.SendHttpRequest (reqDelFriend, callBack, ProtocolNameEnum.RspDelFriend);
	}

	public void DelFriend(NetCallback callBack, params uint[] friendUids){

		ReqDelFriend reqDelFriend = new ReqDelFriend();
		reqDelFriend.header = new ProtoHeader();
		reqDelFriend.header.apiVer = ServerConfig.API_VERSION;
		reqDelFriend.header.userId = DataCenter.Instance.UserInfo.userId;
		
		//request params
		reqDelFriend.friendUid.Clear();
		reqDelFriend.friendUid.AddRange (friendUids);
		HttpRequestManager.Instance.SendHttpRequest (reqDelFriend, callBack, ProtocolNameEnum.RspDelFriend);
	}
	
	public void FindFriend(NetCallback callBack, uint friendUid) {
		
		ReqFindFriend reqFindFriend = new ReqFindFriend();
		reqFindFriend.header = new ProtoHeader();
		reqFindFriend.header.apiVer = ServerConfig.API_VERSION;
		reqFindFriend.header.userId = DataCenter.Instance.UserInfo.userId;
		
		//request params
		reqFindFriend.friendUid = friendUid;
		HttpRequestManager.Instance.SendHttpRequest (reqFindFriend, callBack, ProtocolNameEnum.RspFindFriend);
	}

	//make request packet==>TODO rename to request
	public void GetFriendList(NetCallback callBack,bool ToGetFriend, bool ToGetHelper) {
		
		ReqGetFriend reqGetFriend = new ReqGetFriend();
		reqGetFriend.header = new ProtoHeader();
		reqGetFriend.header.apiVer = ServerConfig.API_VERSION;
		reqGetFriend.header.userId = DataCenter.Instance.UserInfo.userId;
		
		//request params
		reqGetFriend.getFriend = ToGetFriend;
		reqGetFriend.getHelper = ToGetHelper;
		
		HttpRequestManager.Instance.SendHttpRequest (reqGetFriend, callBack, ProtocolNameEnum.RspGetFriend);
	}

	public void GetPremiumHelper(NetCallback callback, EUnitRace race, EUnitType type, int level, int premiumKind) {
		//request params
		ReqGetPremiumHelper reqGetPremiumHelper = new ReqGetPremiumHelper();
		reqGetPremiumHelper.header = new ProtoHeader();
		reqGetPremiumHelper.header.apiVer = ServerConfig.API_VERSION;
		reqGetPremiumHelper.header.userId = DataCenter.Instance.UserInfo.userId;
		
		reqGetPremiumHelper.race = race;
		reqGetPremiumHelper.type = type;
		reqGetPremiumHelper.level = level;
		reqGetPremiumHelper.premiumKind = premiumKind;

		HttpRequestManager.Instance.SendHttpRequest (reqGetPremiumHelper, callback, ProtocolNameEnum.RspGetPremiumHelper);
	}


}
