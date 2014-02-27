using bbproto;
using System.Collections;
using System.Collections.Generic;


public class TFriendInfo : ProtobufDataBase {
	private FriendInfo	instance;
	public TFriendInfo(FriendInfo inst) : base (inst) { 
		instance = inst;
		unit = new TUserUnit (instance.unit);
	}

	private TUserUnit		unit;


	//// property ////
	public	uint			UserId { get { return instance.userId; } }
	public	string			UserName { get { return instance.userName; } }
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
	
	//// property ////
	public List<TFriendInfo> Helper { get { return helper; } }
	public List<TFriendInfo> Friend { get { return friend; } }
	public List<TFriendInfo> FriendIn { get { return friendIn; } }
	public List<TFriendInfo> FriendOut { get { return friendOut; } }
	

	//constructor
	public TFriendList(FriendList inst) : base (inst) { 
		instance = inst;
		AssignFriends ();
	}

	private void AssignFriends() {
		friend = new List<TFriendInfo> ();
		foreach ( FriendInfo fi in instance.friend ) {
			TFriendInfo tfi = new TFriendInfo(fi);
			friend.Add( tfi );
		}
		
		helper = new List<TFriendInfo> ();
		foreach ( FriendInfo fi in instance.helper ) {
			TFriendInfo tfi = new TFriendInfo(fi);
			helper.Add( tfi );
		}
		
		friendIn = new List<TFriendInfo> ();
		foreach ( FriendInfo fi in instance.friendIn ) {
			TFriendInfo tfi = new TFriendInfo(fi);
			friendIn.Add( tfi );
		}
		
		friendOut = new List<TFriendInfo> ();
		foreach ( FriendInfo fi in instance.friendOut ) {
			TFriendInfo tfi = new TFriendInfo(fi);
			friendOut.Add( tfi );
		}
	}
	
}

