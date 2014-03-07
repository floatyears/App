using UnityEngine;
using System.Collections;
using bbproto;

public class LevelUp: ProtoManager {
	private bbproto.ReqAuthUser reqLevelUp;
	private bbproto.RspAuthUser rspLevelUp;

	public LevelUp(){
//		MsgCenter.Instance.AddListener (CommandEnum.ReqLevelUp, OnReceiveCommand);
	}

	~LevelUp() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqLevelUp, OnReceiveCommand);
	}

	public override bool MakePacket () {
		LogHelper.Log ("LevelUp.MakePacket()...");

		Proto = "auth_user";
		reqType = typeof(ReqAuthUser);
		rspType = typeof(RspAuthUser);

		reqLevelUp = new ReqAuthUser ();
		reqLevelUp.header = new ProtoHeader ();
		reqLevelUp.header.apiVer = "1.0";

		ErrorMsg err = SerializeData (reqLevelUp); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

//		rspLevelUp = InstanceObj as bbproto.RspAuthUser;
//		LogHelper.Log("reponse userId:"+rspLevelUp.user.userId);


		//send response to caller
//		MsgCenter.Instance.Invoke (CommandEnum.RspLevelUp, rspLevelUp);
		OnResposeEnd (InstanceObj);
	}

	void OnReceiveCommand(object data) {
		Send (); //send request to server
	}

	int GetMaxExpByLv(int level) {
		return 0;
	}

	int GetRiseLevel(int curExp, int curLv, int gotExp, ref int nextExp) {
		int totalExp = gotExp;
		int riseLv = 0, curLvExp=0;
		
		nextExp = (GetMaxExpByLv( curLv ) - curExp);
		if (totalExp < nextExp) {
			nextExp -= totalExp;
			return 0;
		}
		
		totalExp -= nextExp;
		while (totalExp >= 0) {
			riseLv += 1;
			curLv += 1;
			
			nextExp = totalExp;
			curLvExp = GetMaxExpByLv( curLv );
			if ( curLvExp <= 0 ) { //reach max level
				nextExp = 0;
				break;
			}
			
			totalExp -= curLvExp;
		}
		
		return riseLv;
	}

	public override void OnRequest (object data, DataListener callback) {
		OnRequestBefoure (callback);
		OnReceiveCommand (data);
	}
}

