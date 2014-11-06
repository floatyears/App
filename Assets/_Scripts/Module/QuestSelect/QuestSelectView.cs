using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class QuestSelectView : ViewBase {
	private DragPanel dragPanel;
	private QuestRewardItemView questRewardItem;
	private int currType = -1;
	StageInfo currStage;
	bool isAllLocked = false;

	public override void Init (UIConfigItem uiconfig, Dictionary<string, object> data)
	{
		base.Init (uiconfig, data);
		dragPanel = new DragPanel("QuestSelectDragPanel", "Prefabs/UI/Quest/QuestItemPrefab",typeof(QuestItemView), transform);

		GameObject rewardObj = ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Quest/QuestRewardItem", null) as GameObject;
		rewardObj = Instantiate(rewardObj) as GameObject;
		dragPanel.AddItemToGrid(rewardObj, 5);

		questRewardItem = rewardObj.GetComponent<QuestRewardItemView>();

		UILabel normalText = FindChild<UILabel>("CopyType/Normal/text");
		UILabel eliteText = FindChild<UILabel>("CopyType/Elite/text");
		normalText.text = TextCenter.GetText("CopyNormal");
		eliteText.text = TextCenter.GetText("CopyElite");

	}

	public override void ShowUI(){
		base.ShowUI();

		if (viewData != null) {
			currStage = viewData["data"] as StageInfo;

//			newQuestList.Reverse ();

			UIToggle normal = FindChild<UIToggle>("CopyType/Normal");
			UIToggle elite = FindChild<UIToggle>("CopyType/Elite");
			if(currStage.CopyType == ECopyType.CT_ELITE){
				elite.SendMessage("OnClick");
			}else if(currStage.CopyType == ECopyType.CT_NORMAL && UIToggle.GetActiveToggle (7).name != "Normal"){
				normal.SendMessage("OnClick");
			}

			ShowQuestList(currStage);

			NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST_SELECT);
		}

	}

	public override void CallbackView (params object[] args)
	{
		if (args.Length > 0) {
			currStage = DataCenter.Instance.QuestData.GetStageInfo((uint)args[0]);
			if(args.Length > 1)
				currStage.CopyType = (ECopyType)args[1];
			UIToggle normal = FindChild<UIToggle>("CopyType/Normal");
			UIToggle elite = FindChild<UIToggle>("CopyType/Elite");
			if(currStage.CopyType == ECopyType.CT_ELITE){
				elite.SendMessage("OnClick");
			}else{
				normal.SendMessage("OnClick");
			}
			//			newQuestList.Reverse ();
			
			ShowQuestList(currStage);
			
//			NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.QUEST_SELECT);
		}
	}

	void ShowQuestList(StageInfo newPickedStage) {


		List<QuestInfo> newQuestList = newPickedStage.QuestInfo;

		if(accessQuestList == null){
			accessQuestList = new List<QuestInfo>();
		}
		if(!accessQuestList.Equals(newQuestList)){
			pickedStage = newPickedStage;
			GetAccessQuest(newQuestList,accessQuestList, newPickedStage.CopyType);
			dragPanel.SetData<QuestInfo> (accessQuestList, pickedStage);
			
			questRewardItem.SetData(pickedStage);
		} 

		if (pickedStage != null) {
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"stage",pickedStage.stageName);
		}

		if (isAllLocked) {
			if((ECopyType)currType == ECopyType.CT_ELITE){
				uint prev = DataCenter.Instance.QuestData.QuestClearInfo.prevStageId(currStage.id);
				if( prev == 0 || DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState(prev,ECopyType.CT_ELITE) == StageState.CLEAR){
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Quest_Limit"),TextCenter.GetText("Quest_NormalNeed"),TextCenter.GetText("OK"),o=>{
//						StageInfo temp = DataCenter.Instance.QuestData.GetStageInfo();
//						temp.CopyType = ECopyType.CT_NORMAL;
						ModuleManager.SendMessage(ModuleEnum.QuestSelectModule,currStage.id,ECopyType.CT_NORMAL);
					});	
				}else{
					TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Quest_Limit"),TextCenter.GetText("Quest_Need"),TextCenter.GetText("OK"),o=>{
						ModuleManager.SendMessage(ModuleEnum.QuestSelectModule,DataCenter.Instance.QuestData.QuestClearInfo.GetNewestStage(ECopyType.CT_ELITE),ECopyType.CT_ELITE);
					});	
				}

					
			}else{
				TipsManager.Instance.ShowMsgWindow(TextCenter.GetText("Quest_Limit"),TextCenter.GetText("Quest_Need"),TextCenter.GetText("OK"),o=>{
					ModuleManager.SendMessage(ModuleEnum.QuestSelectModule,DataCenter.Instance.QuestData.QuestClearInfo.GetNewestStage(ECopyType.CT_NORMAL));
				});	
			}
		}
	}

	public void OnSelectCopyType(object data) {
		UIToggle toggle = UIToggle.GetActiveToggle (7);
		if( toggle == null ) {
			return;
		}
		ECopyType selectCopyType = (toggle.name == "Normal" ) ? ECopyType.CT_NORMAL : ECopyType.CT_ELITE;
		if ((int)selectCopyType != currType) {
			currType = (int)selectCopyType;
//			uint newestStageId = DataCenter.Instance.QuestData.QuestClearInfo.GetNewestStage( currCopyType );
//			StageInfo newStage = DataCenter.Instance.QuestData.GetStageInfo( newestStageId );
//			newStage.CopyType = currCopyType;

			currStage.CopyType = selectCopyType;
			ShowQuestList( currStage );
			
//			Debug.Log("toggle lastStageID:"+newestStageId + " UIToggle.GetActiveToggle(5) = "+UIToggle.GetActiveToggle (7).name);

			ModuleManager.SendMessage(ModuleEnum.StageSelectModule, "ChangeCopyType", selectCopyType);	
		}

	}

	protected override void ToggleAnimation (bool isShow)
	{

		if (isShow) {
			gameObject.SetActive(true);

			transform.localPosition = new Vector3(config.localPosition.x, config.localPosition.y, 0);
//			iTween.MoveTo(gameObject, iTween.Hash("x", config.localPosition.x, "time", 0.4f));  
			

		}else{
			transform.localPosition = new Vector3(-1000, config.localPosition.y, 0);	
			gameObject.SetActive(false);
		}
	}

	private StageInfo pickedStage;
	private List<QuestInfo> accessQuestList;

	private void GetAccessQuest(List<QuestInfo> questInfoList, List<QuestInfo> newList, ECopyType copyType){
		newList.Clear ();
		StageState state = StageState.NONE;
		isAllLocked = false;
		for (int i = 0; i < questInfoList.Count; i++){
			newList.Add(questInfoList[ i ]);

			bool isClear = false;
			if(pickedStage.type == QuestType.E_QUEST_STORY){
				state = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryQuestState(pickedStage.id, questInfoList[ i ].id, copyType);
			}
			else if(pickedStage.type == QuestType.E_QUEST_EVENT){
				state = DataCenter.Instance.QuestData.QuestClearInfo.IsEventQuestClear(pickedStage.id, questInfoList[ i ].id);
			}

//			isLocked = DataCenter.Instance.QuestData.QuestClearInfo.stage
			if( state == StageState.CLEAR )
				questInfoList[ i ].state = EQuestState.QS_CLEARED;
			else if (state == StageState.NEW) {
				questInfoList[ i ].state = EQuestState.QS_QUESTING;
			}else if(state == StageState.LOCKED){
				questInfoList[ i ].state = EQuestState.QS_NEW; // it means Locked.
			}
			if(i == 0 && state == StageState.LOCKED){
				isAllLocked = true;
			}
		}

		Debug.Log("GetAccessStageList(), accessStageList count is : " + newList.Count);
	}
	

}
