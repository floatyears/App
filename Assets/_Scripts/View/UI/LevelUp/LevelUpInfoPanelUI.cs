using UnityEngine;
using System.Collections;

public class LevelUpInfoPanelUI : ConcreteComponent{
	public LevelUpInfoPanelUI(string uiName):base(uiName) {}
	public override void CreatUI(){
		base.CreatUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}
  
}

