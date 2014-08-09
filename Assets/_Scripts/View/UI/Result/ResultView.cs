using UnityEngine;
using System.Collections;

public class ResultView : UIComponentUnity {
	UILabel nameLabel;
	UILabel rankLabel;
	UILabel latestPlayLabel;
	UILabel idLabel;
	UILabel gotFriPointLabel;
	UILabel totalFriPointLabel;
	UIButton checkBtn;
	UIButton okBtn;
	UIButton cancelBtn;
	UISprite avatarTex;
	UISprite avatarBorderSpr;
	UISprite avatarBgSpr;
	GameObject rootTop;
	GameObject rootCenter;
	GameObject rootBottom;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
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

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs call = data as CallBackDispatcherArgs;

		switch (call.funcName){
			case "Stylize" :
				CallBackDispatcherHelper.DispatchCallBack(Stylize, call);
				break;
			case "ShowTopView" :
				CallBackDispatcherHelper.DispatchCallBack(ShowTopView, call);
				break;
			case "ShowCenterView" :
				CallBackDispatcherHelper.DispatchCallBack(ShowCenterView, call);
				break;
			default:
				break;
		}
	}

	void ClickCheck(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ConfigBattleUseData.Instance.BattleFriend.FriendPoint = 0;
//		UIManager.Instance.ChangeScene(SceneEnum.Home);

		DGTools.ChangeToQuest();
	}

	void ClickOk(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ConfigBattleUseData.Instance.BattleFriend.FriendPoint = 0;
//		Debug.LogError("Click ok");
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickOk", null);
		ExcuteCallback(call);
	}

	void ClickCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		UIManager.Instance.ChangeScene(SceneEnum.Home);

		DGTools.ChangeToQuest();
	}

	void Stylize(object msg){
		bool canStylize = (bool)msg;
		SetButtonActive(canStylize);
	}

	void ShowTopView(object msg){
		Debug.Log("ResultView.ShowTopView(), start...");
		TFriendInfo viewData = msg as TFriendInfo;
		TUnitInfo tui = viewData.UserUnit.UnitInfo;
		DataCenter.Instance.GetAvatarAtlas (tui.ID, avatarTex);
//		viewData.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar, o=>{
//			avatarTex.mainTexture = o as Texture2D;
//		});
		avatarBgSpr.spriteName = viewData.UserUnit.UnitInfo.GetUnitBackgroundName();
		avatarBorderSpr.spriteName = viewData.UserUnit.UnitInfo.GetUnitBorderSprName();

		nameLabel.text = (viewData.NickName == string.Empty) ? TextCenter.GetText("NO_NAME") : viewData.NickName;
		rankLabel.text = viewData.Rank.ToString();
		latestPlayLabel.text = TimeHelper.GetLatestPlayTime(viewData.LastPlayTime);
		idLabel.text = viewData.UserId.ToString();
	}

	void ShowCenterView(object msg){
		int friPoint = (int)msg;
		if(friPoint == 0){
			gotFriPointLabel.text = TextCenter.GetText("ZeroFriendPoint");
		}
		else{
			gotFriPointLabel.text = TextCenter.GetText("GotFriendPoint", friPoint);
		}
		totalFriPointLabel.text = TextCenter.GetText("TotalFriendPoint", DataCenter.Instance.AccountInfo.FriendPoint);
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
