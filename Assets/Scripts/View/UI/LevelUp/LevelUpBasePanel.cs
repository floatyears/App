using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUpBasePanel : UIComponentUnity {

	DragPanel baseDragPanel;
	Dictionary<GameObject, UnitItemInfo> baseUnitInfoDic = new Dictionary<GameObject, UnitItemInfo>();
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	List<TUserUnit> userUnitInfoList = new List<TUserUnit>();
	List<UnitInfoStruct> unitInfoStruct = new List<UnitInfoStruct>();

	private UnitItemInfo baseSelectItem;
	private Dictionary<int, UnitItemInfo> materialDic = new Dictionary<int, UnitItemInfo> ();	// key is item pos.

	private string currentState = "";
	private List<UnitItemInfo> partyItem = new List<UnitItemInfo> ();
	private List<UnitItemInfo> materialItem = new List<UnitItemInfo> ();
	private List<UnitItemInfo> selectMaterial = new List<UnitItemInfo> ();

	public override void Init(UIInsConfig config, IUICallback origin){
		InitUI();
		base.Init(config, origin);
	}

	public override void ShowUI(){
		AddListener();
		ClearData ();
		base.ShowUI();
		this.gameObject.SetActive(true);

	}
	
	public override void HideUI(){
		RemoveListener();
		base.HideUI();
	}

	void ClearData() {
		for (int i = 0; i < partyItem.Count; i++) {
			SetFalse(partyItem[i]);
		}

		for (int i = 0; i < materialItem.Count; i++) {
			SetFalse(materialItem[i]);
		}
	}

	void SetFalse(UnitItemInfo uii) {
		uii.isSelect = false;
		uii.stateLabel.text = "";
		ShowMask (uii.scrollItem, false);
	}

	private void InitUI(){
		InitDragPanel();
	}

	//CommandEnum.PanelFocus
	void ShowMyself(object data){
		string msg = (string)data;
		currentState = msg;
		if( msg == "Tab_Friend"){
			this.gameObject.SetActive( false );
			return;
		}
		this.gameObject.SetActive(true);

		if(IsInvoking("CrossShow")) {
			CancelInvoke("CrossShow");
		}
		InvokeRepeating("CrossShow",0.1f, 1f);
	}

	void AddListener(){
		MsgCenter.Instance.AddListener(CommandEnum.PanelFocus, ShowMyself);
		MsgCenter.Instance.AddListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
		MsgCenter.Instance.AddListener (CommandEnum.ShieldMaterial, ShieldMaterial);
	}

	void RemoveListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.PanelFocus, ShowMyself);
		MsgCenter.Instance.RemoveListener (CommandEnum.BaseAlreadySelect, BaseAlreadySelect);
		MsgCenter.Instance.RemoveListener (CommandEnum.ShieldMaterial, ShieldMaterial);
	}

	void ShieldMaterial(object data) {
		UnitItemInfo[] unitItem = data as UnitItemInfo[];
		for (int i = 0; i < unitItem.Length; i++) {
			if(unitItem[i] == null) {
				continue;
			}
			if(selectMaterial.Contains(unitItem[i])) {
				selectMaterial.Remove(unitItem[i]);
			}
		}

		for (int i = 0; i < selectMaterial.Count; i++) {
			ShowMask(selectMaterial[i].scrollItem,false);
			selectMaterial.RemoveAt(i);
		}

		for (int i = 0; i < unitItem.Length; i++) {
			if(unitItem[i] != null){
				selectMaterial.Add(unitItem[i]);
				unitItem[i].stateLabel.text = "Material";

			}
		}
		if (selectMaterial.Count == 4) {
			ShieldMaterial (true);
		} else {
			ShieldMaterial(false);
		}

	}

	void ShieldMaterial(bool isShield) {
		if (isShield) {
			for (int i = 0; i < materialItem.Count; i++) {
				ShowMask (materialItem [i].scrollItem, isShield);
			}
		} else {
			for (int i = 0; i < materialItem.Count; i++) {
				if(selectMaterial.Contains(materialItem [i])) {
					ShowMask (materialItem [i].scrollItem, !isShield);
				}else if(!materialItem[i].Equals(baseSelectItem)){
					ShowMask (materialItem [i].scrollItem, isShield);
					materialItem[i].stateLabel.text = "";
				}
			}
		}
	}

	void BaseAlreadySelect(object data) {
		if (data == null) {
			ShieldParty(false);
			if(baseSelectItem != null) {
				baseSelectItem.stateLabel.text = "";
				baseSelectItem = null;
			}
		} else {
			ShieldParty(true);
			baseSelectItem = data as UnitItemInfo;
			baseSelectItem.stateLabel.text = "base";
		}
	}

	void ShieldParty(bool isShield) {
		for (int i = 0; i < partyItem.Count; i++) {
			UnitItemInfo uii = partyItem[i];
			if(baseSelectItem != null && baseSelectItem.Equals(uii)) {
				continue;
			}

			uii.isSelect = isShield;
			ShowMask(uii.scrollItem,isShield);
		}
	}

	private void ClickBaseItem(GameObject item){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UnitItemInfo uui = baseUnitInfoDic [item];
		if (uui.isCollect) {
			return;		
		}
//		Debug.LogError ("uui.userUnitItem.ID : " + uui.userUnitItem.ID);
		if (currentState == "Tab_Base") {
			DisposeBaseClick (uui);
		} else if (currentState == "Tab_Material") {
			DisposeMaterialClick(uui);
		}
	}

	void DisposeMaterialClick(UnitItemInfo uui) {
		if (uui.isPartyItem) {
			return;	
		}
		if (baseSelectItem != null && baseSelectItem.Equals (uui)) {
			return;
		}
		if (selectMaterial.Count == 4 && !CheckIsMaterial(uui)) {
			return;	
		}
		MsgCenter.Instance.Invoke (CommandEnum.PickBaseUnitInfo, uui);
	}

	bool CheckIsMaterial(UnitItemInfo uii) {
		for (int i = 0; i < selectMaterial.Count; i++) {
			if(selectMaterial[i].Equals(uii)) {	
				return true;
			}
		}
		return false;
	}

	void DisposeBaseClick(UnitItemInfo uui) {
		bool first = baseSelectItem != null;

		if (first && !baseSelectItem.Equals(uui)) {
			return;	
		}

		if (!uui.isSelect) {
			uui.isSelect = true;
			ShowMask (uui.scrollItem, true);
		} else {
			uui.isSelect = false;
			ShowMask(uui.scrollItem, false);
		}

		MsgCenter.Instance.Invoke (CommandEnum.PickBaseUnitInfo, uui);
	}
	
	void ShowMask( GameObject target, bool canMask) {
		GameObject maskSpr = target.transform.FindChild("Mask").gameObject;
//		Debug.LogError (target.name + "```" + maskSpr);
		maskSpr.gameObject.SetActive( canMask );
	}

	void PressItem(GameObject item ){
		TUserUnit unitInfo = baseUnitInfoDic[ item ].userUnitItem;
		UIManager.Instance.ChangeScene(SceneEnum.UnitDetail );
		MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, unitInfo);

        }

	void InitDragPanel(){
		if ( DataCenter.Instance.MyUnitList != null)
			userUnitInfoList.AddRange(DataCenter.Instance.MyUnitList.GetAll().Values);
		string name = "BaseDragPanel";
		if(userUnitInfoList == null ){
			Debug.LogWarning("userUnitInfoList is null ");
			return;
		}
		int count = userUnitInfoList.Count;
		string itemSourcePath = "Prefabs/UI/Friend/UnitItem";
		GameObject itemGo =  Resources.Load( itemSourcePath ) as GameObject;
		InitDragPanelArgs();
		baseDragPanel = CreateDragPanel( name, count, itemGo) ;
		FillDragPanel (baseDragPanel);
		baseDragPanel.DragPanelView.SetScrollView(dragPanelArgs);
	}
	
	private DragPanel CreateDragPanel( string name, int count, GameObject item){
		DragPanel panel = new DragPanel(name,item);
		panel.CreatUI();
		panel.AddItem( count);
		return panel;
	}

	//Fill Unit Item by with config data
	void FillDragPanel(DragPanel panel){
		if( panel == null ){
			Debug.LogError( "LevelUpBasePanel.FillDragPanel(), DragPanel is null, return!");
			return;
		}
		List<TUserUnit> temp = new List<TUserUnit> ();
		List<TUnitParty> allParty = DataCenter.Instance.PartyInfo.AllParty;
		for (int i = 0; i < allParty.Count; i++) {
			List<TUserUnit> dic = allParty [i].GetUserUnit ();
			foreach (var item in dic) {
				if(item != null) {
					temp.Add (item);
				}
			}
		}
		StartCoroutine (Fill (panel, temp));
	}

	IEnumerator Fill (DragPanel panel,List<TUserUnit> temp ) {
		for( int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject scrollItem = panel.ScrollItem[ i ];
			TUserUnit uuItem = userUnitInfoList[ i ] ;
			UnitItemInfo uii = new UnitItemInfo();
			uii.userUnitItem = uuItem;
			uii.isCollect = false;
			uii.stateLabel = scrollItem.transform.Find("Label_Party").GetComponent<UILabel>();
			if(!uii.isCollect) {
				TUserUnit tuu = temp.Find(a=>a.ID == uuItem.ID);

				if(tuu == default(TUserUnit)) {
					materialItem.Add(uii);
				}
				else{
					uii.isPartyItem = true;
					int indexTwo = partyItem.FindIndex(a=>a.userUnitItem.ID == uii.userUnitItem.ID);
					if(indexTwo == -1) {
						partyItem.Add(uii);
					}
				}
			}else{
				ShowMask(uii.scrollItem,true);
			}
			
			uii.scrollItem = scrollItem;
			baseUnitInfoDic.Add( scrollItem, uii );
			StoreLabelInfo( uii);
			ShowItem(uii);
			AddEventListener(uii);
			
			if(i > 0 && i % 20 == 0)
				yield return 0;
		}
	}

	void StoreLabelInfo(UnitItemInfo item){

		TUserUnit tuu = item.userUnitItem;
		UnitInfoStruct infoStruct = new UnitInfoStruct();
		infoStruct.text1 = tuu.Level.ToString();
		infoStruct.text2 = (tuu.AddHP + tuu.AddAttack).ToString();
//		Debug.LogError("TUserUnit.Level : " + tuu.Level.ToString());
		infoStruct.targetLabel = item.scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
		unitInfoStruct.Add(infoStruct);
	}

	void ShowItem( UnitItemInfo item){
		Transform avatarGo = item.scrollItem.transform.FindChild( "Texture_Avatar");
		UITexture avatarTex = avatarGo.GetComponent< UITexture >();
		TUserUnit uu = item.userUnitItem;
		uint uid = uu.UnitID;
		avatarTex.mainTexture = DataCenter.Instance.UnitInfo [uid].GetAsset (UnitAssetType.Avatar);
	}
	
	private void AddEventListener( UnitItemInfo item){
		UIEventListenerCustom uiEventListener = UIEventListenerCustom.Get (item.scrollItem);
//		item.listener = uiEventListener;
		uiEventListener.LongPress = PressItem;
		uiEventListener.onClick = ClickBaseItem;
	}
	
	private void InitDragPanelArgs(){
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
	bool exchange = false;
	void CrossShow(){
		if(exchange){
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "+{0}", unitInfoStruct[ i ].text2);
				unitInfoStruct[ i ].targetLabel.color = Color.yellow;
			}
			exchange = false;
		} else {
			for (int i = 0 ; i< unitInfoStruct.Count; i++) {
				unitInfoStruct[ i ].targetLabel.text = string.Format( "Lv{0}", unitInfoStruct[ i ].text1);
				unitInfoStruct[ i ].targetLabel.color = Color.red;
        	}
			exchange = true;
        }
	}//End
        
}

public class UnitInfoStruct{
	public string text1;
	public string text2;
	public UILabel targetLabel;
}


