using UnityEngine;
using System.Collections.Generic;

public class BattleUseData {
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
			ConfigBattleUseData.Instance.storeBattleData.hp = blood;
		}
        get { return blood; }
    }
    private int recoverHP = 0;
    public static int maxEnergyPoint = 0;
    private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();
    private List<TEnemyInfo> currentEnemy = new List<TEnemyInfo>();
    private List<TEnemyInfo> showEnemy = new List<TEnemyInfo>();
    public AttackController ac;
    private ExcuteLeadSkill els;
	public ExcuteLeadSkill Els{
		get {return els;}
	}
    private ExcuteActiveSkill eas;
	public ExcuteActiveSkill excuteActiveSkill {
		get { return eas; }
	}
    private ExcutePassiveSkill eps;
	public ExcutePassiveSkill excutePassiveSkill {
		get { return eps; }
	}
    private ILeaderSkillRecoverHP skillRecoverHP;

    private static Coordinate currentCoor;
    public static Coordinate CurrentCoor {
        get { return currentCoor; }
    }

    private static float countDown = 5f;
    public static float CountDown {
        get { return countDown; }
    }

	private static BattleUseData instance;

	public static BattleUseData Instance{
		get{
			if(instance == null)
				instance = new BattleUseData();
			return instance;
		}
	}

//	private ConfigBattleUseData configBattleUseData;

    private BattleUseData() {
//		battleQuest = bq;
//		configBattleUseData = ConfigBattleUseData.Instance;
		Reset ();
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
	public void InitBattleUseData (TStoreBattleData sbd) {
		if (isInit) {
			return;
		}
		isInit = true;
		els.Excute();
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

		ConfigBattleUseData.Instance.storeBattleData.hp = blood;
		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
		GetBaseData ();
		eas = new ExcuteActiveSkill(upi);
		eps = new ExcutePassiveSkill(upi);
		ac = new AttackController(eps, upi);
		Config.Instance.SwitchCard(els);
		ConfigBattleUseData.Instance.StoreMapData ();
	}

	public void CheckPlayerDead() {
		if(blood <= 0) {
			PlayerDead();
		}
	}

    ~BattleUseData() { }

    void ListenEvent() {
//        MsgCenter.Instance.AddListener(CommandEnum.InquiryBattleBaseData, GetBaseData);
        MsgCenter.Instance.AddListener(CommandEnum.MoveToMapItem, MoveToMapItem);
        MsgCenter.Instance.AddListener(CommandEnum.StartAttack, StartAttack);
        MsgCenter.Instance.AddListener(CommandEnum.RecoverHP, RecoverHP);
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
		ConfigBattleUseData.Instance.StoreMapData ();
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

    void RecoverHP(object data) {
        AttackInfo ai = data as AttackInfo;
//		Blood += System.Convert.ToInt32 (ai.AttackValue);
		AddBlood (System.Convert.ToInt32 (ai.AttackValue));
    }

    public void InitEnemyInfo(TQuestGrid grid) {
		if (ac == null || grid == null) {
//			Debug.LogError(" ac : " + ac + " grid : " + grid);
			return;
		}

        ac.Grid = grid;
    }

    public void InitBoss(List<TEnemyInfo> boss, TDropUnit bossDrop) {
		ac.enemyInfo.Clear ();
		ac.enemyInfo.AddRange (boss);
		ac.dropUnit = bossDrop;
    }

    public List<AttackInfo> CaculateFight(int areaItem, int id, bool isBoost) {
		float value = isBoost ? 1.5f : 1f;
		return upi.CalculateSkill(areaItem, id, maxBlood, value);
    }

    public void StartAttack(object data) {
        attackInfo = upi.Attack;
        List<AttackInfo> temp = SortAttackSequence();
        ac.LeadSkillReduceHurt(els);
        ac.StartAttack(temp);
    }

    public void ClearData() {
        upi.ClearData();
        attackInfo.Clear();
    }

    public void GetBaseData() {
        BattleBaseData bbd = new BattleBaseData();
		bbd.Blood = Blood;
		bbd.maxBlood = maxBlood;
		bbd.EnergyPoint = maxEnergyPoint;
//		MsgCenter.Instance.Invoke(CommandEnum.BattleBaseData, bbd);
		ModuleManager.SendMessage (ModuleEnum.BattleBottomModule, "initdata", bbd);
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
		ConfigBattleUseData.Instance.storeBattleData.sp = maxEnergyPoint;
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

		ConfigBattleUseData.Instance.storeBattleData.hp = blood;
		MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
	}

	public void AddBlood (int value) {
		if (value == 0) {
			return;	
		}

		int addBlood = blood + value;
		blood = addBlood > maxBlood ? maxBlood : addBlood;
		ConfigBattleUseData.Instance.storeBattleData.hp = blood;
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
			ConfigBattleUseData.Instance.storeBattleData.sp = maxEnergyPoint;
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
}

public class BattleBaseData {
	public int maxBlood;

    private int blood;

    public int Blood {
        get {
            return blood;
        }
        set {
            blood = value;
        }
    }

    private int energyPoint;

    public int EnergyPoint {
        get {
            return energyPoint;
        }
        set {
            energyPoint = value;
        }
    }
}

