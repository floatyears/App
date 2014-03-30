using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TFriendInfo : ProtobufDataBase {
    private FriendInfo	instance;
    public TFriendInfo(FriendInfo inst) : base (inst) { 
        instance = inst;
		Debug.LogError ("instance.unit : " + instance.unit);
        if (instance.unit != null) {
			unit = TUserUnit.GetUserUnit(instance.userId, instance.unit); //new TUserUnit(instance.unit);
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

//    public void OnRspFindFriend(object data) {
//        if (data == null)
//            return;
//        
//        LogHelper.Log("TFriendList.OnRspDelFriend() begin");
//        LogHelper.Log(data);
//        RspFindFriend rsp = data as RspFindFriend;
//        // first set it to null
//        searchResult = null;
//        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
//            LogHelper.Log("OnRspFindFriend code:{0}, error:{1}", rsp.header.code, rsp.header.error);
////            if (rsp.header.code == (int)ErrorCode.EF_FRIEND_NOT_EXISTS)
////                return;
//        }
//        searchResult = new TUserInfo(rsp.friend);
//    }
    /// <summary>
    /// Clears the friend. only call in last friend deleted.
    /// </summary>
    public void clearFriend() {
        friend.Clear();
    }
    
    public void clearFriendIn() {
        friendIn.Clear();
    } 

    public void clearFriendOut() {
        friendOut.Clear();
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
//        Debug.LogError("friend count " + instance.friend.Count);
        DataCenter.Instance.FriendCount = instance.friend.Count;
        foreach (FriendInfo fi in instance.friend) {
            TFriendInfo tfi = new TFriendInfo(fi);
//            Debug.Log("friend: NickName " + tfi.NickName);
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
            Debug.Log("helper: userId " + tfi.UserId);
            Debug.Log("helper: userUnit: id" + tfi.UserUnit);
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


