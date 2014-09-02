using UnityEngine;
using System.Collections;

public class CatalogController : ModuleBase {
	public CatalogController(UIConfigItem config):base(  config) {}
	public override void ShowUI () {
		base.ShowUI ();
		//Temp for test
//		uint friendID = 101;
//		TFriendInfo friendInfo = DataCenter.Instance.GetSupporterInfo(friendID);
//		ModuleManger.Instance.ShowModule(ModuleEnum.Result);
//		MsgCenter.Instance.Invoke(CommandEnum.ShowFriendPointUpdateResult, friendInfo);
		//Test end
	}

	public override void HideUI () {
		base.HideUI ();

		base.DestoryUI ();
	}
}
