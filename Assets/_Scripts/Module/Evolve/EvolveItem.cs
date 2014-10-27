using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveItem : MonoBehaviour{
//	public BoxCollider boxCollider;
	UnitInfo userUnit;
	UISprite showTexture;
	UILabel haveLabel;
	UISprite maskSprite;
	UISprite borderSprite;
	UISprite bgprite;

	public uint unitId;

	void Init () {
		showTexture = transform.Find("Texture").GetComponent<UISprite>();
		borderSprite = transform.Find("Sprite_Avatar_Border").GetComponent<UISprite>();
		bgprite = transform.Find("Sprite_Avatar_Bg").GetComponent<UISprite>(); 

		haveLabel = transform.Find ("HaveLabel").GetComponent<UILabel> ();
		maskSprite = transform.Find ("Mask").GetComponent<UISprite> ();
		UIEventListenerCustom.Get(gameObject).onClick = onClick;
	}

	public void RefreshData (uint unitId, int currCount, int totalCount) {
		if (haveLabel == null) {
			Init();	
		}

		this.unitId = unitId;
		if (unitId == 0) {
			showTexture.spriteName = "";
			borderSprite.spriteName = "unit_empty_bg";
			bgprite.spriteName = "";
			haveLabel.enabled = false;
			UIEventListenerCustom.Get(gameObject).LongPress = null;
		} else {
			userUnit = DataCenter.Instance.UnitData.GetUnitInfo(unitId);
			ShowUnitType();
			ShowShield (currCount, totalCount);

			ResourceManager.Instance.GetAvatarAtlas(unitId, showTexture);
		}
	}

	void onClick(GameObject target) {
		if (userUnit != null) {
			ModuleManager.Instance.HideModule(ModuleEnum.UnitLevelupAndEvolveModule);
			ModuleManager.Instance.ShowModule(ModuleEnum.UnitSourceModule,"unit",userUnit);
		}
//			ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"unit",userUnit);
	}

	void ShowShield(int currCount, int totalCount) {
		haveLabel.enabled = true;
		bool show = currCount >= totalCount;
		haveLabel.text = currCount + " / " + totalCount;
		haveLabel.color = show ? Color.green : Color.red;
		maskSprite.enabled = show;
	}

	private void ShowUnitType(){
		switch (userUnit.type){
		case EUnitType.UFIRE :
			bgprite.spriteName = "avatar_bg_fire";
			borderSprite.spriteName = "avatar_border_fire";
			break;
		case EUnitType.UWATER :
			bgprite.spriteName = "avatar_bg_water";
			borderSprite.spriteName = "avatar_border_water";
			
			break;
		case EUnitType.UWIND :
			bgprite.spriteName = "avatar_bg_wind";
			borderSprite.spriteName = "avatar_border_wind";
			
			break;
		case EUnitType.ULIGHT :
			bgprite.spriteName = "avatar_bg_light";
			borderSprite.spriteName = "avatar_border_light";
			
			break;
		case EUnitType.UDARK :
			bgprite.spriteName = "avatar_bg_dark";
			borderSprite.spriteName = "avatar_border_dark";
			
			break;
		case EUnitType.UNONE :
			bgprite.spriteName = "avatar_bg_none";
			borderSprite.spriteName = "avatar_border_none";
			
			break;
		default:
			break;
		}
	}

}