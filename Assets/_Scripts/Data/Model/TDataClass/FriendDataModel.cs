using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FriendDataModel : ProtobufDataBase {

	private FriendList	instance;
	
	private List<FriendInfo> helper;
	private List<FriendInfo> friend;
	private List<FriendInfo> friendIn;
	private List<FriendInfo> friendOut;
	
	private UserInfo searchResult;
	private ErrorMsg errMsg;
	
	//// property ////
	public List<FriendInfo> Helper { get { return helper; } }
	public List<FriendInfo> Friend { get { return friend; } }
	public List<FriendInfo> FriendIn { get { return friendIn; } }
	public List<FriendInfo> FriendOut { get { return friendOut; } }
	public UserInfo SearchResult { get { return searchResult; } }
	public ErrorMsg ErrMsg { get { return errMsg; } }
	
	//constructor
	public FriendDataModel()  { 
		gameTime = GameTimer.GetInstance ();
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
	
	public List<FriendInfo> GetCanUseFriend () {
		List<FriendInfo> UseFriend = new List<FriendInfo> ();
		for (int i = friend.Count; i >= 0 ; i--) {
			FriendInfo tfi = friend[i];
			uint intervTime = GameTimer.GetInstance().GetCurrentSeonds() - tfi.usedTime;
			if(intervTime >= GameTimer.TenMinuteSeconds) {
				UseFriend.Add(tfi);
			}
		}
		return UseFriend;
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
			friend = new List<FriendInfo>();
		}
		//        Debug.LogError("friend count " + instance.friend.Count);
		DataCenter.Instance.FriendData.FriendCount = instance.friend.Count;
		foreach (FriendInfo fi in instance.friend) {
//			FriendInfo tfi = new FriendInfo(fi);
			//            Debug.Log("friend: NickName " + tfi.NickName);
			friend.Add(fi);
		}
	}
	
	private void assignHelper() {
		if (helper != null) {
			helper.Clear();
		}
		else {
			helper = new List<FriendInfo>();
		}
		helper = new List<FriendInfo>();
		foreach (FriendInfo fi in instance.helper) {
//			FriendInfo tfi = new FriendInfo(fi);
//			Debug.Log("helper: NickName " + tfi.nickName);
//			Debug.Log("helper: userId " + tfi.userId);
//			Debug.Log("helper: userUnit: id" + tfi.UserUnit);
			helper.Add(fi);
		}
	}
	
	private void assignFriendIn() {
		if (friendIn != null) {
			friendIn.Clear();
		}
		else {
			friendIn = new List<FriendInfo>();
		}
		foreach (FriendInfo fi in instance.friendIn) {
//			FriendInfo tfi = new FriendInfo(fi);
			//            Debug.Log("helper: NickName " + tfi.NickName);
			friendIn.Add(fi);
		}
	}
	
	
	private void assignFriendOut() {
		if (friendOut != null) {
			friendOut.Clear();
		}
		else {
			friendOut = new List<FriendInfo>();
		}
		foreach (FriendInfo fi in instance.friendIn) {
//			FriendInfo tfi = new FriendInfo(fi);
			friendOut.Add(fi);
		}
		foreach (FriendInfo fi in instance.friendOut) {
//			FriendInfo tfi = new FriendInfo(fi);
			friendOut.Add(fi);
		}
	}



	private List<FriendInfo> supportFriend = new List<FriendInfo> ();
	private List<FriendInfo> userFriend = new List<FriendInfo> ();
	
	private List<FriendInfo> tempFriend = new List<FriendInfo> ();
	private List<FriendInfo> selectFriend = new List<FriendInfo>();
	
	private GameTimer gameTime ;
	
	public const int selectFriendNumber = 10;
	
	public FriendInfo useFriend = null;
	
	public void AddSupportFriend(List<FriendInfo> friendInfo) {
		supportFriend.AddRange (friendInfo);
	}
	
	public void AddSupportFriend(FriendInfo friendInfo) {
		supportFriend.Add (friendInfo);
	}
	
	public List<FriendInfo> GetSupportFriend () {
		selectFriend.Clear ();
		
		//		for (int i = supportFriend.Count - 1; i >= 0; i--) {
		//			TFriendInfo tfi = supportFriend[i];
		//			uint intervTime = gameTime.GetCurrentSeonds() - tfi.UseTime;
		//			if(intervTime < GameTimer.TenMinuteSeconds) {
		//				tempFriend.Add(tfi);
		//				supportFriend.Remove(tfi);
		//			}
		//		}
		uint CurrentTime = gameTime.GetCurrentSeonds ();
		tempFriend = supportFriend.FindAll (a => IsLimitTimeOver(a)); // a.UseTime
		supportFriend.RemoveAll(a=> tempFriend.Contains(a));
		
		//		for (int i = userFriend.Count - 1; i >= 0; i--) {
		//			TFriendInfo tfi = userFriend[i];
		//			uint intervTime = gameTime.GetCurrentSeonds () - tfi.UseTime;
		//			if( intervTime >= GameTimer.TenMinuteSeconds ) {
		//				supportFriend.Add(tfi);
		//				userFriend.Remove(tfi);
		//			}
		//		}
		
		List<FriendInfo> canUseFriend = userFriend.FindAll (a => !IsLimitTimeOver(a));
		supportFriend.AddRange (canUseFriend);
		userFriend.RemoveAll(a=>canUseFriend.Contains(a));
		
		userFriend.AddRange (tempFriend);
		
		tempFriend.Clear ();
		
		for (int i = 0; i < selectFriendNumber; i++) {
			int maxNumber = supportFriend.Count;
			if(maxNumber == 0) {
				break;
			}
			int randomIndex = Random.Range(0, maxNumber);
			FriendInfo tfi = supportFriend[randomIndex];
			selectFriend.Add(tfi);
			supportFriend.RemoveAt(randomIndex);
		}
		
		supportFriend.AddRange (selectFriend);
		
		return selectFriend;
	}
	
	bool IsLimitTimeOver(FriendInfo friendInfo) {
		return ((gameTime.GetCurrentSeonds () - friendInfo.usedTime) < GameTimer.TenMinuteSeconds);
	}
	
	public bool CheckIsMyFriend(FriendInfo tfi) {
		FriendDataModel tfl = DataCenter.Instance.FriendData;
		
		if (tfl == null) {
			return false;
		}
		
		for (int i = 0; i < tfl.Friend.Count; i++) {
			if(tfl.Friend[i].userId == tfi.userId) {
				return true;
			}
		}
		
		for (int i = 0; i < tfl.FriendOut.Count; i++) {
			if(tfl.FriendOut[i].userId == tfi.userId) {
				return true;
			}
		}
		
		return false;
	}

	private int friendCount = -1;
	public int FriendCount {
		get {
			if(friendCount == -1){
				List<FriendInfo> supporters = GetSupportFriend();//SupportFriends;
				if (supporters != null){
					for (int i = 0; i < supporters.Count; i++){
						if (supporters[i].friendState == EFriendState.ISFRIEND){
							friendCount += 1;
						}
					}
				}
			}
			return friendCount;
		}
		set {
			friendCount = value;
		}
	}

	private StatHelperCount helperInfo;
	public StatHelperCount HelperInfo{
		get { return helperInfo; }
		set { helperInfo = value; }
	}

	
	public FriendInfo GetSupporterInfo(uint friendUid){
		foreach (var item in GetSupportFriend()) {
			if (item.userId == friendUid) {
				return item;
			}
		}
		return null;
	}
}
