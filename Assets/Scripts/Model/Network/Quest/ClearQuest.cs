using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ClearQuestParam {
	public uint questId;
	public int getMoney;
	public List<uint> getUnit;
	public List<uint> hitGrid;
}

public class TRspClearQuest {
	public int			rank;
	public int			exp;
	public int			money;			
	public int			friendPoint;
	public int			staminaNow;	
	public int			staminaMax;		
	public uint			staminaRecover;	
	public int			gotMoney;
	public int			gotExp;	
	public int			gotChip;
	public int			gotFriendPoint;
	List<TUserUnit>		gotUnit;
}

public class ClearQuest: ProtoManager {
	private bbproto.ReqClearQuest reqClearQuest;
	private bbproto.RspClearQuest rspClearQuest;
	private ClearQuestParam questParam;

	public ClearQuest(){
		MsgCenter.Instance.AddListener (CommandEnum.ReqClearQuest, OnReceiveCommand);
	}

	~ClearQuest() {
		MsgCenter.Instance.RemoveListener (CommandEnum.ReqClearQuest, OnReceiveCommand);
	}

	public override bool MakePacket () {
//		LogHelper.Log ("ClearQuest.MakePacket()...");

		Proto = Protocol.START_QUEST;
		reqType = typeof(ReqClearQuest);
		rspType = typeof(RspClearQuest);

		reqClearQuest = new ReqClearQuest ();
		reqClearQuest.header = new ProtoHeader ();
		reqClearQuest.header.apiVer = Protocol.API_VERSION;

		if (  GlobalData.userInfo != null )
			reqClearQuest.header.userId = GlobalData.userInfo.UserId;


		reqClearQuest.questId = questParam.questId;
		reqClearQuest.getMoney = questParam.getMoney;
		reqClearQuest.getUnit.AddRange( questParam.getUnit );
		reqClearQuest.hitGrid.AddRange( questParam.hitGrid );

		ErrorMsg err = SerializeData (reqClearQuest); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { return; }

		rspClearQuest = InstanceObj as bbproto.RspClearQuest;
//		LogHelper.Log("reponse userId:"+rspClearQuest.user.userId);

		GlobalData.userInfo.StaminaNow = rspClearQuest.staminaNow;
		GlobalData.userInfo.StaminaRecover = rspClearQuest.staminaRecover;

		LogHelper.Log ("rspClearQuest code:{0}, error:{1}", rspClearQuest.header.code, rspClearQuest.header.error);
		if(rspClearQuest.header.code == 0 ){
			TRspClearQuest cq = new TRspClearQuest();
			cq.rank 		= rspClearQuest.rank;
			cq.exp 			= rspClearQuest.exp;
			cq.money 		= rspClearQuest.money;
			cq.friendPoint 	= rspClearQuest.friendPoint;
			cq.staminaNow	= rspClearQuest.staminaNow;
			cq.staminaMax	= rspClearQuest.staminaMax;
			cq.staminaRecover = rspClearQuest.staminaRecover;

			cq.gotExp		= rspClearQuest.gotExp;
			cq.gotChip		= rspClearQuest.gotChip;
			cq.gotFriendPoint = rspClearQuest.gotFriendPoint;
//			cq.gotUnit		= new List<TUserUnit>();
//			foreach (UserUnit uu in rspClearQuest.gotUnit ) {
//				TUserUnit tuu = new TUserUnit(uu);
//				cq.gotUnit.Add(tuu);
//			}

			//send response to caller
			MsgCenter.Instance.Invoke (CommandEnum.RspClearQuest, null);

		} else{
			MsgCenter.Instance.Invoke (CommandEnum.RspClearQuest, null);
		}

	}

	void OnReceiveCommand(object data) {
		questParam = data as ClearQuestParam;
		if (questParam == null) {
			LogHelper.Log ("ClearQuest: Invalid param data.");
			return;
		}

		LogHelper.Log ("OnReceiveCommand(ClearQuest): questId:{0} getMoney:{1} getUnit.count:{2} hitGrid.count{3}",
		               questParam.questId, questParam.getMoney, questParam.getUnit.Count, questParam.hitGrid.Count);

		Send (); //send request to server
	}

}

