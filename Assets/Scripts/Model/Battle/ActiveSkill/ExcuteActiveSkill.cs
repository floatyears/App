using UnityEngine;
using System.Collections.Generic;

public class ExcuteActiveSkill {
	private Dictionary<uint,IActiveSkillExcute> activeSkill = new Dictionary<uint, IActiveSkillExcute> ();
	private ILeaderSkill leaderSkill;
	public ExcuteActiveSkill(ILeaderSkill ils) {
		leaderSkill = ils;
		foreach (var item in ils.UserUnit.Values) {
			ProtobufDataBase pudb = GlobalData.tempNormalSkill[item.GetActiveSkill()];
			IActiveSkillExcute skill = pudb as IActiveSkillExcute;
			if(skill == null) {
//				Debug.LogError("this userunit : " + item.GetID + " active skill id is error : " +item.GetActiveSkill());
				continue;
			}

			activeSkill.Add(item.GetID,skill);
		}
		MsgCenter.Instance.AddListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
	}

	~ExcuteActiveSkill() {
		MsgCenter.Instance.RemoveListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
	}

	void Excute(object data) {
		UserUnitInfo uui = data as UserUnitInfo;
		if (uui != null) {
			uint id = uui.GetID;
			activeSkill[id].Excute(id, uui.GetAttack);
		}
	}

	void MoveToMapItem(object data) {
//		Debug.LogError ("MoveToMapItem");
		CoolingSkill ();
	}

	void BattleEnd(object data) {
//		Debug.LogError ("BattleEnd");
		CoolingSkill ();
	}

	public void CoolingSkill () {
		foreach (var item in activeSkill.Values) {
			item.RefreashCooling();
		}
	}
}
