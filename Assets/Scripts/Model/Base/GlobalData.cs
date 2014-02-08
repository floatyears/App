﻿using UnityEngine;
using System.Collections.Generic;

public class GlobalData  {
	public static Dictionary<int, ProtobufDataBase> tempNormalSkill = new Dictionary<int, ProtobufDataBase>();
	public static Dictionary<int, TempUnitInfo>	tempUnitInfo = new Dictionary<int, TempUnitInfo> ();
	public static Dictionary<int, UserUnitInfo> tempUserUnitInfo = new Dictionary<int, UserUnitInfo>();
	public static Dictionary<int, TempEnemy> tempEnemyInfo = new Dictionary<int, TempEnemy> ();
	public static Dictionary<int, UnitBaseInfo> tempUnitBaseInfo = new Dictionary<int, UnitBaseInfo> ();
	public const int maxEnergyPoint = 20;
	public const int posStart = 1;
	public const int posEnd = 6;
	public const int minNeedCard = 2;
	public const int maxNeedCard = 5;

	public static Dictionary<int, Object> tempEffect = new Dictionary<int, Object>();
	public static List<int> HaveCard = new List<int>() {111,185,161,101,122,195};

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
				//Debug.Log("FriendBaseInfo is Null");
			}
			return friendBaseInfo;
		}
	}

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
		default:
				break;
		}
		return path;
	}
}
