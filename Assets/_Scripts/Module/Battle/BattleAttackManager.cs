using UnityEngine;
using System.Collections.Generic;

public class BattleAttackManager {
//	public BattleMapModule battleQuest;
    private ErrorMsg errorMsg;
    public TUnitParty upi;
    public int maxBlood = 0;
    private int blood = 0;
    public int Blood {
		set {
//			Debug.LogError("Blood : " + value);
			if(value == 0) {
				blood = value;
				PlayerDead();
			} else if(value < 0) {
				if(blood == 0) 
					return;
				blood = 0;
				MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
				PlayerDead();
			} else if(value > maxBlood) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
				if(blood < maxBlood) {
					blood = maxBlood;
					MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
				}

			} else {
				if(value > blood) {
					AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
					MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
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
    private List<TEnemyInfo> currentEnemy = new List<TEnemyInfo>();
    private List<TEnemyInfo> showEnemy = new List<TEnemyInfo>();
//    public BattleAttackController ac;
    private ExcuteLeadSkill els;
	public ExcuteLeadSkill Els{
		get {return els;}
	}
//    private ExcuteActiveSkill eas;
//	public ExcuteActiveSkill excuteActiveSkill {
//		get { return eas; }
//	}
//    private ExcutePassiveSkill eps;
//	public ExcutePassiveSkill excutePassiveSkill {
//		get { return eps; }
//	}
    private ILeaderSkillRecoverHP skillRecoverHP;

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

    private BattleAttackManager() {
//		battleQuest = bq;
//		configBattleUseData = ConfigBattleUseData.Instance;
		ListenEvent();
		isInit = false;
		errorMsg = new ErrorMsg();
		upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		upi.GetSkillCollection();
		GetBaseData ();

		els = new ExcuteLeadSkill(upi);

		skillRecoverHP = els;

		RegisterEvent ();
		//		configBattleUseData = ConfigBattleUseData.Instance;
		SetEffectTime (2f);

		els.Excute();

//		ConfigBattleUseData.Instance.storeBattleData.hp = blood;
		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		GetBaseData ();
//		eas = new ExcuteActiveSkill(upi);
//		eps = new ExcutePassiveSkill(upi);
		//		ac = new BattleAttackController(eps, upi);
//		Config.Instance.SwitchCard(els);
//		ConfigBattleUseData.Instance.StoreMapData ();


		// active skill
		leaderSkill = upi;
		excuteTrap = new ExcuteTrap ();
		InitPassiveSkill ();
		MsgCenter.Instance.AddListener (CommandEnum.MeetTrap, DisposeTrapEvent);

		//passive skill
		foreach (var item in leaderSkill.UserUnit.Values) {
			if (item==null ){
				continue;
			}
			ProtobufDataBase pudb = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), item.ActiveSkill, SkillType.ActiveSkill); //Skill[item.ActiveSkill];
			ActiveSkill skill = pudb as ActiveSkill;
			if(skill == null) {
				continue;
			}
			activeSkill.Add(item.MakeUserUnitKey(), skill);
			skill.StoreSkillCooling(item.MakeUserUnitKey());
		}
		
		MsgCenter.Instance.AddListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);	// this command use to reduce cooling one round.
    }

	public void ResetBlood () {
		maxBlood = Blood = upi.GetInitBlood();
		maxEnergyPoint = DataCenter.maxEnergyPoint;
	}

	public void Reset () {
		ListenEvent();
		isInit = false;
		errorMsg = new ErrorMsg();
		upi = DataCenter.Instance.PartyInfo.CurrentParty; 
		upi.GetSkillCollection();
		GetBaseData ();
		els = new ExcuteLeadSkill(upi);
		skillRecoverHP = els;
	}

	bool isInit = false;
	public void InitData (TStoreBattleData sbd) {

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

	public void CheckPlayerDead() {
		if(blood <= 0) {
			PlayerDead();
		}
	}

    ~BattleAttackManager() { }

    void ListenEvent() {
//        MsgCenter.Instance.AddListener(CommandEnum.InquiryBattleBaseData, GetBaseData);
//        MsgCenter.Instance.AddListener(CommandEnum.MoveToMapItem, MoveToMapItem);
//        MsgCenter.Instance.AddListener(CommandEnum.StartAttack, StartAttack);
//        MsgCenter.Instance.AddListener(CommandEnum.RecoverHP, RecoverHP);
        MsgCenter.Instance.AddListener(CommandEnum.LeaderSkillDelayTime, DelayCountDownTime);
        MsgCenter.Instance.AddListener(CommandEnum.ActiveSkillRecoverHP, RecoveHPByActiveSkill);
        MsgCenter.Instance.AddListener(CommandEnum.SkillSucide, Sucide);
        MsgCenter.Instance.AddListener(CommandEnum.SkillRecoverSP, RecoverEnergePoint);
        MsgCenter.Instance.AddListener(CommandEnum.TrapMove, TrapMove);
        MsgCenter.Instance.AddListener(CommandEnum.TrapInjuredDead, TrapInjuredDead);
        MsgCenter.Instance.AddListener(CommandEnum.InjuredNotDead, InjuredNotDead);
        MsgCenter.Instance.AddListener(CommandEnum.TrapTargetPoint, TrapTargetPoint);
    }

    public void RemoveListen() {
//        MsgCenter.Instance.RemoveListener(CommandEnum.MoveToMapItem, MoveToMapItem);
//        MsgCenter.Instance.RemoveListener(CommandEnum.StartAttack, StartAttack);
//        MsgCenter.Instance.RemoveListener(CommandEnum.RecoverHP, RecoverHP);
//        MsgCenter.Instance.RemoveListener(CommandEnum.LeaderSkillDelayTime, DelayCountDownTime);
//        MsgCenter.Instance.RemoveListener(CommandEnum.ActiveSkillRecoverHP, RecoveHPByActiveSkill);
//        MsgCenter.Instance.RemoveListener(CommandEnum.SkillSucide, Sucide);
//        MsgCenter.Instance.RemoveListener(CommandEnum.SkillRecoverSP, RecoverEnergePoint);
//        MsgCenter.Instance.RemoveListener(CommandEnum.TrapMove, TrapMove);
//        MsgCenter.Instance.RemoveListener(CommandEnum.TrapInjuredDead, TrapInjuredDead);
//        MsgCenter.Instance.RemoveListener(CommandEnum.InjuredNotDead, InjuredNotDead);
//        MsgCenter.Instance.RemoveListener(CommandEnum.TrapTargetPoint, TrapTargetPoint);
//
//		countDown = 5f;
//        eas.RemoveListener();
//        eps.RemoveListener();
//        ac.RemoveListener();
//        els = null;
//        eas = null;
//        eps = null;
//        ac = null;
    }

    void TrapMove(object data) {
        if (data == null) {
            ConsumeEnergyPoint();
        }
    }

    void TrapInjuredDead(object data) {
        float value = (float)data;
        int hurtValue = System.Convert.ToInt32(value);
		KillHp (hurtValue, true);
		BattleConfigData.Instance.StoreMapData ();
    }

	public void PlayerDead() {
		if (blood <= 0) {
//			MsgCenter.Instance.Invoke(CommandEnum.PlayerDead);
			ModuleManager.SendMessage(ModuleEnum.BattleMapModule,"playerdead");
			ModuleManager.SendMessage(ModuleEnum.BattleBottomModule,"playerdead");
		}
	}

    void InjuredNotDead(object data) {
        float probability = (float)data;
        float residualBlood = blood - maxBlood * probability;
		if (residualBlood < 1) {
			residualBlood = 1;	
        }
        Blood = System.Convert.ToInt32(residualBlood);
    }

    void RecoveHPByActiveSkill(object data) {
        float value = (float)data;
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

    void DelayCountDownTime(object data) {
        float addTime = (float)data;
        countDown += addTime;
    }


    void Sucide(object data) {
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

    public void InitEnemyInfo(TQuestGrid grid) {
        Grid = grid;
    }

    public void InitBoss(List<TEnemyInfo> boss, TDropUnit bossDrop) {
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
        LeadSkillReduceHurt(els);
//        StartAttack(temp);
		MsgCenter.Instance.Invoke (CommandEnum.ShowHands, attack.Count);
		if (attack.Count > 0) {
			List<AttackInfo> extraAttack = leaderSkilllExtarAttack.ExtraAttack ();
			extraAttackCount = extraAttack.Count;
			attack.AddRange (extraAttack);
		}
		
		float multipe = leaderSkillMultiple.MultipleAttack (attack);
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
        MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
    }

    void TrapTargetPoint(object coordinate) {
        currentCoor = (Coordinate)coordinate;
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
        int addBlood = skillRecoverHP.RecoverHP(maxBlood, 2);				// 3: every step.
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
			PlayerDead();

		} else {
			blood = killBlood < 1 ? 1 : killBlood;
		}

		BattleConfigData.Instance.storeBattleData.hp = blood;
		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
	}

	public void AddBlood (int value) {
		if (value == 0) {
			return;	
		}

		int addBlood = blood + value;
		blood = addBlood > maxBlood ? maxBlood : addBlood;
		BattleConfigData.Instance.storeBattleData.hp = blood;
		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		MsgCenter.Instance.Invoke (CommandEnum.ShowHPAnimation);
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
            MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
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

    public void RefreshBlood() {
		PlayerDead ();
    }
			
    int ReductionBloodByProportion(float proportion) {
        return (int)(maxBlood * proportion);
    }


	///-----------attack controller
	public const float normalAttackInterv = 0.7f;
	public const float deadAttackInterv = 0.5f;
	//	private MsgCenter msgCenter;
	//	private BattleUseData bud;
	private Queue<AttackInfo> attackInfoQueue = new Queue<AttackInfo> ();
	public List<TEnemyInfo> enemyInfo = new List<TEnemyInfo>();
	public TDropUnit dropUnit = null;
	private TEnemyInfo targetEnemy;
//	private TUnitParty upi;
	private float countDownTime = 0f;
	private TQuestGrid grid;
	public TQuestGrid Grid {
		get{return grid;}
		set{
			grid = value;
			enemyInfo = new List<TEnemyInfo>();
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

	
	void RegisterEvent () {
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		MsgCenter.Instance.AddListener (CommandEnum.SkillGravity, Gravity);
		MsgCenter.Instance.AddListener (CommandEnum.ReduceDefense, ReduceDefense);
		MsgCenter.Instance.AddListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
		MsgCenter.Instance.AddListener (CommandEnum.TargetEnemy, TargetEnemy);
	}
	
	void RemoveEvent () {
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillAttack, ActiveSkillAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillDrawHP, DrawHP);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillGravity, Gravity);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReduceDefense, ReduceDefense);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackTargetType, AttackTargetTypeEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.TargetEnemy, TargetEnemy);
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
		CheckBattleSuccess ();
	}
	
	AttackInfo reduceInfo = null;
	bool isReduce = false;
	
	void ReduceDefense(object data) {
		reduceInfo = data as AttackInfo;
		if (reduceInfo == null) {
			return;
		}
		
		if (!isReduce && !BattleMapView.reduceDefense) {
			reduceInfo.AttackRange = 1;
			MsgCenter.Instance.Invoke (CommandEnum.PlayAllEffect, reduceInfo);
		}
		
		if (BattleMapView.reduceDefense) {
			BattleMapView.reduceDefense  = false;
		}
		
		if (reduceInfo.AttackRound == 0) {
			reduceInfo = null;
			ReduceEnemy(null);
			isReduce = false;
			return;
		}
		
		ReduceEnemy (reduceInfo);
		isReduce = true;
	}
	
	void ReduceEnemy(AttackInfo attack) {
		for (int i = 0; i < enemyInfo.Count; i++) {
			enemyInfo[i].ReduceDefense(attack == null ? 0 : attack.AttackValue, attack);
		}
	}
	
	public static void SetEffectTime(float time) {
		activeTime = time;
		singleEffectTime = time;
	}
	
	void ActiveSkillAttack (object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}
		BeginAttack (ai);
		GameTimer.GetInstance ().AddCountDown (activeTime, ()=>{
			MsgCenter.Instance.Invoke(CommandEnum.AttackEnemyEnd, 0);
			
			CheckBattleSuccess ();
		});
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
				AttackEnemyOnce (ai);
			}
		}
		CheckBattleSuccess ();
	}
	int endCount = 0;
	int extraAttackCount = 0;
	
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
		if (enemyInfo.Count == 1 && enemyInfo[0].initBlood<= 0) {
			return 0.6f;
		} else {
			return 0.9f;	
		}
	}
	
	float GetEnemyTime () {
		return 0.5f;
	}
	
	void InvokeAttack() {
		countDownTime = GetIntervTime ();
		GameTimer.GetInstance ().AddCountDown (countDownTime, ()=>{
			enemyIndex = 0;
			if (attackInfoQueue.Count == 0) {
				MsgCenter.Instance.Invoke (CommandEnum.ReduceActiveSkillRound);
				
				int blood = leaderSkillRecoverHP.RecoverHP(BattleAttackManager.Instance.maxBlood, 1);	//1: every round.
				
				BattleAttackManager.Instance.AddBlood(blood);
				
//				MsgCenter.Instance.Invoke(CommandEnum.AttackEnemyEnd, endCount);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"attack_enemy_end",endCount);
				ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"attack_enemy_end");
				
				endCount = 0;
				
				if (!CheckBattleSuccess ()) {
					return;
				}
				GameTimer.GetInstance ().AddCountDown (GetEnemyTime(), AttackPlayer);
				return;
			}
			
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [2]);
			
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
				TEnemyInfo te = enemyInfo[i];
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
	
	List<TEnemyInfo> deadEnemy = new List<TEnemyInfo>();
	void CheckEnemyDead () {
		if (enemyInfo.Count == 1) {
			return;	
		}
		for (int i = enemyInfo.Count - 2; i > -1; i--) {
			TEnemyInfo te = enemyInfo[i];
			if(te.initBlood <= 0) {
				deadEnemy.Add(te);
				enemyInfo.Remove(te);
			}
		}
	}
	
	bool CheckBattleSuccess() {
		for (int i = 0; i < deadEnemy.Count; i++) {
			TEnemyInfo tei = deadEnemy[i];
			tei.IsDead = true;
			if(tei.Equals(targetEnemy)) {
				targetEnemy = null;
			}
//			MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, tei);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"enemy_dead",tei);
			if(grid != null) {
//				MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",grid.DropPos);
			} else if(dropUnit != null){
//				MsgCenter.Instance.Invoke(CommandEnum.DropItem, dropUnit.DropPos);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",dropUnit.DropPos);
			}
		}
		deadEnemy.Clear ();
		
		for (int i = enemyInfo.Count - 1; i > -1; i--) {
			int blood = enemyInfo[i].initBlood;
			if(blood <= 0){
				TEnemyInfo te = enemyInfo[i];
				enemyInfo.Remove(te);
				te.IsDead = true;
				if(te.Equals(targetEnemy)) {
					targetEnemy = null;
				}
//				MsgCenter.Instance.Invoke(CommandEnum.EnemyDead, te);
				ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"enemy_dead",te);
				if(grid != null) {
//					MsgCenter.Instance.Invoke(CommandEnum.DropItem, grid.DropPos);
					ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",grid.DropPos);
				} else if(dropUnit != null){
//					MsgCenter.Instance.Invoke(CommandEnum.DropItem, dropUnit.DropPos);
					ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"drop_item",dropUnit.DropPos);
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
		reduceInfo = null;
		MsgCenter.Instance.Invoke (CommandEnum.GridEnd, null);
		//		MsgCenter.Instance.Invoke(CommandEnum.BattleEnd, battleFail);
		ModuleManager.Instance.HideModule(ModuleEnum.BattleManipulationModule);
		ModuleManager.Instance.HideModule (ModuleEnum.BattleEnemyModule);
		ModuleManager.Instance.ShowModule (ModuleEnum.BattleMapModule);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_battle_over);
		ClearData();
		//		AudioManager.Instance.PlayBackgroundAudio (AudioEnum.music_dungeon);
	}
	
	void TargetEnemy(object data) {
		TEnemyInfo enemyInfo = data as TEnemyInfo;
		if(targetEnemy == null || !targetEnemy.Equals(enemyInfo)) {
			targetEnemy = enemyInfo;
		}else{
			targetEnemy = null;
		}
	}
	
	TEnemyInfo prevAttackEnemyInfo;
	
	TEnemyInfo GetTargetEnemy(int restraintType) {
		TEnemyInfo te;
		if (targetEnemy != null && targetEnemy.initBlood != 0) {
			te = targetEnemy;
		} else {
			if(enemyInfo.Count == 1 && enemyInfo[0].initBlood <= 0) {
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
				MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [1]);
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
	TEnemyInfo te;
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
//			MsgCenter.Instance.Invoke (CommandEnum.EnemyAttack, te.EnemySymbol);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,te.EnemySymbol);
			int attackType = te.GetUnitType ();
			float reduceValue = te.AttackValue;
			
			if(leadSkillReuduce != null) {
				reduceValue = leadSkillReuduce.ReduceHurtValue(reduceValue, attackType);
			}
			
			//			Debug.LogError("leadSkillReuduce is null : " + (leadSkillReuduce == null) + " reduceValue : " + reduceValue);
			
			int hurtValue = upi.CaculateInjured (attackType, reduceValue);
			
			//			Debug.LogError("hurtValue : " + hurtValue);
			
			BattleAttackManager.Instance.Hurt(hurtValue);
			te.ResetAttakAround ();	
//			MsgCenter.Instance.Invoke (CommandEnum.EnemyRefresh, te);
			ModuleManager.SendMessage(ModuleEnum.BattleEnemyModule,"refresh_enemy",te);
			//			Debug.LogError("EnemyAttack attackType : " + attackType);
			List<AttackInfo> temp = Dispose(attackType, hurtValue);
			
			for (int i = 0; i < temp.Count; i++) {
				temp[i].EnemyID = te.EnemySymbol;
				antiInfo.Add(temp[i]);
			}
			antiInfo.AddRange(temp);
			// 824920334   1575297093
			//			Debug.LogError("passiveSkill : " + passiveSkill + " temp : " + temp.Count + " antiInfo: " + antiInfo.Count);
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
				MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
				GameTimer.GetInstance ().AddCountDown (0.5f, EnemyAttackEnd);
				return;
			}
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [3]); // stateInfo [3]="PassiveSkill"
			GameTimer.GetInstance ().AddCountDown (0.3f, LoopAntiAttack);
		}
		else{
			EnemyAttackEnd();
			//			bud.battleQuest.battle.ShieldInput(true);	
			ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"banclick",true);
			MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
		}
	}
	
	void Fail () {
		battleFail = true;
		BattleEnd();
		return;
	}
	
	void EnemyAttackEnd () {
		BattleBottomView.notClick = false;
		CheckBattleSuccess ();
		ClearData();
		//		bud.battleQuest.battle.ShieldInput(true);	
		ModuleManager.SendMessage(ModuleEnum.BattleManipulationModule,"enemy_attack_end");
		BattleConfigData.Instance.storeBattleData.attackRound ++;
		BattleConfigData.Instance.storeBattleData.tEnemyInfo = enemyInfo;
		MsgCenter.Instance.Invoke (CommandEnum.EnemyAttackEnd, null);
		BattleConfigData.Instance.StoreMapData ();
	}
	
	void LoopAntiAttack() {
		float intervTime = 0.5f;
		GameTimer.GetInstance ().AddCountDown (intervTime, ()=>{
			if (antiInfo.Count == 0) {
				EnemyAttackEnd();
				MsgCenter.Instance.Invoke (CommandEnum.StateInfo, DGTools.stateInfo [0]);
				return;
			}
			
			AttackInfo ai = antiInfo [0];
			antiInfo.RemoveAt (0);
			TEnemyInfo te = enemyInfo.Find(a=>a.EnemySymbol == ai.EnemyID);
			//		Debug.LogError ("AntiAttack te : " + te);
			if (te == default(TEnemyInfo)) {
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
	private ILeaderSkill leaderSkill;
	
	private ActiveSkill iase;
	private TUserUnit userUnit;
	
	private const float fixEffectTime = 2f;
	
	public static float singleEffectTime = 2f;
	
	/// <summary>
	/// novice guide active skill cooling done.
	/// </summary>
	public static void CoolingDoneLeaderActiveSkill() {
		TUserUnit tuu = DataCenter.Instance.PartyInfo.CurrentParty.UserUnit [0];
		ActiveSkill sbi = DataCenter.Instance.GetSkill (tuu.MakeUserUnitKey (), tuu.UnitInfo.ActiveSkill, SkillType.ActiveSkill) as ActiveSkill;
		sbi.skillBase.skillCooling = 0;
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
	void Excute(object data) {
		userUnit = data as TUserUnit;
		if (userUnit != null) {
			string id = userUnit.MakeUserUnitKey();
			if(activeSkill.TryGetValue(id, out iase)) {
				//				Debug.LogError("activeSkill.TryGetValue true  : " + iase);
				
				MsgCenter.Instance.Invoke(CommandEnum.StateInfo, DGTools.stateInfo[4]);
				
				AudioManager.Instance.PlayAudio(AudioEnum.sound_active_skill);
				
				ai = AttackInfo.GetInstance();
				ai.UserUnitID = userUnit.MakeUserUnitKey();
				ai.SkillID = (iase as ActiveSkill).skillBase.id;
				//				MsgCenter.Instance.Invoke(CommandEnum.ShowActiveSkill, ai);
				ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"activeskill", ai);
				
				
				GameTimer.GetInstance().AddCountDown(BattleAttackEffectView.activeSkillEffectTime, WaitActiveEffect);
			} else {
				//				Debug.LogError("activeSkill.TryGetValue false  : ");
			}
		}
	}
	
	void WaitActiveEffect() {
		//		Debug.LogError("WaitActiveEffect ");
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, true);
		GameTimer.GetInstance().AddCountDown(1f,Excute);
		//		MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillStandReady, userUnit);
		ModuleManager.SendMessage (ModuleEnum.BattleFullScreenTipsModule, "ready",userUnit);
		AudioManager.Instance.PlayAudio (AudioEnum.sound_active_skill);
		
		AudioManager.Instance.PlayAudio (AudioEnum.sound_as_appear);
	}
	
	void Excute() {
		//		Debug.LogError ("Excute active skill iase: " + iase + " userUnit : " + userUnit);
		if (iase == null || userUnit == null) {
			return;	
		}
		
		iase = activeSkill[ai.UserUnitID];
		iase.Excute(ai.UserUnitID, userUnit.Attack);
		iase = null;
		userUnit = null;
		GameTimer.GetInstance ().AddCountDown (fixEffectTime + singleEffectTime, ActiveSkillEnd);
	}
	
	void ActiveSkillEnd() {
		//		Debug.LogError ("ActiveSkillEnd");
		MsgCenter.Instance.Invoke(CommandEnum.ExcuteActiveSkill, false);
	}
	
	void ReduceActiveSkillRound(object data) {
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
	private Dictionary<string,SkillBaseInfo> passiveSkills = new Dictionary<string,SkillBaseInfo> ();
	private Dictionary<string,float> multipe = new Dictionary<string, float> ();
	
//	public ExcutePassiveSkill(ILeaderSkill ls) {
//
//	}
	
	public void RemoveListener () {
		battleFail = false;
		RemoveEvent ();
		MsgCenter.Instance.RemoveListener (CommandEnum.MeetTrap, DisposeTrapEvent);
		MsgCenter.Instance.RemoveListener (CommandEnum.LaunchActiveSkill, Excute);
		MsgCenter.Instance.RemoveListener (CommandEnum.ReduceActiveSkillRound, ReduceActiveSkillRound);
	}
	
	void InitPassiveSkill() {
		foreach (var item in leaderSkill.UserUnit.Values) {
			if (item==null) {
				continue;
			}
			
			int id = item.PassiveSkill;
			
			if(id == -1) {
				continue;
			}
			
			SkillBaseInfo ipe = DataCenter.Instance.GetSkill(item.MakeUserUnitKey(), id, SkillType.PassiveSkill); //Skill[id] as IPassiveExcute;
			
			if(ipe == null) {
				continue;
			}
			
			string name = item.MakeUserUnitKey();
			passiveSkills.Add(name,ipe);
			multipe.Add(name,item.AttackMultiple);
		}
	}
	
	Queue <TrapBase> trap = new Queue<TrapBase>();
	void DisposeTrapEvent(object data) {
		TrapBase tb = data as TrapBase;
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
					foreach (var unitItem in leaderSkill.UserUnit.Values) {
						if(unitItem == null) {
							continue;
						}
						
						if(unitItem.MakeUserUnitKey() == item.Key) {
							AttackInfo ai = AttackInfo.GetInstance();
							ai.UserUnitID = unitItem.MakeUserUnitKey();
							ai.SkillID = item.Value.skillBase.id;
							//							MsgCenter.Instance.Invoke(CommandEnum.ShowPassiveSkill, ai);
							ModuleManager.SendMessage (ModuleEnum.BattleAttackEffectModule,"refreshitem", ai);
							
						}
					}
//				}
			}
		}
	}
	
	private List<AttackInfo> attackList = new List<AttackInfo> ();
	
	public List<AttackInfo> Dispose (int AttackType, int attack) {
		attackList.Clear ();
		foreach (var item in passiveSkills) {
			if(item.Value is TSkillAntiAttack) {
//				AttackInfo ai = item.Value.Excute(AttackType, this) as AttackInfo;
				if(ai != null) {
					ai.SkillID = item.Value.skillBase.id;
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
}


