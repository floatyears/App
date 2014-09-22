using UnityEngine;
using System.Collections.Generic;

public interface ILeaderSkill {
    Dictionary<string, ProtobufDataBase> LeadSkill { get; }
    Dictionary<int,TUserUnit> UserUnit { get ; }
}

public interface ILeadSkillReduceHurt {
    float ReduceHurtValue(float hurt, int type);
}

public interface ILeaderSkillMultipleAttack {
    float MultipleAttack(List<AttackInfo> attackInfo);
}

public interface ILeaderSkillExtraAttack {
	List<AttackInfo> ExtraAttack();
}

public interface ILeaderSkillSwitchCard {
    List<int> SwitchCard(List<int> cardQuene);
    int SwitchCard(int card);
}

public interface ILeaderSkillRecoverHP {
    /// <summary>
    /// Recovers the H.
    /// </summary>
    /// <returns>The H.</returns>
    /// <param name="blood">Blood.</param>
    /// <param name="type">Type. 0 = right now. 1 = every round. 2 = every step.</param>
    int RecoverHP(int blood, int type);
}

//public interface IActiveSkillExcute {
//    bool CoolingDone{ get; }
//    void RefreashCooling();
//    object Excute(string userUnitID, int atk = -1);
//	AttackInfo ExcuteByDisk(AttackInfo ai);
//	void StoreSkillCooling(string id);
//	void InitCooling();
////	void ReadSkillCooling();
//}

public interface IEffectConcrete {
    void Play(List<GameObject> effect, AttackInfo ai);
}

/// <summary>
/// effect colleciton
/// </summary>
public interface IEffectBehavior {
    List<GameObject> EffectAssetList { set; get; }
    void CollectEffectExcute();
    void Excute(List<Vector3> position);
}

/// <summary>
/// single effect implement
/// </summary>
public interface IEffectExcute {
    float AnimTime { set; get; }
    Vector3 StartPosition { set; get; }
    Vector3 EndPosition { set; get; }
    GameObject TargetObject { set; get; }
    Callback EffectStartCallback{ set; }
    Callback EffectUpdateCallback{ set; }
    void Excute(Callback EndCallback);
}


//public interface ITrapExcute {
//    void Excute();
//	void ExcuteByDisk();
//}

//public interface IExcutePassiveSkill {
//    List<AttackInfo> Dispose(int attackType, int attack);
//    void DisposeTrap(bool isAvoid);
//}

//public interface IPassiveExcute {
//	SkillBaseInfo skillBaseInfo { get; }
//    object Excute(object trapBase, IExcutePassiveSkill excutePS);
//}

//public interface IUIParty  {
//    void PartyPaging(object textures);
//}
//
//public interface IUIFriendList {
//    void CustomExtraFunction(object message);
//}

//public interface INetBase {
//    void OnRequest(object data, DataListener callback);
////    void OnRequest(ResponseCallback callback);
////	void OnResponse(IWWWPost post);
//}

//public interface IWWWPost {
//    WWW WwwInfo { get; set; }
//    string Url  { get; set; }
//    int Version { get; set; }
//
//    void ExcuteCallback();
//    void Send(ControllerBase callback, WWWForm wf);
//    void Send(ControllerBase nettemp, string urlPath, byte[] data);
//}

//public interface INetSendPost {
//    void SendHttpPost(IWWWPost post);
//    void SendAssetPost(IWWWPost post);
//}


public delegate void Callback();

public delegate void UICallback(GameObject caller);

public delegate Object UICallbackExtend(Object data);

public delegate void UICallback<T>(T arg1);

public delegate void DataListener(object data);

public delegate void ResourceCallback(Object data);

public delegate void ResponseCallback(ErrorMsg errMsg,object data);

//public delegate void HttpCallback(NetworkBase network);

public delegate void NetCallback(object data);
