using UnityEngine;
using System.Collections;

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

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		base.Init (config, origin);
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUnitDetailInfo();
		ShowUnitScale();
		UIManager.Instance.HideBaseScene();
		TabFocus();

	}
	
	public override void HideUI ()  {
		base.HideUI ();
		UIManager.Instance.ShowBaseScene();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		detaiSprite = FindChild< UITexture >("detailSprite");
		detailWidget = FindChild< UIWidget >("detailSprite");
		UIEventListener.Get( detaiSprite.gameObject ).onClick = BackPreScene;
		string path = "UnitInfoTabs/Content_Status/";
		unitIDLabel = FindChild<UILabel> (path + "InputFrame_No");
		unitNameLabel = FindChild<UILabel> (path + "InputFrame_Name");
		unitLevelLabel = FindChild<UILabel> (path + "InputFrame_Lv");
		unitTypeLabel = FindChild<UILabel> (path + "InputFrame_Type");
		unitRaceLabel = FindChild<UILabel> (path + "InputFrame_Race");
		unitHpLabel = FindChild<UILabel> (path + "InputFrame_HP");
		unitCostLabel = FindChild<UILabel> (path + "InputFrame_Cost");
		unitRareLabel = FindChild<UILabel> (path + "InputFrame_Rare");
		unitAttackLabel = FindChild<UILabel> (path + "InputFrame_ATK");
		unitExpSlider = FindChild<UISlider> (path + "ExperenceBar");

		startToggle = FindChild<UIToggle>("UnitInfoTabs/Tab_Status");
	}
	
	private void TabFocus() {
		startToggle.value = true;
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
	}

	private void BackPreScene( GameObject go )
	{
		SceneEnum preScene = UIManager.Instance.baseScene.PrevScene;

		UIManager.Instance.ChangeScene( preScene );
	}

	private void ShowUnitScale()
	{
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
