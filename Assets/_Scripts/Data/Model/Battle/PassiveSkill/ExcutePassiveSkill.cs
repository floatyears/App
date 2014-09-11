using UnityEngine;
using System.Collections.Generic;

public class ExcutePassiveSkill : IExcutePassiveSkill  {
	private ILeaderSkill leaderSkill;
	public ExcuteTrap excuteTrap;
	private Dictionary<string,IPassiveExcute> passiveSkill = new Dictionary<string,IPassiveExcute> ();
	private Dictionary<string,float> multipe = new Dictionary<string, float> ();

	public ExcutePassiveSkill(ILeaderSkill ls) {
		leaderSkill = ls;
		excuteTrap = new ExcuteTrap ();
		InitPassiveSkill ();
		MsgCenter.Instance.AddListener (CommandEnum.MeetTrap, DisposeTrapEvent);
	}

	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetTrap, DisposeTrapEvent);
	}

	void InitPassiveSkill() {
		foreach (var item in leaderSkill.UserUnit.Values) {
			if (item==null) {
				continue;
			}

			int id = item.PassiveSkill;

			if(id == -1) {
				continue;
			}

			IPassiveExcute ipe = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), id, SkillType.PassiveSkill) as IPassiveExcute; //Skill[id] as IPassiveExcute;

			if(ipe == null) {
				continue;
			}

			string name = item.MakeUserUnitKey();
			passiveSkill.Add(name,ipe);
			multipe.Add(name,item.AttackMultiple);
		}
	}

	Queue <TrapBase> trap = new Queue<TrapBase>();
	void DisposeTrapEvent(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;		
		}
		trap.Enqueue (tb);
		if (passiveSkill.Values.Count == 0) {
			DisposeTrap (false);
		} else {
			foreach (var item in passiveSkill) {
				bool b = (bool)item.Value.Excute(tb, this);
				if(b) {
					foreach (var unitItem in leaderSkill.UserUnit.Values) {
						if(unitItem == null) {
							continue;
						}

						if(unitItem.MakeUserUnitKey() == item.Key) {
							AttackInfo ai = AttackInfo.GetInstance();
							ai.UserUnitID = unitItem.MakeUserUnitKey();
							ai.SkillID = item.Value.skillBaseInfo.skillBase.id;
//							MsgCenter.Instance.Invoke(CommandEnum.ShowPassiveSkill, ai);
							ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refreshitem", ai);

						}
					}
				}
			}
		}
	}

	private List<AttackInfo> attackList = new List<AttackInfo> ();

	public List<AttackInfo> Dispose (int AttackType, int attack) {
		attackList.Clear ();
		foreach (var item in passiveSkill) {
			if(item.Value is TSkillAntiAttack) {
				AttackInfo ai = item.Value.Excute(AttackType, this) as AttackInfo;
				if(ai != null) {
					ai.SkillID = item.Value.skillBaseInfo.skillBase.id;
					ai.UserUnitID = item.Key;
					ai.AttackValue *= attack;
					ai.AttackValue *= multipe[item.Key];
					attackList.Add(ai);
				}
			}
		}
		return attackList;
	}

	public void DisposeTrap (bool isAvoid) {
		if (trap.Count == 0) {
			return;	
		}
		TrapBase tb = trap.Dequeue ();

		if (isAvoid) {
			return;
		} else {
			TrapBase ie = tb as TrapBase;
			excuteTrap.Excute (ie);	
		}
	}
}
