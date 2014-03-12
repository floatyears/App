using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class AuthUser: ProtoManager {
	private bbproto.ReqAuthUser reqAuthUser;
	private bbproto.RspAuthUser rspAuthUser;
	private uint userId;

	public AuthUser(){
//		MsgCenter.Instance.AddListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	~AuthUser() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	public override bool MakePacket () {
		LogHelper.Log ("AuthUser.MakePacket()...");


		
		Proto = Protocol.AUTH_USER;
		reqType = typeof(ReqAuthUser);
		rspType = typeof(RspAuthUser);

		this.userId = GameDataStore.Instance.GetUInt (GameDataStore.USER_ID);
		string uuid = GameDataStore.Instance.GetData (GameDataStore.UUID);
		if (userId == 0 && uuid.Length == 0) {
			uuid = System.Guid.NewGuid ().ToString ();
			GameDataStore.Instance.StoreData (GameDataStore.UUID, uuid);
			LogHelper.Log ("New user first run, generate uuid: " + uuid);
		} else {
			LogHelper.Log ("Exists userid:{0} uuid:{1} ", userId,  uuid);
		}

		reqAuthUser = new ReqAuthUser ();
		reqAuthUser.header = new ProtoHeader ();
		reqAuthUser.header.apiVer = Protocol.API_VERSION;

		reqAuthUser.terminal = new TerminalInfo ();
		reqAuthUser.header.userId = userId;
		reqAuthUser.terminal.uuid = uuid;

		ErrorMsg err = SerializeData (reqAuthUser); // save to Data for send out
		
		return err.Code == ErrorCode.SUCCESS;
	}

	public override void OnResponse (bool success) {
		if (!success) { 
			LogHelper.Log("authUser response failed.");
			return;
		}



	}

	protected override void OnReceiveCommand(object data) {
//		LogHelper.Log ("OnReceiveCommand authUser...");
		Send (); //send request to server
	}

}

