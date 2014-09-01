using UnityEngine;
using System.Collections;
using bbproto;

public enum CatalogState{
	UnKnown,
	Got,
	Meet
}

public class CatalogUnitItem : MyUnitItem {
	private UISprite avatarSprite;
	private UISprite erotemeSpr;
	private UISprite maskSprite;
	private UISprite translucentMaskSpr;
	private UILabel idLabel;
	public UIWidget widget;

	private UISprite type;
	private UISprite bg;
	
	/// <summary>
	/// The earliest execute
	/// </summary>
	/// <param name="item">Item.</param>
	public static CatalogUnitItem Inject(GameObject item){
		CatalogUnitItem view = item.GetComponent<CatalogUnitItem>();
		if (view == null) view = item.AddComponent<CatalogUnitItem>();
		return view;
	}

	protected override void Awake(){
		widget = GetComponent<UIWidget>();
		mWidget = widget;

		avatarSprite = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Erotemer").GetComponent<UISprite>();
		maskSprite = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		maskSpr = maskSprite;
		//translucentMaskSpr = transform.FindChild("Sprite_Translucent").GetComponent<UISprite>();
		idLabel = transform.FindChild("Label_ID").GetComponent<UILabel>();
		type = transform.FindChild ("Sprite_Type").GetComponent<UISprite> ();
		bg = transform.FindChild ("Background").GetComponent<UISprite> ();
	}

	protected override void RefreshState(){
		CatalogUserUnit = userUnit;
	}

	/// <summary>
	/// public interface for the scene of catalog
	/// </summary>
	/// <param name="unitID">Unit I.</param>
	public void Refresh(int unitID){
//		UserUnit userUnit = new UserUnit();
//		userUnit.level = 1;
//		userUnit.exp = 0;
//		userUnit.unitId = (uint)unitID;
//		CatalogUserUnit = new TUserUnit(userUnit);
	}
	
	private TUserUnit catalogUserUnit;
	public TUserUnit CatalogUserUnit{
		get{
			return catalogUserUnit;
		}
		set{
			catalogUserUnit = value;
			if(catalogUserUnit == null){
				Debug.LogError(string.Format("gameObject named {0} , TUserUnit is NULL...", gameObject.name));
				State = CatalogState.UnKnown;
			}
			else{
				//Debug.LogError("gameObject is : " + gameObject.name + "    unitId is : " + catalogUserUnit.UnitID);
				if(DataCenter.Instance.CatalogInfo.IsHaveUnit(catalogUserUnit.UnitID)){
					//Debug.LogError("unitID : " + catalogUserUnit.UnitID+" isHave.");
					State = CatalogState.Got;
				}
				else if(DataCenter.Instance.CatalogInfo.IsMeetNotHaveUnit(catalogUserUnit.UnitID)){
					//Debug.LogError("unitID : " + catalogUserUnit.UnitID+" isMeet.");
					State = CatalogState.Meet;
				}
				else{
					//Debug.LogError("unitid : " + catalogUserUnit.UnitID+" isUnknown.");
					State = CatalogState.UnKnown;
				}
			}
		}
	}

	private CatalogState state;
	public CatalogState State{
		get{
			return state;
		}
		set{
			UIEventListenerCustom.Get(this.gameObject).LongPress = null;
			UIEventListenerCustom.Get(this.gameObject).onClick = null;
			state = value;

			switch (state) {
				case CatalogState.Got : 
//				Debug.LogError("catalogUserUnit.UnitID : " + catalogUserUnit.UnitID + " state: " + state);
//				DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
				DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID, avatarSprite);
//					avatarSprite.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
//					avatarSprite.spriteName = catalogUserUnit.UnitID.ToString();
					erotemeSpr.enabled = false;
					maskSprite.enabled = false;
					//translucentMaskSpr.enabled = false;
					UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
					UIEventListenerCustom.Get(this.gameObject).onClick = PressItem;
					type.spriteName = GetBorderSpriteName();
					bg.spriteName = GetAvatarBgSpriteName();
					break;
				case CatalogState.Meet : 
//					avatarSprite.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
//					avatarSprite.spriteName = catalogUserUnit.UnitID.ToString();
					DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID, avatarSprite);
					maskSprite.enabled = true;
					erotemeSpr.enabled = false;
					//translucentMaskSpr.enabled = true;
					type.spriteName = "avatar_border_none";
					bg.spriteName = "avatar_bg_none";
					break;
				case CatalogState.UnKnown : 
					avatarSprite.atlas = null;
					avatarSprite.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSprite.enabled = false;
					//translucentMaskSpr.enabled = false;
					type.spriteName = "avatar_border_none";
					bg.spriteName = "avatar_bg_none";
					break;
				default:
					avatarSprite.atlas = null;
					avatarSprite.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSprite.enabled = true;
					//translucentMaskSpr.enabled = false;
					break;
			}
			idLabel.text = "No. " + catalogUserUnit.UnitID.ToString();
//			idLabel.color = Color.green;
		}
	}

    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/CatalogUnitPrefab";
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject ;
			}
			return itemPrefab;
		}
	}

	string GetBorderSpriteName () {
		switch (catalogUserUnit.UnitType) {
		case 1:
			return "avatar_border_fire";
		case 2:
			return "avatar_border_water";
		case 3:
			return "avatar_border_wind";
		case 4:
			return "avatar_border_light";
		case 5:
			return "avatar_border_dark";
		case 6:
			return "avatar_border_none";
		default:
			return "avatar_border_none";
			break;
		}
	}
	
	string GetAvatarBgSpriteName() {
		switch (catalogUserUnit.UnitType) {
		case 1:
			return "avatar_bg_fire";
		case 2:
			return "avatar_bg_water";
		case 3:
			return "avatar_bg_wind";
		case 4:
			return "avatar_bg_light";
		case 5:
			return "avatar_bg_dark";
		case 6:
			return "avatar_bg_none";
		default:
			return "avatar_bg_none";
			break;
		}
	}

	private void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(ModuleEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, catalogUserUnit);
	}

}
