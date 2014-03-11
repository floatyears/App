using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyComponent : ConcreteComponent ,IUIFriendList{

	private bool isCustomMadeButton = true;

	public ApplyComponent( string uiName ) : base( uiName ) {}
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


	public void CustomExtraFunction(object message)
	{
		bool isRequest = (bool)message;
		if(!isRequest)
			return;
		IUIFriendList callBack = viewComponent as IUIFriendList;
		if( callBack == null )
			return;
		//Debug.Log("Apply Logic Answer");
		callBack.CustomExtraFunction(string.Empty);
	}

	

	public void Callback(object data)
	{

	}



}
