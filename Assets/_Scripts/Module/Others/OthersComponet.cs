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

		ShowUIAnimation ();
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
	
	void ShowUIAnimation(){
		view.transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
		iTween.MoveTo(view.gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
	}
	
}
