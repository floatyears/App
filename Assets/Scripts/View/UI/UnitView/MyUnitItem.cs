using UnityEngine;
using System.Collections;

public class MyUnitItem : BaseUnitItem {
	protected UISprite lightSpr;
	protected UILabel partyLabel;
	public UILabel PartyLabel{
		get { 
			if(!partyLabel.enabled)
				partyLabel.enabled = true;
			return partyLabel; }
	}
	protected UISprite lockSpr;

	protected bool isParty;
	public bool IsParty{
		get{
			return isParty;
		}
		set{
//			Debug.LogError("isparty : " + isParty + " isenable : " + IsEnable);
			isParty = value;
			UpdatePartyState();
		}
	}

	private bool isFavorite;
	public bool IsFavorite {
		get{
			return isFavorite;
		}
		set{
			isFavorite = value;
			UpdateFavoriteState ();
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

	protected UIWidget mWidget;

	public UIWidget Widget{
		get { return mWidget; }
	}

    private static GameObject itemPrefab;
	public static GameObject ItemPrefab {
		get {
			if(itemPrefab == null) {
				string sourcePath = "Prefabs/UI/UnitItem/MyUnitPrefab";
				itemPrefab = ResourceManager.Instance.LoadLocalAsset(sourcePath,null) as GameObject ;
			}
			return itemPrefab;
		}
	}

	public static MyUnitItem Inject(GameObject item){
		MyUnitItem view = item.GetComponent<MyUnitItem>();
		if (view == null) view = item.AddComponent<MyUnitItem>();
                return view;
	}

	protected override void InitUI(){
		base.InitUI();
		lightSpr = transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		lockSpr = transform.FindChild("Sprite_Lock").GetComponent<UISprite>();
		partyLabel = transform.FindChild("Label_Party").GetComponent<UILabel>();
		partyLabel.enabled = false;
		partyLabel.text = TextCenter.GetText("Text_Party");
		partyLabel.color = Color.red;

		mWidget = GetComponent<UIWidget> ();
	}

	protected override void InitState(){
		base.InitState();
		if(userUnit == null){return;}
		IsFavorite = (userUnit.IsFavorite == 1) ? true : false;
		IsParty = false;
	}

	protected override void ClickItem(GameObject item){}
	protected virtual void UpdatePartyState(){}
	protected virtual void UpdateFocus(){}

	protected virtual void UpdateFavoriteState(){
		lockSpr.enabled = isFavorite;
	}
	
	protected override void SetCommonState(){
		base.SetCommonState();
//		Debug.LogError (userUnit.TUserUnitID + " userUnit.IsFavorite : " + userUnit.IsFavorite);
		IsFavorite = DGTools.CheckFavorate (userUnit);//(userUnit.IsFavorite == 1) ? true : false;
	}

	protected override void PressItem(GameObject item){
		base.PressItem(item);

		MsgCenter.Instance.Invoke(CommandEnum.ShowFavState);
	}

	public void ChangeUserUnit(TUserUnit tuu) {
		BehindChangeUserUnit (tuu);
		userUnit = tuu;
	} 

	protected virtual void BehindChangeUserUnit(TUserUnit tuu) {

	}
}
