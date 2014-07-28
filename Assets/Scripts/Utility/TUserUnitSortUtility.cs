using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public enum SortRule {
	None			=0,
	Attack			= 1,
	HP				= 2,
	Attribute		= 3,
	Race				= 4,
	GetTime		= 5,
	ID					= 6,
	Fav				= 7,
	AddPoint		= 8,
	Login				= 9,
	Rank				= 10
}

public enum SortRuleByUI{
	ApplyView,
	SellView,
	FriendListView,
	PartyView,
	ReceptionView,
	UnitDisplayUnity,
	MyUnitListView,
	LevelUp,
	Evolve
}

public class SortUnitTool{
	public const SortRule DEFAULT_SORT_RULE = SortRule.Attribute;
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
//		Debug.LogError("SortByTargetRule befoure : " + targetRule);
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
				DGTools.InsertSort(targetList, new TUserUnitSortID(), false);
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
//		Debug.LogError("SortByTargetRule end : " + targetRule);
	}

	public static void SortByTargetRule(SortRule targetRule, List<TFriendInfo> targetList){
		switch (targetRule){
			case SortRule.AddPoint : 
				DGTools.InsertSort(targetList, new TFriendUnitSortAddPoint());
				break;
			case SortRule.Attack : 
				DGTools.InsertSort(targetList, new TFriendUnitSortAtk());
				break;
			case SortRule.Attribute : 
				DGTools.InsertSort(targetList, new TFriendUnitSortAttribute());
				break;
			case SortRule.Login : 
				DGTools.InsertSort(targetList, new TFriendUnitSortLoginTime());
				break;
			case SortRule.HP : 
				DGTools.InsertSort(targetList, new TFriendUnitSortHP());
				break;
			case SortRule.ID : 
				DGTools.InsertSort(targetList, new TFriendUnitSortID(), false);
				break;
			case SortRule.Fav : 
				DGTools.InsertSort(targetList, new TFriendUnitSortFavourite());
				break;
			case SortRule.Race : 
				DGTools.InsertSort(targetList, new TFriendUnitSortRace());
				break;
			case SortRule.Rank : 
				DGTools.InsertSort(targetList, new TFriendUnitSortRank());
				break;
			default:
				break;
		}
	}

	public static SortRule GetSortRule(SortRuleByUI srui){
		int data = GameDataStore.Instance.GetIntDataNoEncypt ("SortRule_" + srui.ToString ());

		if (data == 0) {
			return SortRule.Attribute;
		}
		return (SortRule)data;
	} 

	public static void StoreSortRule(SortRule value, SortRuleByUI srui){
		GameDataStore.Instance.StoreIntDatNoEncypt ("SortRule_" + srui.ToString (), (int)value);
	} 
}

//------------------------------TUserUnit-------------------------------
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
		return firstUserUnit.UnitID.CompareTo(secondUserUnit.UnitID);
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
		return firstUserUnit.IsFavorite.CompareTo (secondUserUnit.IsFavorite);
	}
}

public class TUserUnitSortAddPoint : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		return firstUserUnit.AddNumber.CompareTo(secondUserUnit.AddNumber);
	}
}

public class TUserUnitSortAttribute : TUserUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int first = (int)firstUserUnit.UnitInfo.Type;
		int second = (int)secondUserUnit.UnitInfo.Type;
		int compareValue = - first.CompareTo(second);
		return compareValue;
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

//------------------------------TFriendInfo-------------------------------
public class TFriendUnitSortBase : IComparer {
	protected TFriendInfo firstFriendUnit;
	protected TFriendInfo secondFriendUnit;
	public virtual int Compare(object x, object y) {
		firstFriendUnit = x as TFriendInfo;
		secondFriendUnit = y as TFriendInfo;
		return default(int);
	}
}

public class TFriendUnitSortHP : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstHp = firstFriendUnit.UserUnit.Hp;
		int secondHp = secondFriendUnit.UserUnit.Hp;
		return firstHp.CompareTo(secondHp);
	}
}

public class TFriendUnitSortAtk : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstAtk = firstFriendUnit.UserUnit.Attack;
		int secondAtk = secondFriendUnit.UserUnit.Attack;
		return firstAtk.CompareTo(secondAtk);
	}
}

public class TFriendUnitSortID : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		uint firstID = firstFriendUnit.UserUnit.UnitID;
		uint secondID = secondFriendUnit.UserUnit.UnitID;
		return firstID.CompareTo(secondID);
	}
}

public class TFriendUnitSortLoginTime : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		uint firstGetTime = firstFriendUnit.LastPlayTime;
		uint secondGetTime = secondFriendUnit.LastPlayTime;
		return firstGetTime.CompareTo(secondGetTime);
	}
}

public class TFriendUnitSortFavourite : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		//TODO
		return 1;
	}
}

public class TFriendUnitSortAddPoint : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstAddNum = firstFriendUnit.UserUnit.AddNumber;
		int secondAddNum = secondFriendUnit.UserUnit.AddNumber;
		return firstAddNum.CompareTo(secondAddNum);
	}
}

public class TFriendUnitSortAttribute : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstType = (int)firstFriendUnit.UserUnit.UnitInfo.Type;
		int secondType = (int)secondFriendUnit.UserUnit.UnitInfo.Type;
		return -firstType.CompareTo(secondType);
	}		
}

public class TFriendUnitSortRace : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstRace = (int)firstFriendUnit.UserUnit.UnitInfo.Race;
		int secondRace = (int)secondFriendUnit.UserUnit.UnitInfo.Race;
		return firstRace.CompareTo(secondRace);
	}
}

public class TFriendUnitSortRank : TFriendUnitSortBase{
	public override int Compare(object x, object y) {
		base.Compare(x,y);
		int firstRank = (int)firstFriendUnit.Rank;
		int secondRank = (int)secondFriendUnit.Rank;
		return firstRank.CompareTo(secondRank);
	}
}

