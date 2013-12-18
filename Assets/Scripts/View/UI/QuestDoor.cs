using UnityEngine;
using System.Collections;

public class QuestDoor : UIBaseUnity
{
	//Transform
	private Transform leftTransform;
	public Transform LeftTransform
	{
		get{return leftTransform;}
	}
	
	private Transform rightTransform;
	public Transform RightTransform
	{
		get{return rightTransform;}
	}

	//Item
	private GameObject item;
	public GameObject Item
	{
		get{return item;}
	}


	public override void Init (string name)
	{
		base.Init (name);

		leftTransform = transform.Find("Left_Transform");
		rightTransform = transform.Find("Right_Transform");
		
		item = transform.Find("Item").gameObject;
		
		item.SetActive(false);
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
