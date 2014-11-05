using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class StageSelectView : ViewBase{
	private const int MAX_CITY_ID = 5;

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

	private UIButton btnCopyTypeNormal;
	private UIButton btnCopyTypeElite;
	ECopyType currCopyType = ECopyType.CT_NORMAL;

	private int curQuestIndex;
	private UnitDataModel evolveStageInfo;
	private List<StageItemView> storyStageList = new List<StageItemView>();
	private List<GameObject> stageDotList = new List<GameObject>();
	
	private GameObject eventStageRoot;
	private UIButton rightButton, leftButton;

	private string currentCityName = "";
	private uint currentCityId;

	public override void Init(UIConfigItem config, Dictionary<string, object> data = null){
		base.Init(config,data);
		storyStageRoot = transform.FindChild("StoryStages").gameObject;
		eventStageRoot = transform.FindChild("EventStages").gameObject;

		UILabel normalText = FindChild<UILabel>("CopyType/Normal/text");
		UILabel eliteText = FindChild<UILabel>("CopyType/Elite/text");
		normalText.text = TextCenter.GetText("CopyNormal");
		eliteText.text = TextCenter.GetText("CopyElite");


		rightButton = FindChild<UIButton>("Button_Right");
		leftButton = FindChild<UIButton>("Button_Left");
	}
	
	public override void ShowUI(){
		base.ShowUI();
		UIToggle normal = FindChild<UIToggle>("CopyType/Normal");
		UIToggle elite = FindChild<UIToggle>("CopyType/Elite");
//		normal.optionCanBeNone = true;
//		elite.optionCanBeNone = true;

		if (viewData.ContainsKey("story")) {
			currentCityId = (uint) viewData["story"];
//			UIToggle toggle = UIToggle.GetActiveToggle (6);
//			currCopyType = ((toggle==null || toggle.name == "Normal" ) ? ECopyType.CT_NORMAL : ECopyType.CT_ELITE);
			Debug.Log("StageSelect.showUI >>> 111 currCopyType="+currCopyType+" elite.value:"+elite.value+" normal.val:"+normal.value);

			if(currCopyType == ECopyType.CT_ELITE){
				elite.SendMessage("OnClick");
			}
			else if(currCopyType == ECopyType.CT_NORMAL && UIToggle.GetActiveToggle (6).name != "Normal"){
				normal.SendMessage("OnClick");
			}

			Debug.Log("StageSelect.showUI >>> 222 currCopyType="+currCopyType+" elite.value:"+elite.value+" normal.val:"+normal.value);

//			if( toggle != null ) {
//				Debug.Log( "StageSelect1 >>>> UIToggle.GetActiveToggle(6) = "+toggle.name);
//			}
			ShowStoryCityView(currentCityId, currCopyType);

//			if( toggle != null )
//			Debug.Log( "StageSelect2 >>>> UIToggle.GetActiveToggle(6) = "+toggle.name);


			normal.gameObject.SetActive(true);
			elite.gameObject.SetActive(true);

		}else if(viewData.ContainsKey("event")) {
			ShowEventCityView();
			normal.gameObject.SetActive(false);
			elite.gameObject.SetActive(false);
		}
//		else if(viewData.ContainsKey("evolve")){
//			EvolveStartQuest(viewData["evolve"]);
//		}

		if (currentCityName != "") {
			SetSceneName(currentCityName);
		}

	}

	public void OnGoNextCity(object data) {
		currentCityId = currentCityId+1;
		if( currentCityId > MAX_CITY_ID ) {
			currentCityId = 1;
		}

		viewData["story"] = currentCityId;
		ShowStoryCityView(currentCityId, currCopyType);
	}

	public void OnGoPrevCity(object data) {
		currentCityId = currentCityId - 1;
		if( currentCityId < 1 ) {
			currentCityId = MAX_CITY_ID;
		}
		
		viewData["story"] = currentCityId;
		ShowStoryCityView(currentCityId, currCopyType);		
	}

	public void OnSelectCopyType(object data) {
		UIToggle toggle = UIToggle.GetActiveToggle (6);
		if( toggle == null ) {
			return;
		}
		ECopyType temp = (( toggle.name == "Normal" ) ? ECopyType.CT_NORMAL : ECopyType.CT_ELITE);
		if (currCopyType != temp) {
			currCopyType = temp;
			//			currCopyType = (currCopyType != ECopyType.CT_NORMAL  ? ECopyType.CT_NORMAL : ECopyType.CT_ELITE);
			Debug.LogWarning("After Stage.OnSelCopy:  currCopyType="+currCopyType);
			
			uint lastestCityId = DataCenter.Instance.QuestData.QuestClearInfo.GetNewestCity(currCopyType);
			
			ShowStoryCityView(lastestCityId, currCopyType);
		}
		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.STAGE_SELECT);
	}

	public override void CallbackView(params object[] args) {
		string cmd = (string)args[0];
		if(cmd == "ChangeCopyType") {
			currCopyType = (ECopyType)args[1];

//			UIToggle normal = FindChild<UIToggle>("CopyType/Normal");
//			UIToggle elite = FindChild<UIToggle>("CopyType/Elite");

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
//			
				Debug.LogWarning(item.cityId+"|"+item.id+") EndTime:"+cityData[item.cityId].EndTime+" | item.End="+item.EndTime);
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

//	private void GetData(uint cityID){
//		CityInfo received = DataCenter.Instance.QuestData.GetCityInfo(cityID);
//
////		if(currPickedCityInfo == null){
////			//when first time to step in
////			Debug.Log("recorded picked cityInfo is null, as first time to step in, create stage view...");
////			currPickedCityInfo = received;
//////			Debug.LogError("received : " + received);
////			DestoryStages();
////			FillView();
////		} else if(!currPickedCityInfo.Equals(received)){
////			//when picked city changed
////			Debug.Log("recorded picked cityInfo is changed, update stage view...");
////			currPickedCityInfo = received;
////			DestoryStages();
////			FillView();
////		} else{
////			//when picked city not changed
////			Debug.Log("recorded picked cityInfo is not changed, keep stage view...");
////		}
//		currPickedCityInfo = received;
//		//			Debug.LogError("received : " + received);
//		DestoryStages();
//		FillView();
//	}

	private void ShowStoryCityView(object msg, ECopyType currCopyType){
		storyStageRoot.gameObject.SetActive(true);
		eventStageRoot.gameObject.SetActive(false);

		uint cityID = (uint)msg;

		rightButton.gameObject.SetActive(!(cityID >= MAX_CITY_ID || StageState.LOCKED == DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState(cityID+1, currCopyType) ));
		leftButton.gameObject.SetActive( !(cityID <= 1 || StageState.LOCKED == DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState(cityID-1, currCopyType) ));


		currPickedCityInfo = DataCenter.Instance.QuestData.GetCityInfo(cityID);
		currentCityName = currPickedCityInfo.cityName;

		if(currPickedCityInfo != null) {
			DestoryStages();

			GenerateStages( currPickedCityInfo.stages, currCopyType );
		}


		SetSceneName (TextCenter.GetText("City_Name_" + currPickedCityInfo.id));
	}
	
	/// <summary>
	/// Gets the access stage list.
	/// Add the whole cleared stage and the first one not cleared to the list
	/// </summary>
	/// <returns>The access stage list.</returns>
//	private List<StageInfo> GetAccessStageList(List<StageInfo> stageInfoList){
//		List<StageInfo> accessStageList = new List<StageInfo>();
//		for (int i = 0; i < stageInfoList.Count; i++){
//			if(stageInfoList[ i ].type == QuestType.E_QUEST_STORY){
//				accessStageList.Add(stageInfoList[ i ]);
//				if (!DataCenter.Instance.QuestData.QuestClearInfo.IsStoryStageClear(stageInfoList[ i ]))
//					break;					
//			}
//			else{
//				Debug.LogError("Error: invalid quest type:" + stageInfoList[i].type);
//				break;
//			}
//		}
//		return accessStageList;
//	}
	
	/// <summary>
	/// Generates the stage page.
	/// </summary>
	/// <param name="count">Count.</param>
	private void GenerateStages(List<StageInfo> accessStageList, ECopyType currCopyType){
		background = FindChild<UITexture>("Background");
		ResourceManager.Instance.LoadLocalAsset("Stage/1" /*+ currPickedCityInfo.ID*/, o =>{
			background.mainTexture = o as Texture2D;
		});

		storyStageList.Clear ();


//		Debug.LogWarning("currCopyType:"+currCopyType+" toggle.name:"+( toggle != null ? toggle.name:" NULL"));
		bool searchFarthestArrivedStageSucceed = false;
		for (int i = 0; i < accessStageList.Count; i++){
			GameObject cell = NGUITools.AddChild(storyStageRoot, StageItemView.Prefab);
			cell.name = i.ToString();
			StageItemView stageItemView = StageItemView.Inject(cell);

			stageItemView.CopyType = currCopyType;
			accessStageList[ i ].CopyType = currCopyType;
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
//	void EvolveStartQuest (object data) {
//		evolveStageInfo = data as UnitDataModel;
//		GetData(evolveStageInfo.StageInfo.cityId);
//		FillViewEvolve();
//
//		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST);
//		SetSceneName (TextCenter.GetText ("SCENE_NAME_EVOLVETAGE"));
//	}

//	void FillViewEvolve(){
//		if(currPickedCityInfo == null) {
//			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
//			return;
//		}
//
//		foreach (var item in storyStageList) {
//			if(item.Data.Equals(evolveStageInfo.StageInfo)) {
//				item.evolveCallback = ClickEvolve;
//				item.ShowIconByState(StageState.NEW);
//				continue;
//			}
//			else{
//				item.ShowIconByState(StageState.LOCKED);
////				UIEventListenerCustom listener = item.GetComponent<UIEventListenerCustom>();
////				listener.onClick = null;
////				Destroy(listener);
//			}
//		}	
//	}
//
//	void ClickEvolve() {
//		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectStage, evolveStageInfo);
//	}
//
//
	public GameObject GetStageNewItem(){
		if(storyStageRoot != null)
		foreach(var i in storyStageRoot.transform.GetComponentsInChildren<StageItemView>()){
			if(DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(i.Data.id, i.Data.CopyType) == StageState.NEW){
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

		ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"stage", currentCityName);

//		GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
//		obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (name);
	}
}





