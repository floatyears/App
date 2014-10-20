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

	private QuestItemView pickedQuestInfo;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null) {
		base.Init(config,data);
		InitUI();
		transform.localPosition -= transform.parent.localPosition;
		premiumBtn.gameObject.SetActive (false);

		dragPanel = new DragPanel("FriendSelectDragPanel", "Prefabs/UI/UnitItem/HelperUnitPrefab" ,typeof(HelperUnitItem), transform);
	
		premiumBtn.gameObject.SetActive(false);	
	}

	public override void ShowUI() {
		base.ShowUI();
		dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.GetSupportFriend (), ClickHelperItem as DataListener);
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST);

		if (viewData != null) {
			if(viewData.ContainsKey("type")){
				if(viewData["type"].ToString() == "evolve"){
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"evolve");
					evolveItem = viewData["item"] as EvolveItem;
				}else if(viewData["type"].ToString() == "level_up"){
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"level_up");
//					CheckFriend();
				}else if(viewData["type"].ToString() == "quest"){
					ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"quest");
					pickedQuestInfo = viewData["data"] as QuestItemView;	
				}
			}
			dragPanel.SetData<FriendInfo> (DataCenter.Instance.FriendData.GetSupportFriend (), ClickHelperItem as DataListener,viewData["type"].ToString());
		}


		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.FRIEND_SELECT);
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

	public override void HideUI ()
	{
		base.HideUI ();
	}

//	private void CreatePremiumListView(){
//		List<FriendInfo> newest = GetPremiumData();
//
//		if(premiumFriendList == null){
//			premiumFriendList = newest;
//			dragPanel.SetData<FriendInfo> (premiumFriendList, ClickHelperItem as DataListener);
//		} else {
//			if(!premiumFriendList.Equals(newest)){
//				premiumFriendList = newest;
//				dragPanel.SetData<FriendInfo> (premiumFriendList, ClickHelperItem as DataListener);
//				
//			} else {
////				Debug.Log("CreatePremiumListView(), the friend info list is NOT CHANGED, do nothing...");
//			}
//		}
//	}



	void ClickHelperItem(object data){
//		if(viewData["sele"]
		HelperUnitItem item = data as HelperUnitItem;
		foreach (var i in viewData) {
			Debug.Log("key: " + i.Key);
		}
		if(viewData.ContainsKey("type")){
			if(viewData["type"].ToString() == "evolve"){
				ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"evolve");
				ModuleManager.Instance.ShowModule(ModuleEnum.EvolveModule,"friendinfo",item.FriendInfo);
			}else if(viewData["type"].ToString() == "level_up"){
				ModuleManager.Instance.HideModule(ModuleEnum.FriendSelectModule);
				ModuleManager.Instance.ShowModule(ModuleEnum.UnitLevelupAndEvolveModule,"friend_info",item.FriendInfo);
//				CheckFriend();
				
			}else if(viewData["type"].ToString() == "quest"){
				if(pickedQuestInfo == null){
					AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
					return;
				}
				
				if(CheckStaminaEnough()){
					Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
					AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("StaminaLackNoteTitle"),TextCenter.GetText("StaminaLackNoteContent"),TextCenter.GetText("OK"));
					return;
				}
				AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
				
				ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo", pickedQuestInfo,"HelperInfo", item.FriendInfo);//before
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

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			//			Debug.Log("Show Module!: [[[---" + config.moduleName + "---]]]pos: " + config.localPosition.x + " " + config.localPosition.y);
			gameObject.SetActive(true);
			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);

			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);
			iTween.MoveTo (gameObject, iTween.Hash ("x", config.localPosition.x, "time", 0.4f));//, "oncomplete", "FriendITweenEnd", "oncompletetarget", gameObject));   
			//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f, "islocal", true));
		}else{
			//			Debug.Log("Hide Module!: [[[---" + config.moduleName + "---]]]");
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
			//			iTween.MoveTo(gameObject, iTween.Hash("x", -1000, "time", 0.4f, "islocal", true,"oncomplete","AnimationComplete","oncompletetarget",gameObject));
		}
	}


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

//		CreateGeneralListView();
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
