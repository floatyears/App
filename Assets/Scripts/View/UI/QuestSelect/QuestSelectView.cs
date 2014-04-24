using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestSelectView : UIComponentUnity{
	private UIImageButton selectBtn;
	private UILabel doorLabel;
	private UILabel labDoorType;
	private UILabel floorLabel;
	private UILabel staminaLabel;
	private UILabel storyContentLabel;
	private UILabel questInfoLabel;

	private UILabel storyTextLabel;
	private UILabel rewardExpLabel;
	private UILabel rewardCoinLabel;
	private UILabel rewardLineLabel;
	private UILabel questNameLabel;
	private UITexture bossAvatar;
	private DragPanel dragPanel;
	private GameObject scrollView;
	private GameObject questViewItem;
	private List<UITexture> pickEnemiesList = new List<UITexture>();
	private UIToggle firstFocus;
	private Dictionary< string, object > questSelectScrollerArgsDic = new Dictionary< string, object >();

	private List<QuestInfo> questInfoList = new List<QuestInfo>();
	private TStageInfo curStageInfo;
	private int curQuestIndex;
	private TEvolveStart evolveStageInfo;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
//		InitUI();
//		questViewItem = Resources.Load("Prefabs/UI/Quest/QuestItem") as GameObject;

	}
	
	public override void ShowUI(){
		base.ShowUI();
//		MsgCenter.Instance.AddListener(CommandEnum.GetSelectedStage, GetSelectedStage);
//		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
//        firstFocus.value = true;
//		ShowTween();
		MsgCenter.Instance.AddListener(CommandEnum.TransPickedCity, CreateSlidePage);

	}

	public override void HideUI(){
		base.HideUI();
//		MsgCenter.Instance.RemoveListener(CommandEnum.GetSelectedStage, GetSelectedStage);
//		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
		MsgCenter.Instance.RemoveListener(CommandEnum.TransPickedCity, CreateSlidePage);
	}

	void GetSelectedStage(object data) {
		curStageInfo = data as TStageInfo;
		if(curStageInfo != null) CreateQuestDragList();
		else{ Debug.LogError("CreateQuestDragList(), Data is ERROR, return!!!"); }
	}

    public override void ResetUIState(){
        LogHelper.Log("QuestSelectDecoratorUnity.ClearUIState()");
//        CleanQuestInfo();
//        if (dragPanel != null){
//            dragPanel.DestoryUI();
//        }
//        selectBtn.isEnabled = false;
    }   

	void InitUI(){
		firstFocus = FindChild<UIToggle>("Window/window_right/tab_detail");
		scrollView = FindChild("ScrollView");
		selectBtn = FindChild<UIImageButton>("ScrollView/btn_quest_select"); 
		doorLabel = FindChild< UILabel >("Window/title/Label_door_name");
		labDoorType = FindChild< UILabel >("Window/title/Label_door_type_name");
		floorLabel = FindChild< UILabel >("Window/window_left/Label_floor_V");
		staminaLabel = FindChild< UILabel >("Window/window_left/Label_stamina_V");
		storyContentLabel = FindChild< UILabel >("Window/window_right/content_story/Label_story");
		questInfoLabel = FindChild< UILabel >("Window/window_right/content_detail/Label_quest_info");
		storyTextLabel = FindChild<UILabel>("Window/window_right/content_story/Label_story");
		rewardLineLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Line");
		rewardExpLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Exp");
		rewardCoinLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_Reward_Coin");
		questNameLabel = FindChild<UILabel>("Window/window_right/content_detail/Label_quest_name");
		bossAvatar = FindChild<UITexture>("Window/window_left/Texture_Avatar");

		GameObject pickEnemies;
		pickEnemies = FindChild("Window/window_right/content_detail/pickEnemies");
		UITexture[ ] texs = pickEnemies.GetComponentsInChildren<UITexture>();
		foreach (var item in texs){
			pickEnemiesList.Add(item);
		} 

		UIEventListener.Get(selectBtn.gameObject).onClick = ClickFriendSelect;
	}

	void UpdatePanelInfo(object args){
		Dictionary<string,object> info = args as Dictionary<string, object>;
		int index = (int)info["position"];
		TStageInfo tsi = info["data"] as TStageInfo;
		TQuestInfo select =  tsi.QuestInfo [index];
		DataCenter.Instance.currentQuestInfo = select;
		staminaLabel.text = select.Stamina.ToString();
		floorLabel.text = select.Floor.ToString();
		doorLabel.text = tsi.StageName;
		storyContentLabel.text = select.Story;
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = "Cion " + select.RewardMoney.ToString();
		questInfoLabel.text = select.Name;
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
		if (DataCenter.gameStage == GameState.Evolve) {return;}
		TQuestInfo pickedQuest =  item.QuestInfo;
		DataCenter.Instance.currentQuestInfo = pickedQuest;	

		ShowQuestRewardInfo(pickedQuest);
		ShowBossAvatar(pickedQuest.BossID[ 0 ]);
		ShowEnemiesAvatar(pickedQuest.EnemyID);
		ShowFocusQuestState(item);
		curQuestIndex = dragPanel.ScrollItem.IndexOf( item.gameObject );
	}

	private void ShowQuestRewardInfo(TQuestInfo pickedQuest){
		staminaLabel.text = pickedQuest.Stamina.ToString();
		floorLabel.text = pickedQuest.Floor.ToString();
		doorLabel.text = curStageInfo.StageName;
		storyContentLabel.text = pickedQuest.Story;
		rewardLineLabel.text = "/";
		rewardCoinLabel.text = "Cion " + pickedQuest.RewardMoney.ToString();
		questInfoLabel.text = pickedQuest.Name;
		rewardExpLabel.text = "Exp " + pickedQuest.RewardExp.ToString();
		storyTextLabel.text = curStageInfo.Description;
		selectBtn.isEnabled = true;
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

	private QuestItem prevPickedQuestItem;
	private void ShowFocusQuestState(QuestItem curPickedQuestItem){
		if(prevPickedQuestItem != null){
			if(curPickedQuestItem.Equals(prevPickedQuestItem)) return;
			else prevPickedQuestItem.IsFocus = false;
		}
		curPickedQuestItem.IsFocus = true;
		prevPickedQuestItem = curPickedQuestItem;
	}

	private void ClickFriendSelect(GameObject btn){
		AudioManager.Instance.PlayAudio( AudioEnum.sound_click );
		PrepareFriendSelect((DataCenter.gameStage == GameState.Evolve));
	}

	private void EvolveInfoShow (TStageInfo stageInfo) {
		TStageInfo tsi = stageInfo;
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
	
	void CreateQuestDragList(){
		List<TQuestInfo> infoListForShow = GetQuestInfoListForShow(curStageInfo);
		if(infoListForShow == null){ Debug.LogError("GetQuestInfoListForShow(), Data is Error, return!!!"); return; }
		else{
			//Debug.Log("GetQuestInfoListForShow(), infoListForShow count is : " + infoListForShow.Count);
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
					questItem.StageInfo = curStageInfo;
					questItem.QuestInfo = infoListForShow[ i ] ;
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
			if(DataCenter.Instance.QuestClearInfo.IsStoryQuestClear(curStageInfo.ID, questInfoList[ i ].ID)){
				//add the one as long as it is on state of clear
				infoListForShow.Add(questInfoList[ i ]);
			}
			else{
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
		staminaLabel.text = string.Empty;
        floorLabel.text = string.Empty;
		questInfoLabel.text = string.Empty;
		storyTextLabel.text = string.Empty;
		rewardLineLabel.text = string.Empty;
		rewardExpLabel.text = string.Empty;
		rewardCoinLabel.text = string.Empty;
		questNameLabel.text = string.Empty;
		questNameLabel.text = string.Empty;
		bossAvatar.mainTexture = null;
		doorLabel.text = string.Empty;
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

	void PrepareFriendSelect(bool flag){
		TStageInfo stageInfo = null;
		uint questID = 0;
		int staminaNeed = curStageInfo.QuestInfo[ curQuestIndex ].Stamina;
		int staminaNow = DataCenter.Instance.UserInfo.StaminaNow;
		
		if(CheckStaminaEnough(staminaNeed, staminaNow)){
			Debug.LogError("TurnToFriendSelect()......Stamina is not enough, MsgWindow show...");
			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetStaminaLackMsgParams());
			return;
		}
		
		UIManager.Instance.ChangeScene(SceneEnum.FriendSelect);
		if (flag) {
			MsgCenter.Instance.Invoke( CommandEnum.EvolveSelectQuest, evolveStageInfo);
		}
		else {
			stageInfo = curStageInfo;
			questID = stageInfo.QuestInfo[ curQuestIndex ].ID;
			uint stageID = stageInfo.ID;
			Dictionary<string,uint> idArgs = new Dictionary<string, uint>();
			idArgs.Add("QuestID", questID);
			idArgs.Add("StageID", stageID);
			MsgCenter.Instance.Invoke( CommandEnum.GetSelectedQuest, idArgs);
		}
	}

	//MsgWindow show, note stamina is not enough.
	private bool CheckStaminaEnough(int staminaNeed, int staminaNow){
		if(staminaNeed > staminaNow) return true;
		else return false;
	}
	
	private MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.Instace.GetCurrentText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.Instace.GetCurrentText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}

	void EvolveStartQuest (object data) {
		evolveStageInfo = data as TEvolveStart;
		EvolveInfoShow(evolveStageInfo.StageInfo);
	}

	//--------------------------------New---------------------------------------

	private const float STAGE_PAENL_OFFSET_X = 640.0F;
	private const float PAGE_MARK_OFFSET_X = 40.0F;
	private UIButton leftPageBtn;
	private UIButton rightPageBtn;

	private GameObject leftPanel;
	private GameObject centerPanel;
	private GameObject rightPanel;

	private GameObject pageMark;
	private GameObject questRoot;
	private int totalPageCount;

	private void InitUIElement(){
		leftPageBtn = FindChild<UIButton>("Button_Page_Left");
		rightPageBtn = FindChild<UIButton>("Button_Page_Right");
		questRoot = transform.FindChild("Quest").gameObject;
		pageMark = transform.FindChild("PageMark").gameObject;

		UIEventListener.Get(leftPageBtn.gameObject).onClick = ClickLeftPageBtn;
		UIEventListener.Get(rightPageBtn.gameObject).onClick = ClickRightPageBtn;
	}
	
	private TCityInfo cityInfo;
	private List<StageItemView> stageViewList  = new List<StageItemView>();

	private void GetData(uint cityID){
		cityInfo = DataCenter.Instance.GetCityInfo(cityID);
	}


	private void CreateSlidePage(object msg){
		GetData((uint)msg);
		InitUIElement();
		FillView();
	}

	private void FillView(){
		if(cityInfo == null) {
			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
			return;
		}

		totalPageCount = cityInfo.Stages.Count;
		CurrPageIndex = GetCurrStageIndex(cityInfo.Stages);
		//Add the whole cleared stage and the first one not cleared to the list
		GenerateStagePage(cityInfo.Stages.Count);
		GeneratePageMark(cityInfo.Stages.Count);
		InitQuestRootPos(cityInfo.Stages.Count);
		EnableLightSprite(true);
	}
	
	/// <summary>
	/// Generates the stage page.
	/// </summary>
	/// <param name="count">Count.</param>
	private void GenerateStagePage(int count){
		string sourcePath = "Prefabs/UI/Quest/StageItem";
		GameObject stageItemprefab = Resources.Load(sourcePath) as GameObject;
		for (int i = 0; i < count; i++){
			GameObject stageViewItem = NGUITools.AddChild(questRoot, stageItemprefab);
			stageViewItem.name = i.ToString();
			int offsetCount = i - count/2;
			stageViewItem.transform.localPosition = STAGE_PAENL_OFFSET_X * offsetCount * Vector3.right;
		}
	}

	private List<GameObject> pageMarkItemList = new List<GameObject>();
	/// <summary>
	/// Generates the page mark.
	/// </summary>
	/// <param name="count">Count.</param>
	private void GeneratePageMark(int count){
		string sourcePath = "Prefabs/UI/Quest/PageMark";
		GameObject pageMarkPrefab = Resources.Load(sourcePath) as GameObject;
		for (int i = 0; i < count; i++){
			GameObject pageMarkItem = NGUITools.AddChild(pageMark, pageMarkPrefab);
			int offsetCount = i - count/2;
			pageMarkItem.name = i.ToString();
			pageMarkItem.transform.localPosition = PAGE_MARK_OFFSET_X * offsetCount * Vector3.right;
			pageMarkItemList.Add(pageMarkItem);
		}
	}

	private void InitQuestRootPos(int count){
		int offsetCount = count / 2 - (currPageIndex - 1);
		questRoot.transform.localPosition = STAGE_PAENL_OFFSET_X * offsetCount * Vector3.right;
	}

	/// <summary>
	/// Disable left btn as soon as current page is start page.
	/// </summary>
	private bool isStartPage;
	public bool IsStartPage{
		get{
			return isStartPage;
		}
		set{
			isStartPage = value;
			leftPageBtn.gameObject.SetActive(!IsStartPage);
		}
	}

	/// <summary>
	/// Disable right btn as soon as current page is end page.
	/// </summary>
	private bool isEndPage;
	public bool IsEndPage{
		get{
			return isEndPage;
		}
		set{
			isEndPage = value;
			rightPageBtn.gameObject.SetActive(!IsEndPage);
		}
	}
	
	/// <summary>
	/// The curreny page index.
	/// </summary>
	private int currPageIndex;
	public int CurrPageIndex{
		get{
			return currPageIndex;
		}
		set{
			currPageIndex = value;
			//Debug.LogError("currPageIndex : " + currPageIndex);
			//Set left btn disabled as soon as current page is start page.
			IsStartPage = (CurrPageIndex == 1);
			//Set right btn disabled as soon as current page is end page.
			IsEndPage = (CurrPageIndex == totalPageCount);
		}
	}
	
	private void ClickLeftPageBtn(GameObject btn){
		EnableLightSprite(false);
		CurrPageIndex --;
		//float x = questRoot.transform.localPosition.x + STAGE_PAENL_OFFSET_X;
		float x = (cityInfo.Stages.Count/2 - (currPageIndex - 1)) * STAGE_PAENL_OFFSET_X;
		iTween.MoveTo(questRoot, iTween.Hash("x", x, "time", 1.0f, "isLocal", true));
		EnableLightSprite(true);
	}

	private void ClickRightPageBtn(GameObject btn){
		EnableLightSprite(false);
		CurrPageIndex ++;
		//float x = questRoot.transform.localPosition.x - STAGE_PAENL_OFFSET_X;
		float x = (cityInfo.Stages.Count/2 - (currPageIndex - 1)) * STAGE_PAENL_OFFSET_X;
		iTween.MoveTo(questRoot, iTween.Hash("x", x, "time", 1.0f, "isLocal", true));
		EnableLightSprite(true);
	}

	/// <summary>
	/// Gets the index of the curr stage.
	/// The last stage which has been cleared.
	/// </summary>
	/// <returns>The curr stage index.</returns>
	private int GetCurrStageIndex(List<TStageInfo> stageInfoList){
		int index = 1;
		for (int i = stageInfoList.Count; i >0 ; i--){
			if(stageInfoList[ i - 1 ].State == EQuestState.QS_CLEARED){
				index = i;
				Debug.Log("Find the last cleared stage, index is : " + i);
				break;
			}
		}
		Debug.Log("GetCurrStageIndex(), index is : " + index);
		return index;
	}

//	private void LightPageMark(int nextPos){
//		//dark current
//		EnableLightSprite(currPageIndex - 1, false);
//		//light next( left or right )
//		EnableLightSprite(nextPos, true);
//	}

	private void EnableLightSprite(bool isEnabled){
		GameObject target = pageMarkItemList[ currPageIndex - 1 ];
		UISprite lightSpr = target.transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		lightSpr.enabled = isEnabled;
	}

}





