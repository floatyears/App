using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : ViewBase {
	private DragPanel dragPanel;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		dragPanel = new DragPanel("QuestSelectDragPanel", "Prefabs/UI/Quest/QuestItemPrefab",typeof(QuestItemView), transform);
	}

	public override void ShowUI(){
		base.ShowUI();
//		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);

		if (viewData != null) {
			GetQuestInfo();
		}

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

		dragPanel.SetData<QuestInfo> (accessQuestList, pickedStage);
	}
	
	private List<QuestInfo> GetAccessQuest(List<QuestInfo> questInfoList){
		List<QuestInfo> accessQuestList = new List<QuestInfo>();
		bool isLocked = false;
		for (int i = 0; i < questInfoList.Count; i++){
			accessQuestList.Add(questInfoList[ i ]);

			bool isClear = CheckQuestIsClear(pickedStage, questInfoList[ i ].id);
			if( isClear )
				questInfoList[ i ].state = EQuestState.QS_CLEARED;
			else if (!isLocked) {
				questInfoList[ i ].state = EQuestState.QS_QUESTING;
				isLocked = true;
			}else{
				questInfoList[ i ].state = EQuestState.QS_NEW;
			}
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
