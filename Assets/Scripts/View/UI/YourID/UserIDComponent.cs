using UnityEngine;
using System.Collections;

public class UserIDComponent : ConcreteComponent,IUICallback {

	string userID = "110.119.120";
	public UserIDComponent( string uiName ) : base( uiName ) {}
	
	public override void CreatUI() {
		base.CreatUI();
	}
	public override void ShowUI()
	{
		base.ShowUI();
	}
	
	public override void HideUI()
	{
		base.HideUI();
	}
	
	public override void DestoryUI()
	{
		base.DestoryUI();
	}

	public void Callback(object data)
	{
		IUICallback id = viewComponent as IUICallback;
		if( id == null )	return;
		//Debug.Log("Comp call back");
		id.Callback( userID );
	}
}