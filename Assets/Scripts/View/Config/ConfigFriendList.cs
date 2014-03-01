using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigFriendList {
	public static List< UnitBaseInfo > availableFriendList = new List<UnitBaseInfo>();
	private List<FriendInfo> selectableFriendList = new List<FriendInfo>();

	public ConfigFriendList(){
		GenerateSelectableFriendList();
	}

	void GenerateSelectableFriendList(){
		FriendList friendList = new FriendList();
		List< FriendInfo > ownedFriendList = new List<FriendInfo>();
		List<FriendInfo> helperFriendList = new List<FriendInfo>();
		ownedFriendList = friendList.friend;
		helperFriendList = friendList.helper;
		FriendInfo temp;

		temp = new FriendInfo();
		temp.unit = new UserUnit();
		temp.nickName = "Tony";
		temp.friendPoint = 10;
		temp.unit.unitId = 3;
		ownedFriendList.Add( temp );

		temp = new FriendInfo();
		temp.unit = new UserUnit();
		temp.nickName = "Kory";
		temp.friendPoint = 10;
		temp.unit.unitId = 4;
		ownedFriendList.Add( temp );

		temp = new FriendInfo();
		temp.unit = new UserUnit();
		temp.nickName = "Orca";
		temp.unit.unitId = 16;
		ownedFriendList.Add( temp );

		temp = new FriendInfo();
		temp.unit = new UserUnit();
		temp.nickName = "Neon";
		temp.friendPoint = 5;
		temp.unit.unitId = 7;
		helperFriendList.Add( temp );

		temp = new FriendInfo();
		temp.unit = new UserUnit();
		temp.nickName = "Massi";
		temp.friendPoint = 5;
		temp.unit.unitId = 15;
		helperFriendList.Add( temp );

		selectableFriendList.AddRange( ownedFriendList );
		selectableFriendList.AddRange( helperFriendList );

	}

	void DisposeFriendList(){
		foreach (var item in selectableFriendList){
			UnitBaseInfo unitBaseInfo = new UnitBaseInfo();

		}
	}

	void GetAvatarSource( int id){

	}
}

public class AvailableFriend {
	public Texture2D avatar;
	public Texture2D role;
	public string userName;
	//public int 
}
