using UnityEngine;
using System.Collections.Generic;

public class LevelUpModule : ModuleBase {
	public LevelUpModule(UIConfigItem config) : base( config) {
		CreateUI<LevelUpView> ();
	}

	public override void HideUI () {
		base.HideUI ();

		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
			base.DestoryUI();
		}
	}
	
	List<TUserUnit> levelUpInfo = null;

	public override void OnReceiveMessages (object data) {
		levelUpInfo = data as List<TUserUnit>;
		if (levelUpInfo == null || levelUpInfo.Count <= 0) {
			return;
		}
		
		LevelUp netBase = new LevelUp ();
		TUserUnit baseUserUnit = levelUpInfo[0];	
		TUserUnit friendUserUnit = levelUpInfo[1];

		for (int i = levelUpInfo.Count - 1; i > 1; i--) {
			netBase.PartUniqueId.Add(levelUpInfo[i].ID);
		}

		netBase.BaseUniqueId = baseUserUnit.ID;
		netBase.HelperUserId = friendUserUnit.ID;
		netBase.HelperUserUnit = friendUserUnit;
		DataCenter.Instance.levelUpFriend = friendUserUnit;

		netBase.OnRequest (null, NetCallback);
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

			dataCenter.supportFriendManager.useFriend.UseTime = GameTimer.GetInstance().GetCurrentSeonds();

			dataCenter.AccountInfo.Money = (int)rspLevelUp.money;
			uint userId = DataCenter.Instance.UserInfo.UserId;
			dataCenter.oldUserUnitInfo = DataCenter.Instance.UserUnitList.GetMyUnit (rspLevelUp.blendUniqueId);
			dataCenter.levelUpMaterials.Clear();

			for (int i = 0; i < rspLevelUp.partUniqueId.Count; i++) {
				uint uniqueID = rspLevelUp.partUniqueId[i];
				TUserUnit tuu = dataCenter.UserUnitList.Get(uniqueID);
				dataCenter.levelUpMaterials.Add(tuu);
//				Debug.LogError("NetCallback delete unit : " + uniqueID);
				dataCenter.UserUnitList.DelMyUnit(uniqueID);
			}
	
			TUserUnit baseUserUnit = DataCenter.Instance.UserUnitList.AddMyUnit (rspLevelUp.baseUnit);

			ModuleManger.Instance.ShowModule (ModuleEnum.UnitDetailModule);

			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);

			MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}
	}
}
