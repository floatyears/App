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
        if (inst == null)
            return;
        setNewInstance(inst);
        assignFriendList();
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
		Debug.LogError("friend count " + instance.friend.Count);
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
//            Debug.Log("helper: NickName " + tfi.NickName);
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


