using UnityEngine;
using System.Collections.Generic;

public class ExcuteLeadSkill : ILeadSkillReduceHurt, ILeaderSkillExtraAttack, ILeaderSkillSwitchCard,ILeaderSkillRecoverHP {
	ILeadSkill leadSkill;
	List<int> RemoveSkill = new List<int> ();

	public ExcuteLeadSkill (ILeadSkill lead) {
		leadSkill = lead;
	}

	public void Excute() {
//		if (leadSkill.LeadSkill.Count > 0) {
//			DisposeBoostSkill (leadSkill.LeadSkill [0]);	
//		}

		foreach (var item in leadSkill.LeadSkill) {
			if(DisposeBoostSkill(item.Value)){
				RemoveSkill.Add(item.Key);
			}
		}
		RemoveLeaderSkill ();
	}

	void RemoveLeaderSkill () {
		for (int i = 0; i < RemoveSkill.Count; i++) {
			leadSkill.LeadSkill.Remove(i);
		}
	}
	
	bool DisposeBoostSkill (ProtobufDataBase pdb) {
		TempBoostSkill tbs = pdb as TempBoostSkill;
		if (tbs != null) {
			foreach (var item in leadSkill.UserUnit.Values) {
				item.SetAttack(tbs.GetBoostValue, tbs.GetTargetValue, tbs.GetTargetType, tbs.GetBoostType);
			}
			return true;
		}
		else {
			return DisposeDelayOperateTime(pdb);
		}
	}

	bool DisposeDelayOperateTime (ProtobufDataBase pdb) {
		TempSkillTime tst = pdb as TempSkillTime;
		if (tst != null) {
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
			TempReduceHurt trh = item.Value as TempReduceHurt;
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
			return null;
		}
		foreach (var item in leadSkill.LeadSkill) {
			TempSkillExtraAttack tsea = item.Value as TempSkillExtraAttack;
			if(tsea == null) {
				continue;
			}
			int id = item.Key;
			foreach (var item1 in leadSkill.UserUnit) {
				if(item1.Value.GetID == id) {
					AttackInfo attack = tsea.AttackValue(item1.Value.GetAttack,id);
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
			TempConvertUnitType tcut = item.Value as TempConvertUnitType;

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
			TempConvertUnitType tcut = item.Value as TempConvertUnitType;	
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
			TempRecoverHP trhp = item.Value as TempRecoverHP;	
			if(trhp == null) {
				continue;
			}

			blood = trhp.RecoverHP(blood, type);
		}
		return blood;
	}


}

