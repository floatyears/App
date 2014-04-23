using UnityEngine;
using System.Collections;

public class MyUnitItem : BaseUnitItem {
	protected UISprite lightSpr;
	protected UILabel partyLabel;
	protected UISprite lockSpr;

	protected bool isParty;
	public bool IsParty{
		get{
			return isParty;
		}
		set{
			isParty = value;
			UpdatePartyState();
		}
	}

	private bool isFavorite;
	public bool IsFavorite{
		get{
			return isFavorite;
		}
		set{
			isFavorite = value;
			UpdateFavoriteState();
		}
	}

	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			if(isFocus == value) return;
			isFocus = value;
			UpdateFocus();
		}
	}

    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/MyUnitPrefab";
				itemPrefab = Resources.Load(sourcePath) as GameObject ;
			}
			return itemPrefab;
		}
	}

	public static MyUnitItem Inject(GameObject item){
		MyUnitItem view = item.AddComponent<MyUnitItem>();
		if (view == null) view = item.AddComponent<MyUnitItem>();
                return view;
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		lockSpr = transform.FindChild("Sprite_Lock").GetComponent<UISprite>();
		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.enabled = false;
		partyLabel.text = "Party";
		partyLabel.color = Color.red;
	}

	protected override void InitState(){
		base.InitState();
		IsFavorite = (userUnit.IsFavorite == 1) ? true : false;
		IsParty = false;
	}

	protected override void ClickItem(GameObject item){}
	protected virtual void UpdatePartyState(){}
	protected virtual void UpdateFocus(){}

	protected virtual void UpdateFavoriteState(){
		lockSpr.enabled = isFavorite;
		//Debug.Log(string.Format("Name is : {0} : isFavorite is : {1}, lockSpr.enabled is : {2}, pos is {3}",
		//                      userUnit.UnitInfo.Name, isFavorite, lockSpr.enabled, gameObject.name));
	}


	protected override void SetCommonState(){
		base.SetCommonState();
		IsFavorite = (userUnit.IsFavorite == 1) ? true : false;
	}
	
}
