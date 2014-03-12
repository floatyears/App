using UnityEngine;
using System.Collections;
using bbproto;


public class GetFriendList: ProtoManager {
    // req && rsp
    private bbproto.ReqGetFriend reqGetFriend;
    private bbproto.RspGetFriend rspGetFriend;
    // state for req
    private bool bGetHelper = false;
    private bool bGetFriend = true;
    // data
    private TFriendList friendList;

    public GetFriendList() {
    }

    ~GetFriendList () {
    }

    public static void SendRequest(ResponseCallback callBack) {
        GetFriendList getFriends = new GetFriendList();
        getFriends.OnRequest(null, callBack);
    }


    //Property: request server parameters
    public bool ToGetHelper { get { return bGetHelper; } set { bGetHelper = value; } }
    public bool ToGetFriend { get { return bGetFriend; } set { bGetFriend = value; } }


    //make request packet==>TODO rename to request
    public override bool MakePacket() {
        Proto = Protocol.GET_FRIEND;
        reqType = typeof(ReqGetFriend);
        rspType = typeof(RspGetFriend);

        reqGetFriend = new ReqGetFriend();
        reqGetFriend.header = new ProtoHeader();
        reqGetFriend.header.apiVer = Protocol.API_VERSION;
        reqGetFriend.header.userId = DataCenter.Instance.UserInfo.UserId;

        //request params
        reqGetFriend.getFriend = ToGetFriend;
        reqGetFriend.getHelper = ToGetHelper;

        ErrorMsg err = SerializeData(reqGetFriend); // save to Data for send out
		
        return (err.Code == (int)ErrorCode.SUCCESS);
    }

    public void OnRspGetFriend(object data) {

    }  

    protected override void OnResponseEnd(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("TFriendList.Refresh() begin");
        LogHelper.Log(data);
        RspGetFriend rsp = data as RspGetFriend;
        errMsg.SetErrorMsg(rsp.header.code);

        FriendList inst = rsp.friends;
//        DataCenter.Instance.Friends
//        setNewInstance(inst);
//        assignFriendList();
    }
}

