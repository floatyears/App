using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OthersModule : ModuleBase
{
	private Dictionary< object, object > optionsDic = new Dictionary<object, object>();

	public OthersModule(UIConfigItem config):base(  config){
		CreateUI<OthersView> ();
	}

	public override void InitUI(){
		base.InitUI(); 
	}

	public override void ShowUI(){
	  	base.ShowUI();
	}

	public override void HideUI(){
	//		UnityEngine.Debug.LogError("HideScene");
		base.HideUI();

	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	public void SetOtherOptions(object message){

	}
	
}
