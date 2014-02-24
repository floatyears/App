using UnityEngine;
using System.Collections.Generic;

public class BattleUseData {
	private ErrorMsg errorMsg;
	private UnitPartyInfo upi;
	private int maxBlood = 0;
	private int blood = 0;
	public int Blood {
		get {
			return blood;
		}
	}
	private int recoverHP = 0;
	private int maxEnergyPoint = 0;
	private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();
	private List<TempEnemy> currentEnemy =new List<TempEnemy> ();
	private List<ShowEnemyUtility> showEnemy = new List<ShowEnemyUtility>();
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
	public static float CountDown{
		get { return countDown; }
	}

	public BattleUseData () {
		ListenEvent ();
		errorMsg = new ErrorMsg ();
		upi = ModelManager.Instance.GetData (ModelEnum.UnitPartyInfo,errorMsg) as UnitPartyInfo;
		upi.GetSkillCollection ();
		els = new ExcuteLeadSkill (upi);
		skillRecoverHP = els;
		els.Excute ();
		eas = new ExcuteActiveSkill (upi);
		eps = new ExcutePassiveSkill (upi);
		ac = new AttackController (this, eps);
		maxBlood = blood = upi.GetInitBlood ();
		maxEnergyPoint = GlobalData.maxEnergyPoint;
		Config.Instance.SwitchCard (els);	
	}

	~BattleUseData() {

		//RemoveListen ();
	}

	void ListenEvent () {
		MsgCenter.Instance.AddListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.AddListener (CommandEnum.StartAttack, StartAttack);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);
		MsgCenter.Instance.AddListener (CommandEnum.LeaderSkillDelayTime, DelayCountDownTime);
		MsgCenter.Instance.AddListener (CommandEnum.ActiveSkillRecoverHP, RecoveHPByActiveSkill);
		MsgCenter.Instance.AddListener (CommandEnum.SkillSucide, Sucide);
		MsgCenter.Instance.AddListener (CommandEnum.SkillRecoverSP, RecoverEnergePoint);
		MsgCenter.Instance.AddListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.AddListener (CommandEnum.TrapInjuredDead, TrapInjuredDead);
		MsgCenter.Instance.AddListener (CommandEnum.InjuredNotDead, InjuredNotDead);
		MsgCenter.Instance.AddListener (CommandEnum.TrapTargetPoint, TrapTargetPoint);
	}

	public void RemoveListen () {
		MsgCenter.Instance.RemoveListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.StartAttack, StartAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);
		MsgCenter.Instance.RemoveListener (CommandEnum.LeaderSkillDelayTime, DelayCountDownTime);
		MsgCenter.Instance.RemoveListener (CommandEnum.ActiveSkillRecoverHP, RecoveHPByActiveSkill);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillSucide, Sucide);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillRecoverSP, RecoverEnergePoint);
		MsgCenter.Instance.RemoveListener (CommandEnum.TrapMove, TrapMove);
		MsgCenter.Instance.RemoveListener (CommandEnum.TrapInjuredDead, TrapInjuredDead);
		MsgCenter.Instance.RemoveListener (CommandEnum.InjuredNotDead, InjuredNotDead);
		MsgCenter.Instance.RemoveListener (CommandEnum.TrapTargetPoint, TrapTargetPoint);

		countDown = 5f;
		eas.RemoveListener ();
		eps.RemoveListener ();
		ac.RemoveListener ();

		els = null;
		eas = null;
		eps = null;
		ac = null;
	}

	void TrapMove(object data) {
		if (data == null) {
			ConsumeEnergyPoint ();
		}
	}

	void TrapInjuredDead(object data) {
		float value = (float)data;
		int hurtValue = System.Convert.ToInt32 (value);
		blood -= hurtValue;
		RefreshBlood ();
	}

	void InjuredNotDead(object data) {
		float probability = (float)data;
		float residualBlood = blood - maxBlood * probability;
		if (blood < 1) {
			blood = 1;	
		}
		blood = System.Convert.ToInt32 (residualBlood);
		RefreshBlood ();
	}

	void RecoveHPByActiveSkill (object data) {
		int value = (int)data;
		int add = 0;
		if (value < 1) {
			add = blood * value + blood;
		} 
		else {
			add = value + blood;
		}
		RecoverHP (add);
	}

	void DelayCountDownTime(object data) {
		float addTime = (float)data;

		countDown += addTime;
		Debug.LogError ("addTime : " + addTime + " countDown : " + countDown);
	}


	void Sucide(object data) {
		blood = 1;
		RefreshBlood ();
	}

	List<AttackInfo> SortAttackSequence() {
		List<AttackInfo> sortAttack = new List<AttackInfo> ();
		foreach (var item in attackInfo.Values) {
			sortAttack.AddRange(item);
		}
		attackInfo.Clear ();
		int tempCount = 0;
		for (int i = GlobalData.posStart; i < GlobalData.posEnd; i++) {
			List<AttackInfo> temp = sortAttack.FindAll(a => a.UserPos == i);
			if(temp == null) {
				temp = new List<AttackInfo>();
			}
			if(temp.Count > tempCount) {
				tempCount = temp.Count;
			}
			DGTools.InsertSort(temp, new AISortByCardNumber());
			attackInfo.Add(i,temp);
		}
		sortAttack.Clear ();
		for (int i = 0; i < tempCount; i++) {
			for (int j = GlobalData.posStart; j < GlobalData.posEnd; j++) {
				if(attackInfo[j].Count > 0) {
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

	void RecoverHP (object data) {
		AttackInfo ai = data as AttackInfo;
		float tempBlood = ai.AttackValue;
		int addBlood = System.Convert.ToInt32(tempBlood) + blood;
		RecoverHP (addBlood);
	}

	public void RecoverHP (int recoverBlood) {
		AudioManager.Instance.PlayAudio (AudioEnum.sound_hp_recover);
		if (blood < recoverBlood) {
			blood = recoverBlood > maxBlood ? maxBlood : recoverBlood;
			RefreshBlood ();
		}
	}

	public List<ShowEnemyUtility> GetEnemyInfo (List<uint> monster) {
		currentEnemy.Clear ();
		int j = showEnemy.Count - monster.Count;
		for (int i = 0; i < j; i++) {
			showEnemy.RemoveAt(monster.Count + i);
		}
		for (int i = 0; i < monster.Count; i++) {
			TempEnemy te = GlobalData.tempEnemyInfo[monster[i]];
			te.Reset();
			if(i == showEnemy.Count) {
				ShowEnemyUtility seu = new ShowEnemyUtility();
				showEnemy.Add(seu);
			} 
			showEnemy[i].enemyID = te.GetID();
			showEnemy[i].enemyBlood = te.GetBlood();
			showEnemy[i].attackRound = te.GetRound();
			currentEnemy.Add(te);
		}
		ac.enemyInfo = currentEnemy;
		return showEnemy;
	}

	public List<AttackImageUtility> CaculateFight (int areaItem, int id) {
		return upi.CalculateSkill (areaItem, id, blood);
	}

	public void StartAttack(object data) {
		attackInfo = upi.Attack;
		List<AttackInfo> temp = SortAttackSequence ();
		ac.LeadSkillReduceHurt (els);
		ac.StartAttack (temp,upi);
	}

	public void ClearData () {
		upi.ClearData ();
		attackInfo.Clear ();
	}

	public void GetBaseData(object data) {
		BattleBaseData bud = new BattleBaseData ();
		bud.Blood = blood;
		bud.EnergyPoint = maxEnergyPoint;
		MsgCenter.Instance.Invoke (CommandEnum.BattleBaseData, bud);
	}

	void RecoverEnergePoint(object data) {
		int recover = (int)data;
		maxEnergyPoint += recover;
		if (maxEnergyPoint > GlobalData.maxEnergyPoint) {
			maxEnergyPoint = GlobalData.maxEnergyPoint;	
		}
		MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint, maxEnergyPoint);
	}
	void TrapTargetPoint(object coordinate) {
		currentCoor = (Coordinate)coordinate;
		ConsumeEnergyPoint ();
	}

	bool temp = true;
	void MoveToMapItem (object coordinate) {
		currentCoor = (Coordinate)coordinate;
		if (temp) {
			temp = false;
			return;
		}
		MsgCenter.Instance.Invoke (CommandEnum.ActiveSkillCooling, null);	// refresh active skill cooling.
		int addBlood = skillRecoverHP.RecoverHP (blood, 2);	//3: every step.
		RecoverHP (addBlood);
		ConsumeEnergyPoint ();
	}

	void ConsumeEnergyPoint () {
		if (maxEnergyPoint == 0) {
			blood -= ReductionBloodByProportion(0.2f);
			if(blood < 1) {
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
		blood -= hurtValue;
		RefreshBlood ();
	}

	public void RefreshBlood() {
		MsgCenter.Instance.Invoke (CommandEnum.UnitBlood, blood);
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


