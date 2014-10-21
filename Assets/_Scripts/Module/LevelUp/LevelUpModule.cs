using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class LevelUpModule : ModuleBase {
	public LevelUpModule(UIConfigItem config) : base( config) {
//		CreateUI<LevelUpView> ();
	}

	public override void HideUI () {
		base.HideUI ();

//		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
//			base.DestoryUI();
//		}
	}
	
	List<UserUnit> levelUpInfo = null;


}
