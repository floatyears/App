using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitDisplayModule : ModuleBase {
	public UnitDisplayModule (UIConfigItem config) : base (  config) {
	}
	
	public override void ShowUI () {
	
		base.ShowUI ();
//		if(UIManager.Instance.baseScene.PrevScene != ModuleEnum.UnitDetail)
//			ReadData ();
	
	}

	public override void HideUI () {
		base.HideUI ();

//		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
//			base.DestoryUI();
//		}
	}

	public override void InitUI () {
		base.InitUI ();
	}

	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages(params object[] data) {
		base.OnReceiveMessages (data);
		Dictionary<string, object> dicData = data[0] as Dictionary<string, object>;
		foreach (var item in dicData) {
			DisposeCallback (item);
		}
	}
	
	//========================================interface =================================

	public Dictionary<string, object> TransferData = new Dictionary<string, object> ();
	public List<TUserUnit> unitItemData = new List<TUserUnit>();
	private DragPanelSetInfo dpsi;
	
	void ReadData () {
		unitItemData.Clear ();
		unitItemData.AddRange (DataCenter.Instance.UserUnitList.GetAllMyUnit ());
		TransferData.Clear ();
		TransferData.Add (UnitListDragPanelView.RefreshData, unitItemData);
		view.CallbackView (TransferData);
	}

	void DisposeCallback (KeyValuePair<string, object> key) {

		switch (key.Key) {
		case UnitListDragPanelView.SelectBase:
//			Debug.LogError ("DisposeCallback : " + key);
			MsgCenter.Instance.Invoke(CommandEnum.SelectUnitBase,key.Value);
			break;
//		case UnitDisplayUnity.SelectMaterial:
//			MsgCenter.Instance.Invoke(CommandEnum.selectUnitMaterial,key.Value);
//			break;
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
//	UIScrollView.Movement scrollMovement = UIScrollView.Movement.Horizontal;
}