using UnityEngine;
using System.Collections;

public class PlayerInfoBarDecoratorUnity : UIComponentUnity {
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
	private UILabel infoBar_Label_Vaule_ChipNum;
	private UILabel infoBar_Label_Vaule_CurStamina;
	private UILabel infoBar_Label_Vaule_Icon;
	private UILabel infoBar_Label_Vaule_PlayerName;
	private UILabel infoBar_Label_Vaule_Rank;
	private UILabel infoBar_Label_Vaule_TotalStamina;

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();
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
		infoBar_Label_Vaule_ChipNum = FindChild< UILabel >( "InfoBar/Label_Vaule_ChipNum" );
		infoBar_Label_Vaule_CurStamina = FindChild< UILabel >( "InfoBar/Label_Vaule_CurStamina" );
		infoBar_Label_Vaule_Icon = FindChild< UILabel >( "InfoBar/Label_Vaule_Icon" );
		infoBar_Label_Vaule_PlayerName = FindChild< UILabel >( "InfoBar/Label_Vaule_PlayerName" );
		infoBar_Label_Vaule_Rank = FindChild< UILabel >( "InfoBar/Label_Vaule_Rank" );
		infoBar_Label_Vaule_TotalStamina = FindChild< UILabel >( "InfoBar/Label_Vaule_TotalStamina" );
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
}
