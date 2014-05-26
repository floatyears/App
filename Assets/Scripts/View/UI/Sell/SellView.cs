using UnityEngine;
using System.Collections.Generic;

public class SellView : UIComponentUnity{
	private int totalSaleValue = 0;
	private int maxItemCount = 12;
	private DragPanel dragPanel;
	private GameObject mainRoot;
	private GameObject submitRoot;
	private SortRule curSortRule;
	private UIButton lastSureOkBtn;
	private UIButton lastSureCancelBtn;
	private UIImageButton sellImgBtn;
	private UIImageButton clearImgBtn;
	private UILabel coinLabel;
	private UILabel readyCoinLabel;

	private List<SellUnitItem> saleUnitViewList = new List<SellUnitItem>();
	private List<SellUnitItem> pickUnitViewList = new List<SellUnitItem>();
	private List<GameObject> pickItemList = new List<GameObject>();
	private List<GameObject> readyItemList = new List<GameObject>();
	
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		AddCmdListener();
		totalSaleValue = 0;
		pickUnitViewList.Clear();
		SortUnitByCurRule();
		RefreshOwnedUnitCount();
		ShowUIAnimation();	
	}
	
	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "DestoryDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
				break;
			case "ShowLastSureWindow" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowLastSureWindow, cbdArgs);
				break;
			case "BackToMainWindow" : 
				CallBackDispatcherHelper.DispatchCallBack(BackToMainWindow, cbdArgs);
				break;
			default:
				break;
		}
	}

    public override void ResetUIState() {
        SellController controller = origin as SellController;
        if (controller != null){
            controller.ResetUI();
        }
        ResetUIElement();
        sellImgBtn.isEnabled = false;
        clearImgBtn.isEnabled = false;
        coinLabel.text = "0";
    }
    
    void BackToMainWindow(object args){
		mainRoot.SetActive(true);
		submitRoot.SetActive(false);
		ShowUIAnimation();
	}

	void ChangeTotalSaleValue(int value){
		totalSaleValue += value;
		UpdateCoinLabel(totalSaleValue);
	}

	void ShowLastSureWindow(object args){
		mainRoot.SetActive(false);
		submitRoot.SetActive(true);
		submitRoot.transform.localPosition = new Vector3(-1000, -215, 0);
		iTween.MoveTo(submitRoot, iTween.Hash("x", 0, "time", 0.4f));

		List<TUserUnit> readySaleList = args as List<TUserUnit>;
		FillLastSureWindow(readySaleList);
	}

	void FillLastSureWindow(List<TUserUnit> dataInfoList){
		for (int i = 0; i < dataInfoList.Count; i++){
			Texture2D tex2d = dataInfoList[ i ].UnitInfo.GetAsset(UnitAssetType.Avatar);
			string level = dataInfoList[ i ].Level.ToString();
			FindTextureWithPosition( i, readyItemList).mainTexture = tex2d ;
			FindLabelWithPosition(i, readyItemList).text = "Lv: " + level;
		}
	}

	void ClickSellOk(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSellOk", null);
		ExcuteCallback(cbdArgs);
	}

	void ClickSellCancel(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSellCancel", null);
		ExcuteCallback(cbdArgs);
	}

	void UpdateCoinLabel(int coin){
		coinLabel.text = coin.ToString();
		readyCoinLabel.text = coin.ToString();
	}

	void MarkDragItem(int clickPos ,int poolPos){
		GameObject targetScrollItem = dragPanel.ScrollItem[ clickPos ];
		targetScrollItem.transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = true;
		targetScrollItem.transform.FindChild("Sprite_Mask").GetComponent<UISprite>().enabled = true;
		targetScrollItem.transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = (poolPos + 1).ToString();
	}

	void CancelMarkDragItem(int clickPos){
		GameObject targetScrollItem = dragPanel.ScrollItem[ clickPos ];
		targetScrollItem.transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = false;
		targetScrollItem.transform.FindChild("Sprite_Mask").GetComponent<UISprite>().enabled = false;
		targetScrollItem.transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = string.Empty;
	}

	
	void AddViewItem(object args){
		Dictionary<string, object> info = args as Dictionary<string,object>;
		int poolPos = (int)info["poolPos"];
		int clickPos = (int)info["clickPos"];
//		FindTextureWithPosition(poolPos, pickItemList).mainTexture = info["texture"] as Texture2D;
//		FindLabelWithPosition(poolPos, pickItemList).text = "Lv: " + info["label"] as string;
		GameObject targetItem = pickItemList[ poolPos ];

		UITexture targetItemTex = targetItem.GetComponentInChildren<UITexture>();
		targetItemTex.mainTexture = info["texture"] as Texture2D;

		UISprite targetItemBg = targetItem.transform.FindChild("Background").GetComponent<UISprite>();
		targetItemBg.spriteName = info["background"] as string;

		UISprite targetItemBorder = targetItem.transform.FindChild("Sprite_Avatar_Border").GetComponent<UISprite>();
		targetItemBorder.spriteName = info["border"] as string;

		MarkDragItem(clickPos, poolPos);
	}

	void RmvViewItem(object args){
		Dictionary<string, int> info = args as Dictionary<string, int>;
		int poolPos = (int)info["poolPos"];
		int clickPos = (int)info["clickPos"];
		FindTextureWithPosition(poolPos, pickItemList).mainTexture = null;
		FindLabelWithPosition(poolPos, pickItemList).text = string.Empty;
		CancelMarkDragItem(clickPos);
	}
	
	UITexture FindTextureWithPosition(int position, List<GameObject> target){
		return target[ position ].transform.FindChild("Texture").GetComponent<UITexture>();
	}

	UILabel FindLabelWithPosition(int position, List<GameObject> target){
		return target[ position ].transform.FindChild("Label_Right_Bottom").GetComponent<UILabel>();
	}
	
	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, -285, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  
	}

	void InitUIElement(){
		mainRoot = transform.Find("MainWindow").gameObject;
		submitRoot = transform.Find("EnsureWindow").gameObject;
		coinLabel = transform.FindChild("MainWindow/SellCount/Label_Value").GetComponent<UILabel>();
		readyCoinLabel = transform.FindChild("EnsureWindow/Label_GetCoinValue").GetComponent<UILabel>();
		sellImgBtn = transform.FindChild("MainWindow/ImgBtn_Sell").GetComponent<UIImageButton>();
		clearImgBtn = transform.FindChild("MainWindow/ImgBtn_Clear").GetComponent<UIImageButton>();
		lastSureCancelBtn = FindChild<UIButton>("EnsureWindow/Button_Cancel");
		lastSureOkBtn = FindChild<UIButton>("EnsureWindow/Button_Ok");
		UIEventListener.Get(sellImgBtn.gameObject).onClick = ClickSellBtn;
		UIEventListener.Get(clearImgBtn.gameObject).onClick = ClickClearBtn;
		UIEventListener.Get(lastSureOkBtn.gameObject).onClick = ClickSellOk;
		UIEventListener.Get(lastSureCancelBtn.gameObject).onClick = ClickSellCancel;
		InitCells();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
	}

	void ClickSellBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		List<TUserUnit> temp = new List<TUserUnit>();
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			TUserUnit tuu = pickUnitViewList[ i ].UserUnit;
			if(tuu == null)
				continue;

			temp.Add(tuu);
		}
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSell", temp);
		ExcuteCallback(cbdArgs);
	}

	void ClickClearBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ResetUIElement();
		for (int i = 0; i < saleUnitViewList.Count; i++){
			SellUnitItem sui =  saleUnitViewList[ i ];
			for (int j = 0; j < pickUnitViewList.Count; j++) {
				if(sui.Equals(pickUnitViewList[ j ])){
					CancelMarkDragItem( i );
				}
			}
		}
		//clear data
		pickUnitViewList.Clear();
	}

	void InitCells(){
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
	}

	void CreateDragView(object args){
		for (int i = 0; i < saleUnitViewList.Count; i++) {
			if(saleUnitViewList[ i ].gameObject !=null) {
				Destroy( saleUnitViewList[ i ].gameObject);
			}
		}
		saleUnitViewList.Clear();
		List<TUserUnit> dataList = args as List<TUserUnit>;
		dragPanel = new DragPanel("OnSaleDragPanel", SellUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.OnSaleUnitDragPanelArgs, mainRoot.transform);
		for(int i = 0; i< dragPanel.ScrollItem.Count; i++) {
			SellUnitItem suv = SellUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			suv.Init(dataList[ i ]);
			suv.callback = ClickItem;
			saleUnitViewList.Add(suv);
		}
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

	void ClickItem(SellUnitItem item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int clickPos = saleUnitViewList.IndexOf(item);
		int poolPos = 0;
		int index = CheckHaveBeenPicked(item);
		if(index == -1) {//not exist and add to picked list
			index = SearchFirstEmptyPosition(item);
				if(index == -1) {//conditin 1 : not exist empty slot in current picked list and add in the end of the list
					poolPos = pickUnitViewList.Count;
					pickUnitViewList.Add(item);
				}
			else{//conditin 2 : exist empty slot and insert it
					poolPos = index;
					pickUnitViewList[index] = item;
				}
			ChangeTotalSaleValue(item.UserUnit.UnitInfo.SaleValue);
			Dictionary<string,object> temp = new Dictionary<string, object>();
			temp.Add("poolPos", poolPos);
			temp.Add("clickPos", clickPos);
			Texture2D tex = item.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
			temp.Add("texture", tex);
			string sprName = item.UserUnit.UnitInfo.GetUnitBackgroundName();
			temp.Add("background", sprName);
			sprName = item.UserUnit.UnitInfo.GetUnitBorderSprName();
			temp.Add("border", sprName);
			temp.Add("label", item.UserUnit.Level.ToString());
			AddViewItem(temp);
		}
		else{//already exist, treat current click as "cancel sell this unit"
			poolPos = index;
			SellUnitItem suv = pickUnitViewList[ index ];
			pickUnitViewList[ index ] = null;
			Dictionary<string, int> temp = new Dictionary<string, int>();
			temp.Add("poolPos", poolPos);
			temp.Add("clickPos", clickPos);
			RmvViewItem(temp);
			ChangeTotalSaleValue(-item.UserUnit.UnitInfo.SaleValue);
		}
		ActivateButton();
	}

	void DestoryDragView(object args){
        if (dragPanel != null){
            dragPanel.DestoryUI();
        }
	}

	void ResetUIElement(){
		clearImgBtn.isEnabled = false;
		sellImgBtn.isEnabled = false;
		totalSaleValue = 0;
		coinLabel.text = string.Empty;


		for (int i = 0; i < maxItemCount; i++){
			FindTextureWithPosition(i, pickItemList).mainTexture = null;
			FindLabelWithPosition(i, pickItemList).text = string.Empty;

			FindTextureWithPosition(i, readyItemList).mainTexture = null;
			FindLabelWithPosition(i, readyItemList).text = string.Empty;
		}

		mainRoot.SetActive(true);
		submitRoot.SetActive(false);
	}

	void ActivateButton(){
		bool canActivate = CanActivateSellBtn();
		sellImgBtn.isEnabled = canActivate;
		clearImgBtn.isEnabled = canActivate;
	}

	bool CanActivateSellBtn(){
		bool canActivate = false;
		if(GetCurPickedUnitCount() > 0)	return true;
		else
			return false;
	}

	int GetCurPickedUnitCount(){
		int pickedCount = 0;
		for (int i = 0; i < pickItemList.Count; i++){
			if(pickItemList[ i ] !=null) 
				pickedCount++;
		}
		return pickedCount;
	}

	private void ReceiveSortInfo(object msg){
		//curSortRule = SortUnitTool.GetNextRule(curSortRule);
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		List<TUserUnit> unitList = new List<TUserUnit>();
		for (int i = 0; i < saleUnitViewList.Count; i++){
			unitList.Add(saleUnitViewList[ i ].UserUnit);
		}
		
		SortUnitTool.SortByTargetRule(curSortRule, unitList);	
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			SellUnitItem suv = dragPanel.ScrollItem[ i ].GetComponent<SellUnitItem>();
			suv.UserUnit = unitList[ i ];
			suv.CurrentSortRule = curSortRule;
		}
	}

	void RefreshOwnedUnitCount(){
		Dictionary<string, object> countArgs = new Dictionary<string, object>();
		countArgs.Add("title", TextCenter.Instace.GetCurrentText("UnitCounterTitle"));
		countArgs.Add("current", DataCenter.Instance.UserUnitList.GetAllMyUnit().Count);
		countArgs.Add("max", DataCenter.Instance.UserInfo.UnitMax);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshItemCount, countArgs);
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}
        
}
