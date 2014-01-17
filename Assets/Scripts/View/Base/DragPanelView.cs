using UnityEngine;
using System.Collections;

public class DragPanelView : UIBaseUnity {

	private UIPanel clip;

	private UIScrollView scrollView;

	private UIGrid grid;
	
	private UIScrollBar scrollBar;

	public override void Init (string name)
	{
		base.Init (name);

		clip = FindChild<UIPanel>("Scroll View");
		scrollView = FindChild<UIScrollView>("Scroll View");
		scrollBar = FindChild<UIScrollBar>("Scroll Bar");
		grid = FindChild<UIGrid>("Scroll View/UIGrid");


	}

	public override void CreatUI ()
	{
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

	public GameObject AddObject(GameObject obj,int index)
	{
		tempObject = NGUITools.AddChild (grid.gameObject, obj);
		tempObject.name = index.ToString();
		UIDragScrollView uidrag = tempObject.GetComponent<UIDragScrollView> ();
		if (uidrag == null) {
			LogHelper.LogError("drag item is illegal");
			Destroy(tempObject);
			return null;
		}

		uidrag.scrollView = scrollView;
		grid.enabled = true;
		return tempObject;
	}
	
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
	
}
