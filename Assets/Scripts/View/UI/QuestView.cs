using UnityEngine;
using System.Collections;

public class QuestView : UIBase
{
	private QuestUnity window;
	private SceneInfoBar sceneInfoBar;

	private GameObject scrollerItem;
	private DragPanel storyScroller;
	private DragPanel eventScroller;

	public QuestView(string uiName):base(uiName){}
	public override void CreatUI ()
	{
		sceneInfoBar = ViewManager.Instance.GetViewObject( UIConfig.sharePath + "SceneInfoBar" ) as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		window = ViewManager.Instance.GetViewObject( UIConfig.questPath + "QuestWindow") as QuestUnity; 
		window.transform.parent = viewManager.TopPanel.transform;
		window.Init ("Quest");
		currentUIDic.Add(window.UIName, window);

		scrollerItem = Resources.Load("Prefabs/UI/Quest/QuestScrollerItem") as GameObject;
		storyScroller = new DragPanel ("StoryScroller", scrollerItem);
		storyScroller.CreatUI ();
		storyScroller.AddItem (15);
		storyScroller.RootObject.SetItemWidth(230);
		storyScroller.RootObject.gameObject.transform.localPosition = -350*Vector3.up;

		eventScroller = new DragPanel ("EventScroller", scrollerItem);
		eventScroller.CreatUI();
		eventScroller.AddItem (10);
		eventScroller.RootObject.SetItemWidth(230);
		eventScroller.RootObject.gameObject.transform.localPosition = -700*Vector3.up;
	}

	public override void ShowUI ()
	{
		SetUIActive(true);
		sceneInfoBar.BackBtn.isEnabled = false;
		sceneInfoBar.UITitleLab.text = UIName;

		for(int i = 0; i < storyScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(storyScroller.ScrollItem[ i ].gameObject).onClick += TurnToQuest;
		}
		
		for(int i = 0; i < eventScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(eventScroller.ScrollItem[ i ].gameObject).onClick += TurnToQuest;
		}
	}
	
	public override void HideUI ()
	{
		SetUIActive(false);

		for(int i = 0; i < storyScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(storyScroller.ScrollItem[ i ].gameObject).onClick -= TurnToQuest;
		}
		
		for(int i = 0; i < eventScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(eventScroller.ScrollItem[ i ].gameObject).onClick -= TurnToQuest;
		}
	}

	private void SetUIActive(bool b)
	{	
		window.gameObject.SetActive(b);
		storyScroller.RootObject.gameObject.SetActive(b);
		eventScroller.RootObject.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

	//add to selef script
	private void TurnToQuest(GameObject go)
	{
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}
}
