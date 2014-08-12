using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

public class ShopItem : MonoBehaviour {

	private UISprite bg;
	private UILabel btnText;
	private UILabel desc;
	private UILabel count;

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
		set{
			data = value;
			if(bg == null){
				bg = transform.FindChild("Sprite").GetComponent<UISprite>();
				btnText = transform.FindChild("BuyBtn/Label").GetComponent<UILabel>();
				desc = transform.FindChild("Desc").GetComponent<UILabel>();
				count = transform.FindChild("Name").GetComponent<UILabel>();

				UIEventListenerCustom.Get(transform.FindChild("BuyBtn").gameObject).onClick = OnBuy;
			}
			btnText.text = TextCenter.GetText("MoneyUnit") + data.money;

			switch (data.type) {
			case ShopItemEnum.MonthCard:
				count.text = TextCenter.GetText("MonthCard");
				desc.text = TextCenter.GetText("MonthCardDesc");
				bg.enabled = false;
//				btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
				break;
			case ShopItemEnum.WeekCard:
				count.text = TextCenter.GetText("WeekCard");
				desc.text = TextCenter.GetText("WeekCardDesc");
				bg.enabled = false;
				//				btnText.text = TextCenter.GetText("MoneyUnit") + data.money;
				break;
			case ShopItemEnum.Stone:
				count.text = TextCenter.GetText("StoneCount") + data.count;//TextCenter.GetText("");
				bg.enabled = true;
				if(DataCenter.Instance.AccountInfo.PayTotal == 0){
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
			
			//			Debug.Log("reward item: " + data.id + " gift: " + data.giftItem.Count);
		}
	}

	private void OnBuy(GameObject obj){
		StoreInventory.BuyItem (data.itemId);
		MsgCenter.Instance.AddListener (CommandEnum.OnBuyEvent,OnBuyHandler);
	}

	public void Destroy(){
		UIEventListenerCustom.Get(transform.FindChild("BuyBtn").gameObject).onClick = null;
	}

	void OnBuyHandler(object d){
		Dictionary<string,string> dic = d as Dictionary<string,string>;
		if (dic.ContainsKey ["id"] && dic ["id"] == data.itemId) {
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
