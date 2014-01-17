using UnityEngine;
using System.Collections.Generic;

public class BattleUseData {
	private ErrorMsg errorMsg;
	private UnitPartyInfo upi;
	private int blood = 0;
	private int maxEnergyPoint = 0;
	private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();
	private List<TempEnemy> currentEnemy =new List<TempEnemy> ();
	private List<ShowEnemyUtility> showEnemy = new List<ShowEnemyUtility>();

	public BattleUseData () {
		errorMsg = new ErrorMsg ();
		upi = ModelManager.Instance.GetData (ModelEnum.UnitPartyInfo,errorMsg) as UnitPartyInfo;
		upi.GetSkillCollection ();
		blood = upi.GetBlood ();
		maxEnergyPoint = GlobalData.maxEnergyPoint;
		ListenEvent ();
	}

	~BattleUseData() {
		RemoveListen ();
	}

	void ListenEvent () {
		MsgCenter.Instance.AddListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.AddListener (CommandEnum.StartAttack, StartAttack);
		//MsgCenter.Instance.AddListener (CommandEnum.DragCardToBattleArea, CaculateFight);
	}

	void RemoveListen () {
		MsgCenter.Instance.RemoveListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.StartAttack, StartAttack);
		//MsgCenter.Instance.RemoveListener (CommandEnum.DragCardToBattleArea, CaculateFight);
	}

	void CalculateInjury () {

	}

	List<AttackInfo> SortAttackSequence() {
		List<AttackInfo> sortAttack = new List<AttackInfo> ();
		foreach (var item in attackInfo.Values) {
			sortAttack.AddRange(item);
//			foreach (var item1 in item) {
//				sortAttack.Add(item1);
//
//			}
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

		for (int i = 0; i < sortAttack.Count; i++) {
			sortAttack[i].originIndex = i;
			Debug.LogError("-----------befour---------------    index : " + i + " need card : " + sortAttack[i].NeedCardNumber + " UserPos : " + sortAttack[i].UserPos );
		}

		DGTools.InsertSortBySequence(sortAttack, new AISortByCardNumber());
		//DGTools.InsertSort(sortAttack, new AISortByUserpos(),false);
		for (int i = 0; i < sortAttack.Count; i++) {
			Debug.LogError("-----------end---------------    index : " + i + " need card : " + sortAttack[i].NeedCardNumber + " UserPos : " + sortAttack[i].UserPos+ "  index : "+sortAttack[i].originIndex );
		}

		return sortAttack;
	}

	public List<ShowEnemyUtility> GetEnemyInfo (List<int> monster) {
		currentEnemy.Clear ();

		int j = showEnemy.Count - monster.Count;
		for (int i = 0; i < j; i++) {
			showEnemy.RemoveAt(monster.Count + i);
		}

		for (int i = 0; i < monster.Count; i++) {
			TempEnemy te = GlobalData.tempEnemyInfo[monster[i]];
			if(i == showEnemy.Count) {
				ShowEnemyUtility seu = new ShowEnemyUtility();
				showEnemy.Add(seu);
			} 
			showEnemy[i].enemyID = te.GetID();
			showEnemy[i].enemyBlood = te.GetBlood();
			showEnemy[i].attackRound = te.GetRound();
			currentEnemy.Add(te);
		}

		return showEnemy;
	}

	public List<AttackImageUtility> CaculateFight (int areaItem, int id) {
		return upi.CalculateSkill (areaItem, id);
	}

	public void StartAttack(object data) {
		attackInfo = upi.Attack;
		SortAttackSequence ();
	}

	public void ClearData () {
		upi.ClearData ();
	}

	public void GetBaseData(object data) {
		BattleBaseData bud = new BattleBaseData ();
		bud.Blood = blood;
		bud.EnergyPoint = maxEnergyPoint;
		MsgCenter.Instance.Invoke (CommandEnum.BattleBaseData, bud);
	}

	bool temp = true;
	void MoveToMapItem (object coordinate) {
		if (temp) {
			temp = false;
			return;
		}
		
		if (maxEnergyPoint == 0) {
			blood -= ReductionBloodByProportion(0.2f);
			if(blood < 1) {
				blood = 1;
			}
			RefreshBlood();
		} 
		else {
			maxEnergyPoint--;
			MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint,maxEnergyPoint);
		}
	}

	void RefreshBlood() {
		MsgCenter.Instance.Invoke (CommandEnum.UnitBlood, blood);
	}
			
	int ReductionBloodByProportion(float proportion) {
		int maxBlood = upi.GetBlood ();
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


