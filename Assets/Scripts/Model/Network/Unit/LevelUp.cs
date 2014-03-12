using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class LevelUp: ProtoManager {
    private bbproto.ReqLevelUp reqLevelUp;
    private bbproto.RspLevelUp rsp;

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
        get { return rsp.blendExp;}
    }
    //////////////////////////////////////////////////

    public LevelUp() {
    }
    ~LevelUp() {
    }

    public override bool MakePacket() {
        LogHelper.Log("LevelUp.MakePacket()...");

        Proto = Protocol.LEVEL_UP;
        reqType = typeof(ReqLevelUp);
        rspType = typeof(RspLevelUp);

        reqLevelUp = new ReqLevelUp();
        reqLevelUp.header = new ProtoHeader();
        reqLevelUp.header.apiVer = Protocol.API_VERSION;

        if (DataCenter.Instance.UserInfo != null)
            reqLevelUp.header.userId = DataCenter.Instance.UserInfo.UserId;

        reqLevelUp.baseUniqueId = baseUniqueId;
        reqLevelUp.partUniqueId.AddRange(partUniqueId);
        reqLevelUp.helperUserId = helperUserId;
        reqLevelUp.helperUnit = helperUserUnit.Object;

        ErrorMsg err = SerializeData(reqLevelUp); // save to Data for send out
		
        return err.Code == (int)ErrorCode.SUCCESS;
    }

    public override void OnResponse(bool success) {
        if (!success) { //Unserialize data fail
            //TODO: show error window for user to retry
            return; 
        }

        rsp = InstanceObj as bbproto.RspLevelUp;



//		LogHelper.Log("reponse userId:"+rsp.user.userId);


    }

    int GetMaxExpByLv(int level) {
        return 0;
    }

    int GetRiseLevel(int curExp, int curLv, int gotExp, ref int nextExp) {
        int totalExp = gotExp;
        int riseLv = 0, curLvExp = 0;
		
        nextExp = (GetMaxExpByLv(curLv) - curExp);
        if (totalExp < nextExp) {
            nextExp -= totalExp;
            return 0;
        }
		
        totalExp -= nextExp;
        while (totalExp >= 0) {
            riseLv += 1;
            curLv += 1;
			
            nextExp = totalExp;
            curLvExp = GetMaxExpByLv(curLv);
            if (curLvExp <= 0) { //reach max level
                nextExp = 0;
                break;
            }
			
            totalExp -= curLvExp;
        }
		
        return riseLv;
    }

    protected override void OnResponseEnd(object data) {
        if (data == null) {
            Debug.LogError("OnResponseEnd(), data == null");
            return;
        }
        //        Debug.LogError("Login Success : " + Time.realtimeSinceStartup);
        //        Debug.LogError("data=" + data);
        
        bbproto.RspLevelUp rsp = data as bbproto.RspLevelUp;
        errMsg.SetErrorMsg(rsp.header.code);
        if (rsp.header.code != (int)ErrorCode.SUCCESS) {
            return;
        }
        if (rsp == null) {
            //                errMsg.SetErrorMsg(ErrorCode.ILLEGAL_PARAM, ErrorMsgType.RSP_AUTHUSER_NULL);
            LogHelper.Log("levelup OnResponseEnd() response rsp == null");
            return;
        }

        Debug.LogError("rsp.header.error : " + rsp.header.error + " rsp.header.code : " + rsp.header.code);
        
        //update money
        DataCenter.Instance.AccountInfo.Money = (int)rsp.money;
        
        // update unitlist
        uint userId = DataCenter.Instance.UserInfo.UserId;
        if (rsp.unitList != null) {
            //update myUnitList
            DataCenter.Instance.MyUnitList.Clear();
            foreach (UserUnit unit in rsp.unitList) {
                DataCenter.Instance.MyUnitList.Add(userId, unit.uniqueId, new TUserUnit(unit));
            }
            
            // update blendUnit in the userUnitList
            //              TUserUnit blendUnit = DataCenter.Instance.UserUnitList.GetMyUnit( rsp.blendUniqueId );
            //              blendUnit = DataCenter.Instance.MyUnitList.GetMyUnit( rsp.blendUniqueId );
            
            //remove partUniqueId from userUnitList
            foreach (uint partUniqueId in rsp.partUniqueId) {
                DataCenter.Instance.UserUnitList.DelMyUnit(partUniqueId);
            }
            
            LogHelper.Log("rsp add to myUserUnit.count: {0}", rsp.unitList.Count);
        }
    }
}
