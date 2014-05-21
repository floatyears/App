using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : UIComponentUnity {
	private DragPanel dragPanel;

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		ShowUIAnimation();
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetQuestInfo, GetQuestInfo);
	}

	private void ShowUIAnimation(){
		gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
		iTween.MoveTo(gameObject, iTween.Hash("x", 0, "time", 0.4f));  
	}
	
	private TStageInfo pickedStage;
	private List<TQuestInfo> accessQuestList;

	private void GetQuestInfo(object msg){
		TStageInfo newPickedStage = msg as TStageInfo;
		List<TQuestInfo> newQuestList = newPickedStage.QuestInfo;

		if(accessQuestList == null){
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList is NULL as FRIST step in, CREATE list view...");
			pickedStage = newPickedStage;
			accessQuestList = GetAccessQuest(newQuestList);
			UpdateQuestListView();
		}
		else if(!accessQuestList.Equals(newQuestList)){
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList CHANGED, UPDATE prev list view...");
			pickedStage = newPickedStage;
			accessQuestList = GetAccessQuest(newQuestList);
			dragPanel.DestoryUI();
			UpdateQuestListView();
		}
		else{
			Debug.Log("QuestSelectView.GetQuestInfo(), accessQuestList NOT CHANGED, KEEP prev list view...");
		}
	}

	private void UpdateQuestListView(){
		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(accessQuestList.Count);
		CustomDragPanel();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.QuestSelectDragPanelArgs, transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			QuestItemView questItemView = QuestItemView.Inject(dragPanel.ScrollItem[ i ]);
			//do before, store questInfo's stageInfo 
			questItemView.stageInfo = pickedStage;
			//do after, because stageInfo's refresh don't bind with questInfo's
			questItemView.Data = accessQuestList[ i ];
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

			Debug.Log("QuestSelectView, stageID = " + pickedStage.ID 
			          + ", questID = " + questInfoList[ i ].ID 
			          + ", isClear = " + DataCenter.Instance.QuestClearInfo.IsStoryQuestClear(pickedStage.ID, questInfoList[ i ].ID));

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

}
