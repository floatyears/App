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
namespace bbproto{
	public partial class LoginInfo : ProtoBuf.IExtensible {
	//// property ////
		public int rank;
		/// 
		public	List<BonusInfo> Bonus { get { return bonus; } set{bonus.Clear();bonus.AddRange(value);} 
		
		}
			
	}
}