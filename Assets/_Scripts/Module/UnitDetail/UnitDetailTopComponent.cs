using UnityEngine;
using System.Collections;

public class UnitDetailTopComponent : ModuleBase {
	
	public UnitDetailTopComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
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
	
	void CallBackUnitData (object data) {
		ExcuteCallback (data);
	}
}
