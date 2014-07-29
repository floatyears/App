using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelectController : ConcreteComponent{
	public StageSelectController(string uiName):base(uiName){}

	public override void HideUI () {
		base.HideUI ();
//		Debug.LogError (UIManager.Instance.nextScene);
		if (!(UIManager.Instance.nextScene == SceneEnum.QuestSelect)) {
			base.DestoryUI();
		}
	}
}
