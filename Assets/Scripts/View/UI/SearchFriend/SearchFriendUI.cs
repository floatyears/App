using UnityEngine;
using System.Collections;

public class SearchFriendUI : ConcreteComponent,IUICallback {

	public SearchFriendUI( string uiName ) : base( uiName ) {}

	public override void CreatUI()
	{
		base.CreatUI();
	}

	public override void ShowUI()
	{
		base.ShowUI();
	}

	public void Callback(object data) {
		string strID = ( string )data;
		//Debug.Log( strID);
		TempNetwork.VerifySearchedID( strID );
	}
}
