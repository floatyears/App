using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class HomeView : ViewBase{
	private GameObject storyRoot;
	private GameObject eventRoot;
	private GameObject dragItemPrefab;
	private DragPanel storyDragPanel;
	private DragPanel eventDragPanel;
	private Dictionary< GameObject, VStageItemInfo> stageInfo = new Dictionary<GameObject, VStageItemInfo>();
	private Dictionary<GameObject, CityInfo> cityViewInfo = new Dictionary<GameObject, CityInfo>();

	private UISprite fog;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
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
		if((ModuleEnum)data == ModuleEnum.HomeModule){
			if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_PARTY || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_LEVEL_UP || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_EVOLVE_EXE) {
				ModuleManager.Instance.ShowModule (ModuleEnum.UnitsMainModule);	
			} else if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.SCRATCH) {
				ModuleManager.Instance.ShowModule(ModuleEnum.ScratchModule);
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
	
	public override void CallbackView(params object[] args){
//		base.CallbackView(data);
//		CallBackDispatcherArgs cbdArgs = data as CallBackDispatcherArgs;
		switch (args[0].ToString()){
			case "CreateStoryView": 
				CreateStoryView(args[1]);
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
		ModuleManager.Instance.ShowModule (ModuleEnum.RewardModule);
	}

	void ClickNotice(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.OperationNoticeModule);
	}

	void ClickPurchase(GameObject obj){
		ModuleManager.Instance.ShowModule (ModuleEnum.ShopModule);
	}

	void CreateStoryView(object args){
		List<CityInfo> tciList = args as List<CityInfo>;
		storyDragPanel = new DragPanel("StageDragPanel", dragItemPrefab,transform);
		CreateScrollView(storyDragPanel, tciList);
	}

	void CreateScrollView(DragPanel panel, List<CityInfo> cityList){
//		panel.CreatUI();
		panel.AddItem(GetDragPanelCellCount(cityList));               
		UpdateInfo (panel, cityList);

		foreach (var item in panel.ScrollItem)
			UIEventListener.Get(item).onClick = ClickStoryItem;
	}

	void UpdateInfo (DragPanel panel, List<CityInfo> cityList) {
		List<StageInfo> temp = new List<StageInfo>();
		for (int i = 0; i < cityList.Count; i++) {
			CityInfo tci = cityList[ i ];
			for (int j = 0; j < tci.stages.Count; j++) {
				StageInfo tsi = tci.stages[ j ];
				tsi.InitStageId(tci.id);
				temp.Add(tsi);
			}
		}

		for (int i = 0; i < temp.Count; i++) {
			VStageItemInfo vsii = new VStageItemInfo();
			vsii.Refresh(panel.ScrollItem[ i ], temp[ i ]);
			stageInfo.Add(panel.ScrollItem[ i ], vsii);
		}
	}
	
	int GetDragPanelCellCount(List<CityInfo> cityList){
		int count = 0;
		for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++){
			count += cityList[ cityIndex ].stages.Count;
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
		Debug.LogError("ClickStoryItem ");
//		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs();
		ModuleManager.SendMessage (ModuleEnum.HomeModule, "ClickStoryItem", stageInfo [item].StageInfo);
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
		List<CityInfo> data = DataCenter.Instance.GetCityListInfo();
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
                
		if (DataCenter.Instance.QuestClearInfo.GetStoryCityState (cityViewInfo [item].id) == StageState.LOCKED) {
			TipsManager.Instance.ShowTipsLabel (TextCenter.GetText ("Stage_Locked"), item);
		} else {
//			List<TStageInfo> stages = DataCenter.Instance.GetCityInfo(1).Stages;
//			List<TQuestInfo> quests = stages[stages.Count - 1].QuestInfo;
			if(cityViewInfo [item].id == 2 && (DataCenter.Instance.QuestClearInfo.GetStoryCityState(2) == StageState.NEW) && (GameDataPersistence.Instance.GetData("ResourceComplete") != "true")){//QuestClearInfo.GetStoryStageState (cityViewInfo [item].ID)){
				
//				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("DownloadResourceTipTile"),TextCenter.GetText("DownloadResourceTipContent"),TextCenter.GetText("OK"),o=>{
					MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,DownloadComplete);
					ModuleManager.Instance.ShowModule(ModuleEnum.ResourceDownloadModule);
				});
				return;
			}

			AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
			ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityViewInfo [item].id);
//			MsgCenter.Instance.Invoke (CommandEnum.OnPickStoryCity, cityViewInfo [item].ID);
			Debug.Log ("CityID is : " + cityViewInfo [item].id);
		}
	}

	void DownloadComplete(object data){
//		AudioManager.Instance.PlayAudio (AudioEnum.sound_click);
		ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",(uint)2);
	}

	void DownloadCompleteEx(object data){
//		AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
		ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"event",null);
	}

	private void PressEventDoor(GameObject item, bool isPressed){
		if(!isPressed){
			Debug.Log("PressEventDoor()...");
			if(CheckUnitsLimit()){
				return;
			}

			if(DataCenter.Instance.UserInfo.rank < 10){
				
//				MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, mwp);
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("EventRankNeedTitle"),TextCenter.GetText("EventRankNeedContent"),TextCenter.GetText("OK"));
				return;
			}
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);

			if((GameDataPersistence.Instance.GetData("ResourceComplete") != "true")){//QuestClearInfo.GetStoryStageState (cityViewInfo [item].ID)){
				MsgCenter.Instance.AddListener(CommandEnum.ResourceDownloadComplete,DownloadCompleteEx);
				ModuleManager.Instance.ShowModule(ModuleEnum.ResourceDownloadModule);
				return;
			}


			ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"event",null);
			MsgCenter.Instance.Invoke(CommandEnum.OnPickEventCity, null);
		}
	}

	private bool CheckUnitsLimit(){
//		Debug.Log ("click-------------");
		int userUnitMaxCount = DataCenter.Instance.UserInfo.unitMax;
		//Debug.Log("userUnitMaxCount : " + userUnitMaxCount);
		int userCurrGotUnitCount = DataCenter.Instance.UserUnitList.GetAllMyUnit().Count;
		//Debug.Log("userCurrGotUnitCount : " + userCurrGotUnitCount);
	
		if(userUnitMaxCount < userCurrGotUnitCount){
//			Debug.Log("current user's unit count is outnumber of the max!");
			Umeng.GA.Event("UnitLimited");
			Umeng.GA.StartLevel ("UnitLimited");
			Umeng.GA.FinishLevel ("UnitLimited");

//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitCountOverParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("UnitOverflow"),
			                                   TextCenter.GetText("UnitOverflowText",DataCenter.Instance.UserUnitList.GetAllMyUnit().Count,DataCenter.Instance.UserInfo.unitMax),
			                                   TextCenter.GetText("OK"),
			                                   TurnScene);


			return true;
		}
		else
			return false;
	}

	void TurnScene(object msg){
		ModuleManager.Instance.ShowModule(ModuleEnum.ShopModule);
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

