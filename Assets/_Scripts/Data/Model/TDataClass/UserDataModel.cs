using UnityEngine;
using System.Collections;
using bbproto;

public class UserDataModel : ProtobufDataBase {

//	public AccountInfo accountInfo;

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
