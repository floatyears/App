using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnitInfoWindow : UIComponentUnity {

	GameObject buttonChoose;
	GameObject buttonViewInfo;
	GameObject buttonExit;
	int originLayer;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
		ShowSelf(false);
	}
	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
		base.HideUI();
	}

	void InitUIElement(){
		InitButton();
		originLayer = Main.Instance.NguiCamera.eventReceiverMask;
	}

	void ShowSelf(bool canShow){
		this.gameObject.SetActive(canShow);
		if( canShow ){
			SetScreenShelt("ScreenShelt");
		}
		else{
			SetScreenShelt("Default");
		}
	}

	void SetScreenShelt(string layerName){
		if(layerName == "ScreenShelt")
			Main.Instance.NguiCamera.eventReceiverMask = LayerMask.NameToLayer(layerName)<<15;
		else
			Main.Instance.NguiCamera.eventReceiverMask = originLayer;
                
        }

	void InitButton(){
		buttonChoose = transform.FindChild("btn_choose").gameObject;
		UIEventListener.Get(buttonChoose).onClick = Choose;
		buttonViewInfo = transform.FindChild("btn_see_info").gameObject;
		UIEventListener.Get(buttonViewInfo).onClick = ViewInfo;
		buttonExit = transform.FindChild("btn_exit").gameObject;
		UIEventListener.Get(buttonExit).onClick = Exit;
	}

	void Choose(GameObject btn){
	}

	void ViewInfo(GameObject btn){
	}

	void Exit(GameObject btn){
		Debug.Log("SelectUnitInfoWindow.Exit() : ");
		ShowSelf(false);
	}

	void ShowInfo(object info){
	}

	public override void Callback(object data){
		base.Callback(data);
		bool msg = (bool)data;
		ShowSelf(msg);
	}


}
