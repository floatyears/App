using UnityEngine;
using System.Collections;
using Soomla;

public class GameCurrencyEventHandler {

	public GameCurrencyEventHandler(){
//		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnMarketRefund += onMarketRefund;
		StoreEvents.OnItemPurchased += onItemPurchased;
//		StoreEvents.OnGoodEquipped += onGoodEquipped;
//		StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
//		StoreEvents.OnGoodUpgrade += onGoodUpgrade;
//		StoreEvents.OnBillingSupported += onBillingSupported;
		StoreEvents.OnBillingNotSupported += onBillingNotSupported;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
//		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
//		StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
//		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
//		StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
//		StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;
//		StoreEvents.OnStoreControllerInitialized += onStoreControllerInitialized;
//		#if UNITY_ANDROID && !UNITY_EDITOR
//		StoreEvents.OnIabServiceStarted += onIabServiceStarted;
//		StoreEvents.OnIabServiceStopped += onIabServiceStopped;
//		#endif
	}

	public void onItemPurchased(PurchasableVirtualItem pvi){
		Debug.Log ("android.test.purchased");
	}

	public void onMarketRefund(PurchasableVirtualItem pvi){

	}

	public void onBillingNotSupported(){
		MsgWindowParams mwp = new MsgWindowParams ();
		//mwp.btnParams = new BtnParam[1];
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("guide1_title");
		mwp.contentText = TextCenter.GetText("guide1_content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenGuideMsgWindow, mwp);
	}

	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi){
		
	}

	public void onUnexpectedErrorInStore(string err){
		
	}

	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi){
		
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
