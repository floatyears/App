using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestDecoratorUnity : UIComponentUnity {

	private DragPanel storyDragPanel;
	private DragPanel eventDragPanel;

	private GameObject storyWindow;
	private GameObject eventWindow;

	private string scrollerItemSourcePath = TempConfig.questItemSourcePath;
	private string storyTextureSourcePath = TempConfig.storyTextureSourcePath;
	private string eventTextureSourcePath = TempConfig.eventTextureSourcePath;

	private Dictionary< string, object > storyDragPanelArgsDic = new Dictionary< string, object >();
	private Dictionary< string, object > eventDragPanelArgsDic = new Dictionary< string, object >();
	
	List<StageInfo> storyStageInfoList = new List<StageInfo>();
	List<StageInfo> eventStageInfoList = new List<StageInfo>();

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


	void InitUI() {
		storyWindow = FindChild("story_window");
		eventWindow = FindChild("event_window");

		storyStageInfoList= GetStageInfo( 0 );
		storyDragPanel = CreateDragPanel(storyStageInfoList.Count);
		FillDragPanel(storyDragPanel, storyStageInfoList);
		InitStoryPanelArgs();
		storyDragPanel.RootObject.SetScrollView(storyDragPanelArgsDic);

		eventStageInfoList = GetStageInfo( 1 );
		eventDragPanel = CreateDragPanel( eventStageInfoList.Count );
		FillDragPanel( eventDragPanel, eventStageInfoList );
		InitEventPanelArgs();
		eventDragPanel.RootObject.SetScrollView(eventDragPanelArgsDic);
	}

	DragPanel CreateDragPanel(int count){
		GameObject scrollItem = GetScrollItem(UIConfig.stageDragPanelItemPath);
		DragPanel dragPanel = new DragPanel("StageDragPanel", scrollItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(count);
		return dragPanel;
	}

	void FillDragPanel(DragPanel dragPanel, List<StageInfo> infoList){
		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			ShowItemInfo( scrollItem, infoList[i]);
		}
	}

	void ShowItemInfo(GameObject item, StageInfo stageInfo){
		string textureSourcePath = string.Format("Stage/stage_{0}",stageInfo.id);
		//Debug.Log(string.Format("textureSourcePath : {0}", textureSourcePath));
		UITexture texture = item.transform.FindChild("Texture").GetComponent<UITexture>();
		texture.mainTexture = Resources.Load( textureSourcePath ) as Texture2D;
		switch (stageInfo.type){
			case 0 : 
				UIEventListener.Get( item.gameObject ).onClick = ClickStoryStage;
				break;
			case 1:
				UIEventListener.Get( item.gameObject ).onClick = ClickEventStage;
				break;
			default:
				break;
		}
	}

	List<StageInfo> GetStageInfo( int stageType){
		List<StageInfo> srcStageInfo = ConfigStage.stageList;
		List<StageInfo> desStageInfo = new List<StageInfo>();
		foreach ( StageInfo info in srcStageInfo){
			if( info.type != stageType )	continue;
			desStageInfo.Add(info);
		}
		//Debug.Log("StageType == " + stageType + " have " + desStageInfo.Count);
		return desStageInfo;
	}

	GameObject GetScrollItem( string resourcePath ){
		GameObject scrollItem;
		scrollItem = Resources.Load( resourcePath ) as GameObject;
		return scrollItem;
	}

	void ClickStoryStage(GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		if(!storyDragPanel.ScrollItem.Contains(go))	return;
		int index = storyDragPanel.ScrollItem.IndexOf(go);
//		Debug.Log("Click Story Stage Scroll Item's IndexOf is : " + index + " time : " + Time.realtimeSinceStartup);	
		StageInfo curSelectStage = storyStageInfoList[ index ];
		MsgCenter.Instance.Invoke(CommandEnum.TransmitStageInfo, curSelectStage);
//		DeDebug.Log("Click Story Stage Scroll Item's IndexOf is : " + index + " time : " + Time.realtimeSinceStartup);	
	}

	void ClickEventStage(GameObject go) {
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		UIManager.Instance.ChangeScene(SceneEnum.QuestSelect);
		if(!eventDragPanel.ScrollItem.Contains(go))	return;
		int index = eventDragPanel.ScrollItem.IndexOf(go);
		Debug.Log("Click Event Stage Scroll Item's IndexOf is : " + index);
		StageInfo curSelectStage = eventStageInfoList[ index ];
		MsgCenter.Instance.Invoke(CommandEnum.TransmitStageInfo, curSelectStage);
		//Debug.LogError(curSelectStage.quests.Count);
	}

	void ShowTween() {
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
	
	void InitStoryPanelArgs() {
		storyDragPanelArgsDic.Add( "parentTrans", 			storyWindow.transform		);
		storyDragPanelArgsDic.Add( "scrollerScale", 			Vector3.one				);
		storyDragPanelArgsDic.Add( "scrollerLocalPos" ,		215*Vector3.up				);
		storyDragPanelArgsDic.Add( "position", 			Vector3.zero				);
		storyDragPanelArgsDic.Add( "clipRange", 			new Vector4( 0, 0, 640, 200)		);
		storyDragPanelArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		storyDragPanelArgsDic.Add( "maxPerLine", 			0 						);
		storyDragPanelArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)		);
		storyDragPanelArgsDic.Add( "cellWidth", 			230 						);
		storyDragPanelArgsDic.Add( "cellHeight",			150 						);
	}

	void InitEventPanelArgs() {
		eventDragPanelArgsDic.Add( "parentTrans", 			eventWindow.transform       	);
		eventDragPanelArgsDic.Add( "scrollerScale", 		Vector3.one				);
		eventDragPanelArgsDic.Add( "scrollerLocalPos" ,		-140*Vector3.up				);
		eventDragPanelArgsDic.Add( "position", 			Vector3.zero 				);
		eventDragPanelArgsDic.Add( "clipRange", 			new Vector4( 0, 0, 640, 200)		);
		eventDragPanelArgsDic.Add( "gridArrange", 			UIGrid.Arrangement.Horizontal 	);
		eventDragPanelArgsDic.Add( "maxPerLine", 			0 						);
		eventDragPanelArgsDic.Add( "scrollBarPosition", 		new Vector3(-320,-120,0)		);
		eventDragPanelArgsDic.Add( "cellWidth", 			230 						);
		eventDragPanelArgsDic.Add( "cellHeight",			150 						);
	}

}

public class ConfigStage{
	public static List<StageInfo> stageList = new List<StageInfo>();
	public ConfigStage(){
		Config();
	}

	void Config(){
		//Debug.Log("Start to Config the data of stage");

		//-----------------------------------------------------------------------
		StageInfo storyStageItem = new StageInfo();
		storyStageItem.version = 1;
		storyStageItem.id = 1;
		storyStageItem.type = 0;
		storyStageItem.state = EQuestState.QS_CLEARED;
		storyStageItem.stageName = "第一监狱";
		storyStageItem.description = "This is the description of stage.Here many interesting infomation could be caught! Enjoy your game!";
			QuestInfo storyQuestItem = new QuestInfo();
			storyQuestItem.id = 991;
			storyQuestItem.no = 1;
			storyQuestItem.state = EQuestState.QS_CLEARED;
			storyQuestItem.name = "第一小关卡";
			storyQuestItem.story = "This is the first level of the current stage";
			storyQuestItem.stamina = 8;
			storyQuestItem.floor = 1;
			storyQuestItem.rewardExp = 17;
			storyQuestItem.rewardCoin = 69;
			List<uint> storyEnemyList = new List<uint>(){1,3,5,7,9,11,13,15};
			storyQuestItem.enemyId.AddRange(storyEnemyList);
		storyStageItem.quests.Add(storyQuestItem);

			storyQuestItem = new QuestInfo();
			storyQuestItem.id = 992;
			storyQuestItem.no = 2;
			storyQuestItem.state = EQuestState.QS_CLEARED;
			storyQuestItem.name = "第二小关卡";
			storyQuestItem.story = "This is the first level of the current stage";
			storyQuestItem.stamina = 9;
			storyQuestItem.floor = 2;
			storyQuestItem.rewardExp = 25;
			storyQuestItem.rewardCoin = 74;
			storyEnemyList = new List<uint>(){2,4,6,8,10,12,14,16};
			storyQuestItem.enemyId.AddRange(storyEnemyList);
		storyStageItem.quests.Add(storyQuestItem);

			storyQuestItem = new QuestInfo();
			storyQuestItem.id = 993;
			storyQuestItem.no = 3;
			storyQuestItem.state = EQuestState.QS_NEW;
			storyQuestItem.name = "第三小关卡";
			storyQuestItem.story = "This is the first level of the current stage";
			storyQuestItem.stamina = 15;
			storyQuestItem.floor = 8;
			storyQuestItem.rewardExp = 125;
			storyQuestItem.rewardCoin = 287;
			storyEnemyList = new List<uint>(){2,4,6,8,10,12,14,16};
			storyQuestItem.enemyId.AddRange(storyEnemyList);
		storyStageItem.quests.Add(storyQuestItem);

		stageList.Add(storyStageItem);

		//-----------------------------------------------------------------------


		//-----------------------------------------------------------------------
		StageInfo eventStageItem = new StageInfo();
		eventStageItem.version = 1;
		eventStageItem.id = 2;
		eventStageItem.type = 1;
		eventStageItem.state = EQuestState.QS_CLEARED;
		eventStageItem.stageName = "第二监狱";
		eventStageItem.description = "This is the description of stage.Here many interesting infomation could be caught! Enjoy your game!";
			QuestInfo eventQuestItem = new QuestInfo();
			eventQuestItem.id = 991;
			eventQuestItem.no = 1;
			eventQuestItem.state = EQuestState.QS_CLEARED;
			eventQuestItem.name = "第一小关卡";
			eventQuestItem.story = "This is the first level of the current stage";
			eventQuestItem.stamina = 8;
			eventQuestItem.floor = 1;
			eventQuestItem.rewardExp = 17;
			eventQuestItem.rewardCoin = 69;
			List<uint> eventEnemyIdList = new List<uint>(){1,3,5,7,9,11,13,15};
			eventQuestItem.enemyId.AddRange(storyEnemyList);
		eventStageItem.quests.Add(eventQuestItem);
			
			eventQuestItem = new QuestInfo();
			eventQuestItem.id = 992;
			eventQuestItem.no = 2;
			eventQuestItem.state = EQuestState.QS_CLEARED;
			eventQuestItem.name = "第二小关卡";
			eventQuestItem.story = "This is the first level of the current stage";
			eventQuestItem.stamina = 9;
			eventQuestItem.floor = 2;
			eventQuestItem.rewardExp = 25;
			eventQuestItem.rewardCoin = 74;
			eventEnemyIdList = new List<uint>(){2,4,6,8,10,12,14,16};
			eventQuestItem.enemyId.AddRange(storyEnemyList);
		eventStageItem.quests.Add(eventQuestItem);
			
			eventQuestItem = new QuestInfo();
			eventQuestItem.id = 993;
			eventQuestItem.no = 3;
			eventQuestItem.state = EQuestState.QS_NEW;
			eventQuestItem.name = "第三小关卡";
			eventQuestItem.story = "This is the first level of the current stage";
			eventQuestItem.stamina = 15;
			eventQuestItem.floor = 8;
			eventQuestItem.rewardExp = 125;
			eventQuestItem.rewardCoin = 287;
			eventEnemyIdList = new List<uint>(){2,4,6,8,10,12,14,16};
			eventQuestItem.enemyId.AddRange(storyEnemyList);
		eventStageItem.quests.Add(eventQuestItem);
		
		stageList.Add(eventStageItem);
		//Debug.LogError("ConfigStage List Count: " + stageList.Count);
		//---------------------------
	}
}
