using UnityEngine;
using System.Collections;
using Soomla;

public class GameCurrencyView: ViewBase {

	private GameCurrencyEventHandler handler;

	public override void Init(UIConfigItem config) {
		base.Init(config);
		InitUI();


		handler = new GameCurrencyEventHandler ();

		StoreController.Initialize (new GameCurrencyAssets ());
	}
	
	public override void ShowUI() {

#if UNITY_ANDROID
	StoreController.StartIabServiceInBg();
#endif

		base.ShowUI();
	}
	
	public override void HideUI() {

#if UNITY_ANDROID
	StoreController.StopIabServiceInBg();
#endif

		base.HideUI();
	}
	
	public override void DestoryUI() {

		base.DestoryUI();
	}
	
//	public override void CallbackView(params object[] args) {
//		base.CallbackView(data);
//		
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
//		
//		switch (cbdArgs.funcName){
//		case "TurnRequiredFriendListScene": 
//			CallBackDispatcherHelper.DispatchCallBack(TurnToNextScene, cbdArgs);
//			break;
//		default:
//			break;
//		}
//		
//	}
	
	private void InitUI() {

	}

	public void Buy1(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK1.ItemId);
	}

	public void Buy2(){
		StoreInventory.BuyItem (GameCurrencyAssets.CHIP_PACK2.ItemId);
	}
}
