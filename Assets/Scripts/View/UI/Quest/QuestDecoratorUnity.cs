using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestDecoratorUnity : UIComponentUnity {

	private DragPanel storyScroller;
	private DragPanel eventScroller;
	private GameObject scrollerItem;
	private GameObject storyWindow;
	private GameObject eventWindow;

	private string scrollerItemSourcePath = TempConfig.questItemSourcePath;
	private string storyTextureSourcePath = TempConfig.storyTextureSourcePath;
	private string eventTextureSourcePath = TempConfig.eventTextureSourcePath;

	private Dictionary< string, object > storyScrollerArgsDic = new Dictionary< string, object >();
	private Dictionary< string, object > eventScrollerArgsDic = new Dictionary< string, object >();
	public override void Init ( UIInsConfig config, IUIOrigin origin ) {
		InitUI();
		base.Init (config, origin);
	}

	public override void ShowUI () {
		base.ShowUI ();
		ShowTween();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI() {
		storyWindow = FindChild("story_window");
		eventWindow = FindChild("event_window");
		CreateStoryScroller();
		CreateEventScroller();
	}

	void CreateStoryScroller() {
		string scrollerItemPath = "Quest/QuestScrollerItem";
		scrollerItem = Resources.Load( scrollerItemPath ) as GameObject;

		storyScroller = new DragPanel ( "StoryScroller", scrollerItem );
		storyScroller.CreatUI ();
		InitStoryScrollArgs();

		AddStoryQuestItem( storyTextureSourcePath );
		storyScroller.RootObject.SetScrollView( storyScrollerArgsDic );

		for(int i = 0; i < storyScroller.ScrollItem.Count; i++)
			UIEventListener.Get(storyScroller.ScrollItem[ i ].gameObject).onClick = ChangeScene;
	}

	void CreateEventScroller() {
		string scrollerItemPath = "Quest/QuestScrollerItem";
		scrollerItem = Resources.Load( scrollerItemPath ) as GameObject;
		
		eventScroller = new DragPanel ( "EventScroller", scrollerItem );
		eventScroller.CreatUI ();
		InitEventScrollArgs();
		
		AddEventQuestItem( eventTextureSourcePath );
		eventScroller.RootObject.SetScrollView( eventScrollerArgsDic );

		for(int i = 0; i < eventScroller.ScrollItem.Count; i++)
			UIEventListener.Get(eventScroller.ScrollItem[ i ].gameObject).onClick = ChangeScene;
	}

	private void ChangeScene(GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
	}

	private void AddStoryQuestItem( string path) {
		foreach (string textureName in TempConfig.storyQuestDic.Values) {
			storyScroller.AddItem(6, scrollerItem );
			UITexture uiTexture = scrollerItem.GetComponent< UITexture >();
			uiTexture.mainTexture = Resources.Load( path + textureName ) as Texture;
		}
	}

	private void AddEventQuestItem( string path) {
		foreach (string textureName in TempConfig.eventQuestDic.Values) {
			eventScroller.AddItem(5, scrollerItem );
			UITexture uiTexture = scrollerItem.GetComponent< UITexture >();
			uiTexture.mainTexture = Resources.Load( path + textureName ) as Texture;
		}
	}

	
	private void ShowTween() {
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if( list == null )
			return;
		foreach( var tweenPos in list) {		
			if( tweenPos == null )
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	private void InitStoryScrollArgs() {
		storyScrollerArgsDic.Add( "parentTrans", 			storyWindow.transform       	);
		storyScrollerArgsDic.Add( "scrollerScale", 			Vector3.one				);
		storyScrollerArgsDic.Add( "scrollerLocalPos" ,		215*Vector3.up				);
		storyScrollerArgsDic.Add( "position", 				Vector3.zero 				);
		storyScrollerArgsDic.Add( "clipRange", 				new Vector4( 0, 0, 640, 200)		);
		storyScrollerArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		storyScrollerArgsDic.Add( "maxPerLine", 			0 						);
		storyScrollerArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)		);
		storyScrollerArgsDic.Add( "cellWidth", 				230 						);
		storyScrollerArgsDic.Add( "cellHeight",				150 						);
	}

	private void InitEventScrollArgs() {
		eventScrollerArgsDic.Add( "parentTrans", 			eventWindow.transform       	);
		eventScrollerArgsDic.Add( "scrollerScale", 			Vector3.one				);
		eventScrollerArgsDic.Add( "scrollerLocalPos" ,		-140*Vector3.up				);
		eventScrollerArgsDic.Add( "position", 				Vector3.zero 				);
		eventScrollerArgsDic.Add( "clipRange", 			new Vector4( 0, 0, 640, 200)		);
		eventScrollerArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		eventScrollerArgsDic.Add( "maxPerLine", 			0 						);
		eventScrollerArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)		);
		eventScrollerArgsDic.Add( "cellWidth", 				230 						);
		eventScrollerArgsDic.Add( "cellHeight",				150 						);
	}

}

public class ConfigStage{
	public ConfigStage(){
		Config();
	}

	void Config(){
		Debug.Log("Start to Config the data of stage");

		StageInfo stageItem = new StageInfo();
		stageItem.version = 1;
		stageItem.id = 1;
		stageItem.state = EQuestState.QS_NEW;
		stageItem.stageName = "第一监狱";
		stageItem.description = "This is the description of stage.Here many interesting infomation could be caught! Enjoy your game!";
		QuestInfo questItem = new QuestInfo();
		questItem.no = 1;
		questItem.state = EQuestState.QS_CLEARED;
		questItem.name = "第一小关卡";
		questItem.story = "This is the first level of the current stage";
		questItem.stamina = 8;
		questItem.floor = 1;
		questItem.rewardExp = 17;
		questItem.rewardCoin = 69;


	}
}
