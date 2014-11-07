using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UnitController : ControllerBase {

	private static UnitController instance;
	
	public static UnitController Instance{
		get{
			if(instance == null)
				instance = new UnitController();
			return instance;
		}
	}

	public void ChangeParty(PartyInfo partyInfo, NetCallback callback) {
		ReqChangeParty reqChangeParty = new ReqChangeParty();
		reqChangeParty.header = new ProtoHeader();
		reqChangeParty.header.apiVer = ServerConfig.API_VERSION;
		reqChangeParty.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		reqChangeParty.party = partyInfo;

		HttpRequestManager.Instance.SendHttpRequest (reqChangeParty, callback, ProtocolNameEnum.RspChangeParty);
	}
	
	public void Evolve(NetCallback callback, uint baseUnitId, List<uint> partUnitId) {

		ReqEvolve reqEvolve = new ReqEvolve();
		reqEvolve.header = new ProtoHeader();
		reqEvolve.header.apiVer = ServerConfig.API_VERSION;
		reqEvolve.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqEvolve.baseUniqueId = baseUnitId;
//		CollectionHelper.ResetReadOnlyList(reqEvolve.partUniqueId, partUnitId);
		reqEvolve.partUniqueId.AddRange(partUnitId);
//		reqEvolve.helperUserId = helperUserId;
//		reqEvolve.helperUnit = helperUnit;
//		reqEvolve.helperPremium = helperPremium;
		
		HttpRequestManager.Instance.SendHttpRequest (reqEvolve, callback, ProtocolNameEnum.RspEvolve);
	}

//	public void EvolveDone(NetCallback callback, uint questId, uint securityKey, int getMoney, List<uint> getUnit, List<uint> hitGrid) {
//		ReqEvolveDone reqEvolveDone = new ReqEvolveDone();
//		reqEvolveDone.header = new ProtoHeader();
//		reqEvolveDone.header.apiVer = ServerConfig.API_VERSION;
//		reqEvolveDone.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
//		
//		//request params
//		reqEvolveDone.questId = questId;
//		reqEvolveDone.securityKey = securityKey;
//		reqEvolveDone.getMoney = getMoney;
//		CollectionHelper.ResetReadOnlyList(reqEvolveDone.getUnit, getUnit);
//		CollectionHelper.ResetReadOnlyList(reqEvolveDone.hitGrid, hitGrid);
//
//		HttpRequestManager.Instance.SendHttpRequest (reqEvolveDone, callback, ProtocolNameEnum.RspEvolveDone);
//	}

	public void UnitFavorite(NetCallback callBack, uint uniqueid, EFavoriteAction action) {
		ReqUnitFavorite reqUnitFavorite = new ReqUnitFavorite();
		reqUnitFavorite.header = new ProtoHeader();
		reqUnitFavorite.header.apiVer = ServerConfig.API_VERSION;
		reqUnitFavorite.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqUnitFavorite.unitUniqueId = uniqueid;
		reqUnitFavorite.action = action;

		HttpRequestManager.Instance.SendHttpRequest (reqUnitFavorite, callBack, ProtocolNameEnum.RspUnitFavorite);
	}

	public void Gacha(NetCallback callback, int gachaId, int gachaCount) {
		ReqGacha reqGacha = new ReqGacha();
		reqGacha.header = new ProtoHeader();
		reqGacha.header.apiVer = ServerConfig.API_VERSION;
		reqGacha.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		
		reqGacha.gachaId = gachaId;
		reqGacha.gachaCount = gachaCount;

		HttpRequestManager.Instance.SendHttpRequest (reqGacha, callback, ProtocolNameEnum.RspGacha);
	}

	public void UserguideEvoUnit(NetCallback callBack, uint unitId) {
		ReqUserGuideEvolveUnit reqAddEvolveUnit = new ReqUserGuideEvolveUnit();
		reqAddEvolveUnit.header = new ProtoHeader();
		reqAddEvolveUnit.header.apiVer = ServerConfig.API_VERSION;
		reqAddEvolveUnit.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqAddEvolveUnit.unitId = unitId;

		HttpRequestManager.Instance.SendHttpRequest (reqAddEvolveUnit, callBack, ProtocolNameEnum.RspUserGuideEvolveUnit);
	}

	public void LevelUp(NetCallback callback, uint baseUniqueId, List<uint> partUniqueId, uint helperUserId, UserUnit helperUserUnit ){
		ReqLevelUp reqLevelUp = new ReqLevelUp();
		reqLevelUp.header = new ProtoHeader();
		reqLevelUp.header.apiVer = ServerConfig.API_VERSION;
		
		if (DataCenter.Instance.UserData.UserInfo != null)
			reqLevelUp.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		reqLevelUp.baseUniqueId = baseUniqueId;
		reqLevelUp.partUniqueId.AddRange(partUniqueId);
		reqLevelUp.helperUserId = helperUserId;
		reqLevelUp.helperUnit = helperUserUnit;

		HttpRequestManager.Instance.SendHttpRequest (reqLevelUp, callback, ProtocolNameEnum.RspLevelUp);
	}

	public void SellUnit(NetCallback Callback, params uint[] unitUniqueIdArray) {
		ReqSellUnit reqSellUnit = new ReqSellUnit();
		reqSellUnit.header = new ProtoHeader();
		reqSellUnit.header.apiVer = ServerConfig.API_VERSION;
		reqSellUnit.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		reqSellUnit.unitUniqueId.Clear ();
		reqSellUnit.unitUniqueId.AddRange (unitUniqueIdArray);

		HttpRequestManager.Instance.SendHttpRequest (reqSellUnit, Callback, ProtocolNameEnum.RspSellUnit);
	}

	public void SellUnit(NetCallback Callback, List<uint> unitUniqueIdArray) {
		ReqSellUnit reqSellUnit = new ReqSellUnit();
		reqSellUnit.header = new ProtoHeader();
		reqSellUnit.header.apiVer = ServerConfig.API_VERSION;
		reqSellUnit.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		reqSellUnit.unitUniqueId.Clear ();
		reqSellUnit.unitUniqueId.AddRange (unitUniqueIdArray);
		
		HttpRequestManager.Instance.SendHttpRequest (reqSellUnit, Callback, ProtocolNameEnum.RspSellUnit);
	}

	
}
