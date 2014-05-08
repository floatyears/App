using UnityEngine;
using System.Collections.Generic;

public class levelUpOperateUI : ConcreteComponent {
	public levelUpOperateUI(string uiName) : base(uiName) {

	}

	public override void HideUI () {
		base.HideUI ();
		if (UIManager.Instance.baseScene.CurrentScene != SceneEnum.UnitDetail) {
			base.DestoryUI();
		}
	}
	

	public override void CallbackView (object data) {
		Queue<TUserUnit> levelUpInfo = data as Queue<TUserUnit>;
		if (levelUpInfo == null) {
//			Debug.LogError("level up network data is error");	
			return;
		}
		
		LevelUp netBase = new LevelUp ();
		TUserUnit baseUserUnit = levelUpInfo.Dequeue();
		TUserUnit friendUserUnit = levelUpInfo.Dequeue ();
		while (levelUpInfo.Count > 0) {
			netBase.PartUniqueId.Add(levelUpInfo.Dequeue().ID);
//			Debug.LogError("netBase.PartUniqueId : " + netBase.PartUniqueId[netBase.PartUniqueId.Count -1 ]);
		}
//		Debug.LogError (netBase.PartUniqueId.Count);
		netBase.BaseUniqueId = baseUserUnit.ID;
//		Debug.LogError("netBase.BaseUniqueId : " + netBase.BaseUniqueId);
		netBase.HelperUserId = friendUserUnit.ID;
//		Debug.LogError("netBase.HelperUserId " + netBase.HelperUserId);
		netBase.HelperUserUnit = friendUserUnit;
		netBase.OnRequest (null, NetCallback);
	}
	
	
	void NetCallback(object data) {
		//TODO: moving to logic
		if (data != null) {
				bbproto.RspLevelUp rspLevelUp = data as bbproto.RspLevelUp;
				if (rspLevelUp.header.code != (int)ErrorCode.SUCCESS) {
						Debug.LogError ("Rsp code: " + rspLevelUp.header.code + ", error:" + rspLevelUp.header.error);
						ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow (rspLevelUp.header.code);
						return;
				}
	
				//			update money
				DataCenter.Instance.AccountInfo.Money = (int)rspLevelUp.money;
	
				// update unitlist
	
				uint userId = DataCenter.Instance.UserInfo.UserId;
	
				DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.UserUnitList.GetMyUnit (rspLevelUp.blendUniqueId);
	
				DataCenter.Instance.UserUnitList.DelMyUnit (rspLevelUp.blendUniqueId);
			Debug.LogError("rspLevelUp.baseUnit : " + rspLevelUp.baseUnit.uniqueId + " rspLevelUp.blendUniqueId : " + rspLevelUp.blendUniqueId);
				DataCenter.Instance.UserUnitList.AddMyUnit (rspLevelUp.baseUnit);
			Debug.LogError("rspLevelUp.baseUnit : " +DataCenter.Instance.UserUnitList.GetMyUnit(rspLevelUp.blendUniqueId).Level);
				foreach (uint partUniqueId in rspLevelUp.partUniqueId) {
						DataCenter.Instance.UserUnitList.DelMyUnit (partUniqueId);
				}
	
				UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
				MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
							Debug.LogError("rspLevelUp.blendUniqueId : " + rspLevelUp.blendUniqueId);
				MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}
	}
}
