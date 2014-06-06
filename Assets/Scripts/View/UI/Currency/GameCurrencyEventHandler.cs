using UnityEngine;
using System.Collections;
using Soomla;

public class GameCurrencyEventHandler {

	public GameCurrencyEventHandler(){
//		StoreEvents.OnMarketPurchase += onMarketPurchase;
//		StoreEvents.OnMarketRefund += onMarketRefund;
		StoreEvents.OnItemPurchased += onItemPurchased;
//		StoreEvents.OnGoodEquipped += onGoodEquipped;
//		StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
//		StoreEvents.OnGoodUpgrade += onGoodUpgrade;
//		StoreEvents.OnBillingSupported += onBillingSupported;
//		StoreEvents.OnBillingNotSupported += onBillingNotSupported;
//		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
//		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
//		StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
//		StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
//		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
//		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
//		StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
//		StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;
//		StoreEvents.OnStoreControllerInitialized += onStoreControllerInitialized;
//		#if UNITY_ANDROID && !UNITY_EDITOR
//		StoreEvents.OnIabServiceStarted += onIabServiceStarted;
//		StoreEvents.OnIabServiceStopped += onIabServiceStopped;
//		#endif
	}

	public void onItemPurchased(PurchasableVirtualItem pvi){

	}
	
	/// <summary>
	/// Handles a store controller initialized event.
	/// </summary>
	public void onStoreControllerInitialized() {
		GameCurrencyStore.Init();
		
		// some usage examples for add/remove currency
		// some examples
//		if (GameCurrencyStore.VirtualCurrencies.Count>0) {
//			try {
//				StoreInventory.GiveItem(GameCurrencyStore.VirtualCurrencies[0].ItemId,4000);
//				Utils.LogDebug("SOOMLA ExampleEventHandler", "Currency balance:" + StoreInventory.GetItemBalance(GameCurrencyStore.VirtualCurrencies[0].ItemId));
//			} catch (VirtualItemNotFoundException ex){
//				Utils.LogError("SOOMLA ExampleEventHandler", ex.Message);
//			}
//		}
		
//		ExampleWindow.GetInstance().setupItemsTextures();
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	public void onIabServiceStarted() {
		
	}
	public void onIabServiceStopped() {
		
	}
	#endif
}
