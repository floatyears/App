using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public enum GachaFailedType {
    FriendGachaPointNotEnough = 1,
    FriendGachaUnitCountReachedMax,
    RareGachaStoneNotEnough,
    RareGachaUnitCountReachedMax,
    EventGachaNotOpen,
    EventGachaStoneNotEnough,
    EventGachaUnitCountReachedMax,
}

public enum GachaType{
    FriendGacha = 1,
    RareGacha = 2,
    EventGacha = 3,
}

public class ScratchModule : ModuleBase {


	public ScratchModule(UIConfigItem config):base(  config) {
		CreateUI<ScratchView> ();
	}

   
}