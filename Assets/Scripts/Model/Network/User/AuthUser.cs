using UnityEngine;
using System.Collections;
using bbproto;

public class AuthUser: ProtoManager {
	private bbproto.ReqAuthUser reqAuthUser;
	
	public void GetData () { 
		Send (); 
	}

	public override bool MakePacket () {
		LogHelper.Log ("AuthUser.MakePacket()...");

		Proto = "auth_user";
		reqAuthUser = new ReqAuthUser ();
		InstanceType = reqAuthUser.GetType ();

		reqAuthUser.header = new ProtoHeader ();
		reqAuthUser.header.apiVer = "1.0";
		reqAuthUser.terminal = new TerminalInfo ();
		reqAuthUser.terminal.uuid = "kory-abcdefg";

		ErrorMsg err = SerializeData (reqAuthUser); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnReceiveFinish (bool success) {
		if (!success) {
			return;
		}

		bbproto.RspAuthUser rspAuthUser = InstanceObj as bbproto.RspAuthUser;
		LogHelper.Log("reponse userId:"+rspAuthUser.user.userId);
		LogHelper.Log("reponse:"+rspAuthUser.user);
	}

}

