using UnityEngine;
using System.Collections;

public class MaskController : ConcreteComponent {

	public MaskController(string name) : base(name){}
	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	} 

	public override void Callback(object data){
		base.Callback(data);
	}


}
