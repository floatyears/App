using UnityEngine;
using System.Collections.Generic;

public class ExcuteActiveSkill {
	private Dictionary<string,IActiveSkillExcute> activeSkill = new Dictionary<string, IActiveSkillExcute> ();
	private ILeaderSkill leaderSkill;

	private IActiveSkillExcute iase;
	private TUserUnit userUnit;

	public ExcuteActiveSkill(ILeaderSkill ils) {
		leaderSkill = ils;
		foreach (var item in ils.UserUnit.Values) {
			if (item==null ){
				continue;
			}

			ProtobufDataBase pudb = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(),item.ActiveSkill, SkillType.ActiveSkill); //Skill[item.ActiveSkill];
			IActiveSkillExcute skill = pudb as IActiveSkillExcute;
			if(skill == null) {
				continue;
			}
			activeSkill.Add(item.MakeUserUnitKey(), skill);
		}
		MsgCenter.Instance.AddListener (CommandEnum.LaunchActiveSkill, Excute);
//		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);	// move role one step, cooling reduce one round.
		MsgCenter.Instance.AddListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);	// this command use to reduce cooling one round.
		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, ShowHands);	// one normal skill can reduce cooling one round.
//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	public void RemoveListener (){
		MsgCenter.Instance.RemoveListener (CommandEnum.LaunchActiveSkill, Excute);
//		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);
		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, ShowHands);
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	public IActiveSkillExcute GetActiveSkill(string userUnitID) {
		IActiveSkillExcute iase = null;
		activeSkill.TryGetValue (userUnitID, out iase);
		return iase;
	}

	void Excute(object data) {
		userUnit = data as TUserUnit;
		if (userUnit != null) {
			string id = userUnit.MakeUserUnitKey();
			if(activeSkill.TryGetValue(id,out iase)) {
				MsgCenter.Instance.Invoke(CommandEnum.StateInfo, DGTools.stateInfo[4]);
				Debug.LogError("excute active skill : " + userUnit);
				MsgCenter.Instance.Invoke(CommandEnum.ShowActiveSkill, userUnit);			
				GameTimer.GetInstance().AddCountDown(AttackEffect.activeSkillEffectTime, WaitActiveEffect);
			}
		}
	}
	
     void WaitActiveEffect() {
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, true);
		GameTimer.GetInstance().AddCountDown(1f,Excute);
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillStandReady, userUnit);
	}


	void Excute() {
		if (iase == null || userUnit == null) {
			return;	
		} 

		AttackInfo ai = AttackInfo.GetInstance (); //new AttackInfo();
		ai.UserUnitID = userUnit.MakeUserUnitKey();
		MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
		iase = activeSkill[ai.UserUnitID];
		iase.Excute(ai.UserUnitID, userUnit.Attack);
		iase = null;
		userUnit = null;

		GameTimer.GetInstance ().AddCountDown (3f, ActiveSkillEnd);
	}

	void ActiveSkillEnd() {
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, false);
	}

	void MoveToMapItem(object data) {
		CoolingSkill ();
	}

	void ReduceActiveSkillRound(object data) {
		CoolingSkill ();
	}

	void ShowHands(object data) {
		int count = (int)data;
		for (int i = 0; i < count; i++) {
			CoolingSkill ();
		}
	}
	

	public void CoolingSkill () {
		foreach (var item in activeSkill.Values) {
			item.RefreashCooling();
		}
	}
}
