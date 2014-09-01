using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OthersComponent : ModuleBase
{
	private Dictionary< object, object > optionsDic = new Dictionary<object, object>();

	public OthersComponent(string uiName):base(uiName){}

	public override void CreatUI(){
		base.CreatUI(); 
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
