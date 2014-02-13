using UnityEngine;
using System.Collections.Generic;

public class ExcutePassiveSkill : IExcutePassiveSkill  {
	private ILeaderSkill leaderSkill;
	private ExcuteTrap excuteTrap;
	private Dictionary<uint,IPassiveExcute> passiveSkill = new Dictionary<uint,IPassiveExcute> ();
	private Dictionary<uint,float> multipe = new Dictionary<uint, float> ();

	public ExcutePassiveSkill(ILeaderSkill ls) {
		leaderSkill = ls;
		excuteTrap = new ExcuteTrap ();
		InitPassiveSkill ();
		MsgCenter.Instance.AddListener (CommandEnum.MeetTrap, DisposeTrapEvent);
	}

	~ ExcutePassiveSkill () {
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetTrap, DisposeTrapEvent);
	}

	void InitPassiveSkill() {
		foreach (var item in leaderSkill.UserUnit.Values) {
			int id = item.GetPassiveSkill();
			if(id == -1) {
				continue;
			}
			IPassiveExcute ipe = GlobalData.tempNormalSkill[id] as IPassiveExcute;
			passiveSkill.Add(item.GetID,ipe);
			multipe.Add(item.GetID,item.AttackMultiple);
		}
	}

	Queue <TrapBase> trap = new Queue<TrapBase>();
	void DisposeTrapEvent(object data) {
		TrapBase tb = data as TrapBase;
		if (tb == null) {
			return;		
		}
		trap.Enqueue (tb);
		foreach (var item in passiveSkill.Values) {
			item.Excute(tb,this);
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
