using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FriendHelperView : UIComponentUnity{
	protected DragPanel generalDragPanel;
	protected DragPanel premiumDragPanel;

	protected UILabel sortRuleLabel;
	protected SortRule curSortRule;
	protected UIButton premiumBtn;
	protected UILabel premiumBtnLabel;

	protected List<TFriendInfo> generalFriendList;
	protected List<TFriendInfo> premiumFriendList;

	public override void Init(UIInsConfig config, IUICallback origin) {
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI() {
		base.ShowUI();
		AddCmdListener();

		CreateGeneralListView();
		ShowUIAnimation(generalDragPanel);
		isShowPremium = false;
	}

	public override void HideUI() {
		base.HideUI();
		//generalDragPanel.DestoryUI();
		RmvCmdListener();
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
		Debug.Log("Create Premium ListView(), start...");

		List<TFriendInfo> newest = GetPremiumData();

		if(premiumFriendList == null){
			Debug.LogError("CreatePremiumListView(), FIRST step in, create drag panel view...");
			premiumFriendList = newest;
			RefreshDragView(premiumDragPanel, FriendInfoType.Premium);
		}
		else{
			Debug.Log("CreatePremiumListView(), NOT FIRST step into FriendHelper scene...");
			if(!premiumFriendList.Equals(newest)){
				premiumFriendList = newest;
				RefreshDragView(premiumDragPanel, FriendInfoType.Premium);
			}
			else{
				Debug.Log("CreatePremiumListView(), the friend info list is NOT CHANGED, do nothing...");
			}
		}
	}

	private void CreateGeneralListView(){
		Debug.Log("Create General ListView(), start...");

		List<TFriendInfo> newest = DataCenter.Instance.SupportFriends;

		if(generalFriendList == null){
			Debug.LogError("CreateGeneralListView(), FIRST step in, create drag panel view...");
			generalFriendList = newest;
			RefreshDragView(generalDragPanel, FriendInfoType.General);
		}
		else{
			Debug.Log("CreateGeneralListView(), NOT FIRST step into FriendHelper scene...");
			if(!generalFriendList.Equals(newest)){
				Debug.Log("CreateGeneralListView(), the friend info list is CHANGED, update helper list...");
				//helperDragPanel.DestoryUI();
				generalFriendList = newest;
				RefreshDragView(generalDragPanel, FriendInfoType.General);
			}
			else{
				Debug.Log("CreateGeneralListView(), the friend info list is NOT CHANGED, do nothing...");
			}
		}
	}
	enum FriendInfoType{
		General,
		Premium
	}

	void RefreshDragView(DragPanel dragPanel, FriendInfoType friendInfoType){
		string dragPanelName;
		List<TFriendInfo> dataList;

		if(friendInfoType == FriendInfoType.General){
			dragPanelName = "GeneralDragPanel";
			dataList = generalFriendList;
		}
		else{
			dragPanelName = "PremiumDragPanel";
			dataList = premiumFriendList;
		}

		if(dragPanel != null){
			Debug.Log("dragPanel named as " + dragPanel.DragPanelView.gameObject.name + " != NULL, destory->create->refresh...");
			dragPanel.DestoryUI();
		}
		else{
			Debug.Log("dragPanel == NULL, create->refresh...");
		}

		dragPanel = new DragPanel(dragPanelName, HelperUnitItem.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(dataList.Count);
		CustomDragPanel(dragPanel);
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);
		//SortUnitByCurRule();
	}

	private QuestItemView pickedQuestInfo;
	private void RecordPickedInfoForFight(object msg){
		pickedQuestInfo = msg as QuestItemView;
	}

	protected virtual void ClickHelperItem(HelperUnitItem item){
		Debug.Log("ClickHelperItem..." + item);
		
		if(pickedQuestInfo == null){
			Debug.LogError("FriendHelerpView.ClickHelperItem(), pickedQuestInfo is NULL, return!!!");
			return;
		}

		if(CheckStaminaEnough()){
			Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}

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
		//Debug.LogError("FriendHelper.ReceiveSortInfo()...");
		curSortRule = (SortRule)msg;
		SortUnitByCurRule();
	}
	
	private void SortUnitByCurRule(){
		//sortRuleLabel.text = curSortRule.ToString();
		SortUnitTool.SortByTargetRule(curSortRule, generalFriendList);

		for (int i = 0; i < generalDragPanel.ScrollItem.Count; i++){
			HelperUnitItem huv = HelperUnitItem.Inject(generalDragPanel.ScrollItem[ i ]);
			huv.Init(generalFriendList[ i ]);
			huv.callback = ClickHelperItem;
		}
	}

	private void ShowUIAnimation(DragPanel dragPannel){
		if(dragPannel == null) return;
		GameObject targetPanel = dragPannel.DragPanelView.gameObject;
		targetPanel.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(targetPanel, iTween.Hash("x", 0, "time", 0.4f));      
	}
	
	private void AddCmdListener(){
		MsgCenter.Instance.AddListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
		MsgCenter.Instance.AddListener(CommandEnum.SortByRule, ReceiveSortInfo);
//		MsgCenter.Instance.AddListener(CommandEnum.ActivateSortBtn, ActivateSortBtn);
	}

	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickQuest, RecordPickedInfoForFight);
		MsgCenter.Instance.RemoveListener(CommandEnum.SortByRule, ReceiveSortInfo);
	}

	/// <summary>
	/// Customs the drag panel.
	/// Custom this drag panel as vertical drag.
	/// </summary>
	private void CustomDragPanel(DragPanel dragPanel){
		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		GameObject itemRoot = scrollView.transform.FindChild("UIGrid").gameObject;
		//scrollBar.transform.Rotate( new Vector3(0, 0, 270) );
		//scrollBar.gameObject.SetActive(false);
		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();

		uiScrollView.verticalScrollBar = uiScrollBar;
		uiScrollView.horizontalScrollBar = null;
	}

	bool isShowPremium = false;
	protected void ClickPremiumBtn(GameObject btn){
		Debug.Log("Click Premium Btn...");

		TUserUnit leader = DataCenter.Instance.PartyInfo.CurrentParty.GetUserUnit()[ 0 ];

		EUnitRace race = (EUnitRace)leader.UnitRace;
		EUnitType type = (EUnitType)leader.UnitType;
		int level = leader.Level;

		GetPremiumHelper.SendRequest(OnRspGetPremium, race, type, level);
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
			Debug.LogError("rspFriendInfo ERROR, NULL!");
			return;
		}

		List<TFriendInfo> rspPremiumList = new List<TFriendInfo>();

		for (int i = 0; i < rspFriendInfo.Count; i++){
			TFriendInfo tfi = new TFriendInfo(rspFriendInfo[ i ]);
			rspPremiumList.Add(tfi);
		}

		premiumFriendList = rspPremiumList;

		Debug.Log("OnRspGetPremium(), premiumFriendList count is : " + premiumFriendList.Count);

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

}
