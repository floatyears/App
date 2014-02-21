using UnityEngine;
using System.Collections;
using bbproto;
using System.Collections.Generic;

public class UnitDetailDecoratorUnity : UIComponentUnity{
	private UITexture detaiSprite;
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
	GameObject levelUpEffect;

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
	}
	
	public override void HideUI ()  {
		base.HideUI ();
		ClearEffectCache();
		UIManager.Instance.ShowBaseScene();
		MsgCenter.Instance.RemoveListener(CommandEnum.LevelUp , LevelUpUnit);

	}
	void ShowUnitInfoDetail(object info ){
		UnitInfo unitInfo = info as UnitInfo;
		string path = "Role/role00" + unitInfo.id.ToString();
		detaiSprite.mainTexture = Resources.Load(path) as Texture2D;
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
		Debug.LogError(tex.name);
		meshRender.materials[ 0 ].SetTexture("_MainTex", tex);
//		meshRender.materials[ 0 ] = unitMaterial;
//		unitMaterial.mainTexture = GlobalData.tempUnitInfo[ curUnitId ].GetAsset(UnitAssetType.Profile);
//
		effectCache.Add( tempEffect );
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
	void ShowEffect(){

	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		detaiSprite = FindChild< UITexture >("detailSprite");
		detailWidget = FindChild< UIWidget >("detailSprite");
		UIEventListener.Get( detaiSprite.gameObject ).onClick = BackPreScene;
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

		startToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");

		string path2 =  "UnitInfoTabs/Content_Skill2/";
		unitNormalSkill_1_NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill1");
		unitNormalSkill_2_NameLabel = FindChild< UILabel >( path2 + "Label_Normal_Skill2");
	}
	
	private void TabFocus() {
		startToggle.value = true;
	}

	private void ShowUnitDetail( object info){
		UnitInfo unitInfo = info as UnitInfo;
		if( unitInfo == null )	return;
		Debug.Log( string.Format("Catch Info, to show unit detail which named as {0}", unitInfo.name));
	}

	private void ShowUnitDetailInfo() {
		detaiSprite.mainTexture = Resources.Load(ShowUnitInfo.roleSpriteName) as Texture2D;
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
		//Debug.Log("unitNormalSkill_1_Name : "+unitNormalSkill_1_NameLabel.text);

		unitNormalSkill_2_NameLabel.text = ShowUnitInfo.normalSkill_2_Name;
		//Debug.Log("unitNormalSkill_2_Name : "+unitNormalSkill_2_NameLabel.text);
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

}
