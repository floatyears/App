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

	private UIButton startFightBtn;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		InitUI();
	}

	public override void ShowUI() {
		base.ShowUI();
		ShowUIAnimation();
		RecordPickedInfoForFight ();
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.FIGHT_READY);
	}

	public override void DestoryUI () {
		base.DestoryUI ();

	}

	private DragSliderBase dragSlider;

	#region IDragChangeView implementation

	public void RefreshParty (bool isRight) {	
		UnitParty tup = null;
		if (isRight) {
			tup = DataCenter.Instance.UnitData.PartyInfo.PrevParty;
		} else {
			tup = DataCenter.Instance.UnitData.PartyInfo.NextParty;
		}
//		int curPartyIndex = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId + 1;
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

		UIEventListenerCustom.Get(startFightBtn.gameObject).onClick = ClickFightBtn;
		UIEventListenerCustom.Get(prePageBtn.gameObject).onClick = PrevPage;
		UIEventListenerCustom.Get(nextPageBtn.gameObject).onClick = NextPage;

		dragSlider = GetComponent<DragSliderBase>();
		dragSlider.SetDataInterface (this);
	}
	
	private void PrevPage(GameObject go){
		UnitParty preParty = DataCenter.Instance.UnitData.PartyInfo.PrevParty;
		RefreshParty();  
	}
	
	private void NextPage(GameObject go){
		UnitParty nextParty = DataCenter.Instance.UnitData.PartyInfo.NextParty;
		RefreshParty();
	}

	private void RefreshParty(){
		dragSlider.RefreshData ();
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));       
	}

	static public FriendInfo pickedHelperInfo;

	private void RecordPickedInfoForFight(){

		pickedHelperInfo = viewData["HelperInfo"] as FriendInfo;
		RefreshParty();
	}

	void EvolveSelectQuest(object data) {
		Debug.LogError ("EvolveSelectQuest");
		evolveStart = data as UnitDataModel;
		prePageBtn.isEnabled = false;
		nextPageBtn.isEnabled = false;
//		pickedHelperInfo = evolveStart.EvolveStart.friendInfo;
		UnitParty up = new UnitParty (0);
		up.id = 5;
		for (int i = 0; i < evolveStart.evolveParty.Count; i++) {
			PartyItem pi = new PartyItem();
			pi.unitPos = i;
			pi.unitUniqueId = evolveStart.evolveParty[i] == null ? 0 : evolveStart.evolveParty[i].uniqueId;
			up.items.Add(pi);
		}

		dragSlider.StopOperate = true;
		dragSlider.RefreshData (up);
	}
	


	private void ClickFightBtn(GameObject btn){
		Debug.Log("StandbyView.ClickFightBtn(), start...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		StartQuestParam sqp = new StartQuestParam ();
		sqp.currPartyId = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId;
		sqp.helperUserUnit = viewData[ "HelperInfo" ] as FriendInfo;
		QuestItemView questInfo = viewData[ "QuestInfo"] as QuestItemView;
		sqp.questId = questInfo.Data.id;
		sqp.stageId = questInfo.StageID;
		sqp.startNew = 1;
		QuestController.Instance.StartQuest (sqp, RspStartQuest);
	}

	private UnitDataModel evolveStart;
	
	private void RspStartQuest(object data) {
		QuestDungeonData tqdd = null;
		bbproto.RspStartQuest rspStartQuest = data as bbproto.RspStartQuest;
		if (rspStartQuest.header.code != (int)ErrorCode.SUCCESS) {
			Debug.LogError("Rsp code: "+rspStartQuest.header.code+", error:"+rspStartQuest.header.error);
			ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspStartQuest.header.code);
			return;
		}
		if (rspStartQuest.header.code == 0 && rspStartQuest.dungeonData != null) {
			FriendInfo tfi = viewData[ "HelperInfo" ] as FriendInfo;
			tfi.usedTime = GameTimer.GetInstance().GetCurrentSeonds();

			LogHelper.Log("rspStartQuest code:{0}, error:{1}", rspStartQuest.header.code, rspStartQuest.header.error);
			DataCenter.Instance.UserData.UserInfo.staminaNow = rspStartQuest.staminaNow;
			DataCenter.Instance.UserData.UserInfo.staminaRecover = rspStartQuest.staminaRecover;
			tqdd = rspStartQuest.dungeonData;
			tqdd.assignData();
//			ModelManager.Instance.(ModelEnum.MapConfig, tqdd);
//			DataCenter.Instance.SetData(ModelEnum.MapConfig,tqdd);
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

		DataCenter.Instance.UserData.UserInfo.staminaNow = rsp.staminaNow;
		DataCenter.Instance.UserData.UserInfo.staminaRecover = rsp.staminaRecover;
		bbproto.QuestDungeonData questDungeonData = rsp.dungeonData;
		questDungeonData.assignData ();
		EnterBattle (questDungeonData);
	}
	
	private void EnterBattle (QuestDungeonData tqdd) {
		pickedHelperInfo.friendPoint = 0;
		pickedHelperInfo.usedTime = GameTimer.GetInstance ().GetCurrentSeonds ();

		BattleConfigData.Instance.gotFriendPoint = 0;
		BattleConfigData.Instance.BattleFriend = pickedHelperInfo; //pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		BattleConfigData.Instance.ResetFromServer(tqdd);
		ModuleManager.Instance.EnterBattle ();

		BattleConfigData.Instance.StoreData (tqdd.questId);

		Umeng.GA.StartLevel ("Quest" + tqdd.questId);
	}

}
