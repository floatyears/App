using UnityEngine;
using System.Collections.Generic;

public class OnSaleUnitsView : UIComponentUnity{
	DragPanel dragPanel;
	GameObject itemPrefab;
	bool exchange = false;
	GameObject mainRoot;
	GameObject submitRoot;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<string> crossShowTextList = new List<string>();
	List<UnitItemViewInfo> viewInfoList = new List<UnitItemViewInfo>();
	List<GameObject> pickItemList = new List<GameObject>();
	List<GameObject> readyItemList = new List<GameObject>();

	UIButton lastSureOkButton;
	UIButton lastSureCancelButton;
	UIImageButton sellImgBtn;
	UIImageButton clearImgBtn;
	UILabel coinLabel;
	UILabel readyCoinLabel;
	int maxItemCount = 12;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		ShowUIAnimation();	              
	}
	
	public override void HideUI(){
		base.HideUI();
//		ResetUIElement();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "DestoryDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
				break;
			case "AddViewItem" : 
				CallBackDispatcherHelper.DispatchCallBack(AddViewItem, cbdArgs);
				break;
			case "RmvViewItem" : 
				CallBackDispatcherHelper.DispatchCallBack(RmvViewItem, cbdArgs);
				break;
			case "ButtonActive":
				CallBackDispatcherHelper.DispatchCallBack(ActivateButton, cbdArgs);
				break;
			case "UpdateCoinLabel" : 
				CallBackDispatcherHelper.DispatchCallBack(UpdateCoinLabel, cbdArgs);
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
            Debug.LogError(controller);
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

	void UpdateCoinLabel(object args){
		int coin = (int)args;
//		Debug.LogError("UpdateCoinLabel(), current coin is : " + coin.ToString());
		coinLabel.text = coin.ToString();
		readyCoinLabel.text = coin.ToString();
	}

	void MarkDragItem(int clickPos ,int poolPos){
		GameObject targetScrollItem = dragPanel.ScrollItem[ clickPos ];
		targetScrollItem.transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = true;
		targetScrollItem.transform.FindChild("Mask").GetComponent<UISprite>().enabled = true;
		targetScrollItem.transform.FindChild("Label_TopRight").GetComponent<UILabel>().text = (poolPos + 1).ToString();
	}

	void CancelMarkDragItem(int clickPos){
		GameObject targetScrollItem = dragPanel.ScrollItem[ clickPos ];
		targetScrollItem.transform.FindChild("Sprite_Clycle").GetComponent<UISprite>().enabled = false;
		targetScrollItem.transform.FindChild("Mask").GetComponent<UISprite>().enabled = false;
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
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		itemPrefab = Resources.Load( itemSourcePath ) as GameObject;
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
		InitDragPanelArgs();
		InitCells();
	}

	void ClickSellBtn(GameObject btn){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickSell", null);
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
		List<UnitItemViewInfo> viewItemList = args as List<UnitItemViewInfo>;
		viewInfoList = viewItemList;
		int itemCount = viewItemList.Count;
		dragPanel = new DragPanel("OnSaleDragPanel", itemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(itemCount);
		FindCrossShowLabelList();
        AddEventLisenter();
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
		RefreshItemView(viewItemList);
		UpdateCrossShow();
	}

	void RefreshItemView(List<UnitItemViewInfo> dataItemList){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];

			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
			uiTexture.mainTexture = dataItemList[ i ].Avatar;

			UISprite maskSpr = scrollItem.transform.FindChild("Mask").GetComponent<UISprite>();
			UILabel partyLabel = scrollItem.transform.FindChild("Label_Party").GetComponent<UILabel>();
			if(dataItemList[ i ].IsParty){
				maskSpr.enabled = true;
				partyLabel.text = "Party";
				partyLabel.color = Color.red;
			}
			else{
				maskSpr.enabled = false;
			}
		}
	}
	
	void FindCrossShowLabelList(){
		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
			crossShowLabelList.Add(label);
		}
	}

	void UpdateCrossShow(){
		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow", 0f, 1f);
	}

	void AddEventLisenter(){
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject item = dragPanel.ScrollItem[ i ];
			UIEventListener.Get(item).onClick = ClickItem;
			UIEventListenerCustom.Get(item).LongPress = PressItem;
		}
	}

	void ClickItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		int itemIndex = dragPanel.ScrollItem.IndexOf(item);
		//Debug.LogError("OnSaleUnitsView.ClickItem(), item index is : " + itemIndex);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", itemIndex);
		ExcuteCallback(cbdArgs);
	}

	void DestoryDragView(object args){
		crossShowLabelList.Clear();
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

	void ActivateButton(object args){
		bool canActivate = (bool)args;
		sellImgBtn.isEnabled = canActivate;
		clearImgBtn.isEnabled = canActivate;
	}

	void CrossShow(){
		if(exchange){
			for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				crossShowLabelList[ i ].text = "Lv" + viewInfoList[ i ].CrossShowTextBefore;
				crossShowLabelList[ i ].color = Color.yellow;
			}
			exchange = false;
		}
		else{
			for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				crossShowLabelList[ i ].text = "+" + viewInfoList[ i ].CrossShowTextAfter;
				crossShowLabelList[ i ].color = Color.red;
			}
			exchange = true;
		}
	}

	void PressItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
		ExcuteCallback(cbdArgs);
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",			mainRoot.transform);
		dragPanelArgs.Add("scrollerScale",			Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",		-240 * Vector3.up);
		dragPanelArgs.Add("position", 					Vector3.zero);
		dragPanelArgs.Add("clipRange", 				new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 			UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",				3);
		dragPanelArgs.Add("scrollBarPosition",		new Vector3(-320, -340, 0));
		dragPanelArgs.Add("cellWidth", 				120);
		dragPanelArgs.Add("cellHeight",				120);
	}

}
