using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillDelayTime : ActiveSkill {
		public SkillDelayTime(){
	//		skillBase = this.instance.baseInfo;
			if (skillCooling == 0) {
				coolingDone = true;
			}
		}
	//	public bool CoolingDone {
	//		get {
	//			return coolingDone;
	//		}
	//	}
	//	 
	//	public void RefreashCooling () {
	//		DisposeCooling ();
	//	}

		SkillDelayTime  sdt = null;

		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;
			}
			InitCooling ();
	//	 	sdt = DeserializeData<SkillDelayTime> ();
			MsgCenter.Instance.Invoke (CommandEnum.DelayTime, value);
			return null;
		}

		public float DelayTime{
			get {
				return value;
			}
		} 
	}
}