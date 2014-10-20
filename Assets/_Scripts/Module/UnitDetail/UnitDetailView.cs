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
	
//	Material unitMaterial;

	List<UISprite> blockLsit1 = new List<UISprite>();
	List<UISprite> blockLsit2 = new List<UISprite>();

	int currMaxExp, curExp, gotExp, expRiseStep;

	int _curLevel = 0; 
	
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

	private UserUnit curUserUnit;

	public static bool isEvolve = false;
	private bool isFavorStateChanged = false;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config, data);
		InitUI();

//		ResourceManager.Instance.LoadLocalAsset("Materials/UnitMaterial", o=>{
//			unitMaterial = o as Material;
//			if( unitMaterial == null )
//				Debug.LogError("Scene -> UnitDetail : Not Find UnitMaterial");
//		});
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		statusToggle.value = true;
		
		foreach (var item in blockLsit1){
			item.enabled = false;
			item.spriteName = string.Empty;
		}
		foreach (var item in blockLsit2){
			item.enabled = false;
			item.spriteName = string.Empty;
		}

		NoviceGuideStepManager.Instance .StartStep (NoviceGuideStartType.UNITS);

		if(viewData.ContainsKey("unit")){
			curUserUnit = viewData["unit"] as UserUnit;
			ShowInfo (curUserUnit);
			UpdateFavView(curUserUnit.isFavorite);
		}


	}

	public override void HideUI () {
		base.HideUI ();
		iTween.Stop ();

		if( isFavorStateChanged ) {
			ModuleManager.SendMessage(ModuleEnum.LevelUpModule, "RefreshUnitItem", curUserUnit);
			isFavorStateChanged = false;
		}
	}
	
	//----------Init functions of UI Elements----------
	void InitUI() {
		unitBodyTex = FindChild< UITexture >("Bottom/detailSprite");
		materilItem = FindChild<Transform>("Center/MaterialItem").gameObject;
		parent = FindChild<UIGrid> ("Center/UIGrid").gameObject;
		
		GameObject go = FindChild<Transform>("Bottom/Bg").gameObject;
		UIEventListenerCustom.Get (go).onClick = ClickTexture;

		unitLock = FindChild("Top/Button_Lock");
		UIEventListenerCustom.Get(unitLock).onClick = ClickLock;

		unitInfoTabs = transform.Find("Bottom/UnitInfoTabs").gameObject;

		tabSkill2 = transform.Find("Bottom/UnitInfoTabs/Tab_Skill2").gameObject;
		UIEventListenerCustom.Get(tabSkill2).onClick = ClickTab;

		tabProfile = transform.Find("Bottom/UnitInfoTabs/Tab_Profile").gameObject;
		UIEventListenerCustom.Get(tabProfile).onClick = ClickTab;

		tabStatus = transform.Find("Bottom/UnitInfoTabs/Tab_Status").gameObject;
		UIEventListenerCustom.Get(tabStatus).onClick = ClickTab;

		//top
		cost = transform.FindChild("Top/Cost").GetComponent<UILabel>();
		number = transform.FindChild("Top/No").GetComponent<UILabel>();
		name = transform.FindChild("Top/Name").GetComponent<UILabel>();
		type = transform.FindChild ("Top/Type").GetComponent<UISprite> ();
		grayStar = transform.FindChild ("Top/Star2").GetComponent<UISprite> ();
		lightStar = transform.FindChild ("Top/Star2/Star1").GetComponent<UISprite> ();



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

		int count;
		for( count =0; count <=4; count++ ){
			blockLsit1.Add( FindChild<UISprite>("Bottom/UnitInfoTabs/Content_Skill2/Block/Block1/" + count.ToString()) );
		}

		for( count =0; count <=4; count++ ){
			blockLsit2.Add( FindChild<UISprite>("Bottom/UnitInfoTabs/Content_Skill2/Block/Block2/" + count.ToString()) );
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



	}

	bool ShowTexture = false;

	bool isNoviceGUide = true;

	void ClickTexture( GameObject go ){
		AudioManager.Instance.StopAudio (AudioEnum.sound_level_up);

		ShowTexture = false;
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );	

		unitBodyTex.mainTexture = null;
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
					CityInfo cityInfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
					StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);
					if ( cityInfo!=null && stageInfo!= null) {
						gw += cityInfo.cityName+"-"+stageInfo.stageName+"\n";
					}
				}
			}
		}
		return gw;
	}

	//------------------levelup-----------------------------------------


	
	//------------------end-----------------------------------------
	void ShowInfo(UserUnit userUnit, bool isLevelUp = false) {


		if (!isLevelUp) {
			ShowBodyTexture( userUnit ); 
			ShowUnitScale();
		}

		///-----------show top panel
		UnitInfo unitInfo = userUnit.UnitInfo;
		number.text = userUnit.unitId.ToString();
		if (number.text.Length < 3) {
			number.text = (number.text.Length == 1) ? ("00" + number.text) : ("0" + number.text);
		}
		
		name.text = unitInfo.name; //TextCenter.GetText ("UnitName_" + unitInfo.ID);//
		
		type.spriteName = "type_" + unitInfo.UnitType;
		
		cost.text = unitInfo.cost.ToString();
		
		int len = 0;
		if (unitInfo.maxStar > unitInfo.rare) {
			grayStar.enabled = true;
			grayStar.width = (unitInfo.maxStar - unitInfo.rare) * grayWidth;
			len = 2*unitInfo.rare - unitInfo.maxStar;
		} else {
			grayStar.enabled = false;
			len = unitInfo.rare;
		}
		lightStar.width = unitInfo.rare*lightWidth;
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
		
		levelLabel.text = userUnit.level.ToString() + " / " + unitInfo.maxLevel.ToString();
		//		Debug.Log("ShowStatusContent() :: data.Level="+data.Level+" nextExp:"+data.NextExp);
		//next level need
		if ((userUnit.level > unitInfo.maxLevel ) 
		    || (userUnit.level == unitInfo.maxLevel && userUnit.NextExp <= 0) ) {
			levelLabel.text = unitInfo.maxLevel.ToString() + " / " + unitInfo.maxLevel.ToString();
			needExpLabel.text = TextCenter.GetText("Text_Max");
			expSlider.value = 1f;
		} else {
			needExpLabel.text = TextCenter.GetText("Text_Next")+": " + userUnit.NextExp.ToString();
			expSlider.value = userUnit.CurExp*1.0f / (userUnit.CurExp + userUnit.NextExp);
		}

		//////--------skill 1
		int skillId1 = unitInfo.skill1;
		if (skillId1 == 0) {
			normalSkill1NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill1DscpLabel.text = "";
			return;	
		}
		SkillBase sbi1 = DataCenter.Instance.BattleData.GetSkill (userUnit.MakeUserUnitKey (), skillId1, SkillType.NormalSkill); //Skill[ skillId ];
		SkillBase skill1 = sbi1;
		
		normalSkill1NameLabel.text = TextCenter.GetText ("SkillName_"+skill1.id);//skill.name;
		normalSkill1DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill1.id);//skill.description;
		
		NormalSkill ns1 = sbi1 as NormalSkill;
		List<uint> sprNameList1 = ns1.activeBlocks;
		for( int i = 0; i < sprNameList1.Count; i++ ){
			blockLsit1[ i ].enabled = true;
			blockLsit1[ i ].spriteName = sprNameList1[ i ].ToString();
		}

		///skill 2
		int skillId2 = unitInfo.passiveSkill == 0 ? unitInfo.skill2 : unitInfo.passiveSkill;
		if (skillId2 == 0) {
			normalSkill2NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill2DscpLabel.text = "";
			return;	
		}
		
		SkillBase sbi2 = null;//Skill[ skillId ];
		SkillBase skill2 = null;
		
		if (unitInfo.passiveSkill == 0) {
			sbi2 = DataCenter.Instance.BattleData.GetSkill (userUnit.MakeUserUnitKey (), skillId2, SkillType.NormalSkill);
			skill2 = sbi2;
			FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Normal_Skill_2");
			NormalSkill ns2 = sbi2 as NormalSkill;
			List<uint> sprNameList2 = ns2.activeBlocks;
			for( int i = 0; i < sprNameList2.Count; i++ ){
				blockLsit2[ i ].enabled = true;
				blockLsit2[ i ].spriteName = sprNameList2[ i ].ToString();
			}
		}else{
			sbi2 = DataCenter.Instance.BattleData.GetSkill (userUnit.MakeUserUnitKey (), skillId2, SkillType.PassiveSkill);
			skill2 = sbi2;
			FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Passive_Skill");
		}
		normalSkill2NameLabel.text = TextCenter.GetText ("SkillName_"+skill2.id); //skill.name;
		normalSkill2DscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill2.id);//skill.description;


		///------------leader skill
		int skillId3 = unitInfo.leaderSkill;
		if (skillId3 == 0) {
			leaderSkillNameLabel.text = TextCenter.GetText("Text_None");
			leaderSkillDscpLabel.text = "";
			return;	
		}
		SkillBase skill3 = DataCenter.Instance.BattleData.GetSkill (userUnit.MakeUserUnitKey (), skillId3, SkillType.NormalSkill);
		leaderSkillNameLabel.text = TextCenter.GetText ("SkillName_"+skill3.id);//skill.name;
		leaderSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_"+skill3.id);//skill.description;

		////-----------active skill
		/// TUnitInfo unitInfo = data.UnitInfo;
		int skillId4 = unitInfo.activeSkill;
		if (skillId4 == 0) {
			activeSkillNameLabel.text = TextCenter.GetText("Text_None");
			activeSkillDscpLabel.text = "";
			return;	
		} 
		SkillBase skill4 = DataCenter.Instance.BattleData.GetSkill (userUnit.MakeUserUnitKey (), skillId4, SkillType.NormalSkill);
		activeSkillNameLabel.text = TextCenter.GetText ("SkillName_" + skill4.id);//skill.name;
		activeSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_" + skill4.id);//skill.description;
	

		////--------profile content
		if (unitInfo.race == EUnitRace.EVOLVEPARTS || (unitInfo.id >= 49 && unitInfo.id <= 72)) {
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

	void ScaleEnd(){
		if (!isEvolve) {
			return;
		}
		if( DataCenter.evolveInfo != null )
			DataCenter.evolveInfo.ClearData ();
		
		isEvolve = false;
//		
//		evolveEffectIns = NGUITools.AddChild(unitBodyTex.gameObject, evolveEffect);
//		Vector3 pos = new Vector3 (0f, unitBodyTex.height * 0.5f, 0f);
//		evolveEffectIns.transform.localPosition = pos;
//		evolveEffectIns.layer = GameLayer.EffectLayer;
		
		AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
	}
	
	void ShowBodyTexture( UserUnit data ){
		if (data==null) {
			Debug.LogError("ShowBodyTexture(null) >>>> ERROR: data is null!");
			return;
		}
		UnitInfo unitInfo = data.UnitInfo;
		ResourceManager.Instance.GetAvatar( UnitAssetType.Profile,unitInfo.id, o=>{
			Texture2D target = o as Texture2D;
			DGTools.ShowTexture(unitBodyTex, target);
			ShowTexture = true;
		});
		
	}

	
	//---------Exp increase----------





	private void ShowFavState(object msg){
		UpdateFavView(curUserUnit.isFavorite);
	}

	private void ClickLock(GameObject go){
		Debug.LogError ("ClickLock : " + curUserUnit);
		bool isFav = (curUserUnit.isFavorite == 1) ? true : false;
		EFavoriteAction favAction = isFav ? EFavoriteAction.DEL_FAVORITE : EFavoriteAction.ADD_FAVORITE;
		UnitController.Instance.UnitFavorite(OnRspChangeFavState, curUserUnit.uniqueId, favAction);
	}

	private void OnRspChangeFavState(object data){
		if(data == null) {Debug.LogError("OnRspChangeFavState(), data is NULL"); return;}
		bbproto.RspUnitFavorite rsp = data as bbproto.RspUnitFavorite;
		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			LogHelper.LogError("OnRspChangeFavState code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			
			return;
		}

		curUserUnit.isFavorite = (curUserUnit.isFavorite == 1) ? 0 : 1;

		//update the user unit 
		DataCenter.Instance.UnitData.UserUnitList.UpdateMyUnit( curUserUnit );

		UpdateFavView(curUserUnit.isFavorite);

		isFavorStateChanged = true;
	}
		
	private void UpdateFavView(int isFav){
		UISprite background = unitLock.transform.FindChild("Background").GetComponent<UISprite>();

		if ( curUserUnit.userID != DataCenter.Instance.UserData.UserInfo.userId ) {
			background.enabled = false; // hide Lock icon
			return;
		} else {
			background.enabled = true;
		}

		if(isFav == 1){
			background.spriteName = "Lock_close";
		}
		else{
			background.spriteName = "Lock_open";
		}
	}


}
