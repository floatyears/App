using UnityEngine;
using System.Collections;

public class SearchInfoWindow : UIComponentUnity,IUICallback {

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		InitWindow();
	}

	public override void ShowUI(){
		base.ShowUI();
	}

	void InitWindow(){
		SetActive( this.gameObject, false );
	}

	void SetActive( GameObject target, bool isActive ){
		target.SetActive( isActive );
	}

	public void Callback(object data) {
		Debug.Log("Show Info Window");
		bool isActive = (bool)data;
		SetActive( this.gameObject, isActive);
	}

}
