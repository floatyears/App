using UnityEngine;
using System.Collections.Generic;

public class SupportFriendManager {
	private List<TFriendInfo> supportFriend = new List<TFriendInfo> ();
	private List<TFriendInfo> userFriend = new List<TFriendInfo> ();

	public void AddSupportFriend(TFriendInfo friendInfo) {
		supportFriend.Add (friendInfo);
	}

	public List<TFriendInfo> GetSupportFriend () {
		return null;
	}
}
