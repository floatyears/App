using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectModule: ModuleBase{
	public StageSelectModule(UIConfigItem config, params object[] data):base(  config,data){
		CreateUI<StageSelectView> ();
	}

	public override void HideUI () {
		base.HideUI ();
//		Debug.LogError (UIManager.Instance.nextScene);
//		if (!(UIManager.Instance.nextScene == ModuleEnum.QuestSelectModule)) {
//			base.DestoryUI();
//		}
	}
}
