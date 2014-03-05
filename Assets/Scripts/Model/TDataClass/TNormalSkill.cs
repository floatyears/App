using bbproto;
using System.Collections.Generic;
using UnityEngine;

public class TNormalSkill : SkillBaseInfo {
	private NormalSkill instance;
	public TNormalSkill (object instance) : base (instance) {
		this.instance = instance as NormalSkill;
		skillBase = this.instance.baseInfo;
	}
	
	//static int record = 0;
	public int CalculateCard (List<uint> count, int record = 0) {
		while (count.Count >= instance.activeBlocks.Count) {
			bool isExcuteSkill =  DGTools.IsTriggerSkill<uint> (count, instance.activeBlocks);
			if (isExcuteSkill) {
				record++;
				for (int i = 0; i < instance.activeBlocks.Count; i++) {
					count.Remove(instance.activeBlocks[i]);
				}
			}
			else {
				break;
			}
		}
		return record;
	}
	
	public void GetSkillInfo(AttackInfo ai) {
		ai.SkillID = instance.baseInfo.id;
		ai.AttackRange = (int)instance.attackType;
		ai.NeedCardNumber = instance.activeBlocks.Count;
	}
	
	public int GetActiveBlocks() {
		return instance.activeBlocks.Count;
	}
	
	public void DisposeUseSkillID (List<int> skillID) {
		if (skillID.Contains (instance.baseInfo.id)) {
			skillID.Remove(instance.baseInfo.id);
		}
	}

	public int AttackType {
		get {
			return (int)instance.attackUnitType;
		}
	}

	public NormalSkill Object {
		get{
			return  instance;
		}
	}
	
	public int SkillID {
		get{
			return Object.baseInfo.id;
		}
	}
	
	public int AttackRange {
		get{
			return (int)Object .attackType;
		}
	}
	
	public string Name {
		get{
			return Object.baseInfo.name;
		}
	}
	
	public int GetRecoverHP (int blood) {
		return System.Convert.ToInt32 (blood * Object .attackValue);
	}
	
	public float GetAttack (float userUnitAttack) {
		return userUnitAttack * Object.attackValue;
	}

//	public SkillBase GetSkillInfo()
}