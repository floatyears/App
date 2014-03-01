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
		MsgCenter.Instance.AddListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	~AuthUser() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
	}

	public override bool MakePacket () {
		LogHelper.Log ("AuthUser.MakePacket()...");

		Proto = Proto = Protocol.AUTH_USER;
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
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		LogHelper.Log("authUser response success:{0}",success);

		if (!success) { return; }

		rspAuthUser = InstanceObj as bbproto.RspAuthUser;
		if ( rspAuthUser == null ) {
			LogHelper.Log("authUser response rspAuthUser == null");
			 return;
		}

		if (this.userId == 0) {
			userId = rspAuthUser.user.userId;
			LogHelper.Log("New user registeed, save userid:"+userId);
			GameDataStore.Instance.StoreData (GameDataStore.USER_ID, rspAuthUser.user.userId);
		}

		//TODO: update localtime with servertime
		//localTime = rspAuthUser.serverTime

		//save to GlobalData
		if (rspAuthUser.account != null) {
			GlobalData.accountInfo = new TAccountInfo (rspAuthUser.account);
		}

		if ( rspAuthUser.user != null ) {
			GlobalData.userInfo = new TUserInfo (rspAuthUser.user);
			if (rspAuthUser.evolveType != null) {
				GlobalData.userInfo.EvolveType = rspAuthUser.evolveType;
			}

			LogHelper.Log("authUser response userId:"+rspAuthUser.user.userId);
		}else{
			LogHelper.Log("authUser response rspAuthUser.user == null");
		}

		if (rspAuthUser.friends != null) {
			LogHelper.Log ("rsp.friends have {0} friends.", rspAuthUser.friends.Count);
			GlobalData.friends = new List<TFriendInfo> ();
			foreach ( FriendInfo fi in rspAuthUser.friends ) {
				TFriendInfo tfi = new TFriendInfo(fi);
				GlobalData.friends.Add( tfi );
			}
		}
		else {
			LogHelper.Log ("rsp.friends==null");
		}

		if (rspAuthUser.unitList != null) {
			foreach(UserUnit unit in rspAuthUser.unitList) {
				if ( !GlobalData.myUnitList.ContainsKey(unit.uniqueId) )
					GlobalData.myUnitList.Add(unit.uniqueId, new TUserUnit(unit));
				if ( !GlobalData.userUnitInfo.ContainsKey(unit.uniqueId) )
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

