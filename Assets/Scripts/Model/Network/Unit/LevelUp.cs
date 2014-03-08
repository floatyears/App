using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUp: ProtoManager {
	private bbproto.ReqLevelUp reqLevelUp;
	private bbproto.RspLevelUp rspLevelUp;

	private uint baseUniqueId;
	private List<uint> partUniqueId = new List<uint>();
	private uint helperUserId;
	private TUserUnit helperUserUnit;

	//////////////////////////////////////////////////
	//Property:  request parameters
	public uint BaseUniqueId {
		get { return baseUniqueId; } 
		set { baseUniqueId = value; }
	}

	public List<uint> PartUniqueId {
		get { return partUniqueId; } 
		set { partUniqueId = value; }
	}

	public uint HelperUserId {
		get { return helperUserId; } 
		set { helperUserId = value; }
	}

	public TUserUnit HelperUserUnit {
		get { return helperUserUnit; } 
		set { helperUserUnit = value; }
	}

	//Response property
	public int BlendExp {
		get { return rspLevelUp.blendExp ;}
	}
	//////////////////////////////////////////////////

	public LevelUp(){}
	~LevelUp() {}

	public override bool MakePacket () {
		LogHelper.Log ("LevelUp.MakePacket()...");

		Proto = Protocol.LEVEL_UP;
		reqType = typeof(ReqLevelUp);
		rspType = typeof(RspLevelUp);

		reqLevelUp = new ReqLevelUp ();
		reqLevelUp.header = new ProtoHeader ();
		reqLevelUp.header.apiVer = "1.0";
		reqLevelUp.baseUniqueId = baseUniqueId;
		reqLevelUp.partUniqueId.AddRange(partUniqueId);
		reqLevelUp.helperUserId = helperUserId;
		reqLevelUp.helperUnit = helperUserUnit.Object;

		ErrorMsg err = SerializeData (reqLevelUp); // save to Data for send out
		
		return err.Code == ErrorCode.Succeed;
	}

	public override void OnResponse (bool success) {
		if (!success) { //Unserialize data fail
			//TODO: show error window for user to retry
			return; 
		}

		rspLevelUp = InstanceObj as bbproto.RspLevelUp;

		//update money
//		GlobalData.accountInfo.Money = rspLevelUp.money;

		// update unitlist
		uint userId = GlobalData.userInfo.UserId;
		if (rspLevelUp.unitList != null) {
			GlobalData.myUnitList.Clear();	//TODO: maybe havenot to clear all, just for simple right now.
			//GlobalData.userUnitList.Clear(); //TODO: only update the baseUnit
			foreach(UserUnit unit in rspLevelUp.unitList) {
				GlobalData.myUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
				//GlobalData.userUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
			}
			LogHelper.Log("rspLevelUp add to myUserUnit.count: {0}", rspLevelUp.unitList.Count);
		}

//		LogHelper.Log("reponse userId:"+rspLevelUp.user.userId);


		//send response to caller
		OnResposeEnd (InstanceObj);
	}

	int GetMaxExpByLv(int level) {
		return 0;
	}

	int GetRiseLevel(int curExp, int curLv, int gotExp, ref int nextExp) {
		int totalExp = gotExp;
		int riseLv = 0, curLvExp=0;
		
		nextExp = (GetMaxExpByLv( curLv ) - curExp);
		if (totalExp < nextExp) {
			nextExp -= totalExp;
			return 0;
		}
		
		totalExp -= nextExp;
		while (totalExp >= 0) {
			riseLv += 1;
			curLv += 1;
			
			nextExp = totalExp;
			curLvExp = GetMaxExpByLv( curLv );
			if ( curLvExp <= 0 ) { //reach max level
				nextExp = 0;
				break;
			}
			
			totalExp -= curLvExp;
		}
		
		return riseLv;
	}
}
