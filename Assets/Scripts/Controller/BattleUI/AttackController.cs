using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AttackController {
	private MsgCenter msgCenter;
	public AttackController () {
		msgCenter = MsgCenter.Instance;
	}

	private List<AttackInfo> attackInfo = new List<AttackInfo>();
	private List<TempEnemy> enemyInfo = new List<TempEnemy>();
	private float countDownTime = 0f;
	void StartAttack (List<AttackInfo> attackInfo,List<TempEnemy> enemyInfo) {
		this.attackInfo = attackInfo;
		this.enemyInfo = enemyInfo;


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

	void Attack () {
		countDownTime = GetIntervTime ();

		for (int i = 0; i < enemyInfo.Count; i++) {
			if(enemyInfo[i].GetBlood() <= 0) {
				enemyInfo.RemoveAt(i);
			}
		}

		GameTimer.GetInstance ().AddCountDown (countDownTime, AttackEnemy);
	}
	
	void AttackEnemy () {
		if (attackInfo.Count == 0) {
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
		Attack ();
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
		for (int i = 0; i < enemyInfo.Count; i++) {
			if(enemyInfo[i].GetBlood() <= 0) {
				enemyInfo.RemoveAt(i);
			}
		}

		if (enemyInfo.Count == 0) {
			msgCenter.Invoke(CommandEnum.BattleEnd, null);
			return;
		}
	}

}
