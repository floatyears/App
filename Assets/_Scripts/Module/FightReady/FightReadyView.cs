using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class FightReadyView : ViewBase, IDragChangeView {
	private int prevPageIndex = 0;
	public const int PARTY_LIGHT_COUNT = 5;
	public const int PARTY_MEMBER_COUNT = 4;

	private UIButton prePageBtn;
	private UIButton nextPageBtn;

//	private UILabel totalHPLabel;
//	private UILabel totalAtkLabel;
//	private UILabel lightAtkLabel;
//	private UILabel darkAtkLabel;
//	private UILabel fireAtkLabel;
//	private UILabel waterAtkLabel;
//	private UILabel windAtkLabel;
//	private UILabel wuAtkLabel;
//
//	private UILabel helperSkillNameLabel;
//	private UILabel helperSkillDcspLabel;
//	private UILabel ownSkillNameLabel;
//	private UILabel ownSkillDscpLabel;


	private UIButton startFightBtn;
//	private HelperUnitItem helper;
//	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>();

//	public override void Init(UIInsConfig config, IUICallback origin){
//		base.Init(config, origin);
//		InitUI();
//	}

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		InitUI();
	}

	public override void ShowUI() {
		base.ShowUI();
		AddCmdLisenter();
		ShowUIAnimation();
		RecordPickedInfoForFight (viewData["data"]);
		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);
	}

	public override void HideUI(){
		base.HideUI();
		RmvCmdListener();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		RmvCmdListener();
	}

	private DragSliderBase dragSlider;

	#region IDragChangeView implementation

	public void RefreshParty (bool isRight) {	
		TUnitParty tup = null;
		if (isRight) {
			tup = DataCenter.Instance.PartyInfo.PrevParty;
		} else {
			tup = DataCenter.Instance.PartyInfo.NextParty;
		}
//		int curPartyIndex = DataCenter.Instance.PartyInfo.CurrentPartyId + 1;
		RefreshParty();  
		MsgCenter.Instance.Invoke(CommandEnum.RefreshPartyPanelInfo, tup);   
	}

	public void RefreshView (List<PageUnitItem> view)
	{

	}

	public int xInterv {
		get {
			return 620;
		}
	}

	#endregion

	private void InitUI(){
		FindChild<UILabel>("Button_Fight/Label").text = TextCenter.GetText ("Btn_Fight");

		prePageBtn = FindChild<UIButton>("Others/Button_Left");
		nextPageBtn = FindChild<UIButton>("Others/Button_Right");
		startFightBtn = transform.FindChild("Button_Fight").GetComponent<UIButton>();

//		totalHPLabel = transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
//		totalAtkLabel = transform.FindChild("Label_Total_ATK").GetComponent<UILabel>();
//		fireAtkLabel = transform.FindChild("Label_ATK_Fire").GetComponent<UILabel>();
//		waterAtkLabel = transform.FindChild("Label_ATK_Water").GetComponent<UILabel>();
//		lightAtkLabel = transform.FindChild("Label_ATK_Light").GetComponent<UILabel>();
//		darkAtkLabel = transform.FindChild("Label_ATK_Dark").GetComponent<UILabel>();
//		windAtkLabel = transform.FindChild("Label_ATK_Wind").GetComponent<UILabel>();
//		wuAtkLabel = transform.FindChild("Label_ATK_Wu").GetComponent<UILabel>();
//
//		helperSkillNameLabel = transform.FindChild("Label_Helper_Leader_Skill_Name").GetComponent<UILabel>();
//		helperSkillDcspLabel = transform.FindChild("Label_Helper_Skill_Dscp").GetComponent<UILabel>();
//		ownSkillNameLabel = transform.FindChild("Label_Own_Leader_Skill_Name").GetComponent<UILabel>();
//		ownSkillDscpLabel = transform.FindChild("Label_Own_Skill_Dscp").GetComponent<UILabel>();
//		partyNoLabel = transform.FindChild ("Label_Party_No").GetComponent<UILabel> ();

		UIEventListener.Get(startFightBtn.gameObject).onClick = ClickFightBtn;
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

		dragSlider = GetComponent<DragSliderBase>();
		dragSlider.SetDataInterface (this);
//		for (int i = 0; i < 4; i++){
//			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
//			partyView.Add(i, puv);
//		}
//		helper = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
	}
	
	private void PrevPage(GameObject go){
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty();  
	}
	
	private void NextPage(GameObject go){
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty();
	}

	private void RefreshParty(){
		dragSlider.RefreshData ();
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}

	public Dictionary<string, object> pickedInfoForFight;
	static public TFriendInfo pickedHelperInfo;

	private void RecordPickedInfoForFight(object msg){
//		Debug.LogError ("RecordPickedInfoForFight msg : " + msg);

		pickedInfoForFight = msg as Dictionary<string, object>;

//		foreach (var item in pickedInfoForFight) {
//			Debug.LogError("item.key : " + item.Key + " item.value : " + item.Value);
//		}

		pickedHelperInfo = pickedInfoForFight[ "HelperInfo"] as TFriendInfo;
//		ShowHelper(pickedHelperInfo);
//		Debug.LogError ("RecordPickedInfoForFight");
		RefreshParty();
	}

	void EvolveSelectQuest(object data) {
		Debug.LogError ("EvolveSelectQuest");
		evolveStart = data as TEvolveStart;
		prePageBtn.isEnabled = false;
		nextPageBtn.isEnabled = false;
		pickedHelperInfo = evolveStart.EvolveStart.friendInfo;
		UnitParty up = new UnitParty ();
		up.id = 5;
		for (int i = 0; i < evolveStart.evolveParty.Count; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = evolveStart.evolveParty[i] == null ? 0 : evolveStart.evolveParty[i].ID;
			up.items.Add(pi);
		}

		TUnitParty tup = new TUnitParty (up);
		dragSlider.StopOperate = true;
		dragSlider.RefreshData (tup);
	}

	void RefreshParty(List<TUserUnit> evolveParty) {
//		for (int i = 0; i < evolveParty.Count; i++){
//			partyView[ i ].Init(evolveParty [ i ]);
//		}
//
//		for (int i = evolveParty.Count; i < partyView.Count; i++) {
//			partyView[ i ].Init(null);
//		}
//		
//		ShowPartyInfo();
	}

	void ShowHelper(TFriendInfo friendInfo) {
		HelperUnitItem helperUnitItem = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
		//Debug.LogError (friendInfo.UserUnit.UnitInfo.GetAsset (UnitAssetType.Avatar));
		helperUnitItem.Init(friendInfo);
		ShowHelperView();
	} 

	private void ClickFightBtn(GameObject btn){
		Debug.Log("StandbyView.ClickFightBtn(), start...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		if (DataCenter.gameState == GameState.Evolve) {
			evolveStart.EvolveStart.restartNew = 1;
			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} else {
			StartQuest sq = new StartQuest ();
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
			QuestItemView questInfo = pickedInfoForFight[ "QuestInfo"] as QuestItemView;
			sqp.questId = questInfo.Data.ID;
			sqp.stageId = questInfo.StageID;
			sqp.startNew = 1;
			sq.OnRequest (sqp, RspStartQuest);
		}
	}

	private TEvolveStart evolveStart;
	
	private void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			TFriendInfo tfi = pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
			tfi.UseTime = GameTimer.GetInstance().GetCurrentSeonds();

			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
//			ModelManager.Instance.(ModelEnum.MapConfig, tqdd);
			DataCenter.Instance.SetData(ModelEnum.MapConfig,tqdd);
		}
		
		if (data == null || tqdd == null) { return; }
			EnterBattle (tqdd);
	} 

	private void RspEvolveStartQuest (object data) {
		if (data == null){ return; }
		evolveStart.StoreData ();
		bbproto.RspEvolveStart rsp = data as bbproto.RspEvolveStart;
		if (rsp.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rsp.header.code+", error:"+rsp.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rsp.header.code);
			return;
		}

		pickedHelperInfo.UseTime = GameTimer.GetInstance ().GetCurrentSeonds ();

		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
		bbproto.QuestDungeonData questDungeonData = rsp.dungeonData;
		TQuestDungeonData tqdd = new TQuestDungeonData (questDungeonData);
//		ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		BattleConfigData.Instance.gameState = (byte)DataCenter.gameState;
		EnterBattle (tqdd);
	}
	
	private void EnterBattle (TQuestDungeonData tqdd) {
		pickedHelperInfo.FriendPoint = 0;
		pickedHelperInfo.UseTime = GameTimer.GetInstance ().GetCurrentSeonds ();

		BattleConfigData.Instance.gotFriendPoint = 0;
		BattleConfigData.Instance.BattleFriend = pickedHelperInfo; //pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		BattleConfigData.Instance.ResetFromServer(tqdd);
		ModuleManager.Instance.EnterBattle ();
//		UIManager.Instance.EnterBattle();

		Umeng.GA.StartLevel ("Quest" + tqdd.QuestId);
	}

//	private void ShowPartyInfo(){
//		if(pickedHelperInfo == null) return;
//		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
////		partyNoLabel.text = DataCenter.Instance.PartyInfo.CurrentPartyId + 1 + "/5";
////		UpdateOwnLeaderSkillInfo(curParty);
////		UpdateHelperLeaderSkillInfo();
////		UpdatePartyAtkInfo(curParty);
//	}

	private void AddCmdLisenter(){
		MsgCenter.Instance.AddListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}
	
	private void RmvCmdListener(){
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}

	private void ShowHelperView(){
//		Debug.Log("ShowHelperView(), Start...");
//		if(pickedHelperInfo == null){
//			Debug.LogError("ShowHelperView(), pickedHelperInfo is NULL,return!");
//			return;
//		}

//		helper.FriendInfo = pickedHelperInfo;
//		helper.UserUnit = pickedHelperInfo.UserUnit;
	}

//	private void UpdateOwnLeaderSkillInfo(TUnitParty curParty){
//		SkillBase skill = curParty.GetLeaderSkillInfo();
//		UpdateLeaderSkillView(skill, ownSkillNameLabel, ownSkillDscpLabel);
//	}

//	private void UpdateHelperLeaderSkillInfo(){
//		if(pickedHelperInfo == null){
//			return;
//		}
//
//		TUnitInfo unitInfo = pickedHelperInfo.UserUnit.UnitInfo;
//		int skillId = unitInfo.LeaderSkill;
//		if(skillId == 0){
//			UpdateLeaderSkillView(null, helperSkillNameLabel, helperSkillDcspLabel);
//		} else {
//			string userUnitKey = pickedHelperInfo.UserUnit.MakeUserUnitKey();
//			SkillBaseInfo baseInfo = DataCenter.Instance.GetSkill(userUnitKey, skillId, SkillType.NormalSkill);
//			SkillBase leaderSkill = baseInfo.GetSkillInfo();	
//			UpdateLeaderSkillView(leaderSkill, helperSkillNameLabel, helperSkillDcspLabel);
//		}
//	}

//	private void UpdateLeaderSkillView(SkillBase skill, UILabel name, UILabel dscp){
//		if(skill == null){
//			name.text = TextCenter.GetText("LeaderSkillText") +  TextCenter.GetText("Text_None");
//			dscp.text = "";
//		}
//		else{
//			name.text = TextCenter.GetText("LeaderSkillText") + TextCenter.GetText("SkillName_" + skill.id);//skill.name;
//			dscp.text = TextCenter.GetText("SkillDesc_" + skill.id);//skill.description;
//		}
//	}
	
//	private void UpdatePartyAtkInfo(TUnitParty curParty){
//		int totalHp = curParty.TotalHp + pickedHelperInfo.UserUnit.Hp;
//		totalHPLabel.text = totalHp.ToString();
//		
//		int totalAtk = curParty.GetTotalAtk() + pickedHelperInfo.UserUnit.Attack;
//		totalAtkLabel.text = totalAtk.ToString();
//
//		int value = 0;
//		curParty.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
//		fireAtkLabel.text = value.ToString();
//		
//		curParty.TypeAttack.TryGetValue (EUnitType.UWATER, out value);
//		waterAtkLabel.text = value.ToString();
//		
//		curParty.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
//		windAtkLabel.text = value.ToString();
//		
//		curParty.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
//		wuAtkLabel.text = value.ToString();
//		
//		curParty.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
//		lightAtkLabel.text = value.ToString();
//		
//		curParty.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
//		darkAtkLabel.text = value.ToString();
//	}
}
