using UnityEngine;
using System.Collections;
using bbproto;

namespace bbproto{
	public partial class SkillDelayTime : SkillBase, ProtoBuf.IExtensible{
	
		public float DelayTime{
			get {
				return value;
			}
		} 
	}
}