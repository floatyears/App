using UnityEngine;
using System.Collections.Generic;

public class CalculateRecoverHP {

	private TNormalSkill[] recoverHPSkill = new TNormalSkill[4];

	public CalculateRecoverHP() {
		SkillBaseInfo recoverHP;
		for (int i = 1; i <= 4; i++) {
			if(DataCenter.Instance.Skill.TryGetValue(ConfigSkill.RECOVER_HP_ID + i, out recoverHP)) {
				TNormalSkill tns = recoverHP as TNormalSkill;
				recoverHPSkill[i-1] = tns;
			} else {
				Debug.LogError("recover hp not init : " + (ConfigSkill.RECOVER_HP_ID + i));
			}
		}
	}

	public AttackInfo RecoverHP (List<uint> card, List<int> ignorSkillID,int blood) {
		AttackInfo ai = null;
		List<uint> copyCard = new List<uint> (card);
		for (int i = 0; i < recoverHPSkill.Length; i++) {
			TNormalSkill tns = recoverHPSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			if(count > 0){
//				bbproto.AttackInfoProto aip = new bbproto.AttackInfoProto();
//				ai = new AttackInfo (aip);
				ai = AttackInfo.GetInstance();
				tns.GetSkillInfo(ai);
				ai.AttackValue = tns.GetRecoverHP(blood);
				ai.FixRecoverHP = true;
				break;
			}
			else{
				continue;
			}
		}
		return ai;
	}

	public AttackInfo RecoverHP (CalculateSkillUtility csu, int blood) {
		AttackInfo ai = null;
		List<uint> copyCard = new List<uint> (csu.haveCard);
		for (int i = 0; i < recoverHPSkill.Length; i++) {
			TNormalSkill tns = recoverHPSkill[i];
			tns.DisposeUseSkillID(csu.alreadyUseSkill);
			int count = tns.CalculateCard(copyCard);
			if(count > 0){
				csu.alreadyUseSkill.Add(tns);
				csu.ResidualCard();
//				bbproto.AttackInfoProto aip = new bbproto.AttackInfoProto();
//				ai = new AttackInfo (aip);
				ai = AttackInfo.GetInstance();
				tns.GetSkillInfo(ai);
				ai.AttackValue = tns.GetRecoverHP(blood);
				ai.FixRecoverHP = true;
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
