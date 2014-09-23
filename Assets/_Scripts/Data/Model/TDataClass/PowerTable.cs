using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
public partial class PowerTable : ProtoBuf.IExtensible {

	public int GetValue (int level) {
		PowerValue pv = power.Find(a=>a.level == level);
		if(pv == default(PowerValue)) {
			return -1;
		}
		else{
			return pv.value;
		}
	}

	public const int UnitInfoExpType 		= 1;
	public const int UnitInfoAttackType 	= 2;
	public const int UnitInfoHPType 		= 3;
	public const int UserCostMax			 = 4;
	public const int UserExpType 			= 5;
}
}