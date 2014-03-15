using UnityEngine;
using System.Collections;
using bbproto;

public class EvolveFriendUnity : LevelUpFriendWindow {
	public override void Init (UIInsConfig config, IUICallback origin) {
		base.Init (config, origin);
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void Callback (object data) {
		base.Callback (data);
	}

	public void ShieldSelect (TUserUnit baseItem) {
		HelperRequire hr = baseItem.UnitInfo.evolveInfo.helperRequire;

	}
}
