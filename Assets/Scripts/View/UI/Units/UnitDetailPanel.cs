using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailPanel : UIComponentUnity{
	//----------UI elements list----------
	UILabel idLabel;
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

	UILabel skill1DscpLabel;
	UILabel skill1NameLabel;
	
	UILabel skill2DscpLabel;
	UILabel skill2NameLabel;

	UILabel profileLabel;

	UIToggle statusToggle;
	UITexture unitBodyTex;

	GameObject levelUpEffect;
	Material unitMaterial;
	List<GameObject> effectCache = new List<GameObject>();

	int currMaxExp, curExp, gotExp, expRiseStep;

	
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
		AddMsgCmd ();
	}

	public override void HideUI () {

		base.HideUI ();

		ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
		RmvMsgCmd ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}


	//----------deal with message----------
	void AddMsgCmd () {
		MsgCenter.Instance.AddListener(CommandEnum.LevelUp , LevelUpUnit);
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, ShowUnitDetail);
	}

	void RmvMsgCmd () {
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp , LevelUpUnit);
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, ShowUnitDetail);
	}

	
	//----------Init functions of UI Elements----------
	void InitUI() {
		InitTabStatus ();
		InitExpSlider ();
		InitTexture ();
	}
	
	void InitTexture(){
		unitBodyTex = FindChild< UITexture >("detailSprite");
		UIEventListener.Get( unitBodyTex.gameObject ).onClick = ClickTexture;
	}

	void InitTabStatus() {
		string rootPath = "UnitInfoTabs/Content_Status/";

		idLabel		= FindChild<UILabel> (rootPath + "InputFrame_No"		);
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

		string rootPath =  "UnitInfoTabs/Content_Skill2/";

		// skill_1
		skill1NameLabel = FindChild< UILabel >( rootPath + "Label_Normal_Skill1");

		// skill_2
		skill2NameLabel = FindChild< UILabel >( rootPath + "Label_Normal_Skill2");

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
	
	void ShowUnitDetail( object info ){
		UserUnit userUnitInfo = info as UserUnit;
		uint curId = userUnitInfo.unitId;
		unitBodyTex.mainTexture = GlobalData.unitInfo[ curId ].GetAsset( UnitAssetType.Profile);
		idLabel.text = curId.ToString();
		nameLabel.text = GlobalData.unitInfo[ curId ].GetName();
		levelLabel.text = userUnitInfo.level.ToString();
		typeLabel.text = GlobalData.unitInfo[ curId ].GetUnitType();

		TUnitInfo tu = GlobalData.unitInfo[ curId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),userUnitInfo.level);
		hpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), userUnitInfo.level);
		atkLabel.text = atk.ToString();

		int cost = GlobalData.unitInfo[ curId ].GetCost();
		costLabel.text = cost.ToString();

		int rare = GlobalData.unitInfo[ curId ].GetRare();
		rareLabel.text = rare.ToString();

		raceLabel.text = "Human";

	}

	
	void LevelUpUnit( object Info){
		List<UserUnit> packageInfo = Info as List<UserUnit>;
		uint curUnitId = packageInfo[0].unitId;

		GameObject tempEffect = Instantiate( levelUpEffect ) as GameObject;
		GameObject profile = tempEffect.transform.FindChild("ProfileTexture").gameObject;
		if(profile == null)
			Debug.LogError("Profile is not found!");
		MeshRenderer meshRender = profile.GetComponent< MeshRenderer >();
		Texture tex = GlobalData.unitInfo[ curUnitId ].GetAsset(UnitAssetType.Profile);

		meshRender.materials[ 0 ].SetTexture("_MainTex", tex);
		effectCache.Add( tempEffect );

		idLabel.text = curUnitId.ToString();
		nameLabel.text = GlobalData.unitInfo[ curUnitId ].GetName();
		levelLabel.text = packageInfo[0].level.ToString();
		typeLabel.text = GlobalData.unitInfo[ curUnitId ].GetUnitType();
		
		TUnitInfo tu = GlobalData.unitInfo[ curUnitId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),packageInfo[0].level);
		hpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), packageInfo[0].level);
		atkLabel.text = atk.ToString();
		
		int cost = GlobalData.unitInfo[ curUnitId ].GetCost();
		costLabel.text = cost.ToString();
		
		int rare = GlobalData.unitInfo[ curUnitId ].GetRare();
		rareLabel.text = rare.ToString();

		raceLabel.text = "Human";

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
		

	int curLevel;
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
