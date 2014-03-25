using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AttackController {
	private MsgCenter msgCenter;
	private BattleUseData bud;
	private List<AttackInfo> attackInfo = new List<AttackInfo>();

	public List<TEnemyInfo> enemyInfo = new List<TEnemyInfo>();
	private TUnitParty upi;
	private float countDownTime = 0f;
	private TQuestGrid grid;
	public TQuestGrid Grid {
		get{return grid;}
		set{grid = value; enemyInfo = value.Enemy;}
	}
	IExcutePassiveSkill passiveSkill;

	public AttackController (BattleUseData bud,IExcutePassiveSkill ips) {
		msgCenter = MsgCenter.Instance;
		this.bud = bud;
		passiveSkill = ips;
		RegisterEvent ();
	}

	public void RemoveListener () {
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
			TEnemyInfo te = enemyInfo[i];
			if(te.GetUnitType() == att.targetType) {
				AttackInfo ai = att.attackInfo;
				int restraintType = DGTools.RestraintType(ai.AttackType);
				bool b = restraintType == te.GetUnitType();
				int hurtValue = te.CalculateInjured(ai, b);
				ai.InjuryValue = hurtValue;
				tempPreHurtValue = hurtValue;
//				ai.EnemyID = te.EnemyID;//te.GetID();
				ai.EnemyID = te.EnemySymbol;
				AttackEnemyEnd (ai);
			}
		}
		CheckTempEnemy ();
	}

	public void StartAttack (List<AttackInfo> attack, TUnitParty upi) {
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
			return 0.8f;		
		}
	}

	float GetEnemyTime () {
		return 0.5f;
	}

	void Attack () {
		countDownTime = GetIntervTime ();
		enemyIndex = 0;
		MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [2]);

		GameTimer.GetInstance ().AddCountDown (countDownTime, AttackEnemy);

	}
	
	void MultipleAttack () {
		float multipe = leaderSkillMultiple.MultipleAttack (attackInfo);
		if (multipe > 1.0f) {
			for (int i = 0; i < attackInfo.Count; i++) {
				attackInfo[i].AttackValue *= multipe;
			}	
		}
	}

	void AttackEnemy () {
		if (attackInfo.Count == 0) {
			int blood = leaderSkillRecoverHP.RecoverHP(bud.Blood, 1);	//1: every round.
			bud.RecoverHP(blood);
			msgCenter.Invoke(CommandEnum.AttackEnemyEnd, null);
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [1]);
			GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
			return;
		}
		CheckEnemyDead();
		msgCenter.Invoke (CommandEnum.ActiveSkillCooling, null);
		MultipleAttack ();
		AttackInfo ai = attackInfo [0];
		attackInfo.RemoveAt (0);
		BeginAttack (ai);
		Attack ();
	}

	int tempPreHurtValue = 0;
	void BeginAttack(AttackInfo ai) {
//		Debug.LogError ("BeginAttack : " + ai.AttackRange + " ```` " + Time.realtimeSinceStartup);
		switch (ai.AttackRange) {
		case 0:
			DisposeAttackSingle(ai);
			break;
		case 1:
//			DisposeAttackSingle(ai);
			DisposeAttackAll(ai);
			break;
		case 2:
			DisposeRecoverHP(ai);
			break;
		}
	}

	List<TEnemyInfo> deadEnemy = new List<TEnemyInfo>();
	void CheckEnemyDead () {
		if (enemyInfo.Count == 1) {
			return;	
		}
		for (int i = enemyInfo.Count - 2; i > -1; i--) {
			TEnemyInfo te = enemyInfo[i];
			if(te.GetBlood() <= 0) {
				deadEnemy.Add(te);
				enemyInfo.Remove(te);
			}
		}
	}

	bool CheckTempEnemy() {
		for (int i = 0; i < deadEnemy.Count; i++) {
			deadEnemy[i].IsDead = true;
			MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, deadEnemy[i]);
			MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
		}
		deadEnemy.Clear ();
		for (int i = enemyInfo.Count - 1; i > -1; i--) {
			int blood = enemyInfo[i].GetBlood();
			if(blood <= 0){
				TEnemyInfo te = enemyInfo[i];
				enemyInfo.Remove(te);
				te.IsDead = true;
				MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, te);
				if(grid != null) {
					MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
				}
			}
		}
		if (enemyInfo.Count == 0) {
			GameTimer.GetInstance().AddCountDown(2f, BattleEnd);
			return false;
		}
		return true;
	}

	void BattleEnd() {
		msgCenter.Invoke (CommandEnum.GridEnd, null);
		msgCenter.Invoke(CommandEnum.BattleEnd, null);
		bud.ClearData();
	}

	void DisposeRecoverHP (AttackInfo value) {
		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, value);
	}

	void DisposeAttackSingle (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;	
		}

		List<TEnemyInfo> injuredEnemy = enemyInfo.FindAll (a => a.IsInjured () == true);
		int restraintType = DGTools.RestraintType (ai.AttackType);
		TEnemyInfo te = null;
		if (injuredEnemy.Count > 0) {
			DGTools.InsertSort(injuredEnemy,new EnemySortByHP(),false);
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
		ai.EnemyID = te.EnemySymbol;
		AttackEnemyEnd (ai);
	}

	void DisposeAttackAll (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;		
		}
		int restraintType = DGTools.RestraintType (ai.AttackType);
		tempPreHurtValue = 0;
		for (int i = 0; i < enemyInfo.Count; i++) {
			TEnemyInfo te = enemyInfo[i];
			bool b = restraintType == te.GetUnitType();
			int hurtValue = te.CalculateInjured (ai, b);
			ai.InjuryValue = hurtValue;
			tempPreHurtValue += hurtValue;
//			ai.EnemyID = te.EnemyID;//GetID();
			ai.EnemyID = te.EnemySymbol;

			AttackEnemyEnd (ai);
		}
	}

	void AttackEnemyEnd (AttackInfo ai){
//		Debug.LogError ("AttackController AttackEnemyEnd : " + ai.EnemyID);
		msgCenter.Invoke (CommandEnum.AttackEnemy, ai);
	}

	void AttackPlayer () {
		if (CheckTempEnemy ()) {
			LoopEnemyAttack ();	
		}
	}
	int enemyIndex = 0;
	TEnemyInfo te;
	void LoopEnemyAttack () {
		countDownTime = 0.3f;
		te = enemyInfo [enemyIndex];
		te.Next ();
		msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
		GameTimer.GetInstance ().AddCountDown (countDownTime, EnemyAttack);
	}
	List<AttackInfo> antiInfo = new List<AttackInfo>();
	void EnemyAttack () {
		if (te.GetRound () == 0) {
//			msgCenter.Invoke (CommandEnum.EnemyAttack, te.EnemyID);//GetID());
			msgCenter.Invoke (CommandEnum.EnemyAttack, te.EnemySymbol);
			int attackType = te.GetUnitType ();
			int attackValue = te.AttackValue;
			float reduceValue = leadSkillReuduce.ReduceHurtValue(attackValue,attackType);
			int hurtValue = upi.CaculateInjured (attackType, reduceValue);
			bud.Hurt(hurtValue);
			te.ResetAttakAround ();	
			msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
			List<AttackInfo> temp = passiveSkill.Dispose(attackType,hurtValue);
			for (int i = 0; i < temp.Count; i++) {
//				temp[i].EnemyID = te.EnemyID;//GetID();
				temp[i].EnemyID = te.EnemySymbol;
				antiInfo.Add(temp[i]);
			}
		}
		enemyIndex ++;
		if (enemyIndex == enemyInfo.Count) {
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [3]);
			GameTimer.GetInstance ().AddCountDown (1f, LoopAntiAttack);
		}
		else {
			LoopEnemyAttack();
		}
	}    

	void EnemyAttackEnd () {
		CheckTempEnemy ();
		bud.ClearData();
		msgCenter.Invoke (CommandEnum.EnemyAttackEnd, null);
	}

	void LoopAntiAttack() {
		float intervTime = 0.4f;
		GameTimer.GetInstance ().AddCountDown (intervTime, AntiAttack);
	}

	void AntiAttack() {
		if (antiInfo.Count == 0) {
			EnemyAttackEnd();	
			return;
		}

		AttackInfo ai = antiInfo [0];
		antiInfo.RemoveAt (0);
		TEnemyInfo te = enemyInfo.Find(a=>a.EnemySymbol == ai.EnemyID);
		if (te == default(TEnemyInfo)) {
			return;	
		}
		int restraintType = DGTools.RestraintType (ai.AttackType);
		bool restaint = restraintType == te.GetUnitType ();
		int hurtValue = te.CalculateInjured (ai, restaint);
		ai.InjuryValue = hurtValue;
		AttackEnemyEnd (ai);
		//if (CheckEnemy ()) {
			LoopAntiAttack ();		
		//}
	}
}
