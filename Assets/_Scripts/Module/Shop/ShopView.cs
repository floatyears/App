using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;

public class ShopView : ViewBase {
	private Dictionary<string,UIButton> buttonDic = new Dictionary<string, UIButton>();
    private UIButton btnFriendsExpansion;
    private UIButton btnStaminaRecover;
    private UIButton btnUnitExpansion;
	private GameObject infoPanelRoot;
	private GameObject windowRoot;

	private GameCurrencyEventHandler handler;

	private DragPanel dragPanel;

	public override void Init ( UIConfigItem config , Dictionary<string, object> data = null) {
		base.Init (config ,data);
		InitUI();

		handler = new GameCurrencyEventHandler ();

		try{
			StoreController.Initialize (new GameCurrencyAssets ());
		}catch(System.Exception e){
			Debug.LogException(e);
		}


	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUIAnimation();

		#if UNITY_ANDROID
		StoreController.StartIabServiceInBg();
		#endif
	}
	
	public override void HideUI () {
		base.HideUI ();

//		int count = dragPanel.ScrollItem.Count;
//		for (int i = 0; i < count; i++) {
//			GameObject go = dragPanel.ScrollItem[i];
//			GameObject.Destroy(go);
//		}
//		dragPanel.ScrollItem.Clear();

		#if UNITY_ANDROID
		StoreController.StopIabServiceInBg();
		#endif
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
		dragPanel.DestoryUI ();
	}

	private void InitUI() {
//		UIButton[] buttons = FindChild("btns").GetComponentsInChildren< UIButton >();
//		for (int i = 0; i < buttons.Length; i++){
//			buttonDic.Add( string.Format("Chip{0}", i), buttons[ i ] );
//			UIEventListener.Get( buttons[ i ].gameObject ).onClick = ClickButton;
//		}

        btnFriendsExpansion = FindChild<UIButton>("top/FriendsExpansion");
		UILabel friendExpandLabel = FindChild<UILabel>("top/FriendsExpansion/Label");
		friendExpandLabel.text = TextCenter.GetText("Btn_Friend_Expand");

        btnStaminaRecover = FindChild<UIButton>("top/StaminaRecover");
		UILabel recoverLabel = FindChild<UILabel>("top/StaminaRecover/Label");
		recoverLabel.text = TextCenter.GetText("Btn_Stamina_Recover");

        btnUnitExpansion = FindChild<UIButton>("top/UnitExpansion");
		UILabel unitExpandLabel = FindChild<UILabel>("top/UnitExpansion/Label");
		unitExpandLabel.text = TextCenter.GetText("Btn_Unit_Expand");

        UIEventListener.Get(btnFriendsExpansion.gameObject).onClick = OnClickFriendExpansion;
        UIEventListener.Get(btnStaminaRecover.gameObject).onClick = OnClickStaminaRecover;
        UIEventListener.Get(btnUnitExpansion.gameObject).onClick = OnClickUnitExpansion;

		infoPanelRoot = transform.FindChild("top").gameObject;
		windowRoot = transform.FindChild("Bottom").gameObject;

		CreateDragView ();
	}

	private string GetDiscutPercent(int itemIndex) {

		if (DataCenter.Instance.AccountInfo.payTotal == 0) { // 首冲双倍奖励 +100%
			if( itemIndex==1 ) {
				return "0"; //最低价套餐，不赠送
			} else if (itemIndex==101 || itemIndex==102) { //月卡、周卡 不赠送
				return "";
			}
			else { // 首冲双倍奖励
				return "100%";
			}
		}
		else { //
			switch(itemIndex) {
			case 101:
				return "";
			case 102:
				return "";
			case 1:
				return "0";
			case 2:
				return "10%";
			case 3:
				return "20%";
			case 4:
				return "30%";
			case 5:
				return "40%";
			case 6:
				return "50%";
			}
		}

		return "";
	}

	private void CreateDragView(){

		List<ShopItemData> shopItems = new List<ShopItemData> ();

#if LANGUAGE_CN
		{ //国内定价: RMB
			shopItems.Add (new ShopItemData ("30",ShopItemEnum.MonthCard,1,GameCurrencyAssets.PID_MONTH_CARD, GetDiscutPercent(101)));
			shopItems.Add (new ShopItemData ("10",ShopItemEnum.WeekCard, 1,GameCurrencyAssets.PID_WEEK_CARD,  GetDiscutPercent(102)));

			shopItems.Add (new ShopItemData ("6",ShopItemEnum.Stone,60,GameCurrencyAssets.PID_CHIP_PACK1,   GetDiscutPercent(1) ));
			shopItems.Add (new ShopItemData ("30",ShopItemEnum.Stone,300,GameCurrencyAssets.PID_CHIP_PACK2, GetDiscutPercent(2) ));
			shopItems.Add (new ShopItemData ("50",ShopItemEnum.Stone,500,GameCurrencyAssets.PID_CHIP_PACK3, GetDiscutPercent(3) ));
			shopItems.Add (new ShopItemData ("100",ShopItemEnum.Stone,1000,GameCurrencyAssets.PID_CHIP_PACK4, GetDiscutPercent(4)));
			shopItems.Add (new ShopItemData ("200",ShopItemEnum.Stone,2000,GameCurrencyAssets.PID_CHIP_PACK5, GetDiscutPercent(5)));
			shopItems.Add (new ShopItemData ("500",ShopItemEnum.Stone,5000,GameCurrencyAssets.PID_CHIP_PACK6, GetDiscutPercent(6)));
		}
#else 
		{ //国外定价: $
			shopItems.Add (new ShopItemData ("4.99",ShopItemEnum.MonthCard,1,GameCurrencyAssets.PID_MONTH_CARD, GetDiscutPercent(101)));
			shopItems.Add (new ShopItemData ("1.99",ShopItemEnum.WeekCard,1,GameCurrencyAssets.PID_WEEK_CARD, GetDiscutPercent(102)));

			shopItems.Add (new ShopItemData ("1.99",ShopItemEnum.Stone,120,GameCurrencyAssets.PID_CHIP_PACK1, GetDiscutPercent(1) ));
			shopItems.Add (new ShopItemData ("4.99",ShopItemEnum.Stone,300,GameCurrencyAssets.PID_CHIP_PACK2, GetDiscutPercent(2) ));
			shopItems.Add (new ShopItemData ("9.99",ShopItemEnum.Stone,600,GameCurrencyAssets.PID_CHIP_PACK3, GetDiscutPercent(3) ));
			shopItems.Add (new ShopItemData ("19.99",ShopItemEnum.Stone,1200,GameCurrencyAssets.PID_CHIP_PACK4, GetDiscutPercent(4)));
			shopItems.Add (new ShopItemData ("49.99",ShopItemEnum.Stone,3000,GameCurrencyAssets.PID_CHIP_PACK5, GetDiscutPercent(5)));
			shopItems.Add (new ShopItemData ("99.99",ShopItemEnum.Stone,6000,GameCurrencyAssets.PID_CHIP_PACK6, GetDiscutPercent(6)));
		} 
#endif

		dragPanel = new DragPanel("ShopDragPanel", ShopItem.Prefab,windowRoot.transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem (shopItems.Count);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			ShopItem fuv = ShopItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Data = shopItems[ i ];
//			fuv.callback = ClickItem;
		}
	}

	private void ClickButton( GameObject button) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
	}

    private void OnClickFriendExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoFriendExpansion", null);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ShopModule, "DoFriendExpansion");
    }

    private void OnClickStaminaRecover( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoStaminaRecover", null);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ShopModule, "DoStaminaRecover");
    }

    private void OnClickUnitExpansion( GameObject button ){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DoUnitExpansion", null);
//        ExcuteCallback(cbdArgs);
		ModuleManager.SendMessage (ModuleEnum.ShopModule, "DoUnitExpansion");
    }

	private void ShowUIAnimation(){
		infoPanelRoot.transform.localPosition = new Vector3(-1000, -287, 0);
		windowRoot.transform.localPosition = new Vector3(1000, -630, 0);
		iTween.MoveTo(infoPanelRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
		iTween.MoveTo(windowRoot, iTween.Hash("x", 0, "time", 0.4f, "islocal", true));
	}
	
}
