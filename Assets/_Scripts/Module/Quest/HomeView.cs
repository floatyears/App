using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class HomeView : ViewBase{
	private GameObject storyRoot;
	private GameObject eventRoot;
//	private GameObject dragItemPrefab;
//	private DragPanel storyDragPanel;
//	private DragPanel eventDragPanel;
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

		MsgCenter.Instance.AddListener (CommandEnum.RefreshRewardList,OnRefreshRewardList);

		GameTimer.GetInstance ().CheckRefreshServer ();

		ShowRewardInfo ();

		List<CityInfo> cityList = DataCenter.Instance.QuestData.GetCityListInfo();
		for (int i = 0; i < cityList.Count; i++) {
			UISprite cityBg = FindChild("StoryDoor/"+i+"/Background").GetComponent<UISprite>();
			bool isNewCity = (DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState (cityList[i].id, ECopyType.CT_NORMAL) == StageState.NEW); 
			cityBg.GetComponent<TweenAlpha> ().enabled = isNewCity;
		}

		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.HOME);

	}

	private void OnRefreshRewardList(object data){
		ShowRewardInfo ();
	}

	private void ShowRewardInfo(){
		int count = 0;
		foreach (var item in DataCenter.Instance.UserData.LoginInfo.Bonus) {
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

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);

//		MsgCenter.Instance.RemoveListener (CommandEnum.ChangeSceneComplete,OnChangeSceneComplete);
		MsgCenter.Instance.RemoveListener (CommandEnum.RefreshRewardList,OnRefreshRewardList);
		MsgCenter.Instance.RemoveListener(CommandEnum.ResourceDownloadComplete,DownloadComplete);
		MsgCenter.Instance.RemoveListener(CommandEnum.ResourceDownloadComplete,DownloadCompleteEx);
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
		List<CityInfo> data = DataCenter.Instance.QuestData.GetCityListInfo();
		for (int i = 0; i < data.Count; i++){
			GameObject cityItem = transform.FindChild("StoryDoor/" + i.ToString()).gameObject;
			FindChild("StoryDoor/" + i.ToString() + "/Label").GetComponent<UILabel>().text = TextCenter.GetText("City_Name_" + (i+1));
			if(cityItem == null){
//				Debug.LogError(string.Format("Resoures ERROR :: InitWorldMap(), Index[ {0} ] Not Found....!!!", i));
				continue;
			}
			UIEventListenerCustom.Get(cityItem).onClick = PressStoryDoor;
			cityViewInfo.Add(cityItem, data[ i ]);
		}

		eventRoot = transform.FindChild("EventDoor").gameObject;
//		eventRoot.SetActive (false);
		UIEventListenerCustom.Get(eventRoot).onPress = PressEventDoor;
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
                
		if (DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState (cityViewInfo [item].id, ECopyType.CT_NORMAL) == StageState.LOCKED) {
			TipsManager.Instance.ShowTipsLabel (TextCenter.GetText ("Stage_Locked"), item);
		} else {
//			List<TStageInfo> stages = DataCenter.Instance.QuestData.GetCityInfo(1).Stages;
//			List<TQuestInfo> quests = stages[stages.Count - 1].QuestInfo;
			if(cityViewInfo [item].id == 2 && (DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState(2, ECopyType.CT_NORMAL) == StageState.NEW) && (GameDataPersistence.Instance.GetData("ResourceComplete") != "true")){//QuestClearInfo.GetStoryStageState (cityViewInfo [item].ID)){
				
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

			if(DataCenter.Instance.UserData.UserInfo.rank < 10){
				
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
		int userUnitMaxCount = DataCenter.Instance.UserData.UserInfo.unitMax;
		//Debug.Log("userUnitMaxCount : " + userUnitMaxCount);
		int userCurrGotUnitCount = DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count;
		//Debug.Log("userCurrGotUnitCount : " + userCurrGotUnitCount);
	
		if(userUnitMaxCount < userCurrGotUnitCount){
//			Debug.Log("current user's unit count is outnumber of the max!");
			Umeng.GA.Event("UnitLimited");
			Umeng.GA.StartLevel ("UnitLimited");
			Umeng.GA.FinishLevel ("UnitLimited");

//			MsgCenter.Instance.Invoke(CommandEnum.OpenMsgWindow, GetUnitCountOverParams());
			TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("UnitOverflow"),
			                                   TextCenter.GetText("UnitOverflowText",DataCenter.Instance.UnitData.UserUnitList.GetAllMyUnit().Count,DataCenter.Instance.UserData.UserInfo.unitMax),
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
		fog.alpha = 1f;
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

