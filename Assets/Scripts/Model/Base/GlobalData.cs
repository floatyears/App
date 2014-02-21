using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class GlobalData  {
	private static GlobalData instance;
	public static GlobalData Instance{
		get{
			if(instance == null) {
				instance = new GlobalData();
			}
			return instance;
		}

	}
	private GlobalData() {}

	public UnitInfo GetUnitInfo(uint unitID) {
		if(tempUnitInfo.ContainsKey(unitID)) {
			TempUnitInfo tui = tempUnitInfo[unitID];
			return tui.DeserializeData<UnitInfo>();
		}
		else{
			Debug.LogError("unitid is invalid");
			return null;
		}
	}

	public static Dictionary<int, ProtobufDataBase> tempNormalSkill = new Dictionary<int, ProtobufDataBase>();
	public static Dictionary<uint, TempUnitInfo>	tempUnitInfo = new Dictionary<uint, TempUnitInfo> ();
	public static Dictionary<uint, UserUnitInfo> tempUserUnitInfo = new Dictionary<uint, UserUnitInfo>();
	public static Dictionary<uint, TempEnemy> tempEnemyInfo = new Dictionary<uint, TempEnemy> ();
	public static Dictionary<int, UnitBaseInfo> tempUnitBaseInfo = new Dictionary<int, UnitBaseInfo> ();
	public static Dictionary<uint, TrapBase> tempTrapInfo = new Dictionary<uint, TrapBase> ();
	//public static Dictionary<uint,>

	public const int maxEnergyPoint = 20;
	public const int posStart = 1;
	public const int posEnd = 6;
	public const int minNeedCard = 2;
	public const int maxNeedCard = 5;

	public static Dictionary<int, Object> tempEffect = new Dictionary<int, Object>();
	public static List<int> HaveCard = new List<int>() {111,185,161,101,122,195};

	//Temp
	//public static List<int> HaveFriend = new List<int>(){};

	private static GameObject itemObject;
	public static GameObject ItemObject {
		get{
			if(itemObject == null) {
				itemObject = Resources.Load("Prefabs/UI/Friend/FriendScrollerItem") as GameObject;
			}
			return itemObject;
		}
	}

	private static UnitBaseInfo friendBaseInfo ;
	public static UnitBaseInfo FriendBaseInfo {
		get {
			if(friendBaseInfo == null) {
				friendBaseInfo = tempUnitBaseInfo[195];

			}
			return friendBaseInfo;
		}
	}
	

//	public static object GetEffect (Effect effect) {
//		int type = (int)effect;
//		object obj = null;

//		if (effect == Effect.DragCard) {
//			if (!tempEffect.TryGetValue (type, out obj)) {
//				string path = GetEffectPath(type);
//				obj = Resources.Load(path);
//				tempEffect.Add(type,obj);
//			}
//		}
//	}

	public static Object GetEffect (int type) {
		Object obj = null;
		if (!tempEffect.TryGetValue (type, out obj)) {
			string path = GetEffectPath(type);
			obj = Resources.Load(path);
			tempEffect.Add(type,obj);
		}
		return obj;
	}

	static string GetEffectPath(int type) {
		string path = string.Empty;
		switch (type) {
		case 1:
			path = "Effect/fire";
			break;
		case 2:
			path = "Effect/water";
			break;
		case 3:
			path = "Effect/wind";
			break;
		case 8:
			path = "Effect/card_effect";
			break;
		default:
				break;
		}
		return path;
	}
}

public enum Effect {
	DragCard = 8,
}
