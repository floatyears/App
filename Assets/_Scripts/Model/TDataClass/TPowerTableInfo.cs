using UnityEngine;
using System.Collections;
using bbproto;

public class TPowerTableInfo : ProtobufDataBase {
	private PowerTable instance;

	public TPowerTableInfo(object instance) : base (instance) {
		this.instance = instance as PowerTable;
	}
	public int GetValue (int level) {
		PowerValue pv = instance.power.Find(a=>a.level == level);
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
