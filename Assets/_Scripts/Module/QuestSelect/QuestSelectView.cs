using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : ViewBase {
	private DragPanel dragPanel;
	private QuestRewardItemView questRewardItem;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		dragPanel = new DragPanel("QuestSelectDragPanel", "Prefabs/UI/Quest/QuestItemPrefab",typeof(QuestItemView), transform);

		GameObject rewardObj = ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Quest/QuestRewardItem", null) as GameObject;
		rewardObj = Instantiate(rewardObj) as GameObject;
		dragPanel.AddItemToGrid(rewardObj, 5);

		questRewardItem = rewardObj.GetComponent<QuestRewardItemView>();

//		questRewardItem = new QuestRewardItemView();
//		dragPanel.AddItemToGrid(questRewardItem.gameObject, 5);

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
				GetAccessQuest(newQuestList,accessQuestList, newPickedStage.CopyType);
				dragPanel.SetData<QuestInfo> (accessQuestList, pickedStage);

				questRewardItem.SetData(pickedStage);
			} 
			NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST_SELECT);
		}

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
			

		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}

	private StageInfo pickedStage;
	private List<QuestInfo> accessQuestList;

	private void GetAccessQuest(List<QuestInfo> questInfoList, List<QuestInfo> newList, ECopyType copyType){
		newList.Clear ();
		bool isLocked = false;
		for (int i = 0; i < questInfoList.Count; i++){
			newList.Add(questInfoList[ i ]);

			bool isClear = false;
			if(pickedStage.type == QuestType.E_QUEST_STORY){
				isClear = DataCenter.Instance.QuestData.QuestClearInfo.IsStoryQuestClear(pickedStage.id, questInfoList[ i ].id, copyType);
			}
			else if(pickedStage.type == QuestType.E_QUEST_EVENT){
				isClear = DataCenter.Instance.QuestData.QuestClearInfo.IsEventQuestClear(pickedStage.id, questInfoList[ i ].id);
			}

			if( isClear )
				questInfoList[ i ].state = EQuestState.QS_CLEARED;
			else if (!isLocked) {
				questInfoList[ i ].state = EQuestState.QS_QUESTING;
				isLocked = true;
			}else{
				questInfoList[ i ].state = EQuestState.QS_NEW; // it means Locked.
			}
		}

		Debug.Log("GetAccessStageList(), accessStageList count is : " + newList.Count);
	}
	

}
