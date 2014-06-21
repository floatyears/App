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

		avatarSprite = transform.FindChild("Avatar").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Erotemer").GetComponent<UISprite>();
		maskSprite = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		//translucentMaskSpr = transform.FindChild("Sprite_Translucent").GetComponent<UISprite>();
		idLabel = transform.FindChild("Label_ID").GetComponent<UILabel>();
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
			state = value;
			Debug.LogError("catalogUserUnit.UnitID : " + catalogUserUnit.UnitID);
			switch (state) {
				case CatalogState.Got : 

//				DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
				DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID, avatarSprite);
//					avatarSprite.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
//					avatarSprite.spriteName = catalogUserUnit.UnitID.ToString();
					erotemeSpr.enabled = false;
					maskSprite.enabled = false;
					//translucentMaskSpr.enabled = false;
					UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
					break;
				case CatalogState.Meet : 
//					avatarSprite.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
//					avatarSprite.spriteName = catalogUserUnit.UnitID.ToString();
					DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID, avatarSprite);
					erotemeSpr.enabled = true;
					maskSprite.enabled = false;
					//translucentMaskSpr.enabled = true;
					break;
				case CatalogState.UnKnown : 
					avatarSprite.atlas = null;
					avatarSprite.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSprite.enabled = true;
					//translucentMaskSpr.enabled = false;
					break;
				default:
					avatarSprite.atlas = null;
					avatarSprite.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSprite.enabled = true;
					//translucentMaskSpr.enabled = false;
					break;
			}
			idLabel.text = "ID : " + catalogUserUnit.UnitID.ToString();
			idLabel.color = Color.green;
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

	private void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, catalogUserUnit);
	}

}
