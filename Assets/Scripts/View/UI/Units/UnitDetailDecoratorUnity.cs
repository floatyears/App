using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailDecoratorUnity : UIComponentUnity{

	//-----------------------------------
	//content status
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

	//content skill_1
	UILabel skill1DscpLabel;
	UILabel skill1NameLabel;
	
	//content skill_2
	UILabel skill2DscpLabel;
	UILabel skill2NameLabel;
	
	//content profile
	UILabel profileLabel;

	//Tabs Ctrl
	UIToggle startToggle;
	
	UITexture unitBodyTex;

	//Effect
	GameObject levelUpEffect;
	Material unitMaterial;

	//Exp Slider
	int maxExp;
	int curExp;
	int gotExp;
	int expRiseStep;
	//-----------------------------------

	
	//-----------------------------------
	//Init Function

	void InitTexture(){
		unitBodyTex = FindChild< UITexture >("detailSprite");
		UIEventListener.Get( unitBodyTex.gameObject ).onClick = ClickTexture;
	}

	void InitTabContent() {
		string rootPath = "UnitInfoTabs/Content_Status/";

		idLabel = FindChild<UILabel> (rootPath + "InputFrame_No");
		nameLabel = FindChild<UILabel> (rootPath + "InputFrame_Name");
		levelLabel = FindChild<UILabel> (rootPath + "InputFrame_Lv");
		typeLabel = FindChild<UILabel> (rootPath + "InputFrame_Type");
		raceLabel = FindChild<UILabel> (rootPath + "InputFrame_Race");
		hpLabel = FindChild<UILabel> (rootPath + "InputFrame_HP");
		costLabel = FindChild<UILabel> (rootPath + "InputFrame_Cost");
		rareLabel = FindChild<UILabel> (rootPath + "InputFrame_Rare");
		atkLabel = FindChild<UILabel> (rootPath + "InputFrame_ATK");
		needExpLabel = FindChild< UILabel >( rootPath + "Label_Next_Lv_Vaule");
		expSlider	= FindChild<UISlider>	(rootPath + "ExperenceBar");
	}


	void GetUnitMaterial(){
		unitMaterial = Resources.Load("Materials/UnitMaterial") as Material;
		if( unitMaterial == null )
			Debug.LogError("Scene -> UnitDetail : Not Find UnitMaterial");
	}

	List<GameObject> effectCache = new List<GameObject>();

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		GetUnitMaterial();
		InitEffect();
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUnitDetailInfo();
		ShowUnitScale();
		UIManager.Instance.HideBaseScene();
		TabFocus();
		MsgCenter.Instance.AddListener(CommandEnum.LevelUp , LevelUpUnit);
		MsgCenter.Instance.AddListener(CommandEnum.ShowUnitDetail, ShowUnitDetail);
	}

	void ShowUnitDetail( object info ){
		UserUnit userUnitInfo = info as UserUnit;
		uint curId = userUnitInfo.unitId;
		unitBodyTex.mainTexture = GlobalData.tempUnitInfo[ curId ].GetAsset( UnitAssetType.Profile);
		idLabel.text = curId.ToString();
		nameLabel.text = GlobalData.tempUnitInfo[ curId ].GetName();
		levelLabel.text = userUnitInfo.level.ToString();
		typeLabel.text = GlobalData.tempUnitInfo[ curId ].GetUnitType();

		TUnitInfo tu = GlobalData.tempUnitInfo[ curId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),userUnitInfo.level);
		hpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), userUnitInfo.level);
		atkLabel.text = atk.ToString();

		int cost = GlobalData.tempUnitInfo[ curId ].GetCost();
		costLabel.text = cost.ToString();

		int rare = GlobalData.tempUnitInfo[ curId ].GetRare();
		rareLabel.text = rare.ToString();

		raceLabel.text = "Human";

	}


	public override void HideUI ()  {
		base.HideUI ();
		ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp , LevelUpUnit);
		MsgCenter.Instance.RemoveListener(CommandEnum.ShowUnitDetail, ShowUnitDetail);

	}

	void LevelUpUnit( object Info){
		List<UserUnit> packageInfo = Info as List<UserUnit>;
		uint curUnitId = packageInfo[0].unitId;

		GameObject tempEffect = Instantiate( levelUpEffect ) as GameObject;
		GameObject profile = tempEffect.transform.FindChild("ProfileTexture").gameObject;
		if(profile == null)
			Debug.LogError("Profile is not found!");
		MeshRenderer meshRender = profile.GetComponent< MeshRenderer >();
		Texture tex = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Profile);

		meshRender.materials[ 0 ].SetTexture("_MainTex", tex);
		effectCache.Add( tempEffect );

		idLabel.text = curUnitId.ToString();
		nameLabel.text = GlobalData.tempUnitInfo[ curUnitId ].GetName();
		levelLabel.text = packageInfo[0].level.ToString();
		typeLabel.text = GlobalData.tempUnitInfo[ curUnitId ].GetUnitType();
		
		TUnitInfo tu = GlobalData.tempUnitInfo[ curUnitId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),packageInfo[0].level);
		hpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), packageInfo[0].level);
		atkLabel.text = atk.ToString();
		
		int cost = GlobalData.tempUnitInfo[ curUnitId ].GetCost();
		costLabel.text = cost.ToString();
		
		int rare = GlobalData.tempUnitInfo[ curUnitId ].GetRare();
		rareLabel.text = rare.ToString();

		raceLabel.text = "Human";

		ExpRise();
	}

	void ClearEffectCache(){
		foreach (var item in effectCache){
			Destroy( item );
		}
		effectCache.Clear();
	}

	void InitEffect(){
		levelUpEffect = Resources.Load("Prefabs/UI/UnitDetail/LevelUpEffect") as GameObject;
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		InitTabContent();
		InitExpSlider();

		startToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");

		string path2 =  "UnitInfoTabs/Content_Skill2/";
		skill1NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill1");
		skill2NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill2");
	}
	
	private void TabFocus() {
		startToggle.value = true;
	}

	private void ShowUnitDetailInfo() {
		unitBodyTex.mainTexture = Resources.Load(ShowUnitInfo.roleSpriteName) as Texture2D;
		idLabel.text 	= ShowUnitInfo.unitID.ToString();
		nameLabel.text = ShowUnitInfo.unitName;
		levelLabel.text = ShowUnitInfo.level.ToString ();
		typeLabel.text = ShowUnitInfo.unitType;
		raceLabel.text = ShowUnitInfo.race;
		hpLabel.text = ShowUnitInfo.hp.ToString();
		costLabel.text = ShowUnitInfo.cost.ToString();
		rareLabel.text = ShowUnitInfo.Rare;
		atkLabel.text = ShowUnitInfo.attack.ToString ();
		expSlider.value = ShowUnitInfo.experenceProgress;

		skill1NameLabel.text = ShowUnitInfo.normalSkill_1_Name;
		skill2NameLabel.text = ShowUnitInfo.normalSkill_2_Name;
	}

	void ClickTexture( GameObject go ){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_ui_back );
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;
		UIManager.Instance.ChangeScene( preScene );
	}

	private void ShowUnitScale(){
		TweenScale unitScale = gameObject.GetComponentInChildren< TweenScale >();
		TweenAlpha unitAlpha = gameObject.GetComponentInChildren< TweenAlpha >();

		if( unitScale == null || unitAlpha == null )
			return;

		unitScale.Reset();
		unitScale.PlayForward();

		unitAlpha.Reset();
		unitAlpha.PlayForward();
	}



	void OnEnable(){

	}

	void InitExpSlider(){
		curExp = 460;
		gotExp = 3000;
		maxExp = 1000;
		expRiseStep = maxExp / 120;
	}

	void Update(){
		ExpRise();
	}

	void ExpRise () {
		if(gotExp <= 0)	
			return;

		if(gotExp < expRiseStep){
			//remain less than step, add remain
			curExp += gotExp;
			gotExp = 0;
		} else {
			gotExp -= expRiseStep;
			curExp += expRiseStep;
		}

		if(curExp >= maxExp) {
			//current overflow,add back
			gotExp += curExp - maxExp;

			curExp = 0;
		}

		int needExp = maxExp - curExp;
		needExpLabel.text = needExp.ToString();
		float progress = (float)curExp / (float)maxExp;
		expSlider.value = progress;
	}

}
