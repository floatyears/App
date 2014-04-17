using UnityEngine;
using System.Collections.Generic;

public class OnSaleUnitsView : UIComponentUnity{
	DragPanel dragPanel;
	GameObject mainRoot;
	GameObject submitRoot;
	UIButton sortButton;
	private UILabel sortRuleLabel;
	private SortRule curSortRule;
	UIButton lastSureOkButton;
	UIButton lastSureCancelButton;
	UIImageButton sellImgBtn;
	UIImageButton clearImgBtn;
	UILabel coinLabel;
	UILabel readyCoinLabel;
	int maxItemCount = 12;
	int totalSaleValue = 0;
	List<SaleUnitView> saleUnitViewList = new List<SaleUnitView>();
	List<SaleUnitView> pickUnitViewList = new List<SaleUnitView>();
	List<GameObject> pickItemList = new List<GameObject>();
	List<GameObject> readyItemList = new List<GameObject>();
	
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		ShowUIAnimation();	   
		SortUnitByCurRule();
	}
	
	public override void HideUI(){
		base.HideUI();
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
        OnSaleUnitsController controller = origin as OnSaleUnitsController;
        if (controller != null){
            //Debug.LogError(controller);
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
		//	Debug.LogError("ChangeTotalSaleValue(), before TotalValue is " + totalSaleValue);
		totalSaleValue += value;
		//Debug.LogError("ChangeTotalSaleValue(), after TotalValue is " + totalSaleValue);
		UpdateCoinLabel(totalSaleValue);
	}

	void ShowLastSureWindow(object args){
//		Debug.LogError("ShowLastSureWindow().....");
		mainRoot.SetActive(false);
		submitRoot.SetActive(true);
		submitRoot.transform.localPosition = new Vector3(-1000, -200, 0);
		iTween.MoveTo(submitRoot, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));

		List<TUserUnit> readySaleList = args as List<TUserUnit>;
		FillLastSureWindow(readySaleList);
	}

	void FillLastSureWindow(List<TUserUnit> dataInfoList){
//		Debug.LogError("dataInfoList.Count : " + dataInfoList.Count);
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
//		Debug.LogError("UpdateCoinLabel(), current coin is : " + coin.ToString());
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
//		Debug.LogError("AddViewItem(), position is : " + poolPos);
		FindTextureWithPosition(poolPos, pickItemList).mainTexture = info["texture"] as Texture2D;
		FindLabelWithPosition(poolPos, pickItemList).text = "Lv: " + info["label"] as string;

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
		transform.localPosition = new Vector3(-1000, 267, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));  
	}

	void InitUIElement(){
		mainRoot = transform.Find("MainWindow").gameObject;
		submitRoot = transform.Find("EnsureWindow").gameObject;
		coinLabel = transform.FindChild("MainWindow/SellCount/Label_Value").GetComponent<UILabel>();
		readyCoinLabel = transform.FindChild("EnsureWindow/Label_GetCoinValue").GetComponent<UILabel>();
		sellImgBtn = transform.FindChild("MainWindow/ImgBtn_Sell").GetComponent<UIImageButton>();
		clearImgBtn = transform.FindChild("MainWindow/ImgBtn_Clear").GetComponent<UIImageButton>();
		lastSureCancelButton = FindChild<UIButton>("EnsureWindow/Button_Cancel");
		lastSureOkButton = FindChild<UIButton>("EnsureWindow/Button_Ok");
		UIEventListener.Get(sellImgBtn.gameObject).onClick = ClickSellBtn;
		UIEventListener.Get(clearImgBtn.gameObject).onClick = ClickClearBtn;
		UIEventListener.Get(lastSureOkButton.gameObject).onClick = ClickSellOk;
		UIEventListener.Get(lastSureCancelButton.gameObject).onClick = ClickSellCancel;
//		InitDragPanelArgs();
		InitCells();

		sortButton = FindChild<UIButton>("MainWindow/SortButton");
		UIEventListener.Get(sortButton.gameObject).onClick = ClickSortButton;
		sortRuleLabel = sortButton.transform.FindChild("Label").GetComponent<UILabel>();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		sortRuleLabel.text = curSortRule.ToString();
	}

	void ClickSellBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		List<TUserUnit> temp = new List<TUserUnit>();
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			TUserUnit tuu = pickUnitViewList[i].UserUnit;
			if(tuu == null)
				continue;

			temp.Add(tuu);
		}
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSell", temp);
		ExcuteCallback(cbdArgs);
	}

	void ClickClearBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickClear", null);
		ExcuteCallback(cbdArgs);
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
//		Debug.Log(string.Format("PickedCount : {0}---ReadtCount : {1}  ........",pickItemList.Count, readyItemList.Count ));
	}

	void CreateDragView(object args){
		for (int i = 0; i < saleUnitViewList.Count; i++) {
			if(saleUnitViewList[i].gameObject !=null) {
				Destroy( saleUnitViewList[ i ].gameObject);
			}
		}
		saleUnitViewList.Clear();
		List<TUserUnit> dataList = args as List<TUserUnit>;
		dragPanel = new DragPanel("OnSaleDragPanel", SaleUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.OnSaleUnitDragPanelArgs, mainRoot.transform);
		for(int i = 0; i< dragPanel.ScrollItem.Count; i++) {
			SaleUnitView suv = SaleUnitView.Inject(dragPanel.ScrollItem[ i ]);
			suv.Init(dataList[ i ]);
			suv.callback = ClickItem;
			saleUnitViewList.Add(suv);
		}
	}

	int CheckExist(SaleUnitView item){
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			if(pickUnitViewList[i] != null && pickUnitViewList[i].Equals(item)) {
				return i;
			}
		}
		return -1;
	}

	int CheckNull(SaleUnitView item){
		for (int i = 0; i < pickUnitViewList.Count; i++) {
			if(pickUnitViewList[i] == null) {
				return i;
			}
		}
		return -1;
	}

	void ClickItem(SaleUnitView item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int clickPos = saleUnitViewList.IndexOf(item);
		int poolPos = 0;
		int index = CheckExist(item);
		if(index == -1) {
			index = CheckNull(item);
				if(index == -1) {
					poolPos = pickUnitViewList.Count;
					pickUnitViewList.Add(item);
				}
				else{
					poolPos = index;
					pickUnitViewList[index] = item;
				}
			ChangeTotalSaleValue(item.UserUnit.UnitInfo.SaleValue);
			Dictionary<string,object> temp = new Dictionary<string, object>();
			temp.Add("poolPos", poolPos);
			temp.Add("clickPos", clickPos);
			Texture2D tex = item.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
			temp.Add("texture", tex);
			temp.Add("label", item.UserUnit.Level.ToString());
			AddViewItem(temp);
		}
		else{
			poolPos = index;
			SaleUnitView suv = pickUnitViewList[ index ];
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

	void ClickSortButton(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();	
		List<TUserUnit> unitList = new List<TUserUnit>();
		for (int i = 0; i < saleUnitViewList.Count; i++){
			unitList.Add(saleUnitViewList[ i ].UserUnit);
		}
		
		SortUnitTool.SortByTargetRule(curSortRule, unitList);	
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			SaleUnitView suv = dragPanel.ScrollItem[ i ].GetComponent<SaleUnitView>();
			suv.UserUnit = unitList[ i ];
			suv.CurrentSortRule = curSortRule;
		}
	}

}
