using UnityEngine;
using System.Collections.Generic;

public class SupportFriendManager {
	private List<TFriendInfo> supportFriend = new List<TFriendInfo> ();
	private List<TFriendInfo> userFriend = new List<TFriendInfo> ();

	private List<TFriendInfo> tempFriend = new List<TFriendInfo> ();

	private GameTimer gameTime ;

	public SupportFriendManager () {
		gameTime = GameTimer.GetInstance ();
	}

	public void AddSupportFriend(List<TFriendInfo> friendInfo) {
		supportFriend.AddRange (friendInfo);
	}

	public void AddSupportFriend(TFriendInfo friendInfo) {
		supportFriend.Add (friendInfo);
	}

	public List<TFriendInfo> GetSupportFriend () {
		for (int i = supportFriend.Count - 1; i >= 0; i--) {
			uint intervTime = gameTime.GetCurrentSeonds() - supportFriend[i].UseTime;
			if(intervTime < GameTimer.TenMinuteSeconds) {
				TFriendInfo tfi = supportFriend[i];
				tempFriend.Add(tfi);
				supportFriend.Remove(tfi);
			}
		}

		for (int i = userFriend.Count - 1; i >= 0; i--) {
			TFriendInfo tfi = userFriend[i];
			uint intervTime = gameTime.GetCurrentSeonds() - tfi.UseTime;
			if( intervTime >= GameTimer.TenMinuteSeconds ) {
				supportFriend.Add(tfi);
				userFriend.Remove(tfi);
			}
		}

		userFriend.AddRange (tempFriend);

		tempFriend.Clear ();

		return supportFriend;
	}

	public bool CheckIsMyFriend(TFriendInfo tfi) {
		TFriendList tfl = DataCenter.Instance.FriendList;

		for (int i = 0; i < tfl.Friend.Count; i++) {
			if(tfl.Friend[i].UserId == tfi.UserId) {
				return true;
			}
		}

		for (int i = 0; i < tfl.FriendOut.Count; i++) {
			if(tfl.FriendOut[i].UserId == tfi.UserId) {
				return true;
			}
		}

		return false;
	}

}
