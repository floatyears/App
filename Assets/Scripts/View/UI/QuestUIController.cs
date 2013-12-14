using UnityEngine;
using System.Collections;

public class QuestUIController : UIBase
{
	private DragUI activityScroller;
	private DragUI storyScroller;

	private ScrollView sv;

	public QuestUIController(string uiName):base(uiName)
	{
		
	}
	public override void CreatUI ()
	{
		sv = ViewManager.Instance.GetViewObject("QuestScrollView") as ScrollView; 
		sv.transform.localPosition = new Vector3(0, -80, 0);
		currentUIDic.Add(sv.UIName, sv);

		activityScroller = new DragUI(sv.TopLeft, sv.TopRight, sv.ActivityItem);
		activityScroller.ShowData(5);

		storyScroller = new DragUI(sv.BottomLeft, sv.BottomRight, sv.StoryItem);
		storyScroller.ShowData(4);

		for(int i= 0; i<activityScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get(activityScroller.DragList[i]);
		
			listen.onClick = ClickQuest;
		}

		for(int i= 0; i<storyScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get(storyScroller.DragList[i]);	
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
		foreach(var item in currentUIDic.Values)
		{
			item.HideUI();
		}

		activityScroller.insUIObject.SetActive(b);
		storyScroller.insUIObject.SetActive(b);

		sv.gameObject.SetActive(b);
	}
	
	void ClickQuest(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		sv.gameObject.SetActive(false);
	}


}
