using UnityEngine;
using System.Collections.Generic;

public class OnSaleUnitsView : UIComponentUnity{
	DragPanel dragPanel;
	GameObject itemPrefab;
	bool exchange = false;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<string> crossShowTextList = new List<string>();
	List<UnitItemViewInfo> viewInfoList = new List<UnitItemViewInfo>();
	List<GameObject> topItemList = new List<GameObject>();
	UIImageButton imgBtn;
	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUIElement();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		imgBtn.isEnabled = false;
		ShowUIAnimation();	              
	}
	
	public override void HideUI(){
		base.HideUI();
		ResetUI();
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
			default:
				break;
		}
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
		Debug.LogError("AddViewItem(), position is : " + poolPos);
		FindTextureWithPosition(poolPos).mainTexture = info["texture"] as Texture2D;
		FindLabelWithPosition(poolPos).text = "Lv: " + info["label"] as string;

		MarkDragItem(clickPos, poolPos);
	}

	void RmvViewItem(object args){
		Dictionary<string, int> info = args as Dictionary<string, int>;
		int poolPos = (int)info["poolPos"];
		int clickPos = (int)info["clickPos"];
		FindTextureWithPosition(poolPos).mainTexture = null;
		FindLabelWithPosition(poolPos).text = string.Empty;

		CancelMarkDragItem(clickPos);
	}


	UITexture FindTextureWithPosition(int position){
		return topItemList[ position ].transform.FindChild("Texture").GetComponent<UITexture>();
	}

	UILabel FindLabelWithPosition(int position){
		return topItemList[ position].transform.FindChild("Label_Right_Bottom").GetComponent<UILabel>();
	}
	
	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, 267, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));  
	}

	void InitUIElement(){
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		itemPrefab = Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		InitTopItem();
		imgBtn = transform.FindChild("ImgBtn_Sell").GetComponent<UIImageButton>();
	}

	void InitTopItem(){
		for (int i = 0; i < 12; i++){
			GameObject go = transform.FindChild("Cells/" + i.ToString()).gameObject;
			topItemList.Add(go);
		}
		Debug.LogError("InitTopItem, count is : " + topItemList.Count);
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
		}
	}

	void ClickItem(GameObject item){
		int itemIndex = dragPanel.ScrollItem.IndexOf(item);
		Debug.LogError("OnSaleUnitsView.ClickItem(), item index is : " + itemIndex);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", itemIndex);
		ExcuteCallback(cbdArgs);
	}

	void DestoryDragView(object args){
		crossShowLabelList.Clear();
		dragPanel.DestoryUI();
	}

	void ResetUI(){


		for (int i = 0; i < 12; i++){
			FindTextureWithPosition(i).mainTexture = null;
			FindLabelWithPosition(i).text = string.Empty;
		}
	}

	void ActivateButton(object args){
		bool canActivate = (bool)args;
		imgBtn.isEnabled = canActivate;
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

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",			transform);
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
