using UnityEngine;
using System.Collections;

public class QuestSelectModule : ModuleBase {
	public QuestSelectModule(UIConfigItem config) : base(  config){
		CreateUI<QuestSelectView> ();
	}
}
