using UnityEngine;
using System.Collections.Generic;

public class CalculateRecoverHP {

	private TempNormalSkill[] recoverHPSkill = new TempNormalSkill[4];

	public CalculateRecoverHP() {
		for (int i = 0; i < 4; i++) {
			TempNormalSkill tns = GlobalData.tempNormalSkill [29 - i] as TempNormalSkill;
			recoverHPSkill[i] = tns;
		}

	}

	public AttackInfo RecoverHP (List<uint> card, List<int> ignorSkillID,int blood) {
		AttackInfo ai = null;
		List<uint> copyCard = new List<uint> (card);
		for (int i = 0; i < recoverHPSkill.Length; i++) {
			TempNormalSkill tns = recoverHPSkill[i];
			tns.DisposeUseSkillID(ignorSkillID);
			int count = tns.CalculateCard(copyCard);
			if(count > 0){
			 	ai = new AttackInfo ();
				tns.GetSkillInfo(ai);
				ai.AttackValue = tns.GetRecoverHP(blood);
				break;
			}
			else{
				continue;
			}
		}
		return ai;
	}
}
