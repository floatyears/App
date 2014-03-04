using bbproto;
using System.Collections;

public class TUserInfo : ProtobufDataBase {
	public TUserInfo(UserInfo inst) : base (inst) { 
		instance = inst;
		unit = new TUserUnit (instance.unit);
	}

	private UserInfo	instance;
	private TUserUnit	unit;
	private EUnitType	evolvetype;

	//////////////////////////////////////////////////////////////
	/// 
	public	string	Uuid { get { return instance.uuid; } }
	public	uint		UserId { get { return instance.userId; } }
	public	string	NickName { get { return instance.nickName; } set {instance.nickName = value; }}
	public	int		Rank { get { return instance.rank; } }
	public	int		Exp { get { return instance.exp; } }
	public	int		NextExp{ get{ return 843; }} //TODO: get exp for current rank
	public	int		CurTotalExp{ get{ return 9106; }} //TODO: get exp for current rank
	public	int		StaminaNow { 
		get { return instance.staminaNow; } 
		set { instance.staminaNow = value; } 
	}
	public	int		StaminaMax { 
		get { return instance.staminaMax; } 
		set { instance.staminaMax = value; } 
	}
	public	uint	StaminaRecover { 
		get { return instance.staminaRecover; } 
		set { instance.staminaRecover = value; } 
	}
	public	TUserUnit UserUnit { get { return unit; } }
	public	EUnitType EvolveType { get {return evolvetype;} set { evolvetype = value;} }
}