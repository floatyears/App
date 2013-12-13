using UnityEngine;
using System.Collections;

public class QuestUIController : UIBase
{
	private DragUI activityScroller;
	private DragUI storyScroller;

	public QuestUIController(string uiName):base(uiName)
	{
		
	}
	public override void CreatUI ()
	{
		ScrollView actSV = ViewManager.Instance.GetViewObject("QuestActivityScrollView") as ScrollView; 

		ScrollView stySV = ViewManager.Instance.GetViewObject("QuestStoryScrollView") as ScrollView; 

		actSV.transform.localPosition = new Vector3(0, -85, 0);

		stySV.transform.localPosition = new Vector3(0, 60, 0);

		currentUIDic.Add(actSV.UIName, actSV);

		currentUIDic.Add(stySV.UIName, stySV);

		activityScroller = new DragUI(actSV.ActivityLeft, actSV.ActivityRight, actSV.ActivityItem);

		storyScroller = new DragUI(stySV.ActivityLeft, stySV.ActivityRight, stySV.ActivityItem);
		
		activityScroller.ShowData(5);

		storyScroller.ShowData(4);

		UIEventListener listen = UIEventListener.Get(activityScroller.DragList[0]);
		
		listen.onClick = ClickQuest;

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
		foreach(var item in currentUIDic.Values)
			item.HideUI();
		activityScroller.insUIObject.SetActive(b);
	}
	
	void ClickQuest(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}


}
