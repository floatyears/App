 using UnityEngine;
using System.Collections;

public class QuestDecoratorUnity : UIComponentUnity {

	private DragPanel storyScroller;
	private DragPanel eventScroller;
	private GameObject scrollerItem;

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		InitUI();
		base.Init (config, origin);

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
//		storyScroller.RootObject.gameObject.SetActive(b);
//		eventScroller.RootObject.gameObject.SetActive(b);
	}
	
	private void TurnToQuest(GameObject go)
	{
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}

	private void InitUI()
	{
		scrollerItem = Resources.Load("Prefabs/UI/Quest/QuestScrollerItem") as GameObject;
		storyScroller = new DragPanel ("StoryScroller", scrollerItem);
		storyScroller.CreatUI ();
		storyScroller.AddItem (15);
		storyScroller.RootObject.SetItemWidth(230);

		storyScroller.RootObject.gameObject.transform.parent = this.gameObject.transform.FindChild("story_window");
		storyScroller.RootObject.gameObject.transform.localScale = Vector3.one;

		storyScroller.RootObject.gameObject.transform.localPosition = 220*Vector3.up;
		
		eventScroller = new DragPanel ("EventScroller", scrollerItem);
		eventScroller.CreatUI();
		eventScroller.AddItem (10);
		eventScroller.RootObject.SetItemWidth(230);

		eventScroller.RootObject.gameObject.transform.parent = this.gameObject.transform.FindChild( "event_window" );
		eventScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		eventScroller.RootObject.gameObject.transform.localPosition = -140*Vector3.up;
	}
}
