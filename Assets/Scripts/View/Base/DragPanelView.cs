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
		if (position.x > 0) {
			range.x = position.x;
		}

		if (position.y > 0) {
			range.y = position.y;		
		}

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
}
