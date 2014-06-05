using UnityEngine;
using System.Collections;
using HTMLEngine;
using HTMLEngine.NGUI;

public class GameRaiderView : UIComponentUnity {

	NGUIHTML html;

	public override void Init ( UIInsConfig config, IUICallback origin ){
		base.Init (config, origin);

		//HtEngine.RegisterLogger(new Unity3DLogger());
		// our device
		HtEngine.RegisterDevice(new NGUIDevice());
		// link hover color.
//		HtEngine.LinkHoverColor = HtColor.Parse("#FF4444");
		// link pressed factor.
		HtEngine.LinkPressedFactor = 0.5f;
		// link function name.
		HtEngine.DefaultFontSize = 24;
		HtEngine.LinkFunctionName = "onLinkClicked";

		html = FindChild("HTML/Content").GetComponent<NGUIHTML> ();

	}
	
	public override void ShowUI(){
		base.ShowUI ();

		Debug.Log (TextCenter.GetText("Raider_0"));
		html.html = TextCenter.GetText("Raider_0");
	}
	
	public override void HideUI(){

		base.HideUI ();

	}
	
	public override void DestoryUI(){
		Debug.Log ("raider destroy ui");
		base.DestoryUI ();
	}

}
