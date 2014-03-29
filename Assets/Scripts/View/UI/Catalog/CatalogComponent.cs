using UnityEngine;
using System.Collections;

public class CatalogComponent : ConcreteComponent {

	public CatalogComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();

		//Temp for test
		uint friendID = 101;
		TFriendInfo friendInfo = DataCenter.Instance.GetSupporterInfo(friendID);
		UIManager.Instance.ChangeScene(SceneEnum.Result);
		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, friendInfo);
		//Test end

	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

}
