using UnityEngine;
using System.Collections;

public class QuestView : UIBase
{
	private QuestUnity window;
	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	private GameObject scrollerItem;
	private DragPanel storyScroller;
	private DragPanel eventScroller;

	public QuestView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{
		//Add Share UI -- SceneInfoBar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();

		//Add Self UI -- QuestWindow
		window = ViewManager.Instance.GetViewObject("QuestWindow") as QuestUnity; 
		window.transform.parent = viewManager.TopPanel.transform;
		window.Init ("Quest");
		currentUIDic.Add(window.UIName, window);

		//Add Scroller -- StoryScroller && EventScroller
		scrollerItem = Resources.Load("Prefabs/QuestScrollerItem") as GameObject;

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

		//Add Event Listener
		for(int i = 0; i < storyScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(storyScroller.ScrollItem[ i ].gameObject).onClick = TurnToQuest;
		}

		for(int i = 0; i < eventScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(eventScroller.ScrollItem[ i ].gameObject).onClick = TurnToQuest;
		}

	}

	void SetActive(bool b)
	{	
		window.gameObject.SetActive(b);
		storyScroller.RootObject.gameObject.SetActive(b);
		eventScroller.RootObject.gameObject.SetActive(b);
		sceneInfoBar.gameObject.SetActive(b);
	}

	public override void ShowUI ()
	{
		SetActive(true);
		backBtn.isEnabled = false;
		sceneInfoLab.text = uiName;
	}
	
	public override void HideUI ()
	{
		SetActive(false);
	}
	
	public override void DestoryUI ()
	{	

	}

	void TurnToQuest(GameObject go)
	{
		//window.gameObject.SetActive(false);

		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}


}
