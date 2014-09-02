using UnityEngine;
using System.Collections;

public class GameCurrencyComponent : ModuleBase {

	public GameCurrencyComponent (UIConfigItem config) : base (  config) {
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void InitUI () {
		base.InitUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
//	public override void CallbackView (object data) {
//		base.CallbackView (data);
//		Dictionary<string, object> dicData = data as Dictionary<string, object>;
//		foreach (var item in dicData) {
//			DisposeCallback (item);
//		}
//		
//	}
}
