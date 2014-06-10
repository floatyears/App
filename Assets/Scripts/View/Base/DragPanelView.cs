using UnityEngine;
using System.Collections.Generic;

public class DragPanelView : UIBaseUnity {
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

	public override void Init (string name){
		base.Init (name);
		clip = FindChild<UIPanel>("Scroll View");
		scrollView = FindChild<UIScrollView>("Scroll View");
		scrollBar = FindChild<UIScrollBar>("Scroll Bar");
		grid = FindChild<UIGrid>("Scroll View/UIGrid");
	}

	public override void CreatUI (){
		base.CreatUI ();
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

		tempObject = NGUITools.AddChild (grid.gameObject, obj);

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

//		grid.enabled = true;
//		grid.Reposition ();
		//Debug.LogError("tempObject : " + tempObject);
		return tempObject;
	}

	public GameObject AddObject(GameObject obj, int name) {
		
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
		grid.Reposition ();

	}

	public const string ScrollViewDepth = "ScrollViewDepth";
//	public const string 

	public void SetScrollView(Dictionary<string, object> argsDic, Transform parent){
//		Debug.LogError ("gameobject befoure : " + transform.localScale);

		Vector3 scrollerLocalPos = Vector3.zero;
		Vector3 position = Vector3.zero;
		Vector4 clipRange = Vector4.zero;
		Vector3 scrollBarPosition = Vector3.zero;
		UIGrid.Arrangement gridArrange = UIGrid.Arrangement.Horizontal;
		UIScrollView.Movement scrollMovement = UIScrollView.Movement.Horizontal;
		UIScrollBar.FillDirection scrollBarDir = UIProgressBar.FillDirection.LeftToRight;
		int maxPerLine = 0;
		int cellWidth = 100;
		int cellHeight = 100;
		object depth = null;

		if( argsDic.ContainsKey( "scrollBarDir"))
			scrollBarDir = (UIScrollBar.FillDirection)argsDic["scrollBarDir"];
		if( argsDic.ContainsKey( "scrollMovement"))
			scrollMovement = (UIScrollView.Movement)argsDic["scrollMovement"];
		if( argsDic.ContainsKey( "scrollerLocalPos"))
			scrollerLocalPos = (Vector3)argsDic["scrollerLocalPos"];
		if( argsDic.ContainsKey( "position" ))
			position = (Vector3)argsDic["position"];
		if( argsDic.ContainsKey( "clipRange" ))
			clipRange = (Vector4)argsDic["clipRange"];
		if( argsDic.ContainsKey("scrollBarPosition"))
			scrollBarPosition = (Vector3)argsDic["scrollBarPosition"];
		if( argsDic.ContainsKey( "gridArrange"))
			gridArrange = (UIGrid.Arrangement)argsDic["gridArrange"];
		if( argsDic.ContainsKey("maxPerLine"))
			maxPerLine = (int)argsDic["maxPerLine"];
		if( argsDic.ContainsKey("cellWidth"))
			cellWidth = (int)argsDic["cellWidth"];
		if( argsDic.ContainsKey("cellHeight"))
			cellHeight = (int)argsDic["cellHeight"];
		if( argsDic.ContainsKey("depth"))
			scrollView.GetComponent<UIPanel>().depth = (int)argsDic["depth"];

		//argsDic.TryGetValue (ScrollViewDepth,out depth);
		scrollBar.fillDirection = scrollBarDir;
		scrollView.movement = scrollMovement;
//		if (depth != null) {
//			scrollView.GetComponent<UIPanel> ().depth = (int)depth;
//		}

//		Debug.LogError ("set drag panel dictionary : " + scrollView.transform.localPosition);
//		Debug.LogError ("gameobject end aa : " + transform.localScale + " parent : " + parent.transform.localScale);
		gameObject.transform.parent = parent;
		transform.localScale = Vector3.one;
        gameObject.transform.localPosition = scrollerLocalPos;
		scrollView.transform.localPosition = position;
		clip.clipRange = clipRange;
		scrollBar.transform.localPosition = scrollBarPosition;
		grid.arrangement = gridArrange;
		grid.maxPerLine = maxPerLine;
		grid.cellWidth = cellWidth;
		grid.cellHeight = cellHeight;

		if (scrollMovement == UIScrollView.Movement.Vertical) {
			scrollView.horizontalScrollBar = null;	
			scrollView.verticalScrollBar = scrollBar;
		}

		scrollView.ResetPosition ();
//		scrollView.gameObject.AddComponent<SpringPanel> ();
//		
		//Debug.LogError( "  " + gameObject.name + " have finlished SetScrollView(dic)");
	}
	
}
