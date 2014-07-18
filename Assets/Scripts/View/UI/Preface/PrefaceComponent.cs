using UnityEngine;
using System.Collections;

public class PrefaceComponent : ConcreteComponent {

	public PrefaceComponent(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
}
