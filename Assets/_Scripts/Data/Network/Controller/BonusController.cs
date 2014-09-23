using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class BonusController : ControllerBase {

	private static BonusController instance;

	public static BonusController Instance{
		get{
			if(instance == null)
				instance = new BonusController();
			return instance;
		}
	}

	public void AcceptBonus(NetCallback callBack, List<int> bonusId) {
		
		ReqAcceptBonus reqAcceptBonus = new ReqAcceptBonus();
		reqAcceptBonus.header = new ProtoHeader();
		reqAcceptBonus.header.apiVer = ServerConfig.API_VERSION;
		reqAcceptBonus.header.userId = DataCenter.Instance.UserInfo.userId;
		
		//request params
		reqAcceptBonus.bonusId.AddRange( bonusId );

		HttpRequestManager.Instance.SendHttpRequest (reqAcceptBonus, callBack ,ProtocolNameEnum.RspAcceptBonus);
	}

	public void GetBonusList(NetCallback callBack) {
		
		ReqBonusList reqBonusList = new ReqBonusList();
		reqBonusList.header = new ProtoHeader();
		reqBonusList.header.apiVer = ServerConfig.API_VERSION;
		reqBonusList.header.userId = DataCenter.Instance.UserInfo.userId;

		HttpRequestManager.Instance.SendHttpRequest (reqBonusList, callBack, ProtocolNameEnum.RspBonusList);
	}

}
