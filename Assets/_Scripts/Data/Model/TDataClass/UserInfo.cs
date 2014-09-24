using bbproto;
using System.Collections;

namespace bbproto{
	public partial class UserInfo : ProtoBuf.IExtensible
	{
		public UserInfo(){ 

		}

		public void Init(){
			unit.userID = userId;
		}
	
		private EUnitType	evolvetype;

		//////////////////////////////////////////////////////////////
		// NextExp return the exp need to Rise to next rank.
		public int NextExp
		{ 
			get
			{ 
				int nextLevel = rank + 1;
				int totalExp = 0;
				for (int i = 1; i <= nextLevel; i++)
				{
					totalExp += DataCenter.Instance.UnitData.GetUnitValue(PowerTable.UserExpType, i);
				}
				return totalExp - exp;

	//			return DataCenter.Instance.UnitData.GetUnitValue(TPowerTableInfo.UserExpType,Rank + 1); 
			}
		} 

		//return the user's exp of current rank (in total)
		public int CurRankExp { 
			get { 
				int curLevel = rank;
				int totalExp = 0;
				for (int i = 1; i < curLevel; i++) {
					totalExp += DataCenter.Instance.UnitData.GetUnitValue(PowerTable.UserExpType, i);
				}

	//			UnityEngine.Debug.LogError(curLevel+" => CurRankExp:" + totalExp);
				return totalExp;
			}
		} 

		public int CurPrevExp {
			get { 
				int curLevel = rank;
				int totalExp = 0;
				for (int i = 0; i < curLevel; i++) {
					totalExp += DataCenter.Instance.UnitData.GetUnitValue(PowerTable.UserExpType, i);
				}
				return totalExp;
			}
		}

		public int	CurRankExpMax
		{ 
			get
			{ 
				int curLevel = rank + 1;
				//UnityEngine.Debug.LogError("CurRankExpMax :: " + DataCenter.Instance.UnitData.GetUnitValue(TPowerTableInfo.UserExpType, curLevel));
				return DataCenter.Instance.UnitData.GetUnitValue(PowerTable.UserExpType, curLevel);
			}
		} 

		public	EUnitType EvolveType { get { return evolvetype; } set { evolvetype = value; } }

		public void RefreshUserInfo(TRspClearQuest rspClearQuest)
		{
			exp = rspClearQuest.exp;
			rank = rspClearQuest.rank;
	//		UnityEngine.Debug.LogError ("TUserInfo RefreshUserInfo : " + instance.exp + " instance.rank : " + instance.rank);
			staminaMax = rspClearQuest.staminaMax;
			staminaNow = rspClearQuest.staminaNow;
			staminaRecover = rspClearQuest.staminaRecover;
		}
	}
}