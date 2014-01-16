using UnityEngine;
using System.Collections.Generic;

public class BattleUseData {
	private ErrorMsg errorMsg;
	private UnitPartyInfo upi;
	private int blood = 0;
	private int maxEnergyPoint = 0;
	private Dictionary<int,List<AttackInfo>> attackInfo = new Dictionary<int, List<AttackInfo>>();


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
		//MsgCenter.Instance.AddListener (CommandEnum.DragCardToBattleArea, CaculateFight);
	}

	void RemoveListen () {
		MsgCenter.Instance.RemoveListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
		//MsgCenter.Instance.RemoveListener (CommandEnum.DragCardToBattleArea, CaculateFight);
	}

	public List<AttackImageUtility> CaculateFight (int areaItem, int id) {
		return upi.CalculateSkill (areaItem, id);
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


