using UnityEngine;
using System.Collections.Generic;

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
//		Debug.LogError (data);
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
		MsgCenter.Instance.Invoke (CommandEnum.LevelUp, data);
	}
}

