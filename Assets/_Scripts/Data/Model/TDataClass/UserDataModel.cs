using UnityEngine;
using System.Collections;
using bbproto;

public class UserDataModel : ProtobufDataBase {

	public AccountInfo accountInfo;

	public void RefreshAcountInfo(TRspClearQuest clearQuest) {
		accountInfo.money = clearQuest.money;
		accountInfo.friendPoint = clearQuest.friendPoint;
		accountInfo.stone += clearQuest.gotStone;
	}
}
