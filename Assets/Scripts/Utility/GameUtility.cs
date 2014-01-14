using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DGTools
{
	public int RandomToInt(int min,int max)
	{
		return Random.Range(min,max);
	}

	public static bool ListContains<T>(IList<T> big, IList<T> small) {
		for (int i = 0; i < small.Count; i++) {
			if(!big.Contains(small[i])) {
				return false;
			}
		}
		return true;
	}

	public static bool IsFirstBoost<T>(IList<T> first, IList<T> second) {
		if (first.Count < second.Count) {
			return true;
		}
		else {
			return false;
		}
	}

	public static int CaculateAddBlood (int add) {
		return add * 100;
	}

	public static int CaculateAddAttack (int add) {
		return add * 50;
	}

	public static int CaculateAddDefense (int add) {
		return add * 10;
	}



	public static float IntegerSubtriction(int firstInterger,int secondInterger) {
		return (float)firstInterger / (float)secondInterger;
	}
}

public class GameLayer
{

//	private static LayerMask Default = 0;
	
	public static LayerMask TransparentFX = 1;
	
	public static LayerMask IgnoreRaycast = 2;
	
	//null = 3,
	
	public static LayerMask Water = 4;
	
	//null = 5,
	
	//null = 6,
	
	//null = 7,
	
	public static LayerMask ActorCard = 8;

	public static LayerMask BattleCard = 9;

	public static LayerMask IgnoreCard = 10;

	public static LayerMask EnemyCard = 11;

	public static int LayerToInt(LayerMask layer)
	{
		return 1 << layer;
	}
}