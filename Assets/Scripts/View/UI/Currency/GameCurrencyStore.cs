using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

public class GameCurrencyStore {

	public static int CurrencyBalance = 0;
	
	public static Dictionary<string, int> GoodsBalances = new Dictionary<string, int>();
	public static List<VirtualCurrency> VirtualCurrencies = null;
	public static List<VirtualGood> VirtualGoods = null;
	public static List<VirtualCurrencyPack> VirtualCurrencyPacks = null;
	
	public static void UpdateBalances() {
		if (VirtualCurrencies.Count > 0) {
			CurrencyBalance = StoreInventory.GetItemBalance(VirtualCurrencies[0].ItemId);
		}
		foreach(VirtualGood vg in VirtualGoods){
			GoodsBalances[vg.ItemId] = StoreInventory.GetItemBalance(vg.ItemId);
		}
	}
	
	public static void Init() {
		VirtualCurrencies = StoreInfo.GetVirtualCurrencies();
		VirtualGoods = StoreInfo.GetVirtualGoods();
		VirtualCurrencyPacks = StoreInfo.GetVirtualCurrencyPacks();	
		UpdateBalances();
	}
}
