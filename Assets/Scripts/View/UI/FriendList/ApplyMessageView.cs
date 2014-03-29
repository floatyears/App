using UnityEngine;
using System.Collections;

public class ApplyMessageView : UIComponentUnity{
	GameObject rootPanel;

	UILabel titleLabel;
	UILabel noteLabel;
	UILabel nameLabel;
	UILabel rankLabel;
	UILabel timeLabel;
	UILabel idLabel;

	UIButton sureButton;
	UIButton cancelButton;

	UITexture avatarTexture;
	
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowSelf(false);
	}

	public override void HideUI(){
		base.HideUI();
		ShowSelf(false);  
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "StylizeTitle": 
				CallBackDispatcherHelper.DispatchCallBack(ShowTitle, cbdArgs);
				break;
			case "StylizeNote":
				CallBackDispatcherHelper.DispatchCallBack(ShowNote, cbdArgs);
				break;
			case "RefreshContent":
				CallBackDispatcherHelper.DispatchCallBack(ShowCenterContent, cbdArgs);
				ShowSelf(true);
				break;
			case "HidePanel":
				CallBackDispatcherHelper.DispatchCallBack(HidePanel, cbdArgs);
				break;
			default:
				break;
		}
	}

	void HidePanel(object args){
		ShowSelf(false);
	}

	void InitUIElement(){
		rootPanel = FindChild("Window");

		titleLabel = FindChild<UILabel>("Window/Label_Title");
		noteLabel = FindChild<UILabel>("Window/Label_Note");
		nameLabel = FindChild<UILabel>("Window/Label_Vaule/Name");
		rankLabel = FindChild<UILabel>("Window/Label_Vaule/Rank");
		timeLabel = FindChild<UILabel>("Window/Label_Vaule/LastLogin");
		idLabel = FindChild<UILabel>("Window/Label_Vaule/ID");
		avatarTexture = FindChild<UITexture>("Window/Avatar/Texture");
		sureButton = FindChild<UIButton>("Window/Button_Sure");
		cancelButton = FindChild<UIButton>("Window/Button_Cancel");
	
		UIEventListener.Get(sureButton.gameObject).onClick = ClickSure;
		UIEventListener.Get(cancelButton.gameObject).onClick = ClickCancel;                
	}


	void ClickSure(GameObject btn){
		Debug.LogError("ApplyMessageView.ClickSure(),  click...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSure", null);
		ExcuteCallback(cbdArgs);
	}

	
	void ClickCancel(GameObject btn){
//		Debug.LogError("ApplyMessageView.ClickCancel(),  click...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickCancel", null);
		ExcuteCallback(cbdArgs);
	}

	void ShowSelf(bool canShow){
		this.gameObject.SetActive(canShow);
		if (canShow){
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, true));
			rootPanel.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(rootPanel, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		}
		else{
			MsgCenter.Instance.Invoke(CommandEnum.SetBlocker, new BlockerMaskParams(BlockerReason.BriefInfoWindow, false));        
		}
	}

	void ShowTitle(object args){
		titleLabel.text = args as string;
	}

	void ShowNote(object args){
		noteLabel.text = args as string;
	}

	void ShowCenterContent(object args){
		TFriendInfo tfi = args as TFriendInfo;

		avatarTexture.mainTexture = tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		if(tfi.NickName == string.Empty)
			nameLabel.text = "NoName";
		else
			nameLabel.text = tfi.NickName;

		rankLabel.text = tfi.Rank.ToString();
		timeLabel.text = TimeHelper.GetLatestPlayTime(tfi.LastPlayTime);
		idLabel.text = tfi.UserId.ToString();
	}
        
}