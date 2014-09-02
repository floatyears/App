using UnityEngine;
using System.Collections;

public class SearchInfoModule : ModuleBase {

	public SearchInfoModule(UIConfigItem config) : base(   config ) {
		CreateUI<SearchInfoView> ();
	}

	public override void InitUI(){
		base.InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	public void ShowSelf(){
//		IUICallback caller = viewComponent as IUICallback;
//		bool isCaller;
//		isCaller = viewComponent is IUICallback;
//		//Debug.Log( viewComponent.ToString() );
//		if( !isCaller )	return;
//		caller.CallbackView( true );
	}
}
