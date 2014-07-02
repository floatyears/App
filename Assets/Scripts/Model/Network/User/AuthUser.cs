using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class AuthUser: ProtoManager {
    private bbproto.ReqAuthUser reqAuthUser;
    private bbproto.RspAuthUser rspAuthUser;
    private uint userId;
	private uint userSelectRole;
	private int appVersion;

    public uint UserSelectRole {
        set { userSelectRole = value; }
    }

    public AuthUser() {
    }

    ~AuthUser() {
    }

    public static void FirstLogin(uint selectRole,int version, DataListener callback) {
        AuthUser authUser = new AuthUser();
        authUser.UserSelectRole = selectRole;
		authUser.OnRequest(version, callback);
    }

    public override bool MakePacket() {
        LogHelper.Log("AuthUser.MakePacket()...");

        Proto = Protocol.AUTH_USER;
        reqType = typeof(ReqAuthUser);
        rspType = typeof(RspAuthUser);
		bool b = PlayerPrefs.HasKey (GameDataStore.USER_ID);
		this.userId = GameDataStore.Instance.GetUInt(GameDataStore.USER_ID);
        string uuid = GameDataStore.Instance.GetData(GameDataStore.UUID);
        if (userId == 0 && uuid.Length == 0) {
            uuid = System.Guid.NewGuid().ToString();
            GameDataStore.Instance.StoreData(GameDataStore.UUID, uuid);
            LogHelper.Log("New user first run, generate uuid: " + uuid);
        }
        else {
            LogHelper.Log("Exists userid:{0} uuid:{1} ", userId, uuid);
        }

        reqAuthUser = new ReqAuthUser();
        reqAuthUser.header = new ProtoHeader();
        reqAuthUser.header.apiVer = Protocol.API_VERSION;

        reqAuthUser.terminal = new TerminalInfo();
        reqAuthUser.header.userId = userId;
        reqAuthUser.terminal.uuid = uuid;
		reqAuthUser.selectRole = userSelectRole;
		reqAuthUser.appVersion = appVersion;

        ErrorMsg err = SerializeData(reqAuthUser); // save to Data for send out
		
        return err.Code == ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) { 
            LogHelper.Log("authUser response failed.");
            return;
        }
    }

    protected override void OnReceiveCommand(object data) {
//		LogHelper.Log ("OnReceiveCommand authUser...");
		if (data != null ) {
			appVersion = (int)data;
		}
        Send(); //send request to server
    }

}

