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
			bbproto.RspLevelUp rspLevelUp = new bbproto.RspLevelUp();
			//update money
			GlobalData.accountInfo.Money = (int)rspLevelUp.money;
			
			// update unitlist
			uint userId = GlobalData.userInfo.UserId;
			if (rspLevelUp.unitList != null) {
				GlobalData.myUnitList.Clear();	//TODO: maybe havenot to clear all, just for simple right now.
				//GlobalData.userUnitList.Clear(); //TODO: only update the baseUnit
				foreach(UserUnit unit in rspLevelUp.unitList) {
					GlobalData.myUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
					//GlobalData.userUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
				}
				LogHelper.Log("rspLevelUp add to myUserUnit.count: {0}", rspLevelUp.unitList.Count);
			}

		}

		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
	}
}

