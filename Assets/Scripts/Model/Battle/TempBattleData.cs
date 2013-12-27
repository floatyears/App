using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TempBattleData {
	private static Dictionary<int,UserUnit> userUnitDic = new Dictionary<int, UserUnit>();

	private static List<int> partyUser = new List<int> ();

	public static UserUnit GetUserUnit(int id)
	{
		UserUnit user = null;
		userUnitDic.TryGetValue (id, out user);
		return user;
	}

	
}
