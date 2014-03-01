using UnityEngine;
using System.Collections;
using bbproto;

public class QuestFriend: ProtoManager {
	private bbproto.ReqQuestFriend reqQuestFriend;
	private bbproto.RspQuestFriend rspQuestFriend;
	private string newNickName;

	public QuestFriend(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqQuestFriend, OnReceiveCommand);
	}

	~QuestFriend() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqQuestFriend, OnReceiveCommand);
	}

	public override bool MakePacket () {

		Proto = Protocol.RENAME_NICK;
		reqType = typeof(ReqQuestFriend);
		rspType = typeof(RspQuestFriend);

		reqQuestFriend = new ReqQuestFriend ();
		reqQuestFriend.header = new ProtoHeader ();
		reqQuestFriend.header.apiVer = Protocol.API_VERSION;
		if (GlobalData.userInfo != null && GlobalData.userInfo.UserId > 0) {
			reqQuestFriend.header.userId = GlobalData.userInfo.UserId;
		} else {
			reqQuestFriend.header.userId = GameDataStore.Instance.GetUInt (GameDataStore.USER_ID);
		}
		reqQuestFriend.newNickName = newNickName;
		

		ErrorMsg err = SerializeData (reqQuestFriend); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspQuestFriend = InstanceObj as bbproto.RspQuestFriend;
		LogHelper.Log("rename response newNickName : "+rspQuestFriend.newNickName );

		//send response to caller
		bool renameSuccess = (rspQuestFriend.header.code == 0);
		if( renameSuccess && rspQuestFriend.newNickName != null)
			GlobalData.userInfo.NickName = rspQuestFriend.newNickName;
		MsgCenter.Instance.Invoke (CommandEnum.RspQuestFriend, renameSuccess);
	}

	void OnReceiveCommand(object data) {
		this.newNickName = data as string;

		LogHelper.Log ("OnReceiveCommand rename to: {0}", newNickName);
		Send (); //send request to server
	}

}

