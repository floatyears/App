using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ApplyMessageView : ViewBase{
	GameObject rootPanel;

	UILabel titleLabel;
	UILabel noteLabel;
	UILabel nameLabel;
	UILabel rankLabel;
	UILabel timeLabel;
	UILabel idLabel;

	UIButton sureButton;
	UIButton cancelButton;

//	UITexture avatarTexture;
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);


		rootPanel = FindChild("Window");
		
		titleLabel = FindChild<UILabel>("Window/Label_Title");
		noteLabel = FindChild<UILabel>("Window/Label_Note");
		nameLabel = FindChild<UILabel>("Window/Label_Vaule/Name");
		rankLabel = FindChild<UILabel>("Window/Label_Vaule/Rank");
		timeLabel = FindChild<UILabel>("Window/Label_Vaule/LastLogin");
		idLabel = FindChild<UILabel>("Window/Label_Vaule/ID");
		//		avatarTexture = FindChild<UITexture>("Window/Avatar/Texture");
		sureButton = FindChild<UIButton>("Window/Button_Sure");
		cancelButton = FindChild<UIButton>("Window/Button_Cancel");
		
		UIEventListenerCustom.Get(sureButton.gameObject).onClick = ClickSure;
		UIEventListenerCustom.Get(cancelButton.gameObject).onClick = ClickCancel;  
		
		FindChild<UILabel> ("Window/Label_Text/Name").text = TextCenter.GetText ("Text_Name_Colon");
		FindChild<UILabel> ("Window/Label_Text/Rank").text = TextCenter.GetText ("Text_Rank_Colon");
		FindChild<UILabel> ("Window/Label_Text/LastLogin").text = TextCenter.GetText ("Text_LastLogin_Colon");
		FindChild<UILabel> ("Window/Button_Sure/Label").text = TextCenter.GetText ("OK");
		FindChild<UILabel> ("Window/Button_Cancel/Label").text = TextCenter.GetText ("Cancel");
		FindChild<UILabel> ("Window/Label_Note").text = TextCenter.GetText ("ApplyFriendNote");
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

		if(viewData != null){
			if(viewData.ContainsKey("data")){
				ShowCenterContent(viewData["data"]);
			}
			if(viewData.ContainsKey("title")){
				ShowTitle(viewData["title"] as string);
			}
			if(viewData.ContainsKey("content")){
				ShowNote(viewData["content"] as string);
			} 
		}
	}

	public override void CallbackView(params object[] args){
		switch (args[0].ToString()){
			case "StylizeTitle": 
				ShowTitle(args[1]);
				break;
			case "StylizeNote":
				ShowNote(args[1]);
				break;
			default:
				break;
		}
	}

	void ClickSure(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		ModuleManager.SendMessage (ModuleEnum.ApplyMessageModule, "ClickSure");
	}

	
	void ClickCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		ModuleManager.SendMessage (ModuleEnum.ApplyMessageModule, "ClickCancel");
	}

	protected override void ToggleAnimation (bool isShow)
	{
		base.ToggleAnimation (isShow);
		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			rootPanel.transform.localScale = new Vector3(1f, 0f, 1f);
			iTween.ScaleTo(rootPanel, iTween.Hash("y", 1, "time", 0.4f, "easetype", iTween.EaseType.easeOutBounce));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			gameObject.SetActive(false);
		}
	}

	void ShowTitle(object args){
		titleLabel.text = args as string;
	}

	void ShowNote(object args){
		noteLabel.text = args as string;
	}

	void ShowCenterContent(object args){
		FriendInfo tfi = args as FriendInfo;

		if(tfi.nickName == string.Empty)
			nameLabel.text = "NoName";
		else
			nameLabel.text = tfi.nickName;

		rankLabel.text = tfi.rank.ToString();
		timeLabel.text = Utility.TimeHelper.GetLatestPlayTime(tfi.lastPlayTime);
		idLabel.text = tfi.userId.ToString();

		FriendUnitItem fuv = FriendUnitItem.Inject(FindChild("Window/Avatar"));
		fuv.Init(tfi);

		UIEventListenerCustom.Get (FindChild("Window/Avatar")).LongPress = null;
	}

}