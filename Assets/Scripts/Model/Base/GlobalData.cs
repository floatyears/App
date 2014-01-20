using UnityEngine;
using System.Collections.Generic;

public class GlobalData  {
	public static Dictionary<int, ProtobufDataBase> tempNormalSkill = new Dictionary<int, ProtobufDataBase>();
	public static Dictionary<int, TempUnitInfo>	tempUnitInfo = new Dictionary<int, TempUnitInfo> ();
	public static Dictionary<int, UserUnitInfo> tempUserUnitInfo = new Dictionary<int, UserUnitInfo>();
	public static Dictionary<int, TempEnemy> tempEnemyInfo = new Dictionary<int, TempEnemy> ();
	public const int maxEnergyPoint = 20;
	public const int posStart = 1;
	public const int posEnd = 6;
	public const int minNeedCard = 2;
	public const int maxNeedCard = 5;
}
