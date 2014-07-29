//using UnityEngine;
//using System.Collections.Generic;
//using bbproto;
//
//public class GlobalData {
//    private static GlobalData instance;
//    public static GlobalData Instance {
//        get {
//            if (instance == null) {
//                instance = new GlobalData();
//            }
//            return instance;
//        }
//
//    }
//    private GlobalData() {
//    }
//
//    public TUnitInfo GetUnitInfo(uint unitID) {
//        if (unitInfo.ContainsKey(unitID)) {
//            TUnitInfo tui = unitInfo[unitID];
//            return tui;
//        }
//        else {
//            Debug.LogError("unitid is invalid");
//            return null;
//        }
//    }
//	
//    public static TUserInfo userInfo;
//    public static TAccountInfo accountInfo;
//    public static List<TFriendInfo> friends;
//    public static TPartyInfo partyInfo;
//    //TODO: reconstruct myUnitList
//    public static UserUnitList myUnitList = new UserUnitList();
//    public static UserUnitList userUnitList = new UserUnitList();

//    public static Dictionary<int, SkillBaseInfo> skill = new Dictionary<int, SkillBaseInfo>();
//    public static Dictionary<uint, TUnitInfo>	unitInfo = new Dictionary<uint, TUnitInfo>();
//    public static Dictionary<uint, TEnemyInfo> enemyInfo = new Dictionary<uint, TEnemyInfo>();
//    public static Dictionary<int, UnitBaseInfo> unitBaseInfo = new Dictionary<int, UnitBaseInfo>();
//    public static Dictionary<uint, TrapBase> trapInfo = new Dictionary<uint, TrapBase>();
//
//    public const int maxEnergyPoint = 20;
//    public const int posStart = 0;
//    public const int posEnd = 5;
//    public const int minNeedCard = 2;
//    public const int maxNeedCard = 5;
//
//    public static Dictionary<int, Object> tempEffect = new Dictionary<int, Object>();
//    public static List<int> HaveCard = new List<int>() {111,185,161,101,122,195};
//
//
//    public void RefreshUserInfo(TRspClearQuest clearQuest) {
//        if (clearQuest == null) {
//            return;	
//        }
//        userInfo.RefreshUserInfo(clearQuest);
//        accountInfo.RefreshAcountInfo(clearQuest);
//    }
//
//    // return UserCost of curr Rank.
//    public static int UserCost {
//        get {
//            const int TYPE_MAXCOST_OF_RANK = 4; //type = 4: userRank -> cost
//            return DataCenter.Instance.GetUnitValue(TYPE_MAXCOST_OF_RANK, DataCenter.Instance.UserInfo.Rank); 
//        }
//    }
//
//    /// <summary>
//    /// Gets the unit value.  1 =  exp. 2 = attack. 3 = hp. 4 = rankCost
//    /// </summary>
//    /// <returns>The unit value.</returns>
//    /// <param name="type">Type.</param>
//    /// <param name="level">Level.</param>
//

//
//    //Temp
//    //public static List<int> HaveFriend = new List<int>(){};
//
//    private static GameObject itemObject;
//    public static GameObject ItemObject {
//        get {
//            if (itemObject == null) {
//                itemObject = ResourceManager.Instance.LoadLocalAsset("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
//            }
//            return itemObject;
//        }
//    }
//
//    private static UnitBaseInfo friendBaseInfo ;
//    public static UnitBaseInfo FriendBaseInfo {
//        get {
//            if (friendBaseInfo == null) {
//                friendBaseInfo = unitBaseInfo[195];
//
//            }
//            return friendBaseInfo;
//        }
//    }
//	
//
////	public static object GetEffect (Effect effect) {
////		int type = (int)effect;
////		object obj = null;
//
////		if (effect == Effect.DragCard) {
////			if (!tempEffect.TryGetValue (type, out obj)) {
////				string path = GetEffectPath(type);
////				obj = ResourceManager.Instance.LoadLocalAsset(path);
////				tempEffect.Add(type,obj);
////			}
////		}
////	}
//
//    public static Object GetEffect(int type) {
//        Object obj = null;
//        if (!tempEffect.TryGetValue(type, out obj)) {
//            string path = GetEffectPath(type);
//            obj = ResourceManager.Instance.LoadLocalAsset(path);
//            tempEffect.Add(type, obj);
//        }
//        return obj;
//    }
//
//    static string GetEffectPath(int type) {
//        string path = string.Empty;
//        switch (type) {
//        case 1:
//            path = "Effect/fire";
//            break;
//        case 2:
//            path = "Effect/water";
//            break;
//        case 3:
//            path = "Effect/wind";
//            break;
//        case 8:
//            path = "Effect/card_effect";
//            break;
//        default:
//            break;
//        }
//        return path;
//    }
//}
//
////public enum Effect {
////	DragCard = 8,
////}
