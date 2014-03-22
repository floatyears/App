using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class LevelUpReadyPoolUI : ConcreteComponent {
	public LevelUpReadyPoolUI(string uiName):base(uiName) {}

	public override void Callback (object data) {
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
//		Debug.LogError (temp.Count);
		foreach (var item in temp) {
//			Debug.LogError("temp item : " + item.ID);
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
//			Debug.LogError("rspLevelUp.header.error : " + rspLevelUp.header.error + " rspLevelUp.header.code : " + rspLevelUp.header.code);

//			update money
			DataCenter.Instance.AccountInfo.Money = (int)rspLevelUp.money;
			
			// update unitlist

			uint userId = DataCenter.Instance.UserInfo.UserId;

			DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.MyUnitList.GetMyUnit(rspLevelUp.blendUniqueId);
			LogHelper.LogError("LevelUp NetCallback :: blendId:{0} DataCenter.Instance.oldUserUnitInfo:{1}", rspLevelUp.blendUniqueId,  DataCenter.Instance.oldUserUnitInfo);

			if (rspLevelUp.unitList != null) {
//				update myUnitList
//				DataCenter.Instance.oldUserUnitInfo = DataCenter.Instance.MyUnitList.GetMyUnit(rspLevelUp.blendUniqueId);
//				LogHelper.LogError("LevelUp NetCallback :: blendId:{0} DataCenter.Instance.oldUserUnitInfo:{1}", rspLevelUp.blendUniqueId,  DataCenter.Instance.oldUserUnitInfo);
				DataCenter.Instance.MyUnitList.Clear();
				foreach(UserUnit unit in rspLevelUp.unitList) {
					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));

				}

				foreach(uint partUniqueId in rspLevelUp.partUniqueId) {
					DataCenter.Instance.UserUnitList.DelMyUnit(partUniqueId);
				}

				LogHelper.Log("rspLevelUp add to myUserUnit.count: {0}", rspLevelUp.unitList.Count);
			}

			UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
			MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
		}


	}
}

