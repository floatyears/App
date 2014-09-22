using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;
public class CommonController : ControllerBase {
	private static CommonController instance;
	
	public static CommonController Instance{
		get{
			if(instance == null)
				instance = new CommonController();
			return instance;
		}
	}
	public void GetServerTime(NetCallback callBack, uint questid, bool gameover=false) {
		ReqGetServerTime reqGetServerTime = new ReqGetServerTime();
		reqGetServerTime.header = new ProtoHeader();
		reqGetServerTime.header.apiVer = ServerConfig.API_VERSION;
		reqGetServerTime.header.userId = DataCenter.Instance.UserInfo.UserId;
		HttpRequestManager.Instance.SendHttpRequest (reqGetServerTime,callBack, ProtocolNameEnum.RspGetServerTime);
	}

	public void UploadStat(NetCallback callBack, List<EventData> events) {
		ReqUploadStat reqUploadStat = new ReqUploadStat();
		reqUploadStat.header = new ProtoHeader();
		reqUploadStat.header.apiVer = ServerConfig.API_VERSION;
		reqUploadStat.header.userId = DataCenter.Instance.UserInfo != null ? DataCenter.Instance.UserInfo.UserId : 0;
		
		//request params
		reqUploadStat.events.AddRange( events );

		HttpRequestManager.Instance.SendHttpRequest (reqUploadStat, callBack, ProtocolNameEnum.RspUploadStat);
	}

}
