using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class ConfigSkill  {
	public const int RECOVER_HP_ID = 100;
	public ConfigSkill() {
		ConfigHeartSkill ();
	}


	void ConfigHeartSkill () {
		//General recoverHP skillId: [101 - 104]
		Dictionary<int,SkillBase> infoList = new Dictionary<int, SkillBase> ();
		for (int i = 1; i <= 4; i++) {
			NormalSkill ns			= new NormalSkill ();
			ns.baseInfo				= new SkillBase ();
			ns.baseInfo.id			= RECOVER_HP_ID+i;
			ns.attackValue = 0.15f * i;
			if (i==3) { //4 hearts
				ns.attackValue = 0.6f;
			}else if (i==4) { //5 hearts
				ns.attackValue = 1.0f;
			}

			ns.baseInfo.name		= TextCenter.GetText("RecoverHpSkillName", i);
			ns.baseInfo.description = TextCenter.GetText("RecoverHpSkillDesc", (int)(ns.attackValue*100) );

			ns.attackType = EAttackType.RECOVER_HP;
			for (int j = 0; j <= i; j++) {
				ns.activeBlocks.Add( (int)bbproto.EUnitType.UHeart );
			}

			NormalSkill tns = ns;
			if(infoList.ContainsKey(ns.baseInfo.id)) {
				infoList[ns.baseInfo.id] = tns;
			}else{
				infoList.Add(ns.baseInfo.id, tns);
			}
		}

		DataCenter.Instance.SetData(ModelEnum.Skill,infoList);
	}
}