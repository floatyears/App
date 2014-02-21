using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestSelectDecoratorUnity : UIComponentUnity ,IUICallback
{
	StageInfo stageInfo;
	public static UIImageButton btnSelect;
	IUICallback iuiCallback;
	bool temp = false;
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
	bool isInitDragPanelArgs = false;
	private DragPanel questDragPanel;
	private GameObject scrollerItem;
	private GameObject scrollView;
	List<UITexture> pickEnemiesList = new List<UITexture>();

	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	List<QuestInfo> questInfoList = new List<QuestInfo>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		temp = origin is IUICallback;
		InitUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		ShowTween();
		btnSelect.isEnabled = false;
		CleanQuestInfo();
		MsgCenter.Instance.AddListener(CommandEnum.TransmitStageInfo, ReceiveStageInfo);
	}
	
	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.TransmitStageInfo, ReceiveStageInfo);
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	void ReceiveStageInfo( object data ){
		StageInfo receivedStageInfo = data as StageInfo;
		stageInfo = receivedStageInfo;
		questInfoList = stageInfo.quests;
		InitDragPanel();
	}

	void InitUI(){

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
		//avatarTexture.mainTexture ;

		GameObject pickEnemies;
		pickEnemies = FindChild("Window/window_right/content_detail/pickEnemies");
		UITexture[] texs = pickEnemies.GetComponentsInChildren<UITexture>();
		foreach (var item in texs){
			pickEnemiesList.Add(item);
		} 

		UIEventListener.Get(btnSelect.gameObject).onClick = ChangeScene;
	}

	void InitDragPanel(){
		if(questDragPanel != null){
//			Debug.LogError("Not Need Create Drag Panel");
			return ;
		}
		questDragPanel = CreateDragPanel(questInfoList.Count);
		FillDragPanel(questDragPanel, questInfoList);
		InitQuestSelectScrollArgs();
		questDragPanel.RootObject.SetScrollView(questSelectScrollerArgsDic);
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
		string textureSourcePath = string.Format("Avatar/role00{0}",questInfo.no);
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


	void ClickQuestItem(GameObject go ){
		int index = questDragPanel.ScrollItem.IndexOf( go );
//		Debug.LogError("Index : " + index);
//		Debug.LogError("questInfoList Count: " + questInfoList.Count);
		QuestInfo currentInfo = questInfoList[ index ];

		labStaminaVaule.text = currentInfo.stamina.ToString();
		labFloorVaule.text = currentInfo.floor.ToString();
		labQuestInfo.text = currentInfo.name;
		questNameLabel.text = string.Format("Quest : {0}",currentInfo.no);
		labDoorName.text = stageInfo.stageName;
		rewardExpLabel.text = string.Format( "Exp : {0}", currentInfo.rewardExp.ToString() );
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = string.Format("Cion : {0}", currentInfo.rewardMoney.ToString() );

		string avatarTexturePath = "Avatar/role01" + currentInfo.no.ToString();
		avatarTexture.mainTexture = Resources.Load( avatarTexturePath ) as Texture2D;

		int enemyCount = 4;
		for (int i = 0; i < enemyCount; i++){
			string enemyAvatarTexturePath = "Avatar/role02" + i.ToString();
			pickEnemiesList[ i ].mainTexture = Resources.Load(enemyAvatarTexturePath) as Texture2D;
		}

		labStoryContent.text = currentInfo.story;

		btnSelect.isEnabled = true;
	}


	private void ChangeScene(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);
	}
	
	public void Callback(object data){
		bool b = (bool)data;
		btnSelect.isEnabled = b;
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

	private void ShowTween()
	{
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)
			return;
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
				continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}
	
	private void InitQuestSelectScrollArgs(){
		if( isInitDragPanelArgs )	return;
		questSelectScrollerArgsDic.Add("parentTrans",		scrollView.transform);
		questSelectScrollerArgsDic.Add("scrollerScale",		Vector3.one);
		questSelectScrollerArgsDic.Add("scrollerLocalPos",		-96 * Vector3.up);
		questSelectScrollerArgsDic.Add("position",			Vector3.zero);
		questSelectScrollerArgsDic.Add("clipRange",			new Vector4(0, 0, 640, 200));
		questSelectScrollerArgsDic.Add("gridArrange",		UIGrid.Arrangement.Horizontal);
		questSelectScrollerArgsDic.Add("maxPerLine",		0);
		questSelectScrollerArgsDic.Add("scrollBarPosition",	new Vector3(-320, -120, 0));
		questSelectScrollerArgsDic.Add("cellWidth",			130);
		questSelectScrollerArgsDic.Add("cellHeight",			130);

		isInitDragPanelArgs = true;
	}
}