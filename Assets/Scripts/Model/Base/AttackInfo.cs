using UnityEngine;
using System.Collections;
using bbproto;

public class AttackInfo : ProtobufDataBase{
	private static int sequenceID = -1;
	public static void ClearData () {
		sequenceID = -1;
	}

	private AttackInfoProto instance;
	public AttackInfo (AttackInfoProto ins) : base(ins){
		instance = ins;
		if(instance.continueAttackMultip == 0)
			instance.continueAttackMultip = 1;
		if(Mathf.Approximately(instance.attackRate,0f))
			instance.attackRate = 1f;
		sequenceID++;
		instance.attackID = sequenceID;
	}

	public AttackInfoProto Instance{
		get { return instance; }
	}

//	private int attackID = -1;
	public int AttackID {
		get {return instance.attackID;}
	}
	
//	private string userUnitID = null;
	public string UserUnitID {
		get { return instance.userUnitID; }
		set { instance.userUnitID = value; }
	}
	
//	private int userPos = -1;
	public int UserPos {
		get {return instance.userPos;}
		set {instance.userPos = value;}
	}
	
//	private int needCardNumber = -1;
	public int NeedCardNumber {
		get {return instance.needCardNumber;}
		set {instance.needCardNumber = value;}
	}

//	private int skillID = -1;
	public int SkillID {
		get {return instance.skillID;}
		set {instance.skillID = value;}
	}

//	private int attackType = -1;
	public int AttackType {
		get { return instance.attackType; }
		set {instance.attackType = value; }
	}

//	private int attackRace = -1;
	public int AttackRace {
		get { return instance.attackRace; }
		set {instance.attackRace = value; }
	}

//	private int attackRange ;
	/// <summary>
	/// 0 = single attack
	/// 1 = all attack
	/// 2 = recover hp
	/// </summary>
	/// <value>The attack range.</value>
	public int AttackRange {
		get { return instance.attackRange; }
		set { instance.attackRange = value; }
	}
	
//	private float attackValue ;
	public float AttackValue {
		get {return instance.attackValue; }
		set {instance.attackValue = value;}
	}
	
//	private int continuAttackMultip = 1;
	public int ContinuAttackMultip {
		get {return instance.continueAttackMultip;}
		set {instance.continueAttackMultip = value;}
	}
	
//	private uint enemyID = 0;
	public uint EnemyID {
		get {return instance.enemyID;}
		set {instance.enemyID = value;}
	}
	
//	private int injuryValue ;
	public int InjuryValue
	{
		get {return instance.injuryValue;}
		set {instance.injuryValue = value;}
	}

//	private bool ignoreDefense = false;
	public bool IgnoreDefense {
		get {return instance.ignoreDefense;}
		set {instance.ignoreDefense = value;}
	}
	
//	private int attackRound = 1; 
	public int AttackRound {
		get { return instance.attackRound; }
		set { instance.attackRound = value; }
	}

//	private float attackRate = 1f;
	public float AttackRate {
		get { return instance.attackRate; }
		set { instance.attackRate = value; }
	}

	private UISprite attackSprite;
	public UISprite AttackSprite {
		get { return attackSprite; }
		set { attackSprite = value; }
	}

//	public void PlayAttack () {
//		if (attackSprite == null) {
//			return;	
//		}
//		attackSprite.spriteName = "";
//	}

//	private bool fixRecoverHP = false;
	public bool FixRecoverHP {
		get { return instance.fixRecoverHP; }
		set { instance.fixRecoverHP = value; }
	}

//	private int isLink = 0;	
	public int IsLink{
		get { return instance.isLink; }
		set { instance.isLink = value; }
	}

	//------------test need data, delete it behind test done------------//
	//------------------------------------------------------------------//
	//public int originIndex = -1;

	public static AttackInfo GetInstance() {
		AttackInfoProto aip = new AttackInfoProto ();
		AttackInfo ai = new AttackInfo (aip);
		return ai;
	}
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