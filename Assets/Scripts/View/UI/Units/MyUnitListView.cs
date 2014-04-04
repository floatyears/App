using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyUnitListView : UIComponentUnity {
	GameObject dragItem;
	DragPanel dragPanel;
	Dictionary<string, object> dragPanelArgs = new Dictionary<string, object>();

	public override void Init ( UIInsConfig config, IUICallback origin ) {
		base.Init (config, origin);
		InitUIElement();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowUIAnimation();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}

	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs callArgs = data as CallBackDispatcherArgs;
		switch (callArgs.funcName){
			case "CreateDragPanelView" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateDragPanel, callArgs);
				break;
			case "DestoryDragPanelView" : 
				CallBackDispatcherHelper.DispatchCallBack(DestoryDragPanel, callArgs);
				break;
			default:
				break;
		}
	}

	void InitUIElement(){
		InitDragPanelArgs();
	}

	void CreateDragPanel(object args){
		List<TUserUnit> data = args as List<TUserUnit>;
		dragPanel = new DragPanel("DragPanel", MyUnitView.ItemPrefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(data.Count);
		dragPanel.DragPanelView.SetScrollView(dragPanelArgs);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			MyUnitView.Inject(dragPanel.ScrollItem[ i ]).Init(data[ i ]);
		}

	}

	void DestoryDragPanel(object args){
		dragPanel.DestoryUI();
	}
	

	void InitDragPanelArgs(){
		dragPanelArgs.Add("parentTrans", transform);
		dragPanelArgs.Add("scrollerScale", Vector3.one);
		dragPanelArgs.Add("scrollerLocalPos", 220 * Vector3.up);
		dragPanelArgs.Add("position", Vector3.zero);
		dragPanelArgs.Add("clipRange", new Vector4(0, -210, 640, 600));
		dragPanelArgs.Add("gridArrange", UIGrid.Arrangement.Vertical);
		dragPanelArgs.Add("maxPerLine", 4);
		dragPanelArgs.Add("scrollBarPosition", new Vector3(-320, -540, 0));
		dragPanelArgs.Add("cellWidth", 140);
		dragPanelArgs.Add("cellHeight", 140);
	}

	void ShowUIAnimation(){
		transform.localPosition = new Vector3(-1000, 0 , 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));
	}
	
}
