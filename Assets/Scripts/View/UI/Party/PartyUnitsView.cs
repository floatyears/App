using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PartyUnitsView : UIComponentUnity {

	DragPanel dragPanel;
	GameObject unitItem;
	GameObject rejectItem;

	bool exchange = false;
    List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	Dictionary<GameObject, TUserUnit> dragItemViewDic = new Dictionary<GameObject, TUserUnit>();
	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<string> crossShowTextList = new List<string>();
	List<UnitItemViewInfo> viewInfoList = new List<UnitItemViewInfo>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
	}

	void InitDragPanel(){
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		unitItem = Resources.Load( itemSourcePath ) as GameObject;
		rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject ;
		InitDragPanelArgs();
	}

	DragPanel CreateDragPanel( string name, int count){
		DragPanel panel = new DragPanel(name, unitItem);
		panel.CreatUI();
		panel.AddItem( 1, rejectItem);
		panel.AddItem( count, unitItem);
		return panel;
	}

	void ClickItem(GameObject item){
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClickItem", dragPanel.ScrollItem.IndexOf(item));
		//LogHelper.Log("PartyUnitsView.ClickItem(), click drag item, call view respone...");
		ExcuteCallback( cbd );
	}

	void PressItem(GameObject item ){
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
//		LogHelper.Log("PartyUnitsView.PressItem(), click drag item, call view respone...");
		ExcuteCallback( cbd );
	}

	void AddEventListener( GameObject item){
		UIEventListenerCustom.Get( item ).onClick = ClickItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans",	transform);
		dragPanelArgs.Add("scrollerScale",	Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos",	-28 * Vector3.up);
		dragPanelArgs.Add("position", 		Vector3.zero);
		dragPanelArgs.Add("clipRange", 		new Vector4(0, -120, 640, 400));
		dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine",		3);
		dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -315, 0));
		dragPanelArgs.Add("cellWidth", 		110);
		dragPanelArgs.Add("cellHeight",		110);
	}

	void CrossShow(){
		if(exchange){
			for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				crossShowLabelList[ i - 1 ].text = "Lv" + viewInfoList[ i -1 ].CrossShowTextBefore;
				crossShowLabelList[ i - 1 ].color = Color.yellow;
			}
			exchange = false;
		}
		else{
			for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				if(viewInfoList[ i -1 ].CrossShowTextAfter == "0") continue;
				else{
					crossShowLabelList[ i - 1 ].text = "+" + viewInfoList[ i -1 ].CrossShowTextAfter;
					crossShowLabelList[ i - 1 ].color = Color.red;
				}
			}
			exchange = true;
		}
	}

	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
	
	void ClickRejectItem(GameObject go){
		Debug.Log("PartyUnitsView.ClickRejectItem(), Receive reject item click, request logic...");
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClickReject", null);
		ExcuteCallback(cbd);
	}

	void UpdateUnitItemMask(object args){
		List<UnitItemViewInfo> dataItemList = args as List<UnitItemViewInfo>;
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UISprite maskSpr = scrollItem.transform.FindChild("Mask").GetComponent<UISprite>();
			if(dataItemList[ i - 1 ].IsEnable){
				maskSpr.enabled = false;
			}
			else{
				maskSpr.enabled = true;
			}
		}
	}
	
	void UpdatePartyLabel(List<UnitItemViewInfo> dataItemList){
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel partyLabel = scrollItem.transform.FindChild("Label_Party").GetComponent<UILabel>();
			if(dataItemList[ i - 1 ].IsParty){
				partyLabel.text = "Party";
				partyLabel.color = Color.red;
			}
			else{
				partyLabel.text = string.Empty;
			}
		}
	}
	
	void UpdateStarSprite(List<UnitItemViewInfo> dataItemList){
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UISprite starSpr = scrollItem.transform.FindChild("StarMark").GetComponent<UISprite>();
			if(dataItemList[ i - 1 ].IsCollected)
				starSpr.enabled = true;
			else
				starSpr.enabled = false;
		}
	}
	
	void UpdateCrossShow(){
		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow", 0f, 1f);
	}
	
	void UpdateAvatarTexture(List<UnitItemViewInfo> dataItemList){
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
			uiTexture.mainTexture = dataItemList[ i - 1 ].Avatar;
		}
	}
	
	void UpdateEventListener(){
		UIEventListenerCustom.Get(dragPanel.ScrollItem[ 0 ]).onClick = ClickRejectItem;
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			AddEventListener(scrollItem);
		}
	}

	void UpdateDragPanel(object args){
		List<UnitItemViewInfo> itemDataList = args as List<UnitItemViewInfo>;

		UpdatePartyLabel(itemDataList);
		UpdateUnitItemMask(itemDataList);
		UpdateStarSprite(itemDataList);
	}

	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "Activate" : 
				CallBackDispatcherHelper.DispatchCallBack(UpdateUnitItemMask, cbdArgs);
				break; 
			case "RefreshDragList" : 
				CallBackDispatcherHelper.DispatchCallBack(UpdateDragPanel, cbdArgs);
				break;
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "DestoryDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
				break;
			default:
				break;
		}	
	}

	void CreateDragView(object args){
		List<UnitItemViewInfo> itemDataList = args as List<UnitItemViewInfo>;
		viewInfoList = itemDataList;
		dragPanel = CreateDragPanel("DragPanel", itemDataList.Count);
		FindCrossShowLabelList();
		UpdateAvatarTexture(itemDataList);
		UpdateEventListener();
		UpdatePartyLabel(itemDataList);
		UpdateUnitItemMask(itemDataList);
		UpdateStarSprite(itemDataList);
		UpdateCrossShow();
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}

	void DestoryDragView(object args){
		crossShowLabelList.Clear();
		viewInfoList.Clear();

		foreach (var item in dragPanel.ScrollItem){
			GameObject.Destroy(item);
		}
		dragPanel.ScrollItem.Clear();
		GameObject.Destroy(dragPanel.DragPanelView.gameObject);
	}
	
	void FindCrossShowLabelList(){
		for(int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
			crossShowLabelList.Add(label);
		}
	}



}





