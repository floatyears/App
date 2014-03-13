using UnityEngine;
using System.Collections;
using bbproto;


public class ChangeParty: ProtoManager {
    private bbproto.ReqChangeParty reqChangeParty;
    private bbproto.RspChangeParty rspChangeParty;
    private TPartyInfo partyInfo;

    public ChangeParty() {
//		MsgCenter.Instance.AddListener (CommandEnum.ReqChangeParty, OnReceiveCommand);
    }

    ~ChangeParty() {
//		MsgCenter.Instance.RemoveListener (CommandEnum.ReqChangeParty, OnReceiveCommand);
    }

    public override bool MakePacket() {
//		LogHelper.Log ("ChangeParty.MakePacket()...");

        Proto = Protocol.CHANGE_PARTY;
        reqType = typeof(ReqChangeParty);
        rspType = typeof(RspChangeParty);

        reqChangeParty = new ReqChangeParty();
        reqChangeParty.header = new ProtoHeader();
        reqChangeParty.header.apiVer = Protocol.API_VERSION;
        reqChangeParty.header.userId = DataCenter.Instance.UserInfo.UserId;
        reqChangeParty.party = partyInfo.Object;

        ErrorMsg err = SerializeData(reqChangeParty); // save to Data for send out
		
        return (err.Code == ErrorCode.SUCCESS);
    }

    public override void OnResponse(bool success) {
        if (!success) {
            return;
        }
    }

    protected override void OnReceiveCommand(object data) {
        partyInfo = data as TPartyInfo;
        if (partyInfo == null) {
            LogHelper.LogError("ChangeParty: Invalid param data.");
            return;
        }

        LogHelper.Log("OnReceiveCommand(ChangeParty)...");

        Send(); //send request to server
    }

}

