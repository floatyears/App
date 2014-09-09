using UnityEngine;
using System.Collections;

public class LevelUpComponent : ModuleBase {

	public LevelUpComponent(UIConfigItem config):base(  config) {}
	
	public override void InitUI () {
		base.InitUI ();
//		Debug.LogError ("LevelUpComponent : CreatUI");
	}
	
	public override void ShowUI () {
//		Debug.LogError("LevelUpComponent ");
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

//	public void CallbackView(params object[] args)
//	{
//		IUICallback call = viewComponent as IUICallback;
//		if(call != null) {
//			call.CallbackView(data);
//		}
//	}
}
