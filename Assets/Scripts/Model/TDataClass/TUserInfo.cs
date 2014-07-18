using bbproto;
using System.Collections;

public class TUserInfo : ProtobufDataBase
{
	public TUserInfo(UserInfo inst) : base (inst){ 
		instance = inst;
		unit = TUserUnit.GetUserUnit (instance.userId, instance.unit);
	}

	private UserInfo	instance;
	private TUserUnit	unit;
	private EUnitType	evolvetype;

	//////////////////////////////////////////////////////////////
	/// 
	public	string	Uuid { get { return instance.uuid; } }
	public	uint		UserId { get { return instance.userId; } }
	public	string	NickName { get { return instance.nickName; } set { instance.nickName = value; } }
	public	int		Rank { get { return instance.rank; } set { instance.rank = value; } }
	public	int		Exp { get { return instance.exp; } set { instance.exp = value; } }

	public	int		FriendMax { get { return instance.friendMax; } set { instance.friendMax = value; } }
	public	int		CostMax { get { return instance.costMax; } set { instance.costMax = value; } }
	public	int		UnitMax { get { return instance.unitMax; } set { instance.unitMax = value; } }
	public	int		StaminaMax {  get { return instance.staminaMax; } set { instance.staminaMax = value; } }

	// NextExp return the exp need to Rise to next rank.
	public int NextExp
	{ 
		get
		{ 
			int nextLevel = Rank + 1;
			int totalExp = 0;
			for (int i = 1; i <= nextLevel; i++)
			{
				totalExp += DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType, i);
			}
			return totalExp - Exp;

//			return DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType,Rank + 1); 
		}
	} 

	//return the user's exp of current rank (in total)
	public int CurRankExp { 
		get { 
			int curLevel = Rank;
			int totalExp = 0;
			for (int i = 1; i < curLevel; i++) {
				totalExp += DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType, i);
			}

//			UnityEngine.Debug.LogError(curLevel+" => CurRankExp:" + totalExp);
			return totalExp;
		}
	} 

	public int CurPrevExp {
		get { 
			int curLevel = Rank;
			int totalExp = 0;
			for (int i = 0; i < curLevel; i++) {
				totalExp += DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType, i);
			}
			return totalExp;
		}
	}

	public int	CurRankExpMax
	{ 
		get
		{ 
			int curLevel = Rank + 1;
			//UnityEngine.Debug.LogError("CurRankExpMax :: " + DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType, curLevel));
			return DataCenter.Instance.GetUnitValue(TPowerTableInfo.UserExpType, curLevel);
		}
	} 

	public	int		StaminaNow
	{ 
		get { return instance.staminaNow; } 
		set { instance.staminaNow = value; } 
	}

	public	uint	StaminaRecover
	{ 
		get { return instance.staminaRecover; } 
		set { instance.staminaRecover = value; } 
	}
	public	TUserUnit UserUnit { get { return unit; } }
	public	EUnitType EvolveType { get { return evolvetype; } set { evolvetype = value; } }

	public void RefreshUserInfo(TRspClearQuest rspClearQuest)
	{
		instance.exp = rspClearQuest.exp;
		instance.rank = rspClearQuest.rank;
		instance.staminaMax = rspClearQuest.staminaMax;
		instance.staminaNow = rspClearQuest.staminaNow;
		instance.staminaRecover = rspClearQuest.staminaRecover;
	}
}