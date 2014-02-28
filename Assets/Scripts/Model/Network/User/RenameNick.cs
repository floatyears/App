using UnityEngine;
using System.Collections;
using bbproto;

public class RenameNick: ProtoManager {
	private bbproto.ReqRenameNick reqRenameNick;
	private bbproto.RspRenameNick rspRenameNick;
	private string newNickName;

	public RenameNick(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
	}

	~RenameNick() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
	}

	public override bool MakePacket () {

		Proto = Protocol.RENAME_NICK;
		reqType = typeof(ReqRenameNick);
		rspType = typeof(RspRenameNick);

		reqRenameNick = new ReqRenameNick ();
		reqRenameNick.header = new ProtoHeader ();
		reqRenameNick.header.apiVer = Protocol.API_VERSION;
		reqRenameNick.header.userId = 103;
		reqRenameNick.newNickName = newNickName;
		

		ErrorMsg err = SerializeData (reqRenameNick); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspRenameNick = InstanceObj as bbproto.RspRenameNick;
		//		LogHelper.Log("authUser response userId:"+rspRenameNick.header.userId);

		//send response to caller
		bool renameSuccess = (rspRenameNick.header.code == 0);
		MsgCenter.Instance.Invoke (CommandEnum.RspRenameNick, renameSuccess);
	}

	void OnReceiveCommand(object data) {
		this.newNickName = data as string;

		LogHelper.Log ("OnReceiveCommand rename to: {0}", newNickName);
		Send (); //send request to server
	}

}

