using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

public class ShopItem : DragPanelItemBase {

	private UISprite bg;
	private UILabel btnText;
	private UILabel desc;
	private UILabel count;
	private UILabel textName;

	private static GameObject prefab;

	public static ShopItem Inject(GameObject view){
		ShopItem shopItem = view.GetComponent<ShopItem>();
		if(shopItem == null) shopItem = view.AddComponent<ShopItem>();

		return shopItem;
	}

	public static GameObject Prefab{
		get{
			if(prefab == null){
				string sourcePath = "Prefabs/UI/Shop/ShopItem";
				prefab = ResourceManager.Instance.LoadLocalAsset(sourcePath, null) as GameObject;
			}
			return prefab;
		}
	}

	private ShopItemData data;
	public ShopItemData Data{
		get{return data;}
	}

	public override void SetData<T> (T d, params object[] args)
	{
		data = d as ShopItemData;
		if(bg == null){
			bg = transform.FindChild("Sprite").GetComponent<UISprite>();
			btnText = transform.FindChild("BuyBtn/Label").GetComponent<UILabel>();
			desc = transform.FindChild("Desc").GetComponent<UILabel>();
			count = transform.FindChild("Count").GetComponent<UILabel>();
			textName = transform.FindChild("TextName").GetComponent<UILabel>();
			
			UIEventListenerCustom.Get(transform.FindChild("BuyBtn").gameObject).onClick = OnBuy;
		}
		btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
		
		switch (data.type) {
		case ShopItemEnum.MonthCard:
			textName.text = TextCenter.GetText("MonthCard");
			count.text = "";
			desc.text = TextCenter.GetText("MonthCardDesc");
			bg.enabled = false;
			//				btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
			break;
		case ShopItemEnum.WeekCard:
			textName.text = TextCenter.GetText("WeekCard");
			count.text = "";
			desc.text = TextCenter.GetText("WeekCardDesc");
			bg.enabled = false;
			//				btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
			break;
		case ShopItemEnum.Stone:
			textName.text = "";
			count.text = TextCenter.GetText("StoneCount") + data.count;//TextCenter.GetText("");
			bg.enabled = true;
			if(DataCenter.Instance.UserData.AccountInfo.payTotal == 0){
				//					btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
				desc.text = string.Format(TextCenter.GetText("StoneDescFirst"),data.sale);
			}else{
				//					btnText.text = TextCenter.GetText("StonePriceAfterFirst");
				desc.text = string.Format(TextCenter.GetText("StoneDescAfterFirst"),data.sale);
			}
			
			break;
		default:
			count.text = TextCenter.GetText("");
			desc.text = TextCenter.GetText("");
			bg.enabled = false;
			btnText.text = TextCenter.GetText("");
			break;
		}
	}

	public override void ItemCallback (params object[] args)
	{
//		throw new System.NotImplementedException ();
	}

	private void OnBuy(GameObject obj){
		StoreInventory.BuyItem (data.itemId);
		Debug.Log("item id: " + data.itemId);
		MsgCenter.Instance.AddListener (CommandEnum.OnBuyEvent,OnBuyHandler);
	}

	public void Destroy(){
		UIEventListenerCustom.Get(transform.FindChild("BuyBtn").gameObject).onClick = null;
	}

	void OnBuyHandler(object d){
		Dictionary<string,string> dic = d as Dictionary<string,string>;
		Debug.Log("item id: " + data.itemId);
		if (dic.ContainsKey("id") && dic ["id"] == data.itemId) {
			MsgCenter.Instance.RemoveListener (CommandEnum.OnBuyEvent,OnBuyHandler);
			if (dic.ContainsKey ("success") && dic ["success"] == "1") {
				Umeng.GA.Pay (double.Parse (data.money), Umeng.GA.PaySource.AppStore, double.Parse (data.count + ""));	
			}
		}

//		
	}

}

public class ShopItemData{
	public string money;
	public ShopItemEnum type;
	public int count;
	public string itemId;
	public string sale;

	public ShopItemData(string _money, ShopItemEnum _type, int _count, string _itemId, string _sale){
		money = _money;
		type = _type;
		count = _count;
		itemId = _itemId;
		sale = _sale;
	}
}

public enum ShopItemEnum{
	MonthCard,
	WeekCard,
	Stone,
}
