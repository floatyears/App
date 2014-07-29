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
using System.Collections.Generic;
using bbproto;


public class DelFriend: ProtoManager
{
	// req && rsp
	private bbproto.ReqDelFriend reqDelFriend;
	private bbproto.RspDelFriend rspDelFriend;
	// state for req
	private List<uint> friendUids;
	// data
	private TFriendList friendList;
    
	public DelFriend()
	{
	}
    
	~DelFriend ()
	{
	}
    
	public static void SendRequest(DataListener callBack, List<uint> friendUids)
	{
		DelFriend delFriend = new DelFriend();
		delFriend.friendUids = friendUids;
		delFriend.OnRequest(null, callBack);
	}

	public static void SendRequest(DataListener callBack, params uint[] friendUids)
	{
		DelFriend delFriend = new DelFriend();
		List <uint> friendUidList = new List<uint>();
		friendUidList.AddRange(friendUids);
		delFriend.friendUids = friendUidList;
		delFriend.OnRequest(null, callBack);
	}
    
	//Property: request server parameters
	public List<uint> FriendUids { get { return friendUids; } set { friendUids = value; } }
    
    
	//make request packet==>TODO rename to request
	public override bool MakePacket()
	{
		Proto = Protocol.DEL_FRIEND;
		reqType = typeof(ReqDelFriend);
		rspType = typeof(RspDelFriend);
        
		reqDelFriend = new ReqDelFriend();
		reqDelFriend.header = new ProtoHeader();
		reqDelFriend.header.apiVer = Protocol.API_VERSION;
		reqDelFriend.header.userId = DataCenter.Instance.UserInfo.UserId;
        
		//request params
		reqDelFriend.friendUid.Clear();
		for (int i = 0; i < friendUids.Count; i++)
		{
			reqDelFriend.friendUid.Add(friendUids [i]);
		}
		Debug.Log("DelFriend() request, del friend count = " + reqDelFriend.friendUid.Count);
//        reqDelFriend.friendUid = friendUids;
        
		ErrorMsg err = SerializeData(reqDelFriend); // save to Data for send out
        
		return (err.Code == (int)ErrorCode.SUCCESS);
	}
    
}
