using UnityEngine;
using System.Collections.Generic;

public class DragPanelView : UIBaseUnity {

	public const string DragPanelPath = "Prefabs/UI/Share/DragPanelView";
	private UIPanel clip;
	private UIScrollView scrollView;
	public UIGrid grid;
	private UIScrollBar scrollBar;

	public override void Init (string name)
	{
		base.Init (name);

		clip = FindChild<UIPanel>("Scroll View");
		scrollView = FindChild<UIScrollView>("Scroll View");
		scrollBar = FindChild<UIScrollBar>("Scroll Bar");
		grid = FindChild<UIGrid>("Scroll View/UIGrid");
	}

	public override void CreatUI (){
		base.CreatUI ();
	
	}

	public override void ShowUI ()
	{
		base.ShowUI ();

	}

	public override void HideUI ()
	{
		base.HideUI ();

	}

	public override void DestoryUI ()
	{
		base.DestoryUI ();
	
	}
	int a = 0;
	public GameObject AddObject(GameObject obj) {

		tempObject = NGUITools.AddChild (grid.gameObject, obj);

		tempObject.name = a.ToString();
		a++;
		UIDragScrollView uidrag = tempObject.GetComponent<UIDragScrollView> ();
		if (uidrag == null) {
			//Debug.LogError("drag item is illegal");
			Destroy(tempObject);
			return null;
		}
		if(uidrag.scrollView  == null) {
			uidrag.scrollView = scrollView;
		}

		grid.enabled = true;
		grid.Reposition ();
		//Debug.LogError("tempObject : " + tempObject);
		return tempObject;
	}

//	public void RemoveObject (GameObject obj) {
//
//	}
	
	public void SetViewPosition(Vector4 position)
	{
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

	public void SetItemWidth(int width = - 1, int heigt = -1)
	{
		if(width > -1){
			grid.cellWidth = width;
		}

		if(heigt > -1){
			grid.cellHeight = heigt;
		}

		grid.enabled = true;
	}

	public void SetGridArgs(int cellWidth, int cellHeight, UIGrid.Arrangement arrangement = UIGrid.Arrangement.Horizontal, int maxPerLine = 0)
	{
		if( cellWidth < 0 || cellHeight < 0 || maxPerLine < 0 )
		{
			LogHelper.LogError( "Illegal args" );
			return;
		}
		grid.cellWidth = cellWidth;
		grid.cellHeight = cellHeight;
		grid.arrangement = arrangement;
		grid.maxPerLine = maxPerLine;
	}

	public void SetScrollBar( float pos_X, float pos_Y, float pos_Z = 0 )
	{
		scrollBar.gameObject.transform.localPosition = new Vector3( pos_X, pos_Y, pos_Z);
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
		gameObject.transform.parent = dpsi.parentTrans;
		gameObject.transform.localPosition = dpsi.scrollerLocalPos;
		gameObject.transform.localScale = dpsi.scrollerScale;
		scrollView.transform.localPosition = dpsi.position;
		clip.clipRange = dpsi.clipRange;
		scrollBar.transform.localPosition = dpsi.scrollBarPosition;
		grid.arrangement = dpsi.gridArrange;
		grid.maxPerLine = dpsi.maxPerLine;
		grid.cellWidth = dpsi.cellWidth;
		grid.cellHeight = dpsi.cellHeight;
		grid.enabled = true;
		grid.Reposition ();
	}

	public void SetScrollView(Dictionary< string, object > argsDic)
	{
		//default args List
		Transform parentTrans = transform.parent;
		Vector3 scrollerLocalPos = Vector3.zero;
		Vector3 scrollerScale = Vector3.one;
		Vector3 position = Vector3.zero;
		Vector4 clipRange = Vector4.zero;
		Vector3 scrollBarPosition = Vector3.zero;
		UIGrid.Arrangement gridArrange;
		gridArrange = UIGrid.Arrangement.Horizontal;
		int maxPerLine = 0;
		int cellWidth = 100;
		int cellHeight = 100;

		if( argsDic.ContainsKey( "parentTrans"))
			parentTrans = (Transform)argsDic["parentTrans"];
		if( argsDic.ContainsKey( "scrollerLocalPos"))
			scrollerLocalPos = (Vector3)argsDic["scrollerLocalPos"];
		if( argsDic.ContainsKey("scrollerScale"))
			scrollerScale = (Vector3)argsDic["scrollerScale"];
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

		gameObject.transform.parent = parentTrans;
		gameObject.transform.localPosition = scrollerLocalPos;
		gameObject.transform.localScale = scrollerScale;
		scrollView.transform.localPosition = position;
		clip.clipRange = clipRange;
		scrollBar.transform.localPosition = scrollBarPosition;
		grid.arrangement = gridArrange;
		grid.maxPerLine = maxPerLine;
		grid.cellWidth = cellWidth;
		grid.cellHeight = cellHeight;

		//LogHelper.Log( "  " + gameObject.name + " have finlished SetScrollView(dic)");

	}
	
}
