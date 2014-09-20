using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class EvolveModule : ModuleBase {
	public EvolveModule(UIConfigItem config):base(  config) {
		CreateUI<EvolveView> ();
	}
	
	public override void InitUI () {
		base.InitUI ();
	}
	
	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.SelectUnitBase, SelectUnit);
		MsgCenter.Instance.AddListener (CommandEnum.ReturnPreScene, ReturnPreScene);

		ReadData();
	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.SelectUnitBase, SelectUnit);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReturnPreScene, ReturnPreScene);

//		if (UIManager.Instance.nextScene != ModuleEnum.UnitDetailModule) {
//			base.DestoryUI();
//		}
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}

	public override void OnReceiveMessages (params object[] data) {
		List<ProtobufDataBase> evolveInfoLisst = data[0] as List<ProtobufDataBase>;
		TUserUnit baseItem = evolveInfoLisst [0] as TUserUnit ;
		TFriendInfo firendItem = evolveInfoLisst [1] as TFriendInfo;
		TUserUnit tuu = baseItem;
		TUnitInfo tui = tuu.UnitInfo;
		TCityInfo tci = DataCenter.Instance.GetCityInfo (EvolveCityID);
		uint stageID = GetEvolveStageID (tui.Type, tui.Rare);
		uint questID = GetEvolveQuestID (tui.Type, tui.Rare);
		List<uint> partyID = new List<uint> ();
		for (int i = 2; i < evolveInfoLisst.Count; i++) {
			TUserUnit temp = evolveInfoLisst[i] as TUserUnit;
			partyID.Add(temp.ID);
		}

		EvolveStart es = new EvolveStart ();
		es.BaseUnitId = baseItem.ID;
		es.EvolveQuestId = questID;
		es.PartUnitId = partyID;
		es.HelperPremium = 0;
		es.friendInfo = firendItem;
		es.HelperUnit = firendItem.UserUnit.Unit;
		es.HelperUserId = firendItem.UserId;

		TEvolveStart tes = new TEvolveStart ();
		tes.EvolveStart = es;
		tes.StageInfo = tci.GetStage (stageID);
		tes.StageInfo.CityId = EvolveCityID;
		tes.StageInfo.QuestId = questID;
		tes.evolveParty.Add (baseItem);
		for (int i = 2; i < evolveInfoLisst.Count; i++) {
			TUserUnit temp = evolveInfoLisst[i] as TUserUnit;
			tes.evolveParty.Add(temp);
		}
		for (int i = tes.evolveParty.Count; i < 3; i++) {
			tes.evolveParty.Add(null);
		}

		DataCenter.gameState = GameState.Evolve;
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
	public List<TUserUnit> unitItemData = new List<TUserUnit>();


	void ReturnPreScene(object data) {
		ModuleEnum se = (ModuleEnum)data;
		bool showDetail = se == ModuleEnum.UnitDetailModule;
		bool enterEvolve = se == ModuleEnum.StageSelectModule;
		if (!showDetail && !enterEvolve) {
			DataCenter.gameState = GameState.Normal;
		}
	}

	void selectUnitMaterial(object data) {
		if (data == null) {
			return;	
		}
		TransferData.Clear ();
		TransferData.Add(EvolveView.MaterialData, data);
		view.CallbackView (TransferData);
	}

	void SelectUnit (object data) {
		if (data == null) {
			return;
		}
		TransferData.Clear ();
		TransferData.Add(EvolveView.BaseData, data);
		view.CallbackView (TransferData);
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

	void ReadData() {
		unitItemData.Clear ();
		unitItemData.AddRange (DataCenter.Instance.UserUnitList.GetAllMyUnit ());
		TransferData.Clear ();
		TransferData.Add (EvolveView.RefreshData, unitItemData);
		view.CallbackView (TransferData);
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