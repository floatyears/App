using UnityEngine;
using System.Collections.Generic;

#region old
public interface IUIInterface 
{
	/// <summary>
	/// ui name
	/// </summary>
	/// <value>The name.</value>
	string UIName{get;}
	
	/// <summary>
	/// creat ui object
	/// </summary>
	/// <param name="UIRoot">User interface root.</param>
	void CreatUI();

	/// <summary>
	/// show ui object on screen
	/// </summary>
	void ShowUI();

	/// <summary>
	/// hide ui object
	/// </summary>
	void HideUI();

	/// <summary>
	/// destory ui object
	/// </summary>
	void DestoryUI();

	UIState GetState{get;}

	SceneEnum GetScene{get;set;}
}
#endregion

//------------------------------------------------------------------------------------------------------------------------
// new code
//------------------------------------------------------------------------------------------------------------------------

/// <summary>
/// view interface
/// </summary>
public interface IUIBaseComponent {
	UIInsConfig uiConfig{ get;}
	void ShowUI();
	void HideUI();
	void DestoryUI();
}

public interface IUIComponentUnity :  IUIBaseComponent{
	void Init(UIInsConfig config, IUIOrigin originInterface);
}

/// <summary>
/// logic ui interface
/// </summary>
public interface IUIComponent :  IUIBaseComponent{
	void CreatUI();
}

public interface IUIOrigin {

}

/// <summary>
/// ui callback interface
/// </summary>
public interface IUICallback : IUIOrigin {
	void Callback(object data);
}

public interface IUISetBool : IUIOrigin {
	void SetEnable(bool b);
}

public interface ILeaderSkill {
	Dictionary<uint,ProtobufDataBase> LeadSkill { get;}
	Dictionary<int,TUserUnit> UserUnit { get ;}
}

public interface ILeadSkillReduceHurt {
	float ReduceHurtValue(float hurt,int type);
}

public interface ILeaderSkillMultipleAttack {
	float MultipleAttack(List<AttackInfo> attackInfo);
}

public interface ILeaderSkillExtraAttack {
	List<AttackInfo> ExtraAttack ();
}

public interface ILeaderSkillSwitchCard {
	List<int> SwitchCard (List<int> cardQuene);
	int SwitchCard (int card);
}

public interface ILeaderSkillRecoverHP {
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">Type. 0 = right now. 1 = every round. 2 = every step.</param>
	int RecoverHP(int blood,int type);
}

public interface IActiveSkillExcute {
	bool CoolingDone{ get;}
	void RefreashCooling();
	object Excute(uint userUnitID, int atk = -1);
}

public interface IEffectConcrete {
	void Play (List<GameObject> effect,AttackInfo ai);
}

/// <summary>
/// effect colleciton
/// </summary>
public interface IEffectBehavior {
	List<GameObject> EffectAssetList { set; get;}
	void CollectEffectExcute();
	void Excute(List<Vector3> position);
}

/// <summary>
/// single effect implement
/// </summary>
public interface IEffectExcute {
	float AnimTime { set; get;}
	Vector3 StartPosition { set; get;}
	Vector3 EndPosition { set; get;}
	GameObject TargetObject { set; get;}
	Callback EffectStartCallback{ set;}
	Callback EffectUpdateCallback{ set; }
	void Excute(Callback EndCallback);
}


public interface ITrapExcute {
	void Excute();
}

public interface IExcutePassiveSkill {
	List<AttackInfo> Dispose(int attackType ,int attack);
	void DisposeTrap(bool isAvoid);
}

public interface IPassiveExcute {
	object Excute(object trapBase, IExcutePassiveSkill excutePS);
}

public interface IUIParty : IUICallback {
	void PartyPaging( object textures);
}

public interface IUIFriendList : IUICallback {
	void CustomExtraFunction( object message);
}

public interface INetBase {
	void Send();
	void Receive(IWWWPost post);
}

public interface IWWWPost {
	WWW WwwInfo { get; set;}
	string Url  { get; set;}
	int Version { get; set;}

	void ExcuteCallback();
	void Send(INetBase callback,WWWForm wf);
	void Send(INetBase nettemp, string urlPath, byte[] data);
}

public interface INetSendPost {
	void SendHttpPost(IWWWPost post);
	void SendAssetPost(IWWWPost post);
}

public delegate void Callback();

public delegate void UICallback(GameObject caller);

public delegate void UICallback<T>(T arg1);

public delegate void DataListener(object data);

public delegate void HttpCallback(NetworkBase network);

public delegate void NetCallback(IWWWPost post);
