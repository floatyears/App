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
			if(rsp.account.payTotal != -1){
				accountInfo.payTotal = rsp.account.payTotal;
			}
			if(rsp.account.payMonth != -1){
				accountInfo.payMonth = rsp.account.payMonth;
			}
			if(rsp.account.money != -1){
				accountInfo.money = rsp.account.money;
			}
			if(rsp.account.stonePay != -1){
				accountInfo.stonePay = rsp.account.stonePay;
			}
			if(rsp.account.stoneFree != -1){
				accountInfo.stoneFree = rsp.account.stoneFree;
			}
			if(rsp.account.stone != -1){
				accountInfo.stone = rsp.account.stone;
			}
			if(rsp.account.friendPoint != -1){
				accountInfo.friendPoint = rsp.account.friendPoint;
			}
			if(rsp.account.firstSelectNum != -1){
				accountInfo.firstSelectNum = rsp.account.firstSelectNum;
			}
			if(!string.IsNullOrEmpty(rsp.user.userName)){
				userInfo.userName = rsp.user.userName;
			}
			if(!string.IsNullOrEmpty(rsp.user.nickName)){
				userInfo.nickName = rsp.user.nickName;
			}
			if(!string.IsNullOrEmpty(rsp.user.password)){
				userInfo.password = rsp.user.password;
			}
			if(rsp.user.rank != -1){
				userInfo.rank = rsp.user.rank;
			}
			if(rsp.user.exp != -1){
				userInfo.exp = rsp.user.exp;
			}
			if(rsp.user.staminaNow != -1){
				userInfo.staminaNow = rsp.user.staminaNow;
			}
			if(rsp.user.vipLevel != -1){
				userInfo.vipLevel = rsp.user.vipLevel;
			}
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
