using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class PartyUnitsView : ViewBase {
	DragPanel dragPanel;
	GameObject unitItem;
	GameObject rejectItem;
	bool exchange = false;
    List<UserUnit> userUnitInfoList = new List<UserUnit>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	Dictionary<GameObject, UserUnit> dragItemViewDic = new Dictionary<GameObject, UserUnit>();
	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();
	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<string> crossShowTextList = new List<string>();
	List<UnitItemViewInfo> viewInfoList = new List<UnitItemViewInfo>();
	List<UserUnit> currentPaty = new List<UserUnit>();
	List<PartyUnitItem> partyViewList = new List<PartyUnitItem>();
	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config, data);
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);

		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.RefreshPartyUnitList, RefreshOnPartyUnitList);
	}

	void RefreshOnPartyUnitList(object data) {
		currentPaty = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetUserUnit();
		for (int i = 0; i < partyViewList.Count; i++) {
			PartyUnitItem puv = partyViewList[i];
			UserUnit tuu = currentPaty.Find(a=>a.MakeUserUnitKey() == puv.UserUnit.MakeUserUnitKey());
			if(tuu != default(UserUnit)) {
				puv.IsParty = true;
			}else{
				puv.IsParty = false;
			}
			puv.IsEnable = false;
		}
	}

	void InitDragPanel(){
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		ResourceManager.Instance.LoadLocalAsset( itemSourcePath , o =>{
			unitItem = o as GameObject;
		});
		ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/RejectItem", o=>{
			rejectItem = o as GameObject;
			InitDragPanelArgs();
		});

	}

	DragPanel CreateDragPanel( string name, int count){
		DragPanel panel = new DragPanel(name, unitItem,transform);
//		panel.CreatUI();
		panel.AddItem( 1, rejectItem);
		panel.AddItem( count, unitItem);
		return panel;
	}

	void ClickItem(PartyUnitItem puv){
//		PartyUnitView puv = item.GetComponent<PartyUnitView>();
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClickItem", puv);
//		ExcuteCallback( cbd );
//		ModuleManger.SendMessage(ModuleEnum.par
	}

	void PressItem(GameObject item ){
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
//		LogHelper.Log("PartyUnitsView.PressItem(), click drag item, call view respone...");
//		ExcuteCallback( cbd );
	}

	void AddEventListener( GameObject item){
//		UIEventListenerCustom.Get( item ).onClick = ClickItem;
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
		dragPanelArgs.Add("cellWidth", 		120);
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
			tweenPos.ResetToBeginning();
			tweenPos.PlayForward();
		}
	}
	
	void ClickRejectItem(GameObject go){
		Debug.Log("PartyUnitsView.ClickRejectItem(), Receive reject item click, request logic...");
//		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClickReject", null);
//		ExcuteCallback(cbd);
	}

	void UpdateUnitItemEnableState(object args){
		for (int i = 0; i < partyViewList.Count; i++) {
			if(partyViewList[ i ].IsParty)
				partyViewList[ i ].IsEnable = false;
			else
				partyViewList[ i ].IsEnable = true;
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
//			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
//			uiTexture.mainTexture = dataItemList[ i - 1 ].Avatar;
//			dataItemList[ i - 1 ].DataItem.UnitInfo.Type;

//			UISprite typeSpr = scrollItem.transform.FindChild("Sprite_Type").GetComponent<UISprite>();
//			typeSpr.color = dataItemList[ i - 1 ].TypeColor;
//			Debug.LogError("item " + i + "Color is : " + dataItemList[ i - 1 ].TypeColor.ToString());
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
		UpdateUnitItemEnableState(itemDataList);
		UpdateStarSprite(itemDataList);
	}

	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (args[0].ToString()){
			case "Activate" : 
				UpdateUnitItemEnableState(args[1]);
				break; 
			case "RefreshDragList" : 
				UpdateDragPanel(args[1]);
				break;
			case "CreateDragView" : 
				CreateDragView(args[1]);
				break;
			case "DestoryDragView" : 
				DestoryDragView(args[1]);
				break;
			default:
				break;
		}	
	}

	void CreateDragView(object args){
		partyViewList.Clear();
		List<UserUnit> data = args as List<UserUnit>;
		dragPanel = new DragPanel("DragPanel", MyUnitItem.ItemPrefab,transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem(data.Count);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			PartyUnitItem puv = PartyUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			puv.Init(data[ i ]);
			partyViewList.Add(puv);
			puv.callback = ClickItem;
		}

	}

	void DestoryDragView(object args){
		crossShowLabelList.Clear();
		viewInfoList.Clear();

        if (dragPanel != null){
            foreach (var item in dragPanel.ScrollItem){
                GameObject.Destroy(item);
            }
            dragPanel.ScrollItem.Clear();
			GameObject.Destroy(dragPanel.GetDragViewObject());
        }

	}
	
	void FindCrossShowLabelList(){
		for(int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
			crossShowLabelList.Add(label);
		}
	}
}





