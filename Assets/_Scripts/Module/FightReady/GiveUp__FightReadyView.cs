using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class GiveUp__FightReadyView : ViewBase {
	private int prevPageIndex = 0;
	public const int PARTY_LIGHT_COUNT = 5;
	public const int PARTY_MEMBER_COUNT = 4;
	private GameObject pageLightRoot;
	
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	
	private UILabel totalHPLabel;
	private UILabel totalAtkLabel;
	private UILabel lightAtkLabel;
	private UILabel darkAtkLabel;
	private UILabel fireAtkLabel;
	private UILabel waterAtkLabel;
	private UILabel windAtkLabel;
	private UILabel wuAtkLabel;
	
	private UILabel helperSkillNameLabel;
	private UILabel helperSkillDcspLabel;
	private UILabel ownSkillNameLabel;
	private UILabel ownSkillDscpLabel;
	private UILabel partyNoLabel;
	
	private UIButton startFightBtn;
	private HelperUnitItem helper;
	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>(); 
	
	private List<GameObject> pageLightList = new List<GameObject>();
	
	public override void Init(UIConfigItem uiconfig, Dictionary<string, object> data = null){
		base.Init(uiconfig, data);
		InitUI();
	}
	
	public override void ShowUI() {
		base.ShowUI();

		if (viewData != null) {
			RecordPickedInfoForFight (viewData ["data"]);
		}
		AddCmdLisenter(); 
		ShowUIAnimation();
		
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
	
	private void InitUI(){
		pageLightRoot = transform.FindChild("PageLight").gameObject;
		for (int i = 0; i < PARTY_LIGHT_COUNT; i++){
			GameObject curPageLight = pageLightRoot.transform.FindChild(i.ToString()).gameObject;
			pageLightList.Add(curPageLight);
			UIEventListener.Get(curPageLight).onClick = ClickPageLight;
		}
		
		FindChild<UILabel>("Button_Fight/Label").text = TextCenter.GetText ("Btn_Fight");
		
		prePageBtn = FindChild<UIButton>("Button_Left");
		nextPageBtn = FindChild<UIButton>("Button_Right");
		startFightBtn = transform.FindChild("Button_Fight").GetComponent<UIButton>();
		totalHPLabel = transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		totalAtkLabel = transform.FindChild("Label_Total_ATK").GetComponent<UILabel>();
		
		fireAtkLabel = transform.FindChild("Label_ATK_Fire").GetComponent<UILabel>();
		waterAtkLabel = transform.FindChild("Label_ATK_Water").GetComponent<UILabel>();
		lightAtkLabel = transform.FindChild("Label_ATK_Light").GetComponent<UILabel>();
		darkAtkLabel = transform.FindChild("Label_ATK_Dark").GetComponent<UILabel>();
		windAtkLabel = transform.FindChild("Label_ATK_Wind").GetComponent<UILabel>();
		wuAtkLabel = transform.FindChild("Label_ATK_Wu").GetComponent<UILabel>();
		
		helperSkillNameLabel = transform.FindChild("Label_Helper_Leader_Skill_Name").GetComponent<UILabel>();
		helperSkillDcspLabel = transform.FindChild("Label_Helper_Skill_Dscp").GetComponent<UILabel>();
		ownSkillNameLabel = transform.FindChild("Label_Own_Leader_Skill_Name").GetComponent<UILabel>();
		ownSkillDscpLabel = transform.FindChild("Label_Own_Skill_Dscp").GetComponent<UILabel>();
		partyNoLabel = transform.FindChild ("Label_Party_No").GetComponent<UILabel> ();
		
		UIEventListener.Get(startFightBtn.gameObject).onClick = ClickFightBtn;
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;
		
		for (int i = 0; i < 4; i++){
			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
			partyView.Add(i, puv);
		}
		helper = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
		InitPageLight();
	}
	
	private void PrevPage(GameObject go){
		UnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
	}
	
	private void NextPage(GameObject go){
		UnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
	}
	
	private void RefreshParty(UnitParty party){
		List<UserUnit> partyMemberList = party.GetUserUnit();
		for (int i = 0; i < partyMemberList.Count; i++) {
			partyView[ i ].Init(partyMemberList [ i ]);	
		}
		
		ShowPartyInfo();
	}
	
	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}
	
	private Dictionary<string, object> pickedInfoForFight;
	private FriendInfo pickedHelperInfo;
	
	private void RecordPickedInfoForFight(object msg){
		pickedInfoForFight = msg as Dictionary<string, object>;
		pickedHelperInfo = pickedInfoForFight[ "HelperInfo"] as FriendInfo;
		ShowHelper(pickedHelperInfo);
		RefreshParty(DataCenter.Instance.PartyInfo.CurrentParty);
	}
	
	void EvolveSelectQuest(object data) {
		evolveStart = data as UnitDataModel;
		RefreshParty (evolveStart.evolveParty);
		prePageBtn.isEnabled = false;
		nextPageBtn.isEnabled = false;
		
//		pickedHelperInfo = evolveStart.EvolveStart.friendInfo;
		ShowHelper (pickedHelperInfo);
	}
	
	void RefreshParty(List<UserUnit> evolveParty) {
		for (int i = 0; i < evolveParty.Count; i++){
			partyView[ i ].Init(evolveParty [ i ]);
		}
		
		for (int i = evolveParty.Count; i < partyView.Count; i++) {
			partyView[ i ].Init(null);
		}
		
		ShowPartyInfo();
	}
	
	void ShowHelper(FriendInfo friendInfo) {
		HelperUnitItem helperUnitItem = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
		//Debug.LogError (friendInfo.UserUnit.UnitInfo.GetAsset (UnitAssetType.Avatar));
		helperUnitItem.Init(friendInfo);
		ShowHelperView();
	} 
	
	private void ClickFightBtn(GameObject btn){
		Debug.Log("StandbyView.ClickFightBtn(), start...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		StartFight();
	}
	
	private UnitDataModel evolveStart;
	
	private void StartFight(){
		if (DataCenter.gameState == GameState.Evolve) {
//			evolveStart.EvolveStart.restartNew = 1;
//			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} else {
//			StartQuest sq = new StartQuest ();
			StartQuestParam sqp = new StartQuestParam ();
			sqp.currPartyId = DataCenter.Instance.PartyInfo.CurrentPartyId;
			sqp.helperUserUnit = pickedInfoForFight[ "HelperInfo" ] as FriendInfo;
			QuestItemView questInfo = pickedInfoForFight[ "QuestInfo"] as QuestItemView;
			sqp.questId = questInfo.Data.id;
			sqp.stageId = questInfo.StageID;
			sqp.startNew = 1;
			QuestController.Instance.StartQuest (sqp, RspStartQuest);
		}
	}
	
	private void RspStartQuest(object data) {
		QuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			FriendInfo tfi = pickedInfoForFight[ "HelperInfo" ] as FriendInfo;
			tfi.usedTime = GameTimer.GetInstance().GetCurrentSeonds();
			
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.staminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.staminaRecover = rspStartQuest.staminaRecover;
			tqdd = rspStartQuest.dungeonData;
			DataCenter.Instance.SetData(ModelEnum.MapConfig, tqdd);
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
		
		pickedHelperInfo.usedTime = GameTimer.GetInstance ().GetCurrentSeonds ();
		
		DataCenter.Instance.UserInfo.staminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.staminaRecover = rsp.staminaRecover;
		QuestDungeonData tqdd = rsp.dungeonData;
		DataCenter.Instance.SetData(ModelEnum.MapConfig, tqdd);
		BattleConfigData.Instance.gameState = (byte)DataCenter.gameState;
		EnterBattle (tqdd);
	}
	
	private void EnterBattle (QuestDungeonData tqdd) {
		pickedHelperInfo.friendPoint = 0;
		pickedHelperInfo.usedTime = GameTimer.GetInstance ().GetCurrentSeonds ();
		
		BattleConfigData.Instance.gotFriendPoint = 0;
		BattleConfigData.Instance.BattleFriend = pickedHelperInfo; //pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		BattleConfigData.Instance.ResetFromServer(tqdd);
		ModuleManager.Instance.EnterBattle ();
		
		//		Umeng.GA.StartLevel ("Quest" + tqdd.QuestId);
	}
	
	private void ShowPartyInfo(){
		if(pickedHelperInfo == null) return;
		UnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		partyNoLabel.text = DataCenter.Instance.PartyInfo.CurrentPartyId + 1 + "/5";
		UpdateOwnLeaderSkillInfo(curParty);
		UpdateHelperLeaderSkillInfo();
		UpdatePartyAtkInfo(curParty);
		UpdatePageLight(curParty.id);
	}
	
	private void AddCmdLisenter(){
//		MsgCenter.Instance.AddListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}
	
	private void RmvCmdListener(){
//		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSelectQuest, EvolveSelectQuest);
	}
	
	private void ShowHelperView(){
		Debug.Log("ShowHelperView(), Start...");
		if(pickedHelperInfo == null){
			Debug.LogError("ShowHelperView(), pickedHelperInfo is NULL,return!");
			return;
		}
		
		helper.FriendInfo = pickedHelperInfo;
		helper.UserUnit = pickedHelperInfo.UserUnit;
	}
	
	private void UpdateOwnLeaderSkillInfo(UnitParty curParty){
		SkillBase skill = curParty.GetLeaderSkillInfo();
		UpdateLeaderSkillView(skill, ownSkillNameLabel, ownSkillDscpLabel);
	}
	
	private void UpdateHelperLeaderSkillInfo(){
		if(pickedHelperInfo == null){
			//			Debug.LogError("pickedHelperInfo is null, return!");
			return;
		}
		
		UnitInfo unitInfo = pickedHelperInfo.UserUnit.UnitInfo;
		int skillId = unitInfo.leaderSkill;
		if(skillId == 0){
			//			Debug.Log("UpdateHelperLeaderSkillInfo(), skillId == 0, do not have leader skill!");
			UpdateLeaderSkillView(null, helperSkillNameLabel, helperSkillDcspLabel);
		} else {
			string userUnitKey = pickedHelperInfo.UserUnit.MakeUserUnitKey();
			SkillBase baseInfo = DataCenter.Instance.GetSkill(userUnitKey, skillId, SkillType.NormalSkill);
			SkillBase leaderSkill = baseInfo;	
			UpdateLeaderSkillView(leaderSkill, helperSkillNameLabel, helperSkillDcspLabel);
		}
	}
	
	private void UpdateLeaderSkillView(SkillBase skill, UILabel name, UILabel dscp){
		if(skill == null){
			//			Debug.LogError("Leader skill is Null");
			name.text = TextCenter.GetText("LeaderSkillText") +  TextCenter.GetText("Text_None");
			dscp.text = "";
		}
		else{
			name.text = TextCenter.GetText("LeaderSkillText") + TextCenter.GetText("SkillName_" + skill.id);//skill.name;
			dscp.text = TextCenter.GetText("SkillDesc_" + skill.id);//skill.description;
		}
	}
	
	private void UpdatePartyAtkInfo(UnitParty curParty){
		int totalHp = curParty.TotalHp + pickedHelperInfo.UserUnit.Hp;
		totalHPLabel.text = totalHp.ToString();
		
		int totalAtk = curParty.GetTotalAtk() + pickedHelperInfo.UserUnit.Attack;
		totalAtkLabel.text = totalAtk.ToString();
		
		int value = 0;
		curParty.TypeAttack.TryGetValue (EUnitType.UFIRE, out value);
		fireAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UWATER, out value);
		waterAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UWIND, out value);
		windAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UNONE, out value);
		wuAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.ULIGHT, out value);
		lightAtkLabel.text = value.ToString();
		
		curParty.TypeAttack.TryGetValue (EUnitType.UDARK, out value);
		darkAtkLabel.text = value.ToString();
	}
	
	
	private void UpdatePageLight(int curPageIndex){
		Debug.Log("UpdatePageLight(), curPageIndex is : " + curPageIndex);
		SwitchLight(prevPageIndex, false);
		SwitchLight(curPageIndex, true);
		prevPageIndex = curPageIndex;
	}
	
	private void SwitchLight(int index, bool isLight){
		string lightPath = index + "/Sprite_ON";
		UISprite onLightSpr = pageLightRoot.transform.FindChild(lightPath).GetComponent<UISprite>();
		lightPath = index + "/Sprite_OFF";
		UISprite offLightSpr = pageLightRoot.transform.FindChild(lightPath).GetComponent<UISprite>();
		
		lightPath = index + "/Label_Index";
		UILabel indexLabel = pageLightRoot.transform.FindChild(lightPath).GetComponent<UILabel>();
		if(isLight){
			onLightSpr.enabled = true;
			indexLabel.color = new Color(60.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
		}
		else{
			onLightSpr.enabled = false;
			indexLabel.color = new Color(22.0f/255.0f, 140.0f/255.0f, 180.0f/255.0f);
		}
	}
	
	private void InitPageLight(){
		for (int i = 0; i < PARTY_LIGHT_COUNT; i++){
			SetPartyIndexText( i );
			SwitchLight(i, false);
		}
	}
	
	private void SetPartyIndexText(int partyIndex){
		string path = partyIndex + "/Label_Index";
		UILabel indexLabel = pageLightRoot.transform.FindChild(path).GetComponent<UILabel>();
		indexLabel.text = TextCenter.GetText("PARTY") + (partyIndex + 1);
	}
	
	private void ClickPageLight(GameObject lightObj){
		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		
		int newPartyPos = pageLightList.IndexOf(lightObj);
		Debug.Log("ClickPageLight(), newPartyPos is : " + newPartyPos);
		Debug.Log("ChangeParty before :: CurrentPartyId is : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
		UnitParty targetParty = DataCenter.Instance.PartyInfo.TargetParty(newPartyPos);
		Debug.Log("ChangeParty after :: CurrentPartyId is : " + DataCenter.Instance.PartyInfo.CurrentPartyId);
		RefreshParty(targetParty);
	}
	
	
}
