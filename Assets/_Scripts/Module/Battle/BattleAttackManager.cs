using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class BattleAttackManager {
//	public BattleMapModule battleQuest;

	public static string[] stateInfo = new string[] {"Player Phase","Enemy Phase","Normal Skill","Passive Skill","Active Skill"};
    private ErrorMsg errorMsg;
    public UnitParty upi;
    public int maxBlood = 0;
    private int blood = 0;
    public int Blood {
		set {
//			Debug.LogError("Blood : " + value);
			if(value == 0) {
				blood = value;
//				PlayerDead();
			} else if(value < 0) {
				if(blood == 0) 
					return;
				blood = 0;
				ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood);
			} else if(value > maxBlood) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
				if(blood < maxBlood) {
					blood = maxBlood;
					ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood);
				}

			} else {
				if(value > blood) {
					AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
					ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood);
				}
				blood = value;
			}
			BattleConfigData.Instance.storeBattleData.hp = blood;
		}
        get { return blood; }
    }
    private int recoverHP = 0;
    public static int maxEnergyPoint = 0;
    private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();
    private List<EnemyInfo> currentEnemy = new List<EnemyInfo>();
    private List<EnemyInfo> showEnemy = new List<EnemyInfo>();
    private static Coordinate currentCoor;
    public static Coordinate CurrentCoor {
        get { return currentCoor; }
    }

    private static float countDown = 5f;
    public static float CountDown {
        get { return countDown; }
    }

	private static BattleAttackManager instance;

	public static BattleAttackManager Instance{
		get{
			if(instance == null)
				instance = new BattleAttackManager();
			return instance;
		}
	}
	float enemyAttackTime = 0.5f;

    private BattleAttackManager() {
//		battleQuest = bq;
//		configBattleUseData = ConfigBattleUseData.Instance;
//		ListenEvent();
		errorMsg = new ErrorMsg();
		upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		upi.GetSkillCollection();
		GetBaseData ();

//		els = new ExcuteLeadSkill(upi);

//		els = upi;

//		skillRecoverHP = els;
		//		configBattleUseData = ConfigBattleUseData.Instance;
		SetEffectTime (2f);

//		Excute();

		BattleConfigData.Instance.storeBattleData.hp = blood;
//		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood);
//		eas = new ExcuteActiveSkill(upi);
//		eps = new ExcutePassiveSkill(upi);
		//		ac = new BattleAttackController(eps, upi);
//		Config.Instance.SwitchCard(els);
//		ConfigBattleUseData.Instance.StoreMapData ();


		// active skill
		leaderSkill = upi;
		excuteTrap = new ExcuteTrap ();
		InitPassiveSkill ();
//		MsgCenter.Instance.AddListener (CommandEnum.MeetTrap, DisposeTrapEvent);

		//passive skill
		foreach (var item in leaderSkill.UserUnit) {
			if (item==null ){
				continue;
			}
			SkillBase pudb = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), item.ActiveSkill, SkillType.ActiveSkill); //Skill[item.ActiveSkill];
			ActiveSkill skill = pudb as ActiveSkill;
			if(skill == null) {
				continue;
			}
			activeSkill.Add(item.MakeUserUnitKey(), skill);
			skill.StoreSkillCooling(item.MakeUserUnitKey());
		}
		
//		MsgCenter.Instance.AddListener (CommandEnum.LaunchActiveSkill, Excute);
//		MsgCenter.Instance.AddListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);	// this command use to reduce cooling one round.
    }

	public void ResetBlood () {
		maxBlood = Blood = upi.GetInitBlood();
		maxEnergyPoint = DataCenter.maxEnergyPoint;
	}

	public void Reset () {
		errorMsg = new ErrorMsg();
		upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		upi.GetSkillCollection();
		GetBaseData ();
//		els = new ExcuteLeadSkill(upi);
//		skillRecoverHP = els;
	}
	
	public void InitData (StoreBattleData sbd) {

		if (sbd == null) {
			blood = maxBlood = upi.GetInitBlood ();
			maxEnergyPoint = DataCenter.maxEnergyPoint;
		} else {
			maxBlood = upi.GetInitBlood ();
			blood = sbd.hp;
//			if(blood <= 0) {
//				PlayerDead();
//			}
			maxEnergyPoint = sbd.sp;
		}


	}

//	public void CheckPlayerDead() {
//		if(blood <= 0) {
//			PlayerDead();
//		}
//	}

    public void TrapMove(Coordinate cd) {
        if (!cd.Equals(default(Coordinate))) {
            ConsumeEnergyPoint();
        }
    }

	public void TrapInjuredDead(float value) {
        int hurtValue = System.Convert.ToInt32(value);
		KillHp (hurtValue, true);
		BattleConfigData.Instance.StoreMapData ();
    }

//	public void PlayerDead() {
//		if (blood <= 0) {
////			MsgCenter.Instance.Invoke(CommandEnum.PlayerDead);
//			ModuleManager.SendMessage(ModuleEnum.BattleMapModule,"playerdead");
//			ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"playerdead");
//		}
//	}

	public void InjuredNotDead(float probability) {
        float residualBlood = blood - maxBlood * probability;
		if (residualBlood < 1) {
			residualBlood = 1;	
        }
        Blood = System.Convert.ToInt32(residualBlood);
    }

    public void RecoveHPByActiveSkill(object data = null) {
		float value = (data == null ? tempPreHurtValue : (float)data);
        float add = 0;
        if (value <= 1) {
            add = blood * value + blood;
        }
        else {
            add = value + blood;
        }
		AttackInfo ai = AttackInfo.GetInstance ();
		ai.AttackValue = add;
		RecoverHP(ai);
    }

	public void DelayCountDownTime(float addTime) {
        countDown += addTime;
    }


    public void Sucide(object data) {
        Blood = 1;
    }

    List<AttackInfo> SortAttackSequence() {
        List<AttackInfo> sortAttack = new List<AttackInfo>();
        foreach (var item in attackInfo.Values) {
            sortAttack.AddRange(item);
        }
        attackInfo.Clear();
        int tempCount = 0;
        for (int i = DataCenter.posStart; i < DataCenter.posEnd; i++) {
            List<AttackInfo> temp = sortAttack.FindAll(a => a.UserPos == i);
            if (temp == null) {
                temp = new List<AttackInfo>();
            }
            if (temp.Count > tempCount) {
                tempCount = temp.Count;
            }
            DGTools.InsertSort(temp, new AISortByCardNumber());
            attackInfo.Add(i, temp);
        }
        sortAttack.Clear();
        for (int i = 0; i < tempCount; i++) {
            for (int j = DataCenter.posStart; j < DataCenter.posEnd; j++) {
                if (attackInfo[j].Count > 0) {
                    AttackInfo ai = attackInfo[j][0];
                    attackInfo[j].Remove(ai);
                    sortAttack.Add(ai);
                }
            }
        }
//		attackInfo.Clear ();
        DGTools.InsertSortBySequence(sortAttack, new AISortByCardNumber());
		int count = sortAttack.Count;
		float rate =  (count -1) * 0.25f;
		for (int i = 0; i < count; i++) {
			sortAttack[i].AttackRate += rate;
			sortAttack[i].ContinuAttackMultip += i;
			sortAttack[i].AttackValue *= (1 + sortAttack[i].AttackRate);
        }
        return sortAttack;
    }

	void RecoverHP(AttackInfo ai) {
//		Blood += System.Convert.ToInt32 (ai.AttackValue);
		AddBlood (System.Convert.ToInt32 (ai.AttackValue));
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refresh_item", ai, true);
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "attack_enemy",ai);
//		ModuleManager.SendMessage (ModuleEnum.BattleEnemyModule, "recover_item",ai);
    }

    public void InitEnemyInfo(QuestGrid grid) {
        Grid = grid;
    }

    public void InitBoss(List<EnemyInfo> boss,DropUnit bossDrop) {
		enemyInfo.Clear ();
		enemyInfo.AddRange (boss);
		dropUnit = bossDrop;
    }

    public List<AttackInfo> CaculateFight(int areaItem, int id, bool isBoost) {
		float value = isBoost ? 1.5f : 1f;
		return upi.CalculateSkill(areaItem, id, maxBlood, value);
    }

    public void StartAttack(object data) {
        attackInfo = upi.Attack;
		List<AttackInfo> attack = SortAttackSequence();
//        StartAttack(temp);
		MsgCenter.Instance.Invoke (CommandEnum.ShowHands, attack.Count);
		if (attack.Count > 0) {
			List<AttackInfo> extraAttack = ExtraAttack ();
			extraAttackCount = extraAttack.Count;
			attack.AddRange (extraAttack);
		}
		
		float multipe = MultipleAttack (attack);
		if (multipe > 1.0f) {
			for (int i = 0; i < attack.Count; i++) {
				attack[i].AttackValue *= multipe;
			}
		}

		foreach (var item in attack) {
			attackInfoQueue.Enqueue (item);
		}
		endCount = attackInfoQueue.Count;
		InvokeAttack ();
    }

	
    public void ClearData() {
        upi.ClearData();
        attackInfo.Clear();
    }

    public void GetBaseData() {
		ModuleManager.SendMessage (ModuleEnum.BattleBottomModule, "initdata", Blood,maxBlood,maxEnergyPoint);
    }

   public  void RecoverEnergePoint(object data) {
		ModuleManager.SendMessage (ModuleEnum.BattleEnemyModule, "skil_recover_sp", data);
        int recover = (int)data;
		if (maxEnergyPoint == 0 && recover > 0) {
			isLimit = false;
		}
        maxEnergyPoint += recover;

		AudioManager.Instance.PlayAudio (AudioEnum.sound_sp_recover);

        if (maxEnergyPoint > DataCenter.maxEnergyPoint) {
            maxEnergyPoint = DataCenter.maxEnergyPoint;	
        }
		BattleConfigData.Instance.storeBattleData.sp = maxEnergyPoint;

//        MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
		ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"energy_point",maxEnergyPoint);
    }

	public void TrapTargetPoint(Coordinate coordinate) {
        currentCoor = coordinate;
        ConsumeEnergyPoint();
    }

    bool temp = true;
    public void MoveToMapItem(object coordinate) {
        currentCoor = (Coordinate)coordinate;
        if (temp) {
            temp = false;
            return;
        }
//        MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillCooling, null);	// refresh active skill cooling.
        int addBlood = RecoverHP(maxBlood, 2);				// 3: every step.
//		Blood += addBlood;
		AddBlood (addBlood);
        ConsumeEnergyPoint();

		CoolingSkill ();
    }

	bool isLimit = false;

	public void KillHp(int value,bool dead) {
		int killBlood = blood - value;

		if (dead) {
			if(blood == 0) {
				return;
			}
			blood = killBlood < 0 ? 0 : killBlood;
//			PlayerDead();

		} else {
			blood = killBlood < 1 ? 1 : killBlood;
		}

		BattleConfigData.Instance.storeBattleData.hp = blood;
//		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood);
	}

	public void AddBlood (int value) {
		if (value == 0) {
			return;	
		}

		int addBlood = blood + value;
		blood = addBlood > maxBlood ? maxBlood : addBlood;
		BattleConfigData.Instance.storeBattleData.hp = blood;
//		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"update_blood",blood,true);
//		MsgCenter.Instance.Invoke (CommandEnum.ShowHPAnimation);
	}

    void ConsumeEnergyPoint() {	
		AudioManager.Instance.PlayAudio(AudioEnum.sound_walk);

//		if(battleQuest.ChainLinkBattle) {
//			return;
//		}

        if (maxEnergyPoint == 0) {
			KillHp(ReductionBloodByProportion(0.2f), false);
			AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
        } else {
            maxEnergyPoint--;
			BattleConfigData.Instance.storeBattleData.sp = maxEnergyPoint;
//            MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
			ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"energy_point",maxEnergyPoint);
			if(maxEnergyPoint == 0 && !isLimit) {
				isLimit = true;
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",false);
				AudioManager.Instance.PlayAudio(AudioEnum.sound_sp_limited_over);
				ModuleManager.SendMessage(ModuleEnum.BattleFullScreenTipsModule,"splimit", SPLimit as Callback);
			}
        }
    }

	void SPLimit () {
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
	}

    public void Hurt(int hurtValue) {
//		Blood -= hurtValue;
		KillHp (hurtValue, true);
    }

//    public void RefreshBlood() {
//		PlayerDead ();
//    }
			
    int ReductionBloodByProportion(float proportion) {
        return (int)(maxBlood * proportion);
    }


	///-----------attack controller
	public const float normalAttackInterv = 0.7f;
	public const float deadAttackInterv = 0.5f;
	//	private MsgCenter msgCenter;
	//	private BattleUseData bud;
	private Queue<AttackInfo> attackInfoQueue = new Queue<AttackInfo> ();
	public List<EnemyInfo> enemyInfo = new List<EnemyInfo>();
	public DropUnit dropUnit = null;
	private EnemyInfo targetEnemy;
//	private TUnitParty upi;
	private float countDownTime = 0f;
	private QuestGrid grid;
	public QuestGrid Grid {
		get{return grid;}
		set{
			grid = value;
			enemyInfo = new List<EnemyInfo>();
			foreach (var item in value.Enemy) {
				enemyInfo.Add(item);
			}
		}
	}
//	ActiveSkill passiveSkill;
	public bool battleFail = false;
	
	//	private ConfigBattleUseData configBattleUseData;
	
	/// <summary>
	/// The active animation time.
	/// </summary>
	private static float activeTime = 2f;
	
	public bool isBoss = false;

	public void SkillGravity(AttackInfo ai) {
		if (ai == null) {
			return;
		}
		
		for (int i = 0; i < enemyInfo.Count; i++) {
			int initBlood = enemyInfo[i].currentHp;
			int hurtValue = System.Convert.ToInt32(initBlood * ai.AttackValue); //eg: 15%*blood
			enemyInfo[i].KillHP(hurtValue);
		}
		CheckBattleSuccess ();
	}
	
	public void ReduceDefense(AttackInfo reduceInfo) {
		if (reduceInfo == null) {
			return;
		}

		ModuleManager.SendMessage (ModuleEnum.BattleEnemyModule, "reduce_defence", reduceInfo);

		if (!BattleMapView.reduceDefense) {
			reduceInfo.AttackRange = 1;
			MsgCenter.Instance.Invoke (CommandEnum.PlayAllEffect, reduceInfo);
		}
		
		if (BattleMapView.reduceDefense) {
			BattleMapView.reduceDefense  = false;
		}
		
		if (reduceInfo.AttackRound == 0) {
			reduceInfo = null;
		}

		for (int i = 0; i < enemyInfo.Count; i++) {
			enemyInfo[i].ReduceDefense(reduceInfo == null ? 0 : reduceInfo.AttackValue, reduceInfo);
		}
	}
	
	public static void SetEffectTime(float time) {
		activeTime = time;
		singleEffectTime = time;
	}
	
	public void ActiveSkillAttack (AttackInfo ai) {
		if (ai == null) {
			return;	
		}
		BeginAttack (ai);
		GameTimer.GetInstance ().AddCountDown (activeTime, ()=>{
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemyEnd, 0);
			
			CheckBattleSuccess ();
		});
	}
	
	public void AttackTargetTypeEnemy (AttackTargetType att) {
		if (att == null) {
			return;	
		}
		
		tempPreHurtValue = 0;
		for (int i = 0; i < enemyInfo.Count; i++) {
			EnemyInfo te = enemyInfo[i];
			if(te.GetUnitType() == att.targetType || att.targetType == (int)bbproto.EUnitType.UALL) {
				AttackInfo ai = att.attackInfo;
				int restraintType = DGTools.RestraintType(ai.AttackType);
				bool b = restraintType == te.GetUnitType();
				int hurtValue = te.CalculateInjured(ai, b);
				ai.InjuryValue = hurtValue;
				tempPreHurtValue += hurtValue;
				ai.EnemyID = te.EnemySymbol;
				AttackEnemyOnce (ai);
			}
		}
		CheckBattleSuccess ();
	}
	int endCount = 0;
	int extraAttackCount = 0;
	
	void StopAttack () {
		
	}
	
	bool CheckEnemy () {
		if (enemyInfo == null || enemyInfo.Count == 0) {
			return false;
		} else {
			return true;
		}
	}
	
	bool CheckAttackInfo () {
		if (attackInfoQueue == null || attackInfoQueue.Count == 0) {
			return false;	
		} else {
			return true;		
		}
	}
	
	float GetIntervTime () {
		if (enemyInfo.Count == 1 && enemyInfo[0].currentHp<= 0) {
			return 0.6f;
		} else {
			return 0.9f;	
		}
	}
	
	void InvokeAttack() {
		countDownTime = GetIntervTime ();
		GameTimer.GetInstance ().AddCountDown (countDownTime, ()=>{
			enemyIndex = 0;
			if (attackInfoQueue.Count == 0) {
//				MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);
				BattleAttackManager.instance.ReduceActiveSkillRound();
				
				int blood = RecoverHP(BattleAttackManager.Instance.maxBlood, 1);	//1: every round.
				
				BattleAttackManager.Instance.AddBlood(blood);
				
//				MsgCenter.Instance.Invoke(CommandEnum.AttackEnemyEnd, endCount);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"attack_enemy_end",endCount);
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"attack_enemy_end");
				
				endCount = 0;
				
				if (!CheckBattleSuccess ()) {
					return;
				}
				GameTimer.GetInstance ().AddCountDown (enemyAttackTime, AttackPlayer);
				return;
			}
			
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [2]);
			
			CheckEnemyDead();
			if (attackInfoQueue.Count <= extraAttackCount) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_ls_chase);	
			}
			BeginAttack (attackInfoQueue.Dequeue());
			InvokeAttack ();
		});
	}
	
	int tempPreHurtValue = 0;
	void BeginAttack(AttackInfo ai) {
		switch (ai.AttackRange) {
		case 0:
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
			AttackEnemyOnce (ai);
			break;
		case 1:
			if (enemyInfo.Count == 0) {
				return;		
			}
			//交替1和2的数值用以区分这个攻击是不是在一次全体攻击内
			tempAllAttakSignal =  tempAllAttakSignal > 1 ? 1 : 2;
			
			int restraintType1 = DGTools.RestraintType (ai.AttackType);
			tempPreHurtValue = 0;
			for (int i = 0; i < enemyInfo.Count; i++) {
				EnemyInfo te = enemyInfo[i];
				bool b = restraintType1 == te.GetUnitType();
				int hurtValue1 = te.CalculateInjured (ai, b);
				ai.InjuryValue = hurtValue1;
				tempPreHurtValue += hurtValue1;
				ai.EnemyID = te.EnemySymbol;
				ai.IsLink = tempAllAttakSignal;
				AttackEnemyOnce (ai);
			}
			break;
		case 2:
			RecoverHP(ai);
			break;
		}
	}
	
	List<EnemyInfo> deadEnemy = new List<EnemyInfo>();
	void CheckEnemyDead () {
		if (enemyInfo.Count == 1) {
			return;	
		}
		for (int i = enemyInfo.Count - 2; i > -1; i--) {
			EnemyInfo te = enemyInfo[i];
			if(te.currentHp <= 0) {
				deadEnemy.Add(te);
				enemyInfo.Remove(te);
			}
		}
	}
	
	bool CheckBattleSuccess() {
		for (int i = 0; i < deadEnemy.Count; i++) {
			EnemyInfo tei = deadEnemy[i];
			tei.IsDead = true;
			if(tei.Equals(targetEnemy)) {
				targetEnemy = null;
			}
//			MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, tei);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"enemy_dead",tei);
			if(grid != null) {
//				MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",grid.dropPos);
			} else if(dropUnit != null){
//				MsgCenter.Instance.Invoke(CommandEnum.DropItem, dropUnit.DropPos);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",dropUnit.dropPos);
			}
		}
		deadEnemy.Clear ();
		
		for (int i = enemyInfo.Count - 1; i > -1; i--) {
			int blood = enemyInfo[i].currentHp;
			if(blood <= 0){
				EnemyInfo te = enemyInfo[i];
				enemyInfo.Remove(te);
				te.IsDead = true;
				if(te.Equals(targetEnemy)) {
					targetEnemy = null;
				}
//				MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, te);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"enemy_dead",te);
				if(grid != null) {
//					MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
					ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",grid.dropPos);
				} else if(dropUnit != null){
//					MsgCenter.Instance.Invoke(CommandEnum.DropItem, dropUnit.DropPos);
					ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",dropUnit.dropPos);
				}
			}
		}
		
		if (enemyInfo.Count == 0) {
//			BattleBottomView.notClick = false;
			GameTimer.GetInstance().AddCountDown(1f, BattleEnd); //TODO: set time in const config
			return false;
		}
		
		return true;
	}
	
	void BattleEnd() {
		BattleConfigData.Instance.ClearActiveSkill ();
		MsgCenter.Instance.Invoke (CommandEnum.GridEnd, null);
		//		MsgCenter.Instance.Invoke(CommandEnum.BattleEnd, battleFail);
		ModuleManager.Instance.HideModule(ModuleEnum.BattleManipulationModule);
		ModuleManager.Instance.HideModule (ModuleEnum.BattleEnemyModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleMapModule);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_battle_over);
		ClearData();
		//		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_dungeon);
	}
	
	public void TargetEnemy(EnemyInfo enemyInfo) {
		if(targetEnemy == null || !targetEnemy.Equals(enemyInfo)) {
			targetEnemy = enemyInfo;
		}else{
			targetEnemy = null;
		}
	}
	
	EnemyInfo prevAttackEnemyInfo;
	
	EnemyInfo GetTargetEnemy(int restraintType) {
		EnemyInfo te;
		if (targetEnemy != null && targetEnemy.currentHp != 0) {
			te = targetEnemy;
		} else {
			if(enemyInfo.Count == 1 && enemyInfo[0].currentHp <= 0) {
				if(prevAttackEnemyInfo == null) {
					prevAttackEnemyInfo = enemyInfo[0];
				}
				return prevAttackEnemyInfo;
			}
			
			List<EnemyInfo> injuredEnemy = enemyInfo.FindAll (a => a.IsInjured () == true);
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
	
	int tempAllAttakSignal = 1;

	void AttackEnemyOnce (AttackInfo ai){
//		MsgCenter.Instance.Invoke (CommandEnum.AttackEnemy, ai);
		ModuleManager.SendMessage (ModuleEnum.BattleManipulationModule, "attack_enemy",ai);
		ModuleManager.SendMessage (ModuleEnum.BattleEnemyModule, "attack_enemy",ai);
		ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule, "refresh_item", ai,false);
	}
	
	public void FirstAttack () {
		foreach (var item in enemyInfo) {
			item.FirstAttack();
		}
	}
	
	public void AttackPlayer () {
		if (CheckBattleSuccess ()) {
			bool enterEnemyPhase = false;
			for (int i = 0; i < enemyInfo.Count; i++) {
				enemyInfo[i].Next();
				if(enemyInfo[i].GetRound() <= 0) {
					enterEnemyPhase = true;
				}
			}
			if(enterEnemyPhase) {
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [1]);
				LoopEnemyAttack ();	
			}
			else {
				EnemyAttackLoopEnd();
			}
		} else {
			//			bud.battleQuest.battle.ShieldInput(true);
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
		}
	}
	
	int enemyIndex = 0;
	EnemyInfo te;
	void LoopEnemyAttack () {
		countDownTime = 0.4f;
		if (enemyIndex >= enemyInfo.Count) {
			GameTimer.GetInstance ().AddCountDown (countDownTime, EnemyAttackLoopEnd);
			return;
		}
		te = enemyInfo [enemyIndex];
		enemyIndex ++;
		
		if (te.GetRound () > 0) {
			LoopEnemyAttack ();
			return;
		}
		
		GameTimer.GetInstance ().AddCountDown (countDownTime, EnemyAttack);
	}
	
	List<AttackInfo> antiInfo = new List<AttackInfo>();
	
	void EnemyAttack () {
		if (te.GetRound () <= 0) {
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,te.EnemySymbol);
			int attackType = te.GetUnitType ();
			float reduceValue = te.AttackValue;

			reduceValue = ReduceHurtValue(reduceValue, attackType);
			
			//			Debug.LogError("leadSkillReuduce is null : " + (leadSkillReuduce == null) + " reduceValue : " + reduceValue);
			
			int hurtValue = upi.CaculateInjured (attackType, reduceValue);
			
			//			Debug.LogError("hurtValue : " + hurtValue);
			
			BattleAttackManager.Instance.Hurt(hurtValue);
			te.ResetAttakAround ();	
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",te);
			//			Debug.LogError("EnemyAttack attackType : " + attackType);
			List<AttackInfo> temp = Dispose(attackType, hurtValue);
			
			for (int i = 0; i < temp.Count; i++) {
				temp[i].EnemyID = te.EnemySymbol;
				antiInfo.Add(temp[i]);
			}
			antiInfo.AddRange(temp);
			if(!isBoss) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_enemy_attack);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_boss_battle);
			}
		}
		
		if (enemyIndex == enemyInfo.Count) {
			EnemyAttackLoopEnd();
		}
		else {
			LoopEnemyAttack();
		}
	}    
	
	void EnemyAttackLoopEnd() {
		if(BattleAttackManager.Instance.Blood > 0) {
			//			Debug.LogError("antiInfo.Count : " + antiInfo.Count);
			if (antiInfo.Count == 0) {
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [0]);
				GameTimer.GetInstance ().AddCountDown (0.5f, EnemyAttackEnd);
				return;
			}
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [3]); // stateInfo [3]="PassiveSkill"
			GameTimer.GetInstance ().AddCountDown (0.3f, LoopAntiAttack);
		}
		else{
			EnemyAttackEnd();
			//			bud.battleQuest.battle.ShieldInput(true);	
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [0]);
		}
	}
	
	void Fail () {
		battleFail = true;
		BattleEnd();
		return;
	}
	
	void EnemyAttackEnd () {
//		BattleBottomView.notClick = false;
		CheckBattleSuccess ();
		ClearData();
		//		bud.battleQuest.battle.ShieldInput(true);	
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"enemy_attack_end");
		BattleConfigData.Instance.storeBattleData.attackRound ++;
		BattleConfigData.Instance.storeBattleData.EnemyInfo = enemyInfo;
		MsgCenter.Instance.Invoke (CommandEnum.EnemyAttackEnd, null);
		BattleConfigData.Instance.StoreMapData ();
	}
	
	void LoopAntiAttack() {
		float intervTime = 0.5f;
		GameTimer.GetInstance ().AddCountDown (intervTime, ()=>{
			if (antiInfo.Count == 0) {
				EnemyAttackEnd();
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [0]);
				return;
			}
			
			AttackInfo ai = antiInfo [0];
			antiInfo.RemoveAt (0);
			EnemyInfo te = enemyInfo.Find(a=>a.EnemySymbol == ai.EnemyID);
			//		Debug.LogError ("AntiAttack te : " + te);
			if (te == default(EnemyInfo)) {
				return;	
			}
			int restraintType = DGTools.RestraintType (ai.AttackType);
			bool restaint = restraintType == te.GetUnitType ();
			int hurtValue = te.CalculateInjured (ai, restaint);
			ai.InjuryValue = hurtValue;
			AttackEnemyOnce (ai);
			LoopAntiAttack ();
		});
	}

	//
	private Dictionary<string,ActiveSkill> activeSkill = new Dictionary<string, ActiveSkill> ();
	private UnitParty leaderSkill;
	
	private ActiveSkill iase;
	private UserUnit userUnit;
	
	private const float fixEffectTime = 2f;
	
	public static float singleEffectTime = 2f;
	
	/// <summary>
	/// novice guide active skill cooling done.
	/// </summary>
	public static void CoolingDoneLeaderActiveSkill() {
		UserUnit tuu = DataCenter.Instance.PartyInfo.CurrentParty.UserUnit [0];
		ActiveSkill sbi = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), tuu.UnitInfo.activeSkill, SkillType.ActiveSkill) as ActiveSkill;
		sbi.skillCooling = 0;
		sbi.RefreashCooling ();
	}
	
	
//	public ExcuteActiveSkill(ILeaderSkill ils) {
//
//		//		MsgCenter.Instance.AddListener (CommandEnum.ShowHands, ShowHands);	// one normal skill can reduce cooling one round.
//	}
	
	
	public ActiveSkill GetActiveSkill(string userUnitID) {
		ActiveSkill iase = null;
		activeSkill.TryGetValue (userUnitID, out iase);
		return iase;
	}
	AttackInfo ai;
	void ExcuteLeaderSkill(object data) {
		userUnit = data as UserUnit;
		if (userUnit != null) {
			string id = userUnit.MakeUserUnitKey();
			if(activeSkill.TryGetValue(id, out iase)) {
				
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule, stateInfo [4]);
				
				AudioManager.Instance.PlayAudio(AudioEnum.sound_active_skill);
				
				ai = AttackInfo.GetInstance();
				ai.UserUnitID = userUnit.MakeUserUnitKey();
				ai.SkillID = (iase as ActiveSkill).id;
				ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"activeskill", ai);
				
				
				GameTimer.GetInstance().AddCountDown(BattleAttackEffectView.activeSkillEffectTime, ()=>{
					MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, true);
					GameTimer.GetInstance().AddCountDown(1f,ExcuteLeaderSkill);
					ModuleManager.SendMessage (ModuleEnum.BattleFullScreenTipsModule, "ready",userUnit);
					AudioManager.Instance.PlayAudio (AudioEnum.sound_active_skill);
					
					AudioManager.Instance.PlayAudio (AudioEnum.sound_as_appear);
				});
			} else {
				//				Debug.LogError("activeSkill.TryGetValue false  : ");
			}
		}
	}

	void ExcuteLeaderSkill() {
		//		Debug.LogError ("Excute active skill iase: " + iase + " userUnit : " + userUnit);
		if (iase == null || userUnit == null) {
			return;	
		}
		
		iase = activeSkill[ai.UserUnitID];
		iase.Excute(ai.UserUnitID, userUnit.Attack);
		iase = null;
		userUnit = null;
		GameTimer.GetInstance ().AddCountDown (fixEffectTime + singleEffectTime, ()=>{
			MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, false);
		});
	}
	
	public void ReduceActiveSkillRound() {
		CoolingSkill ();
	}
	
	List<ActiveSkill> coolingDoneSkill = new List<ActiveSkill>();
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

//	private ILeaderSkill leaderSkill;
	public ExcuteTrap excuteTrap;
	private Dictionary<string,SkillBase> passiveSkills = new Dictionary<string,SkillBase> ();
	private Dictionary<string,float> multipe = new Dictionary<string, float> ();


	void InitPassiveSkill() {
		foreach (var item in leaderSkill.UserUnit) {
			if (item==null) {
				continue;
			}
			
			int id = item.PassiveSkill;
			
			if(id == -1) {
				continue;
			}
			
			SkillBase ipe = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), id, SkillType.PassiveSkill); //Skill[id] as IPassiveExcute;
			
			if(ipe == null) {
				continue;
			}
			
			string name = item.MakeUserUnitKey();
			passiveSkills.Add(name,ipe);
			multipe.Add(name,item.AttackMultiple);
		}
	}
	
	Queue <TrapBase> trap = new Queue<TrapBase>();
	public void DisposeTrapEvent(TrapBase tb) {
		if (tb == null) {
			return;		
		}
		trap.Enqueue (tb);
		if (passiveSkills.Values.Count == 0) {
			DisposeTrap (false);
		} else {
			foreach (var item in passiveSkills) {
//				bool b = (bool)item.Value.Excute(tb, this);
//				if(b) {
					foreach (var unitItem in leaderSkill.UserUnit) {
						if(unitItem == null) {
							continue;
						}
						
						if(unitItem.MakeUserUnitKey() == item.Key) {
							AttackInfo ai = AttackInfo.GetInstance();
							ai.UserUnitID = unitItem.MakeUserUnitKey();
							ai.SkillID = item.Value.id;
							//							MsgCenter.Instance.Invoke(CommandEnum.ShowPassiveSkill, ai);
							ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refreshitem", ai);
							
						}
					}
//				}
			}
		}
	}
	
	private List<AttackInfo> attackList = new List<AttackInfo> ();
	
	List<AttackInfo> Dispose (int AttackType, int attack) {
		attackList.Clear ();
		foreach (var item in passiveSkills) {
			if(item.Value is TSkillAntiAttack) {
//				AttackInfo ai = item.Value.Excute(AttackType, this) as AttackInfo;
				if(ai != null) {
					ai.SkillID = item.Value.id;
					ai.UserUnitID = item.Key;
					ai.AttackValue *= attack;
					ai.AttackValue *= multipe[item.Key];
					attackList.Add(ai);
				}
			}
		}
		return attackList;
	}
	
	public void DisposeTrap (bool isAvoid) {
		if (trap.Count == 0) {
			return;	
		}
		TrapBase tb = trap.Dequeue ();
		
		if (isAvoid) {
			return;
		} else {
			TrapBase ie = tb as TrapBase;
			excuteTrap.Excute (ie);	
		}
	}

//	TUnitParty leadSkill;
	List<string> RemoveSkill = new List<string> ();
	
	const float time = 0.5f;
	Queue<string> leaderSkillQueue = new Queue<string> ();
	
	public void ExcuteActiveSkill() {
		int temp = 0;
		foreach (var item in leaderSkill.LeadSkill) {
			temp++;
			if(item.Value is TSkillBoost) {
				leaderSkillQueue.Enqueue(item.Key);
				GameTimer.GetInstance().AddCountDown(temp*time, ExcuteStartLeaderSkill);
			}
		}
	}
	
	void ExcuteStartLeaderSkill() {
		string key = leaderSkillQueue.Dequeue ();
		//		Debug.LogError ("ExcuteStartLeaderSkill : " + key);
		DisposeBoostSkill (key, leaderSkill.LeadSkill [key]);
		leaderSkill.LeadSkill.Remove (key);
		if (leaderSkillQueue.Count == 0) {
			MsgCenter.Instance.Invoke (CommandEnum.LeaderSkillEnd, null);
		}
	}
	
	void RemoveLeaderSkill () {
		for (int i = 0; i < RemoveSkill.Count; i++) {
			leaderSkill.LeadSkill.Remove(RemoveSkill[i]);
		}
	}
	
	void DisposeBoostSkill (string userunit, SkillBase pdb) {
		TSkillBoost tbs = pdb as TSkillBoost;
		if (tbs != null) {
			AttackInfo ai = AttackInfo.GetInstance(); //new AttackInfo();
			ai.UserUnitID = userunit;
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
			
			AudioManager.Instance.PlayAudio(AudioEnum.sound_ls_activate);
			
			foreach (var item in leaderSkill.UserUnit) {
				if( item == null) {
					continue;
				}
				item.SetAttack(tbs.GetBoostValue, tbs.GetTargetValue, tbs.GetTargetType, tbs.GetBoostType);
			}
		}
		else {
			DisposeDelayOperateTime(userunit, pdb);
		}
	}
	
	void DisposeDelayOperateTime (string userunit, SkillBase pdb) {
		SkillDelayTime tst = pdb as SkillDelayTime;
		if (tst != null) {
			AttackInfo ai = AttackInfo.GetInstance(); //new AttackInfo();
			ai.UserUnitID = userunit;
			
			AudioManager.Instance.PlayAudio(AudioEnum.sound_ls_activate);
			
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemy, ai);
			//			MsgCenter.Instance.Invoke(CommandEnum.LeaderSkillDelayTime, tst.DelayTime);
			BattleAttackManager.Instance.DelayCountDownTime(tst.DelayTime);
		}
	}
	
	void PlayLeaderSkillAudio() {
		if(!isPlay) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_ls_activate);
			isPlay = true;
		}
	}
	
	void ResetIsPlay() {
		isPlay = false;
	}
	
	bool isPlay = false;
	
	public float ReduceHurtValue (float hurt,int type) {
		if (leaderSkill.LeadSkill.Count == 0) {
			return hurt;	
		}
		foreach (var item in leaderSkill.LeadSkill) {
			SkillReduceHurt trh = item.Value as SkillReduceHurt;
			if(trh != null) {
				
				PlayLeaderSkillAudio();
				
				hurt = trh.ReduceHurt(hurt,type);
				if(trh.CheckUseDone()) {
					RemoveSkill.Add(item.Key);
				}
			}
		}
		RemoveLeaderSkill ();
		
		ResetIsPlay ();
		
		return hurt;
	}
	
	public List<AttackInfo> ExtraAttack (){
		List<AttackInfo> ai = new List<AttackInfo>();
		
		if (leaderSkill.LeadSkill.Count == 0) {
			return ai;
		}
		foreach (var item in leaderSkill.LeadSkill) {
			SkillExtraAttack tsea = item.Value as SkillExtraAttack;
			//			Debug.LogError("tsea : " + tsea + " value : " + item.Value);
			if(tsea == null) {
				continue;
			}
			
			PlayLeaderSkillAudio();
			
			string id = item.Key;
			foreach (var item1 in leaderSkill.UserUnit) {
				if(item1 == null) {
					continue;
				}
				if(item1.MakeUserUnitKey() == id) {
					AttackInfo attack = tsea.AttackValue(item1.Attack, item1);
					ai.Add(attack);
					break;
				}
			}
		}
		
		ResetIsPlay ();
		
		return ai;
	}
	
	public List<int> SwitchCard (List<int> cardQuene) {
		if (leaderSkill.LeadSkill.Count == 0) {
			return null;
		}
		
		foreach (var item in leaderSkill.LeadSkill) {
			SkillConvertUnitType tcut = item.Value as SkillConvertUnitType;
			
			if(tcut == null) {
				continue;
			}
			
			PlayLeaderSkillAudio();
			
			for (int i = 0; i < cardQuene.Count; i++) {
				cardQuene[i] = tcut.SwitchCard(cardQuene[i]);
			}
		}
		ResetIsPlay ();
		return cardQuene;
	}
	
	public int SwitchCard (int card) {
		if (leaderSkill.LeadSkill.Count == 0) {
			return card;
		}
		
		foreach (var item in leaderSkill.LeadSkill) {
			SkillConvertUnitType tcut = item.Value as SkillConvertUnitType;	
			if(tcut == null) {
				continue;
			}
			
			PlayLeaderSkillAudio();
			
			card = tcut.SwitchCard(card);
		}
		ResetIsPlay ();
		return card;
	}
	
	/// <summary>
	/// Recovers the H.
	/// </summary>
	/// <returns>The H.</returns>
	/// <param name="blood">Blood.</param>
	/// <param name="type">Type. 0 = right now. 1 = every round. 2 = every step.</param>
	public int RecoverHP (int blood, int type) {
		if (leaderSkill.LeadSkill.Count == 0) {
			return 0;	//recover zero hp
		}
		int recoverHP = 0;
		foreach (var item in leaderSkill.LeadSkill) {
			SkillRecoverHP trhp = item.Value as SkillRecoverHP;
			if(trhp == null) {
				continue;
			}
			PlayLeaderSkillAudio();
			
			recoverHP = trhp.RecoverHP(blood, type);
		}
		ResetIsPlay ();
		return recoverHP;
	}
	
	public float MultipleAttack (List<AttackInfo> attackInfo) {
		if (leaderSkill.LeadSkill.Count == 0) {
			return 1f;
		}
		float multipe = 0f;
		foreach (var item in leaderSkill.LeadSkill) {
			LeaderSkillMultipleAttack trhp = item.Value as LeaderSkillMultipleAttack;
			if(trhp == null) {
				continue;
			}
			
			PlayLeaderSkillAudio();
			
			multipe += trhp.MultipeAttack(attackInfo);
		}
		
		ResetIsPlay ();
		
		return multipe;
	}
	
	/// <summary>
	/// utility ready move animation
	/// </summary>
	int tempLeaderSkillCount = 0;
	public int CheckLeaderSkillCount () {
		foreach (var item in leaderSkill.LeadSkill.Values) {
			if(item is TSkillBoost) {
				tempLeaderSkillCount ++;
			}else if(item is SkillDelayTime){
				tempLeaderSkillCount ++;
			}
		}
		return tempLeaderSkillCount;
	}
}


