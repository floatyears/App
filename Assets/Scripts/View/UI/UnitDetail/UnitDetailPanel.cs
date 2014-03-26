using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailPanel : UIComponentUnity,IUICallback{

	GameObject unitInfoTabs;
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

	GameObject tabSkill1;
	GameObject tabSkill2;
	GameObject tabStatus;
	GameObject tabProfile;

	UIToggle statusToggle;
	UITexture unitBodyTex;

	GameObject levelUpEffect;
	Material unitMaterial;
	List<GameObject> effectCache = new List<GameObject>();

	List<UISprite> blockLsit1 = new List<UISprite>();
	List<UISprite> blockLsit2 = new List<UISprite>();
        
	int currMaxExp, curExp, gotExp, expRiseStep;

	int _curLevel = 0; 
	int curLevel {
		get {return _curLevel;}
		set {
			_curLevel = value;
			if(levelLabel != null) {
				levelLabel.text = value.ToString();
			}
		}
	}
	
	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		GetUnitMaterial();
		InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {

		base.ShowUI ();
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
		unitInfoTabs = transform.Find("UnitInfoTabs").gameObject;
		tabSkill1 = transform.Find("UnitInfoTabs/Tab_Skill1").gameObject;
		UIEventListener.Get(tabSkill1).onClick = ClickTab;

		tabSkill2 = transform.Find("UnitInfoTabs/Tab_Skill2").gameObject;
		UIEventListener.Get(tabSkill2).onClick = ClickTab;

		tabProfile = transform.Find("UnitInfoTabs/Tab_Profile").gameObject;
		UIEventListener.Get(tabProfile).onClick = ClickTab;

		tabStatus = transform.Find("UnitInfoTabs/Tab_Status").gameObject;
		UIEventListener.Get(tabStatus).onClick = ClickTab;

		InitTabSkill();
		InitTabStatus ();
//		InitExpSlider ();
		InitTexture ();
		InitProfile();
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void InitProfile() {
		string rootPath			= "UnitInfoTabs/Content_Profile/";
		profileLabel			= FindChild<UILabel>(rootPath + "Label_info"	);
	}
	
	void InitTexture(){
		unitBodyTex = FindChild< UITexture >("detailSprite");
		UIEventListener.Get( unitBodyTex.gameObject ).onClick = ClickTexture;
	}

	void InitTabStatus() {
		string rootPath = "UnitInfoTabs/Content_Status/";

		noLabel			= FindChild<UILabel> (rootPath + "InputFrame_No"	);
		nameLabel		= FindChild<UILabel> (rootPath + "InputFrame_Name"	);
		levelLabel		= FindChild<UILabel> (rootPath + "InputFrame_Lv"	);
		typeLabel		= FindChild<UILabel> (rootPath + "InputFrame_Type"	);
		raceLabel		= FindChild<UILabel> (rootPath + "InputFrame_Race"	);
		hpLabel			= FindChild<UILabel> (rootPath + "InputFrame_HP"	);
		costLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Cost"	);
		rareLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Rare"	);
		atkLabel 		= FindChild<UILabel> (rootPath + "InputFrame_ATK"	);
		needExpLabel	= FindChild<UILabel>( rootPath + "Label_Exp_Need"	);
		expSlider		= FindChild<UISlider>	(rootPath + "ExperenceBar"	);

		statusToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");
	}

	void InitTabSkill(){

		string rootPath;

		// skill_1
		rootPath 				=  "UnitInfoTabs/Content_Skill1/Label_Vaule/";
		leaderSkillNameLabel	= FindChild<UILabel>(rootPath + "Leader_Skill");
		leaderSkillDscpLabel	= FindChild<UILabel>(rootPath + "Leader_Skill_Dscp");
		activeSkillNameLabel	= FindChild<UILabel>(rootPath + "Active_Skill");
		activeSkillDscpLabel	= FindChild<UILabel>(rootPath + "Active_Skill_Dscp");
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
		ExpRise();
	}
	

	//----------deal with effect----------
	void ClearEffectCache(){
//		foreach (var item in effectCache){
//			Destroy( item );
//			effectCache.Remove(item);
//		}

		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}

	void InitEffect(){
		string path = "Prefabs/UI/UnitDetail/LevelUpEffect";
		levelUpEffect = Resources.Load( path ) as GameObject;
	}

	void ClickTexture( GameObject go ){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
		UIManager.Instance.ChangeScene( preScene );
	}

	void ShowUnitScale(){

		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();

		unitAlpha.eventReceiver = this.gameObject;
		unitAlpha.callWhenFinished = "PlayCheckRoleAudio";

		if( unitScale == null || unitAlpha == null )
			return;

		unitScale.Reset();
		unitScale.PlayForward();

		unitAlpha.Reset();
		unitAlpha.PlayForward();
	}

	void PlayCheckRoleAudio(){
		//Debug.LogError("callWhenFinished...PlayCheckRoleAudio()");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
	}

	void ShowBodyTexture( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		Texture2D target = unitInfo.GetAsset( UnitAssetType.Profile);
		unitBodyTex.mainTexture = target;
		unitBodyTex.width = target.width;
		unitBodyTex.height = target.height;
	}
		
	void ShowStatusContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;

		noLabel.text = data.UnitID.ToString();
		
		//hp
		int hp = DataCenter.Instance.GetUnitValue( unitInfo.HPType, data.Level );
		hpLabel.text = hp.ToString();
		
		//atk
		int atk = DataCenter.Instance.GetUnitValue(unitInfo.AttackType, data.Level);
		atkLabel.text = atk.ToString();
		
		//name
		nameLabel.text = unitInfo.Name;
		
		//type
		typeLabel.text = unitInfo.UnitType;
		
		//cost
		costLabel.text = unitInfo.Cost.ToString();
		
		//race  
		raceLabel.text = unitInfo.UnitRace.ToString();

		//rare
		rareLabel.text = unitInfo.Rare.ToString();
		
		//next level need
		needExpLabel.text = data.NextExp.ToString();
	}


	void ShowSkill1Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill1;
		SkillBaseInfo sbi = DataCenter.Instance.Skill[ skillId ];
		SkillBase skill =sbi.GetSkillInfo();

		normalSkill1NameLabel.text = skill.name;
		normalSkill1DscpLabel.text = skill.description;

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> sprNameList1 = ns.Object.activeBlocks;
		for( int i = 0; i < sprNameList1.Count; i++ ){
			blockLsit1[ i ].enabled = true;
			blockLsit1[ i ].spriteName = sprNameList1[ i ].ToString();
		}
	}

	void ShowSkill2Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill2;
		SkillBaseInfo sbi = DataCenter.Instance.Skill[ skillId ];
		SkillBase skill =sbi.GetSkillInfo();
                
        normalSkill2NameLabel.text = skill.name;
		normalSkill2DscpLabel.text = skill.description;

		TNormalSkill ns = sbi as TNormalSkill;
		List<uint> sprNameList2 = ns.Object.activeBlocks;
		for( int i = 0; i < sprNameList2.Count; i++ ){
			blockLsit2[ i ].enabled = true;
			blockLsit2[ i ].spriteName = sprNameList2[ i ].ToString();
        }
	}

	void ShowLeaderSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.LeaderSkill;
		SkillBase skill = DataCenter.Instance.Skill[ skillId ].GetSkillInfo();
                
        leaderSkillNameLabel.text = skill.name;
		leaderSkillDscpLabel.text = skill.description;
	}

	void ShowActiveSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.ActiveSkill;
		SkillBase skill = DataCenter.Instance.Skill[ skillId ].GetSkillInfo();		
		activeSkillNameLabel.text = skill.name;
		activeSkillDscpLabel.text = skill.description;
    }
        
	void ShowProfileContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		profileLabel.text = unitInfo.Profile;
	}

	//--------------interface function-------------------------------------
	public void Callback(object data)	{
		TUserUnit userUnit = data as TUserUnit;
		if (userUnit != null) {
			ShowInfo (userUnit);
			levelLabel.text = userUnit.Level.ToString();
		} else {
			RspLevelUp rlu = data as RspLevelUp;
			if(rlu ==null) {
				return;
			}
			PlayLevelUp(rlu);
		}
	}

	//------------------levelup-----------------------------------------
	RspLevelUp levelUpData;
	void PlayLevelUp(RspLevelUp rlu) {
		unitBodyTex.mainTexture = null;
		levelUpData = rlu;

//		TUserUnit blendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
//		gotExp = levelUpData.blendExp;
		unitInfoTabs.SetActive (false);
		InvokeRepeating ("CreatEffect", 0f, 2f);
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;

	void CreatEffect() {
		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);

		GameObject go = Instantiate (levelUpEffect) as GameObject;
		GameObject ProfileTexture = go.transform.Find ("ProfileTexture").gameObject;
		ProfileTexture.renderer.material.mainTexture = newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile);
		effectCache.Add (go);
	
		if (effectCache.Count > 2) {
			CancelInvoke("CreatEffect");
			unitInfoTabs.SetActive (true);
			ShowLevelInfo(newBlendUnit);
			curLevel = oldBlendUnit.Level;
			gotExp = levelUpData.blendExp;
			curExp = oldBlendUnit.CurExp;

			Debug.LogError("CreatEffect :: gotExp : " + gotExp);
			Debug.LogError("CreatEffect :: level : " + newBlendUnit.Level);
			Debug.LogError("CreatEffect :: CurExp : " + curExp);

			Calculate();
		}
	}
	
	//------------------end-----------------------------------------

	void ShowInfo(TUserUnit userUnit) {
		ShowBodyTexture( userUnit ); 
		ShowUnitScale();
		ShowStatusContent( userUnit );
		ShowSkill1Content( userUnit );
		ShowSkill2Content( userUnit );
		ShowLeaderSkillContent( userUnit );
		ShowActiveSkillContent( userUnit );
		ShowProfileContent( userUnit );
	}

	void ShowLevelInfo (TUserUnit userUnit) {
		ShowUnitScale();
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

	void Calculate () {
		if( oldBlendUnit == null ) {
			Debug.LogError("Calculate() :: oldBlendUnit=null");
			return;
		}

		currMaxExp = DataCenter.Instance.GetUnitValue (oldBlendUnit.UnitInfo.ExpType, curLevel);
		expRiseStep = (int)(currMaxExp * 0.01f);
	}


	//---------Exp increase----------

	
	void Update(){
		ExpRise();
	} 

	void ExpRise () {
		if(gotExp <= 0)	
			return;
//		LogHelper.LogError("<<<<<<<<gotExp:{0} expRiseStep:{1} - curExp:{2}  currMaxExp:{3}",gotExp, expRiseStep, curExp, currMaxExp);

		if(gotExp < expRiseStep){
			curExp += gotExp;
			gotExp = 0;
		} 
		else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
		}

		if(curExp >= currMaxExp) {
			LogHelper.LogError("-------gotExp:{0} curExp:{1} - currMaxExp:{2} = {3}",gotExp, curExp, currMaxExp, curExp - currMaxExp);
			gotExp += curExp - currMaxExp;
			curExp = 0;
			if ( curLevel < oldBlendUnit.UnitInfo.MaxLevel ){
				curLevel++;
			}
			else { // reach MaxLevel
				//TODO: show MAX on the progress bar
				curExp = currMaxExp;
				gotExp = 0;
			}

			LogHelper.LogError("=======gotExp:{0} curExp:{1} curLevel:{2} ",gotExp, curExp, curLevel);

			Calculate();
		}

//		LogHelper.LogError(">>>>>>>>>currMaxExp:{0} curExp:{1} curLevel:{2} ",currMaxExp, curExp, curLevel);

		int needExp = currMaxExp - curExp;
		needExpLabel.text = needExp.ToString();
		float progress = (float)curExp / (float)currMaxExp;
		expSlider.value = progress;
	}
}
