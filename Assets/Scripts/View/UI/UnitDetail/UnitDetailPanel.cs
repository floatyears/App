using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailPanel : UIComponentUnity,IUICallback{
//	UIButton favBtn;
	GameObject unitInfoTabs;
//	UILabel noLabel;
	UILabel hpLabel;
	UILabel atkLabel;
	UILabel raceLabel;
//	UILabel costLabel;
//	UILabel rareLabel;
	UILabel levelLabel;
//	UILabel typeLabel;
//	UILabel nameLabel;
	UILabel needExpLabel;
	UISlider expSlider;

	UILabel normalSkill1DscpLabel;
	UILabel normalSkill1NameLabel;
	
	UILabel normalSkill2DscpLabel;
	UILabel normalSkill2NameLabel;
//
//	UILabel leaderSkillNameLabel;
//	UILabel leaderSkillDscpLabel;
//
//	UILabel activeSkillNameLabel;
//	UILabel activeSkillDscpLabel;

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
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		UIManager.Instance.HideBaseScene();
		ResetStartToggle (statusToggle);
		ClearBlock( blockLsit1 );
		ClearBlock( blockLsit2 );

		//TODO:
		//StartCoroutine ("nextState");
		NoviceGuideStepEntityManager.Instance ().StartStep ();
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
	}

	void InitTabSkill(){
		string rootPath;
		// skill_1
		rootPath 				=  "UnitInfoTabs/Content_Skill1/Label_Vaule/";
//		leaderSkillNameLabel	= FindChild<UILabel>(rootPath + "Leader_Skill");
//		leaderSkillDscpLabel	= FindChild<UILabel>(rootPath + "Leader_Skill_Dscp");
//		activeSkillNameLabel	= FindChild<UILabel>(rootPath + "Active_Skill");
//		activeSkillDscpLabel	= FindChild<UILabel>(rootPath + "Active_Skill_Dscp");
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
		for (int i = effectCache.Count - 1; i >= 0 ; i--) {
			GameObject go = effectCache[i];
			Destroy( go );
			effectCache.Remove(go);
		}
	}

	void InitEffect(){
//		string path = "Prefabs/UI/UnitDetail/LevelUpEffect";
		string path = "Effect/HelixHealingYellow";
		levelUpEffect = Resources.Load( path ) as GameObject;
	}

	void ClickTexture( GameObject go ){
		if (fobidClick) {
			return;	
		}
		StopAllCoroutines ();
		ClearEffectCache ();
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
		Debug.LogError ("unit detail SceneEnum : " + preScene);
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

		//next level need
		if ((data.Level > unitInfo.MaxLevel ) 
		    || (data.Level == unitInfo.MaxLevel && data.NextExp <= 0) ) {
			levelLabel.text = unitInfo.MaxLevel.ToString();
			needExpLabel.text = "Max";
			expSlider.value = 1f;
		} else {
			needExpLabel.text = "Next: " + data.NextExp.ToString();
			expSlider.value = data.CurExp*1.0f / (data.CurExp + data.NextExp);
		}
	}


	void ShowSkill1Content( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.NormalSkill1;
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill); //Skill[ skillId ];
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
		if (skillId == 0) {
			return;	
		}
		SkillBaseInfo sbi = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill);//Skill[ skillId ];
		SkillBase skill = sbi.GetSkillInfo();
                
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
		if (skillId == 0) {
			return;	
		}
		SkillBase skill = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill).GetSkillInfo();
//        leaderSkillNameLabel.text = skill.name;
//		leaderSkillDscpLabel.text = skill.description;
	}

	void ShowActiveSkillContent( TUserUnit data){
		TUnitInfo unitInfo = data.UnitInfo;
		int skillId = unitInfo.ActiveSkill;
		if (skillId == 0) {
			return;	
		} 
		SkillBase skill = DataCenter.Instance.GetSkill (data.MakeUserUnitKey (), skillId, SkillType.NormalSkill).GetSkillInfo();
//		activeSkillNameLabel.text = skill.name;
//		activeSkillDscpLabel.text = skill.description;
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

		if (userUnit != null) {
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
		Debug.LogError ("unitBodyTex : " + unitBodyTex + " newBlendUnit : " + newBlendUnit + " newBlendUnit.UnitInfo : " + newBlendUnit.UnitInfo);
		DGTools.ShowTexture (unitBodyTex, newBlendUnit.UnitInfo.GetAsset (UnitAssetType.Profile));
//		ShowUnitScale();
		unitInfoTabs.SetActive (false);
		SetEffectCamera ();
		StartCoroutine (CreatEffect ());
	}

	TUserUnit oldBlendUnit = null;
	TUserUnit newBlendUnit = null;

	bool levelDone = false;

	public void SetEffectCamera() {
		Camera camera = Main.Instance.effectCamera;
		Debug.LogError ("camera : " + camera);
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
			unitInfoTabs.SetActive (true);
			ShowLevelInfo (newBlendUnit);
			curLevel = oldBlendUnit.Level;
			gotExp = levelUpData.blendExp;

			levelDone = gotExp > 0;

			curExp = oldBlendUnit.CurExp;
			Debug.LogError ("CreatEffect :: gotExp : " + gotExp);
			Debug.LogError ("CreatEffect :: level : " + newBlendUnit.Level);
			Debug.LogError ("CreatEffect :: CurExp : " + curExp);
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
		currMaxExp = oldBlendUnit.UnitInfo.GetExp(curLevel); 

		expRiseStep = (int)(currMaxExp * 0.01f);
		if ( expRiseStep < 1 )
			expRiseStep = 1;
//		Debug.LogError ("Calculate : " + currMaxExp + "  expRiseStep : " + expRiseStep);
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

		Debug.Log ("gotExp: " + gotExp + " expRiseStep: " + expRiseStep + " curExp: " + curExp + " currMaxExp: " + currMaxExp);

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

//		LogHelper.LogError(">>>>>>>>>currMaxExp:{0} curExp:{1} curLevel:{2} ",currMaxExp, curExp, curLevel);

		int needExp = currMaxExp - curExp;

		if ((curLevel > oldBlendUnit.UnitInfo.MaxLevel) 
		    || (curLevel == oldBlendUnit.UnitInfo.MaxLevel && needExp <= 0) ) {
			levelLabel.text = oldBlendUnit.UnitInfo.MaxLevel.ToString();
			needExpLabel.text = "Max";
			return;
		} else {
			needExpLabel.text = "Next: " + needExp.ToString();
		}

		float progress = (float)curExp / (float)currMaxExp;
		if (progress == 0) {
			progress = 0.1f;		
		}
		Debug.Log ("exp slide progress: " + progress);
		expSlider.value = progress;
	}

//	private void CollectCurUnit(GameObject go){
//		bool isFav = (curUserUnit.IsFavorite == 1) ? true : false;
//		EFavoriteAction favAction = isFav ? EFavoriteAction.DEL_FAVORITE : EFavoriteAction.ADD_FAVORITE;
//		UnitFavorite.SendRequest(OnRspChangeFavState, curUserUnit.ID, favAction);
//	}

//	private void OnRspChangeFavState(object data){
//		//Debug.Log("OnRspChangeFavState(), start...");
//		if(data == null) {Debug.LogError("OnRspChangeFavState(), data is NULL"); return;}
//		bbproto.RspUnitFavorite rsp = data as bbproto.RspUnitFavorite;
//		if (rsp.header.code != (int)ErrorCode.SUCCESS){
//			LogHelper.LogError("OnRspChangeFavState code:{0}, error:{1}", rsp.header.code, rsp.header.error);
//			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
//
//			return;
//		}
//		curUserUnit.IsFavorite = (curUserUnit.IsFavorite==1) ? 0 : 1;
////		Debug.LogError ("curUserUnit : " + curUserUnit.TUserUnitID);
//		ShowFavView(curUserUnit.IsFavorite);
//	}


//	private void ShowFavView(int isFav){
//		UISprite background = favBtn.transform.FindChild("Background").GetComponent<UISprite>();
//		//Debug.Log("Name is : " + curUserUnit.UnitInfo.Name + "  UpdateFavView(), isFav : " + (isFav == 1));
//		if(isFav == 1){
//			background.spriteName = "Fav_Lock_Close";
//			background.spriteName = "Fav_Lock_Close";
//			background.spriteName = "Fav_Lock_Close";
//			//Debug.Log("UpdateFavView(), isFav == 1, background.spriteName is Fav_Lock_Close");
//		}
//		else{
//			background.spriteName = "Fav_Lock_Open";
//			background.spriteName = "Fav_Lock_Open";
//			background.spriteName = "Fav_Lock_Open";
//			//Debug.Log("UpdateFavView(), isFav != 1, background.spriteName is Fav_Lock_Open");
//		}
//	}
}
