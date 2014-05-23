using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewQuestSelect : UIComponentUnity {
	private DragPanel dragPanel;

	private TEvolveStart evolveStart;
	private List<QuestItemView> questItem = new List<QuestItemView>();

	public override void ShowUI(){
		base.ShowUI();
		MsgCenter.Instance.AddListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		MsgCenter.Instance.AddListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
	}

	public override void HideUI(){
		base.HideUI();
		MsgCenter.Instance.RemoveListener(CommandEnum.GetQuestInfo, GetQuestInfo);
		MsgCenter.Instance.RemoveListener (CommandEnum.EvolveSelectStage, EvolveSelectStage);
	}

	private void GetQuestInfo(object msg){
		TStageInfo pickedStage = msg as TStageInfo;
		GenerateQuestList(pickedStage);
	}

	void EvolveSelectStage(object data) {
		evolveStart = data as TEvolveStart;
		GenerateQuest(evolveStart.StageInfo.QuestInfo, evolveStart.StageInfo);

		foreach (var item in questItem) {
			Debug.LogError("item.Data.ID : " + item.Data.ID + " evolveStart.StageInfo.QuestId : " + evolveStart.StageInfo.QuestId);
			if(item.Data.ID == evolveStart.StageInfo.QuestId) {
				item.evolveCallback = EvolveCallback;
				continue;
			} else {
				Destroy(item.GetComponent<UIEventListener>());
			}
		}
	}

	void EvolveCallback () {
		UIManager.Instance.ChangeScene(SceneEnum.FightReady);//before
		MsgCenter.Instance.Invoke (CommandEnum.EvolveSelectQuest, evolveStart);
	}

	private void GenerateQuestList(TStageInfo targetStage){
		//Debug.Log("QuestSelect.GenerateQuestList(), Start...");
		List<TQuestInfo> accessQuestList = GetAccessQuest(targetStage.QuestInfo);

		GenerateQuest (accessQuestList, targetStage);
	}

	void GenerateQuest(List<TQuestInfo> questInfo, TStageInfo targetStage) {
		dragPanel = new DragPanel("QuestDragPanel", QuestItemView.Prefab);
		dragPanel.CreatUI();
		dragPanel.AddItem(questInfo.Count);
		CustomDragPanel();
		dragPanel.DragPanelView.SetScrollView(ConfigDragPanel.HelperListDragPanelArgs, transform);
		questItem.Clear ();
		for (int i = 0; i < dragPanel.ScrollItem.Count; i++){
			QuestItemView qiv = QuestItemView.Inject(dragPanel.ScrollItem[ i ]);
			qiv.Data = questInfo[ i ];
			qiv.stageInfo = targetStage;
			//			qiv.StageID = targetStage.ID;//StartFight Need
			questItem.Add(qiv);
		}
	}

	private void CustomDragPanel(){
		GameObject scrollView = dragPanel.DragPanelView.transform.FindChild("Scroll View").gameObject;
		GameObject scrollBar = dragPanel.DragPanelView.transform.FindChild("Scroll Bar").gameObject;
		scrollBar.transform.Rotate( new Vector3(0, 0, 270) );
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
//			if (!DataCenter.Instance.QuestClearInfo.IsStoryStageClear(questInfoList[i].ID)){
//				break;					
//			}
		}
		Debug.Log("GetAccessStageList(), accessStageList count is : " + accessQuestList.Count);
		return accessQuestList;
	}

}
