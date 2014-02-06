using UnityEngine;
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
	}

	void RemoveEvent () {
		msgCenter.RemoveListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
	}

	void ActiveSkillAttack (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		BeginAttack (ai);
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

	public void LeadSkillReduceHurt(ExcuteLeadSkill lsr) {
		leadSkillReuduce = lsr as ILeadSkillReduceHurt;
		leaderSkilllExtarAttack = lsr as ILeaderSkillExtraAttack;
		leaderSkillRecoverHP = lsr as ILeaderSkillRecoverHP;
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
	
	void AttackEnemy () {
		if (attackInfo.Count == 0) {
			int blood = leaderSkillRecoverHP.RecoverHP(bud.Blood, 1);	//1: every round.
			bud.RecoverHP(blood);
			GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
			return;
		}
		AttackInfo ai = attackInfo [0];
		attackInfo.RemoveAt (0);
		BeginAttack (ai);
		Attack ();
	}

	void BeginAttack(AttackInfo ai) {
		Debug.LogError ("BeginAttack : " + ai.AttackRange);
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
		for (int i = 0; i < enemyInfo.Count; i++) {
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

	void DisposeRecoverHP (AttackInfo ai) {
		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, ai);
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
		ai.InjuryValue = te.CalculateInjured (ai, restraint);
		ai.EnemyID = te.GetID();
		AttackEnemyEnd (ai);
	}

	void DisposeAttackAll (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;		
		}
		int restraintType = DGTools.RestraintType (ai.AttackType);
		for (int i = 0; i < enemyInfo.Count; i++) {
			TempEnemy te = enemyInfo[i];
			bool b = restraintType == te.GetUnitType();
			ai.InjuryValue = te.CalculateInjured(ai,b);
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
