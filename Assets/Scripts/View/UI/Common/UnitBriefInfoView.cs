using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBriefInfoView : UIComponentUnity {
	public const string CancelCommand = "Cancel";
	public const string EnsureCommand = "Select";

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

	UITexture avatarTex;

//	int originLayer = 1;
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
//		originLayer = Main.Instance.NguiCamera.eventReceiverMask;
	}

	void ShowSelf(bool canShow){
		this.gameObject.SetActive(canShow);
		if( canShow ){
//			SetScreenShelt("ScreenShelt");
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, true));
			window.transform.localScale = new Vector3(1f,0f,1f);
			iTween.ScaleTo(window,iTween.Hash("y",1,"time",0.4f,"easetype",iTween.EaseType.easeOutBounce));
		}
		else{
//			SetScreenShelt("Default");
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, false));
		}
	}
	
//	void SetScreenShelt(string layerName){
//		if(layerName == "ScreenShelt")
//			Main.Instance.NguiCamera.eventReceiverMask = LayerMask.NameToLayer(layerName)<<15;
//		else
//			Main.Instance.NguiCamera.eventReceiverMask = originLayer;
//        }

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
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs(CancelCommand, null);
		ShowSelf(false);
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		ShowSelf(true);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "RefreshPanelView" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshPanelView, cbdArgs);
				break;
			default:
				break;
		}
	}
	
	void RefreshPanelView(object args){
		TUserUnit tuu = args as TUserUnit;
		hpLabel.text = tuu.Level.ToString();
		atkLabel.text = tuu.Attack.ToString();
		lvLabel.text = tuu.Level.ToString();
		nameLabel.text = tuu.UnitInfo.Name;
		raceLabel.text = tuu.UnitInfo.UnitRace.ToString();
		tuu.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
			avatarTex.mainTexture = o as Texture2D;
		});
	}

	void RefreshLastLabel(object args){

	}

	void RefreshTitleLabel(object args){

	}
}
