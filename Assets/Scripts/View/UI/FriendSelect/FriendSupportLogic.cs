using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendSupportLogic : ConcreteComponent{


//	TUnitParty upi;
	List<UnitItemViewInfo> supportFriendViewList = new List<UnitItemViewInfo>();

	Dictionary<int,TUserUnit> userUnit = new Dictionary<int, TUserUnit> ();
	
	public FriendSupportLogic(string uiName):base(uiName) {

	}
	
	public override void CreatUI () {
		base.CreatUI ();
	}

	public override void ShowUI () {
		base.ShowUI ();


//		StartQuestParam p= new StartQuestParam();
//		p.currPartyId=0;
//		p.questId=101;
//		p.stageId=11;
//		p.helperUserId=103;
//		p.helperUniqueId=2;
//		MsgCenter.Instance.Invoke (CommandEnum.ReqStartQuest, p);
		    
		GetSupportFriendInfoList();
		CreateSupportFriendViewList();
	}
	
	public override void HideUI () {
		base.HideUI ();
		DestorySupportFriendList();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();

	}

	List<TUserUnit> GetSupportFriendList(){
		if (DataCenter.Instance.SupportFriends == null){
			LogHelper.LogError("GetFriendUnitItemList(), DataCenter.Instance.SupportFriends == null!!!");
			return null;
		}
		
		List<TUserUnit> tuuList = new List<TUserUnit>();

		for (int i = 0; i < DataCenter.Instance.SupportFriends.Count; i++){
			tuuList.Add(DataCenter.Instance.SupportFriends[ i ].UserUnit);
		}
		
		return tuuList;
	}

	void GetFriendList(){

	}

	void GetSupportFriendInfoList(){
		//First, clear the current if exist
		supportFriendViewList.Clear();
		//Then, get the newest from DataCenter
		List<TUserUnit> unitList = GetSupportFriendList();
		if (unitList == null){
			LogHelper.LogError("GetFriendUnitItemViewList GetUnitList return null.");
			return;
		}

		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), unitList Count is : " + unitList.Count);
		
		for (int i = 0; i < unitList.Count; i++){
			UnitItemViewInfo viewItem = UnitItemViewInfo.Create(unitList [i]);
			supportFriendViewList.Add(viewItem);
		}
		
		LogHelper.Log("FriendListLogic.GetFriendUnitItemViewList(), ViewItem Count is : " + supportFriendViewList.Count);
	}


	void CreateSupportFriendViewList(){
		//LogHelper.LogError("FriendSupportLogic.CreateSupportFriendViewList(), DataCenter.Instance.SupportFriends Exist, Call View Crate Drag List...");

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragView", supportFriendViewList);
		ExcuteCallback(cbdArgs);
	}

	void DestorySupportFriendList(){
		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("DestoryDragView", null);
		ExcuteCallback(cbdArgs);
	}

}
