using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class QuestView : UIComponentUnity{
	private GameObject storyRoot;
	private GameObject eventRoot;
	private GameObject dragItemPrefab;
	private DragPanel storyDragPanel;
	private DragPanel eventDragPanel;
	private Dictionary< GameObject, VStageItemInfo> stageInfo = new Dictionary<GameObject, VStageItemInfo>();

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

