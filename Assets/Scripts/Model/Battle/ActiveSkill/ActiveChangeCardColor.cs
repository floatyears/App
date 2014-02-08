using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveChangeCardColor : ActiveSkill, IActiveSkillExcute {
	public 	ActiveChangeCardColor(object instance) : base (instance) { 
		skillBase = DeserializeData<SkillConvertUnitType> ().baseInfo;	
		if (skillBase.skillCooling == 0) {
			coolingDone = true;
		}
	}
	public bool CoolingDone {
		get {
			return coolingDone;
		}
	}

	public void RefreashCooling () {
		DisposeCooling ();
	}

	public object Excute (int userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType>();
		ChangeCardColor ccc = new ChangeCardColor ();
		if (scut.type == EValueType.RANDOMCOLOR) {
			MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
		}
		else if (scut.type == EValueType.COLORTYPE) {
			ccc.sourceType = (int)scut.unitType1;
			ccc.targetType = (int)scut.unitType2;
			MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
		}
		return null;
	}
}

public class ChangeCardColor {
	public int targetType = -1;
	public int sourceType = -1;
}