using UnityEngine;
using System.Collections;
using bbproto;

public class InjuredTrap : TrapBase {

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
	public override void Excute () {
		TrapInfo ti = GetTrap;
		TrapInjuredValue tiv = null;
		switch (ti.effectType) {
		case 1:
//			ViewManager.Instance.TrapLabel.text = "injured trap: mine";
			DisposeMine(ti);
			break;
		case 2:
//			ViewManager.Instance.TrapLabel.text = "injured trap: trapping";
			DisposeTrapping (ti);
			break;
		case 3:
//			ViewManager.Instance.TrapLabel.text = "injured trap: Hungry";
			DisposeHungry (ti);
			break;
		case 4:
//			ViewManager.Instance.TrapLabel.text = "injured trap: LostMoney";
			DisposeLostMoney (ti);
			break;
		}
	}

	void DisposeLostMoney (TrapInfo ti) {
		MsgCenter.Instance.Invoke(CommandEnum.ConsumeCoin, GetInjuredValue.trapValue);
	}

	void DisposeHungry (TrapInfo ti) {
		MsgCenter.Instance.Invoke(CommandEnum.ConsumeSP, GetInjuredValue.trapValue);
	}

	void DisposeTrapping (TrapInfo ti) {
		MsgCenter.Instance.Invoke(CommandEnum.TrapInjuredDead, GetInjuredValue.trapValue);

		AudioManager.Instance.PlayAudio (AudioEnum.sound_walk_hurt);
	}
	
	void DisposeMine(TrapInfo ti) {
		float value = DGTools.RandomToFloat();
		MsgCenter.Instance.Invoke(CommandEnum.TrapInjuredDead, GetInjuredValue.trapValue);
		if(value <= probability) {
			MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, null);
		}
		else {
			Coordinate cd = BattleUseData.CurrentCoor;
			int xRandom = DGTools.RandomToInt(0, randomRange.Length);
			int yRandom = DGTools.RandomToInt(0, randomRange.Length);
//			MapConfig mc = ModelManager.Instance.GetData(ModelEnum.MapConfig, new ErrorMsg()) as MapConfig;
			if(xRandom > yRandom) {
				cd.x -= randomRange[xRandom];
				if(cd.x < 0) {
					cd.x = 0;
				}
				if(cd.x >= MapConfig.MapWidth) {
					cd.x = MapConfig.MapWidth - 1;
				}
			}
			else{
				cd.y -= randomRange[xRandom];
				if(cd.y < 0) {
					cd.y = 0;
				}
				if(cd.y >= MapConfig.MapHeight) {
					cd.y = MapConfig.MapHeight - 1;
				}
			}

			MsgCenter.Instance.Invoke(CommandEnum.NoSPMove, cd);
		}
		AudioManager.Instance.PlayAudio (AudioEnum.sound_walk_hurt);
	}
}

public class TrapBase : ProtobufDataBase ,ITrapExcute{

	public const string poisonTrapSpriteName = "PoisonTrap";
	public const string environmentSpriteName = "EnvirmentTrap";

	protected TrapInfo instance;
	public TrapBase(object instance) : base (instance) {
		this.instance = instance as TrapInfo;
		trapValueIndex = GetTrap.valueIndex;
	}
	public TrapInfo GetTrap {
		get {
			return instance;
		}
	}

	protected int Round {
		get { return instance.round; }
		set { instance.round = value; }
	}

	protected TrapInjuredValue GetInjuredValue {
		get {
			return TrapInjuredInfo.Instance.FindInfo(trapEffectType, trapValueIndex);
		}
	}
	protected int trapEffectType = -1;
	protected int trapValueIndex = -1;

	public ETrapType GetTrapType () {
		return GetTrap.trapType;
	}
	public int GetLevel {
		get{
			TrapInjuredValue tiv = GetInjuredValue;
			if(tiv == null) {
				return -1;
			}
			else{
				return tiv.trapLevel;
			}
		}
	}

	public string GetTrapSpriteName () {
		string spriteName = "";
		switch (instance.trapType) {
		case ETrapType.Move:
			spriteName = MoveTrapName(instance.effectType);
			break;
		case ETrapType.Injured:
			spriteName = InjuredTrapName(instance.effectType);
			break;
		case ETrapType.StateException:
			spriteName = poisonTrapSpriteName;
			break;
		case ETrapType.ChangeEnvir:
			spriteName = environmentSpriteName;
			break;
		default:
			break;
		}
		return spriteName;
	}

	public string GetItemName() {
		string itemName = "";
		switch (instance.trapType) {
		case ETrapType.Move:
			itemName = MoveItemTrapName(instance.effectType);
			break;
		case ETrapType.Injured:
			itemName = InjuredItemTrapName(instance.effectType);;
			break;
		case ETrapType.StateException:
			itemName = "Poison";
			break;
		case ETrapType.ChangeEnvir:
			itemName = "Shield";
			break;
		default:
			break;
		}
		return itemName;
	}

	public string GetTypeName() {
		string typeName = "";
		switch (instance.trapType) {
		case ETrapType.Move:
			typeName = "Move";
			break;
		case ETrapType.Injured:
			typeName = "Injured";
			break;
		case ETrapType.StateException:
			typeName = "StateException";
			break;
		case ETrapType.ChangeEnvir:
			typeName = "Environment";
			break;
		default:
			break;
		}
		return typeName;
	}

	#region ITrapExcute implementation

	public virtual void Excute () {

	}

	public virtual void ExcuteByDisk () {

	}

	#endregion

	string MoveItemTrapName(int effectType) {
		switch (effectType) {
		case 1:
			return "Prev";
		case 2:
			return "Random";
		case 3:
			return "Start";
		default:
			return "";
				
		}
	}

	string InjuredItemTrapName(int effectType) {
		if (effectType == 1) {
			return "Mine";
		}
		if (effectType == 2) {
			return "Trapping";
		}
		if (effectType == 3) {
			return "Hungry";
		}
		if (effectType == 4) {
			return "LostMoney";
		}
		return "";
	}

	static string MoveTrapName(int effectType) {
		if (effectType == 1) {
			return "MoveTrapPrev";
		}
		if (effectType == 2) {
			return "MoveTrapRandom";
		}
		if (effectType == 3) {
			return "MoveTrapStart";
		}
		return "";
	}

	static string InjuredTrapName(int effectType) {
		if (effectType == 1) {
			return "InjuredTrapMine";
		}
		if (effectType == 2) {
			return "InjuredTrapTrapping";
		}
		if (effectType == 3) {
			return "InjuredTrapHungry";
		}
		if (effectType == 4) {
			return "InjuredTrapLostMoney";
		}
		return "";
	}
}