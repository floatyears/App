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
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { 
			LogHelper.Log("authUser response failed.");
			return;
		}

		rspAuthUser = InstanceObj as bbproto.RspAuthUser;
		if ( rspAuthUser == null ) {
			LogHelper.Log("authUser response rspAuthUser == null");
			 return;
		}

		if (rspAuthUser.header.code != 0) {
			//TODO: showErrMsg()
			LogHelper.Log("rspAuthUser return error: {0} {1}", rspAuthUser.header.code,rspAuthUser.header.error);
			return;
		}
				

		if (this.userId == 0) {
			userId = rspAuthUser.user.userId;
			LogHelper.Log("New user registeed, save userid:"+userId);
			GameDataStore.Instance.StoreData (GameDataStore.USER_ID, rspAuthUser.user.userId);
		}

		if (rspAuthUser.party != null && rspAuthUser.party.partyList!=null) {
			TUnitParty currParty = new TUnitParty (rspAuthUser.party.partyList [rspAuthUser.party.currentParty]);
			ModelManager.Instance.SetData (ModelEnum.UnitPartyInfo, currParty);
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
				GlobalData.myUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
				GlobalData.userUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
				LogHelper.Log("rspAuthUser add userUnit.uniqueId:{0}",unit.uniqueId);
			}
		}

		//TODO: remove test code bellow
//		StartQuestParam p= new StartQuestParam();
//		p.currPartyId=0;
//		p.questId=1101;
//		p.stageId=11;
//		p.helperUserId=103;
//		p.helperUniqueId=2;
//		MsgCenter.Instance.Invoke (CommandEnum.ReqStartQuest, p);


		//send response to caller
		MsgCenter.Instance.Invoke (CommandEnum.RspAuthUser, null);
	}

	void OnReceiveCommand(object data) {
		LogHelper.Log ("OnReceiveCommand authUser...");
		Send (); //send request to server
	}

}

