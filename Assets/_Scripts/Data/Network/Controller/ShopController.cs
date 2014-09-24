using UnityEngine;
using System.Collections;
using bbproto;

public class ShopController : ControllerBase {

	private static ShopController instance;
	
	public static ShopController Instance{
		get{
			if(instance == null)
				instance = new ShopController();
			return instance;
		}
	}

	public void FriendMaxExpand(NetCallback callBack) {
		ReqFriendMaxExpand reqFriendMaxExpand = new ReqFriendMaxExpand();
		reqFriendMaxExpand.header = new ProtoHeader();
		reqFriendMaxExpand.header.apiVer = ServerConfig.API_VERSION;
		reqFriendMaxExpand.header.userId = DataCenter.Instance.UserData.UserInfo.userId;

		HttpRequestManager.Instance.SendHttpRequest (reqFriendMaxExpand, callBack, ProtocolNameEnum.RspFriendMaxExpand);
	}

	public void RestoreStamina(NetCallback callBack) {
		ReqRestoreStamina reqRestoreStamina = new ReqRestoreStamina();
		reqRestoreStamina.header = new ProtoHeader();
		reqRestoreStamina.header.apiVer = ServerConfig.API_VERSION;
		reqRestoreStamina.header.userId = DataCenter.Instance.UserData.UserInfo.userId;

		HttpRequestManager.Instance.SendHttpRequest (reqRestoreStamina, callBack, ProtocolNameEnum.RspRestoreStamina);
	}

	public void ShopBuy(NetCallback callBack, string productId) {
		ReqShopBuy reqShopBuy = new ReqShopBuy();
		reqShopBuy.header = new ProtoHeader();
		reqShopBuy.header.apiVer = ServerConfig.API_VERSION;
		reqShopBuy.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqShopBuy.productId = productId;

		HttpRequestManager.Instance.SendHttpRequest (reqShopBuy, callBack, ProtocolNameEnum.RspShopBuy);
	}

	public void UnitMaxExpand(NetCallback callBack) {
		ReqUnitMaxExpand reqUnitMaxExpand = new ReqUnitMaxExpand();
		reqUnitMaxExpand.header = new ProtoHeader();
		reqUnitMaxExpand.header.apiVer = ServerConfig.API_VERSION;
		reqUnitMaxExpand.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		HttpRequestManager.Instance.SendHttpRequest (reqUnitMaxExpand, callBack, ProtocolNameEnum.RspUnitMaxExpand);
	}


}
