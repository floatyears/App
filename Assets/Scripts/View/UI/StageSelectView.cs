using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class StageSelectView : UIComponentUnity{
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
	
	private GameObject eventStageRoot;

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		storyStageRoot = transform.FindChild("StoryStages").gameObject;
		eventStageRoot = transform.FindChild("EventStages").gameObject;
	}
	
	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener (CommandEnum.EvolveStart, EvolveStartQuest);
		MsgCenter.Instance.AddListener(CommandEnum.OnPickStoryCity, ShowStoryCityView);
		MsgCenter.Instance.AddListener(CommandEnum.OnPickEventCity, ShowEventCityView);
	}

	private void ShowEventCityView(object data){
		List<TStageInfo> eventStageList = DataCenter.Instance.EventStageList;
		if(eventStageList == null){
			Debug.LogError("DataCenter.Instance.EventStageList == NULL, return...");
			return;
		}
		Debug.Log("Now event stage count is :" + eventStageList.Count);

		storyStageRoot.gameObject.SetActive(false);
		eventStageRoot.gameObject.SetActive(true);

		for (int i = 0; i < eventStageRoot.transform.childCount; i++){
			Destroy(eventStageRoot.transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < eventStageList.Count; i++){
			GameObject cell = NGUITools.AddChild(storyStageRoot, StageItemView.Prefab);
			cell.name = i.ToString();
			StageItemView stageItemView = StageItemView.Inject(cell);
		}
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
			Debug.LogError("received : " + received);
			DestoryStages();
			FillView();
		}
		else if(!currPickedCityInfo.Equals(received)){
			//when picked city changed
			Debug.Log("recorded picked cityInfo is changed, update stage view...");
			currPickedCityInfo = received;
			DestoryStages();
			FillView();
		}
		else{
			//when picked city not changed
			Debug.Log("recorded picked cityInfo is not changed, keep stage view...");
		}
	}
	
	private void ShowStoryCityView(object msg){
		storyStageRoot.gameObject.SetActive(true);
		eventStageRoot.gameObject.SetActive(false);
		GetData((uint)msg);
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
		ResourceManager.Instance.LoadLocalAsset("Stage/" + currPickedCityInfo.ID, o =>{
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
		evolveStageInfo = data as TEvolveStart;
		GetData(evolveStageInfo.StageInfo.CityId);
		FillViewEvolve();
	}

	void FillViewEvolve(){
		if(currPickedCityInfo == null) {
			Debug.LogError("CreateSlidePageView(), cityInfo is NULL!");
			return;
		}

		foreach (var item in storyStageList) {
			if(item.Data.Equals(evolveStageInfo.StageInfo)) {
				item.evolveCallback = ClickEvolve;
				continue;
			}
			else{
				UIEventListener listener = item.GetComponent<UIEventListener>();
				listener.onClick = null;
				Destroy(listener);
			}
		}	
	}

	void ClickEvolve() {
		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectStage, evolveStageInfo);
	}
	
}





