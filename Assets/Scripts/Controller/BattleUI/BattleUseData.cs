using UnityEngine;
using System.Collections.Generic;

public class BattleUseData {
    private ErrorMsg errorMsg;
    private TUnitParty upi;
    private int maxBlood = 0;
    private int blood = 0;
    public int Blood {
		set { 
			blood = value; 
			if(blood < 1) {
				MsgCenter.Instance.Invoke(CommandEnum.PlayerDead, null);
			} 
		}
        get { return blood; }
    }
    private int recoverHP = 0;
    private int maxEnergyPoint = 0;
    private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();
    private List<TEnemyInfo> currentEnemy = new List<TEnemyInfo>();
    private List<TEnemyInfo> showEnemy = new List<TEnemyInfo>();
    private AttackController ac;
    private ExcuteLeadSkill els;
    private ExcuteActiveSkill eas;
    private ExcutePassiveSkill eps;
    private ILeaderSkillRecoverHP skillRecoverHP;

    private static Coordinate currentCoor;
    public static Coordinate CurrentCoor {
        get { return currentCoor; }
    }

    private static float countDown = 5f;
    public static float CountDown {
        get { return countDown; }
    }

    public BattleUseData() {
        ListenEvent();
        errorMsg = new ErrorMsg();
        upi = DataCenter.Instance.PartyInfo.CurrentParty; 
        upi.GetSkillCollection();
        els = new ExcuteLeadSkill(upi);
        skillRecoverHP = els;
        els.Excute();
        eas = new ExcuteActiveSkill(upi);
        eps = new ExcutePassiveSkill(upi);
        ac = new AttackController(this, eps);
        maxBlood = Blood = upi.GetInitBlood();
        maxEnergyPoint = DataCenter.maxEnergyPoint;
        Config.Instance.SwitchCard(els);	
    }

    ~BattleUseData() {
    }

    void ListenEvent() {
        MsgCenter.Instance.AddListener(CommandEnum.InquiryBattleBaseData, GetBaseData);
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
        MsgCenter.Instance.RemoveListener(CommandEnum.InquiryBattleBaseData, GetBaseData);
        MsgCenter.Instance.RemoveListener(CommandEnum.MoveToMapItem, MoveToMapItem);
        MsgCenter.Instance.RemoveListener(CommandEnum.StartAttack, StartAttack);
        MsgCenter.Instance.RemoveListener(CommandEnum.RecoverHP, RecoverHP);
        MsgCenter.Instance.RemoveListener(CommandEnum.LeaderSkillDelayTime, DelayCountDownTime);
        MsgCenter.Instance.RemoveListener(CommandEnum.ActiveSkillRecoverHP, RecoveHPByActiveSkill);
        MsgCenter.Instance.RemoveListener(CommandEnum.SkillSucide, Sucide);
        MsgCenter.Instance.RemoveListener(CommandEnum.SkillRecoverSP, RecoverEnergePoint);
        MsgCenter.Instance.RemoveListener(CommandEnum.TrapMove, TrapMove);
        MsgCenter.Instance.RemoveListener(CommandEnum.TrapInjuredDead, TrapInjuredDead);
        MsgCenter.Instance.RemoveListener(CommandEnum.InjuredNotDead, InjuredNotDead);
        MsgCenter.Instance.RemoveListener(CommandEnum.TrapTargetPoint, TrapTargetPoint);

        countDown = 5f;
        eas.RemoveListener();
        eps.RemoveListener();
        ac.RemoveListener();

        els = null;
        eas = null;
        eps = null;
        ac = null;
    }

    void TrapMove(object data) {
        if (data == null) {
            ConsumeEnergyPoint();
        }
    }

    void TrapInjuredDead(object data) {
        float value = (float)data;
        int hurtValue = System.Convert.ToInt32(value);
        Blood -= hurtValue;
        RefreshBlood();
    }

    void InjuredNotDead(object data) {
        float probability = (float)data;
        float residualBlood = blood - maxBlood * probability;
		if (residualBlood < 1) {
			residualBlood = 1;	
        }
        Blood = System.Convert.ToInt32(residualBlood);
        RefreshBlood();
    }

    void RecoveHPByActiveSkill(object data) {
        int value = (int)data;
        int add = 0;
        if (value < 1) {
            add = blood * value + blood;
        }
        else {
            add = value + blood;
        }
        RecoverHP(add);
    }

    void DelayCountDownTime(object data) {
        float addTime = (float)data;
        countDown += addTime;
    }


    void Sucide(object data) {
        blood = 1;
        RefreshBlood();
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
        DGTools.InsertSortBySequence(sortAttack, new AISortByCardNumber());
        for (int i = 0; i < sortAttack.Count; i++) {
            sortAttack[i].AttackValue *= (1 + i * 0.25f);
            sortAttack[i].ContinuAttackMultip += i;
        }
        return sortAttack;
    }

    void RecoverHP(object data) {
        AttackInfo ai = data as AttackInfo;
        float tempBlood = ai.AttackValue;
        int addBlood = System.Convert.ToInt32(tempBlood) + blood;
        RecoverHP(addBlood);
    }

    public void RecoverHP(int recoverBlood) {
        AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
        if (blood < recoverBlood) {
            blood = recoverBlood > maxBlood ? maxBlood : recoverBlood;
            RefreshBlood();
        }
    }

//	public List<TEnemyInfo> GetEnemyInfo (List<uint> monster) {
//		currentEnemy.Clear ();
//		int j = showEnemy.Count - monster.Count;
//
//		for (int i = j - 1; i > -1; i--) {
////			Debug.LogError (showEnemy.Count + "  monster.Count : " + monster.Count + " monster.Count + i : " + (monster.Count + i));
//			showEnemy.RemoveAt(monster.Count + i);
//		}
//		for (int i = 0; i < monster.Count; i++) {
//			TEnemyInfo te = DataCenter.Instance.EnemyInfo[monster[i]];
//			te.Reset();
//			if(i == showEnemy.Count) {
//				showEnemy.Add(te);
//			} 
//			showEnemy[i] = te;
//			currentEnemy.Add(te);
//		}
//		ac.enemyInfo = currentEnemy;
//		return showEnemy;
//	}

//	public void InitEnemyInfo(List<TEnemyInfo> enemyInfo) {
//		ac.enemyInfo = enemyInfo;
//	}

    public void InitEnemyInfo(TQuestGrid grid) {
        ac.Grid = grid;
    }

    public void InitBoss(List<TEnemyInfo> boss) {
        ac.enemyInfo = boss;
    }

    public List<AttackImageUtility> CaculateFight(int areaItem, int id) {
        return upi.CalculateSkill(areaItem, id, blood);
    }

    public void StartAttack(object data) {
        attackInfo = upi.Attack;
//		Debug.LogError ("StartAttack  " + attackInfo.Count);
        List<AttackInfo> temp = SortAttackSequence();
//		Debug.LogError ("StartAttack temp : " + temp.Count);
        ac.LeadSkillReduceHurt(els);
        ac.StartAttack(temp, upi);
    }

    public void ClearData() {
        upi.ClearData();
        attackInfo.Clear();
    }

    public void GetBaseData(object data) {
        BattleBaseData bud = new BattleBaseData();
        bud.Blood = blood;
        bud.EnergyPoint = maxEnergyPoint;
        MsgCenter.Instance.Invoke(CommandEnum.BattleBaseData, bud);
    }

    void RecoverEnergePoint(object data) {
        int recover = (int)data;
        maxEnergyPoint += recover;
        if (maxEnergyPoint > DataCenter.maxEnergyPoint) {
            maxEnergyPoint = DataCenter.maxEnergyPoint;	
        }
        MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
    }
    void TrapTargetPoint(object coordinate) {
        currentCoor = (Coordinate)coordinate;
        ConsumeEnergyPoint();
    }

    bool temp = true;
    void MoveToMapItem(object coordinate) {
        currentCoor = (Coordinate)coordinate;
        if (temp) {
            temp = false;
            return;
        }
        MsgCenter.Instance.Invoke(CommandEnum.ActiveSkillCooling, null);	// refresh active skill cooling.
        int addBlood = skillRecoverHP.RecoverHP(blood, 2);	//3: every step.
        RecoverHP(addBlood);
        ConsumeEnergyPoint();
    }

    void ConsumeEnergyPoint() {
        if (maxEnergyPoint == 0) {
            Blood -= ReductionBloodByProportion(0.2f);
            if (blood < 1) {
                blood = 1;
            }
            RefreshBlood();
        }
        else {
            maxEnergyPoint--;
            MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
        }
    }

    public void Hurt(int hurtValue) {
		Blood -= hurtValue;
        RefreshBlood();
    }

    public void RefreshBlood() {
        MsgCenter.Instance.Invoke(CommandEnum.UnitBlood, blood);
    }
			
    int ReductionBloodByProportion(float proportion) {
        return (int)(maxBlood * proportion);
    }
}

public class BattleBaseData {
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


