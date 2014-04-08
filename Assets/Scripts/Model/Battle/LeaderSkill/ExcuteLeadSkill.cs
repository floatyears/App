using UnityEngine;
using System.Collections.Generic;

public class ExcuteLeadSkill : ILeadSkillReduceHurt, ILeaderSkillExtraAttack, ILeaderSkillSwitchCard,ILeaderSkillRecoverHP, ILeaderSkillMultipleAttack {
	ILeaderSkill leadSkill;
	List<string> RemoveSkill = new List<string> ();

	public ExcuteLeadSkill (ILeaderSkill lead) {
		leadSkill = lead;
	}

	public void Excute() {
		foreach (var item in leadSkill.LeadSkill) {
			if(DisposeBoostSkill(item.Key, item.Value)) {
				RemoveSkill.Add(item.Key);
			}
		}
		if (RemoveSkill.Count == 0) {
			MsgCenter.Instance.Invoke(CommandEnum.StopInput,true);
		}

		MsgCenter.Instance.Invoke (CommandEnum.LeaderSkillEnd, null);
		RemoveLeaderSkill ();
	}

	void RemoveLeaderSkill () {
		for (int i = 0; i < RemoveSkill.Count; i++) {
			leadSkill.LeadSkill.Remove(RemoveSkill[i]);
		}
	}
	
	bool DisposeBoostSkill (string userunit, ProtobufDataBase pdb) {
		TSkillBoost tbs = pdb as TSkillBoost;
		if (tbs != null) {
			AttackInfo ai = new AttackInfo();
			ai.UserUnitID = userunit;
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);

			foreach (var item in leadSkill.UserUnit.Values) {
				if( item == null) {
					continue;
				}
				item.SetAttack(tbs.GetBoostValue, tbs.GetTargetValue, tbs.GetTargetType, tbs.GetBoostType);
			}
			return true;
		}
		else {
			return DisposeDelayOperateTime(userunit,pdb);
		}
	}

	bool DisposeDelayOperateTime (string userunit, ProtobufDataBase pdb) {
		TSkillDelayTime tst = pdb as TSkillDelayTime;
		if (tst != null) {
			AttackInfo ai = new AttackInfo();
			ai.UserUnitID = userunit;
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
			MsgCenter.Instance.Invoke(CommandEnum.LeaderSkillDelayTime, tst.DelayTime);
			return true;
		}
		return false;
	}
	
	public float ReduceHurtValue (float hurt,int type) {
		if (leadSkill.LeadSkill.Count == 0) {
			return hurt;	
		}
		foreach (var item in leadSkill.LeadSkill) {
			TSkillReduceHurt trh = item.Value as TSkillReduceHurt;
			if(trh != null) {
				hurt = trh.ReduceHurt(hurt,type);
				if(trh.CheckUseDone()) {
					RemoveSkill.Add(item.Key);
				}
			}
		}
		RemoveLeaderSkill ();
		return hurt;
	}
		
	public List<AttackInfo> ExtraAttack (){
		List<AttackInfo> ai = new List<AttackInfo>();
		if (leadSkill.LeadSkill.Count == 0) {
			return ai;
		}
		foreach (var item in leadSkill.LeadSkill) {
			TSkillExtraAttack tsea = item.Value as TSkillExtraAttack;
			if(tsea == null) {
				continue;
			}
			string id = item.Key;
			foreach (var item1 in leadSkill.UserUnit) {
				if(item1.Value.MakeUserUnitKey() == id) {
					AttackInfo attack = tsea.AttackValue(item1.Value.Attack, item1.Value);
					ai.Add(attack);
					break;
				}
			}
		}
		return ai;
	}

	public List<int> SwitchCard (List<int> cardQuene) {
		if (leadSkill.LeadSkill.Count == 0) {
			return null;
		}

		foreach (var item in leadSkill.LeadSkill) {
			TSkillConvertUnitType tcut = item.Value as TSkillConvertUnitType;

			if(tcut == null) {
				continue;
			}

			for (int i = 0; i < cardQuene.Count; i++) {
				cardQuene[i] = tcut.SwitchCard(cardQuene[i]);
			}
		}

		return cardQuene;
	}

	public int SwitchCard (int card) {
		if (leadSkill.LeadSkill.Count == 0) {
			return card;
		}
		
		foreach (var item in leadSkill.LeadSkill) {
			TSkillConvertUnitType tcut = item.Value as TSkillConvertUnitType;	
			if(tcut == null) {
				continue;
			}
			card = tcut.SwitchCard(card);
		}
		return card;
	}
	
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">Type. 0 = right now. 1 = every round. 2 = every step.</param>
	public int RecoverHP (int blood, int type) {
		if (leadSkill.LeadSkill.Count == 0) {
			return blood;
		}

		foreach (var item in leadSkill.LeadSkill) {
			TSkillRecoverHP trhp = item.Value as TSkillRecoverHP;	
			if(trhp == null) {
				continue;
			}

			blood = trhp.RecoverHP(blood, type);
		}
		return blood;
	}

	public float MultipleAttack (List<AttackInfo> attackInfo) {
		if (leadSkill.LeadSkill.Count == 0) {
			return 1f;
		}
		float multipe = 0f;
		foreach (var item in leadSkill.LeadSkill) {
			LeaderSkillMultipleAttack trhp = item.Value as LeaderSkillMultipleAttack;
			if(trhp == null) {
				continue;
			}
			multipe += trhp.MultipeAttack(attackInfo);
		}
		return multipe;
	}
}

