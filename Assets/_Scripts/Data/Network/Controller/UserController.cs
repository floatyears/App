using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class UserController : ControllerBase {

	private static UserController instance;
	
	public static UserController Instance{
		get{
			if(instance == null)
				instance = new UserController();
			return instance;
		}
	}


	public void Login(NetCallback callback, NetCallback questStarCallback=null, uint selectRole=0) {

		bool b = PlayerPrefs.HasKey (GameDataPersistence.USER_ID);
		uint userId = GameDataPersistence.Instance.GetUInt(GameDataPersistence.USER_ID);
		string uuid = GameDataPersistence.Instance.GetData(GameDataPersistence.UUID);
		if (userId == 0 && uuid.Length == 0) {
			uuid = System.Guid.NewGuid().ToString();
			GameDataPersistence.Instance.StoreData(GameDataPersistence.UUID, uuid);
			LogHelper.Log("New user first run, generate uuid: " + uuid);
		} else {
			LogHelper.Log("Exists userid:{0} uuid:{1} ", userId, uuid);
		}
		
		ReqAuthUser reqAuthUser = new ReqAuthUser();
		reqAuthUser.header = new ProtoHeader();
		reqAuthUser.header.apiVer = ServerConfig.API_VERSION;
		
		reqAuthUser.terminal = new TerminalInfo();
		reqAuthUser.header.userId = userId;
		reqAuthUser.terminal.uuid = uuid;
		reqAuthUser.selectRole = selectRole;
		reqAuthUser.appVersion = ServerConfig.AppVersion;
		reqAuthUser.terminal.channel = ServerConfig.Channel;

		HttpRequestManager.Instance.SendHttpRequest (reqAuthUser, callback, ProtocolNameEnum.RspAuthUser);

		if( questStarCallback != null ) {
			HttpRequestManager.Instance.AddProtoListener (ProtocolNameEnum.RspQuestStarList, questStarCallback );
		}

	}

	public void RenameNick(NetCallback callback, string newNickName) {
		ReqRenameNick reqRenameNick = new ReqRenameNick();
		reqRenameNick.header = new ProtoHeader();
		reqRenameNick.header.apiVer = ServerConfig.API_VERSION;
		if (DataCenter.Instance.UserData.UserInfo != null && DataCenter.Instance.UserData.UserInfo.userId > 0) {
			reqRenameNick.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		}
		else {
			reqRenameNick.header.userId = GameDataPersistence.Instance.GetUInt(GameDataPersistence.USER_ID);
		}
		reqRenameNick.newNickName = newNickName;
		HttpRequestManager.Instance.SendHttpRequest (reqRenameNick, callback, ProtocolNameEnum.RspRenameNick);
	}

//	public void Login(NetCallback callback){
//		FirstLogin (callback, 0);
//	}


	public void FinishUserGuide(NetCallback callBack, int step) {
		ReqFinishUserGuide reqFinishUserGuide = new ReqFinishUserGuide();
		reqFinishUserGuide.header = new ProtoHeader();
		reqFinishUserGuide.header.apiVer = ServerConfig.API_VERSION;
		reqFinishUserGuide.header.userId = DataCenter.Instance.UserData.UserInfo.userId;
		
		//request params
		reqFinishUserGuide.step = step;

		HttpRequestManager.Instance.SendHttpRequest (reqFinishUserGuide, callBack, ProtocolNameEnum.RspFinishUserGuide,false,null,ProtocolNameEnum.NONE,false);
	}

	public void SendLog(string desc, string stackTrack){
		ReqClientLog req = new ReqClientLog ();
		req.header = new ProtoHeader ();
		req.header.apiVer = ServerConfig.API_VERSION;
		req.header.userId = DataCenter.Instance.UserData.UserInfo.userId;

		req.msg = desc;
		req.stack = stackTrack;

		HttpRequestManager.Instance.SendHttpRequest (req, null, ProtocolNameEnum.NONE, false, null, ProtocolNameEnum.NONE, false);
	}

}
