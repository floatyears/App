using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class ConfigSkill  {
	public ConfigSkill() {
		ConfigNormalSkill ();
		ConfigLeadSkill ();
		ConfigActiveSkill ();
	}

	void ConfigNormalSkill () {
		//------------------------------------------------------------------------------------------------//
		//1,2
		//------------------------------------------------------------------------------------------------//
		NormalSkill ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 0;
		ns.baseInfo.name = "no 0 normal skill";
		ns.baseInfo.description = "two red card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		TempNormalSkill tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 1;
		ns.baseInfo.name = "no 1 normal skill";
		ns.baseInfo.description = "five red card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//3,4
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 2;
		ns.baseInfo.name = "no 2 normal skill";
		ns.baseInfo.description = "two water card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
//		ns.activeBlocks.Add (2);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 3;
		ns.baseInfo.name = "no 3 normal skill";
		ns.baseInfo.description = "five water card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//5,6
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 4;
		ns.baseInfo.name = "no 4 normal skill";
		ns.baseInfo.description = "two wind card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 5;
		ns.baseInfo.name = "no 5 normal skill";
		ns.baseInfo.description = "five wind card generate one";
		ns.attackValue = 2.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (7);
		ns.activeBlocks.Add (7);
		ns.activeBlocks.Add (5);
//		ns.activeBlocks.Add (3);
//		ns.activeBlocks.Add (3);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//7,8
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 6;
		ns.baseInfo.name = "no 6 normal skill";
		ns.baseInfo.description = "two light card generate one";
		ns.attackValue = 1.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (4);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 7;
		ns.baseInfo.name = "no 7 normal skill";
		ns.baseInfo.description = "three light card and two dark card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
//		ns.activeBlocks.Add (4);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//9,10
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 8;
		ns.baseInfo.name = "no 8 normal skill";
		ns.baseInfo.description = "three light card and two dark card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (5);
		ns.activeBlocks.Add (5);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 9;
		ns.baseInfo.name = "no 9 normal skill";
		ns.baseInfo.description = "four dark card and one light card generate one";
		ns.attackValue = 3.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (5);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 10;
		ns.baseInfo.name = "no 10 normal skill";
		ns.baseInfo.description = "two nothing card generate one";
		ns.attackValue = 2f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (5);
		ns.activeBlocks.Add (5);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 11;
		ns.baseInfo.name = "no 11 normal skill";
		ns.baseInfo.description = "every card have one generate one";
		ns.attackValue = 3.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (5);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 12;
		ns.baseInfo.name = "no 12 normal skill";
		ns.baseInfo.description = "two heart card generate one";
		ns.attackValue = 0.2f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 13;
		ns.baseInfo.name = "no 13 normal skill";
		ns.baseInfo.description = "three heart card generate one";
		ns.attackValue = 0.4f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 14;
		ns.baseInfo.name = "no 14 normal skill";
		ns.baseInfo.description = "four heart card generate one";
		ns.attackValue = 0.6f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 15;
		ns.baseInfo.name = "no 15 normal skill";
		ns.baseInfo.description = "four heart card generate one";
		ns.attackValue = 0.8f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 16;
		ns.baseInfo.name = "no 16 normal skill";
		ns.baseInfo.description = "five heart card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
		ConfigNormalSkill2 ();
		ConfigHeartSkill ();
	}

	void ConfigNormalSkill2 () { 
		NormalSkill ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 30;
		ns.baseInfo.name = "no 16 normal skill";
		ns.baseInfo.description = "five heart card generate one";
		ns.attackValue = 1.6f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		TempNormalSkill tns = new TempNormalSkill (ns);
		GlobalData.tempNormalSkill.Add (ns.baseInfo.id, tns);
	}

	void ConfigHeartSkill () {
		for (int i = 2; i < 6; i++) {
			NormalSkill ns			= new NormalSkill ();
			ns.baseInfo				= new SkillBase ();
			ns.baseInfo.id			= 24 + i;
			ns.baseInfo.name		= "no" +  ns.baseInfo.id + "normal skill";
			ns.baseInfo.description = (i + 2) + " herat card generate one";
			ns.attackValue = 0.15f * (i - 1);
			ns.attackType = EAttackType.RECOVER_HP;
			for (int j = 0; j < i; j++) {
				ns.activeBlocks.Add(7);
			}
			TempNormalSkill tns = new TempNormalSkill(ns);
			GlobalData.tempNormalSkill.Add(ns.baseInfo.id,tns);
		}
	}
	
	void ConfigLeadSkill() {
		SkillBoost sb = new SkillBoost ();
		sb.baseInfo = new SkillBase ();
		sb.baseInfo.id = 17;
		sb.baseInfo.name = "no 17 leader skill";
		sb.baseInfo.description = "boost attack leader skill";
		sb.boostType = EBoostType.BOOST_ATTACK;
		sb.boostValue = 2f;
		sb.targetType = EBoostTarget.UNIT_TYPE;
		sb.targetValue = 0;
		TempBoostSkill tbs = new TempBoostSkill (sb);
		GlobalData.tempNormalSkill.Add (sb.baseInfo.id, tbs);
		
		sb = new SkillBoost ();
		sb.baseInfo = new SkillBase ();
		sb.baseInfo.id = 18;
		sb.baseInfo.name = "no 18 leader skill";
		sb.baseInfo.description = "boost hp leader skill";
		sb.boostType = EBoostType.BOOST_HP;
		sb.boostValue = 2f;
		sb.targetType = EBoostTarget.UNIT_RACE;
		sb.targetValue = 0;
		tbs = new TempBoostSkill(sb);
		GlobalData.tempNormalSkill.Add (sb.baseInfo.id, tbs);
		
		SkillRecoverHP srh = new SkillRecoverHP ();
		srh.baseInfo = new SkillBase ();
		srh.baseInfo.id = 19;
		srh.baseInfo.name = "no 19 recover hp";
		srh.baseInfo.description = "fixed recover hp";
		srh.type = EValueType.FIXED;
		srh.value = 100f;
		srh.period = EPeriod.EP_EVERY_ROUND;
		TempRecoverHP trh = new TempRecoverHP (srh);
		GlobalData.tempNormalSkill.Add (srh.baseInfo.id, trh);
		
		srh = new SkillRecoverHP ();
		srh.baseInfo = new SkillBase ();
		srh.baseInfo.id = 20;
		srh.baseInfo.name = "no 20 recover hp";
		srh.baseInfo.description = "percent recover hp";
		srh.type = EValueType.PERCENT;
		srh.value = 0.05f;
		srh.period = EPeriod.EP_EVERY_STEP;
		trh = new TempRecoverHP (srh);
		GlobalData.tempNormalSkill.Add (srh.baseInfo.id, trh);
		
		SkillReduceHurt sreduce = new SkillReduceHurt();
		sreduce.baseInfo = new SkillBase ();
		sreduce.baseInfo.id = 21;
		sreduce.baseInfo.name = "no 21 reduce hurt";
		sreduce.baseInfo.description = "reduce hurt every round";
		sreduce.type = EValueType.PERCENT;
		sreduce.unitType = EUnitType.UWIND;
		sreduce.value = 20f;
		sreduce.period = EPeriod.EP_EVERY_ROUND;
		sreduce.periodValue = 0;
		TempReduceHurt trhurt = new TempReduceHurt (sreduce);
		GlobalData.tempNormalSkill.Add (sreduce.baseInfo.id, trhurt);
		
		SkillDelayTime sdt = new SkillDelayTime ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 22;
		sdt.baseInfo.name = "no 22 skill delay time";
		sdt.baseInfo.description = "delay drag time";
		sdt.type = EValueType.FIXED;
		sdt.value = 1f;
		TempSkillTime tst = new TempSkillTime (sdt);
		GlobalData.tempNormalSkill.Add (sdt.baseInfo.id, tst);
		
		sdt = new SkillDelayTime ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 23;
		sdt.baseInfo.name = "no 23 skill delay time";
		sdt.baseInfo.description = "delay drag time";
		sdt.type = EValueType.FIXED;
		sdt.value = 2f;
		tst = new TempSkillTime (sdt);
		GlobalData.tempNormalSkill.Add (sdt.baseInfo.id, tst);
		
		SkillConvertUnitType scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 24;
		scut.baseInfo.name = "no 24 skill convert unit type";
		scut.baseInfo.description = "convert card color";
		scut.type = EValueType.COLORTYPE;
		scut.unitType1 = EUnitType.UFIRE;
		scut.unitType2 = EUnitType.UWATER;
		TempConvertUnitType tcut = new TempConvertUnitType (scut);
		GlobalData.tempNormalSkill.Add (scut.baseInfo.id, tcut);

	 	scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 31;
		scut.baseInfo.name = "no 31 skill convert unit type";
		scut.baseInfo.description = "convert card color";
		scut.type = EValueType.COLORTYPE;
		scut.unitType1 = EUnitType.UWATER;
		scut.unitType2 = EUnitType.UWIND;
		tcut = new TempConvertUnitType (scut);
		GlobalData.tempNormalSkill.Add (scut.baseInfo.id, tcut);
		
		SkillExtraAttack sea = new SkillExtraAttack ();
		sea.baseInfo = new SkillBase ();
		sea.baseInfo.id = 25;
		sea.baseInfo.name = "no 25 skill extra attack";
		sea.baseInfo.description = "extra all type attack";
		sea.unitType = EUnitType.UWIND;
		sea.attackValue = 2f;
		TempSkillExtraAttack tsea = new TempSkillExtraAttack (sea);
		GlobalData.tempNormalSkill.Add (sea.baseInfo.id, tsea);
	}

	void ConfigActiveSkill () {
		SkillSingleAttack ssa = new SkillSingleAttack ();
		ssa.baseInfo = new SkillBase ();
		ssa.baseInfo.id = 32;
		ssa.baseInfo.name = "no 32 skill single attack";
		ssa.baseInfo.description = "boost a single attack by type";
		ssa.type = EValueType.FIXED;
		ssa.unitType = EUnitType.UFIRE;
		ssa.value = 1000f;

		TempSkillSingleAttack tssa = new TempSkillSingleAttack (ssa);
		GlobalData.tempNormalSkill.Add (ssa.baseInfo.id, tssa);
	}
}

public class TempSkillSingleAttack : ProtobufDataBase{
	public TempSkillSingleAttack(object instance) : base (instance) {

	}


}

public class TempNormalSkill : ProtobufDataBase {
	public TempNormalSkill (object instance) : base (instance) {
		
	}

	//static int record = 0;
	public int CalculateCard (List<uint> count, int record = 0) {
		NormalSkill ns = DeserializeData<NormalSkill> ();

		while (count.Count >= ns.activeBlocks.Count) {
			bool isExcuteSkill =  DGTools.IsTriggerSkill<uint> (count, ns.activeBlocks);
			if (isExcuteSkill) {
				record++;
				for (int i = 0; i < ns.activeBlocks.Count; i++) {
					count.Remove(ns.activeBlocks[i]);
				}
			}
			else {
				break;
			}
		}

		//Debug.LogWarning("record -- : " + record + "``ns : " + ns.baseInfo.name);
		return record;
	}

	public void GetSkillInfo(AttackInfo ai) {
		NormalSkill ns = GetObject ();
		ai.SkillID = ns.baseInfo.id;
		ai.AttackRange = (int)ns.attackType;
		ai.NeedCardNumber = ns.activeBlocks.Count;
	}

	public int GetActiveBlocks() {
		NormalSkill ns = DeserializeData<NormalSkill> ();
		return ns.activeBlocks.Count;
	}

	public void DisposeUseSkillID (List<int> skillID) {
		NormalSkill ns = DeserializeData<NormalSkill> ();
		if (skillID.Contains (ns.baseInfo.id)) {
			skillID.Remove(ns.baseInfo.id);
		}
	}

	NormalSkill GetObject() {
		return  DeserializeData<NormalSkill> ();
	}

	public int GetID() {
		return GetObject().baseInfo.id;
	}

	public int GetAttackRange() {
		return (int)GetObject ().attackType;
	}

	public string GetName () {
		return GetObject ().baseInfo.name;
	}

	public int GetRecoverHP (int blood) {
		return System.Convert.ToInt32 (blood * GetObject ().attackValue);
	}

	public float GetAttack (float userUnitAttack) {
		return userUnitAttack * GetObject ().attackValue;
	}
}

public class TempSkillExtraAttack : ProtobufDataBase {
	public TempSkillExtraAttack (object instance) : base (instance) {
		
	}

	public AttackInfo AttackValue (float attackValue, int id) {
		AttackInfo ai = new AttackInfo ();
		ai.AttackValue = attackValue * DeserializeData<SkillExtraAttack> ().attackValue;
		ai.AttackType = (int)DeserializeData<SkillExtraAttack> ().unitType;
		ai.AttackRange = 1;//attack all enemy
		ai.UserUnitID = id;
		return ai;
	}
}

public class TempConvertUnitType : ProtobufDataBase {
	public TempConvertUnitType(object instance) : base (instance) {
		
	}

	public int SwitchCard (int type) {
		SkillConvertUnitType scut = DeserializeData<SkillConvertUnitType> ();
		if (scut.unitType2 == EUnitType.UALL) {
			List<int> range = new List<int>(Config.Instance.cardTypeID);// Config.Instance.cardTypeID
			range.Remove(type);
			int index = Random.Range(0,range.Count);
			type = range[index];
		}
		else if((int)scut.unitType1 == type) {
			type = (int)scut.unitType2;
		}

		return type;
	}
}

public class TempBoostSkill : ProtobufDataBase {
	public TempBoostSkill (object instance) : base (instance) {
		
	}

	SkillBoost Get(){
		return DeserializeData<SkillBoost> ();
	}

	public float GetBoostValue {
		get{
			return DeserializeData<SkillBoost> ().boostValue;
		}
	}

	public int GetTargetValue {
		get{
			return DeserializeData<SkillBoost> ().targetValue;
		}
	}

	/// <summary>
	/// attack = 0, hp = 1
	/// </summary>
	/// <returns>The boost type.</returns>
	public EBoostType GetBoostType {
		get{
			return Get ().boostType;
		}
	}

	/// <summary>
	/// race = 0, type = 1
	/// </summary>
	/// <returns>The target type.</returns>
	public EBoostTarget GetTargetType { 
		get {
			return Get ().targetType;
		}
	}
}

public class TempRecoverHP : ProtobufDataBase {
	public TempRecoverHP (object instance) : base (instance) {
		
	}

	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">1 = right now. 2 = every round. 3 = every step.</param>
	public int RecoverHP (int blood,int type) {
		SkillRecoverHP srhp = DeserializeData<SkillRecoverHP> ();
		if(type == (int)srhp.period){
			float tempBlood = blood;
			if(srhp.type == EValueType.FIXED) {
				tempBlood += srhp.value;
			}
			else if(srhp.type == EValueType.PERCENT) {
				tempBlood *= (1 + srhp.value);
			}
			blood = System.Convert.ToInt32(tempBlood);
		}
		return blood;
	}
}

public class TempReduceHurt : ProtobufDataBase {
	private int useCount = 0;
	public TempReduceHurt (object instance) : base (instance) {
		
	}

	public float ReduceHurt (float attackValue,int type) {
		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (srh.unitType == EUnitType.UALL || srh.unitType == (EUnitType)type) {
			if(srh.value > 100f) {
				Debug.LogError("ReduceHurt error : reduce proportion error ! ");
			}
			else{
				float proportion = 1f - (float)srh.value / 100f;
				attackValue *= proportion;
			}
		}
		if (srh.periodValue != 0) {
			useCount ++;
		}
		return attackValue;
	}

	public bool CheckUseDone () {
		SkillReduceHurt srh = DeserializeData<SkillReduceHurt> ();
		if (srh.periodValue == 0) {
			return false;
		}

		if (useCount >= srh.periodValue) {
			useCount = 0;
			return true;
		} 
		else {
			return false;
		}
	}

	/// <summary>
	/// 0 = right now, 1 = every round, 2 = every step.
	/// </summary>
	/// <returns>The duration.</returns>
	public int GetDuration() {
		return (int)DeserializeData<SkillReduceHurt> ().period;
	}
}

public class TempSkillTime : ProtobufDataBase {
	public TempSkillTime (object instance) : base (instance) {
		
	}

	public float DelayTime{
		get {
			return DeserializeData<SkillDelayTime>().value;
		}
	} 
}