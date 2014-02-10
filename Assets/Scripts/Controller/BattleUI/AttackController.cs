﻿using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AttackController {
	private MsgCenter msgCenter;
	private BattleUseData bud;
	private List<AttackInfo> attackInfo = new List<AttackInfo>();
	public List<TempEnemy> enemyInfo = new List<TempEnemy>();
	private UnitPartyInfo upi;
	private float countDownTime = 0f;

	public AttackController (BattleUseData bud) {
		msgCenter = MsgCenter.Instance;
		this.bud = bud;

		RegisterEvent ();
	}

	~AttackController() {
		RemoveEvent ();
	}

	void RegisterEvent () {
		msgCenter.AddListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		msgCenter.AddListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		msgCenter.AddListener (CommandEnum.SkillGravity, Gravity);
		msgCenter.AddListener (CommandEnum.ReduceDefense, ReduceDefense);
		msgCenter.AddListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
	}

	void RemoveEvent () {
		msgCenter.RemoveListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		msgCenter.RemoveListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		msgCenter.RemoveListener (CommandEnum.SkillGravity, Gravity);
		msgCenter.RemoveListener (CommandEnum.ReduceDefense, ReduceDefense);
		msgCenter.RemoveListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
	}

	void DrawHP(object data) {
		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, tempPreHurtValue);
	}

	void Gravity(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;
		}

		for (int i = 0; i < enemyInfo.Count; i++) {
			int initBlood = enemyInfo[i].GetInitBlood();
			int hurtValue = System.Convert.ToInt32(initBlood * ai.AttackValue);
			enemyInfo[i].KillHP(hurtValue);
		}
		CheckTempEnemy ();
	}

	TClass<int,int,float> reduceInfo = null;
	void ReduceDefense(object data) {
		reduceInfo = data as TClass<int,int,float>;
		if (reduceInfo == null) {
			return;		
		}

		if (reduceInfo.arg2 == 0) {
			reduceInfo = null;
			ReduceEnemy(0f);
			return;
		}

		ReduceEnemy (reduceInfo.arg3);
	}

	void ReduceEnemy(float value) {
//		Debug.LogError ("ReduceEnemy value : " + value);
		for (int i = 0; i < enemyInfo.Count; i++) {
			enemyInfo[i].ReduceDefense(value);
		}
	}

	void ActiveSkillAttack (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		BeginAttack (ai);
		CheckTempEnemy ();
	}

	void AttackTargetTypeEnemy (object data) {
		AttackTargetType att = data as AttackTargetType;
		if (att == null) {
			return;	
		}

		for (int i = 0; i < enemyInfo.Count; i++) {
			TempEnemy te = enemyInfo[i];
			if(te.GetUnitType() == att.targetType) {
				AttackInfo ai = att.attackInfo;
				int restraintType = DGTools.RestraintType(ai.AttackType);
				bool b = restraintType == te.GetUnitType();
				int hurtValue = te.CalculateInjured(ai, b);
				ai.InjuryValue = hurtValue;
				tempPreHurtValue = hurtValue;
				ai.EnemyID = te.GetID();
				AttackEnemyEnd (ai);
			}
		}

		CheckTempEnemy ();
	}

	public void StartAttack (List<AttackInfo> attack, UnitPartyInfo upi) {
		attack.AddRange (leaderSkilllExtarAttack.ExtraAttack ());
		attackInfo = attack;
		this.upi = upi;
		Attack ();
	}
	
	void StopAttack () {

	}

	ILeadSkillReduceHurt leadSkillReuduce;
	ILeaderSkillExtraAttack leaderSkilllExtarAttack;
	ILeaderSkillRecoverHP leaderSkillRecoverHP;
	ILeaderSkillMultipleAttack leaderSkillMultiple;

	public void LeadSkillReduceHurt(ExcuteLeadSkill lsr) {
		leadSkillReuduce = lsr as ILeadSkillReduceHurt;
		leaderSkilllExtarAttack = lsr as ILeaderSkillExtraAttack;
		leaderSkillRecoverHP = lsr as ILeaderSkillRecoverHP;
		leaderSkillMultiple = lsr as ILeaderSkillMultipleAttack;
	}

	bool CheckEnemy () {
		if (enemyInfo == null || enemyInfo.Count == 0) {
			return false;
		}
		else {
			return true;
		}
	}

	bool CheckAttackInfo () {
		if (attackInfo == null || attackInfo.Count == 0) {
			return false;		
		}
		else {
			return true;		
		}
	}

	float GetIntervTime () {
		if (enemyInfo == null || enemyInfo.Count == 0) {
			return 0.5f;		
		}
		else {
			return 1.0f;		
		}
	}

	float GetEnemyTime () {
		return 0.5f;
	}

	void Attack () {
		countDownTime = GetIntervTime ();

		if (CheckTempEnemy () ) {
			enemyIndex = 0;
			GameTimer.GetInstance ().AddCountDown (countDownTime, AttackEnemy);
		}
	}

	void MultipleAttack () {
		float multipe = leaderSkillMultiple.MultipleAttack (attackInfo);
		if (multipe > 1.0f) {
			for (int i = 0; i < attackInfo.Count; i++) {
//				Debug.LogError("befoure multiple : " + attackInfo[i].AttackValue + "multipe : " +multipe);
				attackInfo[i].AttackValue *= multipe;
//				Debug.LogError("behind multiple : " + attackInfo[i].AttackValue);
			}	
		}
	}

	void AttackEnemy () {
		if (attackInfo.Count == 0) {
			int blood = leaderSkillRecoverHP.RecoverHP(bud.Blood, 1);	//1: every round.
			bud.RecoverHP(blood);
			msgCenter.Invoke(CommandEnum.AttackEnemyEnd, null);
			GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
			return;
		}
		msgCenter.Invoke (CommandEnum.ActiveSkillCooling, null);
		MultipleAttack ();
		AttackInfo ai = attackInfo [0];
		attackInfo.RemoveAt (0);
		BeginAttack (ai);
		Attack ();
	}

	int tempPreHurtValue = 0;
	void BeginAttack(AttackInfo ai) {
		switch (ai.AttackRange) {
		case 0:
			DisposeAttackSingle(ai);
			break;
		case 1:
			DisposeAttackAll(ai);
			break;
		case 2:
			DisposeRecoverHP(ai);
			break;
		}
	}

	bool CheckTempEnemy() {
		for (int i = enemyInfo.Count - 1; i > -1; i--) {
			int blood = enemyInfo[i].GetBlood();
			if(blood <= 0){
				TempEnemy te = enemyInfo[i];
				enemyInfo.RemoveAt(i);
				MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, te);
			}
		}
		if (enemyInfo.Count == 0) {
			msgCenter.Invoke(CommandEnum.BattleEnd, null);
			bud.ClearData();
			return false;
		}
		return true;
	}

	void DisposeRecoverHP (AttackInfo value) {
		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, value);
	}

	void DisposeAttackSingle (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;	
		}
		List<TempEnemy> injuredEnemy = enemyInfo.FindAll (a => a.IsInjured () == true);
		int restraintType = DGTools.RestraintType (ai.AttackType);
		TempEnemy te = null;
		if (injuredEnemy.Count > 0) {
			DGTools.InsertSort(injuredEnemy,new TempEnemySortByHP(),false);
			int index = injuredEnemy.FindIndex (a => a.GetUnitType () == restraintType);
			te = index > - 1 ?  injuredEnemy [index] : injuredEnemy [0];
		} 
		else {
			int index = enemyInfo.FindIndex (a => a.GetUnitType () == restraintType);
			te = index > - 1 ?  enemyInfo [index] : enemyInfo [0];
		}
		bool restraint = restraintType == te.GetUnitType ();
		int hurtValue = te.CalculateInjured (ai, restraint);
		ai.InjuryValue = hurtValue;
		tempPreHurtValue = hurtValue;
		ai.EnemyID = te.GetID();
		AttackEnemyEnd (ai);
	}

	void DisposeAttackAll (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;		
		}
		int restraintType = DGTools.RestraintType (ai.AttackType);
		tempPreHurtValue = 0;
		for (int i = 0; i < enemyInfo.Count; i++) {
			TempEnemy te = enemyInfo[i];
			bool b = restraintType == te.GetUnitType();
			int hurtValue = te.CalculateInjured (ai, b);
			ai.InjuryValue = hurtValue;
			tempPreHurtValue += hurtValue;
			ai.EnemyID = te.GetID();
			AttackEnemyEnd (ai);
		}
	}

	void AttackEnemyEnd (AttackInfo ai){
		msgCenter.Invoke (CommandEnum.AttackEnemy, ai);
	}

	void AttackPlayer () {
		if (CheckTempEnemy ()) {
			LoopEnemyAttack ();	
		}
	}
	int enemyIndex = 0;
	TempEnemy te;
	void LoopEnemyAttack () {
		countDownTime = 0.3f;
		te = enemyInfo [enemyIndex];
		te.Next ();
		msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
		GameTimer.GetInstance ().AddCountDown (countDownTime, EnemyAttack);
	}
	
	void EnemyAttack () {
		if (te.GetRound () == 0) {
			msgCenter.Invoke (CommandEnum.EnemyAttack, te.GetID());
			int attackType = te.GetUnitType ();
			int attackValue = te.GetAttack ();
			float reduceValue = leadSkillReuduce.ReduceHurtValue(attackValue,attackType);
			int hurtValue = upi.CaculateInjured (attackType, reduceValue);
			bud.Hurt(hurtValue);
			te.ResetAttakAround ();	
			msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
		}
		enemyIndex ++;
		if (enemyIndex == enemyInfo.Count) {
			EnemyAttackEnd();
		} 
		else {
			LoopEnemyAttack();
		}
	}    

	void EnemyAttackEnd () {
		bud.ClearData();
		msgCenter.Invoke (CommandEnum.EnemyAttackEnd, null);
	}
}