using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestSelectDecoratorUnity : UIComponentUnity{
	StageInfo stageInfo;
	UIImageButton btnSelect;

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
	private UITexture avatarTexture;
	private GameObject detail_low_light;
	private GameObject story_low_light;
	private UILabel clearLabel;
	private DragPanel dragPanel;
	private GameObject scrollerItem;
	private GameObject scrollView;

	GameObject questViewItem;
//	private bool isEvolve = false;


	List<UITexture> pickEnemiesList = new List<UITexture>();
	UIToggle firstFocus;
	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	List<QuestInfo> questInfoList = new List<QuestInfo>();

	public override void Init(UIInsConfig config, IUICallback origin){
//		Debug.LogError("QuestSelectDecoratorUnity init start");
		base.Init(config, origin);
		InitUI();
		InitQuestSelectScrollArgs();
		questViewItem = Resources.Load("Prefabs/UI/Quest/QuestItem") as GameObject;
//		Debug.LogError("QuestSelectDecoratorUnity init end");
	}
	
	public override void ShowUI(){
//		Debug.LogError("QuestSelectDecoratorUnity ShowUI start");
		base.ShowUI();
		ShowTween();
		btnSelect.isEnabled = false;

		firstFocus.value = true;
//		Debug.LogError("QuestSelectDecoratorUnity ShowUI end");
	}

	public override void HideUI(){
		base.HideUI();
		CleanQuestInfo();
		dragPanel.DestoryUI();
	}
	
	void ReceiveStageInfo( object data ){
		StageInfo receivedStageInfo = data as StageInfo;
		stageInfo = receivedStageInfo;
		questInfoList = stageInfo.quests;
		InitDragPanel();
	}

	void InitUI(){
		firstFocus = FindChild<UIToggle>("Window/window_right/tab_detail");
		scrollView = FindChild("ScrollView");
		btnSelect = FindChild<UIImageButton>("ScrollView/btn_quest_select"); 
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
		avatarTexture = FindChild<UITexture>("Window/window_left/Texture_Avatar");

		GameObject pickEnemies;
		pickEnemies = FindChild("Window/window_right/content_detail/pickEnemies");
		UITexture[] texs = pickEnemies.GetComponentsInChildren<UITexture>();
		foreach (var item in texs){
			pickEnemiesList.Add(item);
		} 

		UIEventListener.Get(btnSelect.gameObject).onClick = ClickFriendSelect;
	}

	void InitDragPanel(){
		if(dragPanel != null){
			return ;
		}
		dragPanel = CreateDragPanel(questInfoList.Count);
		FillDragPanel(dragPanel, questInfoList);

		dragPanel.DragPanelView.SetScrollView(questSelectScrollerArgsDic);
	}

	GameObject GetScrollItem( string resourcePath ){
		GameObject scrollItem;
		scrollItem = Resources.Load( resourcePath ) as GameObject;
		return scrollItem;
	}

	DragPanel CreateDragPanel(int count){
		GameObject scrollItem = GetScrollItem(UIConfig.questDragPanelItemPath);
		if( scrollItem == null)
			Debug.LogError("Not Find The Scroll Item");

		DragPanel dragPanel = new DragPanel("QuestDragPanel", scrollItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(count);
		return dragPanel;
	}

	void FillDragPanel(DragPanel dragPanel, List<QuestInfo> infoList){
		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			ShowItemInfo( scrollItem, infoList[i]);
		}
	}

	void ShowItemInfo(GameObject item, QuestInfo questInfo){
		string textureSourcePath = string.Format("Avatar/{0}_1",questInfo.no);
		UITexture texture = item.transform.FindChild("Texture_Quest").GetComponent<UITexture>();
		texture.mainTexture = Resources.Load( textureSourcePath ) as Texture2D;

		UILabel clearFlagLabel = item.transform.FindChild("Label_Clear_Mark").GetComponent<UILabel>();
		switch (questInfo.state){
			case EQuestState.QS_CLEARED : 
				clearFlagLabel.text = "Clear";
				clearFlagLabel.color = Color.yellow;
				break;
			case EQuestState.QS_NEW :
				clearFlagLabel.text = "New";
				clearFlagLabel.color = Color.green;
				break;
			default:
				break;
		}

		UILabel questNoLabel = item.transform.FindChild("Label_Quest_NO").GetComponent<UILabel>();
		questNoLabel.text = string.Format("Quest : {0}", questInfo.no);

		UIEventListener.Get( item).onClick = ClickQuestItem;
	}

	void UpdatePanelInfo(object args){
		Dictionary<string,object> info = args as Dictionary<string, object>;
		int index = (int)info["position"];
		TStageInfo tsi = info["data"] as TStageInfo;

		labStaminaVaule.text = tsi.QuestInfo[index].Stamina.ToString();
		labFloorVaule.text = tsi.QuestInfo[index].Floor.ToString();
		labDoorName.text = tsi.StageName;
		labStoryContent.text = tsi.QuestInfo[index].Story;
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = "Cion " + tsi.QuestInfo[index].RewardMoney.ToString();
		labQuestInfo.text = tsi.QuestInfo[index].Name;
		rewardExpLabel.text = "Exp " + tsi.QuestInfo[index].RewardExp.ToString();
		storyTextLabel.text = tsi.Description;


		btnSelect.isEnabled = true;

	}

	void ClickQuestItem(GameObject go ){
		if (DataCenter.gameStage == GameState.Evolve) {
			return;	
		}
		int index = dragPanel.ScrollItem.IndexOf( go );
		LightClickItem(index);
//		Debug.LogError("ClickQuestItem(), click item pos : " + index);
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
//		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickFriendSelect", (DataCenter.gameStage == GameState.Evolve));
		ExcuteCallback(cbdArgs);
	}
	
	public override void Callback(object data) {
		base.Callback(data);

		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateQuestList" : 
				CallBackDispatcherHelper.DispatchCallBack(CreateQuestDragList, cbdArgs);
				break;
			case "ShowInfoPanel" : 
				CallBackDispatcherHelper.DispatchCallBack(UpdatePanelInfo, cbdArgs);
				break;
			case "EvolveQuestList":
				CallBackDispatcherHelper.DispatchCallBack(EvolveInfoShow, cbdArgs);
				break;
			default:
				break;
		}
	}

	void EvolveInfoShow (object args) {
		TStageInfo tsi = args as TStageInfo;
//		isEvolve = true;
		Debug.LogError ("EvolveInfoShow : ");
		if (dragPanel != null) {
			dragPanel.DestoryUI ();
		} 
		else {
			dragPanel = new DragPanel("QuestDragPanel",questViewItem);
			dragPanel.CreatUI();
			dragPanel.DragPanelView.SetScrollView(questSelectScrollerArgsDic);
		}

		Debug.LogError (tsi.QuestInfo.Count);

		dragPanel.AddItem (tsi.QuestInfo.Count);
		RefreshQuestInfo (tsi.QuestInfo);
		Dictionary<string, object> tempDic = new Dictionary<string, object> ();
		int inedx = tsi.QuestInfo.FindIndex (a => a.ID == tsi.QuestId);
		tempDic.Add ("position", inedx);
		tempDic.Add ("data", tsi);
		UpdatePanelInfo (tempDic);
		btnSelect.isEnabled = true;
	}
	
	void CreateQuestDragList(object args){
		TStageInfo tsi = args as TStageInfo;
//		isEvolve = false;
		dragPanel = new DragPanel("QuestDragPanel", questViewItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(tsi.QuestInfo.Count);
		dragPanel.DragPanelView.SetScrollView(questSelectScrollerArgsDic);

		RefreshQuestInfo (tsi.QuestInfo);
	}

	void RefreshQuestInfo(List<TQuestInfo> questInfo) {
		if (dragPanel == null) {
			return;	
		}

		for (int i = 0, m = dragPanel.ScrollItem.Count; i < m; i++) {
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			UITexture tex = scrollItem.transform.FindChild("Texture_Quest").GetComponent<UITexture>();
			TQuestInfo tqi = questInfo[i];
			TUnitInfo tui = DataCenter.Instance.GetUnitInfo(11);
			tex.mainTexture = tui.GetAsset(UnitAssetType.Avatar);
			
			UILabel label = scrollItem.transform.FindChild("Label_Quest_NO").GetComponent<UILabel>();
			label.text = "Quest : " + (i+1).ToString();
			
			UIEventListener.Get(scrollItem.gameObject).onClick = ClickQuestItem;
		}
	}

	uint GetBossID(){
		return 11;
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
		avatarTexture.mainTexture = null;
		labDoorName.text = string.Empty;
		foreach (var item in pickEnemiesList){
			item.mainTexture = null;
		}
	}

	void ShowTween(){
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
	
	void InitQuestSelectScrollArgs(){
		questSelectScrollerArgsDic.Add("parentTrans",		scrollView.transform);
		questSelectScrollerArgsDic.Add("scrollerScale",		Vector3.one);
		questSelectScrollerArgsDic.Add("scrollerLocalPos",		-96 * Vector3.up);
		questSelectScrollerArgsDic.Add("position",				Vector3.zero);
		questSelectScrollerArgsDic.Add("clipRange",			new Vector4(0, 0, 640, 200));
		questSelectScrollerArgsDic.Add("gridArrange",		UIGrid.Arrangement.Horizontal);
		questSelectScrollerArgsDic.Add("maxPerLine",		0);
		questSelectScrollerArgsDic.Add("scrollBarPosition",	new Vector3(-320, -120, 0));
		questSelectScrollerArgsDic.Add("cellWidth",			130);
		questSelectScrollerArgsDic.Add("cellHeight",			130);

	}
	

}