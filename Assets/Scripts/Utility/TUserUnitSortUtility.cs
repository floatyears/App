using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SortRule {
	ByAttack			= 0,
	ByHP					= 1,
	ByAttribute			= 2,
	ByRace				= 3,
	ByGetTime			= 4,
	ByID					= 5,
	ByIsCollected		= 6,
	ByAddPoint		= 7
}

public class SortUnitTool{
	const int ruleCount = 8;
	public static SortRule GetNextRule(SortRule currentRule){
		SortRule nextRule;
		int currentIndex = (int)currentRule;
		currentIndex++;
		currentIndex = currentIndex % ruleCount;
		nextRule = (SortRule)currentIndex;
		return nextRule;
	}

	public static void SortByTargetRule(SortRule targetRule, List<TUserUnit> targetList){
		//Debug.Log("Before :: memberList[ 4 ].Level ->" + targetList[ 4 ].Level);
		
		switch (targetRule){
			case SortRule.ByAddPoint : 
				DGTools.InsertSort(targetList, new TUserUnitSortAddPoint());
				break;
			case SortRule.ByAttack : 
				DGTools.InsertSort(targetList, new TUserUnitSortAtk());
				break;
			case SortRule.ByAttribute : 
				DGTools.InsertSort(targetList, new TUserUnitSortAttribute());
				break;
			case SortRule.ByGetTime : 
				DGTools.InsertSort(targetList, new TUserUnitSortGetTime());
				break;
			case SortRule.ByHP : 
				DGTools.InsertSort(targetList, new TUserUnitSortHP());
				break;
			case SortRule.ByID : 
				DGTools.InsertSort(targetList, new TUserUnitSortID());
				break;
			case SortRule.ByIsCollected : 
				DGTools.InsertSort(targetList, new TUserUnitSortFavourite());
				break;
			case SortRule.ByRace : 
				DGTools.InsertSort(targetList, new TUserUnitSortRace());
				break;
			default:
				break;
		}

		//Debug.Log("After :: memberList[ 4 ].Level ->" + targetList[ 4 ].Level);
	}
}

public class TUserUnitSortBase : IComparer {
	protected TUserUnit firstUserUnit;
	protected TUserUnit secondUserUnit;
	public virtual int Compare(object x, object y) {
		firstUserUnit = x as TUserUnit;
		secondUserUnit = y as TUserUnit;
		return default(int);
	}
}

public class TUserUnitSortHP : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return firstUserUnit.Hp.CompareTo(secondUserUnit.Hp);
	}
}

public class TUserUnitSortAtk : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return firstUserUnit.Attack.CompareTo(secondUserUnit.Attack);
	}
}

public class TUserUnitSortID : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return firstUserUnit.ID.CompareTo(secondUserUnit.ID);
	}
}

public class TUserUnitSortGetTime : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return firstUserUnit.Unit.getTime.CompareTo(secondUserUnit.Unit.getTime);
	}
}

public class TUserUnitSortFavourite : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		//TODO
//		if(firstUserUnit.isFavorate !=0 && !secondUserUnit.isFavorate) {
//			return 1;
//		}
//
//		if(firstUserUnit.isFavorate && secondUserUnit.isFavorate) {
//			return 0;
//		}
//
//		if(!firstUserUnit.isFavorate && secondUserUnit.isFavorate) {
//			return -1;
//		}
		return -1;
	}
}

public class TUserUnitSortAddPoint : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return secondUserUnit.AddNumber.CompareTo(firstUserUnit.AddNumber);
	}
}

public class TUserUnitSortAttribute : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		//TODO
		return -1;

	}
}

public class TUserUnitSortRace : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		//TODO
		return -1;

	}
}
