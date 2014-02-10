using UnityEngine;
using System.Collections;

public class FriendListComponent : ConcreteComponent ,IUIFriendList{

	private bool isCustomMadeButton = true;

	public FriendListComponent( string uiName ) : base( uiName ) {}

	public override void CreatUI() {
		base.CreatUI();
	}
	public override void ShowUI() {
		base.ShowUI();
	}

	public override void HideUI() {
		base.HideUI();
	}

	public override void DestoryUI() {
		base.DestoryUI();
	}
	

	public void Callback(object data)
	{

	}

	public void CustomExtraFunction(object message)
	{
		bool isRequest = (bool)message;
		if(!isRequest)
			return;
		IUIFriendList callBack = viewComponent as IUIFriendList;
		if( callBack == null )
			return;
		//Debug.Log("FriendList Logic Answer");
		callBack.CustomExtraFunction("Update");
	}
}
