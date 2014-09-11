using UnityEngine;
using System.Collections;

public class BattleBottomModule : ModuleBase {

	public BattleBottomModule(UIConfigItem config):base(config){
		CreateUI<BattleBottomView> ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
		case "initdata":
			(view as BattleBottomView).InitData((BattleBaseData)data[1]);
			break;
		case "playerdead":
			(view as BattleBottomView).PlayerDead();
			break;
		default:
				break;
		}
	}
}
