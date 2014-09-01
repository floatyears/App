using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserBriefInfoView : ViewBase{
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
	
	UISprite avatarTex;
	
	int originLayer = 1;
	public override void Init(UIConfigItem config){
		base.Init(config);
		InitUIElement();
//		ShowSelf(false);
	}
	public override void ShowUI(){
		base.ShowUI();
	}
	
	public override void HideUI(){
		base.HideUI();
//		ShowSelf(false);
	}
	
	void InitUIElement(){
		window = FindChild("Window");
		
		avatarTex = FindChild<UISprite>("Window/Avatar/Texture");

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
		buttonDelete = transform.FindChild("Window/Button_Delete").gameObject;
		originLayer = Main.Instance.NguiCamera.eventReceiverMask;

		FindChild<UILabel>("Window/Label_Text/Hp").text = TextCenter.GetText("Text_HP_Colon");
		FindChild<UILabel>("Window/Label_Text/ATK").text = TextCenter.GetText("Text_ATK_Colon");
		FindChild<UILabel>("Window/Label_Text/Race").text = TextCenter.GetText("Text_RACE_Colon");
		FindChild<UILabel>("Window/Label_Text/Name").text = TextCenter.GetText("Text_Name_Colon");
//		FindChild<UILabel>("Window/Label_Text/Rank").text = TextCenter.GetText("Text_Rank");
		FindChild<UILabel>("Window/Label_Text/Lv").text = TextCenter.GetText("Text_Level_Colon");
		FindChild<UILabel>("Window/Label_Text/LastLogin").text = TextCenter.GetText("Text_LastLogin_Colon");
		FindChild<UILabel>("Window/btn_choose/Label").text = TextCenter.GetText("Text_Select");
		FindChild<UILabel>("Window/btn_see_info/Label").text = TextCenter.GetText("Text_SeeInfo");
		FindChild<UILabel>("Window/btn_exit/Label").text = TextCenter.GetText("Text_Exit");
		FindChild<UILabel>("Window/Button_Delete/Label").text = TextCenter.GetText("DeleteNoteTitle");
	}
	
	void ShowSelf(bool canShow){
//		this.gameObject.SetActive(canShow);
		if (canShow){
			ShowUI();
//			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, true));
			window.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(window, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		} 
		else{
			HideUI();
//			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, false));            
		}
	}
		
	void Choose(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("Choose", null);
//		ExcuteCallback(cbdArgs);
		ShowSelf(false);

	}
	
	void ViewInfo(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ViewDetailInfo", null);
//		ExcuteCallback(cbd);
		ShowSelf(false);
	}
	
	void Exit(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		Debug.Log("SelectUnitInfoWindow.Exit() : ");
		ShowSelf(false);
	}
	
	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "RefreshUnitInfoView": 
				ShowSelf(true);
				CallBackDispatcherHelper.DispatchCallBack(RefreshUnitInfoView, cbdArgs);
				break;
			case "RefreshLastLogin": 
				ShowSelf(true);
				CallBackDispatcherHelper.DispatchCallBack(RefreshLastLogin, cbdArgs);
				break;
			case "RefreshRank": 
				ShowSelf(true);
				CallBackDispatcherHelper.DispatchCallBack(RefreshRank, cbdArgs);
				break;
			case "RefreshUserName": 
				ShowSelf(true);
				CallBackDispatcherHelper.DispatchCallBack(RefreshUserName, cbdArgs);
				break;
			case "EnableDeleteFriend": 
				CallBackDispatcherHelper.DispatchCallBack(EnableDeleteFriend, cbdArgs);
				break;
			case "HidePanel":
				ShowSelf(false);
				break;
//			case "Stylize": 
//				CallBackDispatcherHelper.DispatchCallBack(Stylize, cbdArgs);
//				break;

			default:
				break;
		}
	}


	void Stylize(object args){
		Dictionary<string,string> stylizeArgs = args as Dictionary<string,string>;
		if(stylizeArgs.ContainsKey("ButtonTop")){
			buttonChoose.transform.FindChild("Label").GetComponent<UILabel>().text = stylizeArgs["Button_Choose"];
		}
	}

	void EnableDeleteFriend(object args){
//		Debug.LogError("Receive Enable Friend Delete From logic ....");
		buttonDelete.gameObject.SetActive(true);
		UIEventListener.Get(buttonDelete.gameObject).onClick = ClickDelete;
	}
	
	void ClickDelete(GameObject btn){
		//Debug.LogError("Receive delete click, call logic to respone.....");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickDelete", null);
//		ExcuteCallback(cbdArgs);
		ShowSelf(false);
	}

	void RefreshUnitInfoView(object args){
		TUserUnit tuu = args as TUserUnit;
		hpLabel.text = tuu.Level.ToString();
		atkLabel.text = tuu.Attack.ToString();
		lvLabel.text = tuu.Level.ToString();
		nameLabel.text = tuu.UnitInfo.Name;
		raceLabel.text = tuu.UnitInfo.UnitRace.ToString();
//		tuu.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//			avatarTex.mainTexture = o as Texture2D;
//		});

		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.ID, avatarTex);

		slvLabel.text = tuu.UnitInfo.ActiveSkill.ToString();
	}

	void RefreshLastLogin(object args){
		lastLoginLabel.text = args as string;
	}

	void RefreshRank(object args){
		rankLabel.text = TextCenter.GetText("Text_Rank") + ": " + args as string;
	}

	void RefreshUserName(object args){
		nickNameLabel.text = args as string;
	}



}
