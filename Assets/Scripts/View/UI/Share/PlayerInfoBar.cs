using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBar : UIComponentUnity {
	private GameObject infoBox;
	private GameObject leftCollider;
    
	private GameObject rightCollider;
	private Vector3 leftPosition;
	private Vector3 rightPosition;
                        
	//Label in "InfoBox"
	private UILabel infoBox_Label_Text_Rank;
	private UILabel infoBox_Label_Text_NextExp;
	private UILabel infoBox_Label_Text_TotalExp;
	private UILabel infoBox_Label_Vaule_Rank;
	private UILabel infoBox_Label_Vaule_NextExp;
	private UILabel infoBox_Label_Vaule_TotalExp;

	//Label in "InfoBar"
	private UILabel infoBar_Label_Text_Line;
	private UILabel infoBar_Label_Text_Rank;
	private UILabel chipLabel;
	private UILabel staminaNowLabel;
	private UILabel cionLabel;
	private UILabel userNameLabel;
	private UILabel rankLabel;
	private UILabel staminaMaxLabel;

	private UISprite curExpSpr;
	private UISprite staminaSpr;

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);


		


		InitUI();

		ReceiveData();
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		if( UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail )
			return;
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		FindObject();
		FindLabel();

		leftPosition = new Vector3( -UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0 );
		rightPosition = new Vector3( UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0 );

		UIEventListener.Get( leftCollider.gameObject ).onPress = ShowInfoBox;
		UIEventListener.Get( rightCollider.gameObject ).onPress = ShowInfoBox;

		infoBox.SetActive( false );

		ShowStaminaInfo();
		ShowExpInfo();
	}
	
	private void FindObject() {
		infoBox = FindChild( "InfoBox" );
		leftCollider = FindChild( "Left_Collider" );
		rightCollider = FindChild( "Right_Collider" );
	}
	private void FindLabel() {
		infoBox_Label_Text_Rank = FindChild< UILabel >( "InfoBox/Label_Text_Rank" );
		infoBox_Label_Text_NextExp = FindChild< UILabel >( "InfoBox/Label_Text_NextExp" );
		infoBox_Label_Text_TotalExp = FindChild< UILabel>( "InfoBox/Label_Text_TotalExp");
		infoBox_Label_Vaule_Rank = FindChild< UILabel>( "InfoBox/Label_Vaule_Rank");
		infoBox_Label_Vaule_NextExp = FindChild< UILabel>( "InfoBox/Label_Vaule_NextExp");
		infoBox_Label_Vaule_TotalExp = FindChild< UILabel>( "InfoBox/Label_Vaule_TotalExp");
		
		infoBar_Label_Text_Line = FindChild< UILabel >( "InfoBar/Label_Text_Line" );
		infoBar_Label_Text_Rank = FindChild< UILabel >( "InfoBar/Label_Text_Rank" );
		chipLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_ChipNum" );
		staminaNowLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_CurStamina" );
		cionLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_Icon" );
		userNameLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_PlayerName" );
		rankLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_Rank" );
		staminaMaxLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_TotalStamina" );

		curExpSpr = FindChild<UISprite>("InfoBar/Sprite_CurExp");
		staminaSpr = FindChild< UISprite >("InfoBar/Sprite_Stamina");
	}

	private void ShowInfoBox( GameObject go, bool isPressed ) {
		infoBox.SetActive( isPressed );
		
		if( go.name == "Left_Collider") {
			infoBox.transform.localPosition = rightPosition;
		}
		else if(go.name == "Right_Collider") {
			infoBox.transform.localPosition = leftPosition;
		}
	}

	int curStamina = 39;
	int maxStamina = 61;
	int curExp = 492;
	int nextRandNeedExp = 1856;

	private void ShowStaminaInfo(){
		staminaNowLabel.text =  curStamina.ToString();
		staminaMaxLabel.text = maxStamina.ToString();
		float percent = (float)curStamina/maxStamina;
		//Debug.LogError(percent);
		staminaSpr.fillAmount = percent;
	}

	private void ShowExpInfo(){
		float percent = (float)curExp/nextRandNeedExp;
		curExpSpr.fillAmount = percent;
		//Debug.LogError(percent);
	}


	void UpdateData( object data ){
		Debug.LogError(GlobalData.userInfo.Rank.ToString());
		rankLabel.text = GlobalData.userInfo.Rank.ToString();
		staminaMaxLabel.text = GlobalData.userInfo.StaminaMax.ToString();
		staminaNowLabel.text = GlobalData.userInfo.StaminaNow.ToString();
		userNameLabel.text = GlobalData.userInfo.NickName;
//		cionLabel.text = GlobalData.userInfo

	}

	void ReceiveData(){
		MsgCenter.Instance.AddListener( CommandEnum.RspAuthUser, UpdateData );
		//TODO:temp for user login
		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}


}
