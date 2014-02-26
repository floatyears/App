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
}
