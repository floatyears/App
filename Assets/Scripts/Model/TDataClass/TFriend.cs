using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TFriendInfo : ProtobufDataBase {
    private FriendInfo	instance;
    public TFriendInfo(FriendInfo inst) : base (inst) { 
        instance = inst;
        if (instance.unit != null) {
            unit = new TUserUnit(instance.unit);
        }
    }

    private TUserUnit		unit;


    //// property ////
    public	uint			UserId { get { return instance.userId; } }
    public	string			NickName { get { return instance.nickName; } }
    public	int				Rank 	{ get { return instance.rank; } }
    public	uint			LastPlayTime { get { return instance.lastPlayTime; } }
    public	EFriendState	FriendState { get { return instance.friendState; } }
    public	uint			FriendStateUpdate { get { return instance.friendStateUpdate; } }
    public	int				FriendPoint { get { return instance.friendPoint; } }

    public	TUserUnit		UserUnit { get { return unit; } }
}

public class TFriendList : ProtobufDataBase {
    private FriendList	instance;

    private List<TFriendInfo> helper;
    private List<TFriendInfo> friend;
    private List<TFriendInfo> friendIn;
    private List<TFriendInfo> friendOut;

    private TUserInfo searchResult;
    private ErrorMsg errMsg;
	
    //// property ////
    public List<TFriendInfo> Helper { get { return helper; } }
    public List<TFriendInfo> Friend { get { return friend; } }
    public List<TFriendInfo> FriendIn { get { return friendIn; } }
    public List<TFriendInfo> FriendOut { get { return friendOut; } }
    public TUserInfo SearchResult { get { return searchResult; } }
    public ErrorMsg ErrMsg { get { return errMsg; } }

    //constructor
    public TFriendList(FriendList inst) : base (inst) { 
        RefreshFriendList(inst);
    }
//    //constructor
//    public TFriendList() : base () { 
//        RefreshFriendList(inst);
//    }

    public void RefreshFriendList(FriendList inst) {
        LogHelper.Log("TFriendList () inst is " + inst);
        setNewInstance(inst);
        assignFriendList();
    }
    #region outter funcs
    public void GetFriends() {
        getFriends();
    }
//
//
//    public void SendFriendApplication(uint friendUid) {
//        addFriend(friendUid);
//    }
//
//    public void RemoveFriend(uint friendUid) {
//        delFriend(friendUid);
//    }
//
//    public void SearchFriend(uint friendUid) {
//        findFriend(friendUid);
//    }
//
//    public void RefuseFriendApplication(uint friendUid) {
//        delFriend(friendUid);
//    }
//
//    public void DelFriendApplication(uint friendUid) {
//        delFriend(friendUid);
//    }
//
//    public void AcceptFriendApplication(uint friendUid) {
//        delFriend(friendUid);
//    }

    #endregion
    public void OnRspGetFriend(object data) {

//        if (data == null)
//            return;
//        
//        LogHelper.Log("TFriendList.Refresh() begin");
//        LogHelper.Log(data);
//        RspGetFriend rsp = data as RspGetFriend;
//
//        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
//            LogHelper.Log("RspGetFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
//            return;
//        }
//
//        FriendList inst = rsp.friends;
//        setNewInstance(inst);
//        assignFriendList();
    }  

    public void OnRspAddFriend(object data) {
        if (data == null)
            return;

        LogHelper.Log("TFriendList.OnRspAddFriend() begin");
        LogHelper.Log(data);
        RspAddFriend rsp = data as RspAddFriend;

        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("RspAddFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }

        // TODO
        if (RspFriendsRefreshedList(data)) {
            LogHelper.Log("OnRspAddFriend(), do refresh list");
//            FriendList inst = rsp.friends;
//            setNewInstance(inst);
//            assignFriendList();
        }
        else {
            LogHelper.Log("OnRspAddFriend(), not refresh list");
        }
    }

    public void OnRspDelFriend(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("TFriendList.OnRspDelFriend() begin");
        LogHelper.Log(data);
        RspDelFriend rsp = data as RspDelFriend;
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("OnRspDelFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        // TODO
        if (RspFriendsRefreshedList(data)) {
            LogHelper.Log("OnRspDelFriend(), do refresh list");
            //            FriendList inst = rsp.friends;
            //            setNewInstance(inst);
            //            assignFriendList();
        }
        else {
            LogHelper.Log("OnRspDelFriend(), not refresh list");
        }
    }

    public void OnRspAcceptFriend(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("TFriendList.OnRspAcceptFriend() begin");
        LogHelper.Log(data);
        RspAcceptFriend rsp = data as RspAcceptFriend;
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("OnRspAcceptFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
            return;
        }
        
        // TODO
        if (RspFriendsRefreshedList(data)) {
            LogHelper.Log("OnRspAcceptFriend(), do refresh list");
            //            FriendList inst = rsp.friends;
            //            setNewInstance(inst);
            //            assignFriendList();
        }
        else {
            LogHelper.Log("OnRspAcceptFriend(), not refresh list");
        }
    }
    
    public void OnRspFindFriend(object data) {
        if (data == null)
            return;
        
        LogHelper.Log("TFriendList.OnRspDelFriend() begin");
        LogHelper.Log(data);
        RspFindFriend rsp = data as RspFindFriend;
        // first set it to null
        searchResult = null;
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
//            if (rsp.header.code == (int)ErrorCode.EF_FRIEND_NOT_EXISTS)
//                return;
        }
        searchResult = new TUserInfo(rsp.friend);
    }
    
    
    /// <summary>
    /// Friends responses refreshed list or not.
    /// </summary>
    /// <returns><c>true</c>, if friends refreshed list was rsped, <c>false</c> otherwise.</returns>
    /// <param name="data">Data.</param>
    private bool RspFriendsRefreshedList(object data) {
        RspGetFriend rspGetFriend = data as RspGetFriend;
        RspAddFriend rspAddFriend = data as RspAddFriend;
        RspAcceptFriend rspAcceptFriend = data as RspAcceptFriend;
        RspDelFriend rspDelFriend = data as RspDelFriend;
        if (rspGetFriend != null && rspGetFriend.friends != null) {
            return true;
        }
        else if (rspAddFriend != null) {
            LogHelper.Log("RspFriendsRefreshedList(), rspAddFriend get refresh msg");
        }
        else if (rspAcceptFriend != null) {
            LogHelper.Log("RspFriendsRefreshedList(), rspAcceptFriend get refresh msg");
        }
        else if (rspDelFriend != null) {
            LogHelper.Log("RspFriendsRefreshedList(), rspDelFriend get refresh msg");
        }
        return false;
    } 

    #region inner calls
    /// inner calls
    private void getFriends() {
//        GetFriendList.SendRequest(OnRspGetFriend);
    }

    private void addFriend(uint friendUid) {
//        AddFriend.SendRequest(OnRspAddFriend, friendUid);
    }

    private void delFriend(uint friendUid) {
//        DelFriend.SendRequest(OnRspDelFriend, friendUid);
    }

    private void findFriend(uint friendUid) {
//        FindFriend.SendRequest(OnRspFindFriend, friendUid);
    }

    private void acceptFriend(uint friendUid) {
//        AcceptFriend.SendRequest(OnRspAcceptFriend, friendUid);
    }
    #endregion



    private void setNewInstance(FriendList inst) {
        instance = inst;
    }

    private void assignFriendList() {
        assignFriend();
        assignHelper();
        assignFriendIn();
        assignFriendOut();
    }

    private void assignFriend() {
        if (friend != null) {
            friend.Clear();
        }
        else {
            friend = new List<TFriendInfo>();
        }
        foreach (FriendInfo fi in instance.friend) {
            TFriendInfo tfi = new TFriendInfo(fi);
            Debug.Log("friend: NickName " + tfi.NickName);
            friend.Add(tfi);
        }
    }

    private void assignHelper() {
        if (helper != null) {
            helper.Clear();
        }
        else {
            helper = new List<TFriendInfo>();
        }
        helper = new List<TFriendInfo>();
        foreach (FriendInfo fi in instance.helper) {
            TFriendInfo tfi = new TFriendInfo(fi);
            Debug.Log("helper: NickName " + tfi.NickName);
            helper.Add(tfi);
        }
    }

    
    private void assignFriendIn() {
        if (friendIn != null) {
            friendIn.Clear();
        }
        else {
            friendIn = new List<TFriendInfo>();
        }
        foreach (FriendInfo fi in instance.friendIn) {
            TFriendInfo tfi = new TFriendInfo(fi);
            Debug.Log("helper: NickName " + tfi.NickName);
            friendIn.Add(tfi);
        }
    }

    
    private void assignFriendOut() {
        if (friendOut != null) {
            friendOut.Clear();
        }
        else {
            friendOut = new List<TFriendInfo>();
        }
        foreach (FriendInfo fi in instance.friendIn) {
            TFriendInfo tfi = new TFriendInfo(fi);
            friendOut.Add(tfi);
        }
        foreach (FriendInfo fi in instance.friendOut) {
            TFriendInfo tfi = new TFriendInfo(fi);
            friendOut.Add(tfi);
        }
    }

	
}


