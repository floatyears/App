using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
	public partial class SkillConvertUnitType : ActiveSkill {
		public 	SkillConvertUnitType(int dummy=0) { 
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

		public override object Excute (string userUnitID, int atk = -1) {
			if (!coolingDone) {
				return null;
			}
			InitCooling ();
	//		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType>();
			ChangeCardColor ccc = new ChangeCardColor ();
			if (type == EValueType.RANDOMCOLOR) {
				MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
			}
			else if (type == EValueType.COLORTYPE) {
				ccc.sourceType = (int)unitType1;
				ccc.targetType = (int)unitType2;
				MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
			}
			return null;
		}

		public int SwitchCard (int type) {
			//		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType> ();
			if (unitType2 == EUnitType.UALL) {
				List<int> range = new List<int>(BattleConfigData.Instance.cardTypeID);// Config.Instance.cardTypeID
				range.Remove(type);
				int index = Random.Range(0,range.Count);
				type = range[index];
			}
			else if((int)unitType1 == type) {
				type = (int)unitType2;
			}
			
			return type;
		}
	}

	public class ChangeCardColor {
		public int targetType = -1;
		public int sourceType = -1;
	}
}