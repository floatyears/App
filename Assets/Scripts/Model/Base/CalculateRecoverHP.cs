using UnityEngine;
using System.Collections.Generic;

public class CalculateRecoverHP {

	private TNormalSkill[] recoverHPSkill = new TNormalSkill[4];

	public CalculateRecoverHP() {
		for (int i = 0; i < 4; i++) {
//			Debug.LogError(29 - i);
			TNormalSkill tns = DataCenter.Instance.Skill [29 - i] as TNormalSkill;
			recoverHPSkill[i] = tns;
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
			 	ai = new AttackInfo ();
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
				ai = new AttackInfo ();
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
