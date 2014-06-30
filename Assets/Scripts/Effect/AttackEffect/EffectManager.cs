using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

public class EffectManager {
	public enum EffectEnum {
		DragCard,

//		PassiveAntiAttack,
//
//		LeaderSkillExtrackAttack,
	}

	private static EffectManager instance;
	public static EffectManager Instance {
		get{
			if(instance == null) {
				instance = new EffectManager();
			}
			return instance;
		}
	}

	public GameObject effectPanel;

	public const int DragCardEffect = -1000;

	private Dictionary<int, string> effectName = new Dictionary<int, string>();
	private Dictionary<int, GameObject> effectObject = new Dictionary<int, GameObject>();
	private Dictionary<string, GameObject> skillEffectObject = new Dictionary<string, GameObject> ();

	public void GetOtherEffect(EffectEnum effect, ResourceCallback resouceCB) {
		string path = "";
		switch (effect) {
		case EffectEnum.DragCard:
			path = "card_effect";
			break;
//		case EffectEnum.PassiveAntiAttack:
//			path = "PS-fight-back";
//			break;
//		case EffectEnum.LeaderSkillExtrackAttack:
//			path = "LS-pursuit";
//			break;
		}

		if (path == "") {
			resouceCB (null);
			return;
		}
			
		GetEffectFromCache (path, resouceCB);
	}

	public void GetMapEffect(bbproto.EQuestGridType type, ResourceCallback resourceCB) {
		string path = "";
		switch (type) {
		case bbproto.EQuestGridType.Q_ENEMY:
			path = "Enconuterenemy";
			break;
		case bbproto.EQuestGridType.Q_EXCLAMATION:
			break;
		case bbproto.EQuestGridType.Q_KEY:
			break;
		case bbproto.EQuestGridType.Q_NONE:
			break;
		case bbproto.EQuestGridType.Q_QUESTION:
			break;
		case bbproto.EQuestGridType.Q_TRAP:
			path = "Trap";
			break;
		case bbproto.EQuestGridType.Q_TREATURE:
			break;
		}

		if (path == "") {
			resourceCB(null);
			return;
		}

		GetEffectFromCache (path, resourceCB);
	}

	public void GetSkillEffectObject(int skillID, string userUnitID, ResourceCallback resouceCb) {
		if (skillID == 0) {
			Debug.LogError ("skillStoreID : " + skillID + " userUnitID : " + userUnitID);
			resouceCb(null);
			return;	
		}
		string skillStoreID = DataCenter.Instance.GetSkillID(userUnitID, skillID);
//		Debug.LogError ("skillStoreID : " + skillStoreID + " userUnitID : " + userUnitID + " skillid :" + skillID);
		SkillBaseInfo sbi = DataCenter.Instance.AllSkill[skillStoreID];
		string path = "";
		TNormalSkill tns = sbi as TNormalSkill;
		if (tns != null) {
			path = GetNormalSkillEffectName (tns);
		} else if (sbi is ActiveSkill) {
			//ActiveAttackTargetType, ActiveChangeCardColor, ActiveDeferAttackRound, ActiveDelayTime, ActiveReduceDefense, ActiveReduceHurt, TSkillSingleAttack, ActiveStrengthenAttack,
			//TSkillAttackRecoverHP, GravityAttack, KnockdownAttack, TSkillRecoverSP, TSkillPoison, TSkillSuicideAttack, RecoverSP
			StringBuilder sb = new StringBuilder ();
			sb.Append ("as-");
			Type type = sbi.GetType ();
			if (type == typeof(TSkillSingleAttack)) {
					GetSingleAttackEffectName (sbi as TSkillSingleAttack, sb);
			} else if (type == typeof(ActiveAttackTargetType)) {
					GetAttackTargetType (sbi as ActiveAttackTargetType, sb);
			} else if (type == typeof(ActiveChangeCardColor)) {
					sb.Append ("color");
			} else if (type == typeof(ActiveDeferAttackRound)) {
					sb.Append ("low");
			} else if (type == typeof(ActiveDelayTime)) {
					sb.Append ("delay");
			} else if (type == typeof(ActiveReduceDefense)) {
					sb.Append ("reduce-def");
			} else if (type == typeof(ActiveReduceHurt)) {
					sb.Append ("reduce-injure");
			} else if (type == typeof(TSkillAttackRecoverHP)) {
					sb.Append ("single-blood-purple");
			} else if (type == typeof(GravityAttack)) {
					sb.Append ("all-2-dark");
			} else if (type == typeof(KnockdownAttack)) {
					sb.Append ("single-2-dark");
			} else if (type == typeof(TSkillRecoverSP)) {
					sb.Append ("sp-recover");
			} else if (type == typeof(TSkillPoison)) {
					sb.Append ("poison");
			} else if (type == typeof(TSkillSuicideAttack)) {
					TSkillSuicideAttack tsa = sbi as TSkillSuicideAttack;
					sb.Append (GetAttackRanger (tsa.AttackRange));
					sb.Append (GetAttackDanger (1, 2)); //2 == second effect.
					sb.Append (GetSkillType (tsa.AttackUnitType));
			}
			path = sb.ToString ();
		} else if (sbi is TSkillExtraAttack) {
			path = "LS-pursuit";
		}else if(sbi is TSkillAntiAttack) {
			path = "PS-fight-back";
		}
//		Debug.LogError ("path : " + path);
		GetEffectFromCache (path, resouceCb);
	}

	void GetEffectFromCache(string path, ResourceCallback resouceCallback) {

		string reallyPath = "Effect/effect/" + path;
//		Debug.LogError ("reallyPath : " + reallyPath);
		if (skillEffectObject.ContainsKey (reallyPath)) {
			resouceCallback(skillEffectObject[reallyPath]);
			return;
		}

		ResourceManager.Instance.LoadLocalAsset(reallyPath, o => { 
			if(o != null) { 
				skillEffectObject.Add(reallyPath,o as GameObject);
			} 
			resouceCallback(o);
		} 
		);
	}

	void GetAttackTargetType(ActiveAttackTargetType aatt,StringBuilder sb) {
		sb.Append(GetAttackRanger(aatt.AttackRange));
		if(aatt.ValueType == bbproto.EValueType.FIXED) {
			sb.Append("1-");
		}
		else{
			sb.Append(GetAttackDanger(aatt.AttackRange ,aatt.AttackValue));
		}
		sb.Append (GetSkillType (aatt.AttackType));
	}

	void GetSingleAttackEffectName(TSkillSingleAttack tssa,StringBuilder sb) {
		sb.Append(GetAttackRanger(tssa.AttackRange));
		if(tssa.ValueType == bbproto.EValueType.FIXED) {
			sb.Append("1-");
		}
		else{
			sb.Append(GetAttackDanger(tssa.AttackRange, tssa.AttackValue));
		}
		sb.Append (GetSkillType (tssa.AttackType));
	}

	string GetNormalSkillEffectName(TNormalSkill tns) {
		StringBuilder sb = new StringBuilder ();
		sb.Append("ns-");
		sb.Append (GetAttackRanger (tns.AttackRange));
		sb.Append (GetAttackDanger (tns.AttackRange, tns.Object.attackValue));
		sb.Append(GetSkillType(tns.AttackType));
//		Debug.LogError ("tns.AttackRange : " + tns.AttackRange + " tns.Object.attackValue : " + tns.Object.attackValue + " tns.AttackType : " + tns.AttackType + " sb : "+ sb.ToString());
		return sb.ToString ();
	}

	string GetAttackRanger(int attackRange) {
		if(attackRange == 0) {
			return "single-";
		}
		else {
			return "all-";
		}
	}
	
	string GetAttackDanger(int attackRange, float attackValue) {
		if (attackRange == 0) {
			if (attackValue < 2.3f) {	
					return "1-";
			} else {
					return "2-";
			}
		} else {
			if (attackValue < 1.8f) {
				return "1-";
			} else {
				return "2-";
			}
		}

	}

	string GetSkillType(int type) {
		switch (type) {
			case 0:
			return "all";
			case 1:
			return "fire";
			case 2:
			return "water";
			case 3:
			return "wind";
			case 4:
			return "light";
			case 5:
			return "dark";
			case 6:
			return "none";
			case 7:
			return "heart";
		}
		return "";
	}

	void SetName() {
//		effectName.Add (4015, "CFX3_Hit_Fire_A_Air");				//fire single
//		effectName.Add (4026, "firerain"); 							//fire all
//		effectName.Add (4061, "firerain"); 							//fire all
//		effectName.Add (4018, "linhunqiu2"); 						//light single
//		effectName.Add (4004, "CFXM3_Hit_Ice_B_Air");				//water single
//		effectName.Add (4017, "zhua");								//wind singele
//		effectName.Add (4028, "zhua");								//wind single
//		effectName.Add (4062, "liandao");							//wind single

		effectName.Add (4021, "effect/ns-all-1-fire");		
		effectName.Add (4038, "effect/ns-all-2-fire");

		effectName.Add (4003, "effect/ns-single-2-fire");	
		effectName.Add (4005, "effect/ns-single-2-water");	

		effectName.Add (1024, "effect/as-all-1-fire");
		effectName.Add (1067, "effect/as-single-1-fire02");
		effectName.Add (1055, "effect/as-single-1-fire02");	
		effectName.Add (1097, "effect/as-reduce-def03");

		effectName.Add (DragCardEffect, "card_effect");
	}

	private Dictionary<string,Type> effectCommand = new Dictionary<string, Type> ();
	private EffectManager() {
//		SetName ();
//		foreach (var item in effectName) {
//			ResourceManager.Instance.LoadLocalAsset("Effect/"+item.Value,o => effectObject.Add(item.Key,o as GameObject));
//		}
	}

	public static GameObject InstantiateEffect(GameObject parent, GameObject obj) {
		Vector3 localScale = obj.transform.localScale;
		Vector3 rotation = obj.transform.eulerAngles;
		GameObject effectIns =  NGUITools.AddChild(parent, obj);
//		effectIns.layer = GameLayer.EffectLayer;
		effectIns.transform.localScale = localScale;
		effectIns.transform.eulerAngles = rotation;
		return effectIns;
	}
}
