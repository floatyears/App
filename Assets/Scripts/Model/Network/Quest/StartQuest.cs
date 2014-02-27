using UnityEngine;
using System.Collections;
using bbproto;

public class StartQuest: ProtoManager {
	private bbproto.ReqStartQuest reqStartQuest;
	private bbproto.RspStartQuest rspStartQuest;

	public StartQuest(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
	}

	~StartQuest() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqStartQuest, OnReceiveCommand);
	}

	public override bool MakePacket () {
//		LogHelper.Log ("StartQuest.MakePacket()...");

		Proto = "start_quest";
		reqType = typeof(ReqStartQuest);
		rspType = typeof(RspStartQuest);

		reqStartQuest = new ReqStartQuest ();
		reqStartQuest.header = new ProtoHeader ();
		reqStartQuest.header.apiVer = "1.0";
		reqStartQuest.header.userId = 101; //read userid from db

		reqStartQuest.stageId = 11;
		reqStartQuest.questId = 1101;
		reqStartQuest.helperUserId = 103;
		reqStartQuest.currentParty = 0;

		if ( GlobalData.userUnitInfo.ContainsKey( reqStartQuest.helperUserId) )
			reqStartQuest.helperUnit = GlobalData.userUnitInfo [reqStartQuest.helperUserId].Object;

		ErrorMsg err = SerializeData (reqStartQuest); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspStartQuest = InstanceObj as bbproto.RspStartQuest;
//		LogHelper.Log("reponse userId:"+rspStartQuest.user.userId);


		//send response to caller
		MsgCenter.Instance.Invoke (CommandEnum.RspStartQuest, null);
	}

	void OnReceiveCommand(object data) {
		Send (); //send request to server
	}

}

