using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBarView : ViewBase{
	GameObject infoBox;
	GameObject leftCollider;
	GameObject rightCollider;
 
	//outside panel 
	UILabel dividedLineLabel;
	UILabel chipNumValueLabel;
//	UILabel stamMaxValueLabel;
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
	UILabel floatRankValueLabel;

	UILabel countDown;
	private uint currentTime;
        
	UISprite expSpr;
	UISprite stamSpr;
	UISprite evolveTypeSprite;

	Vector3 leftPosition;
	Vector3 rightPosition;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init(config, data);
		InitUI();

		uint.TryParse(GameTimer.GetInstance().recovertime + "",out currentTime);
		InvokeRepeating ("AddStamina", 0, 1.0f);


	}

	public override void ShowUI() {
		UpdateData();
		AddCommandListener();
		base.ShowUI();
		LogHelper.LogError("PlayerInfoBar.ShowUI()...");
		MsgCenter.Instance.AddListener (CommandEnum.UpdatePlayerInfo, OnUpdateInfo);
	}

	public override void HideUI() {
		RemoveCommandListener();
		MsgCenter.Instance.AddListener (CommandEnum.UpdatePlayerInfo, OnUpdateInfo);
		base.HideUI();          
	}

//	public override void DestoryUI ()
//	{
//		base.DestoryUI ();
//
//	}

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

		UIEventListenerCustom.Get(leftCollider.gameObject).onPress = ShowInfoBox;
		UIEventListenerCustom.Get(rightCollider.gameObject).onPress = ShowInfoBox;

		infoBox.SetActive(false);

//		MsgCenter.Instance.AddListener (CommandEnum.UpdatePartyInfoPanel,UpdateData);
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

		transform.FindChild ("InfoBox/Label_Text_Rank").GetComponent<UILabel> ().text = TextCenter.GetText ("Float_Rank");
	}

	/// <summary>
	/// Inits the value label in floating info_panel.
	/// </summary>
	private void InitValueLabelFloating(){
		floatIDValueLabel = transform.FindChild("InfoBox/Label_Vaule_ID").GetComponent<UILabel>();
		floatNextExpValueLabel = transform.FindChild("InfoBox/Label_Vaule_NextExp").GetComponent<UILabel>();
		floatTotalExpValueLabel = transform.FindChild("InfoBox/Label_Vaule_TotalExp").GetComponent<UILabel>();
		floatRankValueLabel = transform.FindChild ("InfoBox/Label_Value_Rank").GetComponent<UILabel> ();

	}

	/// <summary>
	/// Inits the value label outside.
	/// </summary>
	private void InitValueLabelOutside(){
		chipNumValueLabel = transform.FindChild("InfoBar/Label_Vaule_Icon").GetComponent<UILabel>();
//		stamMaxValueLabel = transform.FindChild("InfoBar/Label_Vaule_CurStamina").GetComponent<UILabel>();
		stamNowValueLabel = transform.FindChild("InfoBar/Label_Vaule_TotalStamina").GetComponent<UILabel>();
		cionNumValueLabel = transform.FindChild("InfoBar/Label_Vaule_ChipNum").GetComponent<UILabel>();
		userNameValueLabel = transform.FindChild("InfoBar/Label_Vaule_PlayerName").GetComponent<UILabel>();
		outRankValueLabel = transform.FindChild("InfoBar/Label_Vaule_Rank").GetComponent<UILabel>();

		countDown = transform.FindChild ("InfoBar/Label_CountDown").GetComponent<UILabel> ();
	}
	
	private void FindLabel(){

	}

	private void ShowInfoBox(GameObject go, bool isPressed){
		infoBox.SetActive(isPressed);
		
		if (go.name == "Left_Collider") {
			infoBox.transform.localPosition = rightPosition;
		} else if (go.name == "Right_Collider") {
			infoBox.transform.localPosition = leftPosition;
		}
	}
	
	public void UpdateData(){
		if (DataCenter.Instance.UserData.UserInfo == null){
			Debug.Log("PlayerInfoBar.UpdateData() , userInfo is null , return ");
			return;
		}
		//Rank
		outRankValueLabel.text = DataCenter.Instance.UserData.UserInfo.rank.ToString();
		floatIDValueLabel.text = DataCenter.Instance.UserData.UserInfo.userId.ToString();
		//Name
		userNameValueLabel.text = DataCenter.Instance.UserData.UserInfo.nickName;
		//Exp
		int nextExp = DataCenter.Instance.UserData.UserInfo.NextExp;
		int totalExp = DataCenter.Instance.UserData.UserInfo.exp;
		int curRankExp = totalExp - DataCenter.Instance.UserData.UserInfo.CurRankExp;
//		UnityEngine.Debug.LogError("totalExp:"+totalExp+" - CurRankExp:" + DataCenter.Instance.UserData.UserInfo.CurRankExp + " = " +curRankExp+"/"+DataCenter.Instance.UserData.UserInfo.CurRankExpMax);

		floatNextExpValueLabel.text = nextExp.ToString();
		floatTotalExpValueLabel.text = totalExp.ToString();
		//TODO Get current rank max exp 
		expSpr.fillAmount = CountFillCount(curRankExp, DataCenter.Instance.UserData.UserInfo.CurRankExpMax);
		floatRankValueLabel.text = DataCenter.Instance.UserData.UserInfo.rank.ToString ();
		//Cion
		if (DataCenter.Instance.UserData.AccountInfo != null){
			cionNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.money.ToString();
			chipNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.stone.ToString();
		}
		//Stamina
		int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;
		int staminaMax = DataCenter.Instance.UserData.UserInfo.staminaMax;
//		stamMaxValueLabel.text = staminaNow.ToString();
		stamNowValueLabel.text = staminaNow.ToString() + "/" + staminaMax.ToString();
		stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);

		//Evo
		int evoType = (int)DataCenter.Instance.UserData.UserInfo.EvolveType;
	}
	


	void AddCommandListener(){
		// leiliang---------------------------------------------------------------
//		MsgCenter.Instance.AddListener(CommandEnum.ReqRenameNick, ChangeName);
        LogHelper.Log("AddCommandListener() for listener ChangeName");

        MsgCenter.Instance.AddListener(CommandEnum.SyncChips, SyncChips);

        MsgCenter.Instance.AddListener(CommandEnum.SyncStamina, SyncStamina);
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPlayerCoin, SyncCoins);

		}
	
	void RemoveCommandListener(){

		// leiliang---------------------------------------------------------------
//		MsgCenter.Instance.RemoveListener(CommandEnum.ReqRenameNick, ChangeName);
        LogHelper.Log("RemoveListener() for listener ChangeName");

        MsgCenter.Instance.RemoveListener(CommandEnum.SyncChips, SyncChips);

        MsgCenter.Instance.RemoveListener(CommandEnum.SyncStamina, SyncStamina);
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPlayerCoin, SyncCoins);

	}
	void SyncCoins(object args){
		cionNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.money.ToString();
	}

	float  CountFillCount(int cur, int max)
	{
		if (cur < 0 || max <= 0)
			return 0f;
		return (float)cur / (float)max;
	}


    void SyncChips(object args){
        chipNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.stone.ToString();
    }

    void SyncStamina(object args){
        int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;
        int staminaMax = DataCenter.Instance.UserData.UserInfo.staminaMax;
//        stamMaxValueLabel.text = staminaNow.ToString();
		stamNowValueLabel.text = staminaNow.ToString() + "/" + staminaMax.ToString();
        stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);
    }

	void AddStamina(){
		if (currentTime > 0) {
			if(DataCenter.Instance.UserData.UserInfo.staminaNow >= DataCenter.Instance.UserData.UserInfo.staminaMax){
				countDown.text = "";
			}
			else{
				countDown.text = GameTimer.GetMinSecBySeconds (currentTime);
			}

			currentTime--;
		}else{
			currentTime = 600;
			if(DataCenter.Instance.UserData.UserInfo.staminaNow < DataCenter.Instance.UserData.UserInfo.staminaMax)
				DataCenter.Instance.UserData.UserInfo.staminaNow++;
			int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;
			int staminaMax = DataCenter.Instance.UserData.UserInfo.staminaMax;
//			stamMaxValueLabel.text = staminaNow.ToString();
			stamNowValueLabel.text = staminaNow.ToString() + "/" + staminaMax.ToString();
			stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);
		}



	}

	private void OnUpdateInfo(object data){
		chipNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.stone.ToString();
		cionNumValueLabel.text = DataCenter.Instance.UserData.AccountInfo.money.ToString();

		int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;
		int staminaMax = DataCenter.Instance.UserData.UserInfo.staminaMax;
		//        stamMaxValueLabel.text = staminaNow.ToString();
		stamNowValueLabel.text = staminaNow.ToString() + "/" + staminaMax.ToString();
		stamSpr.fillAmount = CountFillCount(staminaNow, staminaMax);
	}
}
