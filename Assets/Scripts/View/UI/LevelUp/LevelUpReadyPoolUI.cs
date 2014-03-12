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

		temp.Remove (baseUserUnit);
		TUserUnit friendUserUnit = temp [1];

		temp.Remove (friendUserUnit);
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
			Debug.LogError("rspLevelUp.header.error : " + rspLevelUp.header.error + " rspLevelUp.header.code : " + rspLevelUp.header.code);

			//update money
			DataCenter.Instance.AccountInfo.Money = (int)rspLevelUp.money;
			
			// update unitlist
			uint userId = DataCenter.Instance.UserInfo.UserId;
			if (rspLevelUp.unitList != null) {
				//update myUnitList
				DataCenter.Instance.MyUnitList.Clear();
				foreach(UserUnit unit in rspLevelUp.unitList) {
					DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
				}

				// update blendUnit in the userUnitList
//				TUserUnit blendUnit = DataCenter.Instance.UserUnitList.GetMyUnit( rspLevelUp.blendUniqueId );
//				blendUnit = DataCenter.Instance.MyUnitList.GetMyUnit( rspLevelUp.blendUniqueId );

				//remove partUniqueId from userUnitList
				foreach(uint partUniqueId in rspLevelUp.partUniqueId) {
					DataCenter.Instance.UserUnitList.DelMyUnit(partUniqueId);
				}

				LogHelper.Log("rspLevelUp add to myUserUnit.count: {0}", rspLevelUp.unitList.Count);
			}

		}

		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
	}
}

