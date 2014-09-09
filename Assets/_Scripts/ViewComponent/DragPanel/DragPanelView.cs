using UnityEngine;
using System.Collections.Generic;

public class DragPanelView : ViewBase {
	public const string DragPanelPath = "Prefabs/UI/Common/DragPanelView";

	[HideInInspector]
	public UIGrid grid;

	[HideInInspector]
	public UIPanel clip;

	[HideInInspector]
	public UIScrollBar scrollBar;

	[HideInInspector]
	public UIScrollView scrollView;

	[HideInInspector]
	public DragPanelDynamic dragPanelDynamic;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
		clip = FindChild<UIPanel>("Scroll View");
		scrollView = FindChild<UIScrollView>("Scroll View");
		scrollBar = FindChild<UIScrollBar>("Scroll Bar");
		grid = FindChild<UIGrid>("Scroll View/UIGrid");
	}


	public override void ShowUI (){
		base.ShowUI ();
	}

	public override void HideUI (){
		base.HideUI ();
	}

	public override void DestoryUI (){
		base.DestoryUI ();
	}
	int a = 0;
	public GameObject AddObject(GameObject obj) {
		GameObject tempObject = null;

		tempObject = NGUITools.AddChild (grid.gameObject, obj);
//		Debug.LogError ("tempObject : " + tempObject.name + " a.ToString() : " + a.ToString ());
		tempObject.name = a.ToString();
		a++;
		UIDragScrollView uidrag = tempObject.GetComponent<UIDragScrollView> ();


		if (uidrag == null) {
			Debug.LogError("drag item is illegal");
			Destroy(tempObject);
			return null;
		}

		if(uidrag.scrollView  == null) {
			uidrag.scrollView = scrollView;
		}

		return tempObject;
	}

	public GameObject AddObject(GameObject obj, int name) {
		GameObject tempObject = null;

		tempObject = NGUITools.AddChild (grid.gameObject, obj);
		
		tempObject.name = name.ToString();
		UIDragScrollView uidrag = tempObject.GetComponent<UIDragScrollView> ();
		if (uidrag == null) {
			Debug.LogError("drag item is illegal");
			Destroy(tempObject);
			return null;
		}
		if(uidrag.scrollView  == null) {
			uidrag.scrollView = scrollView;
		}
		
//		grid.enabled = true;
//		grid.Reposition ();
		//Debug.LogError("tempObject : " + tempObject);
		return tempObject;
	}

//	public void RemoveObject (GameObject obj) {
//
//	}
	
	public void SetViewPosition(Vector4 position){
		Vector4 range = clip.clipRange;

		range.x = position.x;
		range.y = position.y;		

		if (position.z > 0) {
			range.z = position.z;		
		}

		if (position.w > 0) {
			range.w = position.w;		
		}

		clip.clipRange = range;
	}

//	public void UpdateScrollArgument(string key, object value){
//		switch (key)
//		{
//			case "parentTrans":
//				parentTrans = (Transform)value;
//				break;
//			
//			default:
//				break;
//		}
//	}
//
//	public void UpdateScrollArgs(Dictionary< string, object > argsDic){
//		foreach (var key in argsDic.Keys)
//		{
//			UpdateScrollArgument(key, argsDic[key]);
//		}
//	}

	public void SetDragPanel(DragPanelSetInfo dpsi) {
		scrollView.GetComponent<UIPanel> ().depth = dpsi.depth;
		gameObject.transform.parent = dpsi.parentTrans;
		gameObject.transform.localPosition = dpsi.scrollerLocalPos;
		gameObject.transform.localScale = dpsi.scrollerScale;
		scrollView.transform.localPosition = dpsi.position;
//		Debug.LogError ("set drag panel dpsi : " + scrollView.transform.localPosition);
		clip.clipRange = dpsi.clipRange;
		scrollBar.transform.localPosition = dpsi.scrollBarPosition;
		grid.arrangement = dpsi.gridArrange;
		grid.maxPerLine = dpsi.maxPerLine;
		grid.cellWidth = dpsi.cellWidth;
		grid.cellHeight = dpsi.cellHeight;
		grid.enabled = true;

		Transform fg = scrollBar.transform.FindChild ("Foreground");
		if (dpsi.gridArrange == UIGrid.Arrangement.Horizontal) {
			scrollView.horizontalScrollBar = null;	
			scrollView.verticalScrollBar = scrollBar;
			fg.Rotate (0, 0, -90);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)dpsi.clipRange.w;
		} else {
			scrollView.horizontalScrollBar = scrollBar;	
			scrollView.verticalScrollBar = null;
			fg.Rotate (0, 0, 0);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)dpsi.clipRange.z;
		}

		grid.Reposition ();

//		scrollBar.alpha = 1;
	}

	public const string ScrollViewDepth = "ScrollViewDepth";

	public void SetScrollView(DragPanelConfigItem config, Transform parent){
//		Vector3 scrollerLocalPos = Vector3.zero;
//		Vector3 position = Vector3.zero;
//		Vector4 clipRange = Vector4.zero;
//		Vector3 scrollBarPosition = Vector3.zero;
//		UIGrid.Arrangement gridArrange = UIGrid.Arrangement.Horizontal;
//		UIScrollView.Movement scrollMovement = UIScrollView.Movement.Horizontal;
//		UIScrollBar.FillDirection scrollBarDir = UIProgressBar.FillDirection.LeftToRight;
//		int maxPerLine = 0;
//		int cellWidth = 100;
//		int cellHeight = 100;

////		scrollBarDir = (UIScrollBar.FillDirection)config.sc["scrollBarDir"];
//		scrollMovement = config.scrollMovement;//(UIScrollView.Movement)config["scrollMovement"];
//		scrollerLocalPos = config.scrollerLocalPos;//(Vector3)config["scrollerLocalPos"];
//		position = config.position;//(Vector3)config["position"];
//		clipRange = config.clipRange;//(Vector4)config["clipRange"];
//		scrollBarPosition = config.scrollBarPosition;//(Vector3)config["scrollBarPosition"];
//		gridArrange = config.gridArrage;//(UIGrid.Arrangement)config["gridArrange"];
//		maxPerLine = config;(int)config["maxPerLine"];
//		cellWidth = (int)config["cellWidth"];
//		cellHeight = (int)config["cellHeight"];

		scrollBar.GetComponent<UIPanel>().depth = scrollView.GetComponent<UIPanel>().depth = config.depth;

		scrollView.movement = config.scrollMovement;
		gameObject.transform.parent = parent;
		transform.localScale = Vector3.one;
		gameObject.transform.localPosition = config.scrollerLocalPos;
		scrollView.transform.localPosition = config.position;
		clip.clipRange = config.clipRange;
		scrollBar.transform.localPosition = config.scrollBarPosition;

		grid.arrangement = config.gridArrage;
		grid.maxPerLine = config.maxPerLine;
		grid.cellWidth = config.cellWidth;
		grid.cellHeight = config.cellHeight;

		Transform fg = scrollBar.transform.FindChild ("Foreground");
		if (config.scrollMovement == UIScrollView.Movement.Vertical) {
				scrollView.horizontalScrollBar = null;	
				scrollView.verticalScrollBar = scrollBar;
				fg.Rotate (0, 0, -90);
				fg.GetComponent<UISprite> ().alpha = 1;
				fg.GetComponent<UISprite> ().width = (int)config.clipRange.w;
		} else {
				scrollView.horizontalScrollBar = scrollBar;	
				scrollView.verticalScrollBar = null;
				fg.Rotate (0, 0, 0);
				fg.GetComponent<UISprite> ().alpha = 1;
				fg.GetComponent<UISprite> ().width = (int)config.clipRange.z;
		}

		scrollView.ResetPosition ();
//		scrollBar.alpha = 1;
	}
	
}
