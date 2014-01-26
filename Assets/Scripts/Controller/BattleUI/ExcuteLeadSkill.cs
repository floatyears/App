using UnityEngine;
using System.Collections;

public class ExcuteLeadSkill {

	ILeadSkill leadSkill;

	public ExcuteLeadSkill (ILeadSkill lead) {
		leadSkill = lead;
//		Debug.LogError ("leadSkill : " + leadSkill);
	}

	public void Excute() {
		if (leadSkill.LeadSkill.Count > 0) {
			DisposeBoostSkill(leadSkill.LeadSkill[0]);	
		}
			
	}
	
	void DisposeBoostSkill (ProtobufDataBase pdb) {
		TempBoostSkill tbs = pdb as TempBoostSkill;
		if (tbs != null) {
			leadSkill.LeadSkill.RemoveAt (0);
			for (int i = 0; i < leadSkill.UserUnit.Count; i++) {
					leadSkill.UserUnit [i].SetAttack (tbs.GetBoostValue, tbs.GetTargetValue, tbs.GetTargetType, tbs.GetBoostType);
			}
			Excute ();
		} 
		else {
			DisposeDelayOperateTime(pdb);
		}


	}

	void DisposeDelayOperateTime (ProtobufDataBase pdb) {
		TempSkillTime tst = pdb as TempSkillTime;
//		Debug.LogError ("TST : " + tst);
		if (tst != null) {
			leadSkill.LeadSkill.Remove(tst);
//			Debug.LogError("tst.DelayTime : " + tst.DelayTime);
			MsgCenter.Instance.Invoke(CommandEnum.LeaderSkillDelayTime, tst.DelayTime);
			Excute ();
		}
	}


}

