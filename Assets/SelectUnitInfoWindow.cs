using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitInfoBox : UIComponentUnity {

	GameObject buttonChoose;
	GameObject buttonViewInfo;
	GameObject buttonExit;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	void InitUIElement(){
		InitButton();
	}

	void InitButton(){
		buttonChoose = transform.FindChild("btn_choose").gameObject;
		UIEventListener.Get(buttonChoose).onClick = Choose;
		buttonViewInfo = transform.FindChild("btn_see_info").gameObject;
		UIEventListener.Get(buttonViewInfo).onClick = ViewInfo;
		buttonExit = transform.FindChild("btn_see_info").gameObject;
		UIEventListener.Get(buttonExit).onClick = Exit;
	}

	void Choose(GameObject btn){
	}

	void ViewInfo(GameObject btn){
	}

	void Exit(GameObject btn){
	}

	void ShowInfo(object info){
	}


}
