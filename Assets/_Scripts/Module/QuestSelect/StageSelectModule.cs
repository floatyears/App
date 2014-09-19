using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectModule: ModuleBase{
	public StageSelectModule(UIConfigItem config, params object[] data):base(  config,data){
		CreateUI<StageSelectView> ();
	}
	public StageSelectModule(UIConfigItem config):base( config){
		CreateUI<StageSelectView> ();
	}

	public override void HideUI () {
		base.HideUI ();
//		Debug.LogError (UIManager.Instance.nextScene);
//		if (!(UIManager.Instance.nextScene == ModuleEnum.QuestSelectModule)) {
//			base.DestoryUI();
//		}
	}

	public override void OnReceiveMessages(params object[] data) {
		if( (data[0] as string) == "QuestSelected" ) {
			(view as StageSelectView).SetQuestSelected();
		}
	}
}
