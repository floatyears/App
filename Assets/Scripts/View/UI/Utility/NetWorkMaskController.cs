using UnityEngine;
using System.Collections;

public class NetWorkMaskController : MaskController {

	public NetWorkMaskController(string name) : base(name){}

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
