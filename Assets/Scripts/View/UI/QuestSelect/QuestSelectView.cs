using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestSelectView : UIComponentUnity{
	private StageInfo stageInfo;
	private UIImageButton selectBtn;

	private UILabel labDoorName;
	private UILabel labDoorType;
	private UILabel labFloorVaule;
	private UILabel labStaminaVaule;
	private UILabel labStoryContent;
	private UILabel labQuestInfo;

	private UILabel storyTextLabel;
	private UILabel rewardExpLabel;
	private UILabel rewardCoinLabel;
	private UILabel rewardLineLabel;
	private UILabel questNameLabel;
	private UITexture bossAvatar;
	private GameObject detail_low_light;
	private GameObject story_low_light;
	private UILabel clearLabel;
	private DragPanel dragPanel;
	private GameObject scrollerItem;
	private GameObject scrollView;

	private GameObject questViewItem;

	private List<UITexture> pickEnemiesList = new List<UITexture>();
	private UIToggle firstFocus;
	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	private List<QuestInfo> questInfoList = new List<QuestInfo>();


	private TStageInfo curStageInfo;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
		questViewItem = Resources.Load("Prefabs/UI/Quest/QuestItem") as GameObject;
	}
	
	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedStage, SelectedStage);
        firstFocus.value = true;
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedStage, SelectedStage);
	}

	void SelectedStage(object data) {
		curStageInfo = data as TStageInfo;
		CreateQuestDragList(curStageInfo);
	}

    public override void ResetUIState(){
        LogHelper.Log("QuestSelectDecoratorUnity.ClearUIState()");
        CleanQuestInfo();
        if (dragPanel != null){
            dragPanel.DestoryUI();
        }
        selectBtn.isEnabled = false;
//        InitDragPanel();
    }   

//	void ReceiveStageInfo( object data ){
//		StageInfo receivedStageInfo = data as StageInfo;
//		stageInfo = receivedStageInfo;
//		questInfoList = stageInfo.quests;
//		Debug.LogError("questInfoList : " + questInfoList.Count);
////		InitDragPanel();
//	}

	void InitUI(){
		firstFocus = FindChild<UIToggle>("Window/window_right/tab_detail");
		scrollView = FindChild("ScrollView");
		selectBtn = FindChild<UIImageButton>("ScrollView/btn_quest_select"); 
		labDoorName = FindChild< UILabel >("Window/title/Label_door_name");
		labDoorName.text = string.Empty;
		labDoorType = FindChild< UILabel >("Window/title/Label_door_type_name");
		labDoorType.text = string.Empty;
		labFloorVaule = FindChild< UILabel >("Window/window_left/Label_floor_V");
		labFloorVaule.text = string.Empty;
		labStaminaVaule = FindChild< UILabel >("Window/window_left/Label_stamina_V");
		labStaminaVaule.text = string.Empty;
		labStoryContent = FindChild< UILabel >("Window/window_right/content_story/Label_story");
		labStoryContent.text = string.Empty;
		labQuestInfo = FindChild< UILabel >("Window/window_right/content_detail/Label_quest_info");
		storyTextLabel = FindChild<UILabel>("Window/window_right/content_story/Label_story");
		storyTextLabel.text = string.Empty;
		rewardLineLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Line");
		rewardLineLabel.text = string.Empty;
		rewardExpLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Exp");
		rewardExpLabel.text = string.Empty;
		rewardCoinLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Coin");
		rewardCoinLabel.text= string.Empty;
		questNameLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_quest_name");
		questNameLabel.text = string.Empty;
		bossAvatar = FindChild<UITexture>("Window/window_left/Texture_Avatar");

		GameObject pickEnemies;
		pickEnemies = FindChild("Window/window_right/content_detail/pickEnemies");
		UITexture[] texs = pickEnemies.GetComponentsInChildren<UITexture>();
		foreach (var item in texs){
			pickEnemiesList.Add(item);
		} 

		UIEventListener.Get(selectBtn.gameObject).onClick = ClickFriendSelect;
	}

//	void InitDragPanel(){
//		if(dragPanel != null){
//			return ;
//		}
//		dragPanel = CreateDragPanel(questInfoList.Count);
//		Debug.LogError("questInfoList.Count : " + questInfoList.Count);
//		FillDragPanel(dragPanel, questInfoList);
//		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.QuestSelectDragPanelArgs, scrollView.transform);
//	}

	GameObject GetScrollItem( string resourcePath ){
		GameObject scrollItem;
		scrollItem = Resources.Load( resourcePath ) as GameObject;
		return scrollItem;
	}

//	DragPanel CreateDragPanel(int count){
//		GameObject scrollItem = GetScrollItem(UIConfig.questDragPanelItemPath);
//		if( scrollItem == null)
//			Debug.LogError("Not Find The Scroll Item");
//
//		DragPanel dragPanel = new DragPanel("QuestDragPanel", scrollItem);
//		dragPanel.CreatUI();
//		dragPanel.AddItem(count);
//		return dragPanel;
//	}

//	void FillDragPanel(DragPanel dragPanel, List<QuestInfo> infoList){
//		Debug.LogError("dragPanel count : " + dragPanel.ScrollItem.Count);
//		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			GameObject scrollItem = dragPanel.ScrollItem[ i ];
//			ShowItemInfo( scrollItem, infoList[ i ]);
//			Debug.LogError( i );
//		}
//	}

//	void ShowItemInfo(GameObject item, QuestInfo questInfo){
//		string textureSourcePath = string.Format("Avatar/{0}_1",questInfo.no);
//		UITexture texture = item.transform.FindChild("Texture_Quest").GetComponent<UITexture>();
//		texture.mainTexture = Resources.Load( textureSourcePath ) as Texture2D;
//
//		UILabel clearFlagLabel = item.transform.FindChild("Label_Clear_Mark").GetComponent<UILabel>();
//		switch (questInfo.state){
//			case EQuestState.QS_CLEARED : 
//				clearFlagLabel.text = "Clear";
//				clearFlagLabel.color = Color.yellow;
//				Debug.LogError("clearFlagLabel.text : " + clearFlagLabel.text);
//				break;
//			case EQuestState.QS_NEW :
//				clearFlagLabel.text = "New";
//				clearFlagLabel.color = Color.green;
//				Debug.LogError("clearFlagLabel.text : " + clearFlagLabel.text);
//				break;
//			default:
//				break;
//		}
//
//		UILabel questNoLabel = item.transform.FindChild("Label_Quest_NO").GetComponent<UILabel>();
//		questNoLabel.text = string.Format("Quest : {0}", questInfo.no);
//
//		UIEventListener.Get( item).onClick = ClickQuestItem;
//	}

	void UpdatePanelInfo(object args){
		Dictionary<string,object> info = args as Dictionary<string, object>;
		int index = (int)info["position"];
		TStageInfo tsi = info["data"] as TStageInfo;
		TQuestInfo select =  tsi.QuestInfo [index];
		DataCenter.Instance.currentQuestInfo = select;	//store select quest 
		labStaminaVaule.text = select.Stamina.ToString();
		labFloorVaule.text = select.Floor.ToString();
		labDoorName.text = tsi.StageName;
		labStoryContent.text = select.Story;
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = "Cion " + select.RewardMoney.ToString();
		labQuestInfo.text = select.Name;
		rewardExpLabel.text = "Exp " + select.RewardExp.ToString();
		storyTextLabel.text = tsi.Description;
		selectBtn.isEnabled = true;

		ShowBossAvatar(select.BossID[ 0 ]);
		ShowEnemiesAvatar(select.EnemyID);
	}


	void ShowBossAvatar(uint bossId){
		TUnitInfo bossInfo = DataCenter.Instance.GetUnitInfo(bossId);
		if(bossInfo == null){
			Debug.LogError("Boss Info is null!!!");
			return;
		}
		Texture2D sourceTex = bossInfo.GetAsset(UnitAssetType.Avatar);
		if(sourceTex == null){
			Debug.LogError("Source Texture NOT found!!!");
			return;
		}
		bossAvatar.mainTexture = sourceTex;
	}

	int maxEnemyShowCount = 5;
	void ShowEnemiesAvatar(List<uint> enemyIdList){
		List<TUnitInfo> enemyInfoList = new List<TUnitInfo>();
		for (int i = 0; i < enemyIdList.Count; i++){
			if(i >= maxEnemyShowCount) break;
			TUnitInfo tui = DataCenter.Instance.GetUnitInfo(enemyIdList[ i ]);
			if(tui == null)
				return;
			else
				enemyInfoList.Add(tui);
		}

		List<Texture2D> texList = new List<Texture2D>();
		for (int i = 0; i < enemyInfoList.Count; i++){
			Texture2D tex = enemyInfoList[ i ].GetAsset(UnitAssetType.Avatar);
			if(tex == null) return;
			else
				texList.Add(tex);
		}

		for (int i = 0; i < texList.Count; i++){
//			Debug.LogError(i);
			pickEnemiesList[ i ].mainTexture = texList[ i ];
		}
	}

	private void ClickQuestItem(QuestItem item){
		Debug.Log("ClickQuestItem : " + item.name);
		if (DataCenter.gameStage == GameState.Evolve) {
			return;	
		}
		item.IsFocus = true;
		int index = dragPanel.ScrollItem.IndexOf( item.gameObject );
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickQuestItem", index);
		ExcuteCallback(cbdArgs);
		//show info panel data
		TQuestInfo selectQuest =  item.Data;
		DataCenter.Instance.currentQuestInfo = selectQuest;	//store select quest 
		labStaminaVaule.text = selectQuest.Stamina.ToString();
		labFloorVaule.text = selectQuest.Floor.ToString();
//		labDoorName.text = tsi.StageName;
		labStoryContent.text = selectQuest.Story;
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = "Cion " + selectQuest.RewardMoney.ToString();
		labQuestInfo.text = selectQuest.Name;
		rewardExpLabel.text = "Exp " + selectQuest.RewardExp.ToString();
//		storyTextLabel.text = tsi.Description;
		selectBtn.isEnabled = true;
		
		ShowBossAvatar(selectQuest.BossID[ 0 ]);
		ShowEnemiesAvatar(selectQuest.EnemyID);
	}

	void ClickQuestItem(GameObject go ){
		if (DataCenter.gameStage == GameState.Evolve) {
			return;	
		}
		int index = dragPanel.ScrollItem.IndexOf( go );
		LightClickItem(index);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickQuestItem", index);
		ExcuteCallback(cbdArgs);
	}

	UISprite prevSprite;
	void LightClickItem(int position){
		GameObject pickedItem = dragPanel.ScrollItem[ position ];
		UISprite lightSpr = pickedItem.transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		if(lightSpr == null) {
			Debug.LogError("lightSpr is null");
			return;
		}
		if(prevSprite != null) {
			if(lightSpr.Equals( prevSprite))
				return;
			else
				prevSprite.enabled = false;
		}

		lightSpr.enabled = true;
		prevSprite = lightSpr;
	}

	void ClickFriendSelect(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickFriendSelect", (DataCenter.gameStage == GameState.Evolve));
		ExcuteCallback(cbdArgs);
	}
    
	public override void CallbackView(object data) {
		base.CallbackView(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
//			case "ShowInfoPanel" : 
//				CallBackDispatcherHelper.DispatchCallBack(UpdatePanelInfo, cbdArgs);
//				break;
			case "EvolveQuestList":
				CallBackDispatcherHelper.DispatchCallBack(EvolveInfoShow, cbdArgs);
				break;
			default:
				break;
		}
	}

	void EvolveInfoShow (object args) {
		TStageInfo tsi = args as TStageInfo;
		if (dragPanel != null) {
			dragPanel.DestoryUI ();
		} 
		else {
			dragPanel = new DragPanel("QuestDragPanel",questViewItem);
			dragPanel.CreatUI();
			dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.QuestSelectDragPanelArgs, scrollView.transform);
		}
		dragPanel.AddItem (tsi.QuestInfo.Count);
		RefreshQuestInfo (tsi.QuestInfo);
		Dictionary<string, object> tempDic = new Dictionary<string, object> ();
		int inedx = tsi.QuestInfo.FindIndex (a => a.ID == tsi.QuestId);
		tempDic.Add ("position", inedx);
		tempDic.Add ("data", tsi);
		UpdatePanelInfo (tempDic);
		selectBtn.isEnabled = true;
	}
	
	void CreateQuestDragList(object args){
		TStageInfo tsi = args as TStageInfo;
		List<TQuestInfo> infoListForShow = GetQuestInfoListForShow(tsi);
		if(infoListForShow == null){
			Debug.LogError("GetQuestInfoListForShow(), Data is Error, return!!!");
			return;
		}
		else{
			Debug.Log("GetQuestInfoListForShow(), infoListForShow count is : " + infoListForShow.Count);
			dragPanel = new DragPanel("QuestDragPanel", QuestItem.ItemPrefab);
			dragPanel.CreatUI();
			dragPanel.AddItem(infoListForShow.Count);
			dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.QuestSelectDragPanelArgs, scrollView.transform);

			for (int i = 0; i < infoListForShow.Count; i++){
				//Inject data
				GameObject scrollItem = dragPanel.ScrollItem[ i ];
				QuestItem questItem = QuestItem.Inject(scrollItem);
				if(infoListForShow[ i ] == null){
					Debug.LogError(string.Format("infoListForShow{[0]} data is NULL, return!!!", i));
					return;
				}
				else{
					questItem.Data = infoListForShow[ i ] ;
					questItem.Position = i;
					questItem.IsFocus = false;
					questItem.callback = ClickQuestItem;
				}
			}
		}
	}
	
	private List<TQuestInfo> GetQuestInfoListForShow(TStageInfo stageInfo){
		List<TQuestInfo> infoListForShow = new List<TQuestInfo>();
		List<TQuestInfo> questInfoList = stageInfo.QuestInfo;

		for (int i = 0; i < questInfoList.Count; i++){
			if(questInfoList[ i ].state == EQuestState.QS_CLEARED){
				//add the one as long as it is on state of clear
				infoListForShow.Add(questInfoList[ i ]);
			}
			else if(questInfoList[ i ].state == EQuestState.QS_NEW){
				//add the first one that state is new only
				infoListForShow.Add(questInfoList[ i ]);
				return infoListForShow;
			}
		}
		return infoListForShow;
	}

	void RefreshQuestInfo(List<TQuestInfo> questInfo) {
		if (dragPanel == null) {
			return;	
		}
		for (int i = 0, m = dragPanel.ScrollItem.Count; i < m; i++) {
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture tex = scrollItem.transform.FindChild("Texture_Quest").GetComponent<UITexture>();
			TQuestInfo tqi = questInfo[ i ];

			UILabel label = scrollItem.transform.FindChild("Label_Quest_NO").GetComponent<UILabel>();
			label.text = "Quest : " + (i + 1).ToString();
			UIEventListener.Get(scrollItem.gameObject).onClick = ClickQuestItem;
			TUnitInfo tui = DataCenter.Instance.GetUnitInfo(tqi.BossID[ 0 ]);
			if(tui != null) {
				tex.mainTexture = tui.GetAsset(UnitAssetType.Avatar);
			}
			UILabel clearFlagLabel = scrollItem.transform.FindChild("Label_Clear_Mark").GetComponent<UILabel>();
			switch (tqi.state){
				case EQuestState.QS_CLEARED : 
					clearFlagLabel.text = "Clear";
					clearFlagLabel.color = Color.yellow;
					Debug.LogError("clearFlagLabel.text : " + clearFlagLabel.text);
					break;
				case EQuestState.QS_NEW :
					clearFlagLabel.text = "New";
                    clearFlagLabel.color = Color.green;
                    Debug.LogError("clearFlagLabel.text : " + clearFlagLabel.text);
                    break;
                default:
                    break;
			}
		}
	}

	void CleanQuestInfo(){
		labStaminaVaule.text = string.Empty;
        labFloorVaule.text = string.Empty;
		labQuestInfo.text = string.Empty;
		storyTextLabel.text = string.Empty;
		rewardLineLabel.text = string.Empty;
		rewardExpLabel.text = string.Empty;
		rewardCoinLabel.text = string.Empty;
		questNameLabel.text = string.Empty;
		questNameLabel.text = string.Empty;
		bossAvatar.mainTexture = null;
		labDoorName.text = string.Empty;
		foreach (var item in pickEnemiesList){
			item.mainTexture = null;
		}
	}

	void ShowTween(){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null) continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

	public void ShowInfoPanelContent(TQuestInfo questInfo){

	}

}