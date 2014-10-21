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

	void Init(){
		atkLabel = transform.FindChild ("LabelAtk").GetComponent<UILabel>();
		hpLabel = transform.FindChild ("LabelHp").GetComponent<UILabel> ();
		name = transform.FindChild ("Name").GetComponent<UILabel> ();
		lightStar = transform.FindChild ("Star").GetComponent<UISprite> ();
		darkStar = transform.FindChild ("Star/Dark").GetComponent<UISprite> ();

		icon = transform.FindChild ("UnitIcon").GetComponent<MyUnitItem> ();


		transform.FindChild ("Evolve/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Submit_Evolve");
		transform.FindChild ("LevelUp/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Btn_Submit_LevelUp");

		UIEventListenerCustom.Get (transform.FindChild ("LevelUp").gameObject).onClick = ClickLevelUp;
		UIEventListenerCustom.Get (transform.FindChild ("Evolve").gameObject).onClick = ClickEvolve;

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

		int len = 0;
		if (data.UnitInfo.maxStar > data.UnitInfo.rare) {
			darkStar.enabled = true;
			darkStar.width = (data.UnitInfo.maxStar - data.UnitInfo.rare) * 28;
			len = 2*data.UnitInfo.rare - data.UnitInfo.maxStar;
		} else {
			darkStar.enabled = false;
			len = data.UnitInfo.rare;
		}
		lightStar.width = data.UnitInfo.rare*29;

		name.text = data.UnitInfo.name;
		atkLabel.text = data.Attack + "";
		hpLabel.text = data.Hp + "";
	}

	public override void ItemCallback (params object[] args)
	{

	}

	void ClickLevelUp(GameObject obj){
		ModuleManager.Instance.HideModule (ModuleEnum.UnitsListModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "level_up",data);
	}

	void ClickEvolve(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitLevelupAndEvolveModule, "evolve",data);
	}

	void PressItem(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "unit", data);
	}

	void ClickCard(object data){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "unit", this.data);
	}
}
