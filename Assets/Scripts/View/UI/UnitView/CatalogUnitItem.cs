using UnityEngine;
using System.Collections;

public class CatalogUnitItem : BaseUnitItem {
	protected UISprite avatarSpr;
    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				itemPrefab = Resources.Load("Prefabs/UI/UnitItem/CatalogUnitPrefab") as GameObject ;
			}
			return itemPrefab;
		}
	}

	public static CatalogUnitItem Inject(GameObject item){
		CatalogUnitItem view = item.AddComponent<CatalogUnitItem>();
		if (view == null) view = item.AddComponent<CatalogUnitItem>();
                return view;
	}

	protected override void InitUI(){
//		base.InitUI();
		avatarSpr = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		avatarSpr.atlas = DataCenter.Instance.GetAvatarAtlas(UserUnit.UnitID);
		avatarSpr.spriteName = UserUnit.UnitID.ToString();
	}

	protected override void InitState(){
//		base.InitState();
	}
	
	protected override void ClickItem(GameObject item){}

}
