using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class StageSelectView : ViewBase{
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
//	private DragPanel dragPanel;
	private GameObject scrollView;
	private GameObject questViewItem;
	private List<UITexture> pickEnemiesList = new List<UITexture>();
	private UITexture background;

	private List<QuestInfo> questInfoList = new List<QuestInfo>();
	private StageInfo curStageInfo;
	private int curQuestIndex;
	private UnitDataModel evolveStageInfo;
	private List<StageItemView> storyStageList = new List<StageItemView>();
	private List<GameObject> stageDotList = new List<GameObject>();
	
	private GameObject eventStageRoot;

	private string currentCityName = "";

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
		storyStageRoot = transform.FindChild("StoryStages").gameObject;
		eventStageRoot = transform.FindChild("EventStages").gameObject;
	}
	
	public override void ShowUI(){
		base.ShowUI();
		if (viewData.ContainsKey("story")) {
			ShowStoryCityView(viewData["story"]);
		}else if(viewData.ContainsKey("event")){
			ShowEventCityView();
		}else if(viewData.ContainsKey("evolve")){
			EvolveStartQuest(viewData["evolve"]);
		}

		if (currentCityName != "") {
			SetSceneName(currentCityName);
		}
	}

	private void ShowEventCityView(){
		List<StageInfo> eventStageList = FilterEventCityData(DataCenter.Instance.QuestData.EventStageList);
		if(eventStageList == null){
			Debug.LogError("DataCenter.Instance.QuestData.EventStageList == NULL, return...");
			return;
		}

		Debug.Log("Now event stage count is :" + eventStageList.Count);

		for (int i = 0; i < eventStageRoot.transform.childCount; i++) {
			GameObject stageItem = eventStageRoot.transform.GetChild(i).gameObject;
			Destroy(stageItem);
		}

		foreach (var item in stageDotList) {
			GameObject.Destroy(item);
		}
		stageDotList.Clear ();

		storyStageRoot.gameObject.SetActive(false);
		eventStageRoot.gameObject.SetActive(true);

		ResourceManager.Instance.LoadLocalAsset("Stage/1", o =>{
			FindChild<UITexture>("Background").mainTexture = o as Texture2D;
		});

		for (int i = 0; i < eventStageList.Count; i++){
			GameObject cell = NGUITools.AddChild(eventStageRoot, EventItemView.Prefab);
			cell.name = i.ToString();
			EventItemView stageItemView = EventItemView.Inject(cell);
			stageItemView.Data = eventStageList[i];

//			Debug.LogError("ICON id:"+eventStageList[i].id+"start:"+eventStageList[i].StartTime+" endTime:"+eventStageList[i].EndTime);
		}

		SetSceneName (TextCenter.GetText ("SCENE_NAME_EVENTSTAGE"));
	}

	private List<StageInfo> FilterEventCityData(List<StageInfo> eventCityData){
		Dictionary<uint,StageInfo> cityData = new Dictionary<uint,StageInfo> ();
		uint now = GameTimer.GetInstance ().GetCurrentSeonds ();
		foreach (var item in eventCityData) {
			if(cityData.ContainsKey(item.cityId)) {
//				Debug.LogWarning(item.cityId+"|"+item.id+") EndTime:"+cityData[item.cityId].EndTime+" | item.End="+item.EndTime);
				if(cityData[item.cityId].EndTime > item.EndTime){
					if(now < item.EndTime){
						cityData[item.cityId] = item;
					}
				}else{
					if(now > cityData[item.cityId].EndTime){
//						Debug.LogWarning(item.cityId+"|"+item.id+") Now > cityData[cityId].EndTime, UPDATE | item.End="+item.EndTime);
						cityData[item.cityId] = item;
					}
				}
			} else {
				if(now < item.EndTime) {
//					Debug.LogWarning(item.cityId+"|"+item.id+") Now<item.EndTime, Insert | item.End="+item.EndTime);
					cityData[item.cityId] = item;
				}
			}
		}

		return new List<StageInfo> (cityData.Values);
	}

	private void DestoryStages(){
		for (int i = 0; i < storyStageRoot.transform.childCount; i++){
			GameObject stageItem = storyStageRoot.transform.GetChild(i).gameObject;
			Destroy(stageItem);
		}
	}
	
	//MsgWindow show, note stamina is not enough.
	private bool CheckStaminaEnough(int staminaNeed, int staminaNow){
		if(staminaNeed > staminaNow) return true;
		else return false;
	}
	
	private MsgWindowParams GetStaminaLackMsgParams(){
		MsgWindowParams msgParams = new MsgWindowParams();
		msgParams.titleText = TextCenter.GetText("StaminaLackNoteTitle");
		msgParams.contentText = TextCenter.GetText("StaminaLackNoteContent");
		msgParams.btnParam = new BtnParam();
		return msgParams;
	}

	//--------------------------------New---------------------------------------

	private CityInfo currPickedCityInfo;
	private GameObject storyStageRoot;
	private List<StageItemView> stageViewList  = new List<StageItemView>();

	private void GetData(uint cityID){
		CityInfo received = DataCenter.Instance.QuestData.GetCityInfo(cityID);

//		if(currPickedCityInfo == null){
//			//when first time to step in
//			Debug.Log("recorded picked cityInfo is null, as first time to step in, create stage view...");
//			currPickedCityInfo = received;
////			Debug.LogError("received : " + received);
//			DestoryStages();
//			FillView();
//		} else if(!currPickedCityInfo.Equals(received)){
//			//when picked city changed
//			Debug.Log("recorded picked cityInfo is changed, update stage view...");
//			currPickedCityInfo = received;
//			DestoryStages();
//			FillView();
//		} else{
//			//when picked city not changed
//			Debug.Log("recorded picked cityInfo is not changed, keep stage view...");
//		}
		currPickedCityInfo = received;
		//			Debug.LogError("received : " + received);
		DestoryStages();
		FillView();
	}
	
	private void ShowStoryCityView(object msg){
		storyStageRoot.gameObject.SetActive(true);
		eventStageRoot.gameObject.SetActive(false);
		GetData((uint)msg);
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.STAGE_SELECT);
		SetSceneName (TextCenter.GetText("City_Name_" + currPickedCityInfo.id));
	}

	private void FillView(){
		if(currPickedCityInfo == null) {
			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
			return;
		}
	
		List<StageInfo> accessStageList = currPickedCityInfo.stages;
		GenerateStages(accessStageList);

	}

	/// <summary>
	/// Gets the access stage list.
	/// Add the whole cleared stage and the first one not cleared to the list
	/// </summary>
	/// <returns>The access stage list.</returns>
	private List<StageInfo> GetAccessStageList(List<StageInfo> stageInfoList){
		List<StageInfo> accessStageList = new List<StageInfo>();
		for (int i = 0; i < stageInfoList.Count; i++){
			if(stageInfoList[ i ].type == QuestType.E_QUEST_STORY){
				accessStageList.Add(stageInfoList[ i ]);
				if (!DataCenter.Instance.QuestData.QuestClearInfo.IsStoryStageClear(stageInfoList[ i ]))
					break;					
			}
			else{
				Debug.LogError("Error: invalid quest type:" + stageInfoList[i].type);
				break;
			}
		}
		return accessStageList;
	}
	
	/// <summary>
	/// Generates the stage page.
	/// </summary>
	/// <param name="count">Count.</param>
	private void GenerateStages(List<StageInfo> accessStageList){
		background = FindChild<UITexture>("Background");
		ResourceManager.Instance.LoadLocalAsset("Stage/1" /*+ currPickedCityInfo.ID*/, o =>{
			background.mainTexture = o as Texture2D;
		});

		storyStageList.Clear ();

		bool searchFarthestArrivedStageSucceed = false;
		for (int i = 0; i < accessStageList.Count; i++){
			GameObject cell = NGUITools.AddChild(storyStageRoot, StageItemView.Prefab);
			cell.name = i.ToString();
			StageItemView stageItemView = StageItemView.Inject(cell);

//			if(!searchFarthestArrivedStageSucceed){
//
//				if(!DataCenter.Instance.QuestData.QuestClearInfo.IsStoryStageClear(accessStageList[ i ])){
//					stageItemView.IsArrivedStage = true;
//					searchFarthestArrivedStageSucceed = true;
//				}
//				else{
//					stageItemView.IsArrivedStage = false;
//				}
//			}
			stageItemView.Data = accessStageList[ i ];
			storyStageList.Add(stageItemView);
		}

		foreach (var item in stageDotList) {
			GameObject.Destroy(item);
		}
		stageDotList.Clear ();
		ResourceManager.Instance.LoadLocalAsset ("Prefabs/UI/Quest/StageItemDot", o => {
			GameObject dot = o as GameObject;

			for (int i = 1; i < storyStageList.Count; i++) {
				float x1 = storyStageList[i-1].transform.localPosition.x;
				float y1 = storyStageList[i-1].transform.localPosition.y;
				float x2 = storyStageList[i].transform.localPosition.x;
				float y2 = storyStageList[i].transform.localPosition.y;

//				if(x2- x1 > 0){
				GameObject obj1 = NGUITools.AddChild(gameObject,dot);
//				Debug.Log("abs: " + );
				int n1 = Mathf.RoundToInt((x2-x1)/Mathf.Abs(x2-x1));
				int n2 = Mathf.RoundToInt((y2-y1)/Mathf.Abs(y2-y1));
				if(Mathf.Abs(x2-x1-n1*60) > Mathf.Abs(y2 - y1 - n2*60) ){
					obj1.transform.localPosition = new Vector3(x1 + n1* 30 + (x2-x1-n1*60)*1/3, y1 + n2*Mathf.Abs((y2-y1)/(x2-x1)*30) + (y2-y1)/(x2-x1)*(x2-x1-n1*60)*1/3, 0);
					stageDotList.Add(obj1);
					
					GameObject obj2 = NGUITools.AddChild(gameObject,dot);
					obj2.transform.localPosition = new Vector3(x1 + n1*30 + (x2-x1-n1*60)*2/3 , y1 + n2*Mathf.Abs((y2-y1)/(x2-x1)*30) + (y2-y1)/(x2-x1)*(x2-x1-n1*60)*2/3, 0);
					stageDotList.Add(obj2);
				}
				else{
					obj1.transform.localPosition = new Vector3(x1 + n1* Mathf.Abs((x2-x1)/(y2-y1)*30) + (x2-x1)/(y2-y1)*(y2-y1-n2*60)*1/3, y1 + n2*30 + (y2-y1 - n2*60)*1/3, 0);
					stageDotList.Add(obj1);
					
					GameObject obj2 = NGUITools.AddChild(gameObject,dot);
					obj2.transform.localPosition = new Vector3(x1 + n1* Mathf.Abs((x2-x1)/(y2-y1)*30) + (x2-x1)/(y2-y1)*(y2-y1-n2*60)*2/3, y1 + n2*30 + (y2-y1 - n2*60)*2/3, 0);
					stageDotList.Add(obj2);
				}
//				}else{
//					GameObject obj1 = NGUITools.AddChild(gameObject,dot);
//					obj1.transform.localPosition = new Vector3(x1 - 30 + (x2-x1+60)*1/3, y1 - (y2-y1)/(x2-x1)*30 + (y2-y1)/(x2-x1)*(x2-x1+60)*1/3, 0);
//					stageDotList.Add(obj1);
//					
//					GameObject obj2 = NGUITools.AddChild(gameObject,dot);
//					obj2.transform.localPosition = new Vector3(x1 - 30 + (x2-x1+60)*2/3 , y1 - (y2-y1)/(x2-x1)*30 + (y2-y1)/(x2-x1)*(x2-x1+60)*2/3, 0);
//					stageDotList.Add(obj2);
//				}


			}
		});

	}

	private List<GameObject> pageMarkItemList = new List<GameObject>();

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
//			rightPageBtn.gameObject.SetActive(!IsEndPage);
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
//			IsEndPage = (CurrPageIndex == totalPageCount);
		}
	}

	/// <summary>
	/// Gets the index of the curr stage.
	/// The last stage which has been cleared.
	/// </summary>
	/// <returns>The curr stage index.</returns>
	private int GetCurrStageIndex(List<StageInfo> stageInfoList){
		return stageInfoList.Count;
	}

	private void EnableLightSprite(bool isEnabled){
		GameObject target = pageMarkItemList[ currPageIndex - 1 ];
		UISprite lightSpr = target.transform.FindChild("Sprite_Light").GetComponent<UISprite>();
		lightSpr.enabled = isEnabled;
	}
	
	//===========evolve==============================================
	void EvolveStartQuest (object data) {
//		Debug.LogError ("EvolveStartQuest");
		evolveStageInfo = data as UnitDataModel;
		GetData(evolveStageInfo.StageInfo.cityId);
		FillViewEvolve();

		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST);
		SetSceneName (TextCenter.GetText ("SCENE_NAME_EVOLVETAGE"));
	}

	void FillViewEvolve(){
		if(currPickedCityInfo == null) {
			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
			return;
		}

		foreach (var item in storyStageList) {
			if(item.Data.Equals(evolveStageInfo.StageInfo)) {
				item.evolveCallback = ClickEvolve;
				item.ShowIconByState(StageState.NEW);
				continue;
			}
			else{
				item.ShowIconByState(StageState.LOCKED);
//				UIEventListenerCustom listener = item.GetComponent<UIEventListenerCustom>();
//				listener.onClick = null;
//				Destroy(listener);
			}
		}	


	}

	void ClickEvolve() {
		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectStage, evolveStageInfo);
	}


	public GameObject GetStageNewItem(){
		if(storyStageRoot != null)
		foreach(var i in storyStageRoot.transform.GetComponentsInChildren<StageItemView>()){
			if(DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(i.Data.id) == StageState.NEW){
				return i.gameObject;
			}
		}

		return null;
	}

	public GameObject GetStageEvolveItem(){
		foreach (var item in storyStageList) {
			if(item.Data.Equals(evolveStageInfo.StageInfo)) {
				return item.gameObject;
			}
//			else{
//				item.ShowIconByState(StageState.LOCKED);
//				//				UIEventListenerCustom listener = item.GetComponent<UIEventListenerCustom>();
//				//				listener.onClick = null;
//				//				Destroy(listener);
//			}
		}
		return null;
	}

	private void SetSceneName(string name){
		currentCityName = name;
//		GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
//		obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (name);
	}
}





