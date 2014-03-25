using UnityEngine;
using System.Collections;
using bbproto;

public class AttackInfo {
	private static int sequenceID = -1;
	public static void ClearData () {
		sequenceID = -1;
	}

	public AttackInfo (){
		sequenceID++;
		this.attackID = sequenceID;
	}
	
	private int attackID = -1;
	public int AttackID {
		get {return attackID;}
	}
	
	private string userUnitID = null;
	public string UserUnitID {
		get { return userUnitID; }
		set { userUnitID = value; }
	}
	
	private int userPos = -1;
	public int UserPos {
		get {return userPos;}
		set {userPos = value;}
	}
	
	private int needCardNumber = -1;
	public int NeedCardNumber {
		get {return needCardNumber;}
		set {needCardNumber = value;}
	}
	
	private int skillID = -1;
	public int SkillID {
		get {return skillID;}
		set {skillID = value;}
	}
	
	private int attackType = -1;
	public int AttackType {
		get { return attackType; }
		set {attackType = value; }
	}
	
	private int attackRange ;
	/// <summary>
	/// 0 = single attack
	/// 1 = all attack
	/// 2 = recover hp
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get { return attackRange; }
		set { attackRange = value; }
	}
	
	private float attackValue ;
	public float AttackValue {
		get {return attackValue; }
		set {attackValue = value;}
	}
	
	private int continuAttackMultip = 1;
	public int ContinuAttackMultip {
		get {return continuAttackMultip;}
		set {continuAttackMultip = value;}
	}
	
	private uint enemyID = 0;
	
	public uint EnemyID {
		get {return enemyID;}
		set {enemyID = value;}
	}
	
	private int injuryValue ;
	public int InjuryValue
	{
		get {return injuryValue;}
		set {injuryValue = value;}
	}
	
	private Object effect;
	public Object Effect {
		get {return effect;}
		set {effect = value;}
	}
	private bool ignoreDefense = false;
	public bool IgnoreDefense {
		get {return ignoreDefense;}
		set {ignoreDefense = value;}
	}
	
	private int attackRound = 1; 
	public int AttackRound {
		get { return attackRound; }
		set { attackRound = value; }
	}
	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
	//public int originIndex = -1;
}

public class AISortByCardNumber : IComparer{
	public int Compare (object x, object y)
	{
		AttackInfo ai1 = x as AttackInfo;
		AttackInfo ai2 = y as AttackInfo;
		return ai1.NeedCardNumber.CompareTo(ai2.NeedCardNumber);
	}
}

public class AISortByUserpos : IComparer{
	public int Compare (object x, object y)
	{
		AttackInfo ai1 = x as AttackInfo;
		AttackInfo ai2 = x as AttackInfo;
		return ai1.UserPos.CompareTo(ai2.UserPos);
	}
}