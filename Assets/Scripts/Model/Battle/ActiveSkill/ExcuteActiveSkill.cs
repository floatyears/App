using UnityEngine;
using System.Collections.Generic;

public class ExcuteActiveSkill {
	private Dictionary<uint,IActiveSkillExcute> activeSkill = new Dictionary<uint, IActiveSkillExcute> ();
	private ILeaderSkill leaderSkill;
	public ExcuteActiveSkill(ILeaderSkill ils) {
		leaderSkill = ils;
//		Debug.Log("this userunit : " + ils.UserUnit.Count);
		foreach (var item in ils.UserUnit.Values) {
			ProtobufDataBase pudb = DataCenter.Instance.Skill[item.ActiveSkill];
			IActiveSkillExcute skill = pudb as IActiveSkillExcute;
//			Debug.Log("pudb : " + skill + " active skill id is error : " +item.ActiveSkill);
			if(skill == null) {
				continue;
			}
//			Debug.Log("this userunit : " + item.ID + " active skill id is error : " +item.ActiveSkill);
			activeSkill.Add(item.ID,skill);
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
		TUserUnit uui = data as TUserUnit;
		if (uui != null) {
			uint id = uui.ID;
//			Debug.LogError("id : " + id);
			IActiveSkillExcute iase ;
			if(activeSkill.TryGetValue(id,out iase)) {
				iase = activeSkill[id];
				iase.Excute(id, uui.Attack);
			}
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
//		Debug.LogError (activeSkill.Values.Count);
		foreach (var item in activeSkill.Values) {
//			Debug.LogError("CoolingSkill : " + item);
			item.RefreashCooling();
		}
	}
}
