using UnityEngine;
using System.Collections;

public class ScrollView : UIBaseUnity
{

	private Transform activityLeft;
	public Transform ActivityLeft
	{
		get{return activityLeft;}
	}
	
	private Transform activityRight;
	public Transform ActivityRight
	{
		get{return activityRight;}
	}

	private GameObject activityItem;
	public GameObject ActivityItem
	{
		get{return activityItem;}
	}

	DragUI activityScrollView;


	private Transform storyLeft;
	public Transform StoryLeft
	{
		get{return storyLeft;}
	}
	
	private Transform storyRight;
	public Transform StoryRight
	{
		get{return storyRight;}
	}
	
	private GameObject storyItem;
	public GameObject StoryItem
	{
		get{return storyItem;}
	}
	
	DragUI storyScrollView;

	public override void Init (string name)
	{
		base.Init (name);

		activityItem = transform.Find("Item").gameObject;
		activityLeft = transform.Find("LeftPos");
		activityRight = transform.Find("RightPos");
		activityItem.SetActive(false);

		storyItem = transform.Find("Item").gameObject;
		storyLeft = transform.Find("LeftPos");
		storyRight = transform.Find("RightPos");
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
