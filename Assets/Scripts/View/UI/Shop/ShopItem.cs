using UnityEngine;
using System.Collections;
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

			switch (data.type) {
			case ShopItemEnum.MonthCard:
				count.text = TextCenter.GetText("");
				desc.text = TextCenter.GetText("");
				bg.enabled = false;
				break;
			case ShopItemEnum.Stone:
				count.text = TextCenter.GetText("");
				desc.text = TextCenter.GetText("");
				bg.enabled = true;
				break;
			default:
				count.text = TextCenter.GetText("");
				desc.text = TextCenter.GetText("");
				break;
			}
			
			//			Debug.Log("reward item: " + data.id + " gift: " + data.giftItem.Count);
		}
	}

	private void OnBuy(GameObject obj){
		StoreInventory.BuyItem (data.itemId);
	}

	public void Destroy(){
		UIEventListenerCustom.Get(transform.FindChild("BuyBtn").gameObject).onClick = null;
	}
}

public class ShopItemData{
	public int money;
	public ShopItemEnum type;
	public int count;
	public string itemId;

	public ShopItemData(int _money, ShopItemEnum _type, int _count, string _itemId){
		money = _money;
		type = _type;
		count = _count;
		itemId = _itemId;
	}
}

public enum ShopItemEnum{
	MonthCard,
	Stone,
}
