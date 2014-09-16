using UnityEngine;
using System.Collections;

public class BattleEnemyModule : ModuleBase {

	public BattleEnemyModule(UIConfigItem config,params object[] data):base(config,data){
		CreateUI<BattleEnemyView> ();
	}

	public override void OnReceiveMessages (params object[] data)
	{
		base.OnReceiveMessages (data);
	}
}
