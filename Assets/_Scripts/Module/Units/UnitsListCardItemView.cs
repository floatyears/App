using UnityEngine;
using System.Collections;
using bbproto;

public class UnitsListCardItemView : DragPanelItemBase {

	private UILabel atkLabel;
	private UILabel hpLabel;
	private MyUnitItem icon;
	private UISprite lightStar;
	private UISprite darkStar;
	private UILabel name;

	private UserUnit data;

	private GameObject evolveBtn;
	private GameObject levelUpBtn;
	private UILabel levelupLabel;

	void Init(){
		atkLabel = transform.FindChild ("LabelAtk").GetComponent<UILabel>();
		hpLabel = transform.FindChild ("LabelHp").GetComponent<UILabel> ();
		name = transform.FindChild ("Name").GetComponent<UILabel> ();
		lightStar = transform.FindChild ("Star").GetComponent<UISprite> ();
		darkStar = transform.FindChild ("Star/Dark").GetComponent<UISprite> ();

		icon = transform.FindChild ("UnitIcon").GetComponent<MyUnitItem> ();

		levelUpBtn = transform.FindChild ("LevelUp").gameObject;
		evolveBtn = transform.FindChild ("Evolve").gameObject;

		transform.FindChild ("Evolve/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_SuperEvolve");
		levelupLabel = transform.FindChild ("LevelUp/Label").GetComponent<UILabel> ();

		UIEventListenerCustom.Get (transform.FindChild ("LevelUp").gameObject).onClick = ClickLevelUp;
		UIEventListenerCustom.Get (transform.FindChild ("Evolve").gameObject).onClick = ClickSuperEvolve;

		UIEventListenerCustom.Get (gameObject).LongPress = PressItem;
	}

	public override void SetData<T> (T d, params object[] args)
	{
		if (atkLabel == null)
			Init ();

		icon.SetData<T> (d,args);

		UIEventListenerCustom.Get (icon.gameObject).LongPress = null;
		UIEventListenerCustom.Get (icon.gameObject).onClick = ClickCard;
		this.data = d as UserUnit;

		if (data.unitId == 1 || data.unitId == 5 || data.unitId == 9) {
			levelUpBtn.tag = "unit_leader_levelup";
		}else{
			levelUpBtn.tag = "Untagged";
		}
		if (data.level >= data.UnitInfo.maxLevel) {
			if(data.UnitInfo.evolveInfo == null){
				levelUpBtn.SetActive(false);
			}else{
				levelUpBtn.SetActive(true);
				levelupLabel.text = TextCenter.GetText ("Btn_Submit_Evolve");
			}
		} else {
			levelUpBtn.SetActive(true);
			levelupLabel.text = TextCenter.GetText ("Btn_Submit_LevelUp");
		}
		lightStar.width = data.UnitInfo.rare*29;

		if (data.UnitInfo.maxStar > data.UnitInfo.rare) {
			darkStar.enabled = true;
			darkStar.width = (data.UnitInfo.maxStar - data.UnitInfo.rare) * 28;
			darkStar.transform.localPosition = new Vector3(lightStar.width,0,0);
		} else {
			darkStar.enabled = false;
		}

		name.text = data.UnitInfo.name;
		atkLabel.text = data.Attack + "";
		hpLabel.text = data.Hp + "";
	}

	public override void ItemCallback (params object[] args)
	{

	}

	void ClickLevelUp(GameObject obj){
#if !UNITY_EDITOR
		if (DataCenter.Instance.QuestData.QuestClearInfo.GetNewestStage(ECopyType.CT_NORMAL) < 15) {

			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Open_Limit"),TextCenter.GetText("Open_LevelUp"),TextCenter.GetText("OK"),o=>{
//				ModuleManager.Instance.HideModule (ModuleEnum.UnitsListModule);
				ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)1,"tips",4);
			});
			ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"story",(uint)1,"tips",4);
			return;
		}
#endif
		ModuleManager.Instance.HideModule (ModuleEnum.UnitsListModule);
		if (data.level >= data.UnitInfo.maxLevel) {
			ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "evolve",data);
		}else{
			ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "level_up",data);
		}

	}

	void ClickSuperEvolve(GameObject obj){
		TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("FunctionNotOpenTitle"),TextCenter.GetText("FunctionNotOpenContent"),TextCenter.GetText("OK"));
	}

	void PressItem(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "user_unit", data);
	}

	void ClickCard(object data){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "user_unit", this.data);
	}
}
