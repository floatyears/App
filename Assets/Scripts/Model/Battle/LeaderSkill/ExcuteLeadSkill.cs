using UnityEngine;
using System.Collections.Generic;

public class ExcuteLeadSkill : ILeadSkillReduceHurt, ILeaderSkillExtraAttack, ILeaderSkillSwitchCard,ILeaderSkillRecoverHP, ILeaderSkillMultipleAttack {
	ILeaderSkill leadSkill;
	List<string> RemoveSkill = new List<string> ();

	public ExcuteLeadSkill (ILeaderSkill lead) {
		leadSkill = lead;
	}

	const float time = 0.5f;
	Queue<string> leaderSkillQueue = new Queue<string> ();

	public void Excute() {
		int temp = 0;
		foreach (var item in leadSkill.LeadSkill) {
			temp++;
			if(item.Value is TSkillBoost) {
				leaderSkillQueue.Enqueue(item.Key);
				GameTimer.GetInstance().AddCountDown(temp*time, ExcuteStartLeaderSkill);
			}
		}
	}

	void ExcuteStartLeaderSkill() {
		string key = leaderSkillQueue.Dequeue ();
		DisposeBoostSkill (key, leadSkill.LeadSkill [key]);
		leadSkill.LeadSkill.Remove (key);
		if (leaderSkillQueue.Count == 0) {
			MsgCenter.Instance.Invoke (CommandEnum.LeaderSkillEnd, null);
		}
	}

	void RemoveLeaderSkill () {
		for (int i = 0; i < RemoveSkill.Count; i++) {
			leadSkill.LeadSkill.Remove(RemoveSkill[i]);
		}
	}

	void DisposeBoostSkill (string userunit, ProtobufDataBase pdb) {
		TSkillBoost tbs = pdb as TSkillBoost;
		if (tbs != null) {
			AttackInfo ai = AttackInfo.GetInstance(); //new AttackInfo();
			ai.UserUnitID = userunit;
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
			foreach (var item in leadSkill.UserUnit.Values) {
				if( item == null) {
					continue;
				}
				item.SetAttack(tbs.GetBoostValue, tbs.GetTargetValue, tbs.GetTargetType, tbs.GetBoostType);
			}
		}
		else {
			DisposeDelayOperateTime(userunit,pdb);
		}
	}

	void DisposeDelayOperateTime (string userunit, ProtobufDataBase pdb) {
		TSkillDelayTime tst = pdb as TSkillDelayTime;
		if (tst != null) {
			AttackInfo ai = AttackInfo.GetInstance(); //new AttackInfo();
			ai.UserUnitID = userunit;
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
			MsgCenter.Instance.Invoke(CommandEnum.LeaderSkillDelayTime, tst.DelayTime);
		}
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
//		Debug.LogError("leadSkill.LeadSkill.Count : " + leadSkill.LeadSkill.Count);
		if (leadSkill.LeadSkill.Count == 0) {
			return ai;
		}
		foreach (var item in leadSkill.LeadSkill) {
			TSkillExtraAttack tsea = item.Value as TSkillExtraAttack;
//			Debug.LogError("tsea : " + tsea + " value : " + item.Value);
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
			return 0;	//recover zero hp
		}
		int recoverHP = 0;
		foreach (var item in leadSkill.LeadSkill) {
			TSkillRecoverHP trhp = item.Value as TSkillRecoverHP;
			if(trhp == null) {
				continue;
			}
			recoverHP = trhp.RecoverHP(blood, type);
		}
		return recoverHP;
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

	/// <summary>
	/// utility ready move animation
	/// </summary>
	int tempLeaderSkillCount = 0;
	public int CheckLeaderSkillCount () {
		foreach (var item in leadSkill.LeadSkill.Values) {
			if(item is TSkillBoost) {
				tempLeaderSkillCount ++;
			}else if(item is TSkillDelayTime){
				tempLeaderSkillCount ++;
			}
		}
		return tempLeaderSkillCount;
	}
}

