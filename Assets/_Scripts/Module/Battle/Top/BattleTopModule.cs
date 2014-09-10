using UnityEngine;
using System.Collections;

public class BattleTopModule : ModuleBase {

	public BattleTopModule(UIConfigItem config):base(config){
		CreateUI<BattleTopView> ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
		case "refresh":
			(view as BattleTopView).RefreshTopUI();
			break;
		case "setfloor":
			(view as BattleTopView).SetFloor();
			break;
		default:
				break;
		}
	}
}
