using UnityEngine;
using System.Collections;

public class TipsBarModule : ModuleBase  {

	public TipsBarModule(UIConfigItem config):base(  config) {
		CreateUI<TipsBarView> ();
	}

	public override void InitUI () {
		base.InitUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
}
