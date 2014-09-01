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
	private DragPanel dragPanel;
	private GameObject scrollView;
	private GameObject questViewItem;
	private List<UITexture> pickEnemiesList = new List<UITexture>();
	private UITexture background;

	private List<QuestInfo> questInfoList = new List<QuestInfo>();
	private TStageInfo curStageInfo;
	private int curQuestIndex;
	private TEvolveStart evolveStageInfo;
	private List<StageItemView> storyStageList = new List<StageItemView>();
	private List<GameObject> stageDotList = new List<GameObject>();
	
	private GameObject eventStageRoot;

	private string currentCityName = "";

	public override void Init(UIConfigItem config){
		base.Init(config);
		storyStageRoot = transform.FindChild("StoryStages").gameObject;
		eventStageRoot = transform.FindChild("EventStages").gameObject;
	}
	
	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
		MsgCenter.Instance.AddListener(CommandEnum.OnPickStoryCity, ShowStoryCityView);
		MsgCenter.Instance.AddListener(CommandEnum.OnPickEventCity, ShowEventCityView);

		if(NoviceGuideStepEntityManager.CurrentNoviceGuideStage != NoviceGuideStage.EVOVLE_QUEST)
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (currentCityName != "") {
			SetSceneName(currentCityName);
		}
	}

	private void ShowEventCityView(object data){
		List<TStageInfo> eventStageList = FilterEventCityData(DataCenter.Instance.EventStageList);
		if(eventStageList == null){
			Debug.LogError("DataCenter.Instance.EventStageList == NULL, return...");
			return;
		}

		Debug.Log("Now event stage count is :" + eventStageList.Count);

		for (int i = 0; i < eventStageRoot.transform.childCount; i++) {
			GameObject stageItem = eventStageRoot.transform.GetChild(i).gameObject;
			Destroy(stageItem);
		}

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
		}

		SetSceneName (TextCenter.GetText ("SCENE_NAME_EVENTSTAGE"));
	}

	private List<TStageInfo> FilterEventCityData(List<TStageInfo> eventCityData){
		Dictionary<uint,TStageInfo> cityData = new Dictionary<uint,TStageInfo> ();
		uint now = GameTimer.GetInstance ().GetCurrentSeonds ();
		foreach (var item in eventCityData) {
			if(cityData.ContainsKey(item.CityId)) {
				if(cityData[item.CityId].endTime > item.endTime){
					if(now < item.endTime){
						cityData[item.CityId] = item;
					}
				}else{
					if(now > cityData[item.CityId].endTime){
						cityData[item.CityId] = item;
					}
				}
			} else {
				cityData[item.CityId] = item;
			}
		}
		return new List<TStageInfo> (cityData.Values);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveStart, EvolveStartQuest);
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickStoryCity, ShowStoryCityView);
		MsgCenter.Instance.RemoveListener(CommandEnum.OnPickEventCity, ShowEventCityView);
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

	private TCityInfo currPickedCityInfo;
	private GameObject storyStageRoot;
	private List<StageItemView> stageViewList  = new List<StageItemView>();

	private void GetData(uint cityID){
		TCityInfo received = DataCenter.Instance.GetCityInfo(cityID);

		if(currPickedCityInfo == null){
			//when first time to step in
			Debug.Log("recorded picked cityInfo is null, as first time to step in, create stage view...");
			currPickedCityInfo = received;
//			Debug.LogError("received : " + received);
			DestoryStages();
			FillView();
		} else if(!currPickedCityInfo.Equals(received)){
			//when picked city changed
			Debug.Log("recorded picked cityInfo is changed, update stage view...");
			currPickedCityInfo = received;
			DestoryStages();
			FillView();
		} else{
			//when picked city not changed
			Debug.Log("recorded picked cityInfo is not changed, keep stage view...");
		}
	}
	
	private void ShowStoryCityView(object msg){
		storyStageRoot.gameObject.SetActive(true);
		eventStageRoot.gameObject.SetActive(false);
		GetData((uint)msg);

		SetSceneName (TextCenter.GetText("City_Name_" + currPickedCityInfo.ID));
	}

	private void FillView(){
		if(currPickedCityInfo == null) {
			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
			return;
		}
	
		List<TStageInfo> accessStageList = currPickedCityInfo.Stages;
		GenerateStages(accessStageList);

	}

	/// <summary>
	/// Gets the access stage list.
	/// Add the whole cleared stage and the first one not cleared to the list
	/// </summary>
	/// <returns>The access stage list.</returns>
	private List<TStageInfo> GetAccessStageList(List<TStageInfo> stageInfoList){
		List<TStageInfo> accessStageList = new List<TStageInfo>();
		for (int i = 0; i < stageInfoList.Count; i++){
			if(stageInfoList[ i ].Type == QuestType.E_QUEST_STORY){
				accessStageList.Add(stageInfoList[ i ]);
				if (!DataCenter.Instance.QuestClearInfo.IsStoryStageClear(stageInfoList[ i ]))
					break;					
			}
			else{
				Debug.LogError("Error: invalid quest type:" + stageInfoList[i].Type);
				break;
			}
		}
		return accessStageList;
	}
	
	/// <summary>
	/// Generates the stage page.
	/// </summary>
	/// <param name="count">Count.</param>
	private void GenerateStages(List<TStageInfo> accessStageList){
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
//				if(!DataCenter.Instance.QuestClearInfo.IsStoryStageClear(accessStageList[ i ])){
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
	private int GetCurrStageIndex(List<TStageInfo> stageInfoList){
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
		evolveStageInfo = data as TEvolveStart;
		GetData(evolveStageInfo.StageInfo.CityId);
		FillViewEvolve();

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);
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
//				UIEventListener listener = item.GetComponent<UIEventListener>();
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
			if(DataCenter.Instance.QuestClearInfo.GetStoryStageState(i.Data.ID) == StageState.NEW){
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
//				//				UIEventListener listener = item.GetComponent<UIEventListener>();
//				//				listener.onClick = null;
//				//				Destroy(listener);
//			}
		}
		return null;
	}

	private void SetSceneName(string name){
		currentCityName = name;
		GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
		obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (name);
	}
}





