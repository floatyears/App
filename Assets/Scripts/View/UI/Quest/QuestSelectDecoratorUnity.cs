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

	private DragPanel questDragPanel;
	private GameObject scrollerItem;
	private GameObject scrollView;
	List<UITexture> pickEnemiesList = new List<UITexture>();

	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	List<QuestInfo> questInfoList = new List<QuestInfo>();

	public override void Init(UIInsConfig config, IUIOrigin origin){
		base.Init(config, origin);
		temp = origin is IUICallback;

		//tempQuestList = ConfigQuestList.questFriendList;
		InitUI();
	}
	
	public override void ShowUI(){
		base.ShowUI();
		ShowTweenPostion(0.2f);
		btnSelect.isEnabled = false;
		SetUIActive(true);
		CleanQuestInfo();
//		Debug.LogError("ShowUI : " + Time.realtimeSinceStartup);
		MsgCenter.Instance.AddListener(CommandEnum.TransmitStageInfo, ReceiveStageInfo);
	}
	
	public override void HideUI(){
		base.HideUI();
		ShowTweenPostion();
		MsgCenter.Instance.RemoveListener(CommandEnum.TransmitStageInfo, ReceiveStageInfo);
	}
	
	public override void DestoryUI(){
		base.DestoryUI();
	}

	void ReceiveStageInfo( object data ){
		Debug.Log("Receive");
		StageInfo receivedStageInfo = data as StageInfo;
		stageInfo = receivedStageInfo;
		questInfoList = stageInfo.quests;
		InitDragPanel();
//		Debug.LogError("Receive stage's quest count: " + questInfoList.Count);
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
		questDragPanel = CreateDragPanel(questInfoList.Count);
		Debug.LogError("quest drag panel scroll item count : " + questDragPanel.ScrollItem.Count );
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
		Debug.Log("count: " + count);
		GameObject scrollItem = GetScrollItem(UIConfig.questDragPanelItemPath);
		if( scrollItem == null)
			Debug.LogError("Not Find The Scroll Item");
		else
			Debug.LogError("Scroll Item Name : " +scrollItem.name );

		DragPanel dragPanel = new DragPanel("QuestDragPanel", scrollItem);
		dragPanel.CreatUI();
		dragPanel.AddItem(count);
		return dragPanel;
	}

	void FillDragPanel(DragPanel dragPanel, List<QuestInfo> infoList){
//		Debug.LogError(dragPanel.ScrollItem.Count);
		//Debug.LogError(dragPanel.RootObject.gameObject.name);
		for(int i = 0; i < dragPanel.ScrollItem.Count; i++){
			GameObject scrollItem = dragPanel.ScrollItem[ i ];
			ShowItemInfo( scrollItem, infoList[i]);
		}
	}

	void ShowItemInfo(GameObject item, QuestInfo questInfo){
		string textureSourcePath = string.Format("Avatar/role00{0}",questInfo.no);
		Debug.Log(string.Format("textureSourcePath : {0}", textureSourcePath));
		UITexture texture = item.transform.FindChild("Texture_Quest").GetComponent<UITexture>();
		texture.mainTexture = Resources.Load( textureSourcePath ) as Texture2D;
	}
	
	private void ChangeScene(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);
	}
	
	public void Callback(object data){
		bool b = (bool)data;
		btnSelect.isEnabled = b;
	}

	private void SetUIActive(bool b){
		//questDragPanel.RootObject.gameObject.SetActive(b);
	}

	private void PickQuestInfo(GameObject go){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		btnSelect.isEnabled = true;

		int questItemIndex = questDragPanel.ScrollItem.IndexOf(go);
		//Debug.Log(string.Format("Quest Item Index is : {0}",questItemIndex));

		//ShowQuestInfo(tempQuestList[questItemIndex]);
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

		foreach (var item in pickEnemiesList){
			item.mainTexture = null;
		}
	}

	void ShowQuestInfo(TempQuest selectedQuest){
		//Debug.LogError(selectedQuest.questIntroduction);
		//Debug.Log("Quest Select Scene: Show Quest Info");
		labStaminaVaule.text = selectedQuest.stamina.ToString();
		labFloorVaule.text = selectedQuest.floor.ToString();
		labQuestInfo.text = selectedQuest.questIntroduction;
		storyTextLabel.text = selectedQuest.stortText;
		rewardLineLabel.text = "/";
		rewardExpLabel.text = string.Format("Exp:{0}",selectedQuest.expReward.ToString());
		rewardCoinLabel.text = string.Format("Coin:{0}",selectedQuest.coinReward.ToString());
		questNameLabel.text = string.Format("Quest:{0}",selectedQuest.qustID.ToString());
		avatarTexture.mainTexture = Resources.Load(selectedQuest.questTexturePath)as Texture2D;

		for (int i = 0; i < selectedQuest.pickUpEneniesTexturePath.Count; i++){
			pickEnemiesList[ i ].mainTexture = 
				Resources.Load(selectedQuest.pickUpEneniesTexturePath[ i ]) as Texture2D;
		}
	}
	private void ShowTweenPostion(float mDelay = 0f, UITweener.Method mMethod = UITweener.Method.Linear){
		TweenPosition[ ] list = gameObject.GetComponentsInChildren< TweenPosition >();
		
		if (list == null)
			return;
		
		foreach (var tweenPos in list)
		{		
			if (tweenPos == null)
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

	private void InitQuestSelectScrollArgs(){
		questSelectScrollerArgsDic.Add("parentTrans",		scrollView.transform);
		questSelectScrollerArgsDic.Add("scrollerScale",		Vector3.one);
		questSelectScrollerArgsDic.Add("scrollerLocalPos",		-96 * Vector3.up);
		questSelectScrollerArgsDic.Add("position",			Vector3.zero);
		questSelectScrollerArgsDic.Add("clipRange",			new Vector4(0, 0, 640, 200));
		questSelectScrollerArgsDic.Add("gridArrange",		UIGrid.Arrangement.Horizontal);
		questSelectScrollerArgsDic.Add("maxPerLine",		0);
		questSelectScrollerArgsDic.Add("scrollBarPosition",	new Vector3(-320, -120, 0));
		questSelectScrollerArgsDic.Add("cellWidth",			230);
		questSelectScrollerArgsDic.Add("cellHeight",			150);
	}
}

public class ConfigQuestList{
	public static List<TempQuest> questFriendList = new List<TempQuest>();
	public ConfigQuestList(){
		Config();
	}

	void Config(){
//		Debug.Log("Config Temp Quest List");
		TempQuest tempQuest = new TempQuest();

		tempQuest.questTexturePath = "Avatar/role001";
		tempQuest.coinReward = 10;
		tempQuest.stamina = 3;
		tempQuest.floor = 1;
		tempQuest.expReward = 20;
		tempQuest.isClear = true;
		tempQuest.qustID = 1;
		tempQuest.questIntroduction = "魔界火种";
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role001");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role002");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role003");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role004");
		tempQuest.stortText = "Here is the introduction of this quest story." +
			"Many information can be caught in this text.Enjoy your game!";
		questFriendList.Add(tempQuest);

		tempQuest = new TempQuest();
		tempQuest.questTexturePath = "Avatar/role002";
		tempQuest.coinReward = 11;
		tempQuest.stamina = 5;
		tempQuest.floor = 2;
		tempQuest.expReward = 21;
		tempQuest.isClear = false;
		tempQuest.qustID = 2;
		tempQuest.questIntroduction = "燃烧机动源";
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role001");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role002");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role003");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role004");
		tempQuest.stortText = "Here is the introduction of this quest story." +
			"Many information can be caught in this text.Enjoy your game!";
		questFriendList.Add(tempQuest);

		tempQuest = new TempQuest();
		tempQuest.questTexturePath = "Avatar/role003";
		tempQuest.coinReward = 34;
		tempQuest.stamina = 9;
		tempQuest.floor = 3;
		tempQuest.expReward = 20;
		tempQuest.isClear = false;
		tempQuest.qustID = 3;
		tempQuest.questIntroduction = "赤焰龟";
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role001");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role002");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role003");
		tempQuest.pickUpEneniesTexturePath.Add("Avatar/role004");
		tempQuest.stortText = "Here is the introduction of this quest story." +
			"Many information can be caught in this text.Enjoy your game!";
		questFriendList.Add(tempQuest);

//		Debug.Log(string.Format("Config Quest List Count: {0}",questFriendList.Count));
	}

}

public class TempQuest{
	public bool isClear = false;
	public int stamina = 0;
	public int floor = 0;
	public int qustID = 0;
	public string questTexturePath = string.Empty;
	public string questIntroduction = string.Empty;
	public int expReward = 0;
	public int coinReward = 0;
	public List<string> pickUpEneniesTexturePath = new List<string>();
	public string stortText = string.Empty;

}
