using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ResultView : ViewBase {
	UILabel nameLabel;
	UILabel rankLabel;
	UILabel latestPlayLabel;
	UILabel idLabel;
	UILabel gotFriPointLabel;
	UILabel totalFriPointLabel;
	UILabel addFriendTipsLabel;
	UIButton checkBtn;
	UIButton okBtn;
	UIButton cancelBtn;
	UISprite avatarTex;
	UISprite avatarBorderSpr;
	UISprite avatarBgSpr;
	GameObject rootTop;
	GameObject rootCenter;
	GameObject rootBottom;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitUI();
	}

	void InitUI(){
		rootTop = transform.FindChild("Top").gameObject;
		rootCenter = transform.FindChild("Center").gameObject;
		rootBottom = transform.FindChild("Bottom").gameObject;

		nameLabel = rootTop.transform.FindChild("ValueLabels/Label_Name").GetComponent<UILabel>();
		rankLabel = rootTop.transform.FindChild("ValueLabels/Label_Rank").GetComponent<UILabel>();
		latestPlayLabel = rootTop.transform.FindChild("ValueLabels/Label_LatestPlay").GetComponent<UILabel>();
		idLabel = rootTop.transform.FindChild("ValueLabels/Label_ID").GetComponent<UILabel>();

		gotFriPointLabel = rootCenter.transform.FindChild("Label_Top").GetComponent<UILabel>();
		totalFriPointLabel = rootCenter.transform.FindChild("Label_Bottom").GetComponent<UILabel>();
		addFriendTipsLabel = rootCenter.transform.FindChild ("Label_Add").GetComponent<UILabel> ();

		avatarTex = rootTop.transform.FindChild("Avatar/Texture").GetComponent<UISprite>();
		avatarBgSpr = rootTop.transform.FindChild("Avatar/Background").GetComponent<UISprite>();
		avatarBorderSpr = rootTop.transform.FindChild("Avatar/Sprite_Avatar_Border").GetComponent<UISprite>();

		checkBtn = rootBottom.transform.FindChild("Button_Check").GetComponent<UIButton>();
		okBtn = rootBottom.transform.FindChild("Button_OK").GetComponent<UIButton>();
		cancelBtn = rootBottom.transform.FindChild("Button_Cancel").GetComponent<UIButton>();

		okBtn.transform.FindChild("Label").GetComponent<UILabel>().text = TextCenter.GetText("OK");
		cancelBtn.transform.FindChild("Label").GetComponent<UILabel>().text = TextCenter.GetText("Cancel");
		rootTop.transform.FindChild ("TextLabels/Label_Rank").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Rank_Colon");
//		rootTop.transform.FindChild ("TextLabels/Label_ID").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_ID");
		rootTop.transform.FindChild ("TextLabels/Label_Name").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Name_Colon");
		rootTop.transform.FindChild ("TextLabels/Label_LatestPlay").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_LastLogin_Colon");
		rootTop.transform.FindChild ("Label_Title").GetComponent<UILabel> ().text = TextCenter.GetText ("Support_Friend");

		UIEventListener.Get(checkBtn.gameObject).onClick = ClickCheck;
		UIEventListener.Get(okBtn.gameObject).onClick = ClickOk;
		UIEventListener.Get(cancelBtn.gameObject).onClick = ClickCancel;
	}
	public override void ShowUI(){
		MsgCenter.Instance.Invoke (CommandEnum.EnableMenuBtns, false);

		base.ShowUI();
		ShowUIAnimation();
	}

	public override void HideUI(){
		MsgCenter.Instance.Invoke (CommandEnum.EnableMenuBtns, true);

		base.HideUI();
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (args[0].ToString()){
			case "Stylize" :
				Stylize(args[1]);
				break;
			case "ShowTopView" :
				ShowTopView(args[1]);
				break;
			case "ShowCenterView" :
				ShowCenterView(args[1]);
				break;
			default:
				break;
		}
	}

	void ClickCheck(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		BattleConfigData.Instance.BattleFriend.friendPoint = 0;
//		ModuleManger.Instance.ShowModule(ModuleEnum.Home);

		DGTools.ChangeToQuest();
	}

	void ClickOk(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		BattleConfigData.Instance.BattleFriend.friendPoint = 0;
//		Debug.LogError("Click ok");
//		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickOk", null);
//		ExcuteCallback(call);
		ModuleManager.SendMessage (ModuleEnum.ResultModule, "ClickOk");
	}

	void ClickCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		ModuleManger.Instance.ShowModule(ModuleEnum.Home);

		DGTools.ChangeToQuest();
	}

	bool canStylize = false;
	void Stylize(object msg){
		canStylize = (bool)msg;
		SetButtonActive(canStylize);
	}

	void ShowTopView(object msg){
		Debug.Log("ResultView.ShowTopView(), start...");
		FriendInfo viewData = msg as FriendInfo;
		UnitInfo tui = viewData.UserUnit.UnitInfo;
		ResourceManager.Instance.GetAvatarAtlas (tui.id, avatarTex);
//		viewData.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//			avatarTex.mainTexture = o as Texture2D;
//		});
		avatarBgSpr.spriteName = viewData.UserUnit.UnitInfo.GetUnitBackgroundName();
		avatarBorderSpr.spriteName = viewData.UserUnit.UnitInfo.GetUnitBorderSprName();

		nameLabel.text = (viewData.nickName == string.Empty) ? TextCenter.GetText("NO_NAME") : viewData.nickName;
		rankLabel.text = viewData.rank.ToString();
		latestPlayLabel.text = Utility.TimeHelper.GetLatestPlayTime(viewData.lastPlayTime);
		idLabel.text = viewData.userId.ToString();
	}

	void ShowCenterView(object msg){
		int friPoint = (int)msg;
		if(friPoint == 0){
			gotFriPointLabel.text = TextCenter.GetText("ZeroFriendPoint");
		}
		else{
			gotFriPointLabel.text = TextCenter.GetText("GotFriendPoint", friPoint);
		}
		totalFriPointLabel.text = TextCenter.GetText("TotalFriendPoint", DataCenter.Instance.UserData.AccountInfo.friendPoint);

		if (canStylize) {
			addFriendTipsLabel.text = TextCenter.GetText ("AddFriendTips");	
		} else {
			addFriendTipsLabel.text = "";
		}
	}

	void SetButtonActive(bool active){
		okBtn.gameObject.SetActive(active);
		cancelBtn.gameObject.SetActive(active);
		checkBtn.gameObject.SetActive(!active);
	}

	private void ShowUIAnimation(){
		transform.localPosition = 1000 * Vector3.left;
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}
}
