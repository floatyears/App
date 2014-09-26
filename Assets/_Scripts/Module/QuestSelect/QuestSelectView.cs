using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : ViewBase {
	private DragPanel dragPanel;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);

	}

	public override void ShowUI(){
		base.ShowUI();
//		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);

		if (viewData != null) {
			GetQuestInfo();
		}

		MsgCenter.Instance.AddListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
		ShowUIAnimation();

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (pickedStage != null) {
//			GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
//			obj.GetComponent<SceneInfoBarView> ().SetSceneName ();
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"stage",pickedStage.stageName);
		}
	}

	public override void HideUI(){
		base.HideUI();
//		MsgCenter.Instance.RemoveListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
	}
	
	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);
	}
	
	private StageInfo pickedStage;
	private List<QuestInfo> accessQuestList;

	private void GetQuestInfo(){
		StageInfo newPickedStage = viewData["data"] as StageInfo;
		List<QuestInfo> newQuestList = newPickedStage.QuestInfo;
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
			if (dragPanel != null)
				dragPanel.DestoryUI();
			UpdateQuestListView();
		} else{
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList NOT CHANGED, KEEP prev list view...");
		}

//		GameObject obj = GameObject.Find ("SceneInfoBar(Clone)");
//		obj.GetComponent<SceneInfoDecoratorUnity> ().SetSceneName (newPickedStage.StageName);
	}

	private void UpdateQuestListView(){
		if (dragPanel != null)
			dragPanel.DestoryUI ();

		dragPanel = new DragPanel("QuestSelectDragPanel", QuestItemView.Prefab,transform);
//		dragPanel.CreatUI();
		int dataCount = accessQuestList.Count;
		dragPanel.AddItem (dataCount);
		CustomDragPanel();
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
//		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
//		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		
//		scrollBar.transform.Rotate(new Vector3(0, 0, 270));
//		
//		UIScrollView uiScrollView = scrollView.GetComponent<UIScrollView>();
//		UIScrollBar uiScrollBar = scrollBar.GetComponent<UIScrollBar>();
//		
//		uiScrollView.verticalScrollBar = uiScrollBar;
//		uiScrollView.horizontalScrollBar = null	;	
	}
	
	private List<QuestInfo> GetAccessQuest(List<QuestInfo> questInfoList){
		List<QuestInfo> accessQuestList = new List<QuestInfo>();
		for (int i = 0; i < questInfoList.Count; i++){
			accessQuestList.Add(questInfoList[ i ]);

			if(!CheckQuestIsClear(pickedStage, questInfoList[ i ].id)) 
				break;
		}
		Debug.Log("GetAccessStageList(), accessStageList count is : " + accessQuestList.Count);
		return accessQuestList;
	}
	
	private bool CheckQuestIsClear(StageInfo stageInfo, uint questID){
		if(stageInfo.type == QuestType.E_QUEST_STORY){
			return DataCenter.Instance.QuestData.QuestClearInfo.IsStoryQuestClear(stageInfo.id, questID);
		}
		else if(stageInfo.type == QuestType.E_QUEST_EVENT){
			return DataCenter.Instance.QuestData.QuestClearInfo.IsEventQuestClear(stageInfo.id, questID);
		}
		else{
			Debug.LogError("Exception :: CheckQuestIsClear().");
			return false;
		}
	}

	UnitDataModel evolveStart;
	private List<QuestItemView> questItem = new List<QuestItemView>();

	void EvolveSelectStage(object data) {
		evolveStart = data as UnitDataModel;

		GenerateQuest(evolveStart.StageInfo.QuestInfo, evolveStart.StageInfo);
		
		foreach (var item in questItem) {
//			Debug.LogError("item.Data.ID : " + item.Data.ID + " evolveStart.StageInfo.QuestId : " + evolveStart.StageInfo.QuestId);
			if(item.Data.id == evolveStart.StageInfo.QuestId) {
				item.evolveCallback = EvolveCallback;
				continue;
			} else {
				UIEventListenerCustom listener = item.GetComponent<UIEventListenerCustom>();
				listener.onClick = null;
				Destroy(listener);
			}
		}
	}

	void GenerateQuest(List<QuestInfo> questInfo, StageInfo targetStage) {
		if (dragPanel != null)
			dragPanel.DestoryUI ();
		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab,transform);
//		dragPanel.CreatUI();
		dragPanel.AddItem(1);
		QuestInfo quest = questInfo.Find (a => a.id == targetStage.QuestId);
		CustomDragPanel();

		questItem.Clear ();
		if (quest == default(QuestInfo)) {
			return;	
		}
		QuestItemView qiv = QuestItemView.Inject (dragPanel.ScrollItem [0]);
		qiv.Data = quest;
		qiv.stageInfo = targetStage;
		questItem.Add(qiv);
	}

	void EvolveCallback() {
		ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule);//before
		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectQuest, evolveStart);
	}

	public GameObject GetDragItem(int i){
		if (dragPanel != null) {
			return dragPanel.ScrollItem [i];
		}

		return null;
	}
}
