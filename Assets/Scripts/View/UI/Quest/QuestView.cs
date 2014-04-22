using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestView : UIComponentUnity{
	private GameObject storyRoot;
	private GameObject eventRoot;
	private GameObject dragItemPrefab;
	private DragPanel storyDragPanel;
	private DragPanel eventDragPanel;

	Dictionary< string, object > storyDragPanelArgsDic = new Dictionary< string, object >();
	Dictionary< string, object > eventDragPanelArgsDic = new Dictionary< string, object >();
	Dictionary< GameObject, VStageItemInfo> stageInfo = new Dictionary<GameObject, VStageItemInfo> ();

	public override void Init(UIInsConfig config, IUICallback origin){
		base.Init(config, origin);
		InitUI();
	}

	public override void ShowUI(){
		base.ShowUI();
		ShowTween();
	}

	public override void HideUI(){
		base.HideUI();
		storyDragPanel.DestoryUI();
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
		storyRoot = FindChild("story_window");
		eventRoot = FindChild("event_window");
//		StylizeStoryPanel(); 
		dragItemPrefab = Resources.Load("Stage/StageDragPanelItem") as GameObject;
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
//		Debug.LogError("GetCount : " + count);
		return count;
	}

	List<Texture2D> GetTextureList(List<TCityInfo> cityList){
		List<Texture2D> tex2dList = new List<Texture2D>();
		for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++){
			List<TStageInfo> stageList = cityList[ cityIndex ].Stages;
			for (int stageIndex = 0; stageIndex < stageList.Count; stageIndex++) {
				//TODO pick new stage
				string sourcePath = string.Format("Stage/{0}_{1}", cityList[ cityIndex ].ID, stageList[  stageIndex ].ID);
				Debug.LogError("texture path : " + sourcePath);
				Texture2D tex2d = Resources.Load(sourcePath) as Texture2D;
				tex2dList.Add(tex2d);
			}//for
		}//for

		Debug.LogError("Tex2dList count is " + tex2dList.Count);
		return tex2dList;
	}

	List<string> GetClearInfoTextList(List<TCityInfo> cityList){
		List<string> textList = new List<string>();
		for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++){
			List<TStageInfo> stageList = cityList[ cityIndex ].Stages;
			for (int stageIndex = 0; stageIndex < stageList.Count; stageIndex++) {
				textList.Add("New");
			}//for
		}//for

		return textList;
	}

	List<string> GetCityNameTextList(List<TCityInfo> cityList){
		List<string> textList = new List<string>();
		for (int cityIndex = 0; cityIndex < cityList.Count; cityIndex++){
			List<TStageInfo> stageList = cityList[ cityIndex ].Stages;
			for (int stageIndex = 0; stageIndex < stageList.Count; stageIndex++) {
				textList.Add(stageList[ stageIndex ].StageName);
			}//for
		}//for
		
		return textList;
	}

	void UpdateTexture(DragPanel panel, List<Texture2D> tex2dList){
		for (int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject item = panel.ScrollItem [i];
			UITexture texture = item.transform.FindChild("Texture").GetComponent<UITexture>();
			texture.mainTexture = tex2dList[ i ];
		}
	}

	void UpdateLabelTop(DragPanel panel, List<string> textList){
		for (int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject item = panel.ScrollItem [i];
			UILabel label = item.transform.FindChild("Label_Top").GetComponent<UILabel>();
			label.text = textList[ i ];
			label.color = Color.green;
		}
	}

	void UpdateLabelBottom(DragPanel panel, List<string> textList){
		for (int i = 0; i < panel.ScrollItem.Count; i++){
			GameObject item = panel.ScrollItem [i];
			UILabel label = item.transform.FindChild("Label_Bottom").GetComponent<UILabel>();
			label.text = textList[ i ];
			label.color = Color.red;
		}
	}

	void ClickStoryItem(GameObject item){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("ClickStoryItem", stageInfo[item].StageInfo);
		ExcuteCallback(cbdArgs);
	}

	void ClickEventItem(GameObject item){}
	
	void ShowTween(){
		TweenPosition[ ] list = 
			gameObject.GetComponentsInChildren< TweenPosition >();
		if (list == null)	return;
		foreach (var tweenPos in list){		
			if (tweenPos == null)	continue;
			tweenPos.Reset();
			tweenPos.PlayForward();
		}
	}

}

