using bbproto;
using System.Collections.Generic;
using UnityEngine;

public class TNormalSkill : ProtobufDataBase {
	public TNormalSkill (object instance) : base (instance) {
		
	}
	
	//static int record = 0;
	public int CalculateCard (List<uint> count, int record = 0) {
		NormalSkill ns = DeserializeData<NormalSkill> ();
		
		while (count.Count >= ns.activeBlocks.Count) {
			bool isExcuteSkill =  DGTools.IsTriggerSkill<uint> (count, ns.activeBlocks);
			if (isExcuteSkill) {
				record++;
				for (int i = 0; i < ns.activeBlocks.Count; i++) {
					count.Remove(ns.activeBlocks[i]);
				}
			}
			else {
				break;
			}
		}
		
		//Debug.LogWarning("record -- : " + record + "``ns : " + ns.baseInfo.name);
		return record;
	}
	
	public void GetSkillInfo(AttackInfo ai) {
		NormalSkill ns = GetObject ();
		ai.SkillID = ns.baseInfo.id;
		ai.AttackRange = (int)ns.attackType;
		ai.NeedCardNumber = ns.activeBlocks.Count;
	}
	
	public int GetActiveBlocks() {
		NormalSkill ns = DeserializeData<NormalSkill> ();
		return ns.activeBlocks.Count;
	}
	
	public void DisposeUseSkillID (List<int> skillID) {
		NormalSkill ns = DeserializeData<NormalSkill> ();
		if (skillID.Contains (ns.baseInfo.id)) {
			skillID.Remove(ns.baseInfo.id);
		}
	}
	
	NormalSkill GetObject() {
		return  DeserializeData<NormalSkill> ();
	}
	
	public int GetID() {
		return GetObject().baseInfo.id;
	}
	
	public int GetAttackRange() {
		return (int)GetObject ().attackType;
	}
	
	public string GetName () {
		return GetObject ().baseInfo.name;
	}
	
	public int GetRecoverHP (int blood) {
		return System.Convert.ToInt32 (blood * GetObject ().attackValue);
	}
	
	public float GetAttack (float userUnitAttack) {
		return userUnitAttack * GetObject ().attackValue;
	}
}