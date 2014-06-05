using UnityEngine;
using System.Collections;

public class GameRaiderContent : MonoBehaviour {

	NGUIHTML html;

	void Start(){
		html = GetComponent<NGUIHTML> ();
	}

	void onLinkClicked(GameObject clickedObj){
		Debug.Log ("clicked: ");
		var nguiLinkText = clickedObj.GetComponent<NGUILinkText>();
		if (nguiLinkText != null) {
			Debug.Log(nguiLinkText.linkText);
			html.html = TextCenter.GetText(nguiLinkText.linkText);
		}
	}
}
