using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FriendHelperView : UIComponentUnity{
	protected DragPanel generalDragPanel = null;
	protected DragPanel premiumDragPanel = null;

	protected UILabel sortRuleLabel;
	protected SortRule curSortRule;
	protected UIButton premiumBtn;
	protected UILabel premiumBtnLabel;

	protected List<TFriendInfo> generalFriendList;
	protected List<TFriendInfo> premiumFriendList;

	protected FriendInfoType friendInfoTyp = FriendInfoType.General;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
		transform.localPosition -= transform.parent.localPosition;
		premiumBtn.gameObject.SetActive (false);
	}

	public override void ShowUI() {
		base.ShowUI();
		AddCmdListener();
		CreateGeneralListView();
		ShowUIAnimation(generalDragPanel);
		isShowPremium = false;
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (premiumBtn.gameObject.activeSelf) {
			premiumBtn.gameObject.SetActive(false);	
		}
	}

	public override void HideUI() {
//		Debug.LogError("FriendHelperView HideUI befoure : " + Time.realtimeSinceStartup);
		base.HideUI();
		RmvCmdListener();
//		Debug.LogError ("FriendHelperView HideUI end : " + Time.realtimeSinceStartup);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	private void InitUI(){
		curSortRule = SortUnitTool.DEFAULT_SORT_RULE;
		premiumBtn = transform.FindChild("Button_Premium").GetComponent<UIButton>();
		premiumBtnLabel = premiumBtn.GetComponentInChildren<UILabel>();
		premiumBtnLabel.text = TextCenter.GetText("Btn_Premium");
		int manualHeight = Main.Instance.root.manualHeight;
		premiumBtn.transform.localPosition = new Vector3(255, manualHeight/2 - 150, 0);
		UIEventListener.Get(premiumBtn.gameObject).onClick = ClickPremiumBtn;
	}

	List<TFriendInfo> GetPremiumData(){
		List<TFriendInfo> tfiList = new List<TFriendInfo>();
		return tfiList;
	}
	
	private void CreatePremiumListView(){
		List<TFriendInfo> newest = GetPremiumData();

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
		List<TFriendInfo> newest = DataCenter.Instance.supportFriendManager.GetSupportFriend ();//SupportFriends;

		if(generalFriendList == null){
//			Debug.LogError("CreateGeneralListView(), FIRST step in, create drag panel view...");
			generalFriendList = newest;
//			generalDragPanel = new DragPanel("GeneralDragPanel", HelperUnitItem.ItemPrefab);
//			generalDragPanel.CreatUI();
//			generalDragPanel.AddItem(1);
//			Debug.LogError("generalDragPanel 1 generalFriendList == null : " + generalDragPanel);
			generalDragPanel = RefreshDragView(FriendInfoType.General);
//			Debug.LogError("generalDragPanel 2 : " + generalDragPanel);
		}
		else{
//			Debug.Log("CreateGeneralListView(), NOT FIRST step into FriendHelper scene...");
			if(!generalFriendList.Equals(newest)){
//				Debug.Log("CreateGeneralListView(), the friend info list is CHANGED, update helper list...");
				//helperDragPanel.DestoryUI();
//				generalFriendList = newest;
//				generalDragPanel = new DragPanel("GeneralDragPanel", HelperUnitItem.ItemPrefab);
//				Debug.LogError("generalDragPanel 1 : " + generalDragPanel);
				generalDragPanel = RefreshDragView(FriendInfoType.General);
//				Debug.LogError("generalDragPanel 2 : " + generalDragPanel);
			}
			else{
//				Debug.Log("CreateGeneralListView(), the friend info list is NOT CHANGED, do nothing...");
				generalFriendList = newest;
				RefreshData(generalDragPanel);
			}
		}
	}
	public enum FriendInfoType{
		General,
		Premium
	}

	DragPanel RefreshDragView(FriendInfoType fType){
		DragPanel dragPanel = null;
		friendInfoTyp = fType;
		string dragPanelName;
		List<TFriendInfo> dataList;

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

		dragPanel = new DragPanel("GeneralDragPanel", HelperUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		CustomDragPanel(dragPanel);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);

		RefreshData (dragPanel);

		return dragPanel;
	}

	void RefreshData (DragPanel dragP) {
		for (int i = 0; i < dragP.ScrollItem.Count; i++){
			HelperUnitItem huv = HelperUnitItem.Inject(dragP.ScrollItem[ i ]);
			huv.Init(generalFriendList[ i ]);
			huv.callback = ClickHelperItem;
		}
	}

	private QuestItemView pickedQuestInfo;
	private void RecordPickedInfoForFight(object msg){
		pickedQuestInfo = msg as QuestItemView;
	}

	protected virtual void ClickHelperItem(HelperUnitItem item){
		if(pickedQuestInfo == null){
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
			return;
		}

		if(CheckStaminaEnough()){
			Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}
//		Debug.LogError("friend ClickHelperItem");
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);

		Dictionary<string, object> pickedInfo = new Dictionary<string, object>();
		pickedInfo.Add("QuestInfo", pickedQuestInfo);
		pickedInfo.Add("HelperInfo", item.FriendInfo);

		UIManager.Instance.ChangeScene(SceneEnum.FightReady);//before
		MsgCenter.Instance.Invoke(CommandEnum.OnPickHelper, pickedInfo);//after
	}

	/// <summary>
	/// Checks the stamina enough.
	/// MsgWindow show, note stamina is not enough.
	/// </summary>
	/// <returns><c>true</c>, if stamina enough was checked, <c>false</c> otherwise.</returns>
	/// <param name="staminaNeed">Stamina need.</param>
	/// <param name="staminaNow">Stamina now.</param>
	private bool CheckStaminaEnough(){
		int staminaNeed = pickedQuestInfo.Data.Stamina;
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
		if(staminaNeed > staminaNow) return true;
		else return false;
	}
	
	private MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.GetText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}

	private void ReceiveSortInfo(object msg){
		curSortRule = (SortRule)msg;
	}


	private void ShowUIAnimation(DragPanel dragPannel){
		if(dragPannel == null) return;
		GameObject targetPanel = dragPannel.DragPanelView.gameObject;
		targetPanel.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo (targetPanel, iTween.Hash ("x", 0, "time", 0.4f));//, "oncomplete", "FriendITweenEnd", "oncompletetarget", gameObject));      
	}

	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
	}

	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
	}

	/// <summary>
	/// Customs the drag panel.
	/// Custom this drag panel as vertical drag.
	/// </summary>
	private void CustomDragPanel(DragPanel dragPanel){
		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		GameObject itemRoot = scrollView.transform.FindChild("UIGrid").gameObject;
		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();

		uiScrollView.verticalScrollBar = uiScrollBar;
		uiScrollView.horizontalScrollBar = null;
	}

	bool isShowPremium = false;
	protected void ClickPremiumBtn(GameObject btn){
		TUserUnit leader = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[ 0 ];

		EUnitRace race = (EUnitRace)leader.UnitRace;
		EUnitType type = (EUnitType)leader.UnitType;
		int level = leader.Level;

		GetPremiumHelper.SendRequest(OnRspGetPremium, race, type, level, 0);
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

		List<TFriendInfo> rspPremiumList = new List<TFriendInfo>();

		for (int i = 0; i < rspFriendInfo.Count; i++){
			TFriendInfo tfi = new TFriendInfo(rspFriendInfo[ i ]);
			rspPremiumList.Add(tfi);
		}

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
}
