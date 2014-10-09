using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class FriendSelectView : ViewBase{

	public System.Action<FriendInfo> selectFriend;
	public EvolveItem evolveItem;

	protected DragPanel dragPanel = null;

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

		dragPanel = new DragPanel("FriendSelectDragPanel", "Prefabs/UI/UnitItem/HelperUnitPrefab" ,typeof(HelperUnitItem), transform);
	}

	public override void ShowUI() {
		base.ShowUI();
		CreateGeneralListView();
		ShowUIAnimation(dragPanel);
		isShowPremium = false;
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST);

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
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"quest");
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
			dragPanel.SetData<FriendInfo> (premiumFriendList, ClickHelperItem as DataListener);
		} else {
			if(!premiumFriendList.Equals(newest)){
				premiumFriendList = newest;
				dragPanel.SetData<FriendInfo> (premiumFriendList, ClickHelperItem as DataListener);
				
			} else {
//				Debug.Log("CreatePremiumListView(), the friend info list is NOT CHANGED, do nothing...");
			}
		}
	}

	private void CreateGeneralListView(){
		List<FriendInfo> newest = DataCenter.Instance.FriendData.GetSupportFriend ();//SupportFriends;
		if(generalFriendList == null){
			generalFriendList = newest;
			dragPanel.SetData<FriendInfo> (generalFriendList, ClickHelperItem as DataListener);
		} else {

			if(generalFriendList.Equals(newest)){
				generalFriendList = newest;
			}
			dragPanel.SetData<FriendInfo> (generalFriendList, ClickHelperItem as DataListener);
		}
	}

	DragPanel RefreshDragView(FriendInfoType fType){
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

		dragPanel.SetData<FriendInfo> (generalFriendList, ClickHelperItem as DataListener);

		return dragPanel;
	}

	private QuestItemView pickedQuestInfo;
	private void RecordPickedInfoForFight(object msg){
		pickedQuestInfo = msg as QuestItemView;
	}

	protected virtual void ClickHelperItem(object data){
//		if(viewData["sele"]
		HelperUnitItem item = data as HelperUnitItem;
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
			CreateGeneralListView();
			ShowUIAnimation(dragPanel);
		}
		else{
			CreatePremiumListView();
			ShowUIAnimation(dragPanel);
		}
		isShowPremium = !isShowPremium;
	}
	
	public GameObject GetFriendItem(int i){
		if(dragPanel != null)
			return dragPanel.ScrollItem[i];
		return null;
	}
	
	void CheckFriend() {
		if (evolveItem == null) {
			return;	
		}
		
		HelperRequire hr = evolveItem.userUnit.UnitInfo.evolveInfo.helperRequire;
		
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
