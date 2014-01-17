using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DGTools {
	public int RandomToInt(int min,int max) {
		return UnityEngine.Random.Range(min,max);
	}

	public static bool ListContains<T>(List<T> big, List<T> small) {
		if (big.Count < small.Count) {
			return false;
		}
						
		for (int i = 0; i < small.Count; i++) {
			if(!big.Contains(small[i])) {
				return false;
			}
		}

		return true;
	}

	public static bool IsTriggerSkill<T> (List<T> cardList, List<T> skillList) where T : struct {
		if (cardList.Count < skillList.Count) {
			return false;		
		}
		List<T> tempCard = new List<T> (cardList);
		List<T> tempSkillList = new List<T> (skillList);

		for (int i = 0; i < tempSkillList.Count; i++) {
			if(tempCard.Contains(tempSkillList[i])) {
				T value = tempSkillList[i];
				tempCard.Remove(value);
			}
			else  {
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
			//T temp = target[i];
			for (int j = 0; j < i; j++) {
				int compare = compareObject.Compare(target[i], target[j]);
				//Debug.LogWarning(compareObject.GetType() + "``````" +compare + "  sort : " + sort);
				if(sort && compare > 0) {
//					AttackInfo aii = target[i] as AttackInfo;
//					AttackInfo aij = target[j] as AttackInfo;
//					if(aii != null && aij != null) {
//						Debug.LogWarning(" -----------------------------------------------------------------------------------" );
//						Debug.LogWarning("i : " + i + " j : " + j);
//						Debug.LogWarning("aii.originIndex " + aii.originIndex + " aii.NeedCardNumber : " + aii.NeedCardNumber);
//						Debug.LogWarning("aij.originIndex " + aij.originIndex + " aij.NeedCardNumber : " + aij.NeedCardNumber);
//					}
					T temp = target[i];
					target[i] = target[j];
					target[j] = temp;

					continue;
				}
		
				if(!sort && compare < 0) {
					T temp = target[i];
					target[i] = target[j];
					target[j] = temp;
				}
			}
		}
	}

	public static void InsertSortBySequence<T,T1> (IList<T> target, T1 compareObject, bool sort = true) where T1 :  IComparer {
		if (target == null) {
			return;
		}
		int length = target.Count;
		for (int i = 1; i < length; i++) {

			for (int j = 0; j < i; j++) {
				int compare = compareObject.Compare(target[i], target[j]);
				if(sort && compare > 0) {
					T temp = target[j];
					target[j] = target[i];
					int k = j + 1;
					T temp1 ;
					while(k <= i) {
						temp1 = target[k];
						target[k] = temp;
						temp = temp1;
						k++;
					}

					//target[j] = temp;
					continue;
				}
				
				if(!sort && compare < 0) {
					T temp = target[i];
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