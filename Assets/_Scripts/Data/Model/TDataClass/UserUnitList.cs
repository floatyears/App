using UnityEngine;
using System.Collections.Generic;
using bbproto;

//A wrapper to manage userUnitInfo list
public class UserUnitList {
    private Dictionary<string, UserUnit> userUnitInfo;
//	private List<uint> myUnqueIDList = new List<uint>();
    public UserUnitList() { 
        userUnitInfo = new Dictionary<string, UserUnit>(); //key: "{userid}_{unitUniqueId}"
    }

    string MakeUserUnitKey(uint userId, uint uniqueId) {
        return userId.ToString() + "_" + uniqueId.ToString();
    }

//    public  Dictionary<string, UserUnit> GetAll() {
//        return userUnitInfo;
//    }
//
//	public List<UserUnit> GetAllList () {
//		List<UserUnit> temp = new List<UserUnit> ();
//		foreach (var item in userUnitInfo.Values) {
//			temp.Add(item);
//		}
//		return temp;
//	}
//
//    public  void Clear() {
//        userUnitInfo.Clear();
//    }

//    public int Count{
//        get { return userUnitInfo.Count;}
//    }

   	UserUnit Get(uint userId, uint uniqueId) {
        string key = MakeUserUnitKey(userId, uniqueId);
//		Debug.LogError (" key : " + key);
//		foreach (var item in userUnitInfo.Keys) {
//			Debug.Log("unit list: " + item);
//				}
        if (!userUnitInfo.ContainsKey(key)) {
//            Debug.Log("Cannot find key " + key + " in Global.userUnitInfo");
            return null;
        }
	
        UserUnit tuu = userUnitInfo[key];
//		Debug.LogError (" tuu : " + tuu + " key : " + key);
        return tuu;
    }

	public UserUnit Get (string UserId) {
		if (!userUnitInfo.ContainsKey(UserId)) {
//			Debug.Log("Cannot find key " + UserId + " in Global.userUnitInfo");
			return null;
		}
		
		UserUnit tuu = userUnitInfo[UserId];
		return tuu;
	}

	/// <summary>
	/// Get the TUserUnit by uniqueid;
	/// </summary>
	/// <param name="uniqueID">Unique I.</param>
	public UserUnit Get(uint uniqueID) {
//		Debug.LogError ("Get uniqueID : " + uniqueID);
		foreach (var item in userUnitInfo.Values) {
			if(item.uniqueId == uniqueID) {
//				Debug.LogError ("uniqueID : " + uniqueID + " item.ID : " + item.ID);
				return item;
			}
		}
		return null;
	}

	public List<UserUnit> GetAllMyUnit() {
		List<UserUnit> myUnitList = new List<UserUnit> ();
		uint myID = DataCenter.Instance.UserData.UserInfo.userId;
		foreach (var item in userUnitInfo.Values) {
			if(item.userID == myID) {
				myUnitList.Add(item);
//				Debug.LogError("GetAllMyUnit : " + item.MakeUserUnitKey());
			}
		}
		return myUnitList;
	}

    public  UserUnit GetMyUnit(uint uniqueId) {
        if (DataCenter.Instance.UserData.UserInfo == null) {
            return null;
        }
		
        return Get(DataCenter.Instance.UserData.UserInfo.userId, uniqueId);
    }

	public  UserUnit GetMyUnit(string id) {
		if (DataCenter.Instance.UserData.UserInfo == null) {
			return null;
		}

		return Get(id);
	}

    public  void DelMyUnit(uint uniqueId) {
        if (DataCenter.Instance.UserData.UserInfo == null) {
            Debug.LogError ("TUserUnit.GetMyUnit() : Global.userInfo=null");
            return;
        }
	
        Del(DataCenter.Instance.UserData.UserInfo.userId, uniqueId);
//        foreach (var item in userUnitInfo) {
//            TUserUnit tUnit = item.Value as TUserUnit;
//        }
    }

    public  void Add(uint userId, uint uniqueId, UserUnit uu) {
//		string key = uu.MakeUserUnitKey();
//		if (!userUnitInfo.ContainsKey (key)) {
//			userUnitInfo.Add(key, uu);		
//		}
//        else {
//            userUnitInfo[key] = uu;
//        }

		Add(uu);
    }

	public void Add(UserUnit tuu) {
		string key = tuu.MakeUserUnitKey();
//		Debug.LogError ("add userunit : " + key + " containts : " + userUnitInfo.ContainsKey (key));
		if (!userUnitInfo.ContainsKey (key)) {
			userUnitInfo.Add(key, tuu);		
		}
		else {
			userUnitInfo[key] = tuu;
		}
	}

    public List<uint> GetMyUnitUniqueIdList(){
        List <uint> uniqueIdList = new List<uint>();
        foreach (var item in userUnitInfo) {
            UserUnit tUnit = item.Value as UserUnit;
            if (GetMyUnit(tUnit.unitId) != null){
				uniqueIdList.Add(tUnit.unitId);
            }
        }
        return uniqueIdList;
    }

	public UserUnit UpdateMyUnit(UserUnit unit) {
		return AddMyUnit(unit);
	}

    public UserUnit AddMyUnit(UserUnit unit) {
		unit.userID = DataCenter.Instance.UserData.UserInfo.userId;
		Add(DataCenter.Instance.UserData.UserInfo.userId, unit.uniqueId, unit);
		return unit;
    }

    public void AddMyUnitList(List <UserUnit> unitList){
        for (int i = 0; i < unitList.Count; i++){
            AddMyUnit(unitList[i]);
        }
    }

    public void DelMyUnitList(List <uint> uniqueIds){
        for (int i = 0; i < uniqueIds.Count; i++){
            DelMyUnit(uniqueIds[i]);
        }
    }

    public  void Del(uint userId, uint uniqueId) {
        string key = MakeUserUnitKey(userId, uniqueId);
//		Debug.LogError ("del my unit : key  " + key + "userid : " + userId + " unitqueid : " + uniqueId);
        if (userUnitInfo.ContainsKey(key))
            userUnitInfo.Remove(key);
    }

    public List<uint> FirstGetUnits(List<UserUnit> unitList){
        List<uint> ret = new List<uint>();
        foreach (var item in unitList) {
            if (IsNewUnit(item) && !ret.Contains(item.unitId)) {
                LogHelper.Log("FirstGetUnits(), new id {0}", item.unitId);
                ret.Add(item.uniqueId);
            }
            else {
                ret.Add(0);
            }
        }
        return ret;
    }

    private bool IsNewUnit(UserUnit unit){
        foreach (var item in userUnitInfo) {
            if (unit.unitId == item.Value.UnitInfo.id) {
                return false;
            }
        }

        return true;
    }

}