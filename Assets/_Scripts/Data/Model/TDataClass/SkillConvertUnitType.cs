using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class SkillConvertUnitType : SkillBase, ProtoBuf.IExtensible {
	
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
}