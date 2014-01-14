using UnityEngine;
using System.Collections;

public class BattleUseData {
	ErrorMsg errorMsg;
	UnitPartyInfo upi;
	int blood = 0;
	int maxEnergyPoint = 0;

	public BattleUseData () {
		errorMsg = new ErrorMsg ();
		UnitPartyInfo upi = ModelManager.Instance.GetData (ModelEnum.UnitPartyInfo,errorMsg) as UnitPartyInfo;
		blood = upi.GetBlood ();
		maxEnergyPoint = GlobalData.maxEnergyPoint;
		ListenEvent ();
	}

	~BattleUseData() {
		RemoveListen ();
	}

	public void GetBaseData(object data) {
	
		BattleBaseData bud = new BattleBaseData ();
		bud.Blood = blood;
		bud.EnergyPoint = maxEnergyPoint;
		MsgCenter.Instance.Invoke (CommandEnum.BattleBaseData, bud);
	}

	void ListenEvent () {
		MsgCenter.Instance.AddListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.AddListener (CommandEnum.MoveToMapItem, MoveToMapItem);
	}

	void RemoveListen () {
		MsgCenter.Instance.RemoveListener (CommandEnum.InquiryBattleBaseData, GetBaseData);
		MsgCenter.Instance.RemoveListener (CommandEnum.MoveToMapItem, MoveToMapItem);
	}

	void RefreshBlood() {
		MsgCenter.Instance.Invoke (CommandEnum.UnitBlood, blood);
	}

	bool temp = true;
	void MoveToMapItem (object coordinate) {
		if (temp) {
			temp = false;
			return;
		}

		if (maxEnergyPoint == 0) {
			blood -= ReductionBloodByProportion(0.2f);
			RefreshBlood();
		} 
		else {
			maxEnergyPoint--;
			MsgCenter.Instance.Invoke(CommandEnum.EnergyPoint,maxEnergyPoint);
		}
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
