using UnityEngine;
using System.Collections;

public class CatalogComponent : ConcreteComponent {

	public CatalogComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		uint friendID = 101;

		UIManager.Instance.ChangeScene(SceneEnum.Result);
//		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, );
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

}
