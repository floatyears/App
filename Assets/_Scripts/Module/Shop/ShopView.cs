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

	private void CreateDragView(){

		List<ShopItemData> friendOutDataList = new List<ShopItemData> ();

		friendOutDataList.Add (new ShopItemData ("4.99",ShopItemEnum.MonthCard,1,GameCurrencyAssets.PID_MONTH_CARD,""));
		friendOutDataList.Add (new ShopItemData ("1.99",ShopItemEnum.WeekCard,1,GameCurrencyAssets.PID_WEEK_CARD,""));
		if (DataCenter.Instance.AccountInfo.PayTotal == 0) {
			friendOutDataList.Add (new ShopItemData ("1.99",ShopItemEnum.Stone,120,GameCurrencyAssets.PID_CHIP_PACK1,"100%"));
			friendOutDataList.Add (new ShopItemData ("4.99",ShopItemEnum.Stone,300,GameCurrencyAssets.PID_CHIP_PACK2,"100%"));
			friendOutDataList.Add (new ShopItemData ("9.99",ShopItemEnum.Stone,600,GameCurrencyAssets.PID_CHIP_PACK3,"100%"));
			friendOutDataList.Add (new ShopItemData ("19.99",ShopItemEnum.Stone,1200,GameCurrencyAssets.PID_CHIP_PACK4,"100%"));
			friendOutDataList.Add (new ShopItemData ("49.99",ShopItemEnum.Stone,3000,GameCurrencyAssets.PID_CHIP_PACK5,"100%"));
			friendOutDataList.Add (new ShopItemData ("99.99",ShopItemEnum.Stone,6000,GameCurrencyAssets.PID_CHIP_PACK6,"100%"));
		} else {
			friendOutDataList.Add (new ShopItemData ("1.99",ShopItemEnum.Stone,120,GameCurrencyAssets.PID_CHIP_PACK1,"0"));
			friendOutDataList.Add (new ShopItemData ("4.99",ShopItemEnum.Stone,300,GameCurrencyAssets.PID_CHIP_PACK2,"10%"));
			friendOutDataList.Add (new ShopItemData ("9.99",ShopItemEnum.Stone,600,GameCurrencyAssets.PID_CHIP_PACK3,"20%"));
			friendOutDataList.Add (new ShopItemData ("19.99",ShopItemEnum.Stone,1200,GameCurrencyAssets.PID_CHIP_PACK4,"30%"));
			friendOutDataList.Add (new ShopItemData ("49.99",ShopItemEnum.Stone,3000,GameCurrencyAssets.PID_CHIP_PACK5,"40%"));
			friendOutDataList.Add (new ShopItemData ("99.99",ShopItemEnum.Stone,6000,GameCurrencyAssets.PID_CHIP_PACK6,"50%"));
		}


		dragPanel = new DragPanel("ShopDragPanel", ShopItem.Prefab,windowRoot.transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem (friendOutDataList.Count);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			ShopItem fuv = ShopItem.Inject(dragPanel.ScrollItem[ i ]);
			fuv.Data = friendOutDataList[ i ];
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
