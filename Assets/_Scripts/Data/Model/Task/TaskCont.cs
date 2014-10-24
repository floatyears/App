using UnityEngine;
using System.Collections;

namespace bbproto{

	public partial class TaskConf : global::ProtoBuf.IExtensible{

		public TaskStateEnum TaskState {
			get;
			set;
		}
	}
}

public enum TaskStateEnum{
	NONE,
	NotComp,
	TaskComp,
	TaskBonusComp,
}