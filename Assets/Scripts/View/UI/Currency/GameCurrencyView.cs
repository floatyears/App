using UnityEngine;
using System.Collections;
using Soomla;

public class GameCurrencyView: UIComponentUnity {

	private GameCurrencyEventHandler handler;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();

		handler = new GameCurrencyEventHandler ();

		StoreController.Initialize (new GameCurrencyAssets ());
	}
	
	public override void ShowUI() {
		base.ShowUI();
	}
	
	public override void HideUI() {
		base.HideUI();
	}
	
	public override void DestoryUI() {
		base.DestoryUI();
	}
	
//	public override void CallbackView(object data) {
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
		StoreInventory.BuyItem (GameCurrencyAssets.TENMUFF_PACK.ItemId);
	}

	public void Buy2(){
		StoreInventory.BuyItem (GameCurrencyAssets.FIFTYMUFF_PACK.ItemId);
	}
}
