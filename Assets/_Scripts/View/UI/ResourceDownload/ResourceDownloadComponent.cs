using UnityEngine;
using System.Collections;

public class ResourceDownloadComponent : ConcreteComponent {

	public ResourceDownloadComponent(string uiName):base(uiName){}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
}
