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
			return;
		}

		//update user's account
		DataCenter.Instance.AccountInfo.Stone = rsp.stone;
		DataCenter.Instance.AccountInfo.StonePay = rsp.stonePay;
		DataCenter.Instance.AccountInfo.StoneFree = rsp.stoneFree;
		DataCenter.Instance.AccountInfo.PayTotal = rsp.payTotal;
		
		GameObject.Find("PlayerInfoBar(Clone)").GetComponent<PlayerInfoBar>().UpdateData();

		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("PurchaseSuccess_Title");
		mwp.contentText = TextCenter.GetText("PurchaseSuccess_Content");
		BtnParam sure = new BtnParam ();
		sure.callback = o=>{
			//refresh bonusList
			if (rsp.productId == "ms.chip.monthcard" || rsp.productId == "ms.chip.weekcard") {
				GetBonusList.SendRequest (OnBonusList);
			} else {
				UIManager.Instance.ChangeScene(SceneEnum.Home);
				UIManager.Instance.ChangeScene(SceneEnum.Shop);
			}
		};
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);

		Debug.LogError ("OnRspShopBuy now stone=" + rsp.stone);




	}

	private void OnBonusList(object data){
//		bbproto.BonusInfo bsInfo = 
		DataCenter.Instance.LoginInfo.Bonus = data as List<bbproto.BonusInfo>;
		UIManager.Instance.ChangeScene (SceneEnum.Reward);
	}

	public void onItemPurchased(PurchasableVirtualItem pvi){
		Debug.Log ("onItemPurchased: productId=" + pvi.ItemId);
		ShopBuy.SendRequest(OnRspShopBuy, pvi.ItemId);
	}

	public void onMarketRefund(PurchasableVirtualItem pvi){

	}

	public void onBillingNotSupported(){
		MsgWindowParams mwp = new MsgWindowParams ();
		mwp.btnParam = new BtnParam ();
		mwp.titleText = TextCenter.GetText("BillingNotSupported_Title");
		mwp.contentText = TextCenter.GetText("BillingNotSupported_Content");
		
		BtnParam sure = new BtnParam ();
		sure.callback = null;
		sure.text = TextCenter.GetText("OK");
		mwp.btnParam = sure;
		
		MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
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
