using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class HomeView : UIComponentUnity{
	private GameObject storyRoot;
	private GameObject eventRoot;
	private GameObject dragItemPrefab;
	private DragPanel storyDragPanel;
	private DragPanel eventDragPanel;
	private Dictionary< GameObject, VStageItemInfo> stageInfo = new Dictionary<GameObject, VStageItemInfo>();
	private Dictionary<GameObject, TCityInfo> cityViewInfo = new Dictionary<GameObject, TCityInfo>();

	private UISprite fog;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, false);

		MsgCenter.Instance.AddListener (CommandEnum.ChangeSceneComplete,OnChangeSceneComplete);

		MsgCenter.Instance.AddListener (CommandEnum.RefreshRewardList,OnRefreshRewardList);

		GameTimer.GetInstance ().CheckRefreshServer ();

		ShowRewardInfo ();
	}

	private void OnRefreshRewardList(object data){
		ShowRewardInfo ();
	}

	private void ShowRewardInfo(){
		int count = 0;
		foreach (var item in DataCenter.Instance.LoginInfo.Bonus) {
			if(item.enabled == 1){
				count++;
			}
		}
		if (count > 0) {
			FindChild<UILabel> ("Icons/Reward/Num").enabled = true;
			FindChild<UISprite> ("Icons/Reward/NumBg").enabled = true;
			FindChild<UILabel> ("Icons/Reward/Num").text = count + "";	
		} else {
			FindChild<UILabel> ("Icons/Reward/Num").enabled = false;
			FindChild<UISprite> ("Icons/Reward/NumBg").enabled = false;
		}
	}

	private void OnChangeSceneComplete(object data ){
		if((SceneEnum)data == SceneEnum.Home){
			if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_PARTY || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_LEVEL_UP || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_EVOLVE_EXE) {
				UIManager.Instance.ChangeScene (SceneEnum.Units);	
			} else if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.SCRATCH) {
				UIManager.Instance.ChangeScene(SceneEnum.Scratch);
			}else if(NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.FRIEND_SELECT){
				NoviceGuideStepEntityManager.Instance().StartStep(NoviceGuideStartType.QUEST);
			}else if(NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.INPUT_NAME){
				NoviceGuideStepEntityManager.Instance().StartStep(NoviceGuideStartType.OTHERS);
			}
		}
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);

		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeSceneComplete,OnChangeSceneComplete);
		MsgCenter.Instance.RemoveListener (CommandEnum.RefreshRewardList,OnRefreshRewardList);
		MsgCenter.Instance.RemoveListener(CommandEnum.ResourceDownloadComplete,DownloadComplete);
		MsgCenter.Instance.RemoveListener(CommandEnum.ResourceDownloadComplete,DownloadCompleteEx);
	}
	
	public override void CallbackView(object data){
		base.CallbackView(data);
		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (cbdArgs.funcName){
			case "CreateStoryView": 
				CallBackDispatcherHelper.DispatchCallBack(CreateStoryView, cbdArgs);
				break;
			default:
				break;
		}
	}
	
	void InitUI(){
		InitWorldMap();

		UIEventListenerCustom.Get (FindChild ("Icons/Reward").gameObject).onClick = ClickReward;
		UIEventListenerCustom.Get (FindChild ("Icons/Notice").gameObject).onClick = ClickNotice;
		UIEventListenerCustom.Get (FindChild ("Icons/Purchase").gameObject).onClick = ClickPurchase;

		fog = FindChild("Fog").GetComponent<UISprite>();
		FindChild<UILabel> ("EventDoor/Label").text = TextCenter.GetText ("City_Event");
	}

	void ClickReward(GameObject obj){
		UIManager.Instance.ChangeScene (SceneEnum.Reward);
	}

	void ClickNotice(GameObject obj){
		UIManager.Instance.ChangeScene (SceneEnum.OperationNotice);
	}

	void ClickPurchase(GameObject obj){
		UIManager.Instance.ChangeScene (SceneEnum.Shop);
	}

	void CreateStoryView(object args){
		List<TCityInfo> tciList = args as List<TCityInfo>;
		storyDragPanel = new DragPanel("StageDragPanel", dragItemPrefab);
		CreateScrollView(storyDragPanel, tciList);
	}

	void CreateScrollView(DragPanel panel, List<TCityInfo> cityList){
		panel.CreatUI();
		panel.AddItem(GetDragPanelCellCount(cityList));               
		panel.DragPanelView.SetScrollView(ConfigDragPanel.StoryStageDragPanelArgs, storyRoot.transform);
		UpdateInfo (panel, cityList);

		foreach (var item in panel.ScrollItem)
			UIEventListener.Get(item).onClick = ClickStoryItem;
	}

	void UpdateInfo (DragPanel panel, List<TCityInfo> cityList) {
		List<TStageInfo> temp = new List<TStageInfo>();
		for (int i = 0; i < cityList.Count; i++) {
			TCityInfo tci = cityList[ i ];
			for (int j = 0; j < tci.Stages.Count; j++) {
				TStageInfo tsi = tci.Stages[ j ];
				tsi.InitStageId(tci.ID);
				temp.Add(tsi);
			}
		}

		for (int i = 0; i < temp.Count; i++) {
			VStageItemInfo vsii = new VStageItemInfo();
			vsii.Refresh(panel.ScrollItem[ i ], temp[ i ]);
			stageInfo.Add(panel.ScrollItem[ i ], vsii);
		}
	}
	
	int GetDragPanelCellCount(List<TCityInfo> cityList){
		int count = 0;
		for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++){
			count += cityList[ cityIndex ].Stages.Count;
		}
		return count;
	}

	void UpdateTexture(DragPanel panel, List<Texture2D> tex2dList){
		for (int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject item = panel.ScrollItem [i];
			UITexture texture = item.transform.FindChild("Texture").GetComponent<UITexture>();
			texture.mainTexture = tex2dList[ i ];
		}
	}

	void ClickStoryItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickStoryItem", stageInfo[item].StageInfo);
		ExcuteCallback(cbdArgs);
	}

	//----New
	private void InitWorldMap(){
		GetCityViewInfo();
		ShowCityView();
	}
	
	/// <summary>
	/// Gets the city view info, bind gameObject with data.
	/// </summary>
	private void GetCityViewInfo(){
		List<TCityInfo> data = DataCenter.Instance.GetCityListInfo();
		for (int i = 0; i < data.Count; i++){
			GameObject cityItem = transform.FindChild("StoryDoor/" + i.ToString()).gameObject;
			FindChild("StoryDoor/" + i.ToString() + "/Label").GetComponent<UILabel>().text = TextCenter.GetText("City_Name_" + (i+1));
			if(cityItem == null){
//				Debug.LogError(string.Format("Resoures ERROR :: InitWorldMap(), Index[ {0} ] Not Found....!!!", i));
				continue;
			}
			UIEventListener.Get(cityItem).onClick = PressStoryDoor;
			cityViewInfo.Add(cityItem, data[ i ]);
		}

		eventRoot = transform.FindChild("EventDoor").gameObject;
//		eventRoot.SetActive (false);
		UIEventListener.Get(eventRoot).onPress = PressEventDoor;
	}


	/// <summary>
	/// Shows the city sprite and name.
	/// </summary>
	private void ShowCityView(){
		if(cityViewInfo == null){
//			Debug.LogError("QuestView.InitWorldMap(), cityViewInfo is NULL"); 
			return;
		}

//		foreach (var item in cityViewInfo){
//			UISprite bgSpr = item.Key.transform.FindChild("Background").GetComponent<UISprite>();
//			bgSpr.enabled = false;
//		}
	}

	/// <summary>
	/// change scene to quest select with picked cityInfo
	/// </summary>
	/// <param name="item">Item.</param>
	private void PressStoryDoor(GameObject item){
		if(CheckUnitsLimit()){
			return;
		}
                
		if (DataCenter.Instance.QuestClearInfo.GetStoryCityState (cityViewInfo [item].ID) == StageState.LOCKED) {
			ViewManager.Instance.ShowTipsLabel (TextCenter.GetText ("Stage_Locked"), item);
		} else {
//			List<TStageInfo> stages = DataCenter.Instance.GetCityInfo(1).Stages;
//			List<TQuestInfo> quests = stages[stages.Count - 1].QuestInfo;
			if(cityViewInfo [item].ID == 2 && (DataCenter.Instance.QuestClearInfo.GetStoryCityState(2) == StageState.NEW) && (GameDataStore.Instance.GetData("ResourceComplete") != "true")){//QuestClearInfo.GetStoryStageState (cityViewInfo [item].ID)){

				MsgWindowParams mwp = new MsgWindowParams ();
				//mwp.btnParams = new BtnParam[1];
				mwp.btnParam = new BtnParam ();
				mwp.titleText = TextCenter.GetText("DownloadResourceTipTile");
				mwp.contentText = TextCenter.GetText("DownloadResourceTipContent");
				
				BtnParam sure = new BtnParam ();
				sure.callback = o=>{
					MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,DownloadComplete);
					UIManager.Instance.ChangeScene(SceneEnum.ResourceDownload);
				};
				sure.text = TextCenter.GetText("OK");
				mwp.btnParam = sure;
				
				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
				return;
			}

			AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
			UIManager.Instance.ChangeScene (SceneEnum.StageSelect);
			MsgCenter.Instance.Invoke (CommandEnum.OnPickStoryCity, cityViewInfo [item].ID);
			Debug.Log ("CityID is : " + cityViewInfo [item].ID);
		}
	}

	void DownloadComplete(object data){
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		UIManager.Instance.ChangeScene (SceneEnum.StageSelect);
		MsgCenter.Instance.Invoke (CommandEnum.OnPickStoryCity, (uint)2);
	}

	void DownloadCompleteEx(object data){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		UIManager.Instance.ChangeScene(SceneEnum.StageSelect);
		MsgCenter.Instance.Invoke(CommandEnum.OnPickEventCity, null);
	}

	private void PressEventDoor(GameObject item, bool isPressed){
		if(!isPressed){
			Debug.Log("PressEventDoor()...");
			if(CheckUnitsLimit()){
				return;
			}

			if(DataCenter.Instance.UserInfo.Rank < 10){
				MsgWindowParams mwp = new MsgWindowParams ();
				//mwp.btnParams = new BtnParam[1];
				mwp.btnParam = new BtnParam ();
				mwp.titleText = TextCenter.GetText("EventRankNeedTitle");
				mwp.contentText = TextCenter.GetText("EventRankNeedContent");
				
				BtnParam sure = new BtnParam ();
				sure.callback = null;
				sure.text = TextCenter.GetText("OK");
				mwp.btnParam = sure;
				
				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
				return;
			}
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

			if((GameDataStore.Instance.GetData("ResourceComplete") != "true")){//QuestClearInfo.GetStoryStageState (cityViewInfo [item].ID)){
				MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,DownloadCompleteEx);
				UIManager.Instance.ChangeScene(SceneEnum.ResourceDownload);
				return;
			}


			UIManager.Instance.ChangeScene(SceneEnum.StageSelect);
			MsgCenter.Instance.Invoke(CommandEnum.OnPickEventCity, null);
		}
	}

	private bool CheckUnitsLimit(){
//		Debug.Log ("click-------------");
		int userUnitMaxCount = DataCenter.Instance.UserInfo.UnitMax;
		//Debug.Log("userUnitMaxCount : " + userUnitMaxCount);
		int userCurrGotUnitCount = DataCenter.Instance.UserUnitList.GetAllMyUnit().Count;
		//Debug.Log("userCurrGotUnitCount : " + userCurrGotUnitCount);
	
		if(userUnitMaxCount < userCurrGotUnitCount){
//			Debug.Log("current user's unit count is outnumber of the max!");
			Umeng.GA.Event("UnitLimited");
			Umeng.GA.StartLevel ("UnitLimited");
			Umeng.GA.FinishLevel ("UnitLimited");

			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitCountOverParams());

			return true;
		}
		else
			return false;
	}
	
	private MsgWindowParams GetUnitCountOverParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("UnitOverflow");
		msgParams.contentText = TextCenter.GetText("UnitOverflowText", 
		                                           DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,
		                                           DataCenter.Instance.UserInfo.UnitMax);
		msgParams.btnParam = new BtnParam();
		msgParams.btnParam.callback = TurnScene;
		msgParams.fullScreenClick = true;
		return msgParams;
	}

	void TurnScene(object msg){
		UIManager.Instance.ChangeScene(SceneEnum.Shop);
	}

	public void FogFly(){
		fog.GetComponent<TweenPosition> ().enabled = true;
		fog.GetComponent<TweenPosition> ().ResetToBeginning ();
		int dir = 0;
		if (Random.Range (0, 1) > 0.5) {
			dir = -1;
		}else{
			dir = 1;
		}
		float y = Random.Range (100f, -150f);
		fog.GetComponent<TweenPosition> ().from = new Vector3(1024f*dir,y ,0);
		fog.GetComponent<TweenPosition> ().to = new Vector3(-1024f*dir,y ,0);
//		fog.transform.localPosition = new Vector3(?
	}
}

