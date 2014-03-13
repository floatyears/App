using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserBriefInfoView : UIComponentUnity {
	GameObject window;
	GameObject buttonChoose;
	GameObject buttonViewInfo;
	GameObject buttonExit;
	GameObject buttonDelete;


	UILabel lastLoginLabel;
	UILabel nickNameLabel;
	UILabel rankLabel;

	UILabel lvLabel;
	UILabel slvLabel;
	UILabel atkLabel;
	UILabel hpLabel;
	UILabel nameLabel;
	UILabel raceLabel;
	
	UITexture avatarTex;
	
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
		ShowSelf(false);
	}
	
	void InitUIElement(){
		window = FindChild("Window");
		
		avatarTex = FindChild<UITexture>("Window/Avatar/Texture");

		rankLabel = FindChild<UILabel>("Window/Label_Vaule/Rank");
		lastLoginLabel = FindChild<UILabel>("Window/Label_Vaule/LastLogin");
		nickNameLabel = FindChild<UILabel>("Window/Label_Vaule/NickName");

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
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Choose", null);
		ExcuteCallback( cbdArgs );
		ShowSelf(false);
	}
	
	void ViewInfo(GameObject btn){
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ViewDetailInfo", null);
		ExcuteCallback( cbd );
		ShowSelf(false);
	}
	
	void Exit(GameObject btn){
		Debug.Log("SelectUnitInfoWindow.Exit() : ");
		ShowSelf(false);
	}
	
	public override void Callback(object data){
		base.Callback(data);
		ShowSelf(true);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "RefreshUnitInfoView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshUnitInfoView, cbdArgs);
				break;
			case "RefreshLastLogin" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshLastLogin, cbdArgs);
				break;
			case "RefreshRank" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshRank, cbdArgs);
				break;
			case "RefreshUserName" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshUserName, cbdArgs);
				break;
			
			default:
				break;
		}
	}
	
	void RefreshUnitInfoView(object args){
		TUserUnit tuu = args as TUserUnit;
		hpLabel.text = tuu.Level.ToString();
		atkLabel.text = tuu.Attack.ToString();
		lvLabel.text = tuu.Level.ToString();
		nameLabel.text = tuu.UnitInfo.Name;
		raceLabel.text = tuu.UnitInfo.UnitRace.ToString();
		avatarTex.mainTexture = tuu.UnitInfo.GetAsset(UnitAssetType.Avatar);
	}

	void RefreshLastLogin(object args){
		lastLoginLabel.text = "Last Login Time : " + args as string;
	}

	void RefreshRank(object args){
		rankLabel.text = "Rank : " + args as string;
	}

	void RefreshUserName(object args){
		nickNameLabel.text = args as string;
	}



}
