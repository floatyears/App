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

		ti = new TrapInfo ();
		ti.trapID = 2;
		ti.trapType = ETrapType.Injured;
		ti.effectType = 1;
		ti.valueIndex = 1;

		ti = new TrapInfo ();
		ti.trapID = 3;
		ti.trapType = ETrapType.StateException;
		ti.valueIndex = 1;

		ti = new TrapInfo ();
		ti.trapID = 4;
		ti.trapType = ETrapType.ChangeEnvir;
		ti.valueIndex = 1;
	}

}
