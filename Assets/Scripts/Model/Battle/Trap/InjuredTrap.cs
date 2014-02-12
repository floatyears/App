using UnityEngine;
using System.Collections;
using bbproto;

public class InjuredTrap : TrapBase, ITrapExcute {
	public InjuredTrap (object instance) : base (instance) {
		switch (GetTrap.effectType) {
		case 1:
			trapEffectType = TrapInjuredInfo.mineInfo;
			break;
		case 2:
			trapEffectType = TrapInjuredInfo.trappingInfo;
			break;
		case 3:
			trapEffectType = TrapInjuredInfo.hungryInfo;
			break;
		case 4:
			trapEffectType = TrapInjuredInfo.lostMoney;
			break;
		}
	}

	private const float probability = 0.9f;
	private int[] randomRange = new int[2] {1, -1};

	public void Excute () {
		switch (GetTrap.effectType) {
		case 1:
			DisposeMine();
			break;
		case 2:
			DisposeTrapping ();
			break;
		case 3:
			DisposeHungry ();
			break;
		case 4:
			DisposeLostMoney ();
			break;
		}
	}

	void DisposeLostMoney () {
		MsgCenter.Instance.Invoke(CommandEnum.ConsumeCoin, GetInjuredValue.trapValue);
	}

	void DisposeHungry () {
		MsgCenter.Instance.Invoke(CommandEnum.ConsumeSP, GetInjuredValue.trapValue);
	}

	void DisposeTrapping () {
		MsgCenter.Instance.Invoke(CommandEnum.TrapInjuredDead, GetInjuredValue.trapValue);
	}
	
	void DisposeMine() {
		float value = DGTools.RandomToFloat();
		if(value <= probability) {
			MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, null);
		}
		else {
			Coordinate cd = CalculateCoordinate();
			MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, cd);
		}
		MsgCenter.Instance.Invoke(CommandEnum.TrapInjuredDead, GetInjuredValue.trapValue);
	}

	Coordinate CalculateCoordinate () {
		Coordinate cd = BattleUseData.CurrentCoor;
		int xRandom = DGTools.RandomToInt(0, randomRange.Length);
		int yRandom = DGTools.RandomToInt(0, randomRange.Length);
		MapConfig mc = ModelManager.Instance.GetData(ModelEnum.MapConfig, new ErrorMsg()) as MapConfig;
		if(xRandom > yRandom) {
			cd.x -= randomRange[xRandom];
			if(cd.x < 0) {
				cd.x = 1;
			}
			if(cd.x >= mc.mapXLength) {
				cd.x = mc.mapXLength - 2;
			}
		}
		else{
			cd.y -= randomRange[yRandom];
			if(cd.y < 0) {
				cd.y = 1;
			}
			if(cd.y >= mc.mapYLength) {
				cd.y = mc.mapYLength - 2;
			}
		}
		return cd;
	}
}


public class TrapBase : ProtobufDataBase {
	public TrapBase(object instance) : base (instance) {
		trapValueIndex = GetTrap.valueIndex;
	}
	protected TrapInfo GetTrap {
		get {
			return DeserializeData<TrapInfo> ();
		}
	}
	protected TrapInjuredValue GetInjuredValue {
		get {
			return TrapInjuredInfo.Instance.FindInfo(trapEffectType, trapValueIndex);
		}
	}
	protected int trapEffectType = -1;
	protected int trapValueIndex = -1;   

	public int GetTrapType () {
		return (int)GetTrap.trapType;
	}
	public int GetLevel() {
		return GetInjuredValue.trapLevel;
	}
}