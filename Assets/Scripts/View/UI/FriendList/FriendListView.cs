using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendListView : UIComponentUnity{

	DragPanel dragPanel;
	GameObject unitItem;

	UIButton sortButton;
	UIButton updateFriendButton;
	UIButton refuseAllApplyButton;

	UILabel curCountLabel;
	UILabel maxCountLabel;

	bool exchange = false;
	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<UnitItemViewInfo> friendViewInfoList = new List<UnitItemViewInfo>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUIElement();
	}

	public override void ShowUI() {
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
	}

	public override void Callback(object data){
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;

		switch (cbdArgs.funcName){
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "ShowFriendName" : 
				CallBackDispatcherHelper.DispatchCallBack(ShowFriendName, cbdArgs);
				break;
			case "DestoryDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
				break;
			case "EnableUpdateButton" : 
				CallBackDispatcherHelper.DispatchCallBack(EnableUpdateButton, cbdArgs);
				break;
        case "RefreshFriendListView": 
            CallBackDispatcherHelper.DispatchCallBack(RefreshFriendListView, cbdArgs);

			break;
						case "EnableRefuseButton" : 
				CallBackDispatcherHelper.DispatchCallBack(EnableRefuseButton, cbdArgs);
                        break;
			default:
				break;
		}

	}

    void RefreshFriendListView(object args) {

    }

	void EnableUpdateButton(object args){
		updateFriendButton.gameObject.SetActive(true);
		UIEventListener.Get(updateFriendButton.gameObject).onClick = ClickUpdateFriendButton;
	}

	void ClickUpdateFriendButton(GameObject button){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateFriendButtonClick", null);
		ExcuteCallback(cbdArgs);
	}

	void EnableRefuseButton(object args){
		refuseAllApplyButton.gameObject.SetActive(true);
		UIEventListener.Get(refuseAllApplyButton.gameObject).onClick = ClickRefuseButton;
	}

	void ClickRefuseButton(GameObject args){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("RefuseApplyButtonClick", null);
		ExcuteCallback(cbdArgs);
	}

	void InitUIElement(){
		string itemSourcePath = "Prefabs/UI/Friend/AvailFriendItem";
		unitItem = Resources.Load( itemSourcePath ) as GameObject;
		refuseAllApplyButton = FindChild<UIButton>("Button_Refuse");
		sortButton = FindChild<UIButton>("Button_Sort");
		updateFriendButton = FindChild<UIButton>("Button_Update");
		curCountLabel = FindChild<UILabel>("CountItem/Label_Count_Cur");
		maxCountLabel = FindChild<UILabel>("CountItem/Label_Count_Max");

		InitDragPanelArgs();
	}

	DragPanel CreateDragPanel( string name, int count){
		DragPanel panel = new DragPanel(name, unitItem);
		panel.CreatUI();
		panel.AddItem( count, unitItem);
		return panel;
	}

	void CreateDragView(object args){
		LogHelper.Log("FriendListView.CreateDragView(), receive call from logic, to create ui...");
		List<UnitItemViewInfo> viewInfoList = args as List<UnitItemViewInfo>;
		
		friendViewInfoList = viewInfoList;
		dragPanel = CreateDragPanel("FriendDragPanel", viewInfoList.Count);
		FindCrossShowLabelList();
		UpdateAvatarTexture(viewInfoList);
		UpdateCountLabel(viewInfoList.Count, 50);
		UpdateEventListener();
		//UpdateStarSprite(viewInfoList);
		UpdateCrossShow();
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}

	void ShowFriendName(object args){
		List<string> nameList = args as List<string>;

		for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel label = scrollItem.transform.FindChild("Label_Name").GetComponent<UILabel>();
			label.text = nameList[ i ];
		}
	}

	void FindCrossShowLabelList(){
		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
			crossShowLabelList.Add(label);
		}
	}

	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",	transform);
		dragPanelArgs.Add("scrollerScale",	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	220 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, -210, 640, 600));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",		4);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -540, 0));
		dragPanelArgs.Add("cellWidth", 		140);
		dragPanelArgs.Add("cellHeight",		140);
	}

	void UpdateAvatarTexture(List<UnitItemViewInfo> dataItemList){
		//LogHelper.Log("UpdateAvatarTexture(), ....");
		for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
			uiTexture.mainTexture = dataItemList[ i ].Avatar;
		}
	}

	void UpdateCountLabel(int cur, int max){
		curCountLabel.text = cur.ToString();
		maxCountLabel.text = max.ToString();
	}
	
	void UpdateCrossShow(){
		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow", 0f, 1f);
	}

	void CrossShow(){
		if(exchange){
			for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				crossShowLabelList[ i ].text = "Lv" + friendViewInfoList[ i ].CrossShowTextBefore;
				crossShowLabelList[ i ].color = Color.yellow;
			}
			exchange = false;
		}
		else{
			for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				crossShowLabelList[ i ].text = "+" + friendViewInfoList[ i ].CrossShowTextAfter;
				crossShowLabelList[ i ].color = Color.red;
			}
			exchange = true;
		}
	}

	void UpdateStarSprite(List<UnitItemViewInfo> dataItemList){
		for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UISprite starSpr = scrollItem.transform.FindChild("StarMark").GetComponent<UISprite>();
			if(dataItemList[ i ].IsCollected)
				starSpr.enabled = true;
			else
				starSpr.enabled = false;
		}
	}

	void UpdateEventListener(){
		for( int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UIEventListenerCustom.Get( scrollItem ).onClick = ClickItem;
			UIEventListenerCustom.Get( scrollItem ).LongPress = PressItem;
		}
	}

	void ClickItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", dragPanel.ScrollItem.IndexOf(item));
		ExcuteCallback(cbdArgs);
	}

	void PressItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
		ExcuteCallback(cbdArgs);
	}

	void DestoryDragView(object args){
		crossShowLabelList.Clear();
		friendViewInfoList.Clear();
		crossShowLabelList.Clear();
		
		foreach (var item in dragPanel.ScrollItem){
			GameObject.Destroy(item);
		}
		dragPanel.ScrollItem.Clear();
		GameObject.Destroy(dragPanel.DragPanelView.gameObject);
	}
}


