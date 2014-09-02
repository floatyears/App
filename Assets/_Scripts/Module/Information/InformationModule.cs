using UnityEngine;
using System.Collections;

public class InformationModule : ModuleBase {
	
	public InformationModule( UIConfigItem config ) : base(   config ) {
		CreateUI<InformationView> ();
	}
	
	public override void InitUI() {
		base.InitUI();
	}
	public override void ShowUI()
	{
		base.ShowUI();
	}
	
	public override void HideUI()
	{
		base.HideUI();
	}
	
	public override void DestoryUI()
	{
		base.DestoryUI();
	}
}