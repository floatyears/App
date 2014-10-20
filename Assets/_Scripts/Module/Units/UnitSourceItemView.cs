using UnityEngine;
using System.Collections;
using bbproto;

public class UnitSourceItemView : DragPanelItemBase {

	private UnitGetWay data;

	private GameObject goBtn;
	private UILabel nameLabel;
	private UILabel infoLabel1;
	private UILabel infoLabel2;

	private object goData;

	void Init(){
		goBtn = transform.FindChild ("GoBtn").gameObject;
		UIEventListenerCustom.Get (goBtn).onClick = GotoGet;
		nameLabel = transform.FindChild ("Lable_Name").GetComponent<UILabel> ();
		infoLabel1 = transform.FindChild ("Lable_Info1").GetComponent<UILabel> ();
		infoLabel2 = transform.FindChild ("Lable_Info2").GetComponent<UILabel> ();
	}

	public override void ItemCallback (params object[] args)
	{
		throw new System.NotImplementedException ();
	}

	public override void SetData<T> (T d, params object[] args)
	{
		data = d as UnitGetWay;
		switch (data.getType) {
		case  EUnitGetType.E_NORMAL_QUEST:
		case EUnitGetType.E_EVENT_QUEST:
		case EUnitGetType.E_STAGE:
			uint questId = data.getPath;
			uint stageId =  questId/10;
			if ( data.getType == EUnitGetType.E_STAGE ) {
				stageId = data.getPath;
			}					
			uint cityId = stageId/10;
			if ( cityId>0 && stageId>0 ) {
				CityInfo cityInfo = DataCenter.Instance.QuestData.GetCityInfo(cityId);
				StageInfo stageInfo = DataCenter.Instance.QuestData.GetStageInfo(stageId);
				if ( cityInfo!=null && stageInfo!= null) {
					goData = stageInfo;
					nameLabel.text = cityInfo.cityName + "-" + stageInfo.stageName ;
				}
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
			break;
		case EUnitGetType.E_STAGE:
		case  EUnitGetType.E_NORMAL_QUEST:
			ModuleManager.Instance.ShowModule(ModuleEnum.QuestSelectModule,"stage",goData);
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
