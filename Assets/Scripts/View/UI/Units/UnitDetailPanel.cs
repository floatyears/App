using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailPanel : UIComponentUnity,IUICallback{
	//----------UI elements list----------
	UILabel noLabel;
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel raceLabel;
	UILabel costLabel;
	UILabel rareLabel;
	UILabel levelLabel;
	UILabel typeLabel;
	UILabel nameLabel;
	UILabel needExpLabel;
	UISlider expSlider;

	UILabel normalSkill1DscpLabel;
	UILabel normalSkill1NameLabel;
	
	UILabel normalSkill2DscpLabel;
	UILabel normalSkill2NameLabel;

	UILabel leaderSkillNameLabel;
	UILabel leaderSkillDscpLabel;

	UILabel activeSkillNameLabel;
	UILabel activeSkillDscpLabel;

	UILabel profileLabel;

	UIToggle statusToggle;
	UITexture unitBodyTex;

	GameObject levelUpEffect;
	Material unitMaterial;
	List<GameObject> effectCache = new List<GameObject>();

	List<UISprite> blockLsit1 = new List<UISprite>();
	List<UISprite> blockLsit2 = new List<UISprite>();
        
        protected int currMaxExp, curExp, gotExp, expRiseStep;

	
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		GetUnitMaterial();
		InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {

		base.ShowUI ();

		ShowUnitScale();
		UIManager.Instance.HideBaseScene();
		ResetStartToggle (statusToggle);
	}

	public override void HideUI () {

		base.HideUI ();

		ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}


	//----------Init functions of UI Elements----------
	void InitUI() {
		InitTabSkill();
		InitTabStatus ();
		InitExpSlider ();
		InitTexture ();
		InitProfile();
	}
	
	void InitProfile() {
		string rootPath			= "UnitInfoTabs/Content_Profile/";
		profileLabel			= FindChild<UILabel>(rootPath + "Label_info"			);
	}
	
	void InitTexture(){
		unitBodyTex = FindChild< UITexture >("detailSprite");
		UIEventListener.Get( unitBodyTex.gameObject ).onClick = ClickTexture;
	}

	void InitTabStatus() {
		string rootPath = "UnitInfoTabs/Content_Status/";

		noLabel				= FindChild<UILabel> (rootPath + "InputFrame_No"		);
		nameLabel		= FindChild<UILabel> (rootPath + "InputFrame_Name"	);
		levelLabel		= FindChild<UILabel> (rootPath + "InputFrame_Lv"		);
		typeLabel		= FindChild<UILabel> (rootPath + "InputFrame_Type"	);
		raceLabel		= FindChild<UILabel> (rootPath + "InputFrame_Race"	);
		hpLabel		= FindChild<UILabel> (rootPath + "InputFrame_HP"		);
		costLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Cost"	);
		rareLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Rare"	);
		atkLabel 		= FindChild<UILabel> (rootPath + "InputFrame_ATK"	);
		needExpLabel	= FindChild<UILabel>( rootPath + "Label_Exp_Need"	);
		expSlider		= FindChild<UISlider>	(rootPath + "ExperenceBar"		);

		statusToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");
	}

	void InitTabSkill(){

		string rootPath;

		// skill_1
		rootPath 				=  "UnitInfoTabs/Content_Skill1/Label_Vaule/";
		leaderSkillNameLabel		= FindChild<UILabel>(rootPath + "Leader_Skill");
		leaderSkillDscpLabel		= FindChild<UILabel>(rootPath + "Leader_Skill_Dscp");
		activeSkillNameLabel		= FindChild<UILabel>(rootPath + "Active_Skill");
		activeSkillDscpLabel		= FindChild<UILabel>(rootPath + "Active_Skill_Dscp");
		// skill_2
		rootPath 				= "UnitInfoTabs/Content_Skill2/Label_Vaule/";
		normalSkill1NameLabel	= FindChild<UILabel>(rootPath + "Normal_Skill1");
		normalSkill1DscpLabel	= FindChild<UILabel>(rootPath + "Normal_Skill1_Dscp");
		normalSkill2NameLabel 	= FindChild<UILabel>(rootPath + "Normal_Skill2");
		normalSkill2DscpLabel	= FindChild<UILabel>(rootPath + "Normal_Skill2_Dscp");

		rootPath 				= "UnitInfoTabs/Content_Skill2/Block/Block1/";
		UISprite spr;
		int count;
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>(rootPath + count.ToString());
			blockLsit1.Add( spr );
		}
		ClearBlock( blockLsit1 );
//		Debug.LogError( "BlockList1 count : " + blockLsit1.Count);

		rootPath 				= "UnitInfoTabs/Content_Skill2/Block/Block2/";
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>(rootPath + count.ToString());
			blockLsit2.Add( spr );
		}
		ClearBlock( blockLsit2 );
//                Debug.LogError( "BlockList2 count : " + blockLsit2.Count);

	}
	
	//Make panel focus on the same tab every time when this ui show
	void ResetStartToggle( UIToggle target) {
		target.value = true;
	}

	
	void GetUnitMaterial(){
		unitMaterial = Resources.Load("Materials/UnitMaterial") as Material;
		if( unitMaterial == null )
			Debug.LogError("Scene -> UnitDetail : Not Find UnitMaterial");
	}
	


	
	void LevelUp( object data){
		//Get BaseUnitInfo
		TUserUnit baseUnitData = data as TUserUnit;
		//ShowPanelContent( baseUnitData );

		//Get Material and Friend Data

		//Show exp increase process
		ExpRise();
	}
	

	//----------deal with effect----------
	void ClearEffectCache(){
		foreach (var item in effectCache){
			Destroy( item );
		}
		effectCache.Clear();
	}

	void InitEffect(){
		string path = "Prefabs/UI/UnitDetail/LevelUpEffect";
		levelUpEffect = Resources.Load( path ) as GameObject;
	}
	//----------end deal with effect----------
	


	void ClickTexture( GameObject go ){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
		UIManager.Instance.ChangeScene( preScene );
	}


	//----------deal with animation---------- 
	private void ShowUnitScale(){
		TweenScale unitScale = 
			gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha =
			gameObject.GetComponentInChildren< TweenAlpha >();

		if( unitScale == null || unitAlpha == null )
			return;

		unitScale.Reset();
		unitScale.PlayForward();

		unitAlpha.Reset();
		unitAlpha.PlayForward();
	}

	void ShowBodyTexture( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		unitBodyTex.mainTexture = unitInfo.GetAsset( UnitAssetType.Profile);
	}
		
	void ShowStatusContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		//no
		noLabel.text = data.UnitID.ToString();
		
		//level
		levelLabel.text = data.Level.ToString();
		
		//hp
		int hp = GlobalData.Instance.GetUnitValue( unitInfo.HPType, data.Level );
		hpLabel.text = hp.ToString();
		
		//atk
		int atk = GlobalData.Instance.GetUnitValue(unitInfo.AttackType, data.Level);
		atkLabel.text = atk.ToString();
		
		//name
		nameLabel.text = unitInfo.Name;
		
		//type
		typeLabel.text = unitInfo.UnitType;
		
		//cost
		costLabel.text = unitInfo.Cost.ToString();
		
		//race  
		raceLabel.text = unitInfo.Race.ToString();
		Debug.LogError("unitInfo.Race : "+unitInfo.Race.ToString());

		
		//rare
		rareLabel.text = unitInfo.Rare.ToString();
		Debug.LogError("unitInfo.Rare : "+unitInfo.Rare.ToString());
		
		//next level need
		needExpLabel.text = data.Exp.ToString();
	}


	void ShowSkill1Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill1;
		SkillBaseInfo sbi = GlobalData.skill[ skillId ];
		SkillBase skill =sbi.GetSkillInfo();

		normalSkill1NameLabel.text = skill.name;
		normalSkill1DscpLabel.text = skill.description;

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> sprNameList1 = ns.GetObject().activeBlocks;
		for( int i = 0; i < sprNameList1.Count; i++ ){
			blockLsit1[ i ].enabled = true;
			blockLsit1[ i ].spriteName = sprNameList1[ i ].ToString();
		}
	}

	void ShowSkill2Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill2;
		SkillBaseInfo sbi = GlobalData.skill[ skillId ];
		SkillBase skill =sbi.GetSkillInfo();
                
                normalSkill2NameLabel.text = skill.name;
		normalSkill2DscpLabel.text = skill.description;

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> sprNameList2 = ns.GetObject().activeBlocks;
		for( int i = 0; i < sprNameList2.Count; i++ ){
			blockLsit1[ i ].enabled = true;
			blockLsit1[ i ].spriteName = sprNameList2[ i ].ToString();
                }
        }

	void ShowLeaderSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.LeaderSkill;
		SkillBase skill = GlobalData.skill[ skillId ].GetSkillInfo();
                
                leaderSkillNameLabel.text = skill.name;
		leaderSkillDscpLabel.text = skill.description;
        }

	void ShowActiveSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.ActiveSkill;
		SkillBase skill = GlobalData.skill[ skillId ].GetSkillInfo();
		
		activeSkillNameLabel.text = skill.name;
		activeSkillDscpLabel.text = skill.description;
        }
        
        void ShowProfileContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		profileLabel.text = unitInfo.Profile;
	}

	public void Callback(object data)	{
		TUserUnit userUnit = data as TUserUnit;
		Debug.Log("UnitDetailPanel.Callback()");
		ShowBodyTexture( userUnit ); 
		ShowStatusContent( userUnit );
		ShowSkill1Content( userUnit );
		ShowSkill2Content( userUnit );
		ShowLeaderSkillContent( userUnit );
		ShowActiveSkillContent( userUnit );
		ShowProfileContent( userUnit );
        }
        
        void ClearBlock(List<UISprite> blocks){
		foreach (var item in blocks){
			item.enabled = false;
			item.spriteName = string.Empty;
		}
	}

        protected int curLevel;
	//---------Exp increase----------
	void InitExpSlider(){
		curExp = 460;
		curLevel = 10;
		gotExp = 3000;
		expRiseStep = currMaxExp / 120;
	}
	
	void Update(){
		ExpRise();
	}

	void ExpRise () {
		if(gotExp <= 0)	
			return;

		currMaxExp = GetMaxExpByLv( curLevel );

		if(gotExp < expRiseStep){
			//remain less than step, add remain
			curExp += gotExp;
			gotExp = 0;
		} else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
		}

		if(curExp >= currMaxExp) {
			//current overflow,add back
			gotExp += curExp - currMaxExp;

			curExp = 0;
			curLevel++;
			currMaxExp = GetMaxExpByLv( curLevel );
		}

		int needExp = currMaxExp - curExp;
		needExpLabel.text = needExp.ToString();
		float progress = (float)curExp / (float)currMaxExp;
		expSlider.value = progress;
	}

	int GetMaxExpByLv( int level) {
		return level*level + 1000; 
	}
	
}
