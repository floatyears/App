using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : UIComponentUnity {
	private DragPanel dragPanel;

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
		ShowUIAnimation();

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (pickedStage != null) {
			GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
			obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (pickedStage.StageName);
		}
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
	}
	
	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);
	}
	
	private TStageInfo pickedStage;
	private List<TQuestInfo> accessQuestList;

	private void GetQuestInfo(object msg){
		TStageInfo newPickedStage = msg as TStageInfo;
		List<TQuestInfo> newQuestList = newPickedStage.QuestInfo;
//		newQuestList.Reverse ();

		if(accessQuestList == null){
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList is NULL as FRIST step in, CREATE list view...");
			pickedStage = newPickedStage;
			accessQuestList = GetAccessQuest(newQuestList);
			UpdateQuestListView();
		} else if(!accessQuestList.Equals(newQuestList)){
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList CHANGED, UPDATE prev list view...");
			pickedStage = newPickedStage;
			accessQuestList = GetAccessQuest(newQuestList);
			dragPanel.DestoryUI();
			UpdateQuestListView();
		} else{
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList NOT CHANGED, KEEP prev list view...");
		}

		GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
		obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (newPickedStage.StageName);
	}

	private void UpdateQuestListView(){
		if (dragPanel != null)
			dragPanel.DestoryUI ();

		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab);
		dragPanel.CreatUI();
		int dataCount = accessQuestList.Count;
		dragPanel.AddItem (dataCount);
		CustomDragPanel();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.QuestSelectDragPanelArgs, transform);
		dataCount--;
		for (int i = dragPanel.ScrollItem.Count - 1; i >=0 ; i--){
			QuestItemView questItemView = QuestItemView.Inject(dragPanel.ScrollItem[ i ]);
			//do before, store questInfo's stageInfo 
			questItemView.stageInfo = pickedStage;
			//do after, because stageInfo's refresh don't bind with questInfo's
			questItemView.Data = accessQuestList[ dataCount - i ];
		}
	}

	private void CustomDragPanel(){
		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		
		scrollBar.transform.Rotate(new Vector3(0, 0, 270));
		
		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();
		
		uiScrollView.verticalScrollBar = uiScrollBar;
		uiScrollView.horizontalScrollBar = null	;	
	}
	
	private List<TQuestInfo> GetAccessQuest(List<TQuestInfo> questInfoList){
		List<TQuestInfo> accessQuestList = new List<TQuestInfo>();
		for (int i = 0; i < questInfoList.Count; i++){
			accessQuestList.Add(questInfoList[ i ]);

////			Debug.Log("QuestSelectView, stageID = " + pickedStage.ID 
//			          + ", questID = " + questInfoList[ i ].ID 
//			          + ", isClear = " + DataCenter.Instance.QuestClearInfo.IsStoryQuestClear(pickedStage.ID, questInfoList[ i ].ID));

			if(!CheckQuestIsClear(pickedStage, questInfoList[ i ].ID)) break;

		}
		Debug.Log("GetAccessStageList(), accessStageList count is : " + accessQuestList.Count);
		return accessQuestList;
	}
	
	private bool CheckQuestIsClear(TStageInfo stageInfo, uint questID){
		if(stageInfo.Type == QuestType.E_QUEST_STORY){
			return DataCenter.Instance.QuestClearInfo.IsStoryQuestClear(stageInfo.ID, questID);
		}
		else if(stageInfo.Type == QuestType.E_QUEST_EVENT){
			return DataCenter.Instance.QuestClearInfo.IsEventQuestClear(stageInfo.ID, questID);
		}
		else{
			Debug.LogError("Exception :: CheckQuestIsClear().");
			return false;
		}
	}

	TEvolveStart evolveStart;
	private List<QuestItemView> questItem = new List<QuestItemView>();

	void EvolveSelectStage(object data) {
		evolveStart = data as TEvolveStart;

		GenerateQuest(evolveStart.StageInfo.QuestInfo, evolveStart.StageInfo);
		
		foreach (var item in questItem) {
//			Debug.LogError("item.Data.ID : " + item.Data.ID + " evolveStart.StageInfo.QuestId : " + evolveStart.StageInfo.QuestId);
			if(item.Data.ID == evolveStart.StageInfo.QuestId) {
				item.evolveCallback = EvolveCallback;
				continue;
			} else {
				UIEventListener listener = item.GetComponent<UIEventListener>();
				listener.onClick = null;
				Destroy(listener);
			}
		}
	}

	void GenerateQuest(List<TQuestInfo> questInfo, TStageInfo targetStage) {
		if (dragPanel != null)
			dragPanel.DestoryUI ();
		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(1);
		TQuestInfo quest = questInfo.Find (a => a.ID == targetStage.QuestId);
		CustomDragPanel();

		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);
		questItem.Clear ();
		if (quest == default(TQuestInfo)) {
			return;	
		}
		QuestItemView qiv = QuestItemView.Inject (dragPanel.ScrollItem [0]);
		qiv.Data = quest;
		qiv.stageInfo = targetStage;
		questItem.Add(qiv);
//		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
//			QuestItemView qiv = QuestItemView.Inject(dragPanel.ScrollItem[ i ]);
//			qiv.Data = questInfo[ i ];
//			qiv.stageInfo = targetStage;
//			//			qiv.StageID = targetStage.ID;//StartFight Need
//			questItem.Add(qiv);
//		}
	}

	void EvolveCallback() {
		UIManager.Instance.ChangeScene(SceneEnum.FightReady);//before
		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectQuest, evolveStart);
	}

	public GameObject GetDragItem(int i){
		if (dragPanel != null) {
			return dragPanel.ScrollItem [i];
		}

		return null;
	}
}
