using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class LevelUpReadyPoolUI : ConcreteComponent {
	public LevelUpReadyPoolUI(string uiName):base(uiName) {
    }

	public override void CallbackView (object data) {
		List<TUserUnit> temp = data as List<TUserUnit>;
		if (temp == null) {
			Debug.LogError("level up network data is error");	
			return;
		}

		LevelUp netBase = new LevelUp ();
		TUserUnit baseUserUnit = temp [0];
		temp.RemoveAt (0);
		TUserUnit friendUserUnit = temp [0];
		temp.RemoveAt (0);
		foreach (var item in temp) {
			netBase.PartUniqueId.Add(item.ID);
		}
		netBase.BaseUniqueId = baseUserUnit.ID;
		netBase.HelperUserId = friendUserUnit.ID;
		netBase.HelperUserUnit = friendUserUnit;
		netBase.OnRequest (null, NetCallback);
	}


	void NetCallback(object data) {
		//TODO: moving to logic
		if( data != null ) {
			bbproto.RspLevelUp rspLevelUp = data as bbproto.RspLevelUp;
			if (rspLevelUp.header.code != (int)ErrorCode.SUCCESS) {
				Debug.LogError("Rsp code: "+rspLevelUp.header.code+", error:"+rspLevelUp.header.error);
				ErrorMsgCenter.Instance.OpenNetWorkErrorMsgWindow(rspLevelUp.header.code);
				return;
			}

//			update money
			DataCenter.Instance.AccountInfo.Money = (int)rspLevelUp.money;
			
			// update unitlist

			uint userId = DataCenter.Instance.UserInfo.UserId;

			DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.MyUnitList.GetMyUnit(rspLevelUp.blendUniqueId);

			DataCenter.Instance.UserUnitList.DelMyUnit(rspLevelUp.blendUniqueId);
//			TUserUnit tuu = TUserUnit.GetUserUnit(rspLevelUp.blendUniqueId, rspLevelUp.baseUnit);
			DataCenter.Instance.UserUnitList.AddMyUnit(rspLevelUp.baseUnit);
			foreach(uint partUniqueId in rspLevelUp.partUniqueId) {
				DataCenter.Instance.UserUnitList.DelMyUnit(partUniqueId);
			}

//				LogHelper.Log("rspLevelUp add to myUserUnit.count: {0}", rspLevelUp.unitList.Count);
		

			UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
//			Debug.LogError("rspLevelUp.blendUniqueId : " + rspLevelUp.blendUniqueId);
            MsgCenter.Instance.Invoke (CommandEnum.LevelUpSucceed, rspLevelUp.blendUniqueId);
		}


	}
}

