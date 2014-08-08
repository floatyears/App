using UnityEngine;
using System.Collections.Generic;

public class SupportFriendManager {
	private List<TFriendInfo> supportFriend = new List<TFriendInfo> ();
	private List<TFriendInfo> userFriend = new List<TFriendInfo> ();

	private List<TFriendInfo> tempFriend = new List<TFriendInfo> ();
	private List<TFriendInfo> selectFriend = new List<TFriendInfo>();

	private GameTimer gameTime ;

	public const int selectFriendNumber = 10;

	public TFriendInfo useFriend = null;

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
		selectFriend.Clear ();

		for (int i = supportFriend.Count - 1; i >= 0; i--) {
			TFriendInfo tfi = supportFriend[i];
			uint intervTime = gameTime.GetCurrentSeonds() - tfi.UseTime;
			if(intervTime < GameTimer.TenMinuteSeconds) {
				tempFriend.Add(tfi);
				supportFriend.Remove(tfi);
			}
		}

		for (int i = userFriend.Count - 1; i >= 0; i--) {
			TFriendInfo tfi = userFriend[i];
			uint intervTime = gameTime.GetCurrentSeonds () - tfi.UseTime;
			if( intervTime >= GameTimer.TenMinuteSeconds ) {
				supportFriend.Add(tfi);
				userFriend.Remove(tfi);
			}
		}

		userFriend.AddRange (tempFriend);

		tempFriend.Clear ();

		for (int i = 0; i < selectFriendNumber; i++) {
			int maxNumber = supportFriend.Count;
			if(maxNumber == 0) {
				break;
			}
			int randomIndex = Random.Range(0, maxNumber);
			TFriendInfo tfi = supportFriend[randomIndex];
			selectFriend.Add(tfi);
			supportFriend.RemoveAt(randomIndex);
		}

		supportFriend.AddRange (selectFriend);

		return selectFriend;
	}

	public bool CheckIsMyFriend(TFriendInfo tfi) {
		TFriendList tfl = DataCenter.Instance.FriendList;
		if (tfl == null) {
			return false;
		}
		for (int i = 0; i < tfl.Friend.Count; i++) {
//			Debug.LogError ("tfl.Friend[i] : " + tfl.Friend[i]);
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
