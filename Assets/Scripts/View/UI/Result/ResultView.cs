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
	UITexture avatarTex;
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

		nameLabel = rootTop.transform.FindChild("Label_Name").GetComponent<UILabel>();
		rankLabel = rootTop.transform.FindChild("Label_Rank").GetComponent<UILabel>();
		latestPlayLabel = rootTop.transform.FindChild("Label_LatestPlay").GetComponent<UILabel>();
		idLabel = rootTop.transform.FindChild("Label_ID").GetComponent<UILabel>();

		gotFriPointLabel = rootCenter.transform.FindChild("Label_Top").GetComponent<UILabel>();
		totalFriPointLabel = rootCenter.transform.FindChild("Label_Bottom").GetComponent<UILabel>();

		avatarTex = rootTop.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();

		checkBtn = transform.FindChild("Button_Check").GetComponent<UIButton>();
		okBtn = transform.FindChild("Button_Ok").GetComponent<UIButton>();
		cancelBtn = transform.FindChild("Button_Cancel").GetComponent<UIButton>();
		UIEventListener.Get(checkBtn.gameObject).onClick = ClickCheck;
		UIEventListener.Get(okBtn.gameObject).onClick = ClickCheck;
		UIEventListener.Get(cancelBtn.gameObject).onClick = ClickCheck;
	}
	public override void ShowUI(){
		base.ShowUI();
	}

	public override void HideUI(){
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
		UIManager.Instance.ChangeScene(SceneEnum.Quest);
	}

	void ClickOk(GameObject btn){
		CallBackDispatcherArgs call = new CallBackDispatcherArgs("ClickOk", null);
		ExcuteCallback(call);
	}

	void ClickCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UIManager.Instance.ChangeScene(SceneEnum.Quest);
	}

	void Stylize(object msg){
		bool canStylize = (bool)msg;
		SetButtonActive(canStylize);
	}

	void ShowTopView(object msg){
		Debug.Log("ResultView.ShowTopView(), start...");
		TFriendInfo viewData = msg as TFriendInfo;

		avatarTex.mainTexture = viewData.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		nameLabel.text = viewData.NickName;
		rankLabel.text = viewData.Rank.ToString();
		latestPlayLabel.text = TimeHelper.GetLatestPlayTime(viewData.LastPlayTime);
		idLabel.text = viewData.UserId.ToString();
	}

	void ShowCenterView(object msg){
		int friPoint = (int)msg;
		if(friPoint == 0){
			gotFriPointLabel.text = TextCenter.Instace.GetCurrentText("ZeroFriendPoint");
		}
		else{
			gotFriPointLabel.text = TextCenter.Instace.GetCurrentText("GotFriendPoint", friPoint);
		}

		totalFriPointLabel.text = DataCenter.Instance.AccountInfo.FriendPoint.ToString();
	}

	void SetButtonActive(bool active){
		okBtn.gameObject.SetActive(active);
		cancelBtn.gameObject.SetActive(active);
		checkBtn.gameObject.SetActive(!active);
	}
}
