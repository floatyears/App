using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
	public partial class QuestInfo : ProtoBuf.IExtensible {
		
		public ECopyType CopyType = ECopyType.CT_NORMAL;
	}
}