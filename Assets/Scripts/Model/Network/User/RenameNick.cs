using UnityEngine;
using System.Collections;
using bbproto;

public class RenameNick: ProtoManager {
	private bbproto.ReqRenameNick reqRenameNick;
	private bbproto.RspRenameNick rspRenameNick;

	public RenameNick(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
	}

	~RenameNick() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
	}

	public override bool MakePacket () {
		LogHelper.Log ("RenameNick.MakePacket()...");

		Proto = "auth_user";
		reqType = typeof(ReqRenameNick);
		rspType = typeof(RspRenameNick);

		reqRenameNick = new ReqRenameNick ();
		reqRenameNick.header = new ProtoHeader ();
		reqRenameNick.header.apiVer = "1.0";
		reqRenameNick.newNickName = "newName";
		

		ErrorMsg err = SerializeData (reqRenameNick); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspRenameNick = InstanceObj as bbproto.RspRenameNick;
		//		LogHelper.Log("authUser response userId:"+rspRenameNick.header.userId);

		//send response to caller
		MsgCenter.Instance.Invoke (CommandEnum.RspRenameNick, null);
	}

	void OnReceiveCommand(object data) {
		LogHelper.Log ("OnReceiveCommand authUser...");
		Send (); //send request to server
	}

}

