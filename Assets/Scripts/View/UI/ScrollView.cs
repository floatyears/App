using UnityEngine;
using System.Collections;

public class ScrollView : UIBaseUnity
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
	
	private GameObject storyItem;
	public GameObject StoryItem
	{
		get{return storyItem;}
	}

	private GameObject activityItem;
	public GameObject ActivityItem
	{
		get{return activityItem;}
	}
	
	public override void Init (string name)
	{
		base.Init (name);

		topLeft = transform.Find("TopLeft");
		topRight = transform.Find("TopRight");
		
		bottomLeft = transform.Find("BottomLeft");
		bottomRight = transform.Find("BottomRight");
		
		activityItem = transform.Find("ItemActivity").gameObject;
		storyItem = transform.Find("ItemStory").gameObject;
		
		activityItem.SetActive(false);
		storyItem.SetActive(false);
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
