using UnityEngine;
using System.Collections;
using bbproto;

public class TPowerTableInfo : ProtobufDataBase {
	public TPowerTableInfo(object instance) : base (instance) {
		instance = instance as PowerTable;
	}
	PowerTable instance;
	public int GetValue (int level) {
		PowerValue pv = instance.power.Find(a=>a.level == level);
		if(pv == default(PowerValue)) {
			return -1;
		}
		else{
			return pv.value;
		}
	}
}
