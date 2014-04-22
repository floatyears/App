using UnityEngine;
using System.Collections;
using bbproto;

public enum CatalogState{
	UnKnown,
	Got,
	Meet
}

public class CatalogUnitItem : MonoBehaviour {
	private UISprite avatarSpr;
	private UISprite erotemeSpr;
	private UISprite maskSpr;
	private UISprite translucentMaskSpr;
	private UILabel idLabel;

	/// <summary>
	/// The earliest execute
	/// </summary>
	/// <param name="item">Item.</param>
	public static CatalogUnitItem Inject(GameObject item){
		CatalogUnitItem view = item.GetComponent<CatalogUnitItem>();
		if (view == null) view = item.AddComponent<CatalogUnitItem>();
		return view;
	}

	/// <summary>
	/// Awake() execute as soon as  the static function CatalogUnitItem.Inject() execute
	/// </summary>
	private void Awake(){
		avatarSpr = transform.FindChild("Sprite_Avatar").GetComponent<UISprite>();
		erotemeSpr = transform.FindChild("Sprite_Erotemer").GetComponent<UISprite>();
		maskSpr = transform.FindChild("Sprite_Mask").GetComponent<UISprite>();
		translucentMaskSpr = transform.FindChild("Sprite_Translucent").GetComponent<UISprite>();
		idLabel = transform.FindChild("Label_ID").GetComponent<UILabel>();
	}

	/// <summary>
	/// public interface for the scene of catalog
	/// </summary>
	/// <param name="unitID">Unit I.</param>
	public void Refresh(int unitID){
		UserUnit userUnit = new UserUnit();
		userUnit.level = 1;
		userUnit.exp = 0;
		userUnit.unitId = (uint)unitID;
		CatalogUserUnit = new TUserUnit(userUnit);
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
			switch (state) {
				case CatalogState.Got : 
					avatarSpr.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
					avatarSpr.spriteName = catalogUserUnit.UnitID.ToString();
					erotemeSpr.enabled = false;
					maskSpr.enabled = false;
					translucentMaskSpr.enabled = false;
					UIEventListenerCustom.Get(this.gameObject).LongPress = PressItem;
					break;
				case CatalogState.Meet : 
					avatarSpr.atlas = DataCenter.Instance.GetAvatarAtlas(catalogUserUnit.UnitID);
					avatarSpr.spriteName = catalogUserUnit.UnitID.ToString();
					erotemeSpr.enabled = true;
					maskSpr.enabled = false;
					translucentMaskSpr.enabled = true;
//					UIEventListenerCustom.Get(this.gameObject).LongPress = null;
					break;
				case CatalogState.UnKnown : 
					avatarSpr.atlas = null;
					avatarSpr.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSpr.enabled = true;
					translucentMaskSpr.enabled = false;
//					UIEventListenerCustom.Get(this.gameObject).LongPress = null;
					break;
				default:
					avatarSpr.atlas = null;
					avatarSpr.spriteName = string.Empty;
					erotemeSpr.enabled = true;
					maskSpr.enabled = true;
					translucentMaskSpr.enabled = false;
//					UIEventListenerCustom.Get(this.gameObject).LongPress = null;
					break;
			}
			idLabel.text = "ID : " + catalogUserUnit.UnitID.ToString();
			idLabel.color = Color.green;
		}
	}

//	void OnBecameInvisible() {
//		gameObject.SetActive (false);
//	}
//
//	void OnBecameVisible() {
//		gameObject.SetActive (true);
//	}
	
    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/CatalogUnitPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
			}
			return itemPrefab;
		}
	}

	private void PressItem(GameObject item){
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, catalogUserUnit);
	}

}
