using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Collections;



public class CalculateSkillUtility {
	public List<uint> haveCard = new List<uint>();
	public List<NormalSkill> alreadyUseSkill = new List<NormalSkill>();

	public List<uint> residualCard;

	public List<uint> ResidualCard () {
//		if (haveCard.Count == 5) {
//			residualCard = null;
//		} else {
//			residualCard = new List<uint> (haveCard);
//			
//			for (int i = 0; i < alreadyUseSkill.Count; i++) {
//				List<uint> blocks = alreadyUseSkill[i].Blocks;
//				for (int j = 0; j < blocks.Count; j++) {
//					residualCard.Remove(blocks[j]);
//				}
//			}
//		}

		return residualCard;
	}
}
