using UnityEngine;
using System.Collections;

public class QuestSelectModule : ModuleBase {
	public QuestSelectModule(UIConfigItem config, params object[] data) : base(  config,data){
		CreateUI<QuestSelectView> ();
	}
}
