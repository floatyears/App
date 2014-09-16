using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TSkillConvertUnitType : SkillBaseInfo {
	private SkillConvertUnitType instance;
	public TSkillConvertUnitType(object instance) : base (instance) {
		this.instance = instance as SkillConvertUnitType;
		skillBase = this.instance.baseInfo;
	}
	
	public int SwitchCard (int type) {
//		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType> ();
		if (instance.unitType2 == EUnitType.UALL) {
			List<int> range = new List<int>(BattleConfigData.Instance.cardTypeID);// Config.Instance.cardTypeID
			range.Remove(type);
			int index = Random.Range(0,range.Count);
			type = range[index];
		}
		else if((int)instance.unitType1 == type) {
			type = (int)instance.unitType2;
		}
		
		return type;
	}
}
