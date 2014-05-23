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
	public	List<BonusInfo> Bonus { get { return instance.bonus; }  }

	public int LoginTotal { get {return instance.loginTotal; } }
	public int LoginChain { get {return instance.loginTotal; } }
	public uint LastLoginTime { get {return instance.lastLoginTime; } }
}


