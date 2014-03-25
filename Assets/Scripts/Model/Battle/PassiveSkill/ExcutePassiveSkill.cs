using UnityEngine;
using System.Collections.Generic;

public class ExcutePassiveSkill : IExcutePassiveSkill  {
	private ILeaderSkill leaderSkill;
	private ExcuteTrap excuteTrap;
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
//		Debug.LogError("passive id : " + leaderSkill.UserUnit.Count);
		foreach (var item in leaderSkill.UserUnit.Values) {
			int id = item.PassiveSkill;

			if(id == -1) {
				continue;
			}
			IPassiveExcute ipe = DataCenter.Instance.Skill[id] as IPassiveExcute;
			if(ipe == null) {
				continue;
			}
			passiveSkill.Add(item.MakeUserUnitKey(),ipe);
			multipe.Add(item.MakeUserUnitKey(),item.AttackMultiple);
		}
	}

	Queue <TrapBase> trap = new Queue<TrapBase>();
	void DisposeTrapEvent(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;		
		}
//		Debug.LogError (tb);
		trap.Enqueue (tb);
		if (passiveSkill.Values.Count == 0) {
			DisposeTrap (false);
		} else {
			foreach (var item in passiveSkill.Values) {
				item.Excute(tb, this);
			}
		}
	}

	private List<AttackInfo> attackList = new List<AttackInfo> ();

	public List<AttackInfo> Dispose (int AttackType, int attack) {
		attackList.Clear ();
		foreach (var item in passiveSkill) {
			AttackInfo ai = item.Value.Excute(AttackType, this) as AttackInfo;
			if(ai != null) {
				ai.UserUnitID = item.Key;
				ai.AttackValue *= attack;
				ai.AttackValue *= multipe[item.Key];
				attackList.Add(ai);
			}
		}
		return attackList;
	}

	public void DisposeTrap (bool isAvoid) {
//		Debug.LogError ("trap.Count : " + trap.Count + " isAvoid : " + isAvoid);
		if (trap.Count == 0) {
			return;	
		}
		TrapBase tb = trap.Dequeue ();

		if (isAvoid) {
			return;
		} else {
			ITrapExcute ie = tb as ITrapExcute;
			excuteTrap.Excute (ie);	
		}
	}
	
}
