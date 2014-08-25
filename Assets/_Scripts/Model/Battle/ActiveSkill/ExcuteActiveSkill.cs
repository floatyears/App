using UnityEngine;
using System.Collections.Generic;

public class ExcuteActiveSkill {
	private Dictionary<string,IActiveSkillExcute> activeSkill = new Dictionary<string, IActiveSkillExcute> ();
	private ILeaderSkill leaderSkill;

	private IActiveSkillExcute iase;
	private TUserUnit userUnit;

	private const float fixEffectTime = 2f;

	public static float singleEffectTime = 2f;

	/// <summary>
	/// novice guide active skill cooling done.
	/// </summary>
	public static void CoolingDoneLeaderActiveSkill() {
		TUserUnit tuu = DataCenter.Instance.PartyInfo.CurrentParty.UserUnit [0];
		ActiveSkill sbi = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), tuu.UnitInfo.ActiveSkill, SkillType.ActiveSkill) as ActiveSkill;
		sbi.skillBase.skillCooling = 0;
		sbi.RefreashCooling ();
	}


	public ExcuteActiveSkill(ILeaderSkill ils) {
		leaderSkill = ils;
		foreach (var item in ils.UserUnit.Values) {
			if (item==null ){
				continue;
			}
			ProtobufDataBase pudb = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), item.ActiveSkill, SkillType.ActiveSkill); //Skill[item.ActiveSkill];
			IActiveSkillExcute skill = pudb as IActiveSkillExcute;
			if(skill == null) {
				continue;
			}
			activeSkill.Add(item.MakeUserUnitKey(), skill);
			skill.StoreSkillCooling(item.MakeUserUnitKey());
		}

		MsgCenter.Instance.AddListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);	// this command use to reduce cooling one round.
//		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, ShowHands);	// one normal skill can reduce cooling one round.
	}

	public void RemoveListener () {
		MsgCenter.Instance.RemoveListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);
//		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, ShowHands);
	}

	public IActiveSkillExcute GetActiveSkill(string userUnitID) {
		IActiveSkillExcute iase = null;
		activeSkill.TryGetValue (userUnitID, out iase);
		return iase;
	}
	AttackInfo ai;
	void Excute(object data) {
		userUnit = data as TUserUnit;
		if (userUnit != null) {
			string id = userUnit.MakeUserUnitKey();
			if(activeSkill.TryGetValue(id, out iase)) {
//				Debug.LogError("activeSkill.TryGetValue true  : " + iase);

				MsgCenter.Instance.Invoke(CommandEnum.StateInfo, DGTools.stateInfo[4]);

				AudioManager.Instance.PlayAudio(AudioEnum.sound_active_skill);

				ai = AttackInfo.GetInstance();
				ai.UserUnitID = userUnit.MakeUserUnitKey();
				ai.SkillID = (iase as ActiveSkill).skillBase.id;
				MsgCenter.Instance.Invoke(CommandEnum.ShowActiveSkill, ai);
				GameTimer.GetInstance().AddCountDown(AttackEffect.activeSkillEffectTime, WaitActiveEffect);
			} else {
//				Debug.LogError("activeSkill.TryGetValue false  : ");
			}
		}
	}
	
     void WaitActiveEffect() {
//		Debug.LogError("WaitActiveEffect ");
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, true);
		GameTimer.GetInstance().AddCountDown(1f,Excute);
		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillStandReady, userUnit);

		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_appear);
	}
   
	void Excute() {
//		Debug.LogError ("Excute active skill iase: " + iase + " userUnit : " + userUnit);
		if (iase == null || userUnit == null) {
			return;	
		}

		iase = activeSkill[ai.UserUnitID];
		iase.Excute(ai.UserUnitID, userUnit.Attack);
		iase = null;
		userUnit = null;
		GameTimer.GetInstance ().AddCountDown (fixEffectTime + singleEffectTime, ActiveSkillEnd);
	}

	void ActiveSkillEnd() {
//		Debug.LogError ("ActiveSkillEnd");
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, false);
	}

	void MoveToMapItem(object data) {
		CoolingSkill ();
	}

	void ReduceActiveSkillRound(object data) {
		CoolingSkill ();
	}

	List<IActiveSkillExcute> coolingDoneSkill = new List<IActiveSkillExcute>();
	public void CoolingSkill () {
		foreach (var item in activeSkill.Values) {
			item.RefreashCooling();
		}
	}

	public void ResetSkill() {
		foreach (var item in activeSkill.Values) {
			item.InitCooling ();
		}
	}
}
