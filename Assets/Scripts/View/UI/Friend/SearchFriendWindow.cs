using UnityEngine;
using System.Collections;

public class SearchFriendWindow : UIComponentUnity {
	UIButton buttonSearch;
	UILabel labelInput;


	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitWindow();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	void InitWindow(){
		buttonSearch = FindChild< UIButton >("Button_Search");
		UIEventListener.Get( buttonSearch.gameObject ).onClick = ClickButton;
		labelInput = FindChild< UILabel >("Input/Label");
		labelInput.text = string.Empty;
	}

	void ClickButton( GameObject button ){
//		Debug.Log( "Click Button,Call.......");
		IUICallback buttonBacker = origin as IUICallback;
		bool isBacker;
		isBacker = origin is IUICallback;
		if( !isBacker )	return;
		string strInput = labelInput.text;
		buttonBacker.Callback( strInput );
	}

	void SetActive( GameObject target, bool isActive ){
		target.SetActive( isActive );
	}


}
