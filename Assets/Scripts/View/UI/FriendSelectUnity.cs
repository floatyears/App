﻿using UnityEngine;
using System.Collections;

public class FriendSelectUnity : UIBaseUnity 
{
	private Transform leftTranform;
	public Transform LeftTransform
	{
		get{return leftTranform;}
	}

	private Transform rightTransform;
	public Transform RightTransform
	{
		get{return rightTransform;}
	}

	private GameObject friendItem;
	public GameObject FriendItem
	{
		get{return friendItem;}
	}
	
	public override void Init(string name)
	{
		base.Init(name);

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
