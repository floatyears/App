﻿using UnityEngine;
using System.Collections;

public class ResourceDownloadComponent : ConcreteComponent {

	public ResourceDownloadComponent(string uiName):base(uiName){}
	
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
}