using UnityEngine;
using System.Collections.Generic;

public class BattleDataMode {
	private static Dictionary<int,List<int>> battleCardData = new Dictionary<int, List<int>>();

	public static int GenerateCard(int areaID,int cardID)
	{
		List<int> targetValue = null;
		if (battleCardData.TryGetValue (areaID, out targetValue)) {
			if (targetValue == null) {
					targetValue = new List<int> ();
			}
			targetValue.Add (cardID);
		} 
		else {
			targetValue = new List<int>();
			targetValue.Add(cardID);
			battleCardData.Add(areaID,targetValue);
		}

		return DisposeColor (targetValue);
	}

	public static int DisposeColor(List<int> target){
		int index = target.Count - 1;
		int returnValue = target [index];
		//target.Clear ();
		return returnValue;
	}

	public static int hurtValue(){

		return 0;
	}
}
