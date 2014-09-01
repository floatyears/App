using UnityEngine;
using System.Collections;

public class SearchInfoWindow : ViewBase {

	public override void Init(UIConfigItem config){
		base.Init(config);
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

	public void CallbackView(object data) {
		//Debug.Log("Show Info Window");
		bool isActive = (bool)data;
		SetActive( this.gameObject, isActive);
	}

}
