using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailView : ViewBase{
	public UILabel hpTextLabel;
	public UILabel atkTextLabel;
	public UILabel raceTextLabel;
	public UILabel statusTextLabel;
	public UILabel normalSkillTextLabel;
	public UILabel profileTextLabel;

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
	UILabel profileTitle;
	
	GameObject tabSkill2;
	GameObject tabStatus;
	GameObject tabProfile;

	UIToggle statusToggle;
	
	Material unitMaterial;

	List<UISprite> blockLsit1 = new List<UISprite>();
	List<UISprite> blockLsit2 = new List<UISprite>();

	int currMaxExp, curExp, gotExp, expRiseStep;

	int _curLevel = 0; 
	int curLevel {
		get {return _curLevel;}
		set {
			_curLevel = value;
		}
	}
	
	//top
	UISprite type;
	UILabel cost;
	UILabel number;
	UILabel name;
	UISprite lightStar;
	UISprite grayStar;
	
	private int grayWidth = 28;
	private int lightWidth = 29;
	
	public bool fobidClick = false;
	GameObject unitLock;

	//center
	UITexture unitBodyTex;
	GameObject materilItem;
	GameObject parent;

	public static bool isEvolve = false;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitUI();

		ResourceManager.Instance.LoadLocalAsset("Materials/UnitMaterial", o=>{
			unitMaterial = o as Material;
			if( unitMaterial == null )
				Debug.LogError("Scene -> UnitDetail : Not Find UnitMaterial");
		});
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		if (!gameObject.activeSelf) {
			gameObject.SetActive(true);	
		}

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);

		if (viewData.ContainsKey ("levelup")) {
			RspLevelUp rlu = viewData["levelup"] as RspLevelUp;
			isNoviceGUide = false;
			if(rlu ==null) {
				return;
			}
			isNoviceGUide = true;
			PlayLevelUp(rlu);
		}else if(viewData.ContainsKey("evolve")){

		}else if(viewData.ContainsKey("unit")){
			curUserUnit = viewData["unit"] as TUserUnit;
			ShowInfo (curUserUnit);
		}
	}
	
	public override void HideUI () {
		base.HideUI ();
		ClearEffectCache();
		iTween.Stop ();

		if (friendEffect.gameObject.activeSelf) {
			friendEffect.gameObject.SetActive(false);
		}
	}
	
	//----------Init functions of UI Elements----------
	void InitUI() {
		unitBodyTex = FindChild< UITexture >("Bottom/detailSprite");
		materilItem = FindChild<Transform>("Center/MaterialItem").gameObject;
		parent = FindChild<UIGrid> ("Center/UIGrid").gameObject;
		
		GameObject go = FindChild<Transform>("Bottom/Bg").gameObject;
		UIEventListener.Get (go).onClick = ClickTexture;

		unitLock = FindChild("Top/Button_Lock");
		UIEventListener.Get(unitLock).onClick = ClickLock;

		unitInfoTabs = transform.Find("Bottom/UnitInfoTabs").gameObject;

		tabSkill2 = transform.Find("Bottom/UnitInfoTabs/Tab_Skill2").gameObject;
		UIEventListener.Get(tabSkill2).onClick = ClickTab;

		tabProfile = transform.Find("Bottom/UnitInfoTabs/Tab_Profile").gameObject;
		UIEventListener.Get(tabProfile).onClick = ClickTab;

		tabStatus = transform.Find("Bottom/UnitInfoTabs/Tab_Status").gameObject;
		UIEventListener.Get(tabStatus).onClick = ClickTab;

		//top
		cost = transform.FindChild("Top/Cost").GetComponent<UILabel>();
		number = transform.FindChild("Top/No").GetComponent<UILabel>();
		name = transform.FindChild("Top/Name").GetComponent<UILabel>();
		type = transform.FindChild ("Top/Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Top/Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Top/Star2/Star1").GetComponent<UISprite> ();

		//center
		friendEffect = FindChild<UISprite>("Center/AE");
		friendSprite = FindChild<UISprite>("Center/AE/Avatar");
		friendEffect.gameObject.SetActive (false);

		initPos = FindChild<Transform> ("Center/InitPosition").localPosition;;
		endPos = FindChild<Transform> ("Center/EndPosition").localPosition;

		////--------------Tab Skill
		leaderSkillNameLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Status/Desc_LeaderSkill");
		leaderSkillDscpLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Status/Label_ActiveSkill");
		activeSkillNameLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Status/Desc_ActiveSkill");
		activeSkillDscpLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Status/Label_LeaderSkill");
		// skill_2

		normalSkill1NameLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Skill2/Label_Vaule/Normal_Skill1");
		normalSkill1DscpLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Skill2/Label_Vaule/Normal_Skill1_Dscp");
		normalSkill2NameLabel 	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Skill2/Label_Vaule/Normal_Skill2");
		normalSkill2DscpLabel	= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Skill2/Label_Vaule/Normal_Skill2_Dscp");

		UISprite spr;
		int count;
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>("Bottom/UnitInfoTabs/Content_Skill2/Block/Block1/" + count.ToString());
			blockLsit1.Add( spr );
		}

		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>("Bottom/UnitInfoTabs/Content_Skill2/Block/Block2/" + count.ToString());
			blockLsit2.Add( spr );
		}

		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_1").text = TextCenter.GetText ("Text_Normal_Skill_1");
		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Normal_Skill_2");


		///------------Tab Status
		
		levelLabel		= FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/InputFrame_Lv"	);
		raceLabel		= FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/InputFrame_Race"	);
		hpLabel			= FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/InputFrame_HP"	);
		atkLabel 		= FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/InputFrame_ATK"	);
		needExpLabel	= FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/Label_Exp_Need"	);
		expSlider		= FindChild<UISlider>("Bottom/UnitInfoTabs/Content_Status/ExperenceBar"	);
		
		statusToggle = FindChild<UIToggle>("Bottom/UnitInfoTabs/Tab_Status");
		
		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/Bg_Input/Leader_Skill").text = TextCenter.GetText ("Text_Leader_Skill");
		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Status/Bg_Input/Active_Skill").text = TextCenter.GetText ("Text_Active_Skill");

		////-----------profile
		profileLabel			= FindChild<UILabel>("Bottom/UnitInfoTabs/Content_Profile/Label_info"	);
		profileTitle = FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Profile/Title");

		///-----------texture label
		hpTextLabel.text = TextCenter.GetText("Text_HP");
		atkTextLabel.text = TextCenter.GetText("Text_ATK");
		statusTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Status");
		normalSkillTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Normal_Skill");
		profileTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Prifile");
		raceTextLabel.text = TextCenter.GetText ("Text_RACE");


		////---------------Effect
		if (levelUpEffect == null) {

			ResourceManager.Instance.LoadLocalAsset("Effect/effect/LevelUpEffect" , o =>{
				levelUpEffect = o as GameObject;
			});	
		}
		if (swallowEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/level_up01" , o =>{
				swallowEffect = o as GameObject;
			});
		}
		if (linhunqiuEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/linhunqiu1" , o =>{
				linhunqiuEffect = o as GameObject;
			});	
		}
		if (evolveEffect == null) {
			ResourceManager.Instance.LoadLocalAsset( "Effect/effect/evolve" , o =>{
				evolveEffect = o as GameObject;
			});	
		}
	}

	private void Reset(){
		statusToggle.value = true;

		foreach (var item in blockLsit1){
			item.enabled = false;
			item.spriteName = string.Empty;
		}
		foreach (var item in blockLsit2){
			item.enabled = false;
			item.spriteName = string.Empty;
		}
	}

	bool ShowTexture = false;

	bool isNoviceGUide = true;

	void ClickTexture( GameObject go ){
		if (!ShowTexture) {
			return;	
		}

		if (isNoviceGUide && NoviceGuideStepEntityManager.isInNoviceGuide ()) {
			return;	
		}

		AudioManager.Instance.StopAudio (AudioEnum.sound_level_up);

		ShowTexture = false;
		StopAllCoroutines ();
		ClearEffectCache ();
		LevelUpEnd ();

		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );	

		unitBodyTex.mainTexture = null;

		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.EVOLVE) {
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
		}

		ModuleManager.Instance.HideModule (ModuleEnum.UnitDetailModule);
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}
//
//	void LevelUpFunc(object data) {
//		RspLevelUp rlu = data as RspLevelUp;
//		if(rlu ==null) {
//			return;
//		}
//		PlayLevelUp(rlu);
//	} 

	void ScaleEnd(){
		if (DataCenter.gameState != GameState.Evolve && !isEvolve) {
			return;
		}
		
		DataCenter.evolveInfo.ClearData ();
		
		isEvolve = false;
		
		evolveEffectIns = NGUITools.AddChild(unitBodyTex.gameObject, evolveEffect);
		Vector3 pos = new Vector3 (0f, unitBodyTex.height * 0.5f, 0f);
		evolveEffectIns.transform.localPosition = pos;
		evolveEffectIns.layer = GameLayer.EffectLayer;

		AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
	}

	string GetWayString(List<bbproto.UnitGetWay> getway){
		string gw = "";
		foreach (var item in getway) {
			if ( item.getType ==  EUnitGetType.E_NORMAL_QUEST || item.getType ==  EUnitGetType.E_EVENT_QUEST || item.getType == EUnitGetType.E_STAGE ) {
				uint questId = item.getPath;
				uint stageId =  questId/10;
				if ( item.getType == EUnitGetType.E_STAGE ) {
					stageId = item.getPath;
				}					
				uint cityId = stageId/10;
				if ( cityId>0 && stageId>0 ) {
					TCityInfo cityInfo = DataCenter.Instance.GetCityInfo(cityId);
					TStageInfo stageInfo = DataCenter.Instance.GetStageInfo(stageId);
					if ( cityInfo!=null && stageInfo!= null) {
						gw += cityInfo.CityName+"-"+stageInfo.StageName+"\n";
					}
				}
			}
		}
		return gw;
	}

	//------------------levelup-----------------------------------------
	RspLevelUp levelUpData;
	UISprite friendEffect;
	UISprite friendSprite;
	Vector3 initPos;
	Vector3 endPos;

	private const float yCoor = -142;

	void PlayLevelUp(RspLevelUp rlu) {
		Umeng.GA.Event ("PowerUp");

		levelUpData = rlu;
		oldBlendUnit = DataCenter.Instance.oldUserUnitInfo;
		newBlendUnit = DataCenter.Instance.UserUnitList.GetMyUnit(levelUpData.blendUniqueId);
		curUserUnit = newBlendUnit;
		ShowInfo (oldBlendUnit, true);
		TUserUnit tuu = DataCenter.Instance.levelUpFriend;
		ResourceManager.Instance.GetAvatarAtlas (tuu.UnitInfo.ID, friendSprite);
		friendEffect.gameObject.SetActive (true);
		friendEffect.spriteName = tuu.UnitType.ToString ();
		Transform effectTrans = friendEffect.transform;
		effectTrans.localPosition = initPos;
		Vector3 downEndPos = endPos + (-100f * Vector3.up);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_friend_up);

		iTween.MoveTo(friendEffect.gameObject,iTween.Hash("position",endPos,"time",0.35f,"easetype",iTween.EaseType.easeInQuart,"islocal",true));
		iTween.RotateFrom (friendEffect.gameObject, iTween.Hash ("z", 10, "time", 0.15f,"delay",0.35f, "easetype", iTween.EaseType.easeOutBack));
		iTween.MoveTo(friendEffect.gameObject,iTween.Hash("position",downEndPos,"time", 0.15f,"delay",0.5f,"easetype",iTween.EaseType.easeOutQuart,"islocal",true,"oncomplete","ShowLevelup","oncompletetarget",gameObject));
	}

	void ShowLevelup() {
		friendEffect.gameObject.SetActive (false);

		DataCenter dataCenter = DataCenter.Instance;
		for (int i = 0; i < dataCenter.levelUpMaterials.Count; i++) {
			TUnitInfo tui = dataCenter.levelUpMaterials[i].UnitInfo;
			GameObject go = NGUITools.AddChild(parent, materilItem);
			go.SetActive(true);
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			ResourceManager.Instance.GetAvatarAtlas(tui.ID, sprite);
			go.transform.Find("Background").GetComponent<UISprite>().spriteName = DGTools.GetItemBackgroundName(tui.Type);
			go.transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = DGTools.GetItemBorderName(tui.Type);
			material.Enqueue(go);
		}
		DataCenter.Instance.levelUpMaterials.Clear ();
		parent.GetComponent<UIGrid> ().Reposition ();
		count = material.Count;
		newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile, o => {
			DGTools.ShowTexture (unitBodyTex, o as Texture2D);
			ShowTexture = true;

			Vector3 localposition = unitBodyTex.transform.localPosition; 
			Vector3 tPos = new Vector3(localposition.x, localposition.y + unitBodyTex.height * 0.5f - 480f, localposition.z);
			targetPosition = tPos - parent.transform.localPosition; 
			ShowUnitScale();
			SetEffectCamera();
			StartCoroutine(SwallowUserUnit());
		});
	}

	void ShowLevelupInfo(object data) {
		ShowInfo (newBlendUnit, true);
		curLevel = oldBlendUnit.Level;
		gotExp = levelUpData.blendExp;
		
		levelDone = gotExp > 0;
		
		curExp = oldBlendUnit.CurExp;
		levelLabel.text = curLevel + " / " + oldBlendUnit.UnitInfo.MaxLevel;
		Calculate ();
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;
	
	private TUserUnit curUserUnit;
	
	bool levelDone = false;

	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
		camera.orthographicSize = 1.3f;
	}

	public void RecoverEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		camera.orthographicSize = 1f;
	}

	
	//------------------end-----------------------------------------
	void ShowInfo(TUserUnit userUnit, bool isLevelUp = false) {
		if (!isLevelUp) {
			ShowBodyTexture( userUnit ); 
			ShowUnitScale();
		}

		///-----------show top panel
		TUnitInfo unitInfo = userUnit.UnitInfo;
		number.text = userUnit.UnitID.ToString();
		
		name.text = unitInfo.Name; //TextCenter.GetText ("UnitName_" + unitInfo.ID);//
		
		type.spriteName = "type_" + unitInfo.UnitType;
		
		cost.text = unitInfo.Cost.ToString();
		
		int len = 0;
		if (unitInfo.MaxRare > unitInfo.Rare) {
			grayStar.enabled = true;
			grayStar.width = (unitInfo.MaxRare - unitInfo.Rare) * grayWidth;
			len = 2*unitInfo.Rare - unitInfo.MaxRare;
		} else {
			grayStar.enabled = false;
			len = unitInfo.Rare;
		}
		lightStar.width = unitInfo.Rare*lightWidth;
		grayStar.transform.localPosition = new Vector3(len * 15,-82,0);

		//-----status
		//hp
		hpLabel.text = userUnit.Hp.ToString();
		
		//atk
		atkLabel.text = userUnit.Attack.ToString ();
		
		//race  
		raceLabel.text = unitInfo.UnitRace;
		
		//rare
		//		rareLabel.text = unitInfo.Rare.ToString();
		
		levelLabel.text = userUnit.Level.ToString() + " / " + unitInfo.MaxLevel.ToString();
		//		Debug.Log("ShowStatusContent() :: data.Level="+data.Level+" nextExp:"+data.NextExp);
		//next level need
		if ((userUnit.Level > unitInfo.MaxLevel ) 
		    || (userUnit.Level == unitInfo.MaxLevel && userUnit.NextExp <= 0) ) {
			levelLabel.text = unitInfo.MaxLevel.ToString() + " / " + unitInfo.MaxLevel.ToString();
			needExpLabel.text = TextCenter.GetText("Text_Max");
			expSlider.value = 1f;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next")+": " + userUnit.NextExp.ToString();
			expSlider.value = userUnit.CurExp*1.0f / (userUnit.CurExp + userUnit.NextExp);
		}

		//////--------skill 1
		int skillId1 = unitInfo.NormalSkill1;
		if (skillId1 == 0) {
			normalSkill1NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill1DscpLabel.text = "";
			return;	
		}
		SkillBaseInfo sbi1 = DataCenter.Instance.GetSkill (userUnit.MakeUserUnitKey (), skillId1, SkillType.NormalSkill); //Skill[ skillId ];
		SkillBase skill1 = sbi1.GetSkillInfo();
		
		normalSkill1NameLabel.text = TextCenter.GetText ("SkillName_"+skill1.id);//skill.name;
		normalSkill1DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill1.id);//skill.description;
		
		TNormalSkill ns1 = sbi1 as TNormalSkill;
		List<uint> sprNameList1 = ns1.Object.activeBlocks;
		for( int i = 0; i < sprNameList1.Count; i++ ){
			blockLsit1[ i ].enabled = true;
			blockLsit1[ i ].spriteName = sprNameList1[ i ].ToString();
		}

		///skill 2
		int skillId2 = unitInfo.PassiveSkill == 0 ? unitInfo.NormalSkill2 : unitInfo.PassiveSkill;
		if (skillId2 == 0) {
			normalSkill2NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill2DscpLabel.text = "";
			return;	
		}
		
		SkillBaseInfo sbi2 = null;//Skill[ skillId ];
		SkillBase skill2 = null;
		
		if (unitInfo.PassiveSkill == 0) {
			sbi2 = DataCenter.Instance.GetSkill (userUnit.MakeUserUnitKey (), skillId2, SkillType.NormalSkill);
			skill2 = sbi2.GetSkillInfo();
			FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Normal_Skill_2");
			TNormalSkill ns2 = sbi2 as TNormalSkill;
			List<uint> sprNameList2 = ns2.Object.activeBlocks;
			for( int i = 0; i < sprNameList2.Count; i++ ){
				blockLsit2[ i ].enabled = true;
				blockLsit2[ i ].spriteName = sprNameList2[ i ].ToString();
			}
		}else{
			sbi2 = DataCenter.Instance.GetSkill (userUnit.MakeUserUnitKey (), skillId2, SkillType.PassiveSkill);
			skill2 = sbi2.GetSkillInfo();
			FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Passive_Skill");
		}
		normalSkill2NameLabel.text = TextCenter.GetText ("SkillName_"+skill2.id); //skill.name;
		normalSkill2DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill2.id);//skill.description;


		///------------leader skill
		int skillId3 = unitInfo.LeaderSkill;
		if (skillId3 == 0) {
			leaderSkillNameLabel.text = TextCenter.GetText("Text_None");
			leaderSkillDscpLabel.text = "";
			return;	
		}
		SkillBase skill3 = DataCenter.Instance.GetSkill (userUnit.MakeUserUnitKey (), skillId3, SkillType.NormalSkill).GetSkillInfo();
		leaderSkillNameLabel.text = TextCenter.GetText ("SkillName_"+skill3.id);//skill.name;
		leaderSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill3.id);//skill.description;

		////-----------active skill
		/// TUnitInfo unitInfo = data.UnitInfo;
		int skillId4 = unitInfo.ActiveSkill;
		if (skillId4 == 0) {
			activeSkillNameLabel.text = TextCenter.GetText("Text_None");
			activeSkillDscpLabel.text = "";
			return;	
		} 
		SkillBase skill4 = DataCenter.Instance.GetSkill (userUnit.MakeUserUnitKey (), skillId4, SkillType.NormalSkill).GetSkillInfo();
		activeSkillNameLabel.text = TextCenter.GetText ("SkillName_" + skill4.id);//skill.name;
		activeSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_" + skill4.id);//skill.description;
	

		////--------profile content
		if (unitInfo.Race == EUnitRace.EVOLVEPARTS || (unitInfo.ID >= 49 && unitInfo.ID <= 72)) {
			profileLabel.text = GetWayString(unitInfo.UnitGetWay);
			profileTitle.text = TextCenter.GetText ("UnitDetail_EvolveTitle");
		}else{
			profileLabel.text = string.Format(TextCenter.GetText ("UnitDetail_LevelUpContent") , unitInfo.DevourExp) + "\n" + string.Format(TextCenter.GetText("UnitDetail_LevelUpAttr"),unitInfo.UnitTypeText,(int)(unitInfo.DevourExp*1.5));
			profileTitle.text = TextCenter.GetText ("UnitDetail_LevelUpTitle");
		}

	}
	
	void ShowUnitScale(){
		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();
		
		unitAlpha.eventReceiver = this.gameObject;
		unitAlpha.callWhenFinished = "ScaleEnd";
		
		if( unitScale == null || unitAlpha == null )
			return;
		
		unitScale.ResetToBeginning();
		unitScale.PlayForward();
		
		unitAlpha.ResetToBeginning();
		unitAlpha.PlayForward();
	}
	
	void ShowBodyTexture( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		unitInfo.GetAsset( UnitAssetType.Profile, o=>{
			Texture2D target = o as Texture2D;
			DGTools.ShowTexture(unitBodyTex, target);
			ShowTexture = true;
		});
		
	}

	void Calculate () {
		if( oldBlendUnit == null ) {
			Debug.LogError("Calculate() :: oldBlendUnit=null");
			return;
		}

		levelLabel.text = curLevel.ToString () + " / " + oldBlendUnit.UnitInfo.MaxLevel;

		currMaxExp = oldBlendUnit.UnitInfo.GetLevelExp(curLevel); 

		expRiseStep = (int)(currMaxExp * 0.01f);
		if ( expRiseStep < 1 )
			expRiseStep = 1;
	}
	
	//---------Exp increase----------

	void PlayLevelupEffect () {
		AudioManager.Instance.PlayAudio(AudioEnum.sound_level_up);
		GameObject go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);
	}

	void Update(){
		ExpRise();
	} 

	void LevelUpEnd() {
		gotExp = 0;

		RecoverEffectCamera ();
		AudioManager.Instance.StopAudio (AudioEnum.sound_get_exp);

		if (oldBlendUnit != null) {
			if(curLevel >= oldBlendUnit.UnitInfo.MaxLevel && NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_EVOLVE) {
				UserguideEvoUnit.SendRequest(o=>{
					RspUserGuideEvolveUnit rsp = o as RspUserGuideEvolveUnit;
					if (rsp.header.code == ErrorCode.SUCCESS) {
						if (rsp != null ) {
							DataCenter.Instance.UserUnitList.AddMyUnitList(rsp.addUnit);
							NoviceGuideStepEntityManager.CurrentNoviceGuideStage = NoviceGuideStage.UNIT_EVOLVE_EXE;
						}
					}else {
						Debug.LogError("UserGuideEvolveUnit ret err:"+rsp.header.code);
					}
				},oldBlendUnit.UnitID);
			}

			oldBlendUnit = null;	
		}
	}

	void LevelupExpRiseEnd() {
		isNoviceGUide = false;
		AudioManager.Instance.StopAudio (AudioEnum.sound_get_exp);
	}

	void ExpRise () {
		if (gotExp <= 0) {
			if(levelDone) {
				MsgCenter.Instance.Invoke(CommandEnum.levelDone);
				levelDone = false;
				GameTimer.GetInstance().AddCountDown(1f, LevelupExpRiseEnd);
			}
			return;	
		}	

		if(gotExp < expRiseStep){
			curExp += gotExp;
			gotExp = 0;

			if(AudioManager.Instance.GetPlayAuioInfo() != AudioEnum.sound_get_exp)
				AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
		} else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;

			if(AudioManager.Instance.GetPlayAuioInfo() != AudioEnum.sound_get_exp)
				AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
		}

		if(curExp >= currMaxExp) {
			gotExp += curExp - currMaxExp;
			curExp = 0;
			if ( curLevel < oldBlendUnit.UnitInfo.MaxLevel ){
				curLevel ++;
				PlayLevelupEffect();
			} else { // reach MaxLevel
				//TODO: show MAX on the progress bar
				curExp = currMaxExp;
				gotExp = 0;
			}

//			LogHelper.LogError("=======gotExp:{0} curExp:{1} curLevel:{2} ",gotExp, curExp, curLevel);

			Calculate();
		}

		int needExp = currMaxExp - curExp;

		if ((curLevel > oldBlendUnit.UnitInfo.MaxLevel) 
		    || (curLevel == oldBlendUnit.UnitInfo.MaxLevel && needExp <= 0) ) {
			levelLabel.text = oldBlendUnit.UnitInfo.MaxLevel.ToString() + "/" + oldBlendUnit.UnitInfo.MaxLevel.ToString();
			needExpLabel.text = "Max";
			expSlider.value = 1.0f;
			return;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next") +": " + needExp.ToString();
		}

		float progress = (float)curExp / (float)currMaxExp;
		if (progress == 0) {
			progress = 0.1f;
		}
		expSlider.value = progress;
	}

	private void ShowFavState(object msg){
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void ClickLock(GameObject go){
		Debug.LogError ("ClickLock : " + curUserUnit);
		bool isFav = (curUserUnit.IsFavorite == 1) ? true : false;
		EFavoriteAction favAction = isFav ? EFavoriteAction.DEL_FAVORITE : EFavoriteAction.ADD_FAVORITE;
		UnitFavorite.SendRequest(OnRspChangeFavState, curUserUnit.ID, favAction);
	}

	private void OnRspChangeFavState(object data){
		if(data == null) {Debug.LogError("OnRspChangeFavState(), data is NULL"); return;}
		bbproto.RspUnitFavorite rsp = data as bbproto.RspUnitFavorite;
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.LogError("OnRspChangeFavState code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			
			return;
		}
		curUserUnit.IsFavorite = (curUserUnit.IsFavorite == 1) ? 0 : 1;
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void UpdateFavView(int isFav){
		UISprite background = unitLock.transform.FindChild("Background").GetComponent<UISprite>();
		Debug.Log("Name is : " + curUserUnit.UnitInfo.Name + "  UpdateFavView(), isFav : " + (isFav == 1));
		if(isFav == 1){
			background.spriteName = "Lock_close";
			Debug.Log("UpdateFavView(), isFav == 1, background.spriteName is Fav_Lock_Close");
		}
		else{
			background.spriteName = "Lock_open";
			Debug.Log("UpdateFavView(), isFav != 1, background.spriteName is Fav_Lock_Open");
		}
	}

	//center
	Vector3 targetPosition;
	
	Queue<GameObject> material = new Queue<GameObject> ();
	int count = 0;
	List<GameObject> effectCache = new List<GameObject>();
	GameObject levelUpEffect = null;
	GameObject swallowEffect = null;
	GameObject linhunqiuEffect = null;
	GameObject evolveEffect = null;

	
	GameObject materilUse = null;
	GameObject linhunqiuIns = null;
	GameObject swallowEffectIns = null;
	GameObject evolveEffectIns = null;
	
	IEnumerator SwallowUserUnit () {
		yield return new WaitForSeconds(1f);
		
		while (material.Count > 0) {
			materilUse = material.Dequeue();
			iTween.ScaleTo(materilUse, iTween.Hash("y", 0f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);
			Destroy(materilUse);
			linhunqiuIns = NGUITools.AddChild(parent, linhunqiuEffect);
			linhunqiuIns.transform.localPosition = materilUse.transform.localPosition;
			linhunqiuIns.transform.localScale = Vector3.zero;
			iTween.ScaleTo(linhunqiuIns, iTween.Hash("y", 1f, "time", 0.2f));
			yield return new WaitForSeconds(0.2f);
			iTween.MoveTo(linhunqiuIns, iTween.Hash("position", targetPosition, "time", 0.3f, "islocal", true));
			yield return new WaitForSeconds(0.3f);

			AudioManager.Instance.PlayAudio(AudioEnum.sound_devour_unit);

			Destroy(linhunqiuIns);
			swallowEffectIns = NGUITools.AddChild(gameObject, swallowEffect);
			yield return new WaitForSeconds(0.4f);
			Destroy(swallowEffectIns);
		}

		LevelUpAnim ();
	}

	void LevelUpAnim() {

		curLevel = oldBlendUnit.Level;
		levelLabel.text = curLevel + " / " + oldBlendUnit.UnitInfo.MaxLevel;
		gotExp = levelUpData.blendExp;
		
		levelDone = gotExp > 0;
		
		curExp = oldBlendUnit.CurExp;
		Calculate ();
	}
	
	void ClearEffectCache(){
		while (material.Count > 0) {
			Destroy(material.Dequeue());
		}

//		DGTools.SafeDestory (materilUse);
//		DGTools.SafeDestory (linhunqiuIns);
//		DGTools.SafeDestory (swallowEffectIns);
//		DGTools.SafeDestory (evolveEffectIns);

		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}
}
