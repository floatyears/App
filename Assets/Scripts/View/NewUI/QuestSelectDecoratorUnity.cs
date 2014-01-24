using UnityEngine;
using System.Collections.Generic;

public class QuestSelectDecoratorUnity : UIComponentUnity ,IUICallback{

	public static UIImageButton btnSelect;
	IUICallback iuiCallback;
	bool temp = false;
	private UILabel labDoorName;
	private UILabel labDoorType;
	private UILabel labFloorVaule;
	private UILabel labStaminaVaule;
	private UILabel labStoryContent;
	private UILabel labQuestInfo;
	private UILabel labRewardInfo;

	private GameObject contentDetail;
	private GameObject contentStory;

	private GameObject detail_low_light;
	private GameObject story_low_light;

	private DragPanel questSelectScroller;
	private GameObject scrollerItem;
	private GameObject scrollView;

	private string scrollerItemSourcePath = TempConfig.questItemSourcePath;
	private string questSelectTextureSourcePath = TempConfig.storyTextureSourcePath;

	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	public override void Init (UIInsConfig config, IUIOrigin origin) {

		base.Init (config, origin);
		temp = origin is IUICallback;
		InitUI();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		ShowTweenPostion(0.2f);
		btnSelect.isEnabled = false;
		SetUIActive(true);
	}
	
	public override void HideUI () {
		base.HideUI ();
		ShowTweenPostion();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	private void InitUI(){

		scrollView = FindChild("ScrollView");
		btnSelect = FindChild<UIImageButton>("ScrollView/btn_quest_select"); 

		labDoorName = FindChild( "Window/title/Label_door_name").GetComponent< UILabel>();
		labDoorName.text = string.Empty;
		labDoorType = FindChild("Window/title/Label_door_type_name" ).GetComponent< UILabel >();
		labDoorType.text = string.Empty;

		labFloorVaule = FindChild("Window/window_left/Label_floor_V").GetComponent< UILabel >();
		labFloorVaule.text = string.Empty;

		labStaminaVaule = FindChild("Window/window_left/Label_stamina_V").GetComponent< UILabel >();
		labStaminaVaule.text = string.Empty;

		labStoryContent = FindChild("Window/window_right/content_story/Label_story").GetComponent< UILabel >();
		labStoryContent.text = string.Empty;

		labQuestInfo = FindChild("Window/window_right/content_detail/Label_quest_info").GetComponent< UILabel >();
		labQuestInfo.text = string.Empty;

		labRewardInfo = FindChild("Window/window_right/content_detail/Label_reward_info").GetComponent< UILabel >();
		labRewardInfo.text = string.Empty;

		UIEventListener.Get( btnSelect.gameObject ).onClick = ChangeScene;
		CreateScroller();

	}

	private void AddStoryQuestItem( string path) {
		foreach (string textureName in TempConfig.storyQuestDic.Values) {
			questSelectScroller.AddItem(6, scrollerItem );
			UITexture uiTexture = scrollerItem.GetComponent< UITexture >();
			uiTexture.mainTexture = Resources.Load( path + textureName ) as Texture;
			//Debug.Log( uiTexture.mainTexture.name);
		}
	}

	private void CreateScroller() {
		string scrollerItemPath = "Quest/QuestScrollerItem";
		scrollerItem = Resources.Load( scrollerItemPath ) as GameObject;
		
		questSelectScroller = new DragPanel ( "QuestSelectScroller", scrollerItem );
		questSelectScroller.CreatUI ();
		InitQuestSelectScrollArgs();
		
		AddStoryQuestItem( questSelectTextureSourcePath );
		questSelectScroller.RootObject.SetScrollView( questSelectScrollerArgsDic );
		
		for(int i = 0; i < questSelectScroller.ScrollItem.Count; i++)
			UIEventListener.Get(questSelectScroller.ScrollItem[ i ].gameObject).onClick = PickQuestInfo;
	}

	private void ChangeScene( GameObject btn ) {
		UIManager.Instance.ChangeScene( SceneEnum.FriendSelect );
	}
	
	public void Callback (object data) {
		bool b = (bool)data;
		btnSelect.isEnabled = b;
	}

	private void SetUIActive(bool b) {
		questSelectScroller.RootObject.gameObject.SetActive(b);
	}

	private void PickQuestInfo(GameObject go) {
		btnSelect.isEnabled = true;
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

	private void InitQuestSelectScrollArgs() {
		questSelectScrollerArgsDic.Add( "parentTrans", 				scrollView.transform       		);
		questSelectScrollerArgsDic.Add( "scrollerScale", 			Vector3.one								);
		questSelectScrollerArgsDic.Add( "scrollerLocalPos" ,		-96*Vector3.up							);
		questSelectScrollerArgsDic.Add( "position", 					Vector3.zero 								);
		questSelectScrollerArgsDic.Add( "clipRange", 				new Vector4( 0, 0, 640, 200)			);
		questSelectScrollerArgsDic.Add( "gridArrange", 				UIGrid.Arrangement.Horizontal 	);
		questSelectScrollerArgsDic.Add( "maxPerLine", 				0 												);
		questSelectScrollerArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)			);
		questSelectScrollerArgsDic.Add( "cellWidth", 				230 											);
		questSelectScrollerArgsDic.Add( "cellHeight",				150 											);
		
		//Debug.Log( "  storyScroller have finlished InitStoryScrollArgs() ");
	}
}
