using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendSupportLogic : ConcreteComponent{


//	TUnitParty upi;


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
		CreateSupportFriendList();
	}
	
	public override void HideUI () {
		base.HideUI ();
	}
	
	public override void DestoryUI () {
		base.DestoryUI ();
	}
	
//	public void Callback (object data)
//	{
//		int partyID = 0;
//		try {
//			partyID = (int)data;
//		} 
//		catch (System.Exception ex) {
//			Debug.LogError(ex.Message);
//			return;
//		}
//		IUICallback call = viewComponent as IUICallback;
//		//Debug.LogError( "Comp : " +viewComponent.ToString());
//		if (call == null) {
//			return;		
//		}
//		if (partyID == 1) {
//			upi = DataCenter.Instance.PartyInfo.CurrentParty; //ModelManager.Instance.GetData (ModelEnum.UnitPartyInfo, errMsg) as TUnitParty;
//			if (upi == null) {
//				Debug.LogError("ModelManager.GetData( UnitPartyInfo ) return null");
//				return;
//			}
//			Dictionary<int,uint> temp = upi.GetPartyItem();
//			Dictionary<int,UnitBaseInfo> viewInfo = new Dictionary<int, UnitBaseInfo>();
//			foreach(var item in temp) {
//				TUserUnit uui =  DataCenter.Instance.UserUnitList.GetMyUnit(item.Value);
//				if(!userUnit.ContainsKey(item.Key)) {
//					userUnit.Add(item.Key,uui);
//				}
////				UnitBaseInfo ubi = DataCenter.Instance.UnitBaseInfo[uui.unitBaseInfo];
////				viewInfo.Add(item.Key,ubi);
//			}
//			call.Callback (viewInfo);
//		}
//		else {
//			call.Callback (null);
//		}
//	}

	void CreateSupportFriendList(){
		if(DataCenter.Instance.SupportFriends == null){
			LogHelper.LogError("FriendSupportLogic.CreateSupportFriendList(), DataCenter.Instance.SupportFriends == null, return!!!");
			return;
		}

		LogHelper.LogError("FriendSupportLogic.CreateSupportFriendList(), DataCenter.Instance.SupportFriends Exist, Call View Crate Drag List...");

		CallBackDispatcherArgs cbdArgs = new CallBackDispatcherArgs("CreateDragList", DataCenter.Instance.SupportFriends);
		ExcuteCallback(cbdArgs);
	}

}
