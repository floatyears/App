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

		if (viewData != null) {
			StageInfo newPickedStage = viewData["data"] as StageInfo;
			List<QuestInfo> newQuestList = newPickedStage.QuestInfo;
//			newQuestList.Reverse ();
			
			if(accessQuestList == null){
				accessQuestList = new List<QuestInfo>();
			}
			if(!accessQuestList.Equals(newQuestList)){
				pickedStage = newPickedStage;
				GetAccessQuest(newQuestList,accessQuestList);
				dragPanel.SetData<QuestInfo> (accessQuestList, pickedStage);
			} 
		}

		NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);

		if (pickedStage != null) {
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"stage",pickedStage.stageName);
		}
	}

	protected override void ToggleAnimation (bool isShow)
	{
		if (isShow) {
			gameObject.SetActive(true);

			transform.localPosition = new Vector3(-1000, 0, 0);
			iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  
			
			NoviceGuideStepEntityManager.Instance ().StartStep (NoviceGuideStartType.QUEST);
		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}

	private StageInfo pickedStage;
	private List<QuestInfo> accessQuestList;

	private void GetAccessQuest(List<QuestInfo> questInfoList, List<QuestInfo> newList){
		newList.Clear ();
		bool isLocked = false;
		for (int i = 0; i < questInfoList.Count; i++){
			newList.Add(questInfoList[ i ]);

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

		Debug.Log("GetAccessStageList(), accessStageList count is : " + newList.Count);
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
	
}
