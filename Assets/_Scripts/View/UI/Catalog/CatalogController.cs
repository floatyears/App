using UnityEngine;
using System.Collections;

public class CatalogController : ConcreteComponent {
	public CatalogController(string uiName):base(uiName) {}
	public override void ShowUI () {
		base.ShowUI ();
		//Temp for test
//		uint friendID = 101;
//		TFriendInfo friendInfo = DataCenter.Instance.GetSupporterInfo(friendID);
//		UIManager.Instance.ChangeScene(SceneEnum.Result);
//		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, friendInfo);
		//Test end
	}
}
