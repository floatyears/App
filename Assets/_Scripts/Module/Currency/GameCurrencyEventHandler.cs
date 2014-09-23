using UnityEngine;
using System.Collections;
using Soomla;
using System.Collections.Generic;

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

	void OnRspShopBuy(object data) {
		bbproto.RspShopBuy rsp = data as bbproto.RspShopBuy;
		if (rsp == null || rsp.stone == null ) {
			Debug.LogError ("OnRspShopBuy failed.");
			return;
		}

		if (rsp.header.code != (int)ErrorCode.SUCCESS){
			Debug.LogError("OnRspShopBuy code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			MsgCenter.Instance.Invoke(CommandEnum.OnBuyEvent,new Dictionary<string,string>(){{"id",rsp.productId},{"success","0"}});
			return;
		}
		MsgCenter.Instance.Invoke(CommandEnum.OnBuyEvent,new Dictionary<string,string>(){{"id",rsp.productId},{"success","1"}});
		//update user's account
		DataCenter.Instance.AccountInfo.stone = rsp.stone;
		DataCenter.Instance.AccountInfo.stonePay = rsp.stonePay;
		DataCenter.Instance.AccountInfo.stoneFree = rsp.stoneFree;
		DataCenter.Instance.AccountInfo.payTotal = rsp.payTotal;
		
		GameObject.Find("PlayerInfoBar(Clone)").GetComponent<PlayerInfoBarView>().UpdateData();

		TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("PurchaseSuccess_Title"),TextCenter.GetText("PurchaseSuccess_Content"),TextCenter.GetText("OK"),o=>{
			//refresh bonusList
			if (rsp.productId == GameCurrencyAssets.PID_MONTH_CARD || rsp.productId == GameCurrencyAssets.PID_WEEK_CARD) {
				BonusController.Instance.GetBonusList (OnBonusList);
			} else {
				ModuleManager.Instance.ShowModule(ModuleEnum.HomeModule);
				ModuleManager.Instance.ShowModule(ModuleEnum.ShopModule);
			}
		});

		Debug.LogError ("OnRspShopBuy now stone=" + rsp.stone);




	}

	private void OnBonusList(object data){
//		bbproto.BonusInfo bsInfo = 
		Debug.Log ("purchase success, change to reward. rsp data:"+data);
		bbproto.RspBonusList rsp = data as bbproto.RspBonusList;
		if (rsp != null && rsp.bonus != null ) {
			DataCenter.Instance.LoginInfo.Bonus = rsp.bonus;
			ModuleManager.Instance.ShowModule (ModuleEnum.RewardModule);
			MsgCenter.Instance.Invoke(CommandEnum.GotoRewardMonthCardTab,4);
		}
	}

	public void onItemPurchased(PurchasableVirtualItem pvi){
		Debug.Log ("onItemPurchased: productId=" + pvi.ItemId);

		Umeng.GA.Event("OnBuyOK", pvi.ItemId);

//		double price = getProductInfo(pvi.ItemId).Price;
//		double gotStones = getProductInfo(pvi.ItemId).Stones;
//		Umeng.GA.Pay (price, Umeng.GA.PaySource.AppStore, gotStones);

		ShopController.Instance.ShopBuy(OnRspShopBuy, pvi.ItemId);
	}

	public void onMarketRefund(PurchasableVirtualItem pvi){

	}

	public void onBillingNotSupported(){
		Umeng.GA.Event("BillingNotSupport");

//		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
		TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("BillingNotSupported_Title"),TextCenter.GetText("BillingNotSupported_Content"),TextCenter.GetText("OK"));
	}

	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi){
		
	}

	public void onUnexpectedErrorInStore(string err){
		Umeng.GA.Event("UnexpectedErrorOnBuy", err);
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
