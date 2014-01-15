﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DGTools {
	public int RandomToInt(int min,int max) {
		return UnityEngine.Random.Range(min,max);
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

//	public static int CaculateAttack (UserUnit uu, UnitInfo ui) {
//		int attack = uu.addHp * 50 + ui.power[
//	}

	public static int CaculateAddDefense (int add) {
		return add * 10;
	}



	public static float IntegerSubtriction(int firstInterger,int secondInterger) {
		return (float)firstInterger / (float)secondInterger;
	}

	/// <summary>
	/// Inserts the sort.
	/// </summary>
	/// <param name="target">Target collections.</param>
	/// <param name="sort">Sort is by Ascending or Descending.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void InsertSort<T,T1> (IList<T> target, T1 compareObject, bool sort = true) where T1 :  IComparer {
		if (target == null) {
			return;
		}
		int length = target.Count;
		for (int i = 1; i < length; i++) {
			T temp = target[i];
			for (int j = 0; j < i; j++) {
				int compare = compareObject.Compare(temp,target[j]);
				if(sort && compare > 0) {
					temp = target[i];
					target[i] = target[j];
					target[j] = temp;

					continue;
				}
		
				if(!sort && compare < 0) {
					temp = target[i];
					target[i] = target[j];
					target[j] = temp;
				}
			}
		}
	}

	public static void SwitchObject<T>(ref T arg1,ref T arg2) {
		T temp = arg1;
		arg1 = arg2;
		arg2 = temp;
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