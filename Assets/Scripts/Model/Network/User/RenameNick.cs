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
		if (GlobalData.userInfo != null && GlobalData.userInfo.UserId > 0) {
			reqRenameNick.header.userId = GlobalData.userInfo.UserId;
		} else {
			reqRenameNick.header.userId = GameDataStore.Instance.GetUInt (GameDataStore.USER_ID);
		}
		reqRenameNick.newNickName = newNickName;
		

		ErrorMsg err = SerializeData (reqRenameNick); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspRenameNick = InstanceObj as bbproto.RspRenameNick;
		LogHelper.Log("rename response newNickName : "+rspRenameNick.newNickName );

		//send response to caller
		bool renameSuccess = (rspRenameNick.header.code == 0);
		if( renameSuccess && rspRenameNick.newNickName != null)
			GlobalData.userInfo.NickName = rspRenameNick.newNickName;
		MsgCenter.Instance.Invoke (CommandEnum.RspRenameNick, renameSuccess);
	}

	void OnReceiveCommand(object data) {
		this.newNickName = data as string;

		LogHelper.Log ("OnReceiveCommand rename to: {0}", newNickName);
		Send (); //send request to server
	}

}

