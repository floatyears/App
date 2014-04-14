using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public enum SortRule {
	Attack			= 0,
	HP				= 1,
	Attribute		= 2,
	Race				= 3,
	GetTime		= 4,
	ID					= 5,
	Fav				= 6,
	AddPoint		= 7
}

public class SortUnitTool{
	public const SortRule DEFAULT_SORT_RULE = SortRule.GetTime;
	public const int RULE_KIND_COUNT = 8;

	public static SortRule GetNextRule(SortRule currentRule){
		SortRule nextRule;
		int currentIndex = (int)currentRule;
		currentIndex++;
		currentIndex = currentIndex % RULE_KIND_COUNT;
		nextRule = (SortRule)currentIndex;
		return nextRule;
	}

	public static void SortByTargetRule(SortRule targetRule, List<TUserUnit> targetList){
		//Debug.Log("Before :: memberList[ 4 ].Level ->" + targetList[ 4 ].Level);
		
		switch (targetRule){
			case SortRule.AddPoint : 
				DGTools.InsertSort(targetList, new TUserUnitSortAddPoint());
				break;
			case SortRule.Attack : 
				DGTools.InsertSort(targetList, new TUserUnitSortAtk());
				break;
			case SortRule.Attribute : 
				DGTools.InsertSort(targetList, new TUserUnitSortAttribute());
				break;
			case SortRule.GetTime : 
				DGTools.InsertSort(targetList, new TUserUnitSortGetTime());
				break;
			case SortRule.HP : 
				DGTools.InsertSort(targetList, new TUserUnitSortHP());
				break;
			case SortRule.ID : 
				DGTools.InsertSort(targetList, new TUserUnitSortID());
				break;
			case SortRule.Fav : 
				DGTools.InsertSort(targetList, new TUserUnitSortFavourite());
				break;
			case SortRule.Race : 
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
		return 1;
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
		int first = (int)firstUserUnit.UnitInfo.Type;
		int second = (int)secondUserUnit.UnitInfo.Type;
		return first.CompareTo(second);
	}		
}

public class TUserUnitSortRace : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int first = (int)firstUserUnit.UnitInfo.Race;
		int second = (int)secondUserUnit.UnitInfo.Race;
		return first.CompareTo(second);

	}
}
