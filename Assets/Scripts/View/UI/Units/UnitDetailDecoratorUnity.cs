using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailDecoratorUnity : UIComponentUnity{
	GameObject levelUpEffect;
	
	private UITexture detailSprite;
	private UIWidget detailWidget;
	private UILabel 	unitIDLabel;
	private UILabel 	unitNameLabel;
	private UILabel 	unitLevelLabel;
	private UILabel 	unitTypeLabel;
	private UILabel 	unitRaceLabel;
	private UILabel 	unitHpLabel;
	private UILabel 	unitCostLabel;
	private UILabel 	unitRareLabel;
	private UILabel 	unitAttackLabel;
	private UISlider 	unitExpSlider;
	private UIToggle startToggle;
	private UILabel unitNormalSkill_1_NameLabel;
	private UILabel unitNormalSkill_2_NameLabel;
	UILabel nextLevelExpNeedLabel;

	Material unitMaterial;

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
		detailSprite.mainTexture = GlobalData.tempUnitInfo[ curId ].GetAsset( UnitAssetType.Profile);
		unitIDLabel.text = curId.ToString();
		unitNameLabel.text = GlobalData.tempUnitInfo[ curId ].GetName();
		unitLevelLabel.text = userUnitInfo.level.ToString();
		unitTypeLabel.text = GlobalData.tempUnitInfo[ curId ].GetUnitType();

		TempUnitInfo tu = GlobalData.tempUnitInfo[ curId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),userUnitInfo.level);
		unitHpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), userUnitInfo.level);
		unitAttackLabel.text = atk.ToString();

		int cost = GlobalData.tempUnitInfo[ curId ].GetCost();
		unitCostLabel.text = cost.ToString();

		int rare = GlobalData.tempUnitInfo[ curId ].GetRare();
		unitRareLabel.text = rare.ToString();

		unitRaceLabel.text = "Human";

	}

	void FindLabel(){
		string path = "UnitInfoTabs/Content_Status/Label_Next_Lv_Vaule";
		nextLevelExpNeedLabel = FindChild< UILabel >( path );
		string path1 = "UnitInfoTabs/Content_Status/";
		unitIDLabel = FindChild<UILabel> (path1 + "InputFrame_No");
		unitNameLabel = FindChild<UILabel> (path1 + "InputFrame_Name");
		unitLevelLabel = FindChild<UILabel> (path1 + "InputFrame_Lv");
		unitTypeLabel = FindChild<UILabel> (path1 + "InputFrame_Type");
		unitRaceLabel = FindChild<UILabel> (path1 + "InputFrame_Race");
		unitHpLabel = FindChild<UILabel> (path1 + "InputFrame_HP");
		unitCostLabel = FindChild<UILabel> (path1 + "InputFrame_Cost");
		unitRareLabel = FindChild<UILabel> (path1 + "InputFrame_Rare");
		unitAttackLabel = FindChild<UILabel> (path1 + "InputFrame_ATK");
		unitExpSlider = FindChild<UISlider> (path1 + "ExperenceBar");
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

		unitIDLabel.text = curUnitId.ToString();
		unitNameLabel.text = GlobalData.tempUnitInfo[ curUnitId ].GetName();
		unitLevelLabel.text = packageInfo[0].level.ToString();
		unitTypeLabel.text = GlobalData.tempUnitInfo[ curUnitId ].GetUnitType();
		
		TempUnitInfo tu = GlobalData.tempUnitInfo[ curUnitId ];
		int hp = GlobalData.Instance.GetUnitValue(tu.GetHPType(),packageInfo[0].level);
		unitHpLabel.text = hp.ToString();
		int atk = GlobalData.Instance.GetUnitValue(tu.GetAttackType(), packageInfo[0].level);
		unitAttackLabel.text = atk.ToString();
		
		int cost = GlobalData.tempUnitInfo[ curUnitId ].GetCost();
		unitCostLabel.text = cost.ToString();
		
		int rare = GlobalData.tempUnitInfo[ curUnitId ].GetRare();
		unitRareLabel.text = rare.ToString();

		unitRaceLabel.text = "Human";

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
		FindLabel();
		detailSprite = FindChild< UITexture >("detailSprite");
		detailWidget = FindChild< UIWidget >("detailSprite");
		UIEventListener.Get( detailSprite.gameObject ).onClick = BackPreScene;
	

		startToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");

		string path2 =  "UnitInfoTabs/Content_Skill2/";
		unitNormalSkill_1_NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill1");
		unitNormalSkill_2_NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill2");
	}
	
	private void TabFocus() {
		startToggle.value = true;
	}

	private void ShowUnitDetailInfo() {
		detailSprite.mainTexture = Resources.Load(ShowUnitInfo.roleSpriteName) as Texture2D;
		unitIDLabel.text 	= ShowUnitInfo.unitID.ToString();
		unitNameLabel.text = ShowUnitInfo.unitName;
		unitLevelLabel.text = ShowUnitInfo.level.ToString ();
		unitTypeLabel.text = ShowUnitInfo.unitType;
		unitRaceLabel.text = ShowUnitInfo.race;
		unitHpLabel.text = ShowUnitInfo.hp.ToString();
		unitCostLabel.text = ShowUnitInfo.cost.ToString();
		unitRareLabel.text = ShowUnitInfo.Rare;
		unitAttackLabel.text = ShowUnitInfo.attack.ToString ();
		unitExpSlider.value = ShowUnitInfo.experenceProgress;

		unitNormalSkill_1_NameLabel.text = ShowUnitInfo.normalSkill_1_Name;
		unitNormalSkill_2_NameLabel.text = ShowUnitInfo.normalSkill_2_Name;
	}

	private void BackPreScene( GameObject go ){
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

	int maxExp = 2080;
	int curExp = 1009;
	int gotExp = 3307;


	void Update(){
		ExpRise();
	}

	void ExpRise () {
		if(gotExp <= 0)	return;
		gotExp -= 19;
		curExp += 19;
		if(curExp >= maxExp) {
			curExp = 0;
		}
		int value = maxExp - curExp;
		nextLevelExpNeedLabel.text = value.ToString();
		float progress = (float)curExp / (float)maxExp;
		unitExpSlider.value = progress;
	}

}
