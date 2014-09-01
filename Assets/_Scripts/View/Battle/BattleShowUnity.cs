using UnityEngine;
using System.Collections;

public class BattleShowUnity : ViewBase 
{
	private Transform topLeft;
	public Transform TopLeft
	{
		get{return topLeft;}
	}

	private Transform topRight;
	public Transform TopRight
	{
		get{return topRight;}
	}

	private Transform bottomLeft;
	public Transform BottomLeft
	{
		get{return bottomLeft;}
	}

	private Transform bottomRight;
	public Transform BottomRight
	{
		get{return bottomRight;}
	}

	private GameObject dragItem;

	public GameObject DragItem
	{
		get{return dragItem;}
	}

	public override void Init (UIConfigItem config)
	{
		base.Init (config);

		topLeft = transform.Find("Top_Left");
		topRight = transform.Find("Top_Right");
		
		bottomLeft = transform.Find("Bottom_Left");
		bottomRight = transform.Find("Bottom_Right");
		
		dragItem = transform.Find("Item").gameObject;
		
		dragItem.SetActive(false);
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


}
