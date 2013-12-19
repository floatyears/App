using UnityEngine;
using System.Collections;

public class QuestView : UIBase
{
	private ScrollView StoryScroller;
	private ScrollView EventScroller;
	
	private QuestUnity StoryDoor;
	private QuestUnity EventDoor;

	public QuestView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		StoryDoor = ViewManager.Instance.GetViewObject("StoryDoor") as QuestUnity; 

		StoryDoor.transform.parent = viewManager.TopPanel.transform;

		StoryDoor.transform.localPosition = -400*Vector3.up;

		StoryDoor.Init ("StoryDoor");

		currentUIDic.Add(StoryDoor.UIName, StoryDoor);

		StoryScroller = new ScrollView(StoryDoor.LeftTransform, StoryDoor.RightTransform, StoryDoor.Item);

		StoryScroller.ShowData(5);
		
		for(int i= 0; i<StoryScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( StoryScroller.DragList[ i ] );	
			listen.onClick = ClickQuest;
		}

		EventDoor = ViewManager.Instance.GetViewObject("EventDoor") as QuestUnity; 
		EventDoor.transform.parent = viewManager.TopPanel.transform;
		EventDoor.transform.localPosition = -740*Vector3.up;
		EventDoor.Init ("EventDoor");
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
		StoryDoor.gameObject.SetActive(false);
		EventDoor.gameObject.SetActive(false);
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}


}
