// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using bbproto;


public class AddFriend: ProtoManager {
    // req && rsp
    private bbproto.ReqAddFriend reqAddFriend;
    private bbproto.RspAddFriend rspAddFriend;
    // state for req
    private uint friendUid;
    // data
    private TFriendList friendList;
    
    public AddFriend() {
    }
    
    ~AddFriend () {
    }
    
    public static void SendRequest(ResponseCallback callBack, uint friendUid) {
        AddFriend addFriend = new AddFriend();
        addFriend.friendUid = friendUid;
        addFriend.OnRequest(null, callBack);
    }

    //Property: request server parameters
    public uint FriendUid { get { return friendUid; } set { friendUid = value; } }

    
    //make request packet==>TODO rename to request
    public override bool MakePacket() {
        Proto = Protocol.ADD_FRIEND;
        reqType = typeof(ReqAddFriend);
        rspType = typeof(RspAddFriend);
        
        reqAddFriend = new ReqAddFriend();
        reqAddFriend.header = new ProtoHeader();
        reqAddFriend.header.apiVer = Protocol.API_VERSION;
        reqAddFriend.header.userId = DataCenter.Instance.UserInfo.UserId;
        
        //request params
        reqAddFriend.friendUid = friendUid;

        ErrorMsg err = SerializeData(reqAddFriend); // save to Data for send out
        
        return (err.Code == (int)ErrorCode.SUCCESS);
    }

}

