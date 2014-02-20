using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitInfoBox : MonoBehaviour {

	GameObject buttonChoose;
	GameObject buttonViewInfo;
	GameObject buttonExit;
	void OnEnable(){
		Init();
	}

	void Init(){
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
		//ShowInfo();
	}

	void Exit(GameObject btn){
		GameObject.Destroy(this);
	}

	void ShowInfo(object info){
		//Dictionary<string, object> infoDic = info as Dictionary<string, object>;
	}


}
