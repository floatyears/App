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

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, false);

		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_PARTY || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_LEVEL_UP || NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.UNIT_EVOLVE) {
			UIManager.Instance.ChangeScene(SceneEnum.Units);	
		}

	}

	public override void HideUI(){
		base.HideUI();
//		Debug.LogError("HideUI");
		MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
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
			if(cityItem == null){
				Debug.LogError(string.Format("Resoures ERROR :: InitWorldMap(), Index[ {0} ] Not Found....!!!", i));
				continue;
			}
			UIEventListener.Get(cityItem).onPress = PressStoryDoor;
			cityViewInfo.Add(cityItem, data[ i ]);
		}

		eventRoot = transform.FindChild("EventDoor").gameObject;
		UIEventListener.Get(eventRoot).onPress = PressEventDoor;
	}


	/// <summary>
	/// Shows the city sprite and name.
	/// </summary>
	private void ShowCityView(){
		if(cityViewInfo == null){
			Debug.LogError("QuestView.InitWorldMap(), cityViewInfo is NULL"); 
			return;
		}

		foreach (var item in cityViewInfo){
			UISprite bgSpr = item.Key.transform.FindChild("Background").GetComponent<UISprite>();
			bgSpr.enabled = false;
		}
	}

	/// <summary>
	/// change scene to quest select with picked cityInfo
	/// </summary>
	/// <param name="item">Item.</param>
	private void PressStoryDoor(GameObject item, bool isPressed){
		UISprite bgSpr = item.transform.FindChild("Background").GetComponent<UISprite>();
		bgSpr.enabled = isPressed;
		if(!isPressed){
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
			UIManager.Instance.ChangeScene(SceneEnum.StageSelect);
			MsgCenter.Instance.Invoke(CommandEnum.OnPickStoryCity, cityViewInfo[ item ].ID);
			//Debug.Log("CityID is : " + cityViewInfo[ item ].ID) ;
		}
	}

	private void PressEventDoor(GameObject item, bool isPressed){
		if(!isPressed){
			Debug.Log("PressEventDoor()...");
			AudioManager.Instance.PlayAudio(AudioEnum.sound_click);
			UIManager.Instance.ChangeScene(SceneEnum.StageSelect);
			MsgCenter.Instance.Invoke(CommandEnum.OnPickEventCity, null);
		}
	}

}

