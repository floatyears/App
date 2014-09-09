using UnityEngine;
using System.Collections;
using Soomla;

/// <summary>
/// This class defines our game's economy, which includes virtual goods, virtual currencies
/// and currency packs, virtual categories, and non-consumable items.
/// </summary>
public class GameCurrencyAssets :  IStoreAssets{

	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() {
		return 0;
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{MUFFIN_CURRENCY};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() {
		//return new VirtualGood[] {MUFFINCAKE_GOOD, PAVLOVA_GOOD,CHOCLATECAKE_GOOD, CREAMCUP_GOOD};
		return new VirtualGood[]{};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {MONTH_CARD, WEEK_CARD, CHIP_PACK1, CHIP_PACK2, CHIP_PACK3, CHIP_PACK4, CHIP_PACK5, CHIP_PACK6};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{};//GENERAL_CATEGORY};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public NonConsumableItem[] GetNonConsumableItems() {
		return new NonConsumableItem[]{};//NO_ADDS_NONCONS};
	}
	
	/** Static Final Members **/

	public const string MUFFIN_CURRENCY_ITEM_ID      = ServerConfig.ProductIDPrefix+"_currency";

	public const string PID_MONTH_CARD = ServerConfig.ProductIDPrefix + ".chip.monthcard";

	public const string PID_WEEK_CARD = ServerConfig.ProductIDPrefix + ".chip.weekcard";

	public const string PID_CHIP_PACK1 = ServerConfig.ProductIDPrefix + ".chip.pack1";//"slotsempire.coins.pack2";//"ms.chip.pack1";
	
	public const string PID_CHIP_PACK2 = ServerConfig.ProductIDPrefix + ".chip.pack2";//"android.test.purchased";//"slotsempire.gems.pack2";

	public const string PID_CHIP_PACK3 = ServerConfig.ProductIDPrefix + ".chip.pack3";

	public const string PID_CHIP_PACK4 = ServerConfig.ProductIDPrefix + ".chip.pack4";

	public const string PID_CHIP_PACK5 = ServerConfig.ProductIDPrefix + ".chip.pack5";

	public const string PID_CHIP_PACK6 = ServerConfig.ProductIDPrefix + ".chip.pack6";//"android.test.purchased";
	
//	public const string FOURHUNDMUFF_PACK_PRODUCT_ID = "android.test.purchased";
//	
//	public const string THOUSANDMUFF_PACK_PRODUCT_ID = "2500_pack";
//	
//	public const string NO_ADDS_NONCONS_PRODUCT_ID   = "no_ads";
//	
//	public const string MUFFINCAKE_ITEM_ID   = "fruit_cake";
//	
//	public const string PAVLOVA_ITEM_ID   = "pavlova";
//	
//	public const string CHOCLATECAKE_ITEM_ID   = "chocolate_cake";
//	
//	public const string CREAMCUP_ITEM_ID   = "cream_cup";
	
	
	/** Virtual Currencies **/
	
	public static VirtualCurrency MUFFIN_CURRENCY = new VirtualCurrency(
		"Muffins",										// name
		"",												// description
		MUFFIN_CURRENCY_ITEM_ID							// item id
		);
	
	
	/** Virtual Currency Packs **/
	public static VirtualCurrencyPack MONTH_CARD = new VirtualCurrencyPack(
		"MonthCard",                                   // name
		"",                       // description
		PID_MONTH_CARD,                                   // item id
		6,												// number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_MONTH_CARD, 5.99)
		);

	public static VirtualCurrencyPack WEEK_CARD = new VirtualCurrencyPack(
		"WeekCard",                                   // name
		"",                       // description
		PID_WEEK_CARD,                                   // item id
		6,												// number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_WEEK_CARD, 1.99)
		);

	public static VirtualCurrencyPack CHIP_PACK1 = new VirtualCurrencyPack(
		"10 Muffins",                                   // name
		"Test refund of an item",                       // description
		PID_CHIP_PACK1,                                   // item id
		6,												// number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK1, 0.99)
		);
	
	public static VirtualCurrencyPack CHIP_PACK2 = new VirtualCurrencyPack(
		"50 Muffins",                                   // name
		"Test cancellation of an item",                 // description
		PID_CHIP_PACK2,                                   // item id
		36,                                             // number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK2, 4.99)
		);

	public static VirtualCurrencyPack CHIP_PACK3 = new VirtualCurrencyPack(
		"50 Muffins",                                   // name
		"Test cancellation of an item",                 // description
		PID_CHIP_PACK3,                                   // item id
		36,                                             // number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK3, 4.99)
		);

	public static VirtualCurrencyPack CHIP_PACK4 = new VirtualCurrencyPack(
		"50 Muffins",                                   // name
		"Test cancellation of an item",                 // description
		PID_CHIP_PACK4,                                   // item id
		36,                                             // number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK4, 4.99)
		);

	public static VirtualCurrencyPack CHIP_PACK5 = new VirtualCurrencyPack(
		"50 Muffins",                                   // name
		"Test cancellation of an item",                 // description
		PID_CHIP_PACK5,                                   // item id
		36,                                             // number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK5, 4.99)
		);

	public static VirtualCurrencyPack CHIP_PACK6 = new VirtualCurrencyPack(
		"50 Muffins",                                   // name
		"Test cancellation of an item",                 // description
		PID_CHIP_PACK6,                                   // item id
		36,                                             // number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(PID_CHIP_PACK6, 4.99)
		);
	
//	public static VirtualCurrencyPack FOURHUNDMUFF_PACK = new VirtualCurrencyPack(
//		"400 Muffins",                                  // name
//		"Test purchase of an item",                 	// description
//		"muffins_400",                                  // item id
//		400,                                            // number of currencies in the pack
//		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
//		new PurchaseWithMarket(FOURHUNDMUFF_PACK_PRODUCT_ID, 4.99)
//		);
//	
//	public static VirtualCurrencyPack THOUSANDMUFF_PACK = new VirtualCurrencyPack(
//		"1000 Muffins",                                 // name
//		"Test item unavailable",                 		// description
//		"muffins_1000",                                 // item id
//		1000,                                           // number of currencies in the pack
//		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
//		new PurchaseWithMarket(THOUSANDMUFF_PACK_PRODUCT_ID, 8.99)
//		);
//	
//	/** Virtual Goods **/
//	
//	public static VirtualGood MUFFINCAKE_GOOD = new SingleUseVG(
//		"Fruit Cake",                                       		// name
//		"Customers buy a double portion on each purchase of this cake", // description
//		"fruit_cake",                                       		// item id
//		new PurchaseWithVirtualItem(MUFFIN_CURRENCY_ITEM_ID, 225)); // the way this virtual good is purchased
//	
//	public static VirtualGood PAVLOVA_GOOD = new SingleUseVG(
//		"Pavlova",                                         			// name
//		"Gives customers a sugar rush and they call their friends", // description
//		"pavlova",                                          		// item id
//		new PurchaseWithVirtualItem(MUFFIN_CURRENCY_ITEM_ID, 175)); // the way this virtual good is purchased
//	
//	public static VirtualGood CHOCLATECAKE_GOOD = new SingleUseVG(
//		"Chocolate Cake",                                   		// name
//		"A classic cake to maximize customer satisfaction",	 		// description
//		"chocolate_cake",                                   		// item id
//		new PurchaseWithVirtualItem(MUFFIN_CURRENCY_ITEM_ID, 250)); // the way this virtual good is purchased
//	
//	
//	public static VirtualGood CREAMCUP_GOOD = new SingleUseVG(
//		"Cream Cup",                                        		// name
//		"Increase bakery reputation with this original pastry",   	// description
//		"cream_cup",                                        		// item id
//		new PurchaseWithVirtualItem(MUFFIN_CURRENCY_ITEM_ID, 50));  // the way this virtual good is purchased
	
	
	/** Virtual Categories **/
	// The muffin rush theme doesn't support categories, so we just put everything under a general category.
//	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
//		"General", new List<string>(new string[] { MUFFINCAKE_ITEM_ID, PAVLOVA_ITEM_ID, CHOCLATECAKE_ITEM_ID, CREAMCUP_ITEM_ID }));
//	
	
	/** Market MANAGED Items **/
	
//	public static NonConsumableItem NO_ADDS_NONCONS  = new NonConsumableItem(
//		"No Ads",
//		"Test purchase of MANAGED item.",
//		"no_ads",
//		new PurchaseWithMarket(new MarketItem(NO_ADDS_NONCONS_PRODUCT_ID, MarketItem.Consumable.NONCONSUMABLE , 1.99))
//		);

}
