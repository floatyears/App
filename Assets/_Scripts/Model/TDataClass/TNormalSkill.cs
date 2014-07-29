﻿using bbproto;
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

//	public int CalculateNeedCard(List<int> haveSprite) {
//		int count = Mathf.Abs (haveSprite.Count - instance.activeBlocks.Count);
//		if (count != 1) { //only 
//			return -1;	
//		}
//	}
	
	public void GetSkillInfo(AttackInfo ai) {
		ai.SkillID = instance.baseInfo.id;
		ai.AttackRange = (int)instance.attackType;
		ai.NeedCardNumber = instance.activeBlocks.Count;
	}
	
	public int GetActiveBlocks() {
		return instance.activeBlocks.Count;
	}

	public List<uint> Blocks {
		get {
			return instance.activeBlocks;
		}
	}

	public void DisposeUseSkillID (List<int> skillID) {
		if (skillID.Contains (instance.baseInfo.id)) {
			skillID.Remove(instance.baseInfo.id);
		}
	}

	public void DisposeUseSkillID (List<TNormalSkill> skillID) {
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

	/// <summary>
	/// 0 = single attack. 1 = allattack. 2 = recover hp.
	/// </summary>
	/// <value>The attack range.</value>
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