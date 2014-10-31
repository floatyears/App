using UnityEngine;
using System.Collections;
using bbproto;

public class UserDataModel : ProtobufDataBase {

//	public AccountInfo accountInfo;

	public UserDataModel(){
		HttpRequestManager.Instance.AddProtoListener (ProtocolNameEnum.RspRefreshUser, OnRefreshUserInfo);
		HttpRequestManager.Instance.AddProtoListener (ProtocolNameEnum.RspUserGuideStep, OnUserGuide);
	}

	void OnRefreshUserInfo(object data){
		RspRefreshUser rsp = data as RspRefreshUser;
		if(rsp.header.code == ErrorCode.SUCCESS){
			Debug.Log("acount: " + rsp.account + " userinfo: " + rsp.user);
			accountInfo = rsp.account;
			userInfo = rsp.user;
		}
	}

	void OnUserGuide(object data){
		RspUserGuideStep rsp = data as RspUserGuideStep;
		if (rsp.header.code == ErrorCode.SUCCESS) {
			NoviceGuideStepManager.Instance.CurrentGuideStep = (NoviceGuideStage)rsp.step;
		}
	}

	private UserInfo userInfo;

	public UserInfo UserInfo { 
		get { return userInfo; } 
		set { 
			userInfo =  value;
			userInfo.Init();
		} 
	}

	
	private AccountInfo accountInfo;
	public AccountInfo AccountInfo {
		get { return accountInfo; }
		set { accountInfo = value; }
	}


	private LoginInfo loginInfo;
	public LoginInfo LoginInfo { 
		get { return loginInfo; }
		set { loginInfo = value; }
	}

	
	public void RefreshUserInfo(TRspClearQuest clearQuest) {
		if (clearQuest == null) {
			return; 
		}
		userInfo.RefreshUserInfo(clearQuest);
		accountInfo.RefreshAcountInfo(clearQuest);
	}

}
