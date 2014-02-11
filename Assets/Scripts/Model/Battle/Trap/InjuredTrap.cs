using UnityEngine;
using System.Collections;
using bbproto;

public class InjuredTrap : ProtobufDataBase, ITrapExcute {
	public InjuredTrap (object instance) : base (instance) {

	}
	private const float probability = 0.9f;
	public void Excute () {
		TrapInfo ti = DeserializeData<TrapInfo> ();
		TrapInjuredValue tiv = null;
		switch (ti.effectType) {
		case 1:
			tiv = TrapInjuredInfo.Instance.FindMineInfo(ti.valueIndex);
			float value = DGTools.RandomToFloat();
			if(value <= probability) {
				MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, null);
			}
			else {
//				MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, 
			}
			break;
		case 2:
			tiv = TrapInjuredInfo.Instance.FindTrappingInfo(ti.valueIndex);
			break;
		case 3:
			tiv = TrapInjuredInfo.Instance.FindHungryInfo(ti.valueIndex);
			break;
		case 4:
			tiv = TrapInjuredInfo.Instance.FindLostMoney(ti.valueIndex);
			break;
		}
	}
}
