using UnityEngine;
using System.Collections;

public class UnitDisplayUnity : UIComponentUnity {
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

	}

	//==========================================interface end ==========================

	private Object unitItem;
	private DragPanel unitItemDragPanel;

	void InitUI () {
		unitItem = Resources.Load("Prefabs/UI/Friend/UnitItem");
	}


}
