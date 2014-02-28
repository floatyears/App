using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBar : UIComponentUnity {
	GameObject infoBox;
	GameObject leftCollider;
	GameObject rightCollider;
 
	//Out 
	UILabel TLineLabel;
	UILabel TRankLabel;
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

	Vector3 leftPosition;
	Vector3 rightPosition;

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
		TRankHideLabel = FindChild< UILabel >( "InfoBox/Label_Text_Rank" );
		TNeedExpHideLabel = FindChild< UILabel >( "InfoBox/Label_Text_NextExp" );
		TTotalExpHideLabel = FindChild< UILabel>( "InfoBox/Label_Text_TotalExp");
		VRankHideLabel = FindChild< UILabel>( "InfoBox/Label_Vaule_Rank");
		VNeedExpHideLabel = FindChild< UILabel>( "InfoBox/Label_Vaule_NextExp");
		VTotalExpHideLabel = FindChild< UILabel>( "InfoBox/Label_Vaule_TotalExp");
		
		TLineLabel = FindChild< UILabel >( "InfoBar/Label_Text_Line" );
		TRankLabel = FindChild< UILabel >( "InfoBar/Label_Text_Rank" );
		VChipCountLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_ChipNum" );
		VStamMaxLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_CurStamina" );
		VCionCountLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_Icon" );
		VUserNameLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_PlayerName" );
		VRankLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_Rank" );
		VStaminaNowLabel = FindChild< UILabel >( "InfoBar/Label_Vaule_TotalStamina" );

		expSprite = FindChild<UISprite>("InfoBar/Sprite_CurExp");
		staminaSprite = FindChild< UISprite >("InfoBar/Sprite_Stamina");
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
		VStamMaxLabel.text =  curStamina.ToString();
		VStaminaNowLabel.text = maxStamina.ToString();
		float percent = (float)curStamina/maxStamina;
		//Debug.LogError(percent);
		staminaSprite.fillAmount = percent;
	}

	private void ShowExpInfo(){
		float percent = (float)curExp/nextRandNeedExp;
		expSprite.fillAmount = percent;
		//Debug.LogError(percent);
	}


	void UpdateData( object data ){
		//Debug.LogError(GlobalData.userInfo.Rank.ToString());
		VRankLabel.text = GlobalData.userInfo.Rank.ToString();
		VUserNameLabel.text = GlobalData.userInfo.NickName;

		int staminaNow = GlobalData.userInfo.StaminaNow;
		int staminaMax = GlobalData.userInfo.StaminaMax;
		VStamMaxLabel.text = staminaNow.ToString();
		VStaminaNowLabel.text = staminaMax.ToString();
		staminaSprite.fillAmount = CountPercent(staminaNow, staminaMax);

		int expNow = GlobalData.userInfo.Exp;
//		int expMax = GlobalData.
//		expSpr.fillAmount = CountPercent();


	}

	void ReceiveData(){
		MsgCenter.Instance.AddListener( CommandEnum.RspAuthUser, UpdateData );
		//TODO:temp for user login
		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}

	float  CountPercent( int cur, int max ){
		if( cur < 0 || max <= 0 ) return 0f;
		return (float)cur/(float)max;
	}
	
}
