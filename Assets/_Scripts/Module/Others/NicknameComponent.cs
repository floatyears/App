using UnityEngine;
using System.Collections;

public class NicknameModule : ModuleBase {

	public NicknameModule(UIConfigItem config):base(  config){}
	
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
}
