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
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

	public void RemoveListener (){
		MsgCenter.Instance.RemoveListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	}

	void Excute(object data) {
		userUnit = data as TUserUnit;
		if (userUnit != null) {
			string id = userUnit.MakeUserUnitKey();
			if(activeSkill.TryGetValue(id,out iase)) {
				GameTimer.GetInstance().AddCountDown(1f,Excute);
				MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillStandReady, userUnit);
			}
		}
	}

	void Excute() {
		if (iase == null || userUnit == null) {
			return;	
		} 

		AttackInfo ai = new AttackInfo();
		ai.UserUnitID = userUnit.MakeUserUnitKey();
		MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
		MsgCenter.Instance.Invoke(CommandEnum.StateInfo, DGTools.stateInfo[4]);
		iase = activeSkill[ai.UserUnitID];
		iase.Excute(ai.UserUnitID, userUnit.Attack);
		iase = null;
		userUnit = null;
	}

	void MoveToMapItem(object data) {
		CoolingSkill ();
	}

	void BattleEnd(object data) {
		CoolingSkill ();
	}

	public void CoolingSkill () {
		foreach (var item in activeSkill.Values) {
			item.RefreashCooling();
		}
	}
}
