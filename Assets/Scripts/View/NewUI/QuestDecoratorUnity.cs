 using UnityEngine;
using System.Collections.Generic;

public class QuestDecoratorUnity : UIComponentUnity {

	private DragPanel storyScroller;
	private DragPanel eventScroller;
	private GameObject scrollerItem;
	private string scrollerItemSourcePath = TempConfig.questItemSourcePath;
	private string storyTextureSourcePath = TempConfig.storyTextureSourcePath;

	private Dictionary< string, object > storyScrollerArgsDic = new Dictionary< string, object >();

	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		InitUI();
		base.Init (config, origin);

	}

	public override void ShowUI () {
		base.ShowUI ();
		//ShowTweenPostion(0.2f);
		//SetUIActive(true);
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
		//ShowTweenPostion();
		base.HideUI ();

		//SetUIActive(false);
		
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

	private void InitUI() {
		CreateStoryScroller();
		//CreateEventScroller();
		gameObject.transform.localPosition = Vector3.zero;
	}
	


	void CreateStoryScroller() {
		string scrollerItemPath = "Quest/QuestScrollerItem";
		scrollerItem = Resources.Load( scrollerItemPath ) as GameObject;

		storyScroller = new DragPanel ( "StoryScroller", scrollerItem );
		storyScroller.CreatUI ();
		InitStoryScrollArgs();

		AddStoryQuestItem( storyTextureSourcePath );
		storyScroller.RootObject.SetScrollView( storyScrollerArgsDic );
	}

	void CreateEventScroller() {
		eventScroller = new DragPanel ("EventScroller", scrollerItem);
		eventScroller.CreatUI();
		eventScroller.AddItem (10);
		eventScroller.RootObject.SetItemWidth(230);
		
		eventScroller.RootObject.gameObject.transform.parent = this.gameObject.transform.FindChild( "event_window" );
		eventScroller.RootObject.gameObject.transform.localScale = Vector3.one;
		eventScroller.RootObject.gameObject.transform.localPosition = -140*Vector3.up;
	}


	private void TurnToQuest(GameObject go)
	{
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}

	private void ShowTweenPostion( float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear ) 
	{
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if( list == null )
			return;
		
		foreach( var tweenPos in list)
		{		
			if( tweenPos == null )
				continue;
			
			Vector3 temp;
			temp = tweenPos.to;
			tweenPos.to = tweenPos.from;
			tweenPos.from = temp;
			
			tweenPos.delay = mDelay;
			tweenPos.method = mMethod;
			
			tweenPos.Reset();
			tweenPos.PlayForward();

		}
	}

	private void AddStoryQuestItem( string path) {
		foreach (string textureName in TempConfig.storyQuestDic.Values) {
			storyScroller.AddScrollerItem( scrollerItem );
			UITexture uiTexture = scrollerItem.GetComponent< UITexture >();
			uiTexture.mainTexture = Resources.Load( path + textureName ) as Texture;
			//Debug.Log( uiTexture.mainTexture.name);
		}
	}

	private void InitStoryScrollArgs() {
		storyScrollerArgsDic.Add( "parentTrans", 			gameObject.transform       			);
		storyScrollerArgsDic.Add( "scrollerScale", 			Vector3.one								);
		storyScrollerArgsDic.Add( "scrollerLocalPos" ,	215*Vector3.up							);
		storyScrollerArgsDic.Add( "position", 				Vector3.zero 								);
		storyScrollerArgsDic.Add( "clipRange", 				new Vector4( 0, 0, 640, 200)			);
		storyScrollerArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		storyScrollerArgsDic.Add( "maxPerLine", 			1 												);
		storyScrollerArgsDic.Add( "scrollBarPosition", 	new Vector3(-320,-110,0)			);
		storyScrollerArgsDic.Add( "cellWidth", 				230 											);
		storyScrollerArgsDic.Add( "cellHeight",				150 											);

		//Debug.Log( "  storyScroller have finlished InitStoryScrollArgs() ");
	}
}
