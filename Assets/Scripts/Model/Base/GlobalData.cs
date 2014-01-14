using UnityEngine;
using System.Collections.Generic;

public class GlobalData  {
	public static Dictionary<int,ProtobufDataBase> tempNormalSkill = new Dictionary<int, ProtobufDataBase>();
	public static Dictionary<int, TempUnitInfo>	tempUnitInfo = new Dictionary<int, TempUnitInfo> ();
	public static Dictionary<int, UserUnitInfo> tempUserUnitInfo = new Dictionary<int, UserUnitInfo>();
	public const int maxEnergyPoint = 20;
}
