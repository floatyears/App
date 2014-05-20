using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBar : UIComponentUnity{
	GameObject infoBox;
	GameObject leftCollider;
	GameObject rightCollider;
 
	//Out 
	UILabel TLineLabel;

	UILabel VChipCountLabel;
	UILabel VStamMaxLabel;
	UILabel VCionCountLabel;
	UILabel VUserNameLabel;
	UILabel VRankLabel;
	UILabel VStaminaNowLabel;
	
	//Hide
	UILabel TRankHideLabel;
	UILabel VRankHideLabel;
	UILabel TNeedExpHideLabel;
	UILabel TTotalExpHideLabel;
	UILabel VNeedExpHideLabel;
	UILabel VTotalExpHideLabel;
        
	UISprite expSprite;
	UISprite staminaSprite;
	UISprite evolveTypeSprite;

	Vector3 leftPosition;
	Vector3 rightPosition;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI() {
		UpdateData();
		AddCommandListener();
		base.ShowUI();
		LogHelper.LogError("PlayerInfoBar.ShowUI()...");
	}

	public override void HideUI() {
		RemoveCommandListener();
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail)
			return;
		base.HideUI();
                
	}

	private void InitUI(){
		FindObject();
		FindLabel();

		leftPosition = new Vector3(-UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0);
		rightPosition = new Vector3(UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0);

		UIEventListener.Get(leftCollider.gameObject).onPress = ShowInfoBox;
		UIEventListener.Get(rightCollider.gameObject).onPress = ShowInfoBox;

		infoBox.SetActive(false);
	}
	
	private void FindObject(){
		infoBox = FindChild("InfoBox");
		leftCollider = FindChild("Left_Collider");
		rightCollider = FindChild("Right_Collider");
	}
	private void FindLabel(){
		TRankHideLabel = FindChild< UILabel >("InfoBox/Label_Text_Rank");
		TNeedExpHideLabel = FindChild< UILabel >("InfoBox/Label_Text_NextExp");
		TTotalExpHideLabel = FindChild< UILabel>("InfoBox/Label_Text_TotalExp");
		VRankHideLabel = FindChild< UILabel>("InfoBox/Label_Vaule_Rank");
		VNeedExpHideLabel = FindChild< UILabel>("InfoBox/Label_Vaule_NextExp");
		VTotalExpHideLabel = FindChild< UILabel>("InfoBox/Label_Vaule_TotalExp");
		
		TLineLabel = FindChild< UILabel >("InfoBar/Label_Text_Line");
		VChipCountLabel = FindChild< UILabel >("InfoBar/Label_Vaule_ChipNum");
		VStamMaxLabel = FindChild< UILabel >("InfoBar/Label_Vaule_CurStamina");
		VCionCountLabel = FindChild< UILabel >("InfoBar/Label_Vaule_Icon");
		VUserNameLabel = FindChild< UILabel >("InfoBar/Label_Vaule_PlayerName");
		VRankLabel = FindChild< UILabel >("InfoBar/Label_Vaule_Rank");
		VStaminaNowLabel = FindChild< UILabel >("InfoBar/Label_Vaule_TotalStamina");

		expSprite = FindChild<UISprite>("InfoBar/Foreground_Exp");
		staminaSprite = FindChild< UISprite >("InfoBar/Foreground_Stamina");

		//evolveTypeSprite = FindChild<UISprite>("InfoBar/Sprite_EvolveType");
	}

	private void ShowInfoBox(GameObject go, bool isPressed){
		infoBox.SetActive(isPressed);
		
		if (go.name == "Left_Collider") {
			infoBox.transform.localPosition = rightPosition;
		} 
		else if (go.name == "Right_Collider") {
			infoBox.transform.localPosition = leftPosition;
		}
	}
	
	void UpdateData(){
		if (DataCenter.Instance.UserInfo == null){
			Debug.Log("PlayerInfoBar.UpdateData() , userInfo is null , return ");
			return;
		}
		//Rank
		VRankLabel.text = DataCenter.Instance.UserInfo.Rank.ToString();
		VRankHideLabel.text = DataCenter.Instance.UserInfo.Rank.ToString();
		//Name
		VUserNameLabel.text = DataCenter.Instance.UserInfo.NickName;
		//Exp
		int nextExp = DataCenter.Instance.UserInfo.NextExp;
		int totalExp = DataCenter.Instance.UserInfo.Exp;
		int curRankExp = totalExp - DataCenter.Instance.UserInfo.CurRankExp;

		VNeedExpHideLabel.text = nextExp.ToString();
		VTotalExpHideLabel.text = totalExp.ToString();
		//TODO Get current rank max exp 
		expSprite.fillAmount = CountFillCount(curRankExp, DataCenter.Instance.UserInfo.CurRankExpMax);
		//Cion
		if (DataCenter.Instance.AccountInfo != null){
			VCionCountLabel.text = DataCenter.Instance.AccountInfo.Money.ToString();
			VChipCountLabel.text = DataCenter.Instance.AccountInfo.Stone.ToString();
		}
		//Stamina
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
		int staminaMax = DataCenter.Instance.UserInfo.StaminaMax;
		VStamMaxLabel.text = staminaNow.ToString();
		VStaminaNowLabel.text = staminaMax.ToString();
		staminaSprite.fillAmount = CountFillCount(staminaNow, staminaMax);

		//Evo
		int evoType = (int)DataCenter.Instance.UserInfo.EvolveType;
	}
	

	void ReName(object data){
		if (data != null && DataCenter.Instance.UserInfo != null)
		{
			bbproto.RspRenameNick rspRenameNick = data as bbproto.RspRenameNick;
			Debug.Log("rename response newNickName : " + rspRenameNick.newNickName);

			if (rspRenameNick.header.code != (int)ErrorCode.SUCCESS) {
				Debug.LogError("Rsp code: "+rspRenameNick.header.code+", error:"+rspRenameNick.header.error);
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspRenameNick.header.code);	
				return;
			}

			bool renameSuccess = (rspRenameNick.header.code == 0);
			if (renameSuccess && rspRenameNick.newNickName != null)
			{
				DataCenter.Instance.UserInfo.NickName = rspRenameNick.newNickName;

				VUserNameLabel.text = DataCenter.Instance.UserInfo.NickName;
			} else
			{
				//TODO: show error msgbox.
			}
		}

		UIManager.Instance.ChangeScene(SceneEnum.World);
	}

	void AddCommandListener(){
		// leiliang---------------------------------------------------------------
		MsgCenter.Instance.AddListener(CommandEnum.ReqRenameNick, ChangeName);
        LogHelper.Log("AddCommandListener() for listener ChangeName");

        MsgCenter.Instance.AddListener(CommandEnum.SyncChips, SyncChips);

        MsgCenter.Instance.AddListener(CommandEnum.SyncStamina, SyncStamina);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPlayerCoin, SyncCoins);

		}
	
	void RemoveCommandListener(){

		// leiliang---------------------------------------------------------------
		MsgCenter.Instance.RemoveListener(CommandEnum.ReqRenameNick, ChangeName);
        LogHelper.Log("RemoveListener() for listener ChangeName");

        MsgCenter.Instance.RemoveListener(CommandEnum.SyncChips, SyncChips);

        MsgCenter.Instance.RemoveListener(CommandEnum.SyncStamina, SyncStamina);
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPlayerCoin, SyncCoins);

	}
	void SyncCoins(object args){
		VCionCountLabel.text = DataCenter.Instance.AccountInfo.Money.ToString();
	}

	float  CountFillCount(int cur, int max)
	{
		if (cur < 0 || max <= 0)
			return 0f;
		return (float)cur / (float)max;
	}

	// leiliang--------------------------------------------------------------------
	private INetBase changeName;
	public void ChangeName(object  name)
	{
//        LogHelper.Log("ChangeName(), start");
//		if (changeName == null)
//		{
		changeName = new RenameNick();
		changeName.OnRequest(name, ReName);
//			UIManager.Instance.ChangeScene(SceneEnum.Start);
//		}
	}

    void SyncChips(object args){
        VChipCountLabel.text = DataCenter.Instance.AccountInfo.Stone.ToString();
    }

    void SyncStamina(object args){
        int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
        int staminaMax = DataCenter.Instance.UserInfo.StaminaMax;
        VStamMaxLabel.text = staminaNow.ToString();
        VStaminaNowLabel.text = staminaMax.ToString();
        staminaSprite.fillAmount = CountFillCount(staminaNow, staminaMax);
    }
}
