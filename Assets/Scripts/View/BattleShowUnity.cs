using UnityEngine;
using System.Collections;

public class BattleShowUnity : UIBaseUnity 
{
	public Transform topLeft;
	public Transform topRight;

	public Transform bottomLeft;
	public Transform bottomRight;

	private GameObject dragItem;

	public GameObject DragItem
	{
		get{return dragItem;}
	}

	public override void Init (string name)
	{
		base.Init (name);

		CreatUI();
	}

	public override void CreatUI ()
	{
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
