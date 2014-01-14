using UnityEngine;
using System.Collections;

public class QuestComponent : ConcreteComponent {

	private DragPanel storyScroller;
	private DragPanel eventScroller;
	private GameObject scrollerItem;

	public QuestComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
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
	
	public override void ShowUI () {
		base.ShowUI ();
		SetUIActive(true);
		for(int i = 0; i < storyScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(storyScroller.ScrollItem[ i ].gameObject).onClick += TurnToQuest;
		}
		
		for(int i = 0; i < eventScroller.ScrollItem.Count; i++)
		{
			UIEventListener.Get(eventScroller.ScrollItem[ i ].gameObject).onClick += TurnToQuest;
		}

	}
	
	public override void HideUI () {
		base.HideUI ();
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
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void SetUIActive(bool b)
	{	
		storyScroller.RootObject.gameObject.SetActive(b);
		eventScroller.RootObject.gameObject.SetActive(b);
	}

	private void TurnToQuest(GameObject go)
	{
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}
}
