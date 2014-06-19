using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBar : UIComponentUnity{
	GameObject infoBox;
	GameObject leftCollider;
	GameObject rightCollider;
 
	//outside panel 
	UILabel dividedLineLabel;
	UILabel chipNumValueLabel;
	UILabel stamMaxValueLabel;
	UILabel cionNumValueLabel;
	UILabel userNameValueLabel;
	UILabel outRankValueLabel;
	UILabel stamNowValueLabel;
	
	//floating panel when pressed
	UILabel floatIDTextLabel;
	UILabel floatIDValueLabel;
	UILabel floatNextExpTextLabel;
	UILabel floatTotalExoTextLabel;
	UILabel floatNextExpValueLabel;
	UILabel floatTotalExpValueLabel;
        
	UISprite expSpr;
	UISprite stamSpr;
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
		base.HideUI();          
	}

	private void InitUI(){
		infoBox = FindChild("InfoBox");
		leftCollider = FindChild("Left_Collider");
		rightCollider = FindChild("Right_Collider");

		InitTextLabelFloating();
		InitValueLabelFloating();
		InitValueLabelOutside();
		
		dividedLineLabel = transform.FindChild("InfoBar/Label_Text_Line").GetComponent<UILabel>();
		dividedLineLabel.text = TextCenter.GetText("Divided_Line");
		
		expSpr = transform.FindChild("InfoBar/Slider_Exp_Fg").GetComponent<UISprite>();
		stamSpr = transform.FindChild("InfoBar/Slider_Stamina_Fg").GetComponent<UISprite>();

		leftPosition = new Vector3(-UIConfig.playerInfoBox_X, 0, 0);
		rightPosition = new Vector3(UIConfig.playerInfoBox_X, 0, 0);

		UIEventListener.Get(leftCollider.gameObject).onPress = ShowInfoBox;
		UIEventListener.Get(rightCollider.gameObject).onPress = ShowInfoBox;

		infoBox.SetActive(false);
	}

	/// <summary>
	/// Inits the text label in floating info_panel.
	/// </summary>
	private void InitTextLabelFloating(){
		floatIDTextLabel = transform.FindChild("InfoBox/Label_Text_ID").GetComponent<UILabel>();
		floatIDTextLabel.text = TextCenter.GetText("Float_User_ID");
		
		floatNextExpTextLabel = transform.FindChild("InfoBox/Label_Text_NextExp").GetComponent<UILabel>();
		floatNextExpTextLabel.text = TextCenter.GetText("Float_NextExp");
		
		floatTotalExoTextLabel = transform.FindChild("InfoBox/Label_Text_TotalExp").GetComponent<UILabel>();
		floatTotalExoTextLabel.text = TextCenter.GetText("Float_TotalExp");
	}

	/// <summary>
	/// Inits the value label in floating info_panel.
	/// </summary>
	private void InitValueLabelFloating(){
		floatIDValueLabel = transform.FindChild("InfoBox/Label_Vaule_ID").GetComponent<UILabel>();
		floatNextExpValueLabel = transform.FindChild("InfoBox/Label_Vaule_NextExp").GetComponent<UILabel>();
		floatTotalExpValueLabel = transform.FindChild("InfoBox/Label_Vaule_TotalExp").GetComponent<UILabel>();
	}

	/// <summary>
	/// Inits the value label outside.
	/// </summary>
	private void InitValueLabelOutside(){
		chipNumValueLabel = transform.FindChild("InfoBar/Label_Vaule_ChipNum").GetComponent<UILabel>();
		stamMaxValueLabel = transform.FindChild("InfoBar/Label_Vaule_CurStamina").GetComponent<UILabel>();
		stamNowValueLabel = transform.FindChild("InfoBar/Label_Vaule_TotalStamina").GetComponent<UILabel>();
		cionNumValueLabel = transform.FindChild("InfoBar/Label_Vaule_Icon").GetComponent<UILabel>();
		userNameValueLabel = transform.FindChild("InfoBar/Label_Vaule_PlayerName").GetComponent<UILabel>();
		outRankValueLabel = transform.FindChild("InfoBar/Label_Vaule_Rank").GetComponent<UILabel>();
	}
	
	private void FindLabel(){

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
		outRankValueLabel.text = DataCenter.Instance.UserInfo.Rank.ToString();
		floatIDValueLabel.text = DataCenter.Instance.UserInfo.UserId.ToString();
		//Name
		userNameValueLabel.text = DataCenter.Instance.UserInfo.NickName;
		//Exp
		int nextExp = DataCenter.Instance.UserInfo.NextExp;
		int totalExp = DataCenter.Instance.UserInfo.Exp;
		int curRankExp = totalExp - DataCenter.Instance.UserInfo.CurRankExp;

		floatNextExpValueLabel.text = nextExp.ToString();
		floatTotalExpValueLabel.text = totalExp.ToString();
		//TODO Get current rank max exp 
		expSpr.fillAmount = CountFillCount(curRankExp, DataCenter.Instance.UserInfo.CurRankExpMax);
		//Cion
		if (DataCenter.Instance.AccountInfo != null){
			cionNumValueLabel.text = DataCenter.Instance.AccountInfo.Money.ToString();
			chipNumValueLabel.text = DataCenter.Instance.AccountInfo.Stone.ToString();
		}
		//Stamina
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
		int staminaMax = DataCenter.Instance.UserInfo.StaminaMax;
		stamMaxValueLabel.text = staminaNow.ToString();
		stamNowValueLabel.text = staminaMax.ToString();
		stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);

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

				userNameValueLabel.text = DataCenter.Instance.UserInfo.NickName;
			} else
			{
				//TODO: show error msgbox.
			}
		}

		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
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
		cionNumValueLabel.text = DataCenter.Instance.AccountInfo.Money.ToString();
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
        chipNumValueLabel.text = DataCenter.Instance.AccountInfo.Stone.ToString();
    }

    void SyncStamina(object args){
        int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
        int staminaMax = DataCenter.Instance.UserInfo.StaminaMax;
        stamMaxValueLabel.text = staminaNow.ToString();
        stamNowValueLabel.text = staminaMax.ToString();
        stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);
    }
}
