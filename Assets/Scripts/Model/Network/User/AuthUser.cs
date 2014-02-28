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

		Proto = "auth_user";
		reqType = typeof(ReqAuthUser);
		rspType = typeof(RspAuthUser);

		reqAuthUser = new ReqAuthUser ();
		reqAuthUser.header = new ProtoHeader ();
		reqAuthUser.header.apiVer = "1.0";
		reqAuthUser.terminal = new TerminalInfo ();
		reqAuthUser.terminal.uuid = "kory-abcdefg";

		ErrorMsg err = SerializeData (reqAuthUser); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspAuthUser = InstanceObj as bbproto.RspAuthUser;
		Debug.Log("authUser response userId:"+rspAuthUser.user.userId);
		if (rspAuthUser == null)
				return;

		//TODO: update localtime with servertime
		//localTime = rspAuthUser.serverTime

		//save to GlobalData
		if ( rspAuthUser.user != null ) {
			GlobalData.userInfo = new TUserInfo (rspAuthUser.user);
		}

		if (rspAuthUser.friends != null){
			GlobalData.friendList = new TFriendList (rspAuthUser.friends);
		}
		else {
			LogHelper.Log ("rsp.friends==null");
		}

		if (rspAuthUser.unitList != null) {
			foreach(UserUnit unit in rspAuthUser.unitList) {
				GlobalData.myUnitList.Add(unit.uniqueId, new TUserUnit(unit));
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

