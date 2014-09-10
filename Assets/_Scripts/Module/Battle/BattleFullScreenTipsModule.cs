using UnityEngine;
using System.Collections;

public class BattleFullScreenTipsModule : ModuleBase {

	public BattleFullScreenTipsModule(UIConfigItem config):base(config){
		CreateUI<BattleFullScreenTipsView> ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
		case "boss":
			break;
		case "gate":
			break;
		case "first":
			break;
		case "back":
			break;
		default:
				break;
		}
	}
}
