using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectInfoWindow : UIComponentUnity {
	GameObject window;
	GameObject buttonChoose;
	GameObject buttonViewInfo;
	GameObject buttonExit;

	UILabel lvLabel;
	UILabel slvLabel;
	UILabel atkLabel;
	UILabel hpLabel;
	UILabel nameLabel;
	UILabel raceLabel;


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
		window = FindChild("Window");

		lvLabel = FindChild<UILabel>("Window/Label_Vaule/Lv");
		slvLabel = FindChild<UILabel>("Window/Label_Vaule/SLv");
		atkLabel = FindChild<UILabel>("Window/Label_Vaule/ATK");
		hpLabel = FindChild<UILabel>("Window/Label_Vaule/Hp");
		nameLabel = FindChild<UILabel>("Window/Label_Vaule/Name");
		raceLabel = FindChild<UILabel>("Window/Label_Vaule/Race");

		buttonChoose = transform.FindChild("Window/btn_choose").gameObject;
		UIEventListener.Get(buttonChoose).onClick = Choose;
		buttonViewInfo = transform.FindChild("Window/btn_see_info").gameObject;
		UIEventListener.Get(buttonViewInfo).onClick = ViewInfo;
		buttonExit = transform.FindChild("Window/btn_exit").gameObject;
		UIEventListener.Get(buttonExit).onClick = Exit;
		originLayer = Main.Instance.NguiCamera.eventReceiverMask;
	}

	void ShowSelf(bool canShow){
		this.gameObject.SetActive(canShow);
		if( canShow ){

			SetScreenShelt("ScreenShelt");
			window.transform.localScale = new Vector3(1f,0f,1f);
			iTween.ScaleTo(window,iTween.Hash("y",1,"time",0.4f,"easetype",iTween.EaseType.easeOutBounce));
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

	void Choose(GameObject btn){
	}

	void ViewInfo(GameObject btn){

	}

	void Exit(GameObject btn){
		Debug.Log("SelectUnitInfoWindow.Exit() : ");
		ShowSelf(false);
	}

	public override void Callback(object data){
		base.Callback(data);
		TUserUnit tuu = data as TUserUnit;
		hpLabel.text = tuu.Level.ToString();
		atkLabel.text = tuu.Attack.ToString();
//		nameLabel.text = 


	}
	
}
