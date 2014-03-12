using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class AuthUser: ProtoManager {
    private bbproto.ReqAuthUser reqAuthUser;
    private bbproto.RspAuthUser rspAuthUser;
    private uint userId;

    public AuthUser() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
    }

    ~AuthUser() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqAuthUser, OnReceiveCommand);
    }

    public override bool MakePacket() {
        LogHelper.Log("AuthUser.MakePacket()...");


		
        Proto = Protocol.AUTH_USER;
        reqType = typeof(ReqAuthUser);
        rspType = typeof(RspAuthUser);

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

        ErrorMsg err = SerializeData(reqAuthUser); // save to Data for send out
		
        return err.Code == (int)ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) { 
            LogHelper.Log("authUser response failed.");
            return;
        }


    }

    protected override void OnReceiveCommand(object data) {
//		LogHelper.Log ("OnReceiveCommand authUser...");
        Send(); //send request to server
    }

    protected override void OnResponseEnd(object data) {
        if (data == null) {
            Debug.LogError("OnResponseEnd(), data == null");
            return;
        }
//        Debug.LogError("Login Success : " + Time.realtimeSinceStartup);
//        Debug.LogError("data=" + data);

        bbproto.RspAuthUser rspAuthUser = data as bbproto.RspAuthUser;
//        Debug.LogError("rspAuthUser=" + rspAuthUser);
//        Debug.Log(string.Format("Auth User, OnResponseEnd, code = {0}", rspAuthUser.header.code));
        errMsg.SetErrorMsg(rspAuthUser.header.code);
        if (rspAuthUser.header.code != (int)ErrorCode.SUCCESS) {
            return;
        }
        if (rspAuthUser == null) {
            //                errMsg.SetErrorMsg(ErrorCode.ILLEGAL_PARAM, ErrorMsgType.RSP_AUTHUSER_NULL);
            LogHelper.Log("authUser response rspAuthUser == null");
            return;
        }
        
        //            errMsg.SetErrorMsg(ErrorCode.ILLEGAL_PARAM, ErrorMsgType.RSP_AUTHUSER_NULL);
        uint userId = GameDataStore.Instance.GetUInt(GameDataStore.USER_ID);
        if (userId == 0) {
            userId = rspAuthUser.user.userId;
            LogHelper.Log("New user registeed, save userid:" + userId);
            GameDataStore.Instance.StoreData(GameDataStore.USER_ID, rspAuthUser.user.userId);
        }
        
        //TODO: update localtime with servertime
        //localTime = rspAuthUser.serverTime
        
        //save to GlobalData
        if (rspAuthUser.account != null) {
            DataCenter.Instance.AccountInfo = new TAccountInfo(rspAuthUser.account);
        }
        
        if (rspAuthUser.user != null) {
            DataCenter.Instance.UserInfo = new TUserInfo(rspAuthUser.user);
            if (rspAuthUser.evolveType != null) {
                DataCenter.Instance.UserInfo.EvolveType = rspAuthUser.evolveType;
            }
            
            LogHelper.Log("authUser response userId:" + rspAuthUser.user.userId);
        }
        else {
            LogHelper.Log("authUser response rspAuthUser.user == null");
        }
        
        if (rspAuthUser.friends != null) {
            LogHelper.Log("rsp.friends have {0} friends.", rspAuthUser.friends.Count);
            DataCenter.Instance.SupportFriends = new List<TFriendInfo>();
            //              Debug.LogError(rspAuthUser.friends.Count);
            foreach (FriendInfo fi in rspAuthUser.friends) {
                TFriendInfo tfi = new TFriendInfo(fi);
                DataCenter.Instance.SupportFriends.Add(tfi);
            }
        }
        else {
            LogHelper.Log("rsp.friends==null");
        }
        
        if (rspAuthUser.unitList != null) {
            foreach (UserUnit unit in rspAuthUser.unitList) {
                DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
                DataCenter.Instance.UserUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
            }
            LogHelper.Log("rspAuthUser add to myUserUnit.count: {0}", rspAuthUser.unitList.Count);
        }
        
        if (rspAuthUser.party != null && rspAuthUser.party.partyList != null) {
            DataCenter.Instance.PartyInfo = new TPartyInfo(rspAuthUser.party);
            
            //TODO: replace ModelManager.GetData(UnitPartyInfo) with DataCenter.Instance.PartyInfo.CurrentParty
            ModelManager.Instance.SetData(ModelEnum.UnitPartyInfo, DataCenter.Instance.PartyInfo.CurrentParty);
        }
    }

}

