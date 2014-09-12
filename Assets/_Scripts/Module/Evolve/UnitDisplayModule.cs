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

