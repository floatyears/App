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
		LogHelper.Log("reponse userId:"+rspAuthUser.user.userId);
		LogHelper.Log("reponse rank:"+rspAuthUser.user.rank);
		LogHelper.Log("reponse staminaNow:"+rspAuthUser.user.staminaNow);
		LogHelper.Log("reponse staminaMax:"+rspAuthUser.user.staminaMax);
		LogHelper.Log("reponse staminaRecover:"+rspAuthUser.user.staminaRecover);

		//save to GlobalData
		GlobalData.userInfo = new TUserInfo (rspAuthUser.user);

		//send response to caller
		MsgCenter.Instance.Invoke (CommandEnum.RspAuthUser, rspAuthUser);
	}

	void OnReceiveCommand(object data) {
		Send (); //send request to server
	}

}

