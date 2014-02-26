using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TSkillConvertUnitType : ProtobufDataBase {
	public TSkillConvertUnitType(object instance) : base (instance) {
		
	}
	
	public int SwitchCard (int type) {
		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType> ();
		if (scut.unitType2 == EUnitType.UALL) {
			List<int> range = new List<int>(Config.Instance.cardTypeID);// Config.Instance.cardTypeID
			range.Remove(type);
			int index = Random.Range(0,range.Count);
			type = range[index];
		}
		else if((int)scut.unitType1 == type) {
			type = (int)scut.unitType2;
		}
		
		return type;
	}
}
