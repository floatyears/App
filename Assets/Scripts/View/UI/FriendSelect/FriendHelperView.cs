using UnityEngine;
using System.Collections.Generic;

public class FriendHelperView : UIComponentUnity{
	private UnitBaseInfo friendBaseInfo;
	private GameObject itemLeft;
	private DragPanel dragPanel;
    private UIImageButton startQuestBtn;
	private UILabel rightIndexLabel;
	private UIButton prePageButton;
	private UIButton nextPageButton;
    private int currentPartyIndex;
    private int partyTotalCount;

	private UIButton sortBtn;
	private UILabel sortRuleLabel;
	private SortRule curSortRule;

	private TFriendInfo selectedHelper;
	private uint questID;
    private uint stageID;
	private TEvolveStart evolveStart = null;
	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>();
    private Dictionary<int, UITexture> partySprite = new Dictionary<int,UITexture>();
    private Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo>();
   
	private Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();
	private List<UnitItemViewInfo> supportViewList = new List<UnitItemViewInfo>();
	private List<TFriendInfo> helperDataList = new List<TFriendInfo>();

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
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}

	void AddCommandListener(){
		MsgCenter.Instance.AddListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	void RemoveCommandListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.ChooseHelper, ChooseHelper);
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedQuest, RecordSelectedQuest);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	public override void HideUI() {
		base.HideUI();
		SetBottomButtonActive(false);
		RemoveCommandListener();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
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
            default:
				break;
		}
	}
	
	void CreateDragView(object args){
		helperDataList = DataCenter.Instance.SupportFriends;//merge
		dragPanel = new DragPanel("FriendHelperDragPanel", HelperUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(helperDataList.Count);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			HelperUnitItem huv = HelperUnitItem.Inject(dragPanel.ScrollItem[ i ]);
			huv.Init(helperDataList[ i ]);
			huv.callback = ClickItem;
		}
		SortUnitByCurRule();
	}

	void ClickItem(HelperUnitItem item){
		if (UIManager.Instance.baseScene.CurrentScene == SceneEnum.FriendSelect 
		    && DataCenter.gameStage == GameState.Evolve) {
			return;
		}
		int pos = dragPanel.ScrollItem.IndexOf(item.gameObject);
		ShowHelperInfo(pos);

		prevPosition = dragPanel.ScrollItem.IndexOf(item.gameObject);
	}

    private void InitUI() {
		sortBtn = FindChild<UIButton>("SortButton");
		UIEventListener.Get(sortBtn.gameObject).onClick = ClickSortBtn;
		sortRuleLabel = sortBtn.transform.FindChild("Label").GetComponent<UILabel>();

		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		sortRuleLabel.text = curSortRule.ToString();

        friendBaseInfo = DataCenter.Instance.FriendBaseInfo;
		startQuestBtn = FindChild<UIImageButton>("Button_QuestStart");

		FindItemLeft();
		InitPagePanel();
    }

	void UpdateViewAfterChooseHelper(){
		startQuestBtn.isEnabled = true;
		UIEventListener.Get(startQuestBtn.gameObject).onClick = ClickBottomButton;
		LightClickItem();
	}

	int prevPosition = -1;
	UISprite prevSprite;

	void LightClickItem(){
//		Debug.LogError("FriendHelperView.LightClickItem()...");
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
		startQuestBtn.isEnabled = false;
		QuestStart();
	}

	void DestoryDragView(object args){
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
		startQuestBtn.isEnabled = active;
	}

	void AddHelperItem(TFriendInfo tfi ){
		Texture2D tex = tfi.UserUnit.UnitInfo.GetAsset(UnitAssetType.Avatar);
//		Debug.LogError ("AddHelperItem : " + itemLeft + " time : " + Time.realtimeSinceStartup);
		if (itemLeft == null) {
			FindItemLeft();
		}
		UITexture uiTexture = itemLeft.transform.FindChild("Texture").GetComponent<UITexture>();
		uiTexture.mainTexture = tex;
	}

	void FindItemLeft(){
		itemLeft = transform.FindChild("Item_Left").gameObject;
//		Debug.LogError ("FindItemLeft : " + itemLeft + " time : " + Time.realtimeSinceStartup);
	}

	private void InitPagePanel(){
		rightIndexLabel = FindChild<UILabel>("Label_Cur_Party");
		prePageButton = FindChild<UIButton>("Button_Left");
		UIEventListener.Get(prePageButton.gameObject).onClick = PrevPage;
		nextPageButton = FindChild<UIButton>("Button_Right");
		UIEventListener.Get(nextPageButton.gameObject).onClick = NextPage;
		
		for (int i = 0; i < 4; i++){
			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
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

	private void ClickSortBtn(GameObject btn){
		MsgCenter.Instance.Invoke(CommandEnum.OpenSortRuleWindow, true);
	}

	private void ReceiveSortInfo(object msg){
		//Debug.LogError("FriendHelper.ReceiveSortInfo()...");
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}

	private void SortUnitByCurRule(){
		sortRuleLabel.text = curSortRule.ToString();
//		List<TUserUnit> unitList = new List<TUserUnit>();
//		for (int i = 0; i < helperDataList.Count; i++){
//			unitList.Add(helperDataList[ i ].UserUnit);
//		}

		SortUnitTool.SortByTargetRule(curSortRule, helperDataList);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			HelperUnitItem huv = dragPanel.ScrollItem[ i ].GetComponent<HelperUnitItem>();
			//Debug.Log(string.Format("SortUnitByCurRule :: Before:: ScrollItem's index -> {0}, huv's addPoint -> {1}", i, huv.UserUnit.AddNumber));
			huv.UserUnit = helperDataList[ i ].UserUnit;
			huv.CurrentSortRule = curSortRule;
		}
	}

	void ShowHelperInfo(int pos){
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
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
			//	Debug.Log("OnRspEvolveStart(), response null");
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
		//Debug.LogError (rspStartQuest.header.code  + "  " + rspStartQuest.header.error);
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
			ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		}
		
		if (data == null || tqdd == null) {
			//Debug.LogError("Request quest info fail : data " + data + "  TQuestDungeonData : " + tqdd);
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
