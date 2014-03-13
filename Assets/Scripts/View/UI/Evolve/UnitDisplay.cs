using UnityEngine;
using System.Collections.Generic;

public class UnitDisplay : ConcreteComponent {
	public UnitDisplay (string name) : base (name) {

	}

	public override void Callback (object data) {
		base.Callback (data);
	}

	public override void ShowUI () {
		base.ShowUI ();
	}

	public override void HideUI () {
		base.HideUI ();
	}

	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
	//========================================interface =================================

	public Dictionary<string, object> TransferData = new Dictionary<string, object> ();


}


public class DragPanelSetInfo {
//	dragPanelArgs.Add("parentTrans",	transform);
//	dragPanelArgs.Add("scrollerScale",	Vector3.one);
//	dragPanelArgs.Add("scrollerLocalPos",	-28 * Vector3.up);
//	dragPanelArgs.Add("position", 		Vector3.zero);
//	dragPanelArgs.Add("clipRange", 		new Vector4(0, -120, 640, 400));
//	dragPanelArgs.Add("gridArrange", 	UIGrid.Arrangement.Vertical);
//	dragPanelArgs.Add("maxPerLine",		3);
//	dragPanelArgs.Add("scrollBarPosition",	new Vector3(-320, -315, 0));
//	dragPanelArgs.Add("cellWidth", 		110);
//	dragPanelArgs.Add("cellHeight",		110);
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