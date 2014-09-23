using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class LevelUpModule : ModuleBase {
	public LevelUpModule(UIConfigItem config) : base( config) {
		CreateUI<LevelUpView> ();
	}

	public override void HideUI () {
		base.HideUI ();

//		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
//			base.DestoryUI();
//		}
	}
	
	List<UserUnit> levelUpInfo = null;

	public override void OnReceiveMessages(params object[] data) {
		string action = data[0] as string;
		if ( action == "RefreshUnitItem" ) {
			UserUnit tuu = data[1] as UserUnit;

			(view as LevelUpView).RefreshUnitItem(tuu);
			return;

		} else if ( action == "DoLevelUp" ) {
			levelUpInfo = data[1] as List<UserUnit>;
			if (levelUpInfo == null || levelUpInfo.Count <= 0) {
				return;
			}
			
//			LevelUp netBase = new LevelUp ();
			UserUnit baseUserUnit = levelUpInfo[0];	
			UserUnit friendUserUnit = levelUpInfo[1];
			List<uint> PartUniqueId = new List<uint>();
			for (int i = levelUpInfo.Count - 1; i > 1; i--) {
				PartUniqueId.Add(levelUpInfo[i].uniqueId);
			}
			DataCenter.Instance.levelUpFriend = friendUserUnit;

			UnitController.Instance.LevelUp (NetCallback,baseUserUnit.uniqueId,PartUniqueId,friendUserUnit.uniqueId,friendUserUnit);
		}
	}
	
//	public bool CheckState() {
//		if (view.gameObject.activeSelf) {
//			return true;
//		} else {
//			return false;
//		}
//	}

	void NetCallback(object data) {
		//TODO: moving to logic
		if (data != null) {
			bbproto.RspLevelUp rspLevelUp = data as bbproto.RspLevelUp;
			if (rspLevelUp.header.code != (int)ErrorCode.SUCCESS) {
					ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow (rspLevelUp.header.code);
					DataCenter.Instance.levelUpFriend = null;
					return;
			}

			levelUpInfo.Clear();
			DataCenter dataCenter = DataCenter.Instance;

			dataCenter.supportFriendManager.useFriend.usedTime = GameTimer.GetInstance().GetCurrentSeonds();

			dataCenter.AccountInfo.money = (int)rspLevelUp.money;
			uint userId = DataCenter.Instance.UserInfo.userId;
			dataCenter.oldUserUnitInfo = DataCenter.Instance.UserUnitList.GetMyUnit (rspLevelUp.blendUniqueId);
			dataCenter.levelUpMaterials.Clear();

			//删除消耗的材料
			for (int i = 0; i < rspLevelUp.partUniqueId.Count; i++) {
				uint uniqueID = rspLevelUp.partUniqueId[i];
				UserUnit tuu = dataCenter.UserUnitList.Get(uniqueID);
				dataCenter.levelUpMaterials.Add(tuu);
//				Debug.LogError("NetCallback delete unit : " + uniqueID);
				dataCenter.UserUnitList.DelMyUnit(uniqueID);
			}
	
			//更新强化后的base卡牌数据
			DataCenter.Instance.UserUnitList.UpdateMyUnit(rspLevelUp.baseUnit);

			ModuleManager.Instance.ShowModule (ModuleEnum.UnitDetailModule,"levelup",rspLevelUp);

//			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);

//			MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
			(view as LevelUpView).ResetUIAfterLevelUp(rspLevelUp.blendUniqueId);
		}
	}
}
