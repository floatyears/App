using UnityEngine;
using System.Collections.Generic;
using bbproto;

public enum ModelEnum {
    UserInfo = 100,
    AccountInfo,
    PartyInfo,
    MyUnitList,
    UserUnitList,
    UnitValue,
    Skill,
    UnitInfo,
    EnemyInfo,
    UnitBaseInfo,
    TrapInfo,

    User            = 1000,
    UnitPartyInfo   = 1001,
    
    UIInsConfig     = 2000,
    MapConfig       = 2001,
}

public class DataCenter {
    public TUserInfo UserInfo { 
        get { return getData(ModelEnum.UserInfo) as TUnitInfo; } 
        set { setData(ModelEnum.UserInfo, value); } 
    };
    public TAccountInfo AccountInfo {
        get { return getData(ModelEnum.AccountInfo) as TAccountInfo; }
        set { setData(ModelEnum.AccountInfo, value); }
    };
    public List<TFriendInfo> Friends { 
        get { return getData(ModelEnum.AccountInfo) as List<TFriendInfo>; }
        set { setData(ModelEnum.AccountInfo, value); } 
    };
    public TPartyInfo PartyInfo { 
        get { return getData(ModelEnum.PartyInfo) as TPartyInfo; }
        set { setData(ModelEnum.PartyInfo, value); }
    };

    //TODO: reconstruct myUnitList
    public UserUnitList MyUnitList { 
        get { 
            UserUnitList ret = getData(ModelEnum.MyUnitList);
            if (ret == null){
                ret = new UserUnitList();
                setData(ModelEnum.MyUnitList, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.MyUnitList, value); } 
    };
    public UserUnitList UserUnitList{
            get { 
                UserUnitList ret = getData(ModelEnum.UserUnitList);
                if (ret == null){
                    ret = new UserUnitList();
                    setData(ModelEnum.UserUnitList, ret);
                }
                return ret; 
            }
            set { setData(ModelEnum.UserUnitList, value); } 
    };

    // unit configs table(come from config file: ) e.g.<hp, hpLevelConfigList>
    public Dictionary<int,TPowerTableInfo> UnitValue{
        get { 
            Dictionary<int,TPowerTableInfo> ret = getData(ModelEnum.UnitValue);
            if (ret == null){
                ret = new Dictionary<int,TPowerTableInfo>();
                setData(ModelEnum.UnitValue, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitValue, value); } 
    };

    public Dictionary<int, SkillBaseInfo> Skill {
        get { 
            Dictionary<int, SkillBaseInfo> ret = getData(ModelEnum.Skill);
            if (ret == null){
                ret = new Dictionary<int, SkillBaseInfo>();
                setData(ModelEnum.Skill, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.Skill, value); } 
    };

    public Dictionary<uint, TUnitInfo>  UnitInfo{
        get { 
            Dictionary<uint, TUnitInfo> ret = getData(ModelEnum.UnitInfo);
            if (ret == null){
                ret = new Dictionary<uint, TUnitInfo>();
                setData(ModelEnum.UnitInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitInfo, value); } 
    };

    public Dictionary<uint, TEnemyInfo> EnemyInfo{
        get { 
            Dictionary<uint, TEnemyInfo> ret = getData(ModelEnum.EnemyInfo);
            if (ret == null){
                ret = new Dictionary<uint, TEnemyInfo>();
                setData(ModelEnum.EnemyInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.EnemyInfo, value); } 
    };

    public Dictionary<int, UnitBaseInfo> UnitBaseInfo {
        get { 
            Dictionary<int, UnitBaseInfo> ret = getData(ModelEnum.UnitBaseInfo);
            if (ret == null){
                ret = new Dictionary<int, UnitBaseInfo>();
                setData(ModelEnum.UnitBaseInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.UnitBaseInfo, value); } 
    };

    public Dictionary<uint, TrapBase> TrapInfo {
        get { 
            Dictionary<uint, TrapBase> ret = getData(ModelEnum.TrapInfo);
            if (ret == null){
                ret = new Dictionary<uint, TrapBase>();
                setData(ModelEnum.TrapInfo, ret);
            }
            return ret; 
        }
        set { setData(ModelEnum.TrapInfo, value); } 
    };
    
    public const int maxEnergyPoint = 20;
    public const int posStart = 0;
    public const int posEnd = 5;
    public const int minNeedCard = 2;
    public const int maxNeedCard = 5;

    public static DataCenter Instance {
        get {
            if (instance == null) {
                instance = new GlobalData();
            }
            return instance;
        }
        
    }

    private static DataCenter instance;
    private DataCenter() {
    }

    private void setData(ModelEnum modelType, object modelData) {
        ModelManager.Instance.SetData(modelType, modelData);
    }

    private object getData(ModelEnum modelType) {
        return ModelManager.Instance.GetData(modelType);
    }
}