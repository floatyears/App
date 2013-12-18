using UnityEngine;
using System.Collections;

public class QuestView : UIBase
{
	private ScrollView StoryScroller;
	private ScrollView EventScroller;
	
	private QuestDoor StoryDoor;
	private QuestDoor EventDoor;

	public QuestView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		StoryDoor = ViewManager.Instance.GetViewObject("StoryDoor") as QuestDoor; 
		StoryDoor.transform.localPosition = 210*Vector3.up;
		currentUIDic.Add(StoryDoor.UIName, StoryDoor);
		StoryScroller = new ScrollView(StoryDoor.LeftTransform, StoryDoor.RightTransform, StoryDoor.Item);
		StoryScroller.ShowData(5);
		
		for(int i= 0; i<StoryScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( StoryScroller.DragList[ i ] );	
			listen.onClick = ClickQuest;
		}

		EventDoor = ViewManager.Instance.GetViewObject("EventDoor") as QuestDoor; 
		EventDoor.transform.localPosition = -100*Vector3.up;
		currentUIDic.Add(EventDoor.UIName, EventDoor);
		EventScroller = new ScrollView(EventDoor.LeftTransform, EventDoor.RightTransform, EventDoor.Item);
		EventScroller.ShowData(5);
		
		for(int i= 0; i<EventScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( EventScroller.DragList[ i ] );	
			listen.onClick = ClickQuest;
		}

	}

	public override void ShowUI ()
	{
		SetActive(true);
	}
	
	public override void HideUI ()
	{
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{	
	}
	
	void SetActive(bool b)
	{
		StoryScroller.insUIObject.SetActive(b);
		EventScroller.insUIObject.SetActive(b);

		StoryDoor.gameObject.SetActive(b);
		EventDoor.gameObject.SetActive(b);
	}
	
	void ClickQuest(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		StoryDoor.gameObject.SetActive(false);
		EventDoor.gameObject.SetActive(false);
	}


}
