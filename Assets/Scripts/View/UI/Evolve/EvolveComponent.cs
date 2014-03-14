using UnityEngine;
using System.Collections.Generic;

public class EvolveComponent : ConcreteComponent {

	public EvolveComponent(string uiName):base(uiName) {}
	
	public override void CreatUI () {
		base.CreatUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.SelectUnitBase, SelectUnit);
		MsgCenter.Instance.AddListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.SelectUnitBase, SelectUnit);
		MsgCenter.Instance.AddListener (CommandEnum.selectUnitMaterial, selectUnitMaterial);
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	//================================================================================
	private Dictionary<string, object> TransferData = new Dictionary<string, object> ();

	void selectUnitMaterial(object data) {
		if (data == null) {
			return;	
		}

		TransferData.Clear ();
		TransferData.Add(EvolveDecoratorUnity.MaterialData, data);

		ExcuteCallback (TransferData);
	}

	void SelectUnit (object data) {
		if (data == null) {
			return;	
		}
		TransferData.Clear ();
		TransferData.Add(EvolveDecoratorUnity.BaseData, data);

		ExcuteCallback (TransferData);
	}

	uint GetEvolveStageID (bbproto.EUnitType unitType, int  unitRare) {
		uint stageID = 0;
		if (unitRare > 6) {
			return stageID;	
		}

		switch (unitType) {
		case bbproto.EUnitType.UWIND :
			stageID = 1;
			break;
		case bbproto.EUnitType.UFIRE : 
			stageID = 2;
			break;
		case bbproto.EUnitType.UWATER:
			stageID = 3;
			break;
		case bbproto.EUnitType.ULIGHT:
			stageID = 4;
			break;
		case bbproto.EUnitType.UDARK:
			stageID = 5;
			break;
		case bbproto.EUnitType.UNONE:
			stageID = 6;
			break;
		default:
			stageID = 0;
			break;
		}

		return stageID;
	}

	uint GetEvolveQuestID (int  unitRare,uint stageID) {
		uint questID = 0;
		if (unitRare > 6) {
			return questID;	
		}

		switch (unitRare) {
		case 1:
			questID = 1;
			break;
		case 2:
			questID =  1;
			break;
		case 3:
			questID =  2;
			break;
		case 4:
			questID =  3;
			break;
		case 5:
			questID =  4;
			break;
		case 6:
			questID =  5;
			break;
		default:
			return 0;
			break;
		}
		questID += stageID * 100 + questID;
		return questID;

	}

//	func GetEvolveQuestId(unitType bbproto.EUnitType, unitRare int32) (stageId, questId uint32) {
//		if unitRare > consts.N_MAX_RARE {
//			return 0, 0
//		}
//		
//		switch unitType {
//		case bbproto.EUnitType_UWIND:
//			stageId = 1
//				case bbproto.EUnitType_UFIRE:
//				stageId = 2
//					case bbproto.EUnitType_UWATER:
//					stageId = 3
//					case bbproto.EUnitType_ULIGHT:
//					stageId = 4
//					case bbproto.EUnitType_UDARK:
//					stageId = 5
//					case bbproto.EUnitType_UNONE:
//					stageId = 6
//					default:
//					return 0, 0
//		}
//		
//		stageId += 20
//			
//			baseQuestId := uint32(1)
//			switch unitRare {
//				case 1:
//				questId = baseQuestId + 0
//				case 2:
//				questId = baseQuestId + 1
//				case 3:
//				questId = baseQuestId + 2
//				case 4:
//				questId = baseQuestId + 3
//				case 5:
//				questId = baseQuestId + 4
//				case 6:
//				questId = baseQuestId + 5
//				default:
//				return 0, 0
//			}
//		
//		questId += stageId*100 + questId
//			
//			return stageId, questId
//	}
}
