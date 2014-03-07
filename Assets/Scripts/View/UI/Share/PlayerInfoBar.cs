using bbproto;
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
	UISprite evolveTypeSprite;

	Vector3 leftPosition;
	Vector3 rightPosition;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);

		InitUI();
		RequestData();
	}

	public override void ShowUI () {
		UpdateData ();
		AddCommandListener();
		base.ShowUI ();
                
        }

	public override void HideUI () {
        	RemoveCommandListener();
                if( UIManager.Instance.baseScene.CurrentScene == SceneEnum.UnitDetail )
			return;
		base.HideUI ();
                
        }

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	void TurnToReName(){
//		Debug.Log("PlayerInfoBar.TurnToReName() : Start");
		if(GlobalData.userInfo == null ){
			Debug.LogError("GlobalData.userInfo is null");
			return;
		}
		
		if(GlobalData.userInfo.NickName == null ) {
			Debug.LogError("GlobalData.userInfo.NickName is null");
			return;
		}
		
		if(GlobalData.userInfo.NickName.Length == 0){
			UIManager.Instance.ChangeScene( SceneEnum.Others );
			Debug.Log ("PlayerInfoBar.ChangeScene( Others ).");
		}
			Debug.Log("PlayerInfoBar.TurnToReName() : End. NickName is " + GlobalData.userInfo.NickName);
        }

        private void InitUI() {
		FindObject();
		FindLabel();

		leftPosition = new Vector3( -UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0 );
		rightPosition = new Vector3( UIConfig.playerInfoBox_X, UIConfig.playerInfoBox_Y, 0 );

		UIEventListener.Get( leftCollider.gameObject ).onPress = ShowInfoBox;
		UIEventListener.Get( rightCollider.gameObject ).onPress = ShowInfoBox;

		infoBox.SetActive( false );

		//ShowStaminaInfo();
		//ShowExpInfo();
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

		evolveTypeSprite = FindChild<UISprite>("InfoBar/Sprite_EvolveType");
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


	void UpdateData() {
		if( GlobalData.userInfo == null ){
			Debug.Log("PlayerInfoBar.UpdateData() , userInfo is null , return ");
			return;
		}
		//Rank
		VRankLabel.text = GlobalData.userInfo.Rank.ToString();
		VRankHideLabel.text = GlobalData.userInfo.Rank.ToString();
		//Name
		VUserNameLabel.text = GlobalData.userInfo.NickName;
		//Exp
		int nextExp = GlobalData.userInfo.NextExp;
		int curTotalExp = GlobalData.userInfo.CurRankExp;
		VNeedExpHideLabel.text = nextExp.ToString();
		VTotalExpHideLabel.text = curTotalExp.ToString();
		//TODO Get current rank max exp 
		expSprite.fillAmount = CountFillCount( curTotalExp - nextExp, curTotalExp );
		//Cion
		if ( GlobalData.accountInfo != null ){
			VCionCountLabel.text = GlobalData.accountInfo.Money.ToString();
			VChipCountLabel.text = GlobalData.accountInfo.Stone.ToString();
		}
		//Stamina
		int staminaNow = GlobalData.userInfo.StaminaNow;
		int staminaMax = GlobalData.userInfo.StaminaMax;
		VStamMaxLabel.text = staminaNow.ToString();
		VStaminaNowLabel.text = staminaMax.ToString();
		staminaSprite.fillAmount = CountFillCount(staminaNow, staminaMax);

		//Evo
		int evoType = (int)GlobalData.userInfo.EvolveType;
		evolveTypeSprite.spriteName = evoType.ToString();

		//Debug.Log("PlayerInfoBar,GlobalData.userInfo.EvolveType : " + evoType.ToString());
		TurnToReName();

	}
	

	void ReName( object data ){
		bool state = (bool)data;
		Debug.LogError("PlayerInfoBar.ReName(), NickName : " + GlobalData.userInfo.NickName);
		if(state){
			VUserNameLabel.text = GlobalData.userInfo.NickName;
		}
		UIManager.Instance.ChangeScene( SceneEnum.Quest );
	}

	void AddCommandListener(){
//		MsgCenter.Instance.AddListener(CommandEnum.RspRenameNick, ReName );

		// leiliang---------------------------------------------------------------
		MsgCenter.Instance.AddListener (CommandEnum.ReqRenameNick, ChangeName);
	}
	
	void RemoveCommandListener(){
//		MsgCenter.Instance.RemoveListener(CommandEnum.RspRenameNick, ReName );

		// leiliang---------------------------------------------------------------
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqRenameNick, ChangeName);
     }

	void RequestData(){
//		MsgCenter.Instance.AddListener( CommandEnum.RspAuthUser, UpdateData );
		//TODO:temp for user login
//		MsgCenter.Instance.Invoke(CommandEnum.ReqAuthUser, null);
	}
	
	float  CountFillCount( int cur, int max ){
		if( cur < 0 || max <= 0 ) return 0f;
		return (float)cur/(float)max;
	}

	// leiliang--------------------------------------------------------------------
	private INetBase changeName;
	public void ChangeName(object  name) {
		if (changeName == null) {
			changeName = new RenameNick();
			changeName.OnRequest(name,ReName);
			UIManager.Instance.ChangeScene(SceneEnum.Start);
		}
	}
}
