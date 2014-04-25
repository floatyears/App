using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AttackController {
	public const float normalAttackInterv = 0.7f;
	public const float deadAttackInterv = 0.5f;
	private MsgCenter msgCenter;
	private BattleUseData bud;
	private Queue<AttackInfo> attackInfoQueue = new Queue<AttackInfo> ();
	public List<TEnemyInfo> enemyInfo = new List<TEnemyInfo>();
	private TEnemyInfo targetEnemy;
	private TUnitParty upi;
	private float countDownTime = 0f;
	private TQuestGrid grid;
	public TQuestGrid Grid {
		get{return grid;}
		set{
			grid = value;
			enemyInfo = new List<TEnemyInfo>();
			foreach (var item in value.Enemy) {
//				item.instance.hp += 1000;
//				item.initBlood += 1000;

				enemyInfo.Add(item);
			}

			foreach (var item in enemyInfo) {
				item.drop = grid.Drop;
			}
		}
	}
	IExcutePassiveSkill passiveSkill;
	public bool battleFail = false;

	public bool isBoss = false;
	public AttackController (BattleUseData bud,IExcutePassiveSkill ips,TUnitParty tup) {
		upi = tup;
		msgCenter = MsgCenter.Instance;
		this.bud = bud;
		passiveSkill = ips;
		RegisterEvent ();
	}

	public void RemoveListener () {
		battleFail = false;
		RemoveEvent ();
	}

	void RegisterEvent () {
		msgCenter.AddListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		msgCenter.AddListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		msgCenter.AddListener (CommandEnum.SkillGravity, Gravity);
		msgCenter.AddListener (CommandEnum.ReduceDefense, ReduceDefense);
		msgCenter.AddListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
		msgCenter.AddListener (CommandEnum.TargetEnemy, TargetEnemy);
	}

	void RemoveEvent () {
		msgCenter.RemoveListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		msgCenter.RemoveListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		msgCenter.RemoveListener (CommandEnum.SkillGravity, Gravity);
		msgCenter.RemoveListener (CommandEnum.ReduceDefense, ReduceDefense);
		msgCenter.RemoveListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
		msgCenter.RemoveListener (CommandEnum.TargetEnemy, TargetEnemy);
	}

	void DrawHP(object data) {
		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillRecoverHP, (float)tempPreHurtValue);
	}

	void Gravity(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;
		}

		for (int i = 0; i < enemyInfo.Count; i++) {
			int initBlood = enemyInfo[i].GetInitBlood();
			int hurtValue = System.Convert.ToInt32(initBlood * ai.AttackValue); //eg: 15%*blood
			enemyInfo[i].KillHP(hurtValue);
		}
		CheckTempEnemy ();
	}

	TClass<string,int,float> reduceInfo = null;

	void ReduceDefense(object data) {
		reduceInfo = data as TClass<string,int,float>;
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

		tempPreHurtValue = 0;
		for (int i = 0; i < enemyInfo.Count; i++) {
			TEnemyInfo te = enemyInfo[i];
			if(te.GetUnitType() == att.targetType || att.targetType == (int)bbproto.EUnitType.UALL) {
				AttackInfo ai = att.attackInfo;
				int restraintType = DGTools.RestraintType(ai.AttackType);
				bool b = restraintType == te.GetUnitType();
				int hurtValue = te.CalculateInjured(ai, b);
				ai.InjuryValue = hurtValue;
				tempPreHurtValue += hurtValue;
				ai.EnemyID = te.EnemySymbol;
				AttackEnemyEnd (ai);
			}
		}
		CheckTempEnemy ();
	}

	public void StartAttack (List<AttackInfo> attack) {
		msgCenter.Invoke (CommandEnum.ShowHands, attack.Count);
		attack.AddRange (leaderSkilllExtarAttack.ExtraAttack ());
		MultipleAttack (attack);
//		attackInfo = attack;
		foreach (var item in attack) {
			attackInfoQueue.Enqueue(item);
		}
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
		if (attackInfoQueue == null || attackInfoQueue.Count == 0) {
			return false;		
		}
		else {
			return true;		
		}
	}

	float GetIntervTime () {
		if (enemyInfo.Count == 1 && enemyInfo[0].GetBlood() <= 0) {
			return 0.4f;		
		}
		else {
			return 0.6f;		
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
	
	void MultipleAttack (List<AttackInfo> attackInfo) {
		float multipe = leaderSkillMultiple.MultipleAttack (attackInfo);
		if (multipe > 1.0f) {
			for (int i = 0; i < attackInfo.Count; i++) {
				attackInfo[i].AttackValue *= multipe;
			}	
		}
	}

	void AttackEnemy () {
		if (attackInfoQueue.Count == 0) {
			int blood = leaderSkillRecoverHP.RecoverHP(bud.maxBlood, 1);	//1: every round.
			bud.Blood += blood;
			msgCenter.Invoke(CommandEnum.AttackEnemyEnd, null);
			if (!CheckTempEnemy ()) {
				return;
			}
			bud.battleQuest.battle.ShieldInput(false);
			GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
			return;
		}
		CheckEnemyDead();
		msgCenter.Invoke (CommandEnum.ActiveSkillCooling, null);
		AttackInfo ai = attackInfoQueue.Dequeue();
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
			TEnemyInfo tei = deadEnemy[i];
			tei.IsDead = true;
			if(tei.Equals(targetEnemy)) {
				targetEnemy = null;
			}
			MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, tei);
			if(grid != null) {
				MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
			}
		}
		deadEnemy.Clear ();

		for (int i = enemyInfo.Count - 1; i > -1; i--) {
			int blood = enemyInfo[i].GetBlood();
			if(blood <= 0){
				TEnemyInfo te = enemyInfo[i];
				enemyInfo.Remove(te);
				te.IsDead = true;
				if(te.Equals(targetEnemy)) {
					targetEnemy = null;
				}

				MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, te);
				if(grid != null) {
					MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
				}
			}
		}
		if (enemyInfo.Count == 0) {
			BattleBottom.notClick = false;
			GameTimer.GetInstance().AddCountDown(1f, BattleEnd); //TODO: set time in const config
			return false;
		}

		AudioManager.Instance.PlayAudio (AudioEnum.sound_enemy_die);
		return true;
	}

	public void BattleEnd() {
		msgCenter.Invoke (CommandEnum.GridEnd, null);
		msgCenter.Invoke(CommandEnum.BattleEnd, battleFail);
		bud.ClearData();
		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_dungeon);
	}

	void TargetEnemy(object data) {
		TEnemyInfo enemyInfo = data as TEnemyInfo;
		if(targetEnemy == null || !targetEnemy.Equals(enemyInfo)) {
			targetEnemy = enemyInfo;
		}else{
			targetEnemy = null;
		}
	}

	void DisposeRecoverHP (AttackInfo value) {
		MsgCenter.Instance.Invoke (CommandEnum.RecoverHP, value);
	}

	void DisposeAttackSingle (AttackInfo ai) {
		if (enemyInfo.Count == 0) {
			return;	
		}

		int restraintType = DGTools.RestraintType (ai.AttackType);
		prevAttackEnemyInfo = GetTargetEnemy (restraintType);
		bool restraint = restraintType == prevAttackEnemyInfo.GetUnitType ();
		int hurtValue = prevAttackEnemyInfo.CalculateInjured (ai, restraint);
		ai.InjuryValue = hurtValue;
		tempPreHurtValue = hurtValue;
		ai.EnemyID = prevAttackEnemyInfo.EnemySymbol;
		AttackEnemyEnd (ai);
	}

	TEnemyInfo prevAttackEnemyInfo;

	TEnemyInfo GetTargetEnemy(int restraintType) {
		TEnemyInfo te;
		if (targetEnemy != null && targetEnemy.GetBlood () != 0) {
			te = targetEnemy;
		} else {
			if(enemyInfo.Count == 1 && enemyInfo[0].GetBlood() <= 0) {
				if(prevAttackEnemyInfo == null) {
					prevAttackEnemyInfo = enemyInfo[0];
				}
				return prevAttackEnemyInfo;
			}

			List<TEnemyInfo> injuredEnemy = enemyInfo.FindAll (a => a.IsInjured () == true);
			if (injuredEnemy.Count > 0) {
				DGTools.InsertSort(injuredEnemy,new EnemySortByHP(),false);
				int index = injuredEnemy.FindIndex (a => a.GetUnitType () == restraintType);
				te = index > - 1 ?  injuredEnemy [index] : injuredEnemy [0];
			}
			else {
				int index = enemyInfo.FindIndex (a => a.GetUnitType () == restraintType);
				te = index > - 1 ?  enemyInfo [index] : enemyInfo [0];
			}
		}
		return te;
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
			ai.EnemyID = te.EnemySymbol;
			AttackEnemyEnd (ai);
		}
	}

	void AttackEnemyEnd (AttackInfo ai){
		msgCenter.Invoke (CommandEnum.AttackEnemy, ai);
	}

	public void FirstAttack () {
		foreach (var item in enemyInfo) {
			item.FirstAttack();
		}
	}

	public void AttackPlayer () {
		if (CheckTempEnemy ()) {
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [1]);
			for (int i = 0; i < enemyInfo.Count; i++) {
				enemyInfo[i].Next();
			}
			LoopEnemyAttack ();	
		} else {
			bud.battleQuest.battle.ShieldInput(true);		
		}
	}

	int enemyIndex = 0;
	TEnemyInfo te;
	void LoopEnemyAttack () {
		countDownTime = 0.4f;
		te = enemyInfo [enemyIndex];
//		te.Next ();
		GameTimer.GetInstance ().AddCountDown (countDownTime, EnemyAttack);
	}

	List<AttackInfo> antiInfo = new List<AttackInfo>();

	void EnemyAttack () {
		enemyIndex ++;
		if (te.GetRound () == 0) {
			msgCenter.Invoke (CommandEnum.EnemyAttack, te.EnemySymbol);
			int attackType = te.GetUnitType ();
			int attackValue = te.AttackValue;
			float reduceValue;
			if(leadSkillReuduce != null) {
				reduceValue = leadSkillReuduce.ReduceHurtValue(attackValue,attackType);
			}
			else{
				reduceValue = attackValue;
			}
			int hurtValue = upi.CaculateInjured (attackType, reduceValue);
			bud.Hurt(hurtValue);
			te.ResetAttakAround ();	
			msgCenter.Invoke (CommandEnum.EnemyRefresh, te);
			List<AttackInfo> temp = passiveSkill.Dispose(attackType, hurtValue);
			for (int i = 0; i < temp.Count; i++) {
				temp[i].EnemyID = te.EnemySymbol;
				antiInfo.Add(temp[i]);
			}

			if(!isBoss) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_boss_battle);
			}
		}

		if (enemyIndex == enemyInfo.Count) {

			if(bud.Blood > 0) {
				if (antiInfo.Count == 0) {
					GameTimer.GetInstance ().AddCountDown (0.5f, EnemyAttackEnd);
					return;
				}
				MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [3]); // stateInfo [3]="PassiveSkill"
				GameTimer.GetInstance ().AddCountDown (1f, LoopAntiAttack);
			}
			else{
				bud.battleQuest.battle.ShieldInput(true);	
				MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
			}
		}
		else {
			LoopEnemyAttack();
		}
	}    

	void Fail () {
		battleFail = true;
		BattleEnd();
		return;
	}

	void EnemyAttackEnd () {
		BattleBottom.notClick = false;
		CheckTempEnemy ();
		bud.ClearData();
		bud.battleQuest.battle.ShieldInput(true);	
		msgCenter.Invoke (CommandEnum.EnemyAttackEnd, null);
	}

	void LoopAntiAttack() {
		float intervTime = 0.8f;
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
		LoopAntiAttack ();		
	}
}
