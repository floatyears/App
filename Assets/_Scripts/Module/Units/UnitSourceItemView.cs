using UnityEngine;
using System.Collections;
using bbproto;

public class UnitSourceItemView : DragPanelItemBase {

	private UnitGetWay data;

	private GameObject goBtn;
	private UILabel nameLabel;
	private UILabel infoLabel1;
	private UILabel infoLabel2;

	private object goData1;
	private object goData2;

	void Init(){
		goBtn = transform.FindChild ("GoBtn").gameObject;
		UIEventListenerCustom.Get (goBtn).onClick = GotoGet;
		nameLabel = transform.FindChild ("Lable_Name").GetComponent<UILabel> ();
		infoLabel1 = transform.FindChild ("Label_Info1").GetComponent<UILabel> ();
		infoLabel2 = transform.FindChild ("Label_Info2").GetComponent<UILabel> ();
	}

	public override void ItemCallback (params object[] args)
	{
		throw new System.NotImplementedException ();
	}

	public override void SetData<T> (T d, params object[] args)
	{
		if (nameLabel == null)
			Init ();
		data = d as UnitGetWay;

		switch (data.getType) {
		
		case EUnitGetType.E_EVENT_QUEST:
		case  EUnitGetType.E_NORMAL_QUEST:
		
			//				CityInfo cityInfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
			QuestInfo questInfo = DataCenter.Instance.QuestData.GetQuestInfo(data.getPath);

			if ( questInfo!=null) {
				goData1 = questInfo;
				nameLabel.text = questInfo.name;
				infoLabel1.text = questInfo.stamina + "";
				infoLabel2.text = "";
			}
			break;
		case EUnitGetType.E_STAGE:
			StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(data.getPath);
			if ( stageInfo!=null) {
				goData1 = stageInfo;
				nameLabel.text = stageInfo.stageName;
				infoLabel1.text = "";
				infoLabel2.text = "";
			}
			break;
		case EUnitGetType.E_GACHA_EVENT:
			break;
		case EUnitGetType.E_GACHA_NORMAL:
			break;
		case EUnitGetType.E_BUY:
			break;
		case EUnitGetType.E_BONUS:
			break;
		}

	}

	void GotoGet(GameObject obj){
		switch (data.getType) {
		case EUnitGetType.E_EVENT_QUEST:
			ModuleManager.Instance.ShowModule(ModuleEnum.StageSelectModule,"event");
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			break;
		case EUnitGetType.E_STAGE:
			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"data",goData1 as StageInfo);
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			break;
		case  EUnitGetType.E_NORMAL_QUEST:
			ModuleManager.Instance.ShowModule(ModuleEnum.FightReadyModule,"QuestInfo",goData1 as QuestInfo);
			ModuleManager.SendMessage(ModuleEnum.SceneInfoBarModule,"unti_source");
			break;
		case EUnitGetType.E_GACHA_EVENT:
			break;
		case EUnitGetType.E_GACHA_NORMAL:
			break;
		case EUnitGetType.E_BUY:
			break;
		case EUnitGetType.E_BONUS:
			break;
		}
	}

}
