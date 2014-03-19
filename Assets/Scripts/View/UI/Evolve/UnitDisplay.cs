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

	public override void Callback (object data) {
		base.Callback (data);
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
		dpsi.scrollBarPosition = new Vector3 (-320, -315, 0);
		dpsi.cellWidth = 110;
		dpsi.cellHeight = 110;
	}

	void InitDragpanel () {

		CreatArgs ();
		TransferData.Clear ();
		TransferData.Add (UnitDisplayUnity.SetDragPanel, dpsi);
		ExcuteCallback (TransferData);
	}

	void ReadData () {
		unitItemData.Clear ();
		unitItemData.AddRange (DataCenter.Instance.MyUnitList.GetAll ().Values);
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
	public Vector3 scrollerScale;
	public Vector3 scrollerLocalPos;
	public Vector3 position;
	public Vector4 clipRange;
	public UIGrid.Arrangement gridArrange;
	public int maxPerLine;
	public Vector3 scrollBarPosition;
	public int cellWidth;
	public int cellHeight;
}