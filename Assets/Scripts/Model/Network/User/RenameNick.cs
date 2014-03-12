using UnityEngine;
using System.Collections;
using bbproto;

public class RenameNick: ProtoManager {
    private bbproto.ReqRenameNick reqRenameNick;
    private bbproto.RspRenameNick rspRenameNick;
    private string newNickName;

    public string NewNickName {
        get {
            return newNickName;
        }
        set { newNickName = value; }
    }

    public RenameNick() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
    }

    ~RenameNick() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqRenameNick, OnReceiveCommand);
    }

    public override bool MakePacket() {

        Proto = Protocol.RENAME_NICK;
        reqType = typeof(ReqRenameNick);
        rspType = typeof(RspRenameNick);

        reqRenameNick = new ReqRenameNick();
        reqRenameNick.header = new ProtoHeader();
        reqRenameNick.header.apiVer = Protocol.API_VERSION;
        if (DataCenter.Instance.UserInfo != null && DataCenter.Instance.UserInfo.UserId > 0) {
            reqRenameNick.header.userId = DataCenter.Instance.UserInfo.UserId;
        }
        else {
            reqRenameNick.header.userId = GameDataStore.Instance.GetUInt(GameDataStore.USER_ID);
        }
        reqRenameNick.newNickName = newNickName;
		

        ErrorMsg err = SerializeData(reqRenameNick); // save to Data for send out
		
        return err.Code == (int)ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) {
            return;
        }
    }

    protected override void OnReceiveCommand(object data) {
        this.newNickName = data as string;

        LogHelper.Log("OnReceiveCommand rename to: {0}", newNickName);
        Send(); //send request to server
    }

    protected override void OnResponseEnd(object data) {
        if (data == null) {
            Debug.LogError("OnResponseEnd(), data == null");
            return;
        }
        //        Debug.LogError("Login Success : " + Time.realtimeSinceStartup);
        //        Debug.LogError("data=" + data);
        
        bbproto.RspRenameNick rsp = data as bbproto.RspRenameNick;
        errMsg.SetErrorMsg(rsp.header.code);
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            return;
        }
        if (rsp == null) {
            //                errMsg.SetErrorMsg(ErrorCode.ILLEGAL_PARAM, ErrorMsgType.RSP_AUTHUSER_NULL);
            LogHelper.Log("RenameNick OnResponseEnd() response rsp == null");
            return;
        }
    }


}

