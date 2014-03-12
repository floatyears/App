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


	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitDragPanel();
	}

	public override void ShowUI(){
		base.ShowUI();

		InvokeCrossShow(0.1f, 1.0f);
		OnLightDragItem(true);
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
	}

	void InitDragPanel(){
		if ( DataCenter.Instance.MyUnitList != null)
			userUnitInfoList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		else{
			Debug.Log("DataCenter.Instance.MyUnitList is null, return");
			return;
		}

		if(userUnitInfoList == null ){
			Debug.LogWarning("userUnitInfoList is null ");
			return;
		}
//
		int unitCount = userUnitInfoList.Count;
		Debug.LogError("unitCount: " + unitCount);
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		unitItem = Resources.Load( itemSourcePath ) as GameObject;
		rejectItem = Resources.Load("Prefabs/UI/Friend/RejectItem") as GameObject ;
		InitDragPanelArgs();
		dragPanel = new DragPanel("UnitScroller", unitItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(1, rejectItem);
		dragPanel.AddItem(unitCount, unitItem);
		FillDragPanel( dragPanel );
		dragPanel.RootObject.SetScrollView(dragPanelArgs);
	}

	protected DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count, item);
		return panel;
	}

	void FillDragPanel(DragPanel panel){
		if( panel == null ){
			Debug.LogError( "PartyUnitsView.FillDragPanel(), DragPanel is null, return!");
			return;
		}
	
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject scrollItem = panel.ScrollItem[ i ];
			if( i == 0 ){
				UIEventListenerCustom.Get( scrollItem ).onClick = ClickRejectItem;
				continue;
			}
//
//			TUserUnit uuItem = userUnitInfoList[ i - 1 ] ;
//			dragItemViewDic.Add( scrollItem, uuItem );
//			
//			StoreLabelInfo( scrollItem);
//			ShowItem( scrollItem );
//			AddEventListener( scrollItem );
		}
	}

	void ShowItem( GameObject item){
		ShowMask(item,true);
		GameObject avatarGo = item.transform.FindChild( "Texture_Avatar").gameObject;
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		
		uint uid = dragItemViewDic[item].UnitID;
		avatarTex.mainTexture = DataCenter.Instance.UnitInfo[ uid ].GetAsset(UnitAssetType.Avatar);
		
		int addAttack = dragItemViewDic[ item ].AddAttack;

		int addHp = dragItemViewDic[ item ].AddHP;

		int level = dragItemViewDic[ item ].Level;

		int addPoint = addAttack + addHp;
	}

	void ClickItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ItemClick", dragItemViewDic[ item ]);
		LogHelper.Log("PartyUnitsView.ClickDragItem(), click drag item, call view respone...");
		ExcuteCallback( cbd );
	}

	void PressItem(GameObject item ){
		TUserUnit unitInfo = dragItemViewDic[ item ];
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);	
	}

	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
		maskSpr.gameObject.SetActive( canMask );
	}

	void AddEventListener( GameObject item){
		UIEventListenerCustom.Get( item ).onClick = ClickItem;
		UIEventListenerCustom.Get( item ).LongPress = PressItem;
	}

	void StoreLabelInfo(GameObject item){
		TUserUnit tuu = dragItemViewDic[ item ];
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
		infoStruct.targetLabel = item.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
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
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
				unitInfoStruct[ i ].targetLabel.color = Color.yellow;
			}
			exchange = false;
		}
		else{
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
				unitInfoStruct[ i ].targetLabel.color = Color.red;                          
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

	void InvokeCrossShow(float start, float cycle){
		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
                InvokeRepeating("CrossShow", start, cycle);
        }

        void OnLightDragItem(bool b){
		LogHelper.Log("PartyUnitsView.ActivateAllMask(), Receive callBack from Logic, to activate panel...");
		foreach (var item in dragItemViewDic)
			ShowMask(item.Key, b);

		LogHelper.Log("PartyUnitsView.ActivateAllMask(), End...");
	}

	void ClickRejectItem(GameObject go){
		Debug.Log("PartyUnitsView.ClickRejectItem(), Receive reject item click, request logic...");
		CallBackDispatcherArgs cbd = new CallBackDispatcherArgs("ClickReject", null);
		ExcuteCallback(cbd);
	}
	
	public override void Callback(object data){
		base.Callback(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "activate" : 
				OnLightDragItem(false);
                        	break; 
			case "RefreshDragList" : 
				CallBackDispatcherHelper.DispatchCallBack(RefreshDragPanel, cbdArgs);
				break;
			default:
                        	break;
                }	
        }


	
	void ClearDragItem(){
		LogHelper.Log("ClearDragItem(), Clear Before : Drag Item Count is : " + dragPanel.ScrollItem.Count);
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject dragItem = dragPanel.ScrollItem[ i ];
			dragPanel.RemoveItem(dragItem);
		}
		LogHelper.Log("ClearDragItem(), Clear After : Drag Item Count is : " + dragPanel.ScrollItem.Count);
	}

	void AddDragItem(int count){
		dragPanel.AddItem(count);
	}

	void RefreshDragPanel(object args){
		//ClearDragItem();

		List<PartyUnitItemView> itemDataList = args as List<PartyUnitItemView>;

		//AddDragItem(itemDataList.Count);
		for( int i = 1; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture uiTexture = scrollItem.transform.FindChild("Texture_Avatar").GetComponent<UITexture>();
			uiTexture.mainTexture = itemDataList[ i-1 ].Avatar;

			UILabel partyLabel = scrollItem.transform.FindChild("Label_Party").GetComponent<UILabel>();
			if(itemDataList[ i -1 ].IsParty){
				partyLabel.text = "Party";
				partyLabel.color = Color.red;
			}
			else{
				partyLabel.text = string.Empty;
			}

			UISprite starSpr = scrollItem.transform.FindChild("StarMark").GetComponent<UISprite>();
			if(itemDataList[ i -1 ].IsCollected){
				starSpr.enabled = true;
			}
			else{
				starSpr.enabled = false;
			}

			UISprite maskSpr = scrollItem.transform.FindChild("Mask").GetComponent<UISprite>();
			if(itemDataList[ i -1 ].IsEnable){
				maskSpr.enabled = false;
			}
			else{
				maskSpr.enabled = true;
			}
                }
		
	}
}





