using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class QuestInfo : ProtoBuf.IExtensible {

	public bool IsClear {
		get {return (state == EQuestState.QS_CLEARED);}
	}

}
}