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
	
	public const string ScrollViewDepth = "ScrollViewDepth";

	public void SetScrollView(DragPanelConfigItem config, Transform parent){

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
		if (config.scrollMovement == UIScrollView.Movement.Vertical ) {
			scrollView.horizontalScrollBar = null;	
			scrollView.verticalScrollBar = scrollBar;
//			scrollBar.fillDirection = UIProgressBar.FillDirection.RightToLeft;

			fg.Rotate (0, 0, 0);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)config.clipRange.w;

		}else {
			scrollView.horizontalScrollBar = scrollBar;	
			scrollView.verticalScrollBar = null;

			fg.Rotate (0, 0, -90);
			fg.GetComponent<UISprite> ().alpha = 1;
			fg.GetComponent<UISprite> ().width = (int)config.clipRange.z;

			scrollBar.fillDirection = UIProgressBar.FillDirection.RightToLeft;
		}

		scrollView.ResetPosition ();
//		scrollBar.alpha = 1;
	}
	
}
