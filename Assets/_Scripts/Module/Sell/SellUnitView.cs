using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class SellUnitView : ViewBase{
	private int totalSaleValue = 0;
	private int maxItemCount = 12;
	private DragPanel dragPanel;
	private GameObject mainRoot;
	private GameObject submitRoot;
	private SortRule curSortRule;
	private UIButton lastSureOkBtn;
	private UIButton lastSureCancelBtn;
	private UIButton sellBtn;
	private UIButton clearBtn;
	private UILabel coinLabel;
	private UILabel readyCoinLabel;

	private List<SellUnitItem> pickUnitViewList = new List<SellUnitItem>();
	private List<GameObject> pickItemList = new List<GameObject>();
	private List<GameObject> readyItemList = new List<GameObject>();
	List<UserUnit> pickedUnitList = new List<UserUnit>();
	
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config ,data);
		InitUIElement();

		dragPanel = new DragPanel("SellUnitDragPanel", "Prefabs/UI/UnitItem/SellUnitPrefab",typeof(SellUnitItem), mainRoot.transform);

	}
	
	public override void ShowUI(){
		base.ShowUI();

		ModuleManager.Instance.ShowModule (ModuleEnum.UnitSortModule,"from","sell_unit");
		ModuleManager.Instance.ShowModule (ModuleEnum.ItemCounterModule,"from","sell_unit");
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);

//		ResetUIState();
		totalSaleValue = 0;
		pickUnitViewList.Clear();
		RefreshOwnedUnitCount();

		ResetUIElement();
		dragPanel.SetData<UserUnit> (DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit(), ClickItem as DataListener);

//		ShowUIAnimation();	
	}
	
	public override void HideUI(){
		base.HideUI();

		ModuleManager.Instance.HideModule (ModuleEnum.UnitSortModule);
		ModuleManager.Instance.HideModule (ModuleEnum.ItemCounterModule);

		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

    Vector3 pos = Vector3.zero;
    void BackToMainWindow(bool IsRefresh) {
		mainRoot.transform.localPosition = pos;
		submitRoot.SetActive(false);
		ResetReadyPool();
		dragPanel.SetData<UserUnit> (DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit(), ClickItem as DataListener);
		dragPanel.ItemCallback ("cancel_all");
		if (IsRefresh) {
			ResetUIElement();		
		}
	}

	void ResetReadyPool() {
		for (int i = 0; i < readyItemList.Count; i++) {
			Transform trans = readyItemList[ i ].transform;
			UISprite bg = trans.FindChild("Background").GetComponent<UISprite>();
			UISprite border = trans.FindChild("Sprite_Frame_Out").GetComponent<UISprite>();
			UISprite avatar = trans.FindChild("Texture").GetComponent<UISprite>();
			UILabel levelLabel = trans.FindChild("Label_Right_Bottom").GetComponent<UILabel>();
			
			bg.spriteName = "unit_empty_bg";
			border.spriteName = "";
			avatar.spriteName = "";
			levelLabel.text = string.Empty;	
		}
	}

	void ChangeTotalSaleValue(int value){
		totalSaleValue += value;
		coinLabel.text = totalSaleValue.ToString();
		readyCoinLabel.text = TextCenter.GetText ("Sell_GetCoin") + ":" + totalSaleValue.ToString();
	}

	void ShowLastSureWindow(object args){
		mainRoot.transform.localPosition = new Vector3(0f,10000f,0f);
		submitRoot.SetActive(true);
		submitRoot.transform.localPosition = new Vector3(-1000, -215, 0);
		iTween.MoveTo(submitRoot, iTween.Hash("x", 0, "time", 0.4f));

		List<UserUnit> readySaleList = args as List<UserUnit>;
		for (int i = 0; i < readySaleList.Count; i++){
			ResourceManager.Instance.GetAvatarAtlas( readySaleList[ i ].UnitInfo.id, readyItemList[i].transform.FindChild("Texture").GetComponent<UISprite>() );
			
			string level = readySaleList[ i ].level.ToString();
			UISprite bgSpr = readyItemList[ i ].transform.FindChild("Background").GetComponent<UISprite>();
			UISprite borderSor = readyItemList[ i ].transform.FindChild("Sprite_Frame_Out").GetComponent<UISprite>();
			ShowUnitType(readySaleList[ i ], bgSpr, borderSor);
		}
	}
	
	void ClickSellOk(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		ModuleManager.SendMessage (ModuleEnum.SellUnitModule, "ClickSellOk");
		UnitController.Instance.SellUnit(OnRspSellUnit,GetOnSaleUnitIDList());
	}

	void ClickSellCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		BackToMainWindow (false);
	}

	protected override void ToggleAnimation (bool isShow)
	{
		base.ToggleAnimation (isShow);

		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
//			transform.localPosition = new Vector3(-1000, -285, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}
	}

	void InitUIElement(){
		mainRoot = transform.Find("MainWindow").gameObject;
		pos = mainRoot.transform.localPosition;
		submitRoot = transform.Find("EnsureWindow").gameObject;
		coinLabel = transform.FindChild("MainWindow/SellCount/Label_Value").GetComponent<UILabel>();
		readyCoinLabel = transform.FindChild("EnsureWindow/Label_GetCoinValue").GetComponent<UILabel>();

		sellBtn = transform.FindChild("MainWindow/Button_Sell").GetComponent<UIButton>();
		UILabel sellLabel = FindChild<UILabel>("MainWindow/Button_Sell/Label");
		sellLabel.text = TextCenter.GetText("Sell");

		clearBtn = transform.FindChild("MainWindow/Button_Clear").GetComponent<UIButton>();
		UILabel clearLabel = FindChild<UILabel>("MainWindow/Button_Clear/Label");
		clearLabel.text = TextCenter.GetText("Clear");

		lastSureCancelBtn = FindChild<UIButton>("EnsureWindow/Button_Cancel");
		lastSureOkBtn = FindChild<UIButton>("EnsureWindow/Button_Ok");
		UIEventListenerCustom.Get(sellBtn.gameObject).onClick = ClickSellBtn;
		UIEventListenerCustom.Get(clearBtn.gameObject).onClick = ClickClearBtn;
		UIEventListenerCustom.Get(lastSureOkBtn.gameObject).onClick = ClickSellOk;
		UIEventListenerCustom.Get(lastSureCancelBtn.gameObject).onClick = ClickSellCancel;

		for (int i = 0; i < maxItemCount; i++){
			string path;
			GameObject go;
			
			path = "MainWindow/Cells/" + i.ToString();
			go = transform.FindChild(path).gameObject;
			pickItemList.Add(go);
			
			path = "EnsureWindow/Cells/" + i.ToString();
			go = transform.FindChild(path).gameObject;
			readyItemList.Add(go);
		}

		curSortRule = SortUnitTool.GetSortRule (SortRuleByUI.SellView);//DEFAULT_SORT_RULE;

		FindChild ("MainWindow/Button_Clear/Label").GetComponent<UILabel>().text = TextCenter.GetText ("Btn_Clear_PickedToSell");
		FindChild ("MainWindow/Button_Sell/Label").GetComponent<UILabel>().text = TextCenter.GetText ("Btn_Submit_Sell");
		FindChild ("MainWindow/SellCount/Label_Text").GetComponent<UILabel>().text = TextCenter.GetText ("Text_Sell_Price");

		FindChild ("EnsureWindow/Label_Title").GetComponent<UILabel>().text = TextCenter.GetText ("Sell_EnsureTitle");
		FindChild ("EnsureWindow/Label_Content").GetComponent<UILabel>().text = TextCenter.GetText ("Sell_EnsureContent");
		FindChild ("EnsureWindow/Label_Ensure").GetComponent<UILabel>().text = TextCenter.GetText ("Sell_Ensure");
//		FindChild ("EnsureWindow/Label_GetCoin").GetComponent<UILabel>().text = TextCenter.GetText ("Sell_GetCoin");

		FindChild ("EnsureWindow/Button_Ok/Label").GetComponent<UILabel>().text = TextCenter.GetText ("OK");
		FindChild ("EnsureWindow/Button_Cancel/Label").GetComponent<UILabel>().text = TextCenter.GetText ("CANCEL");
	}

	void ClickSellBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			SellUnitItem sellUnitItem = pickUnitViewList[ i ];
			if(sellUnitItem == null)
				continue;
			pickedUnitList.Add(sellUnitItem.UserUnit);
//			sellUnitItem.
		}
		if (CheckPickedUnitRare ()) {
			TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("BigRareWarning"), TextCenter.GetText ("BigRareWarningText"), TextCenter.GetText ("OK"), TextCenter.GetText ("CANCEL"), o=>{
				ShowLastSureWindow(pickedUnitList);
			});	
		}
		else {
			ShowLastSureWindow(pickedUnitList);
		}
	}

	void ClickClearBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ResetUIElement();
//		dragPanel.Clear ();
		pickedUnitList.Clear ();
		dragPanel.ItemCallback ("cancel_all");
		//clear data
	}
	
	int CheckHaveBeenPicked(SellUnitItem item){
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			if(pickUnitViewList[ i ] != null && pickUnitViewList[ i ].Equals(item)) {
				return i;
			}
		}
		return -1;
	}

	int SearchFirstEmptyPosition(SellUnitItem item){
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			if(pickUnitViewList[ i ] == null) {
				return i;
			}
		}
		return -1;
	}

	void ClickItem(object data){
		SellUnitItem item = data as SellUnitItem;
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

//		int clickPos = 0;//dataList.IndexOf(item);
		int poolPos = 0;
		int index = CheckHaveBeenPicked(item);
		if(index == -1) {
			//not exist and add to picked list
			index = SearchFirstEmptyPosition(item);
			if(index == -1) {
				if (pickUnitViewList.Count >= 12)
					return;
			//conditin 1 : not exist empty slot in current picked list and add in the end of the list
				poolPos = pickUnitViewList.Count;
				pickUnitViewList.Add(item);
			} else{
			//conditin 2 : exist empty slot and insert it
				poolPos = index;
				pickUnitViewList[index] = item;
			}

			ChangeTotalSaleValue(item.UserUnit.UnitInfo.saleValue);

			UISprite sprite = pickItemList[ poolPos ].transform.Find("Texture").GetComponent<UISprite>();
			ResourceManager.Instance.GetAvatarAtlas(item.UserUnit.UnitInfo.id, sprite, returnValue => {
				GameObject targetItem = pickItemList[ poolPos ];
				
				targetItem.transform.FindChild("Background").GetComponent<UISprite>().spriteName = item.UserUnit.UnitInfo.GetUnitBackgroundName();
				
				targetItem.transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = item.UserUnit.UnitInfo.GetUnitBorderSprName();
				
				targetItem.transform.FindChild("Label_Right_Bottom").GetComponent<UILabel>().text = item.UserUnit.level.ToString();
				
				dragPanel.ItemCallback ("mark_item", item, poolPos);
				
				ActivateButton();
			});
			return;
		}
		else{
			poolPos = index;
			if(poolPos > 11)
				return;
			pickUnitViewList[ index ] = null;

			GameObject obj =  pickItemList[poolPos];
			
			obj.transform.FindChild("Texture").GetComponent<UISprite>().spriteName = "";
			
			obj.transform.FindChild("Label_Right_Bottom").GetComponent<UILabel>().text = string.Empty;
			
			obj.transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = "avatar_border_6";
			
			obj.transform.FindChild("Background").GetComponent<UISprite>().spriteName = "unit_empty_bg";
			
			dragPanel.ItemCallback ("cancel_mark", item);

			ChangeTotalSaleValue(-item.UserUnit.UnitInfo.saleValue);
		}
		ActivateButton();
	}

	void DestoryDragView(object args){
        if (dragPanel != null){
            dragPanel.DestoryUI();
        }
	}

	void ResetUIElement(){
//		Debug.LogError(clearBtn.name  + "     " + clearBtn.disabledSprite);
		clearBtn.isEnabled = false;
		sellBtn.isEnabled = false;

		mainRoot.SetActive(true);
		submitRoot.SetActive(false);

		totalSaleValue = 0;
		coinLabel.text = "0";

		for (int i = 0; i < maxItemCount; i++){
			pickItemList[i].transform.FindChild("Texture").GetComponent<UISprite>().spriteName = "";
			pickItemList[ i ].transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>().spriteName = "avatar_border_6";
			pickItemList[ i ].transform.FindChild("Background").GetComponent<UISprite>().spriteName = "unit_empty_bg";

			readyItemList[i].transform.FindChild("Texture").GetComponent<UISprite>().spriteName = "";
			readyItemList[i].transform.FindChild("Label_Right_Bottom").GetComponent<UILabel>().text = string.Empty;
		}

		pickUnitViewList.Clear ();

		ResetReadyPool();

	}

	private void ActivateButton(){
		int pickedCount = 0;
		for (int i = 0; i < pickUnitViewList.Count; i++){
			if(pickUnitViewList[ i ] !=null) 
				pickedCount++;
		}
		clearBtn.isEnabled = sellBtn.isEnabled = pickedCount > 0 ? true : false;
	}

	private void ReceiveSortInfo(object msg){
		//curSortRule = SortUnitTool.GetNextRule(curSortRule);
		curSortRule = (SortRule)msg;
		SortUnitTool.StoreSortRule (curSortRule, SortRuleByUI.SellView);
		
		List<UserUnit> unitList = new List<UserUnit>();
		//		for (int i = 0; i < saleUnitViewList.Count; i++){
		//			unitList.Add(saleUnitViewList[ i ].UserUnit);
		//		}
		
		SortUnitTool.SortByTargetRule(curSortRule, unitList);	
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			SellUnitItem suv = dragPanel.ScrollItem[ i ].GetComponent<SellUnitItem>();
			suv.SetData<UserUnit>(unitList[ i ]);
		}
	}

	void RefreshOwnedUnitCount(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.GetText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserData.UserInfo.unitMax);
		countArgs.Add ("posy", -705);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void ShowUnitType(UserUnit tuu, UISprite avatarBg, UISprite avatarBorderSpr){
		switch (tuu.UnitInfo.type){
			case EUnitType.UFIRE :
				avatarBg.spriteName = "avatar_bg_fire";
				avatarBorderSpr.spriteName = "avatar_border_fire";
				break;
			case EUnitType.UWATER :
				avatarBg.spriteName = "avatar_bg_water";
				avatarBorderSpr.spriteName = "avatar_border_water";
				
				break;
			case EUnitType.UWIND :
				avatarBg.spriteName = "avatar_bg_wind";
				avatarBorderSpr.spriteName = "avatar_border_wind";
				
				break;
			case EUnitType.ULIGHT :
				avatarBg.spriteName = "avatar_bg_light";
				avatarBorderSpr.spriteName = "avatar_border_light";
				
				break;
			case EUnitType.UDARK :
				avatarBg.spriteName = "avatar_bg_dark";
				avatarBorderSpr.spriteName = "avatar_border_dark";
				
				break;
			case EUnitType.UNONE :
				avatarBg.spriteName = "avatar_bg_none";
				avatarBorderSpr.spriteName = "avatar_border_none";
				
				break;
			default:
				break;
		}
	}

	bool CheckPickedUnitRare(){
		bool noteSignal = false;
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] == null) continue;
			if(pickedUnitList[ i ].UnitInfo.rare >= 3){
				noteSignal = true;
				break;
			}
		}
		return noteSignal;
	}

	
	List<uint> GetOnSaleUnitIDList(){
		List<uint> idList = new List<uint>();
		for (int i = 0; i < pickedUnitList.Count; i++){
			
			if(pickedUnitList[ i ] != null)
				idList.Add(pickedUnitList[ i ].uniqueId);
		}
		return idList;
	}

	
	List<UserUnit> GetReadySaleUnitList(){
		List<UserUnit> readySaleList = new List<UserUnit>();
		for (int i = 0; i < pickedUnitList.Count; i++){
			if(pickedUnitList[ i ] != null)
				readySaleList.Add(pickedUnitList[ i ]);
		}
		return readySaleList;
	}

	private void OnRspSellUnit(object data) {
		if (data == null)
			return;
		
		bbproto.RspSellUnit rsp = data as bbproto.RspSellUnit;
		
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}
		
		int money = rsp.money;
		int gotMoney = rsp.gotMoney;
		List<UserUnit> unitList = rsp.unitList;
		
		DataCenter.Instance.UserData.AccountInfo.money = rsp.money;
		
		DataCenter.Instance.UnitData.UserUnitList.DelMyUnitList(GetOnSaleUnitIDList());
		
		pickedUnitList.Clear();
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPlayerCoin, null);
		//		base.HideUI ();
		////		SellView view = view as SellView;
		////		view.ResetUIState();
		//		base.ShowUI ();
		
		BackToMainWindow(true);

		AudioManager.Instance.PlayAudio(AudioEnum.sound_sold_out);
	}

  
}
