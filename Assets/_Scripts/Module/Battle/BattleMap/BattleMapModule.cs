using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleMapModule : ModuleBase {
	private GameObject rootObject;

//	private RoleStateException roleStateException;

//	public static int battleData = 0;

	public BattleMapModule (UIConfigItem config) : base(  config) {
		CreateUI<BattleMapView> ();

//		battleData = ConfigBattleUseData.Instance.hasBattleData ();
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

//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
//		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);

//		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);

//		if (battleData > 0) {
//			(view as BattleMapView).ContineBattle ();
//		} else {
//			ConfigBattleUseData.Instance.StoreData(ConfigBattleUseData.Instance.questDungeonData.QuestId);
//		}
	}

	public override void HideUI () {
		BattleAttackManager.Instance.ResetSkill();
		BattleAttackManager.Instance.RemoveListen ();
		base.HideUI ();

//		MsgCenter.Instance.RemoveListener (CommandEnum.BattleBaseData, BattleBase);
//		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.GridEnd, GridEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.PlayerDead, BattleFail);
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
//		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);


//		roleStateException.RemoveListener ();
	}

//	void BattleEnd(object data){
//		(view as BattleMapView).ArriveAtCell (data);
//	}

//	void AttackEnemy (object data) {
//		AttackInfo ai = data as AttackInfo;
//		if (ai == null) {
//			return;
//		}
////		attackEffect.RefreshItem ();
//		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule, "refreshitem", ai.UserUnitID, ai.SkillID, ai.AttackValue, false);
//	}

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

//		Main.Instance.GInput.IsCheckInput = false;
		BattleBottomView.notClick = true;

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
			BattleBottomView.notClick = false;
		}, BattleConfigData.Instance.questDungeonData.QuestId);
		BattleAttackManager.Instance.ClearData ();
	}


	void BattleFailExit(object data) {
		RetireQuest.SendRequest (o=>{
			AudioManager.Instance.PlayAudio (AudioEnum.sound_game_over);
			
			ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule, "over", (Callback)(()=>{
//				BattleManipulationView.colorIndex = 0;
//				BattleManipulationView.isShow = false;
//				BattleEnd ();

				ModuleManager.Instance.ExitBattle ();
			}));
			BattleConfigData.Instance.ClearData ();
		}, BattleConfigData.Instance.questDungeonData.QuestId, true);
	}
}
