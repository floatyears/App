using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailPanel : UIComponentUnity,IUICallback{
	public UILabel hpTextLabel;
	public UILabel atkTextLabel;
	public UILabel raceTextLabel;
	public UILabel statusTextLabel;
	public UILabel normalSkillTextLabel;
	public UILabel profileTextLabel;

	UnitDetailTopPanel topPanel;

	GameObject unitInfoTabs;

	UILabel hpLabel;
	UILabel atkLabel;
	UILabel raceLabel;

	UILabel levelLabel;
	UILabel needExpLabel;
	UISlider expSlider;

	UILabel leaderSkillNameLabel;
	UILabel leaderSkillDscpLabel;
	UILabel activeSkillNameLabel;
	UILabel activeSkillDscpLabel;

	UILabel normalSkill1DscpLabel;
	UILabel normalSkill1NameLabel;
	
	UILabel normalSkill2DscpLabel;
	UILabel normalSkill2NameLabel;

	UILabel profileLabel;

//	GameObject tabSkill1;
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
        
	public bool fobidClick = false;

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

		topPanel = GameObject.Find ("UnitDetailTopPanel(Clone)").GetComponent<UnitDetailTopPanel> ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		UIManager.Instance.HideBaseScene();
		ResetStartToggle (statusToggle);
		ClearBlock( blockLsit1 );
		ClearBlock( blockLsit2 );

		//TODO:
		//StartCoroutine ("nextState");
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
	}

//	IEnumerator nextState()
//	{
//		yield return new WaitForSeconds (1);
//		NoviceGuideStepEntityManager.Instance ().NextState ();
//	}

	public override void HideUI () {
		base.HideUI ();
		if (IsInvoking ("CreatEffect")) {
			CancelInvoke("CreatEffect");
		}
		ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}


	//----------Init functions of UI Elements----------
	void InitUI() {
//		favBtn = transform.FindChild("Button_Lock").GetComponent<UIButton>();
//		UIEventListener.Get(favBtn.gameObject).onClick = CollectCurUnit;

		unitInfoTabs = transform.Find("UnitInfoTabs").gameObject;
//		tabSkill1 = transform.Find("UnitInfoTabs/Tab_Skill1").gameObject;
//		UIEventListener.Get(tabSkill1).onClick = ClickTab;

		tabSkill2 = transform.Find("UnitInfoTabs/Tab_Skill2").gameObject;
		UIEventListener.Get(tabSkill2).onClick = ClickTab;

		tabProfile = transform.Find("UnitInfoTabs/Tab_Profile").gameObject;
		UIEventListener.Get(tabProfile).onClick = ClickTab;

		tabStatus = transform.Find("UnitInfoTabs/Tab_Status").gameObject;
		UIEventListener.Get(tabStatus).onClick = ClickTab;

		InitTabSkill();
		InitTabStatus ();
		InitTexture ();
		InitProfile();
		InitTextLabel();
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

//		noLabel			= FindChild<UILabel> (rootPath + "InputFrame_No"	);
//		nameLabel		= FindChild<UILabel> (rootPath + "InputFrame_Name"	);
		levelLabel		= FindChild<UILabel> (rootPath + "InputFrame_Lv"	);
//		typeLabel		= FindChild<UILabel> (rootPath + "InputFrame_Type"	);
		raceLabel		= FindChild<UILabel> (rootPath + "InputFrame_Race"	);
		hpLabel			= FindChild<UILabel> (rootPath + "InputFrame_HP"	);
//		costLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Cost"	);
//		rareLabel 		= FindChild<UILabel> (rootPath + "InputFrame_Rare"	);
		atkLabel 		= FindChild<UILabel> (rootPath + "InputFrame_ATK"	);
		needExpLabel	= FindChild<UILabel>( rootPath + "Label_Exp_Need"	);
		expSlider		= FindChild<UISlider>	(rootPath + "ExperenceBar"	);

		statusToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");

		FindChild<UILabel> (rootPath + "Bg_Input/Leader_Skill").text = TextCenter.GetText ("Text_Leader_Skill");
		FindChild<UILabel> (rootPath + "Bg_Input/Active_Skill").text = TextCenter.GetText ("Text_Active_Skill");
	}

	void InitTabSkill(){
		string rootPath;
		// skill_1
		rootPath 				=  "UnitInfoTabs/Content_Status/";
		leaderSkillNameLabel	= FindChild<UILabel>(rootPath + "Desc_LeaderSkill");
		leaderSkillDscpLabel	= FindChild<UILabel>(rootPath + "Label_ActiveSkill");
		activeSkillNameLabel	= FindChild<UILabel>(rootPath + "Desc_ActiveSkill");
		activeSkillDscpLabel	= FindChild<UILabel>(rootPath + "Label_LeaderSkill");
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

		rootPath 				= "UnitInfoTabs/Content_Skill2/Block/Block2/";
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>(rootPath + count.ToString());
			blockLsit2.Add( spr );
		}

		FindChild<UILabel> ("UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_1").text = TextCenter.GetText ("Text_Normal_Skill_1");
		FindChild<UILabel> ("UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Normal_Skill_2");
	}
	
	//Make panel focus on the same tab every time when this ui show
	void ResetStartToggle( UIToggle target) {
		target.value = true;
	}

	void GetUnitMaterial(){
		ResourceManager.Instance.LoadLocalAsset("Materials/UnitMaterial", o=>{
			unitMaterial = o as Material;
			if( unitMaterial == null )
				Debug.LogError("Scene -> UnitDetail : Not Find UnitMaterial");
		});

	}

	void LevelUp( object data){
		//Get BaseUnitInfo
		TUserUnit baseUnitData = data as TUserUnit;
		ExpRise();
	}

	//----------deal with effect----------
	void ClearEffectCache(){
		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}

	void InitEffect(){
//		string path = "Prefabs/UI/UnitDetail/LevelUpEffect";
		string path = "Effect/HelixHealingYellow";
		ResourceManager.Instance.LoadLocalAsset( path , o =>{
			levelUpEffect = o as GameObject;
		});
	}

	void ClickTexture( GameObject go ){
		if (fobidClick) {
			return;	
		}
		StopAllCoroutines ();
		ClearEffectCache ();
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
//		Debug.LogError ("unit detail SceneEnum : " + preScene);
		UIManager.Instance.ChangeScene( preScene );
	}

//	void ShowUnitScale(){
//		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
//		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();
//
//		unitAlpha.eventReceiver = this.gameObject;
//		unitAlpha.callWhenFinished = "PlayCheckRoleAudio";
//
//		if( unitScale == null || unitAlpha == null )
//			return;
//
//		unitScale.Reset();
//		unitScale.PlayForward();
//
//		unitAlpha.Reset();
//		unitAlpha.PlayForward();
//	}

	void PlayCheckRoleAudio(){
		//Debug.LogError("callWhenFinished...PlayCheckRoleAudio()");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
	}

//	void ShowBodyTexture( TUserUnit data ){
//		TUnitInfo unitInfo = data.UnitInfo;
//		Texture2D target = unitInfo.GetAsset( UnitAssetType.Profile);
//		unitBodyTex.mainTexture = target;
//		if (target == null) {
//			return;	
//		}
//		unitBodyTex.width = target.width;
//		unitBodyTex.height = target.height;
//	}
		
	void ShowStatusContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;

//		noLabel.text = data.UnitID.ToString();
		
		//hp
		hpLabel.text = data.Hp.ToString();
		
		//atk
		atkLabel.text = data.Attack.ToString();
		
		//name
//		nameLabel.text = unitInfo.Name;
		
		//type
//		typeLabel.text = unitInfo.UnitType;
		
		//cost
//		costLabel.text = unitInfo.Cost.ToString();
		
		//race  
		raceLabel.text = unitInfo.UnitRace;

		//rare
//		rareLabel.text = unitInfo.Rare.ToString();

		levelLabel.text = data.Level.ToString();

//		Debug.LogError("ShowInfo :: Lv.text:"+levelLabel.text);

		//next level need
		if ((data.Level > unitInfo.MaxLevel ) 
		    || (data.Level == unitInfo.MaxLevel && data.NextExp <= 0) ) {
			levelLabel.text = unitInfo.MaxLevel.ToString();
			needExpLabel.text = TextCenter.GetText("Text_Max");
			expSlider.value = 1f;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next")+": " + data.NextExp.ToString();
//			Debug.LogError("ShowInfo ->  needExpLabel.text="+needExpLabel.text);
			expSlider.value = data.CurExp*1.0f / (data.CurExp + data.NextExp);
		}
	}


	void ShowSkill1Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill1;
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill); //Skill[ skillId ];
		SkillBase skill =sbi.GetSkillInfo();

		normalSkill1NameLabel.text = TextCenter.GetText ("SkillName_"+skill.id);//skill.name;
		normalSkill1DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill.id);//skill.description;

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
		if (skillId == 0) {
			return;	
		}
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill);//Skill[ skillId ];
		SkillBase skill = sbi.GetSkillInfo();
                
		normalSkill2NameLabel.text = TextCenter.GetText ("SkillName_"+skill.id); //skill.name;
		normalSkill2DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill.id);//skill.description;

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
		if (skillId == 0) {
			return;	
		}
		SkillBase skill = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill).GetSkillInfo();
		leaderSkillNameLabel.text = TextCenter.GetText ("SkillName_"+skill.id);//skill.name;
		leaderSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill.id);//skill.description;
	}

	void ShowActiveSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.ActiveSkill;
		if (skillId == 0) {
			return;	
		} 
		SkillBase skill = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill).GetSkillInfo();
		activeSkillNameLabel.text = TextCenter.GetText ("SkillName_" + skill.id);//skill.name;
		activeSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_" + skill.id);//skill.description;
    }
        
	void ShowProfileContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		profileLabel.text = unitInfo.Profile;
	}

	//--------------interface function-------------------------------------
	private TUserUnit curUserUnit;
	public void CallbackView(object data)	{
		TUserUnit userUnit = data as TUserUnit;

		curUserUnit = userUnit;

		if ( oldBlendUnit != null ) {
//			Debug.LogError("CallbackView :: ShowInfo for oldBlendUnit...");
			ShowInfo (oldBlendUnit);
		}
		else if (userUnit != null) {
//			Debug.LogError("CallbackView :: ShowInfo for currentUnit...");
			ShowInfo (userUnit);
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
		levelUpData = rlu;
		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
//		Debug.LogError ("PlayLevelUp :: newBlend.UnitId:"+newBlendUnit.UnitInfo.ID);
//		Debug.LogError ("unitBodyTex : " + unitBodyTex + " newBlendUnit : " + newBlendUnit + " newBlendUnit.UnitInfo : " + newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile));
//		DGTools.ShowTexture (unitBodyTex, newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile));
//		unitInfoTabs.SetActive (false);
		SetEffectCamera ();
		StartCoroutine (CreatEffect ());
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;

	bool levelDone = false;

	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
//		Debug.LogError ("camera : " + camera);
		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
		camera.orthographicSize = 1.3f;
	}

	public void RecoverEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		camera.orthographicSize = 1f;
	}

	IEnumerator CreatEffect() {
		yield return new WaitForSeconds(0.5f);
		GameObject go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);
		yield return new WaitForSeconds(0.1f);
		go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);

		if (effectCache.Count == 6) {
//			CancelInvoke("CreatEffect");
//			yield break;
			yield return new WaitForSeconds(2f);

			ClearEffectCache ();
//			unitInfoTabs.SetActive (true);
			topPanel.ShowPanel();

			ShowLevelInfo (newBlendUnit);
			curLevel = oldBlendUnit.Level;
			gotExp = levelUpData.blendExp;

			levelDone = gotExp > 0;

			curExp = oldBlendUnit.CurExp;
//			Debug.Log ("CreatEffect :: gotExp : " + gotExp);
//			Debug.Log ("CreatEffect :: newBlendUnit.level : " + newBlendUnit.Level);
//			Debug.Log ("CreatEffect :: oldBlendUnit.CurExp : " + curExp + " oldBlendUnit.Lv:"+oldBlendUnit.Level +" unitId:"+oldBlendUnit.UnitID);

			Calculate ();

			RecoverEffectCamera();
		} else {
			yield return new WaitForSeconds(1.5f);
			StartCoroutine (CreatEffect ());
		}
	}
	
	//------------------end-----------------------------------------
	void ShowInfo(TUserUnit userUnit) {
//		ShowFavView(curUserUnit.IsFavorite);
//		ShowBodyTexture( userUnit ); 
//		ShowUnitScale();
		ShowStatusContent( userUnit );
		ShowSkill1Content( userUnit );
		ShowSkill2Content( userUnit );
		ShowLeaderSkillContent( userUnit );
		ShowActiveSkillContent( userUnit );
		ShowProfileContent( userUnit );

	}

	void ShowLevelInfo (TUserUnit userUnit) {
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
//		Debug.LogError("curlevel : " +curLevel + " MaxLevel : "+ oldBlendUnit.UnitInfo.MaxLevel) ;

		levelLabel.text = curLevel.ToString ();

		//DataCenter.Instance.GetUnitValue (oldBlendUnit.UnitInfo.ExpType, curLevel);
		currMaxExp = oldBlendUnit.UnitInfo.GetLevelExp(curLevel); 

		expRiseStep = (int)(currMaxExp * 0.01f);
		if ( expRiseStep < 1 )
			expRiseStep = 1;
//		Debug.LogError ("Calculate => currMaxExp:" + currMaxExp + "  expRiseStep : " + expRiseStep + " curlevel : " +curLevel + " MaxLevel : "+ oldBlendUnit.UnitInfo.MaxLevel);
	}
	
	//---------Exp increase----------
	
	void Update(){
		ExpRise();
	} 

	void ExpRise () {
		if (gotExp <= 0) {
			if(levelDone) {
				MsgCenter.Instance.Invoke(CommandEnum.levelDone);
				levelDone = false;
				oldBlendUnit = null;
			}
			return;	
		}	
			
//		LogHelper.LogError("<<<<<<<<gotExp:{0} expRiseStep:{1} - curExp:{2}  currMaxExp:{3}",gotExp, expRiseStep, curExp, currMaxExp);

		if(gotExp < expRiseStep){
			curExp += gotExp;
			gotExp = 0;
		} 
		else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
		}

//		Debug.LogError ("gotExp: " + gotExp + " expRiseStep: " + expRiseStep + " curExp: " + curExp + " currMaxExp: " + currMaxExp);

		if(curExp >= currMaxExp) {
//			LogHelper.LogError("-------gotExp:{0} curExp:{1} - currMaxExp:{2} = {3}",gotExp, curExp, currMaxExp, curExp - currMaxExp);
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

//			LogHelper.LogError("=======gotExp:{0} curExp:{1} curLevel:{2} ",gotExp, curExp, curLevel);

			Calculate();
		}



		int needExp = currMaxExp - curExp;

//		LogHelper.LogError(">>>>>>>>>currMaxExp:{0} - curExp:{1} = needExp{2} , curLevel:{3} expRiseStep:{4} ",currMaxExp, curExp, needExp, curLevel, expRiseStep);

		if ((curLevel > oldBlendUnit.UnitInfo.MaxLevel) 
		    || (curLevel == oldBlendUnit.UnitInfo.MaxLevel && needExp <= 0) ) {
			levelLabel.text = oldBlendUnit.UnitInfo.MaxLevel.ToString();
			needExpLabel.text = "Max";
			expSlider.value = 1.0f;
			return;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next") + needExp.ToString();
		}

		float progress = (float)curExp / (float)currMaxExp;
		if (progress == 0) {
			progress = 0.1f;
		}
//		Debug.Log ("exp slide progress: " + progress);
		expSlider.value = progress;
	}

	private void InitTextLabel(){
		hpTextLabel.text = TextCenter.GetText("Text_HP");
		atkTextLabel.text = TextCenter.GetText("Text_ATK");
		statusTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Status");
		normalSkillTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Normal_Skill");
		profileTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Prifile");
		raceTextLabel.text = TextCenter.GetText ("Text_RACE");
	}
}
