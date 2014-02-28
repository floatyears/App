using UnityEngine;
using System.Collections;
using bbproto;

public class AuthUser: ProtoManager {
	private bbproto.ReqAuthUser reqAuthUser;
	private bbproto.RspAuthUser rspAuthUser;

	public AuthUser(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	~AuthUser() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	public override bool MakePacket () {
		LogHelper.Log ("AuthUser.MakePacket()...");

		Proto = Proto = Protocol.AUTH_USER;
		reqType = typeof(ReqAuthUser);
		rspType = typeof(RspAuthUser);

		reqAuthUser = new ReqAuthUser ();
		reqAuthUser.header = new ProtoHeader ();
		reqAuthUser.header.apiVer = Protocol.API_VERSION;
		reqAuthUser.terminal = new TerminalInfo ();
		reqAuthUser.terminal.uuid = "5e654e3c-ac0d-49ed-93f4-bf51518fab26";//System.Guid.NewGuid().ToString();

		ErrorMsg err = SerializeData (reqAuthUser); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		LogHelper.Log("authUser response success:{0}",success);

		if (!success) { return; }

		rspAuthUser = InstanceObj as bbproto.RspAuthUser;
		if ( rspAuthUser == null ) {
			LogHelper.Log("authUser response rspAuthUser == null");
			 return;
		}

		//TODO: update localtime with servertime
		//localTime = rspAuthUser.serverTime

		//save to GlobalData
		if ( rspAuthUser.user != null ) {
			GlobalData.userInfo = new TUserInfo (rspAuthUser.user);
			LogHelper.Log("authUser response userId:"+rspAuthUser.user.userId);
		}else{
			LogHelper.Log("authUser response rspAuthUser.user == null");
		}

		if (rspAuthUser.friends != null) {
			LogHelper.Log ("rsp.friends have some friends.");
			GlobalData.friendList = new TFriendList (rspAuthUser.friends);
		}
		else {
			LogHelper.Log ("rsp.friends==null");
		}

		if (rspAuthUser.unitList != null) {
			foreach(UserUnit unit in rspAuthUser.unitList) {
				if ( !GlobalData.myUnitList.ContainsKey(unit.uniqueId) )
					GlobalData.myUnitList.Add(unit.uniqueId, new TUserUnit(unit));
				if ( !GlobalData.userUnitInfo.ContainsKey(unit.uniqueId) )
					GlobalData.userUnitInfo.Add( unit.uniqueId, new TUserUnit(unit));
			}
		}

		//send response to caller
		MsgCenter.Instance.Invoke (CommandEnum.RspAuthUser, null);
	}

	void OnReceiveCommand(object data) {
		LogHelper.Log ("OnReceiveCommand authUser...");
		Send (); //send request to server
	}

}

