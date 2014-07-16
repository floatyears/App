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
			if(levelLabel != null) {
				levelLabel.text = value.ToString();
			}
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

//	private bool isOwnUnit = false;

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		GetUnitMaterial();
		InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		//clear the last texture 
		DGTools.ShowTexture (unitBodyTex, null);

		ResetStartToggle (statusToggle);
		ClearBlock( blockLsit1 );
		ClearBlock( blockLsit2 );
		MsgCenter.Instance.AddListener (CommandEnum.ShowLevelupInfo, ShowLevelupInfo);
		//TODO:
		//StartCoroutine ("nextState");
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.UNITS);
		MsgCenter.Instance.AddListener(CommandEnum.ShowFavState,  ShowFavState);
	}

 

	public override void HideUI () {
		base.HideUI ();
		if (IsInvoking ("CreatEffect")) {
			CancelInvoke("CreatEffect");
		}

//		UIManager.Instance.ShowBaseScene();

		MsgCenter.Instance.RemoveListener(CommandEnum.ShowFavState,  ShowFavState);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShowLevelupInfo, ShowLevelupInfo);
		ClearEffectCache();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	//----------Init functions of UI Elements----------
	void InitUI() {

		//center
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

		InitTabSkill();
		InitTabStatus ();
		InitProfile();
		InitTextLabel();
	}

	void ClickTexture( GameObject go ){
		StopAllCoroutines ();
		ClearEffectCache ();
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
		Debug.LogError ("ClickTexture : " + preScene);
		UIManager.Instance.ChangeScene( preScene );
	}

	void ClickTab(GameObject tab){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
	}

	void InitProfile() {
		string rootPath			= "Bottom/UnitInfoTabs/Content_Profile/";
		profileLabel			= FindChild<UILabel>(rootPath + "Label_info"	);
		profileTitle = FindChild<UILabel> (rootPath + "Title");
	}

	void LevelUpFunc(object data) {
		RspLevelUp rlu = data as RspLevelUp;
		if(rlu ==null) {
			return;
		}
		PlayLevelUp(rlu);
	} 

	void InitTabStatus() {
		string rootPath = "Bottom/UnitInfoTabs/Content_Status/";

		levelLabel		= FindChild<UILabel> (rootPath + "InputFrame_Lv"	);
		raceLabel		= FindChild<UILabel> (rootPath + "InputFrame_Race"	);
		hpLabel			= FindChild<UILabel> (rootPath + "InputFrame_HP"	);
		atkLabel 		= FindChild<UILabel> (rootPath + "InputFrame_ATK"	);
		needExpLabel	= FindChild<UILabel>( rootPath + "Label_Exp_Need"	);
		expSlider		= FindChild<UISlider>	(rootPath + "ExperenceBar"	);

		statusToggle = FindChild<UIToggle>("Bottom/UnitInfoTabs/Tab_Status");

		FindChild<UILabel> (rootPath + "Bg_Input/Leader_Skill").text = TextCenter.GetText ("Text_Leader_Skill");
		FindChild<UILabel> (rootPath + "Bg_Input/Active_Skill").text = TextCenter.GetText ("Text_Active_Skill");
	}

	void InitTabSkill(){
		string rootPath;
		// skill_1
		rootPath 				=  "Bottom/UnitInfoTabs/Content_Status/";
		leaderSkillNameLabel	= FindChild<UILabel>(rootPath + "Desc_LeaderSkill");
		leaderSkillDscpLabel	= FindChild<UILabel>(rootPath + "Label_ActiveSkill");
		activeSkillNameLabel	= FindChild<UILabel>(rootPath + "Desc_ActiveSkill");
		activeSkillDscpLabel	= FindChild<UILabel>(rootPath + "Label_LeaderSkill");
		// skill_2
		rootPath 				= "Bottom/UnitInfoTabs/Content_Skill2/Label_Vaule/";
		normalSkill1NameLabel	= FindChild<UILabel>(rootPath + "Normal_Skill1");
		normalSkill1DscpLabel	= FindChild<UILabel>(rootPath + "Normal_Skill1_Dscp");
		normalSkill2NameLabel 	= FindChild<UILabel>(rootPath + "Normal_Skill2");
		normalSkill2DscpLabel	= FindChild<UILabel>(rootPath + "Normal_Skill2_Dscp");

		rootPath 				= "Bottom/UnitInfoTabs/Content_Skill2/Block/Block1/";
		UISprite spr;
		int count;
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>(rootPath + count.ToString());
			blockLsit1.Add( spr );
		}

		rootPath 				= "Bottom/UnitInfoTabs/Content_Skill2/Block/Block2/";
		for( count =0; count <=4; count++ ){
			spr				= FindChild<UISprite>(rootPath + count.ToString());
			blockLsit2.Add( spr );
		}

		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_1").text = TextCenter.GetText ("Text_Normal_Skill_1");
		FindChild<UILabel> ("Bottom/UnitInfoTabs/Content_Skill2/Label_Text/Normal_Skill_2").text = TextCenter.GetText ("Text_Normal_Skill_2");
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
		TUserUnit baseUnitData = data as TUserUnit;
		ExpRise();
	}

	void PlayCheckRoleAudio(){
		PlayEvolveEffect ();
		AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
	}
		
	void PlayEvolveEffect () {
		Debug.LogError ("DataCenter.gameState : " + DataCenter.gameState);

		if (DataCenter.gameState != GameState.Evolve) {
			return;
		}

		evolveEffectIns = NGUITools.AddChild(gameObject, evolveEffect);
		evolveEffectIns.layer = GameLayer.EffectLayer;
	}

	void ShowStatusContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		
		//hp
		hpLabel.text = data.Hp.ToString();
		
		//atk
		atkLabel.text = data.Attack.ToString ();
		
		//race  
		raceLabel.text = unitInfo.UnitRace;

		//rare
//		rareLabel.text = unitInfo.Rare.ToString();

		levelLabel.text = data.Level.ToString();

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
		if (skillId == 0) {
			normalSkill1NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill1DscpLabel.text = "";
			return;	
		}
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
			normalSkill2NameLabel.text = TextCenter.GetText("Text_None");
			normalSkill2DscpLabel.text = "";
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
			leaderSkillNameLabel.text = TextCenter.GetText("Text_None");
			leaderSkillDscpLabel.text = "";
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
			activeSkillNameLabel.text = TextCenter.GetText("Text_None");
			activeSkillDscpLabel.text = "";
			return;	
		} 
		SkillBase skill = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill).GetSkillInfo();
		activeSkillNameLabel.text = TextCenter.GetText ("SkillName_" + skill.id);//skill.name;
		activeSkillDscpLabel.text = TextCenter.GetText ("SkillDesc_" + skill.id);//skill.description;
    }
        
	void ShowProfileContent( TUserUnit data ){
		TUnitInfo unitInfo = data.UnitInfo;
		if (unitInfo.Race == EUnitRace.EVOLVEPARTS || (unitInfo.ID >= 49 && unitInfo.ID <= 72)) {
			profileLabel.text = GetWayString(unitInfo.UnitGetWay);
			profileTitle.text = TextCenter.GetText ("UnitDetail_EvolveTitle");
		}else{
			profileLabel.text = string.Format(TextCenter.GetText ("UnitDetail_LevelUpContent") , unitInfo.DevourExp) + "\n" + string.Format(TextCenter.GetText("UnitDetail_LevelUpAttr"),unitInfo.UnitTypeText,(int)(unitInfo.DevourExp*1.5));
			profileTitle.text = TextCenter.GetText ("UnitDetail_LevelUpTitle");
		}

	}

	string GetWayString(List<bbproto.UnitGetWay> getway){
		string gw = "";
		foreach (var item in getway) {
			gw += item.getPath;
		}
		return gw;
	}

	//--------------interface function-------------------------------------
//	private TUserUnit curUserUnit;
	public void CallbackView(object data)	{
		TUserUnit userUnit = data as TUserUnit;


		curUserUnit = userUnit;

		if ( oldBlendUnit != null ) {
			Debug.LogError("CallbackView :: ShowInfo for oldBlendUnit...");
			ShowInfo (oldBlendUnit);
		}
		else if (userUnit != null) {
			Debug.LogError("CallbackView :: ShowInfo for currentUnit... : " + userUnit.UnitInfo.ID);
			if (userUnit.userID == DataCenter.Instance.UserInfo.UserId) {
				unitLock.SetActive(true);
			} else {
				unitLock.SetActive(false);
			}

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
		
		for (int i = 0; i < rlu.partUniqueId.Count; i++) {
			TUnitInfo tui = DataCenter.Instance.UserUnitList.Get(rlu.partUniqueId[i]).UnitInfo;
			GameObject go = NGUITools.AddChild(parent, materilItem);
			go.SetActive(true);
			UISprite sprite = go.transform.Find("Avatar").GetComponent<UISprite>();
			DataCenter.Instance.GetAvatarAtlas(tui.ID, sprite);
			go.transform.Find("Background").GetComponent<UISprite>().spriteName = DGTools.GetItemBackgroundName(tui.Type);
			go.transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = DGTools.GetItemBorderName(tui.Type);
			material.Enqueue(go);
			
			DataCenter.Instance.UserUnitList.DelMyUnit (rlu.partUniqueId[i]);
		}

		parent.GetComponent<UIGrid> ().Reposition ();
		count = material.Count;
		newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile, o =>{
			AudioManager.Instance.PlayAudio(AudioEnum.sound_check_role);
			DGTools.ShowTexture (unitBodyTex, o as Texture2D);
			Vector3 localposition = unitBodyTex.transform.localPosition;
			targetPosition = new Vector3(localposition.x, localposition.y + unitBodyTex.height * 0.5f, localposition.z) - parent.transform.localPosition; //unitBodyTex.transform.localPosition + Vector3.up * (unitBodyTex.height * 0.5f);
			ShowUnitScale();
			SetEffectCamera();
			StartCoroutine(SwallowUserUnit());
		});
	}

	void ShowLevelupInfo(object data) {
		ShowLevelInfo (newBlendUnit);
		curLevel = oldBlendUnit.Level;
		gotExp = levelUpData.blendExp;
		
		levelDone = gotExp > 0;
		
		curExp = oldBlendUnit.CurExp;
		Calculate ();
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;
	
	private TUserUnit curUserUnit;
//	private void CallBackUnitData(object d){
//		TUserUnit userUnit = d as TUserUnit;
//
//		if (userUnit != null) {
//			ShowInfo (userUnit);
//		} else {
//			RspLevelUp rlu = d as RspLevelUp;
//			if(rlu ==null) {
//				return;
//			}
//			ShowLevelupInfo(rlu);
//			PlayLevelUp(rlu);
//		}
//	}



	bool levelDone = false;

	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (15f, 0f, 0f);
		camera.orthographicSize = 1.3f;
	}

	
	//------------------end-----------------------------------------
	void ShowInfo(TUserUnit userUnit) {
//		ShowFavView(curUserUnit.IsFavorite);
		ShowBodyTexture( userUnit ); 
		ShowUnitScale();
		ShowTopPanel (userUnit);

		ShowStatusContent( userUnit );
		ShowSkill1Content( userUnit );
		ShowSkill2Content( userUnit );
		ShowLeaderSkillContent( userUnit );
		ShowActiveSkillContent( userUnit );
		ShowProfileContent( userUnit );

	}

	void ShowLevelInfo (TUserUnit userUnit) {
		ShowTopPanel (userUnit);
		ShowStatusContent( userUnit );
		ShowSkill1Content( userUnit );
		ShowSkill2Content( userUnit );
		ShowLeaderSkillContent( userUnit );
		ShowActiveSkillContent( userUnit );
		ShowProfileContent( userUnit );
	}
        
	void ShowTopPanel(TUserUnit data){

		TUnitInfo unitInfo = data.UnitInfo;
		number.text = data.UnitID.ToString();
		
		name.text = TextCenter.GetText ("UnitName_" + unitInfo.ID);//unitInfo.Name;
		
		type.spriteName = "type_" + unitInfo.UnitType;
		
		cost.text = unitInfo.Cost.ToString();
		
		//Debug.Log ("rare : " + unitInfo.Rare + "max rare: " + unitInfo.MaxRare);	
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
		//Debug.Log ("position:  " +len * 15  );
		grayStar.transform.localPosition = new Vector3(len * 15,-82,0);
	}

	
	void ShowUnitScale(){
		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();
		
		unitAlpha.eventReceiver = this.gameObject;
		unitAlpha.callWhenFinished = "PlayCheckRoleAudio";
		
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
		});
		
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

		if(gotExp < expRiseStep){
			curExp += gotExp;
			AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
			gotExp = 0;
		} else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
			AudioManager.Instance.PlayAudio(AudioEnum.sound_get_exp);
		}

		if(curExp >= currMaxExp) {
			gotExp += curExp - currMaxExp;
			curExp = 0;
			if ( curLevel < oldBlendUnit.UnitInfo.MaxLevel ){
				curLevel ++;

				AudioManager.Instance.PlayAudio(AudioEnum.sound_level_up);
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
			levelLabel.text = oldBlendUnit.UnitInfo.MaxLevel.ToString();
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

	
	private void InitTextLabel(){
		hpTextLabel.text = TextCenter.GetText("Text_HP");
		atkTextLabel.text = TextCenter.GetText("Text_ATK");
		statusTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Status");
		normalSkillTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Normal_Skill");
		profileTextLabel.text = TextCenter.GetText("Unit_Detail_Tab_Prifile");
		raceTextLabel.text = TextCenter.GetText ("Text_RACE");
	}

	private void ShowFavState(object msg){
		UpdateFavView(curUserUnit.IsFavorite);
	}

	private void ClickLock(GameObject go){
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
			background.spriteName = "lock_closed";
			Debug.Log("UpdateFavView(), isFav == 1, background.spriteName is Fav_Lock_Close");
		}
		else{
			background.spriteName = "lock_open";
			Debug.Log("UpdateFavView(), isFav != 1, background.spriteName is Fav_Lock_Open");
		}
	}

	//center
	Vector3 targetPosition;
	
	Queue<GameObject> material = new Queue<GameObject> ();
	int count = 0;
	
	public void RecoverEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		camera.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		camera.orthographicSize = 1f;
	}
	
	List<GameObject> effectCache = new List<GameObject>();
	GameObject levelUpEffect = null;
	GameObject swallowEffect = null;
	GameObject linhunqiuEffect = null;
	GameObject evolveEffect = null;
	
	void InitEffect(){
		string path = "";

		if (levelUpEffect == null) {
			path = "Effect/effect/LevelUpEffect";
			ResourceManager.Instance.LoadLocalAsset( path , o =>{
				levelUpEffect = o as GameObject;
			});	
		}
	
		if (swallowEffect == null) {
			path = "Effect/effect/level_up01";
			ResourceManager.Instance.LoadLocalAsset( path , o =>{
				swallowEffect = o as GameObject;
			});
		}

		if (linhunqiuEffect == null) {
			path = "Effect/effect/linhunqiu1";
			ResourceManager.Instance.LoadLocalAsset( path , o =>{
				linhunqiuEffect = o as GameObject;
			});	
		}

		if (evolveEffect == null) {
			path = "Effect/effect/evolve";
			ResourceManager.Instance.LoadLocalAsset( path , o =>{
				evolveEffect = o as GameObject;
			});	
		}
	}
	
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
			GameObject linhunqiuIns = NGUITools.AddChild(parent, linhunqiuEffect);
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
		
		StartCoroutine (CreatEffect ());
	}
	
	IEnumerator CreatEffect() {
		yield return new WaitForSeconds(0.5f);
		GameObject go = Instantiate (levelUpEffect) as GameObject;
		effectCache.Add (go);
//		yield return new WaitForSeconds(0.1f);
//		go = Instantiate (levelUpEffect) as GameObject;
//		effectCache.Add (go);
		
		if (effectCache.Count == count) {
			yield return new WaitForSeconds(2f);
			
//			ClearEffectCache ();
//			HideUI();
			UIManager.Instance.ChangeScene( UIManager.Instance.baseScene.PrevScene );
			
			ShowLevelInfo (newBlendUnit);
			curLevel = oldBlendUnit.Level;
			gotExp = levelUpData.blendExp;
			
			levelDone = gotExp > 0;
			
			curExp = oldBlendUnit.CurExp;
			Calculate ();
			
			RecoverEffectCamera();
		} else {
			yield return new WaitForSeconds(1.5f);
			StartCoroutine (CreatEffect ());
		}
	}
	
	void ClearEffectCache(){

		
		DGTools.SafeDestory (materilUse);
		DGTools.SafeDestory (linhunqiuIns);
		DGTools.SafeDestory (swallowEffectIns);
		DGTools.SafeDestory (evolveEffectIns);

		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}

//		gameObject.SetActive (false);
//		HideUI ();
	}
}
