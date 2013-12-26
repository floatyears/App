using UnityEngine;
using System.Collections;

public class QuestView : UIBase
{
	private ScrollView StoryScroller;
	private ScrollView EventScroller;

	private QuestUnity StoryDoor;
	private QuestUnity EventDoor;

	private SceneInfoBar sceneInfoBar;
	private UIImageButton backBtn;
	private UILabel sceneInfoLab;

	public QuestView(string uiName):base(uiName)
	{

	}
	public override void CreatUI ()
	{

		//add scene info bar
		sceneInfoBar = ViewManager.Instance.GetViewObject("SceneInfoBar") as SceneInfoBar;
		sceneInfoBar.transform.parent = viewManager.TopPanel.transform;
		sceneInfoBar.transform.localPosition = Vector3.zero;

		sceneInfoLab = sceneInfoBar.transform.Find("Lab_UI_Name").GetComponent<UILabel>();
		backBtn = sceneInfoBar.transform.Find("ImgBtn_Arrow").GetComponent<UIImageButton>();



		StoryDoor = ViewManager.Instance.GetViewObject("StoryDoor") as QuestUnity; 
		StoryDoor.transform.parent = viewManager.TopPanel.transform;
		StoryDoor.transform.localPosition = -350*Vector3.up;
		StoryDoor.Init ("StoryDoor");
		currentUIDic.Add(StoryDoor.UIName, StoryDoor);

//		GameObject go = Resources.Load ("Prefabs/DragPanelItem") as GameObject;
//		DragPanel dp = new DragPanel ("Test", go);
//		dp.CreatUI ();
//		dp.AddItem (5);

		StoryScroller = new ScrollView(StoryDoor.LeftTransform, StoryDoor.RightTransform, StoryDoor.Item);
		StoryScroller.ShowData(5);
		
		for(int i= 0; i<StoryScroller.DragList.Count; i++)
		{
			UIEventListener listen = UIEventListener.Get( StoryScroller.DragList[ i ] );	
			listen.onClick = ClickQuest;
		}

		EventDoor = ViewManager.Instance.GetViewObject("EventDoor") as QuestUnity; 
		EventDoor.transform.parent = viewManager.TopPanel.transform;
		EventDoor.transform.localPosition = -650*Vector3.up;
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

	void SetActive(bool b)
	{
		StoryScroller.insUIObject.SetActive(b);
		EventScroller.insUIObject.SetActive(b);
		
		StoryDoor.gameObject.SetActive(b);
		EventDoor.gameObject.SetActive(b);
		
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
	

	
	void ClickQuest(GameObject go)
	{
		StoryDoor.gameObject.SetActive(false);
		EventDoor.gameObject.SetActive(false);
		ControllerManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}


}
