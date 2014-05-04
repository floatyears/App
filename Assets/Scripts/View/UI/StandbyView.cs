using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandbyView : UIComponentUnity {
	private UIButton prePageBtn;
	private UIButton nextPageBtn;
	private UIImageButton startFightBtn;
	private UILabel totalHPLabel;
	private UILabel totalAtkLabel;


	private Dictionary<int, PageUnitItem> partyView = new Dictionary<int, PageUnitItem>();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
		TUnitParty curParty = DataCenter.Instance.PartyInfo.CurrentParty;
		RefreshParty(curParty);
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
	}

	public override void DestoryUI () {
		base.DestoryUI ();
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickHelper, RecordPickedInfoForFight);
	}

	private void InitUI(){
		prePageBtn = FindChild<UIButton>("Button_Left");
		nextPageBtn = FindChild<UIButton>("Button_Right");
		startFightBtn = transform.FindChild("ImgBtn_Fight").GetComponent<UIImageButton>();
		totalHPLabel = transform.FindChild("Label_Total_HP").GetComponent<UILabel>();
		totalAtkLabel = transform.FindChild("Label_Total_ATK").GetComponent<UILabel>();
	

		UIEventListener.Get(startFightBtn.gameObject).onClick = ClickFightBtn;
		UIEventListener.Get(prePageBtn.gameObject).onClick = PrevPage;
		UIEventListener.Get(nextPageBtn.gameObject).onClick = NextPage;

		for (int i = 0; i < 4; i++){
			PageUnitItem puv = FindChild<PageUnitItem>(i.ToString());
			partyView.Add(i, puv);
		}
	}
	
	private void PrevPage(GameObject go){
		Debug.Log("PrevPage");
		TUnitParty preParty = DataCenter.Instance.PartyInfo.PrevParty;
		RefreshParty(preParty);  
	}
	
	private void NextPage(GameObject go){
		Debug.Log("NextPage");
		TUnitParty nextParty = DataCenter.Instance.PartyInfo.NextParty;
		RefreshParty(nextParty);
	}

	private void RefreshParty(TUnitParty party){
		List<TUserUnit> partyMemberList = party.GetUserUnit();
		for (int i = 0; i < partyMemberList.Count; i++){
			partyView[ i ].Init(partyMemberList [ i ]);
		}

		ShowPartyInfo();
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}

	private Dictionary<string, object> pickedInfoForFight;
	private TFriendInfo pickedHelperInfo;
	private void RecordPickedInfoForFight(object msg){
		Debug.Log("StartbyView.RecordPickedInfoForFight(), received info...");
		pickedInfoForFight = msg as Dictionary<string, object>;

		//Show helper view as soon as fill helperViewItem with helper data(data bind with view)
		pickedHelperInfo = pickedInfoForFight[ "HelperInfo"] as TFriendInfo;
		HelperUnitItem helperUnitItem = transform.FindChild("Helper").GetComponent<HelperUnitItem>();
		helperUnitItem.Init(pickedHelperInfo);

		ShowPartyInfo();
	}

	private void ClickFightBtn(GameObject btn){
		Debug.Log("StandbyView.ClickFightBtn(), start...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		StartFight();
	}

	private TEvolveStart evolveStart;
	private void StartFight(){
		if (DataCenter.gameStage == GameState.Evolve) {
			evolveStart.EvolveStart.restartNew = 1;
			evolveStart.EvolveStart.OnRequest(null, RspEvolveStartQuest);
		} 
		else {
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

	private void RspStartQuest(object data) {
		TQuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserInfo.StaminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserInfo.StaminaRecover = rspStartQuest.staminaRecover;
			tqdd = new TQuestDungeonData(rspStartQuest.dungeonData);
//			tqdd.assignData();
			ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
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

		DataCenter.Instance.UserInfo.StaminaNow = rsp.staminaNow;
		DataCenter.Instance.UserInfo.StaminaRecover = rsp.staminaRecover;
		bbproto.QuestDungeonData questDungeonData = rsp.dungeonData;
		TQuestDungeonData tqdd = new TQuestDungeonData (questDungeonData);
		ModelManager.Instance.SetData(ModelEnum.MapConfig, tqdd);
		
		EnterBattle (tqdd);
	}
	
	private void EnterBattle (TQuestDungeonData tqdd) {
		ConfigBattleUseData.Instance.BattleFriend = pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		ConfigBattleUseData.Instance.ResetFromServer(tqdd);
		UIManager.Instance.EnterBattle();
	} 

	private void ShowPartyInfo(){
		if(pickedHelperInfo == null) return;
		int totalHp = DataCenter.Instance.PartyInfo.CurrentParty.TotalHp + pickedHelperInfo.UserUnit.Hp;
		totalHPLabel.text = totalHp.ToString();
		
		int totalAtk = DataCenter.Instance.PartyInfo.CurrentParty.GetTotalAtk() + pickedHelperInfo.UserUnit.Attack;
		totalAtkLabel.text = totalAtk.ToString();
	}

}
