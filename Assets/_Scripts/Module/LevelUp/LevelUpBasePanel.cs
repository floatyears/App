//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using bbproto;
//
//public class LevelUpBasePanel : UIComponentUnity {
//	DragPanel baseDragPanel;
//	Dictionary<GameObject, UnitItemInfo> baseUnitInfoDic = new Dictionary<GameObject, UnitItemInfo>();
//	List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
//	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();
//
//	private UnitItemInfo baseSelectItem;
//	private Dictionary<int, UnitItemInfo> materialDic = new Dictionary<int, UnitItemInfo> ();	// key is item pos.
//
//	private string currentState = "";
//	private List<UnitItemInfo> partyItem = new List<UnitItemInfo> ();
//	private List<UnitItemInfo> materialItem = new List<UnitItemInfo> ();
//	private List<UnitItemInfo> selectMaterial = new List<UnitItemInfo> ();
//
//	UnitItemInfo readyPanelSelect;
//
//    bool beginOnAfterLevelUp = false; // if == true, turn to material tab at start;
//
//	public override void Init(UIInsConfig config){
//        InitUI();
//        MsgCenter.Instance.AddListener (CommandEnum.LevelUpSucceed, ResetUIAfterLevelUp);
//        base.Init(config);
//	}
//
//	public override void ShowUI(){
//		base.ShowUI();
//		if (!gameObject.activeSelf) {
//			gameObject.SetActive(true);	
//		}
//		AddListener();
//        if (beginOnAfterLevelUp){
////            MsgCenter.Instance.Invoke(CommandEnum.FocusLevelUpPanel, 2);
//            beginOnAfterLevelUp = false;
//        }
////		Debug.LogError("LevelUpBasePanel end showui");
//	}
//	
//	public override void HideUI(){
////		ClearData ();
//		RemoveListener();
//		base.HideUI();
//	}
//
//    public override void ResetUIState(){
//        ClearData();
//        gameObject.SetActive(true); 
//        InitDragPanel();
//    }
//
//	public override void DestoryUI ()
//	{
//		MsgCenter.Instance.RemoveListener (CommandEnum.LevelUpSucceed, ResetUIAfterLevelUp);
//		base.DestoryUI ();
//	}
//
//    public void ResetUIAfterLevelUp(object args){
//        ResetUIState();
//        ShowMyself("Tab_Base");
////        MsgCenter.Instance.Invoke(CommandEnum.FocusLevelUpPanel, 0);
//        uint uniqueId = (uint)args;
//        LogHelper.Log("ResetUIAfterLevelUp(), uniqueId {0}", uniqueId);
//        if (uniqueId != 0){
//            SelectUnitById(uniqueId);
//        }
//        ShieldParty(true);
//        baseSelectItem.stateLabel.text = "base";
//        beginOnAfterLevelUp = true;
//    }
//    
//    public void ClearData() {
//		if (baseDragPanel != null) {
//			baseDragPanel.DestoryUI ();
//		}
//		baseUnitInfoDic.Clear ();
//		userUnitInfoList.Clear ();
//		unitInfoStruct.Clear ();
////		Debug.LogError("clear data");
//		if(baseSelectItem != null)
//			baseSelectItem.stateLabel.text  = "";
//		materialDic.Clear ();
//		partyItem.Clear ();
//		materialItem.Clear ();
//		selectMaterial.Clear ();
//	}
//
//	void SetFalse(UnitItemInfo uii) {
//		uii.isSelect = false;
//		uii.stateLabel.text = "";
//		ShowMask (uii.scrollItem, false);
//	}
//
//	private void InitUI(){
////		InitDragPanelArgs();
//	}
//
//	//CommandEnum.PanelFocus
//	void ShowMyself(object data){
//		string msg = (string)data;
//		currentState = msg;
//		if( msg == "Tab_Friend"){
//			this.gameObject.SetActive( false );
//			return;
//		}
//		this.gameObject.SetActive(true);
//
//		if(IsInvoking("CrossShow")) {
//			CancelInvoke("CrossShow");
//		}
//		InvokeRepeating("CrossShow",0.1f, 1f);
//	}
//
//    void SelectUnitById(uint uniqueId){
//        foreach (var item in baseUnitInfoDic) {
//            LogHelper.Log("SelectUnitById(), id {0}, listUnitId {1}", uniqueId, item.Value.userUnitItem.ID);
//            if (item.Value.userUnitItem.ID == uniqueId){
//                SelectBaseItem(item.Key);
//                return;
//            }
//        }
//    }
//
//	void AddListener(){
//		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, ShowMyself);
//		MsgCenter.Instance.AddListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
//		MsgCenter.Instance.AddListener (CommandEnum.ShieldMaterial, ShieldMaterial);
//	}
//
//	void RemoveListener(){
//		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, ShowMyself);
//		MsgCenter.Instance.RemoveListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
//		MsgCenter.Instance.RemoveListener (CommandEnum.ShieldMaterial, ShieldMaterial);
//    }
//    
//    void ShieldMaterial(object data) {
//		UnitItemInfo[] unitItem = data as UnitItemInfo[];
//		for (int i = 0; i < unitItem.Length; i++) {
//			if(unitItem[i] == null) {
//				continue;
//			}
//			if(selectMaterial.Contains(unitItem[i])) {
//				selectMaterial.Remove(unitItem[i]);
//			}
//		}
//
//		for (int i = 0; i < selectMaterial.Count; i++) {
//			ShowMask(selectMaterial[i].scrollItem,false);
//			selectMaterial.RemoveAt(i);
//		}
//
//		for (int i = 0; i < unitItem.Length; i++) {
//			if(unitItem[i] != null){
//				selectMaterial.Add(unitItem[i]);
//				unitItem[i].stateLabel.text = "Material";
//
//			}
//		}
//		if (selectMaterial.Count == 4) {
//			ShieldMaterial (true);
//		} else {
//			ShieldMaterial(false);
//		}
//
//	}
//
//	void ShieldMaterial(bool isShield) {
//		if (isShield) {
//			for (int i = 0; i < materialItem.Count; i++) {
//				ShowMask (materialItem [i].scrollItem, isShield);
//			}
//		} else {
//			for (int i = 0; i < materialItem.Count; i++) {
//				if(selectMaterial.Contains(materialItem [i])) {
//					ShowMask (materialItem [i].scrollItem, !isShield);
//				}else if(!materialItem[i].Equals(baseSelectItem)){
//					ShowMask (materialItem [i].scrollItem, isShield);
//					materialItem[i].stateLabel.text = "";
//				}
//			}
//		}
//	}
//
//	void BaseAlreadySelect(object data) {
//		if (data == null) {
//			ShieldParty(false);
//			if(baseSelectItem != null) {
//				if(DataCenter.Instance.PartyInfo.UnitIsInParty(baseSelectItem.userUnitItem.ID)) {
//					baseSelectItem.stateLabel.text = "Party";
//				}
//				else{
//					baseSelectItem.stateLabel.text = "";
//				}
//
//				baseSelectItem = null;
//			}
//		} else {
//			ShieldParty(true);
//			baseSelectItem = data as UnitItemInfo;
//			baseSelectItem.stateLabel.text = "base";
//		}
//	}
//
//	void ShieldParty(bool isShield) {
//		for (int i = 0; i < partyItem.Count; i++) {
//			UnitItemInfo uii = partyItem[i];
//			if(baseSelectItem != null && baseSelectItem.Equals(uii)) {
//				continue;
//			}
//
//			uii.isSelect = isShield;
//			ShowMask(uii.scrollItem,isShield);
//		}
//	}
//
//    private void SelectBaseItem(GameObject item){
//        LogHelper.Log("SelectBaseItem() item {0}, currentState {1}", item, currentState);
//        UnitItemInfo uui = baseUnitInfoDic [item];
//        
//        if (currentState == "Tab_Base") {
//            DisposeBaseClick (uui);
//        } else if (currentState == "Tab_Material") {
//            if (uui.isCollect) {
//                return;     
//            }
//            DisposeMaterialClick(uui);
//        }
//    }
//
//	private void ClickBaseItem(GameObject item){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//        SelectBaseItem(item);
//	}
//
//	void DisposeMaterialClick(UnitItemInfo uui) {
//		if (uui.isPartyItem) {
//			return;	
//		}
//		if (baseSelectItem != null && baseSelectItem.Equals (uui)) {
//			return;
//		}
//		if (selectMaterial.Count == 4 && !CheckIsMaterial(uui)) {
//			return;	
//		}
//		MsgCenter.Instance.Invoke (CommandEnum.PickBaseUnitInfo, uui);
//	}
//
//	bool CheckIsMaterial(UnitItemInfo uii) {
//		for (int i = 0; i < selectMaterial.Count; i++) {
//			if(selectMaterial[i].Equals(uii)) {	
//				return true;
//			}
//		}
//		return false;
//	}
//
//    void SetDisposeBaseItemLabel(UnitItemInfo uui){
//        if (DataCenter.Instance.PartyInfo.UnitIsInParty(uui.userUnitItem.ID)){
//            uui.stateLabel.text = "Party";
//        }
//        else {
//            uui.stateLabel.text = "";
//        }
//    }
//
//	void DisposeBaseClick(UnitItemInfo uui) {
//		UnitItemInfo temp = selectMaterial.Find (a => a.userUnitItem.ID == uui.userUnitItem.ID);
//		if (temp != default(UnitItemInfo)) {
//			Debug.LogError("temp : " + temp.userUnitItem.ID);
//			return;		
//		}
//        LogHelper.Log("DisposeBaseClick(), uui {0}, baseSelectItem {1}", uui, baseSelectItem);
//		if (baseSelectItem == null) {
//			baseSelectItem = uui;
//			ShowMask (uui.scrollItem, true);
//		} 
//        else{
//            SetDisposeBaseItemLabel(baseSelectItem);
//            if (baseSelectItem.Equals (uui)) {
//                ShowMask (uui.scrollItem, false);
//                baseSelectItem = null;
//            } 
//            else {
//                ShowMask(baseSelectItem.scrollItem,false);
//                baseSelectItem = uui;
//                ShowMask(baseSelectItem.scrollItem,true);
//            }
//        }
//
//		MsgCenter.Instance.Invoke (CommandEnum.PickBaseUnitInfo, baseSelectItem);
//	}
//	
//	void ShowMask( GameObject target, bool canMask) {
//		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
//		maskSpr.gameObject.SetActive( canMask );
//	}
//
//	void PressItem(GameObject item ){
//		TUserUnit unitInfo = baseUnitInfoDic[ item ].userUnitItem;
//		ModuleManger.Instance.ShowModule(ModuleEnum.UnitDetail );
//		ModuleManger.Instance.SendMessage(ModuleEnum.UnitDetailModule, unitInfo);
//	}
//
//	void InitDragPanel(){
////		Debug.LogError(" start levelup base panel InitDragPanel ");
//		if ( DataCenter.Instance.MyUnitList != null)
//			userUnitInfoList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
//
////		Debug.LogError (userUnitInfoList.Count);
//
//		string name = "BaseDragPanel";
//		if(userUnitInfoList == null ){
//			Debug.LogWarning("userUnitInfoList is null ");
//			return;
//		}
//		int count = userUnitInfoList.Count;
//		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
//		GameObject itemGo =  ResourceManager.Instance.LoadLocalAsset( itemSourcePath ) as GameObject;
//
//		baseDragPanel = CreateDragPanel( name, count, itemGo) ;
//		FillDragPanel (baseDragPanel);
//		baseDragPanel.DragPanelView.SetScrollView(ConfigDragPanel.LevelUpBaseDragPanelArgs, transform);
//	}
//	
//	private DragPanel CreateDragPanel( string name, int count, GameObject item){
//		DragPanel panel = new DragPanel(name,item);
//		panel.CreatUI();
//		panel.AddItem( count);
//		return panel;
//	}
//	
//	void FillDragPanel(DragPanel panel){
//		if( panel == null ){
//			Debug.LogError( "LevelUpBasePanel.FillDragPanel(), DragPanel is null, return!");
//			return;
//		}
//		List<TUserUnit> temp = new List<TUserUnit> ();
//		List<TUnitParty> allParty = DataCenter.Instance.PartyInfo.AllParty;
//		for (int i = 0; i < allParty.Count; i++) {
//			List<TUserUnit> dic = allParty [i].GetUserUnit ();
//			foreach (var item in dic) {
//				if(item != null) {
//					temp.Add (item);
//				}
//			}
//		}
//		StartCoroutine (Fill (panel, temp));
//	}
//
//	IEnumerator Fill (DragPanel panel,List<TUserUnit> temp ) {
//		for( int i = 0; i < panel.ScrollItem.Count; i++){
//			GameObject scrollItem = panel.ScrollItem[ i ];
//			TUserUnit uuItem = userUnitInfoList[ i ] ;
//			UnitItemInfo uii = scrollItem.AddComponent<UnitItemInfo>();
//			uii.userUnitItem = uuItem;
//			uii.isCollect = false;
//			uii.stateLabel = scrollItem.transform.Find("Label_Party").GetComponent<UILabel>();
//			if(!uii.isCollect) {
//				TUserUnit tuu = temp.Find(a=>a.ID == uuItem.ID);
//
//				if(tuu == default(TUserUnit)) {
//					materialItem.Add(uii);
//				}
//				else{
//					uii.isPartyItem = true;
//					int indexTwo = partyItem.FindIndex(a=>a.userUnitItem.ID == uii.userUnitItem.ID);
//					if(indexTwo == -1) {
//						partyItem.Add(uii);
//						uii.stateLabel.text = "Party";
//					}
//				}
//			}else{
//				ShowMask(uii.scrollItem,true);
//			}
//			
//			uii.scrollItem = scrollItem;
//			baseUnitInfoDic.Add( scrollItem, uii );
//			StoreLabelInfo( uii);
//			ShowItem(uii);
//			AddEventListener(uii);
//			
//			if(i > 0 && i % 20 == 0)
//				yield return 0;
//		}
//	}
//
//	void StoreLabelInfo(UnitItemInfo item){
//
//		TUserUnit tuu = item.userUnitItem;
//		UnitInfoStruct infoStruct = new UnitInfoStruct();
//		infoStruct.text1 = tuu.Level.ToString();
//		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
//		infoStruct.targetLabel = item.scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
//		unitInfoStruct.Add(infoStruct);
//	}
//
//	void ShowItem( UnitItemInfo item){
//		Transform avatarGo = item.scrollItem.transform.FindChild( "Texture_Avatar");
//		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
//		TUserUnit uu = item.userUnitItem;
//		uint uid = uu.UnitID;
//		avatarTex.mainTexture = DataCenter.Instance.GetUnitInfo (uid).GetAsset (UnitAssetType.Avatar); //UnitInfo [uid].GetAsset (UnitAssetType.Avatar);
//		ShowMask (item.scrollItem, false);
//	}
//	
//	private void AddEventListener( UnitItemInfo item){
//		UIEventListenerCustom uiEventListener = UIEventListenerCustom.Get (item.scrollItem);
//		uiEventListener.LongPress = PressItem;
//		uiEventListener.onClick = ClickBaseItem;
//	}
//
//	bool exchange = false;
//	void CrossShow(){
//		if(exchange){
//			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
//				if(unitInfoStruct[ i ].text2 == "0") continue;
//				else{
//					unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
//					unitInfoStruct[ i ].targetLabel.color = Color.yellow;
//				}
//			}
//			exchange = false;
//		}
//		else {
//			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
//				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
//				unitInfoStruct[ i ].targetLabel.color = Color.red;
//        	}
//			exchange = true;
//        }
//	}//End
//        
//}
//
public class UnitInfoStruct{
	public string text1;
	public string text2;
	public UILabel targetLabel;
}
//
//
//
