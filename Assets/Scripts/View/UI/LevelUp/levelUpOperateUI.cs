using UnityEngine;
using System.Collections.Generic;

public class levelUpOperateUI : ConcreteComponent {
	public levelUpOperateUI(string uiName) : base(uiName) {

	}

	public override void HideUI () {
		base.HideUI ();

		if (UIManager.Instance.nextScene != SceneEnum.UnitDetail) {
//			Debug.LogError ("levelup DestoryUI ui : " + UIManager.Instance.nextScene);
			base.DestoryUI();
//			UIManager.Instance.RemoveUI ();
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
		}
		netBase.BaseUniqueId = baseUserUnit.ID;
		netBase.HelperUserId = friendUserUnit.ID;
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
//			Debug.LogError("rspLevelUp.baseUnit : " + rspLevelUp.baseUnit.uniqueId + " rspLevelUp.blendUniqueId : " + rspLevelUp.blendUniqueId);
				TUserUnit tuu = DataCenter.Instance.UserUnitList.AddMyUnit (rspLevelUp.baseUnit);
//			Debug.LogError("rspLevelUp.baseUnit : " +DataCenter.Instance.UserUnitList.GetMyUnit(rspLevelUp.blendUniqueId).Level);
				foreach (uint partUniqueId in rspLevelUp.partUniqueId) {
						DataCenter.Instance.UserUnitList.DelMyUnit (partUniqueId);
				}
	
				UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);

				MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
				
			MsgCenter.Instance.Invoke(CommandEnum.ShowUnitDetail, tuu);
//							Debug.LogError("rspLevelUp.blendUniqueId : " + rspLevelUp.blendUniqueId);
				MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}
	}
}
