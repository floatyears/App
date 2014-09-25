using bbproto;
using System.Collections.Generic;
using UnityEngine;

namespace bbproto{
public partial class NormalSkill : SkillBase{

	
	//static int record = 0;
	public int CalculateCard (List<uint> count, int record = 0) {
		while (count.Count >= activeBlocks.Count) {
			bool isExcuteSkill =  DGTools.IsTriggerSkill<uint> (count, activeBlocks);
			if (isExcuteSkill) {
				record++;
				for (int i = 0; i < activeBlocks.Count; i++) {
					count.Remove(activeBlocks[i]);
				}
			}
			else {
				break;
			}
		}
		return record;
	}

//	public int CalculateNeedCard(List<int> haveSprite) {
//		int count = Mathf.Abs (haveSprite.Count - instance.activeBlocks.Count);
//		if (count != 1) { //only 
//			return -1;	
//		}
//	}
	
	public void GetSkillInfo(AttackInfoProto ai) {
		ai.skillID = baseInfo.id;
		ai.attackRange = (int)attackType;
		ai.needCardNumber = activeBlocks.Count;
	}
	
	public int GetActiveBlocks() {
		return activeBlocks.Count;
	}

	public List<uint> Blocks {
		get {
			return activeBlocks;
		}
	}

	public void DisposeUseSkillID (List<int> skillID) {
		if (skillID.Contains (baseInfo.id)) {
			skillID.Remove(baseInfo.id);
		}
	}

	public void DisposeUseSkillID (List<NormalSkill> skillID) {
		if (skillID.Contains (this)) {
			skillID.Remove(this);
		}
	}
	/// <summary>
	/// 0=all, 1=fire, 2=water, 3=wind, 4=light, 5=dark, 6=none, 7=heart.
	/// </summary>
	/// <value>The type of the attack.</value>
	public int AttackType {
		get {
			return (int)attackUnitType;
		}
	}
	
	public int SkillID {
		get{
			return baseInfo.id;
		}
	}

	/// <summary>
	/// 0 = single attack. 1 = allattack. 2 = recover hp.
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get{
			return (int)attackType;
		}
	}
	
	public string Name {
		get{
			return baseInfo.name;
		}
	}
	
	public int GetRecoverHP (int blood) {
		return System.Convert.ToInt32 (blood * attackValue);
	}
	
	public float GetAttack (float userUnitAttack) {
		return userUnitAttack * attackValue;
	}

//	public SkillBase GetSkillInfo()
}
}