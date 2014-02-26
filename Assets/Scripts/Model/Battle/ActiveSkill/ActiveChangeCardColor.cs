using UnityEngine;
using System.Collections;
using bbproto;

public class ActiveChangeCardColor : ActiveSkill, IActiveSkillExcute {
	private SkillConvertUnitType instance;
	public 	ActiveChangeCardColor(object instance) : base (instance) { 
		this.instance = instance as SkillConvertUnitType;
		skillBase = this.instance.baseInfo;	
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

	public object Excute (uint userUnitID, int atk = -1) {
		if (!coolingDone) {
			return null;
		}
		InitCooling ();
//		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType>();
		ChangeCardColor ccc = new ChangeCardColor ();
		if (instance.type == EValueType.RANDOMCOLOR) {
			MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
		}
		else if (instance.type == EValueType.COLORTYPE) {
			ccc.sourceType = (int)instance.unitType1;
			ccc.targetType = (int)instance.unitType2;
			MsgCenter.Instance.Invoke (CommandEnum.ChangeCardColor, ccc);
		}
		return null;
	}
}

public class ChangeCardColor {
	public int targetType = -1;
	public int sourceType = -1;
}