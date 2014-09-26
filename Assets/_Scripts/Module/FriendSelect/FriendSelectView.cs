using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FriendSelectView : ViewBase{

	public System.Action<FriendInfo> selectFriend;
	public EvolveItem evolveItem;

	protected DragPanel generalDragPanel = null;
	protected DragPanel premiumDragPanel = null;

	protected UILabel sortRuleLabel;
	protected SortRule curSortRule;
	protected UIButton premiumBtn;
	protected UILabel premiumBtnLabel;

	protected List<FriendInfo> generalFriendList;
	protected List<FriendInfo> premiumFriendList;

	protected FriendInfoType friendInfoTyp = FriendInfoType.General;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init(config,data);
		InitUI();
		transform.localPosition -= transform.parent.localPosition;
		premiumBtn.gameObject.SetActive (false);
	}

	public override void ShowUI() {
		base.ShowUI();
		CreateGeneralListView();
		ShowUIAnimation(generalDragPanel);
		isShowPremium = false;
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (premiumBtn.gameObject.activeSelf) {
			premiumBtn.gameObject.SetActive(false);	
		}

		if (viewData != null) {
			if(viewData.ContainsKey("type")){
				if(viewData["type"].ToString() == "evolve"){
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"evolve");
					evolveItem = viewData["item"] as EvolveItem;
				}else if(viewData["type"].ToString() == "levelup"){
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"levelup");
					CheckFriend();

				}else if(viewData["type"].ToString() == "quest"){
					RecordPickedInfoForFight(viewData["data"]);	
				}
			}

		}
	}

	private void InitUI(){
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		premiumBtn = transform.FindChild("Button_Premium").GetComponent<UIButton>();
		premiumBtnLabel = FindChild<UILabel>("Button_Premium/Label");
		premiumBtnLabel.text = TextCenter.GetText("Btn_Premium");
		int manualHeight = Main.Instance.root.manualHeight;
		premiumBtn.transform.localPosition = new Vector3(255, manualHeight/2 - 150, 0);
		UIEventListenerCustom.Get(premiumBtn.gameObject).onClick = ClickPremiumBtn;
	}

	List<FriendInfo> GetPremiumData(){
		List<FriendInfo> tfiList = new List<FriendInfo>();
		return tfiList;
	}
	
	private void CreatePremiumListView(){
		List<FriendInfo> newest = GetPremiumData();

		if(premiumFriendList == null){
			premiumFriendList = newest;
			premiumDragPanel = RefreshDragView(FriendInfoType.Premium);
		} else {
			if(!premiumFriendList.Equals(newest)){
				premiumFriendList = newest;
				premiumDragPanel = RefreshDragView(FriendInfoType.Premium);
			} else {
//				Debug.Log("CreatePremiumListView(), the friend info list is NOT CHANGED, do nothing...");
			}
		}
	}

	private void CreateGeneralListView(){
		List<FriendInfo> newest = DataCenter.Instance.FriendData.GetSupportFriend ();//SupportFriends;
		if(generalFriendList == null){
			generalFriendList = newest;
			generalDragPanel = RefreshDragView(FriendInfoType.General);
		} else {

			if(!generalFriendList.Equals(newest)){
				generalDragPanel = RefreshDragView(FriendInfoType.General);
			} else {
				generalFriendList = newest;
				RefreshData(generalDragPanel);
			}
		}
	}

	DragPanel RefreshDragView(FriendInfoType fType){
		DragPanel dragPanel = null;
		friendInfoTyp = fType;
		string dragPanelName;
		List<FriendInfo> dataList;

		if(fType == FriendInfoType.General){
			dragPanelName = "GeneralDragPanel";
			dataList = generalFriendList;
		}
		else{
			dragPanelName = "PremiumDragPanel";
			dataList = premiumFriendList;
		}

		if(dragPanel != null){
//			Debug.Log("dragPanel named as " + dragPanel.DragPanelView.gameObject.name + " != NULL, destory->create->refresh...");
			dragPanel.DestoryUI();
		}
		else{
//			Debug.Log("dragPanel == NULL, create->refresh...");
		}

		dragPanel = new DragPanel("FriendSelectDragPanel", HelperUnitItem.ItemPrefab,transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		CustomDragPanel(dragPanel);

		RefreshData (dragPanel);

		return dragPanel;
	}

	void RefreshData (DragPanel dragP) {
		for (int i = 0; i < dragP.ScrollItem.Count; i++){
			if(dragP.ScrollItem[i] == null) {
				continue;
			}

			HelperUnitItem huv = HelperUnitItem.Inject(dragP.ScrollItem[ i ]);
			if(i < generalFriendList.Count) {
				huv.Init(generalFriendList[ i ]);
				huv.callback = ClickHelperItem;
			}
			else{
				GameObject.Destroy (huv.gameObject);
			}
		}

		for (int i = dragP.ScrollItem.Count - 1; i >= 0; i--) {
			if(dragP.ScrollItem[i] == null) {
				dragP.ScrollItem.RemoveAt(i);
			}
		}
	}

	private QuestItemView pickedQuestInfo;
	private void RecordPickedInfoForFight(object msg){
		pickedQuestInfo = msg as QuestItemView;
	}

	protected virtual void ClickHelperItem(HelperUnitItem item){
//		if(viewData["sele"]
		foreach (var i in viewData) {
			Debug.Log("key: " + i.Key);
		}
		if(viewData.ContainsKey("type")){
			if(viewData["type"].ToString() == "evolve"){
				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"evolve");
				ModuleManager.Instance.ShowModule(ModuleEnum.EvolveModule,"friendinfo",item.FriendInfo);
			}else if(viewData["type"].ToString() == "levelup"){
				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"levelup");
				ModuleManager.Instance.ShowModule(ModuleEnum.LevelUpModule,"friendinfo",item.FriendInfo);
				CheckFriend();
				
			}else if(viewData["type"].ToString() == "quest"){
				if(pickedQuestInfo == null){
					AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
					return;
				}
				
				if(CheckStaminaEnough()){
					Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
					AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
					//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaLackNoteTitle"),TextCenter.GetText("StaminaLackNoteContent"),TextCenter.GetText("OK"));
					return;
				}
				//		Debug.LogError("friend ClickHelperItem");
				AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
				
				Dictionary<string, object> pickedInfo = new Dictionary<string, object>();
				pickedInfo.Add("QuestInfo", pickedQuestInfo);
				pickedInfo.Add("HelperInfo", item.FriendInfo);
				
				ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"data",pickedInfo);//before
				//		MsgCenter.Instance.Invoke(CommandEnum.OnPickHelper);//after
			}
		}

	}

	/// <summary>
	/// Checks the stamina enough.
	/// MsgWindow show, note stamina is not enough.
	/// </summary>
	/// <returns><c>true</c>, if stamina enough was checked, <c>false</c> otherwise.</returns>
	/// <param name="staminaNeed">Stamina need.</param>
	/// <param name="staminaNow">Stamina now.</param>
	private bool CheckStaminaEnough(){
		int staminaNeed = pickedQuestInfo.Data.stamina;
		int staminaNow = DataCenter.Instance.UserData.UserInfo.staminaNow;
		if(staminaNeed > staminaNow) return true;
		else return false;
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
	}


	private void ShowUIAnimation(DragPanel dragPannel){
		if(dragPannel == null) return;
		GameObject targetPanel = dragPannel.GetDragViewObject();
		targetPanel.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo (targetPanel, iTween.Hash ("x", 0, "time", 0.4f));//, "oncomplete", "FriendITweenEnd", "oncompletetarget", gameObject));      
	}

	/// <summary>
	/// Customs the drag panel.
	/// Custom this drag panel as vertical drag.
	/// </summary>
	private void CustomDragPanel(DragPanel dragPanel){
		GameObject scrollView = dragPanel.GetDragViewObject().transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.GetDragViewObject().transform.FindChild("Scroll Bar").gameObject;
		GameObject itemRoot = scrollView.transform.FindChild("UIGrid").gameObject;
		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();

		uiScrollView.verticalScrollBar = uiScrollBar;
		uiScrollView.horizontalScrollBar = null;
	}

	bool isShowPremium = false;
	protected void ClickPremiumBtn(GameObject btn){
		UserUnit leader = DataCenter.Instance.UnitData.PartyInfo.CurrentParty.GetUserUnit()[ 0 ];

		EUnitRace race = (EUnitRace)leader.UnitRace;
		EUnitType type = (EUnitType)leader.UnitType;
		int level = leader.level;

		FriendController.Instance.GetPremiumHelper(OnRspGetPremium, race, type, level, 0);
	}

	void OnRspGetPremium(object data){
		if (data == null)
			return;
		RspGetPremiumHelper rsp = data as RspGetPremiumHelper;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}

		List<FriendInfo> rspFriendInfo = rsp.helpers;
		if(rspFriendInfo == null){
			return;
		}

		List<FriendInfo> rspPremiumList = new List<FriendInfo>();

		rspPremiumList.AddRange(rspFriendInfo);

		premiumFriendList = rspPremiumList;

		if(isShowPremium){
			premiumDragPanel.DestoryUI();
			CreateGeneralListView();
			ShowUIAnimation(generalDragPanel);
		}
		else{
			generalDragPanel.DestoryUI();
			CreatePremiumListView();
			ShowUIAnimation(premiumDragPanel);
		}
		isShowPremium = !isShowPremium;
	}
	
	public GameObject GetFriendItem(int i){
		if(generalDragPanel != null)
			return generalDragPanel.ScrollItem[i];
		return null;
	}
	
	void CheckFriend() {
		if (evolveItem == null) {
			return;	
		}
		
		DragPanel dragPanel = null;
		
		HelperRequire hr = evolveItem.userUnit.UnitInfo.evolveInfo.helperRequire;
		
		dragPanel = (friendInfoTyp == FriendInfoType.General ? generalDragPanel : premiumDragPanel);
		
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++) {
			HelperUnitItem hui = dragPanel.ScrollItem[i].GetComponent<HelperUnitItem>();
			if(! CheckEvolve(hr, hui.UserUnit)) {
				hui.IsEnable = false;
			}
		}
	}
	
	bool CheckEvolve(HelperRequire hr, UserUnit tuu) {

		if (tuu.level >= hr.level && ((hr.race == 0) || (tuu.UnitRace == (int)hr.race)) && ((hr.type == 0) || (tuu.UnitType == (int)hr.type))) {
			return true;	
		} else {
			return false;	
		}
	}
}

public enum FriendInfoType{
	General,
	Premium
}
