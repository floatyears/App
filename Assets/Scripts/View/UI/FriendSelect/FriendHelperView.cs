using UnityEngine;
using System.Collections.Generic;

public class FriendHelperView : UIComponentUnity{
//    private GameObject msgBox;
    private UIImageButton bottomButton;
    private UIButton btnSure;
    private UIButton btnCancel;
    private UIButton btnSeeInfo;

	private UILabel rightIndexLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;

//    private GameObject friendItem;
    private int currentPartyIndex;
    private int partyTotalCount;

	private Dictionary<int, PageUnitView> partyView = new Dictionary<int, PageUnitView>();

    private Dictionary<int, UITexture> partySprite = new Dictionary<int,UITexture>();
    private Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo>();
    private UITexture friendSprite;
    private UnitBaseInfo friendBaseInfo;
	GameObject itemLeft;
//	GameObject dragPanelCell;
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
//	bool exchange = false;
//	List<UILabel> crossShowLabelList = new List<UILabel>();
	List<UnitItemViewInfo> supportViewList = new List<UnitItemViewInfo>();

	private List<TFriendInfo> helperDataList = new List<TFriendInfo>();

	UIButton sortButton;
	private UILabel sortRuleLabel;
	private SortRule curSortRule;


	TFriendInfo selectedHelper;
	uint questID;
	uint stageID;

	private TEvolveStart evolveStart = null;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();
		ShowUIAnimation();
		SetBottomButtonActive(false);
		prevPosition = -1;
		AddCommandListener();
	

		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, curParty);
	
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f, "easetype", iTween.EaseType.linear));       
	}

	void AddCommandListener(){
//		MsgCenter.Instance.AddListener(CommandEnum.AddHelperItem, AddHelperItem);
		MsgCenter.Instance.AddListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
//		MsgCenter.Instance.AddListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}

	void RemoveCommandListener(){
//		MsgCenter.Instance.RemoveListener(CommandEnum.AddHelperItem, AddHelperItem);
		MsgCenter.Instance.RemoveListener(CommandEnum.ChooseHelper, ChooseHelper);
//		MsgCenter.Instance.AddListener(CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}

	public override void HideUI() {
		base.HideUI();
		SetBottomButtonActive(false);
		RemoveCommandListener();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateDragView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragView, cbdArgs);
				break;
			case "DestoryDragView": 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragView, cbdArgs);
                break;
//			case "UpdateViewAfterChooseHelper":
//				CallBackDispatcherHelper.DispatchCallBack(UpdateViewAfterChooseHelper, cbdArgs);
//				break;
            default:
				break;
		}
	}

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", 			transform);
		dragPanelArgs.Add("scrollerScale", 			Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos", 		-105 * Vector3.up);
		dragPanelArgs.Add("position", 					Vector3.zero);
		dragPanelArgs.Add("clipRange", 				new Vector4(0, 0, 640, 220));
		dragPanelArgs.Add("gridArrange", 			UIGrid.Arrangement.Horizontal);
		dragPanelArgs.Add("maxPerLine", 			0);
		dragPanelArgs.Add("scrollBarPosition",		new Vector3(-320, -120, 0));
        dragPanelArgs.Add("cellWidth", 				120);
        dragPanelArgs.Add("cellHeight", 				120);
    }
        
//	DragPanel CreateDragPanel(string name, int count){
//		DragPanel panel = new DragPanel(name, dragPanelCell);
//		panel.CreatUI();
//		panel.AddItem(count, dragPanelCell);
//		return panel;
//	}

	void CreateDragView(object args){

		List<TFriendInfo> dataList = DataCenter.Instance.SupportFriends;
		dragPanel = new DragPanel("FriendHelperDragPanel", HelperUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			HelperUnitView huv = HelperUnitView.Inject(dragPanel.ScrollItem[ i ]);
			huv.Init(dataList[ i ]);
			huv.callback = ClickItem;
			helperDataList.Add(huv.FriendInfo);
		}

		SortHelperByCurRule();
	}

//	void UpdateCrossShow(){
//		if (IsInvoking("CrossShow")){
//			CancelInvoke("CrossShow");
//		}
//		InvokeRepeating("CrossShow", 0f, 1f);
//	}

//	void UpdateSupportInfo(List<UnitItemViewInfo> friendInfoList){
////		Debug.Log("UpdateSupportType(), Start...");
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem [i];
//			UILabel typeLabel = scrollItem.transform.FindChild("Label_Friend_Type").GetComponent<UILabel>();
//			UILabel pointLabel = scrollItem.transform.FindChild("Label_Friend_Point").GetComponent<UILabel>();
//
//			switch (friendInfoList[ i ].HelperItem.FriendState) {
//				case bbproto.EFriendState.FRIENDHELPER : 
//					typeLabel.text = "Support";
//					typeLabel.color = Color.green;
//					pointLabel.color = Color.green;
//					break;
//				case bbproto.EFriendState.ISFRIEND : 
//					typeLabel.text = "Friend";
//					typeLabel.color = Color.yellow;
//					pointLabel.color = Color.yellow;
//					break;
//				default:
//					typeLabel.text = string.Empty;
//					break;
//			}
//			if(friendInfoList[ i ].HelperItem.FriendPoint != 0){
//				pointLabel.text = string.Format("{0}pt", friendInfoList[ i ].HelperItem.FriendPoint.ToString());
//			}
//			else{
//				pointLabel.text = string.Empty;
//			}
//		}
//	}

//	void FindCrossShowLabelList(){
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem [i];
//			UILabel label = scrollItem.transform.FindChild("Label_Info").GetComponent<UILabel>();
//			crossShowLabelList.Add(label);
//		}
//	}

//	void CrossShow(){
//		if (exchange){
//			for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//				GameObject scrollItem = dragPanel.ScrollItem [i];
//				crossShowLabelList [i].text = "Lv" + supportViewList [i].CrossShowTextBefore;
//				crossShowLabelList [i].color = Color.yellow;
//			}
//			exchange = false;
//		}
//		else{
//			for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//				GameObject scrollItem = dragPanel.ScrollItem [i];
//				if(supportViewList [i].CrossShowTextAfter == "0") continue;
//				else{
//					crossShowLabelList [ i ].text = "+" + supportViewList [i].CrossShowTextAfter;
//					crossShowLabelList [ i ].color = Color.red;
//				}
//			}
//			exchange = true;
//		}
//	}


//	void ShowFriendName(List<UnitItemViewInfo> friendInfoList){
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem [i];
//			UILabel nameLabel = scrollItem.transform.FindChild("Label_Name").GetComponent<UILabel>();
//			nameLabel.text = friendInfoList[ i ].HelperItem.NickName;
//		}
//	}

	void ClickItem(HelperUnitView item){
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.FriendSelect && DataCenter.gameStage == GameState.Evolve) {
			return;
		}

//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickItem", dragPanel.ScrollItem.IndexOf(item.gameObject));
//		ExcuteCallback(cbdArgs);
		int pos = dragPanel.ScrollItem.IndexOf(item.gameObject);
		ShowHelperInfo(pos);

		prevPosition = dragPanel.ScrollItem.IndexOf(item.gameObject);
	}
//	
//	void PressItem(GameObject item){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("PressItem", dragPanel.ScrollItem.IndexOf(item));
//                ExcuteCallback(cbdArgs);
//    }
	
//	void UpdateEventListener(){
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem [i];
//			UIEventListenerCustom.Get(scrollItem).onClick = ClickItem;
//            UIEventListenerCustom.Get(scrollItem).LongPress = PressItem;
//		}
//	}

//	void UpdateAvatarTexture(List<UnitItemViewInfo> friendInfoList){
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem[ i ];
//
//			UISprite typeSpr = scrollItem.transform.FindChild("Sprite_Type").GetComponent<UISprite>();
//			typeSpr.color = friendInfoList[ i ].TypeColor;
//		}
//	}
//

    private void InitUI() {
		sortButton = FindChild<UIButton>("SortButton");
		UIEventListener.Get(sortButton.gameObject).onClick = ClickSortButton;
		sortRuleLabel = sortButton.transform.FindChild("Label").GetComponent<UILabel>();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		sortRuleLabel.text = curSortRule.ToString();

        friendBaseInfo = DataCenter.Instance.FriendBaseInfo;
		bottomButton = FindChild<UIImageButton>("Button_QuestStart");
//		dragPanelCell = Resources.Load("Prefabs/UI/Friend/AvailFriendItem") as GameObject;
		InitDragPanelArgs();
		FindItemLeft();
		InitPagePanel();
    }

	void UpdateViewAfterChooseHelper(){
		bottomButton.isEnabled = true;
		UIEventListener.Get(bottomButton.gameObject).onClick = ClickBottomButton;
		LightClickItem();
	}

	int prevPosition = -1;
	UISprite prevSprite;

	void LightClickItem(){
		Debug.LogError("FriendHelperView.LightClickItem()...");
		if(prevPosition == -1) return;
		GameObject pickedItem = dragPanel.ScrollItem[ prevPosition ];
		UISprite lightSpr = pickedItem.transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		if(lightSpr == null) {
			Debug.LogError("lightSpr is null");
			return;
		}
		if(prevSprite != null) {
			if(lightSpr.Equals( prevSprite))
				return;
			else
				prevSprite.enabled = false;
		}
		
		lightSpr.enabled = true;
		prevSprite = lightSpr;
	}

	void ClickBottomButton(GameObject btn){
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickBottomButton", null);
		bottomButton.isEnabled = false;
		QuestStart();
//		ExcuteCallback(cbdArgs);
	}

//    void ClickStartBtn(GameObject btn) {
//        AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
////        RequestStartQuest();
//    }
	
	void DestoryDragView(object args){
//		crossShowLabelList.Clear();
		supportViewList.Clear();
		if (dragPanel.DragPanelView == null) {
			return;	
		}
		foreach (var item in dragPanel.ScrollItem){
			GameObject.Destroy(item);
		}
		dragPanel.ScrollItem.Clear();
		GameObject.Destroy(dragPanel.DragPanelView.gameObject);
	}

	void SetBottomButtonActive(bool active){
		bottomButton.isEnabled = active;
	}

	void AddHelperItem(TFriendInfo tfi ){
//		TFriendInfo tfi = msg as TFriendInfo;
//		Refresh (tfi);
		Texture2D tex = tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
		UITexture uiTexture = itemLeft.transform.FindChild("Texture").GetComponent<UITexture>();
		uiTexture.mainTexture = tex;
	}

//	void Refresh (TFriendInfo tfi) {
//
//		Texture2D tex = tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
//		UITexture uiTexture = itemLeft.transform.FindChild("Texture").GetComponent<UITexture>();
//		uiTexture.mainTexture = tex;
//	}

	void FindItemLeft(){
		itemLeft = transform.FindChild("Item_Left").gameObject;
	}

	private void InitPagePanel(){
		rightIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;
		
		for (int i = 0; i < 4; i++){
			PageUnitView puv = FindChild<PageUnitView>(i.ToString());
			partyView.Add(i, puv);
		}
	}

	void PrevPage(GameObject go){
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, preParty);         
	}

	void NextPage(GameObject go){
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, nextParty);
	}

	void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		rightIndexLabel.text = curPartyIndex.ToString();
		
		//Debug.Log("Current party's member count is : " + partyMemberList.Count);
		for (int i = 0; i < partyMemberList.Count; i++){
			partyView[ i ].Init(partyMemberList [ i ]);
		}
	}

	private void ClickSortButton(GameObject btn){
		curSortRule = SortUnitTool.GetNextRule(curSortRule);
		SortHelperByCurRule();
	}

	private void SortHelperByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();
		List<TUserUnit> unitList = new List<TUserUnit>();
		for (int i = 0; i < helperDataList.Count; i++){
			unitList.Add(helperDataList[ i ].UserUnit);
		}

		SortUnitTool.SortByTargetRule(curSortRule, unitList);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			HelperUnitView huv = dragPanel.ScrollItem[ i ].GetComponent<HelperUnitView>();
			huv.UserUnit = helperDataList[ i ].UserUnit;
			huv.CurrentSortRule = curSortRule;
		}
	}

	void ShowHelperInfo(int pos){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
//		TFriendInfo helper = DataCenter.Instance.SupportFriends[ pos ];

		RecordSelectedHelper(helperDataList[ pos ]);
		MsgCenter.Instance.Invoke(CommandEnum.FriendBriefInfoShow, helperDataList[ pos ]);
	}
	
	void RecordSelectedHelper(TFriendInfo tfi){
		selectedHelper = tfi;
	}

	void ChooseHelper(object msg){
		if(selectedHelper == null) { 
			Debug.Log("selectedHelper is NULL, return...");
			return;
		}
		AddHelperItem(selectedHelper);
//		MsgCenter.Instance.Invoke(CommandEnum.AddHelperItem, selectedHelper);
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("UpdateViewAfterChooseHelper", null);
//		ExcuteCallback(cbdArgs);
		UpdateViewAfterChooseHelper();
	}

	void QuestStart(){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		
		if (DataCenter.gameStage == GameState.Evolve) {
			evolveStart.EvolveStart.restartNew = 1;
			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} 
		else {
			StartQuest sq = new StartQuest ();
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = selectedHelper;
			sqp.questId = questID;
			sqp.stageId = stageID;
			sqp.startNew = 1;
			sq.OnRequest (sqp, RspStartQuest);
		}
	}

	void RecordSelectedQuest(object msg){
		Dictionary<string,uint> idArgs = msg as Dictionary<string,uint>;
		questID = idArgs["QuestID"];
		stageID = idArgs["StageID"];
	}

	void RspEvolveStartQuest (object data) {
		if (data == null){
			//			Debug.Log("OnRspEvolveStart(), response null");
			return;
		}
		evolveStart.StoreData ();
		
		bbproto.RspEvolveStart rsp = data as bbproto.RspEvolveStart;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			LogHelper.LogError("RspEvolveStart code:{0}, error:{1}", rsp.header.code, rsp.header.error);
			return;
		}
		// TODO do evolve start over;
		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
		bbproto.QuestDungeonData questDungeonData = rsp.dungeonData;
		TQuestDungeonData tqdd = new TQuestDungeonData (questDungeonData);
		ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		
		EnterBattle ();
	}
	
	void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		//		Debug.LogError (rspStartQuest.header.code  + "  " + rspStartQuest.header.error);
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
			ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		}
		
		if (data == null || tqdd == null) {
			//			Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
			return;
		}
		EnterBattle ();
	} 
	
	void EnterBattle () {
		DataCenter.Instance.BattleFriend = selectedHelper;
		UIManager.Instance.EnterBattle();
	} 
	
	MsgWindowParams GetStartQuestError () {
		MsgWindowParams mwp = new MsgWindowParams ();
		return mwp;
	}

}
