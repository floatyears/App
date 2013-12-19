using UnityEngine;
using System.Collections;

public class OthersScrollView : UIBaseUnity
{

	private Transform left;
	public Transform Left
	{
		get{return left;}
	}
	
	private Transform right;
	public Transform Right
	{
		get{return right;}
	}
	
	private GameObject item;
	public GameObject Item
	{
		get{return item;}
	}
	
	public override void Init (string name)
	{
		base.Init (name);

		left = transform.Find("Left");
		right = transform.Find("Right");
		
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
