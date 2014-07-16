using bbproto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class TBonusInfo : ProtobufDataBase {
//	private BonusInfo	instance;
//	public TBonusInfo(BonusInfo inst):base(inst) { 
//		instance = inst;
//	}
//}

public class TLoginInfo : ProtobufDataBase {
	private LoginInfo	instance;
	public TLoginInfo(LoginInfo inst):base(inst) { 
		instance = inst;
	}

	//// property ////
	public	List<BonusInfo> Bonus { get { return instance.bonus; } set{instance.bonus.Clear();instance.bonus.AddRange(value);} }

	public int LoginDayTotal { get {return instance.loginDayTotal; } }
	public int LoginChain { get {return instance.loginChain; } }
	public uint LastLoginTime { get {return instance.lastLoginTime; } }
}


