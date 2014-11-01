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

	private FriendInfo friendInfo;

	private FightReadyPage pageLeft;
	private FightReadyPage pageRight;
	private FightReadyPage moveParent;

	private QuestInfo questInfo;
	private StageInfo stageInfo;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		InitUI();

		pageLeft = FindChild<FightReadyPage> ("LeftParent");
		pageLeft.Init ();
		pageRight = FindChild<FightReadyPage> ("RightParent");
		pageRight.Init ();
		moveParent = FindChild<FightReadyPage> ("MoveParent");
		moveParent.Init ();
	}

	public override void ShowUI() {
		base.ShowUI();
//		RecordPickedInfoForFight ();
		RefreshParty ();
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.FIGHT_READY);

		if (viewData != null){
			if(viewData.ContainsKey ("helper_info")) {
				moveParent.HelperInfo = pageRight.HelperInfo = pageLeft.HelperInfo = viewData["helper_info"] as FriendInfo;
			}
			if(viewData.ContainsKey("QuestInfo")){
				questInfo = viewData["QuestInfo"] as QuestInfo;
			}
			if(viewData.ContainsKey("StageInfo")){
				stageInfo = viewData["StageInfo"] as StageInfo;
			}
		}
	}

	public override void DestoryUI () {
		base.DestoryUI ();

	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);
			gameObject.transform.localPosition = new Vector3(config.localPosition.x,  config.localPosition.y, 0);
			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f));       
		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
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


//	private void RecordPickedInfoForFight(){
//
//		pickedHelperInfo = viewData["HelperInfo"] as FriendInfo;
//		RefreshParty();
//	}

	private void ClickFightBtn(GameObject btn){
		Debug.Log("StandbyView.ClickFightBtn(), start...");
		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

		StartQuestParam sqp = new StartQuestParam ();
		sqp.currPartyId = DataCenter.Instance.UnitData.PartyInfo.CurrentPartyId;
//		if (viewData.ContainsKey ("HelperInfo")) {
//			sqp.helperUserUnit = viewData[ "HelperInfo" ] as FriendInfo;	
//		}
		sqp.questId = questInfo.id;
		sqp.stageId = stageInfo.id;
		sqp.copyType = stageInfo.CopyType;
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

			if(friendInfo != null)
				friendInfo.usedTime = GameTimer.GetInstance().GetCurrentSeonds();

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

	private void EnterBattle (QuestDungeonData tqdd) {
		BattleConfigData.Instance.BattleFriend = null;
		if (friendInfo != null) {
			friendInfo.friendPoint = 0;
			friendInfo.usedTime = GameTimer.GetInstance ().GetCurrentSeonds ();
			BattleConfigData.Instance.BattleFriend = friendInfo;
		}

		BattleConfigData.Instance.gotFriendPoint = 0;
		 //pickedInfoForFight[ "HelperInfo" ] as TFriendInfo;
		BattleConfigData.Instance.ResetFromServer(tqdd);
		ModuleManager.Instance.EnterBattle ();

		BattleConfigData.Instance.StoreData (tqdd.questId);

		Umeng.GA.StartLevel ("Quest" + tqdd.questId);
	}

}
