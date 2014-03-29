using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class ConfigSkill  {
	public ConfigSkill() {
		ConfigNormalSkill ();
		ConfigLeadSkill ();
		ConfigActiveSkill ();
		ConfigActiveSkill2 ();
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
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UFIRE;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		TNormalSkill tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 1;
		ns.baseInfo.name = "no 1 normal skill";
		ns.baseInfo.description = "five red card generate one";
		ns.attackValue = 4f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UFIRE;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (1);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//3,4
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 2;
		ns.baseInfo.name = "no 2 normal skill";
		ns.baseInfo.description = "two water card generate one";
		ns.attackValue = 2f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UWATER;
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
//		ns.activeBlocks.Add (2);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 3;
		ns.baseInfo.name = "no 3 normal skill";
		ns.baseInfo.description = "five water card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.UWATER;
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (2);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		//5,6
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 4;
		ns.baseInfo.name = "no 4 normal skill";
		ns.baseInfo.description = "two wind card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.UWIND;
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 5;
		ns.baseInfo.name = "no 5 normal skill";
		ns.baseInfo.description = "five wind card generate one";
		ns.attackValue = 5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.UHeart;
		ns.activeBlocks.Add (7);
		ns.activeBlocks.Add (7);
		ns.activeBlocks.Add (5);
//		ns.activeBlocks.Add (3);
//		ns.activeBlocks.Add (3);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
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
		ns.attackUnitType = EUnitType.ULIGHT;
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (4);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 7;
		ns.baseInfo.name = "no 7 normal skill";
		ns.baseInfo.description = "three light card and two dark card generate one";
		ns.attackValue = 3f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.ULIGHT;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
//		ns.activeBlocks.Add (4);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
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
		ns.attackUnitType = EUnitType.UDARK;
		ns.activeBlocks.Add (5);
		ns.activeBlocks.Add (5);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 9;
		ns.baseInfo.name = "no 9 normal skill";
		ns.baseInfo.description = "four dark card and one light card generate one";
		ns.attackValue = 3.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.UDARK;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (5);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		//------------------------------------------------------------------------------------------------//
		
		//------------------------------------------------------------------------------------------------//
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 10;
		ns.baseInfo.name = "no 10 normal skill";
		ns.baseInfo.description = "two nothing card generate one";
		ns.attackValue = 2f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UDARK;
		ns.activeBlocks.Add (5);
		ns.activeBlocks.Add (5);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 11;
		ns.baseInfo.name = "no 11 normal skill";
		ns.baseInfo.description = "every card have one generate one";
		ns.attackValue = 3.5f;
		ns.attackType = EAttackType.ATK_ALL;
		ns.attackUnitType = EUnitType.UDARK;
		ns.activeBlocks.Add (1);
		ns.activeBlocks.Add (2);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (4);
		ns.activeBlocks.Add (5);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 12;
		ns.baseInfo.name = "no 12 normal skill";
		ns.baseInfo.description = "two heart card generate one";
		ns.attackValue = 0.2f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UNONE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 13;
		ns.baseInfo.name = "no 13 normal skill";
		ns.baseInfo.description = "three heart card generate one";
		ns.attackValue = 0.4f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UNONE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 14;
		ns.baseInfo.name = "no 14 normal skill";
		ns.baseInfo.description = "four heart card generate one";
		ns.attackValue = 0.6f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UNONE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 15;
		ns.baseInfo.name = "no 15 normal skill";
		ns.baseInfo.description = "four heart card generate one";
		ns.attackValue = 0.8f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UNONE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
		
		ns = new NormalSkill ();
		ns.baseInfo = new SkillBase ();
		ns.baseInfo.id = 16;
		ns.baseInfo.name = "no 16 normal skill";
		ns.baseInfo.description = "five heart card generate one";
		ns.attackValue = 1f;
		ns.attackType = EAttackType.ATK_SINGLE;
		ns.attackUnitType = EUnitType.UNONE;
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		ns.activeBlocks.Add (6);
		tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
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
		ns.attackUnitType = EUnitType.UWIND;
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		ns.activeBlocks.Add (3);
		TNormalSkill tns = new TNormalSkill (ns);
		DataCenter.Instance.Skill.Add (ns.baseInfo.id, tns);
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
			TNormalSkill tns = new TNormalSkill(ns);
			DataCenter.Instance.Skill.Add(ns.baseInfo.id,tns);
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
		TSkillBoost tbs = new TSkillBoost (sb);
		DataCenter.Instance.Skill.Add (sb.baseInfo.id, tbs);
		
		sb = new SkillBoost ();
		sb.baseInfo = new SkillBase ();
		sb.baseInfo.id = 18;
		sb.baseInfo.name = "no 18 leader skill";
		sb.baseInfo.description = "boost hp leader skill";
		sb.boostType = EBoostType.BOOST_HP;
		sb.boostValue = 2f;
		sb.targetType = EBoostTarget.UNIT_RACE;
		sb.targetValue = 0;
		tbs = new TSkillBoost(sb);
		DataCenter.Instance.Skill.Add (sb.baseInfo.id, tbs);
		
		SkillRecoverHP srh = new SkillRecoverHP ();
		srh.baseInfo = new SkillBase ();
		srh.baseInfo.id = 19;
		srh.baseInfo.name = "no 19 recover hp";
		srh.baseInfo.description = "fixed recover hp";
		srh.type = EValueType.FIXED;
		srh.value = 200f;
		srh.period = EPeriod.EP_EVERY_ROUND;
		TSkillRecoverHP trh = new TSkillRecoverHP (srh);
		DataCenter.Instance.Skill.Add (srh.baseInfo.id, trh);
		
		srh = new SkillRecoverHP ();
		srh.baseInfo = new SkillBase ();
		srh.baseInfo.id = 20;
		srh.baseInfo.name = "no 20 recover hp";
		srh.baseInfo.description = "percent recover hp";
		srh.type = EValueType.PERCENT;
		srh.value = 0.05f;
		srh.period = EPeriod.EP_EVERY_STEP;
		trh = new TSkillRecoverHP (srh);
		DataCenter.Instance.Skill.Add (srh.baseInfo.id, trh);
		
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
		TSkillReduceHurt trhurt = new TSkillReduceHurt (sreduce);
		DataCenter.Instance.Skill.Add (sreduce.baseInfo.id, trhurt);
		
		SkillDelayTime sdt = new SkillDelayTime ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 22;
		sdt.baseInfo.name = "no 22 skill delay time";
		sdt.baseInfo.description = "delay drag time";
		sdt.type = EValueType.FIXED;
		sdt.value = 1f;
		TSkillDelayTime tst = new TSkillDelayTime (sdt);
		DataCenter.Instance.Skill.Add (sdt.baseInfo.id, tst);
		
		sdt = new SkillDelayTime ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 23;
		sdt.baseInfo.name = "no 23 skill delay time";
		sdt.baseInfo.description = "delay drag time";
		sdt.type = EValueType.FIXED;
		sdt.value = 2f;
		tst = new TSkillDelayTime (sdt);
		DataCenter.Instance.Skill.Add (sdt.baseInfo.id, tst);
		
		SkillConvertUnitType scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 24;
		scut.baseInfo.name = "no 24 skill convert unit type";
		scut.baseInfo.description = "convert card color";
		scut.type = EValueType.COLORTYPE;
		scut.unitType1 = EUnitType.UFIRE;
		scut.unitType2 = EUnitType.UWATER;
		TSkillConvertUnitType tcut = new TSkillConvertUnitType (scut);
		DataCenter.Instance.Skill.Add (scut.baseInfo.id, tcut);

	 	scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 31;
		scut.baseInfo.name = "no 31 skill convert unit type";
		scut.baseInfo.description = "convert card color";
		scut.type = EValueType.COLORTYPE;
		scut.unitType1 = EUnitType.UWATER;
		scut.unitType2 = EUnitType.UWIND;
		tcut = new TSkillConvertUnitType (scut);
		DataCenter.Instance.Skill.Add (scut.baseInfo.id, tcut);
		
		SkillExtraAttack sea = new SkillExtraAttack ();
		sea.baseInfo = new SkillBase ();
		sea.baseInfo.id = 25;
		sea.baseInfo.name = "no 25 skill extra attack";
		sea.baseInfo.description = "extra all type attack";
		sea.unitType = EUnitType.UWIND;
		sea.attackValue = 2f;
		TSkillExtraAttack tsea = new TSkillExtraAttack (sea);
		DataCenter.Instance.Skill.Add (sea.baseInfo.id, tsea);
	}

	void ConfigActiveSkill () {
		SkillSingleAttack ssa = new SkillSingleAttack ();
		ssa.baseInfo = new SkillBase ();
		ssa.baseInfo.id = 32;
		ssa.baseInfo.name = "no 32 SkillSingleAttack";
		ssa.baseInfo.description = "single attack, all attack, recover hp";
		ssa.baseInfo.skillCooling = 3;
		ssa.type = EValueType.MULTIPLE;
		ssa.unitType = EUnitType.UFIRE;
		ssa.value = 4f;
		ssa.attackRange = EAttackType.ATK_ALL;
		ssa.ignoreDefense = false;
		TSkillSingleAttack tssa = new TSkillSingleAttack (ssa);
		DataCenter.Instance.Skill.Add (ssa.baseInfo.id, tssa);

		SkillAttackRecoverHP ssarh = new SkillAttackRecoverHP ();
		ssarh.baseInfo = new SkillBase ();
		ssarh.baseInfo.id = 33;
		ssarh.baseInfo.name = "no 33 SkillSingleAtkRecoverHP";
		ssarh.baseInfo.description = "attack enemy and recover hp, the recover value equals attack value";
		ssarh.baseInfo.skillCooling = 0;
		ssarh.type = EValueType.MULTIPLE;
		ssarh.value = 5f;
		ssarh.unitType = EUnitType.UFIRE;
		TSkillAttackRecoverHP arh = new TSkillAttackRecoverHP (ssarh);
		DataCenter.Instance.Skill.Add (ssarh.baseInfo.id, arh);

		SkillSuicideAttack sSucideAttack = new SkillSuicideAttack ();
		sSucideAttack.baseInfo = new SkillBase ();
		sSucideAttack.baseInfo.id = 34;
		sSucideAttack.baseInfo.name = "no 34 SucideAttack";
		sSucideAttack.baseInfo.description = "boost a attack and self blood will be one";
		sSucideAttack.baseInfo.skillCooling = 0;
		sSucideAttack.type = EValueType.MULTIPLE;
		sSucideAttack.value = 100f;
		sSucideAttack.unitType = EUnitType.UFIRE;
		sSucideAttack.attackType = EAttackType.ATK_ALL;
		TSkillSuicideAttack sa = new TSkillSuicideAttack (sSucideAttack);
		DataCenter.Instance.Skill.Add (sSucideAttack.baseInfo.id, sa);

		ssa = new SkillSingleAttack ();
		ssa.baseInfo = new SkillBase ();
		ssa.baseInfo.id = 35;
		ssa.baseInfo.name = "no 35 knock down";
		ssa.baseInfo.description = "knock down enemy";
		ssa.baseInfo.skillCooling = 0;
		ssa.value = 0.1f;
		ssa.attackRange = EAttackType.ATK_SINGLE;
		ssa.ignoreDefense = true;
		KnockdownAttack kka = new KnockdownAttack (ssa);
		DataCenter.Instance.Skill.Add (ssa.baseInfo.id, kka);

		ConfigActiveSkill1 ();
	}

	void ConfigActiveSkill1 () {
		SkillKillHP skh = new SkillKillHP ();
		skh.baseInfo = new SkillBase ();
		skh.baseInfo.id = 38;
		skh.baseInfo.name = "no 36 gravity";
		skh.baseInfo.description = "gravity kill enemy hp";
		skh.baseInfo.skillCooling = 0;
		skh.type = EValueType.PERCENT;
		skh.value = 0.3f;
		GravityAttack ga = new GravityAttack (skh);
		DataCenter.Instance.Skill.Add (skh.baseInfo.id, ga);

		SkillRecoverSP srs = new SkillRecoverSP ();
		srs.baseInfo = new SkillBase ();
		srs.baseInfo.id = 37;
		srs.baseInfo.name = "no 37 recover sp";
		srs.baseInfo.description = "recover sp";
		srs.baseInfo.skillCooling = 0;
		srs.type = EValueType.FIXED;
		srs.value = 4f;
		TSkillRecoverSP rs = new TSkillRecoverSP (srs);
		DataCenter.Instance.Skill.Add (srs.baseInfo.id, rs);

		SkillPoison sp = new SkillPoison ();
		sp.baseInfo = new SkillBase ();
		sp.baseInfo.id =36 ;
		sp.baseInfo.name = "no 38 skill poison";
		sp.baseInfo.description = "poison attack";
		sp.baseInfo.skillCooling = 0;
		sp.type = EValueType.ROUND;
		sp.value = 5f;
		sp.roundValue = 3;
		TSkillPoison spa = new TSkillPoison (sp);
		DataCenter.Instance.Skill.Add (sp.baseInfo.id, spa);


	}

	void ConfigActiveSkill2 () {
		SkillConvertUnitType scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 39;
		scut.baseInfo.name = "no 39 convert card color";
		scut.baseInfo.description = "random change card color";
		scut.baseInfo.skillCooling = 0;
		scut.type = EValueType.RANDOMCOLOR;
		scut.unitType1 = EUnitType.UALL;
		scut.unitType2 = EUnitType.UALL;
		ActiveChangeCardColor acc = new ActiveChangeCardColor (scut);
		DataCenter.Instance.Skill.Add (scut.baseInfo.id, acc);

		scut = new SkillConvertUnitType ();
		scut.baseInfo = new SkillBase ();
		scut.baseInfo.id = 40;
		scut.baseInfo.name = "no 40 skill convert unit type";
		scut.baseInfo.description = "convert card color";
		scut.baseInfo.skillCooling = 0;
		scut.type = EValueType.COLORTYPE;
		scut.unitType1 = EUnitType.UWATER;
		scut.unitType2 = EUnitType.UWIND;
		acc = new ActiveChangeCardColor (scut);
		DataCenter.Instance.Skill.Add (scut.baseInfo.id, acc);

		SkillReduceDefence srd = new SkillReduceDefence ();
		srd.baseInfo = new SkillBase ();
		srd.baseInfo.id = 41;
		srd.baseInfo.name = "no 41. Reduce Defense";
		srd.baseInfo.description = "reduce enemy defense by percent";
		srd.baseInfo.skillCooling = 0;
		srd.type = EValueType.PERCENT;
		srd.period = 1;
		srd.value = 1f;
		ActiveReduceDefense ard = new ActiveReduceDefense (srd);
		DataCenter.Instance.Skill.Add (srd.baseInfo.id, ard);

		SkillDelayTime sdt = new SkillDelayTime ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 42;
		sdt.baseInfo.name = "no 42. Delay Time";
		sdt.baseInfo.description = "delay drag card time";
		sdt.baseInfo.skillCooling = 0;
		sdt.type = EValueType.FIXED;
		sdt.value = 2f;
		ActiveDelayTime adt = new ActiveDelayTime (sdt);
		DataCenter.Instance.Skill.Add (sdt.baseInfo.id, adt);

		ConfigActiveSkill3 ();
	}

	void ConfigActiveSkill3 () {
		SkillReduceHurt srh = new SkillReduceHurt ();
		srh.baseInfo = new SkillBase ();
		srh.baseInfo.id = 43;
		srh.baseInfo.name = "no 43. Reduce Hurt";
		srh.baseInfo.description = "reduce enemy hurt value";
		srh.baseInfo.skillCooling = 0;
		srh.type = EValueType.MULTIPLE;
		srh.period = EPeriod.EP_EVERY_ROUND;
		srh.periodValue = 3;
		srh.value = 1f;
		ActiveReduceHurt arh = new ActiveReduceHurt (srh);
		DataCenter.Instance.Skill.Add (srh.baseInfo.id, arh);

		SkillDeferAttackRound sdar = new SkillDeferAttackRound ();
		sdar.baseInfo = new SkillBase ();
		sdar.baseInfo.id = 44;
		sdar.baseInfo.name = "no 44. defer Attack Round";
		sdar.baseInfo.description = "defer enemy attack round";
		sdar.baseInfo.skillCooling = 0;
		sdar.type = EValueType.ROUND;
		sdar.value = 2f;
		ActiveDeferAttackRound adar = new ActiveDeferAttackRound (sdar);
		DataCenter.Instance.Skill.Add (sdar.baseInfo.id, adar);
		ConfigActiveSkill4 ();
	}

	void ConfigActiveSkill4 () {
		SkillTargetTypeAttack stta = new SkillTargetTypeAttack ();
		stta.baseInfo = new SkillBase ();
		stta.baseInfo.id = 45;
		stta.baseInfo.name = "no 45. Attack Target Type";
		stta.baseInfo.description = "Attack target type enemey";
		stta.baseInfo.skillCooling = 0;
		stta.type = EValueType.MULTIPLE;
		stta.value = 2f;
		stta.targetUnitType = EUnitType.UFIRE;
		stta.hurtUnitType = EUnitType.UWATER;
		ActiveAttackTargetType aatt = new ActiveAttackTargetType (stta);
		DataCenter.Instance.Skill.Add (stta.baseInfo.id, aatt);

		SkillStrengthenAttack ssa = new SkillStrengthenAttack ();
		ssa.baseInfo = new SkillBase ();
		ssa.baseInfo.id = 46;
		ssa.baseInfo.name = "no 46.Skill StrengthenAttack";
		ssa.baseInfo.description = "strengthen target unit type role attack";
		ssa.baseInfo.skillCooling = 0;
		ssa.periodValue = 3;
		ssa.targetType = EUnitType.UFIRE;
		ssa.type = EValueType.MULTIPLE;
		ssa.value = 2f;
		ActiveStrengthenAttack asa = new ActiveStrengthenAttack (ssa);
		DataCenter.Instance.Skill.Add (ssa.baseInfo.id, asa);

		SkillMultipleAttack sma = new SkillMultipleAttack ();
		sma.baseInfo = new SkillBase ();
		sma.baseInfo.id = 47;
		sma.baseInfo.name = "no 47. MultipleAttack";
		sma.baseInfo.description = "three color can boost multiple attack value";
		sma.unitTypeCount = 2;
		LeaderSkillMultipleAttack lsma = new LeaderSkillMultipleAttack (sma);
		DataCenter.Instance.Skill.Add (sma.baseInfo.id, lsma);

		ConfigPassiveSkil ();
	}

	void ConfigPassiveSkil () {
		SkillDodgeTrap sdt = new SkillDodgeTrap ();
		sdt.baseInfo = new SkillBase ();
		sdt.baseInfo.id = 48;
		sdt.baseInfo.name = "no 48. Dodge trap";
		sdt.baseInfo.description = "avoid trap";
		sdt.trapLevel = 1;
		sdt.trapType = ETrapType.Injured;
		PassiveDodgeTrap pdt = new PassiveDodgeTrap (sdt);
		DataCenter.Instance.Skill.Add (sdt.baseInfo.id, pdt);

		SkillAntiAttack saa = new SkillAntiAttack ();
		saa.baseInfo = new SkillBase ();
		saa.baseInfo.id = 49;
		saa.baseInfo.name = "no 49. Anti Attak";
		saa.baseInfo.description = "anti enemy attack";
		saa.attackSource = EUnitType.UALL;
		saa.antiAttack = EUnitType.UFIRE;
		saa.antiAtkRatio = 1f;
		saa.probability = 1f;
TSkillAntiAttack paa = new TSkillAntiAttack (saa);
		DataCenter.Instance.Skill.Add (saa.baseInfo.id, paa);
	}
}