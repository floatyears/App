using UnityEngine;
using System.Collections.Generic;
using bbproto;


public class CalculateRecoverHP {

	private NormalSkill[] recoverHPSkill = new NormalSkill[4];

	public CalculateRecoverHP() {
		SkillBase recoverHP;
		for (int i = 1; i <= 4; i++) {
			if(DataCenter.Instance.BattleData.Skill.TryGetValue(ConfigSkill.RECOVER_HP_ID + i, out recoverHP)) {
				NormalSkill tns = recoverHP as NormalSkill;
				recoverHPSkill[i-1] = tns;
			} else {
				Debug.LogError("recover hp not init : " + (ConfigSkill.RECOVER_HP_ID + i));
			}
		}
	}

	public AttackInfoProto RecoverHP (List<uint> card, List<int> ignorSkillID,int blood) {
		AttackInfoProto ai = null;
		List<uint> copyCard = new List<uint> (card);
		for (int i = 0; i < recoverHPSkill.Length; i++) {
			NormalSkill tns = recoverHPSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			if(count > 0){
//				bbproto.AttackInfoProto aip = new bbproto.AttackInfoProto();
//				ai = new AttackInfo (aip);
				ai = new AttackInfoProto();
				tns.GetSkillInfo(ai);
				ai.attackValue = tns.GetRecoverHP(blood);
				ai.fixRecoverHP = true;
				break;
			}
			else{
				continue;
			}
		}
		return ai;
	}

	public AttackInfoProto RecoverHP (CalculateSkillUtility csu, int blood) {
		AttackInfoProto ai = null;
		List<uint> copyCard = new List<uint> (csu.haveCard);
		for (int i = 0; i < recoverHPSkill.Length; i++) {
			NormalSkill tns = recoverHPSkill[i];
			tns.DisposeUseSkillID(csu.alreadyUseSkill);
			int count = tns.CalculateCard(copyCard);
			if(count > 0){
				csu.alreadyUseSkill.Add(tns);
				csu.ResidualCard();
//				bbproto.AttackInfoProto aip = new bbproto.AttackInfoProto();
//				ai = new AttackInfo (aip);
				ai = new AttackInfoProto();
				tns.GetSkillInfo(ai);
				ai.attackValue = tns.GetRecoverHP(blood);
				ai.fixRecoverHP = true;
				break;
			}
			else{
				continue;
			}
		}
		return ai;
	}

	public int RecoverHPNeedCard (CalculateSkillUtility csu) {
		if (csu.haveCard.Count < 5) {
			int index = -1;
			foreach (var item in recoverHPSkill) {
				index = DGTools.NeedOneTriggerSkill(csu.haveCard, item.Blocks);

				if(index != -1) {
					break;
				}
			}
			return index;

		} else {
			return -1;	
		}
	}
}
