using UnityEngine;
using System.Collections;
using bbproto;

public class ConfigTrap {
	public ConfigTrap () {
		ConfigTrapInfo ();
	}

	public void ConfigTrapInfo () {
		TrapInfo ti = new TrapInfo ();
		ti.trapID = 1;
		ti.trapType = ETrapType.Move;
		ti.effectType = 1;	// 1: blocking. 2: teleporter. 3: restart.
		MoveTrap mt = new MoveTrap (ti);
		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
						DataCenter.Instance.TrapInfo [ti.trapID] = mt;
				} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, mt);
		}


		ti = new TrapInfo ();
		ti.trapID = 5;
		ti.trapType = ETrapType.Move;
		ti.effectType = 2;	// 1: blocking. 2: teleporter. 3: restart.
		mt = new MoveTrap (ti);

		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
			DataCenter.Instance.TrapInfo [ti.trapID] = mt;
		} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, mt);
		}

//		DataCenter.Instance.TrapInfo.Add (ti.trapID, mt);

		ti = new TrapInfo ();
		ti.trapID = 6;
		ti.trapType = ETrapType.Move;
		ti.effectType = 3;	// 1: blocking. 2: teleporter. 3: restart.
		mt = new MoveTrap (ti);

		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
			DataCenter.Instance.TrapInfo [ti.trapID] = mt;
		} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, mt);
		}

//		DataCenter.Instance.TrapInfo.Add (ti.trapID, mt);

		ti = new TrapInfo ();
		ti.trapID = 2;
		ti.trapType = ETrapType.Injured;
		ti.effectType = 1;
		ti.valueIndex = 1;
		InjuredTrap it = new InjuredTrap (ti);

		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
			DataCenter.Instance.TrapInfo [ti.trapID] = it;
		} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, it);
		}

//		DataCenter.Instance.TrapInfo.Add (ti.trapID, it);

		ti = new TrapInfo ();
		ti.trapID = 3;
		ti.trapType = ETrapType.StateException;
		ti.valueIndex = 1;
		ti.effectType = 5; //use effectType as round=5 for Poison
		TrapPosion tp = new TrapPosion (ti);
//		DataCenter.Instance.TrapInfo.Add (ti.trapID, tp);

		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
			DataCenter.Instance.TrapInfo [ti.trapID] = tp;
		} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, tp);
		}

		ti = new TrapInfo ();
		ti.trapID = 4;
		ti.trapType = ETrapType.ChangeEnvir;
		ti.valueIndex = 2;
		EnvironmentTrap et = new EnvironmentTrap (ti);
//		DataCenter.Instance.TrapInfo.Add (ti.trapID, et);

		if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
			DataCenter.Instance.TrapInfo [ti.trapID] = et;
		} else {
			DataCenter.Instance.TrapInfo.Add (ti.trapID, et);
		}
	}

}
