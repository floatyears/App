using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HTMLEngine;
using HTMLEngine.NGUI;

public class GameRaiderView : ViewBase {

	NGUIHTML html;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null){
		base.Init (config,data);

		//HtEngine.RegisterLogger(new Unity3DLogger());
		// our device
		HtEngine.RegisterDevice(new NGUIDevice());
		// link hover color.
//		HtEngine.LinkHoverColor = HtColor.Parse("#FF4444");
		// link pressed factor.
		HtEngine.LinkPressedFactor = 0.5f;
		// link function name.
		HtEngine.DefaultFontSize = 24;
		HtEngine.DefaultFontFace = "Dimbo Regular";
		HtEngine.LinkFunctionName = "onLinkClicked";

		html = FindChild("HTML/Content").GetComponent<NGUIHTML> ();

	}
	
	public override void ShowUI(){
		base.ShowUI ();

		Debug.Log (TextCenter.GetText("Raider_0"));
		html.html = "<p align=center>游戏帮助</p>";
		StartCoroutine (ShowContent());
//						
//		);


	}

	private IEnumerator ShowContent(){
		yield return 0;
		html.html = TextCenter.GetText ("Raider_0");

//		ShowUIAnimation ();
	}
	
	public override void HideUI(){

		base.HideUI ();
//		iTween.Stop (gameObject);

	}
	
	public override void DestoryUI(){
		Debug.Log ("raider destroy ui");
		base.DestoryUI ();
	}
	


}
