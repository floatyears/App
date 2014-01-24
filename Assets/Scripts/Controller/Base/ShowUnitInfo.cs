using UnityEngine;
using System.Collections;

public class ShowUnitInfo {
	public ShowUnitInfo() {
		MsgCenter.Instance.AddListener (CommandEnum.EnterUnitInfo, EnterUnitInfo);
	}

	~ShowUnitInfo () {
		MsgCenter.Instance.RemoveListener (CommandEnum.EnterUnitInfo, EnterUnitInfo);
	}
	
	void EnterUnitInfo (object data) {
		string spriteName = (string)data;
		Messager.toViewUnitName = spriteName;
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
	}
}
