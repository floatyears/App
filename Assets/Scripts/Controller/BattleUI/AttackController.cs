using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AttackController {
	private MsgCenter msgCenter;
	private BattleUseData bud;
	public AttackController (BattleUseData bud) {
		msgCenter = MsgCenter.Instance;
		this.bud = bud;
	}

	private List<AttackInfo> attackInfo = new List<AttackInfo>();
	private List<TempEnemy> enemyInfo = new List<TempEnemy>();
	private UnitPartyInfo upi;

	private float countDownTime = 0f;

	public void StartAttack (List<AttackInfo> attackInfo,List<TempEnemy> enemyInfo,UnitPartyInfo upi) {
		this.attackInfo = attackInfo;
		this.enemyInfo = enemyInfo;
		this.upi = upi;
		Attack ();
	}
	
	void StopAttack () {

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
			GameTimer.GetInstance ().AddCountDown (countDownTime, AttackEnemy);
		}
	}
	
	void AttackEnemy () {
		if (attackInfo.Count == 0) {
			GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
			return;
		}
		AttackInfo ai = attackInfo [0];
		attackInfo.RemoveAt (0);
		switch (ai.AttackRange) {
		case 0:
			DisposeAttackSingle(ai);
			break;
		case 1:
			DisposeAttackAll(ai);
			break;
		}
		//CheckTempEnemy ();

		Attack ();
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
		ai.InjuryValue = te.CalculateInjured (ai.AttackValue, restraint, ai.AttackType);
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
			ai.InjuryValue = te.CalculateInjured(ai.AttackValue,b,ai.AttackType);
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
		// te = enemyInfo [enemyIndex];
		//msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
		if (te.GetRound () == 0) {
			msgCenter.Invoke (CommandEnum.EnemyAttack, te.GetID());
			int attackType = te.GetUnitType ();
			int attackValue = te.GetAttack ();
			upi.CaculateInjured (attackType, attackValue);
			bud.RefreshBlood ();
			te.ResetAttakAround ();	

			msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
		}

		enemyIndex ++;

		if (enemyIndex == enemyInfo.Count) {
			bud.ClearData();
			msgCenter.Invoke (CommandEnum.EnemyAttackEnd, null);
		} 
		else {
			LoopEnemyAttack();
		}
	}     
}
