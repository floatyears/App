using UnityEngine;
using System.Collections;

public class GameCurrencyComponent : ConcreteComponent {

	public GameCurrencyComponent (string name) : base (name) {
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void CreatUI () {
		base.CreatUI ();
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
