using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveModule : ModuleBase {
	public EvolveModule(UIConfigItem config):base(  config) {
//		CreateUI<EvolveView> ();
	}
	
	public override void InitUI () {
		base.InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
	}
	
	public override void HideUI () {
		base.HideUI ();
//		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
//			base.DestoryUI();
//		}
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages (params object[] data) {
		List<ProtoBuf.IExtensible> evolveInfoLisst = data[0] as List<ProtoBuf.IExtensible>;
		UserUnit baseItem = evolveInfoLisst [0] as UserUnit ;
		FriendInfo firendItem = evolveInfoLisst [1] as FriendInfo;
		UserUnit tuu = baseItem;
		UnitInfo tui = tuu.UnitInfo;
		CityInfo tci = DataCenter.Instance.QuestData.GetCityInfo (EvolveCityID);
		uint stageID = GetEvolveStageID (tui.type, tui.rare);
		uint questID = GetEvolveQuestID (tui.type, tui.rare);
		List<uint> partyID = new List<uint> ();
		for (int i = 2; i < evolveInfoLisst.Count; i++) {
			UserUnit temp = evolveInfoLisst[i] as UserUnit;
			partyID.Add(temp.uniqueId);
		}

//		EvolveStart es = new e ();
//		es.BaseUnitId = baseItem.ID;
//		es.EvolveQuestId = questID;
//		es.PartUnitId = partyID;
//		es.HelperPremium = 0;
//		es.friendInfo = firendItem;
//		es.HelperUnit = firendItem.UserUnit.Unit;
//		es.HelperUserId = firendItem.UserId;

		UnitDataModel tes = new UnitDataModel ();
//		tes.EvolveStart = es;
		tes.StageInfo = tci.GetStage (stageID);
		tes.StageInfo.cityId = EvolveCityID;
		tes.StageInfo.QuestId = questID;
		tes.evolveParty.Add (baseItem);
		for (int i = 2; i < evolveInfoLisst.Count; i++) {
			UserUnit temp = evolveInfoLisst[i] as UserUnit;
			tes.evolveParty.Add(temp);
		}
		for (int i = tes.evolveParty.Count; i < 3; i++) {
			tes.evolveParty.Add(null);
		}

//		DataCenter.gameState = GameState.Evolve;
		ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"evolve",tes);

	}
		
//	public bool CheckState () {
//		if (view.gameObject.activeSelf) {
//			return true;	
//		} else {
//			return false;	
//		}
//	}

	//================================================================================
	private Dictionary<string, object> TransferData = new Dictionary<string, object> ();
	private const uint EvolveCityID = 100;
	public List<UserUnit> unitItemData = new List<UserUnit>();


	void ReturnPreScene(object data) {
		ModuleEnum se = (ModuleEnum)data;
		bool showDetail = se == ModuleEnum.UnitDetailModule;
		bool enterEvolve = se == ModuleEnum.StageSelectModule;
//		if (!showDetail && !enterEvolve) {
//			DataCenter.gameState = GameState.Normal;
//		}
	}

	public static uint GetEvolveQuestID(EUnitType unitType, int  unitRare) {
		return GetEvolveQuestID(unitRare, GetEvolveStageID(unitType, unitRare));
	}
	
	static uint GetEvolveStageID (EUnitType unitType, int  unitRare) {
		uint stageID = 0;
		if (unitRare > 6) {
			return stageID;	
		}
		switch (unitType) {
			case bbproto.EUnitType.UFIRE:
				stageID = 1;
				break;
			case bbproto.EUnitType.UWATER:
				stageID = 2;
				break;
			case bbproto.EUnitType.UWIND:
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
		stageID += 1000; //cityId=100; stageId += cityId*10
		return stageID;
	}
	 
	static uint GetEvolveQuestID (int unitRare,uint stageID) {
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
		questID = stageID*10 + questID;
		return questID;
	}
	
}

public class DragPanelSetInfo {
	public Transform parentTrans;
	public Vector3 scrollerScale = Vector3.one;
	public Vector3 scrollerLocalPos = Vector3.zero;
	public Vector3 position = Vector3.zero;
	public Vector4 clipRange = Vector4.zero;
	public UIGrid.Arrangement gridArrange = UIGrid.Arrangement.Horizontal;
	public int maxPerLine = 1;
	public Vector3 scrollBarPosition = Vector3.zero;
	public int cellWidth = 100;
	public int cellHeight = 100;
	public int depth = 0;
	public UIScrollView.Movement scrollMovement = UIScrollView.Movement.Horizontal;
}