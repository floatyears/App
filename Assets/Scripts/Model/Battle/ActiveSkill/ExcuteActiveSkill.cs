using UnityEngine;
using System.Collections.Generic;

public class ExcuteActiveSkill {
	private Dictionary<string,IActiveSkillExcute> activeSkill = new Dictionary<string, IActiveSkillExcute> ();
	private ILeaderSkill leaderSkill;
	public ExcuteActiveSkill(ILeaderSkill ils) {
		leaderSkill = ils;
		foreach (var item in ils.UserUnit.Values) {
			if (item==null ){
				continue;
			}

			ProtobufDataBase pudb = DataCenter.Instance.Skill[item.ActiveSkill];
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
		TUserUnit uui = data as TUserUnit;
		if (uui != null) {
			string id = uui.MakeUserUnitKey();
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
