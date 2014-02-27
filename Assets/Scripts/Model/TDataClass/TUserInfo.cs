using bbproto;
using System.Collections;

public class TUserInfo : ProtobufDataBase {
	public TUserInfo(UserInfo inst) : base (inst) { 
		instance = inst;

		unit = new TUserUnit (instance.unit);
	}

	private UserInfo	instance;
	private TUserUnit	unit;

	//////////////////////////////////////////////////////////////
	/// 
	public	string	Uuid { get { return instance.uuid; } }
	public	uint	UserId { get { return instance.userId; } }
	public	string	NickName { get { return instance.nickName; } }
	public	int		Rank { get { return instance.rank; } }
	public	int		Exp { get { return instance.exp; } }
	public	int		StaminaNow { get { return instance.staminaNow; } }
	public	int		StaminaMax { get { return instance.staminaMax; } }
	public	uint	StaminaRecover { get { return instance.staminaRecover; } }
	public	TUserUnit UserUnit { get { return unit; } }
	
}