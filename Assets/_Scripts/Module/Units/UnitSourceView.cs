using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitSourceView : ViewBase {

	private DragPanel dragPanel;
	private UnitInfo unitInfo;

	private UISprite avatar;
	private UISprite avatarBg;
	private UISprite avatarBorder;
	private UILabel nameLabel;	

	private GameObject unitIcon;

	public override void Init (UIConfigItem uiconfig, System.Collections.Generic.Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

		avatar = FindChild<UISprite> ("Unit/UnitIcon/Avatar");
		avatarBg = FindChild<UISprite> ("Unit/UnitIcon/Background");
		avatarBorder = FindChild<UISprite> ("Unit/UnitIcon/Sprite_Avatar_Border");
		nameLabel = FindChild<UILabel> ("Unit/Name");
		unitIcon = FindChild("Unit/UnitIcon");
		FindChild<UILabel> ("Unit/Label").text = TextCenter.GetText ("UnitSource_GetWay");

		UIEventListenerCustom.Get (unitIcon).onClick = OnDetail;

		dragPanel = new DragPanel ("UnitSourceDragPanel", "Prefabs/UI/Units/UnitSourceItem", typeof(UnitSourceItemView), transform);
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		if (viewData != null && viewData.ContainsKey ("unit")) {
			unitInfo = viewData["unit"] as UnitInfo;
			ShowInfo();
			dragPanel.SetData<UnitGetWay>(unitInfo.getWay);
//			
		}
	}

	public override void HideUI ()
	{
		base.HideUI ();
	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	}

	private void ShowInfo(){
		ShowUnitType ();
		nameLabel.text = TextCenter.GetText ("UnitName_" + unitInfo.id);
		ResourceManager.Instance.GetAvatarAtlas(unitInfo.id, avatar);
	}

	private void OnDetail(GameObject obj){
		UserUnit u = new UserUnit ();
		u.unitId = unitInfo.id;
		u.level = 1;
		u.userID = 0;
		ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"user_unit",u);
	}

	private void ShowUnitType(){
		switch (unitInfo.type){
		case EUnitType.UFIRE :
			avatarBg.spriteName = "avatar_bg_fire";
			avatarBorder.spriteName = "avatar_border_fire";
			break;
		case EUnitType.UWATER :
			avatarBg.spriteName = "avatar_bg_water";
			avatarBorder.spriteName = "avatar_border_water";
			
			break;
		case EUnitType.UWIND :
			avatarBg.spriteName = "avatar_bg_wind";
			avatarBorder.spriteName = "avatar_border_wind";
			
			break;
		case EUnitType.ULIGHT :
			avatarBg.spriteName = "avatar_bg_light";
			avatarBorder.spriteName = "avatar_border_light";
			
			break;
		case EUnitType.UDARK :
			avatarBg.spriteName = "avatar_bg_dark";
			avatarBorder.spriteName = "avatar_border_dark";
			
			break;
		case EUnitType.UNONE :
			avatarBg.spriteName = "avatar_bg_none";
			avatarBorder.spriteName = "avatar_border_none";
			
			break;
		default:
			break;
		}
	}
}
