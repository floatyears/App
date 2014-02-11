using UnityEngine;
using System.Collections;

public class SearchInfoUI : ConcreteComponent {

	public SearchInfoUI( string uiName ) : base( uiName ) {}

	public override void CreatUI(){
		base.CreatUI();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public void ShowSelf(){
		Debug.Log("Class SearchInfoUI");
		IUICallback caller = viewComponent as IUICallback;
		bool isCaller;
		isCaller = viewComponent is IUICallback;
		Debug.Log( viewComponent.ToString() );
		if( !isCaller )	return;
		caller.Callback( true );
	}
}
