using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;

	public BattleMapModule (UIConfigItem config) : base(  config) {
		CreateUI<BattleMapView> ();

		string tempName = "Map";
	}

	public override void OnReceiveMessages (params object[] data)
	{
		switch (data[0].ToString()) {
//			case "rolecoor":
//				RoleCoordinate((Coordinate)data[1]);
//				break;
			case "playerdead":
				BattleFail();
				break;
			default:
					break;
		}
	}

	public override void ShowUI () {
		base.ShowUI ();
		BattleAttackManager.Instance.GetBaseData ();
	}

	public override void HideUI () {
		BattleAttackManager.Instance.ResetSkill();
//		BattleAttackManager.Instance.RemoveListen ();
		base.HideUI ();
	}

	public void HaveFriendExit() {
		ModuleManager.Instance.ExitBattle ();
		ModuleManager.Instance.ShowModule(ModuleEnum.ResultModule);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, BattleConfigData.Instance.BattleFriend);
	}

	private EQuestGridType gridType = EQuestGridType.Q_NONE;
	private Coordinate _currentCoor;
	public Coordinate currentCoor {
		set { _currentCoor = value; BattleConfigData.Instance.storeBattleData.roleCoordinate = _currentCoor; }
		get { return _currentCoor;}
	}

	void BattleFail() {
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);

		TipsManager.Instance.ShowMsgWindow (TextCenter.GetText ("ResumeQuestTitle"), TextCenter.GetText ("ResumeQuestContent", DataCenter.resumeQuestStone), TextCenter.GetText ("OK"), TextCenter.GetText ("Cancel"), BattleFailRecover, BattleFailExit);
	}

	void BattleFailRecover(object data) {
		if (DataCenter.Instance.AccountInfo.Stone < DataCenter.redoQuestStone) {
			TipsManager.Instance.ShowTipsLabel(TextCenter.GetText("NotEnoughStone"));
			return;
		}

		ResumeQuest.SendRequest (o=>{
			Umeng.GA.Buy ("ResumeQuest" , 1, DataCenter.resumeQuestStone);
			BattleAttackManager.Instance.AddBlood (BattleAttackManager.Instance.maxBlood);
			BattleAttackManager.Instance.RecoverEnergePoint (DataCenter.maxEnergyPoint);
			BattleConfigData.Instance.StoreMapData ();
			
//			Main.Instance.GInput.IsCheckInput = true;
//			BattleBottomView.notClick = false;
		}, BattleConfigData.Instance.questDungeonData.QuestId);
		BattleAttackManager.Instance.ClearData ();
	}


	void BattleFailExit(object data) {
		RetireQuest.SendRequest (o=>{
			AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);
			
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "over", (Callback)(()=>{

				ModuleManager.Instance.ExitBattle ();
			}));
			BattleConfigData.Instance.ClearData ();
		}, BattleConfigData.Instance.questDungeonData.QuestId, true);
	}
}
