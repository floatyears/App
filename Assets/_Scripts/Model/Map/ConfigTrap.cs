using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ConfigTrap {
	public ConfigTrap () {

		ConfigTrapInfo ();
	}

	public void ConfigTrapMove () {
		Dictionary<uint,TrapBase> trapList = new Dictionary<uint, TrapBase> ();

		TrapInfo ti = new TrapInfo ();
		ti.trapID = 11; // type1, lv1
		ti.trapType = ETrapType.Move;
		ti.effectType = 1;	// 1: blocking. 2: teleporter. 3: restart.
		MoveTrap mt = new MoveTrap (ti);
		if (trapList.ContainsKey (ti.trapID)) {
			trapList[ti.trapID] = mt;
		} else {
			trapList.Add (ti.trapID, mt);
		}

		ti = new TrapInfo ();
		ti.trapID = 23; //type2, lv=3
		ti.trapType = ETrapType.Move;
		ti.effectType = 2;	// 2: teleporter
		mt = new MoveTrap (ti);
		if (trapList.ContainsKey (ti.trapID)) {
			trapList [ti.trapID] = mt;
		} else {
			trapList.Add (ti.trapID, mt);
		}

		ti = new TrapInfo ();
		ti.trapID = 94; //type9, lv=4
		ti.trapType = ETrapType.Move;
		ti.effectType = 3;	// 3: restart: move back to start point
		mt = new MoveTrap (ti);


		if (trapList.ContainsKey (ti.trapID)) {
			trapList [ti.trapID] = mt;
		} else {
			trapList.Add (ti.trapID, mt);
		}
		DataCenter.Instance.SetData (ModelEnum.TrapInfo, trapList);
	}

	public void ConfigTrapInjured (int effectType) {
		// effectType = 1, 2, 3, 4
		for(uint index=0; index<10; index++) {
			TrapInfo ti = new TrapInfo ();
			ti.effectType = effectType;
			ti.valueIndex = (int)index;

			uint trapid = 2 + (uint)effectType; //convert: 1,2,3,4 -> 3, 4, 5, 6
			ti.trapID = trapid*10 + index;
			ti.trapType = ETrapType.Injured;

			InjuredTrap it = new InjuredTrap (ti);
			if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
				DataCenter.Instance.TrapInfo [ti.trapID] = it;
			} else {
				DataCenter.Instance.TrapInfo.Add (ti.trapID, it);
			}
		}
	}

	public void ConfigTrapChangeEnvir () {
		uint trapid = 7;

		for(uint index=0; index<10; index++) {
			TrapInfo ti = new TrapInfo ();
			ti.trapID = trapid*10 + index;
			ti.trapType = ETrapType.ChangeEnvir;
			ti.valueIndex =  (int)index;
			EnvironmentTrap et = new EnvironmentTrap (ti);
			
			if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
				DataCenter.Instance.TrapInfo [ti.trapID] = et;
			} else {
				DataCenter.Instance.TrapInfo.Add (ti.trapID, et);
			}
		}
	}
	
	public void ConfigTrapStateException () {
		uint trapid = 8;
		for(uint index=0; index<10; index++) {
			TrapInfo ti = new TrapInfo ();
			ti.trapID = trapid*10 + index;
			ti.trapType = ETrapType.StateException;
			ti.valueIndex =  (int)index;
			ti.effectType = 5; //PS: use effectType as round=5 for Poison
			TrapPosion tp = new TrapPosion (ti);

			if (DataCenter.Instance.TrapInfo.ContainsKey (ti.trapID)) {
				DataCenter.Instance.TrapInfo [ti.trapID] = tp;
			} else {
				DataCenter.Instance.TrapInfo.Add (ti.trapID, tp);
			}
		}
	}

	public void ConfigTrapInfo () {
		ConfigTrapMove ();

		//TrapInjured effectType:
		//		mineInfo		= 1;
		//		trappingInfo	= 2;
		//		hungryInfo		= 3;
		//		lostMoney		= 4;
		for(int effectType = 1; effectType <= 4; effectType++) {
			ConfigTrapInjured( effectType );
		}

		ConfigTrapStateException();

		ConfigTrapChangeEnvir();
	}

}
