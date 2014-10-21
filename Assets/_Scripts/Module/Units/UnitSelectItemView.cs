using UnityEngine;
using System.Collections;
using bbproto;

public class UnitSelectItemView : DragPanelItemBase {
	
	private UILabel atkLabel;
	private UILabel hpLabel;
	private MyUnitItem icon;
	private UISprite lightStar;
	private UISprite darkStar;
	private UILabel name;
	
	private UserUnit data;

	private DataListener callback;
	
	void Init(){
		atkLabel = transform.FindChild ("LabelAtk").GetComponent<UILabel>();
		hpLabel = transform.FindChild ("LabelHp").GetComponent<UILabel> ();
		name = transform.FindChild ("Name").GetComponent<UILabel> ();
		lightStar = transform.FindChild ("Star").GetComponent<UISprite> ();
		darkStar = transform.FindChild ("Star/Dark").GetComponent<UISprite> ();
		
		icon = transform.FindChild ("UnitIcon").GetComponent<MyUnitItem> ();
		
		
		transform.FindChild ("SelectBtn/Label").GetComponent<UILabel> ().text = TextCenter.GetText ("Text_Select");

		UIEventListenerCustom.Get (transform.FindChild ("SelectBtn").gameObject).onClick = SelectItem;
		UIEventListenerCustom.Get (icon.gameObject).onClick = ClickCard;
		UIEventListenerCustom.Get (gameObject).LongPress = PressItem;
	}
	
	public override void SetData<T> (T d, params object[] args)
	{
		if (atkLabel == null)
			Init ();
		
		icon.SetData<T> (d,args);
		UIEventListenerCustom.Get (icon.gameObject).LongPress = null;

		if (args.Length > 0) {
			callback = args[0] as DataListener;

		}

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
	
	void SelectItem(GameObject obj){
		if (callback != null) {
			callback(data);	
		}
	}

	
	void PressItem(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "unit", data);
	}
	
	void ClickCard(GameObject data){
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule, "unit", data);
	}
}