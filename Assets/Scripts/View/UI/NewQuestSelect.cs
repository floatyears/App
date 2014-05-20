using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewQuestSelect : UIComponentUnity {
	private DragPanel dragPanel;

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetQuestInfo, GetQuestInfo);
	}

	private TStageInfo pickedStage;
	private void GetQuestInfo(object msg){
		TStageInfo newPickedStage = msg as TStageInfo;

		if(newPickedStage.Equals(pickedStage)){
			Debug.Log("PickedStageInfo do not be changed, return!");
			return;
		}
		else if(pickedStage == null){
			//first create this ui, data is null,need to create quest list ui 
			GenerateQuestList(newPickedStage);
			//store stageInfo
			pickedStage = newPickedStage;
		}
		else{
			//pick new stage, so change the stageInfo
			//first destory old ui
			dragPanel.DestoryUI();
			Debug.Log("After destory quest list, count is : " + dragPanel.ScrollItem.Count);

			//then create new ui
			GenerateQuestList(newPickedStage);
			pickedStage = newPickedStage;
		}
	}

	private void GenerateQuestList(TStageInfo targetStage){
		//Debug.Log("QuestSelect.GenerateQuestList(), Start...");
		List<TQuestInfo> accessQuestList = GetAccessQuest(targetStage.QuestInfo);

		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(accessQuestList.Count);
		CustomDragPanel();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);

		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			QuestItemView qiv = QuestItemView.Inject(dragPanel.ScrollItem[ i ]);
			qiv.Data = accessQuestList[ i ];
			qiv.stageInfo = targetStage;
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

	/// <summary>
	/// Gets the access quest list.
	/// Add the whole cleared quest and the first one not cleared to the list
	/// </summary>
	/// <returns>The access quest list.</returns>
	private List<TQuestInfo> GetAccessQuest(List<TQuestInfo> questInfoList){
		List<TQuestInfo> accessQuestList = new List<TQuestInfo>();
		for (int i = 0; i < questInfoList.Count; i++){
			accessQuestList.Add(questInfoList[ i ]);
			if (!DataCenter.Instance.QuestClearInfo.IsStoryStageClear(questInfoList[i].ID)){
				break;					
			}
		}
		Debug.Log("GetAccessStageList(), accessStageList count is : " + accessQuestList.Count);
		return accessQuestList;
	}

}
