using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectController : ModuleBase{
	public StageSelectController(string uiName):base(uiName){}

	public override void HideUI () {
		base.HideUI ();
//		Debug.LogError (UIManager.Instance.nextScene);
		if (!(UIManager.Instance.nextScene == ModuleEnum.QuestSelect)) {
			base.DestoryUI();
		}
	}
}
