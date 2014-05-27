using UnityEngine;
using System.Collections.Generic;

public class UnitDisplay : ConcreteComponent {
	public UnitDisplay (string name) : base (name) {
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ReadData ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void CreatUI () {
		base.CreatUI ();
		InitDragpanel ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void CallbackView (object data) {
		base.CallbackView (data);
		Dictionary<string, object> dicData = data as Dictionary<string, object>;
		foreach (var item in dicData) {
			DisposeCallback (item);
		}

	}
	
	//========================================interface =================================

	public Dictionary<string, object> TransferData = new Dictionary<string, object> ();
	public List<TUserUnit> unitItemData = new List<TUserUnit>();
	private DragPanelSetInfo dpsi;

	void CreatArgs () {
		if (dpsi != null) {
			return;	
		}
		dpsi = new DragPanelSetInfo ();
		dpsi.parentTrans = viewComponent.transform;
		dpsi.scrollerScale = Vector3.one;
		dpsi.position = -28 * Vector3.up;
		dpsi.clipRange = new Vector4 (0, -120, 640, 400);
		dpsi.gridArrange = UIGrid.Arrangement.Vertical;
		dpsi.maxPerLine = 3;
		dpsi.scrollBarPosition = new Vector3 (-320, -303, 0);
		dpsi.cellWidth = 100;
		dpsi.cellHeight = 100;
		dpsi.depth = 2;
	}

	void InitDragpanel () {

		CreatArgs ();
		TransferData.Clear ();
		TransferData.Add (UnitDisplayUnity.SetDragPanel, dpsi);
		ExcuteCallback (TransferData);
	}

	void ReadData () {
		unitItemData.Clear ();
		unitItemData.AddRange (DataCenter.Instance.UserUnitList.GetAllMyUnit ());
		TransferData.Clear ();
		TransferData.Add (UnitDisplayUnity.RefreshData, unitItemData);
		ExcuteCallback (TransferData);
	}

	void DisposeCallback (KeyValuePair<string, object> key) {

		switch (key.Key) {
		case UnitDisplayUnity.SelectBase:
//			Debug.LogError ("DisposeCallback : " + key);
			MsgCenter.Instance.Invoke(CommandEnum.SelectUnitBase,key.Value);
			break;
		case UnitDisplayUnity.SelectMaterial:
			MsgCenter.Instance.Invoke(CommandEnum.selectUnitMaterial,key.Value);
			break;
		default:
			break;
		}
	}
}

public class DragPanelSetInfo {
	public Transform parentTrans;
	public Vector3 scrollerScale = Vector3.one;
	public Vector3 scrollerLocalPos = Vector3.zero;
	public Vector3 position = Vector3.zero;
	public Vector4 clipRange = Vector4.zero;
	public UIGrid.Arrangement gridArrange = UIGrid.Arrangement.Horizontal;
	public int maxPerLine = 1;
	public Vector3 scrollBarPosition = Vector3.zero;
	public int cellWidth = 100;
	public int cellHeight = 100;
	public int depth = 0;
}