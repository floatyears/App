using UnityEngine;
using System.Collections;
using bbproto;

public class TStageInfo : ProtobufDataBase {
	private StageInfo instance;
	public TStageInfo (StageInfo si) : base (si) {
		instance = si;
	}

	public StageInfo stageInfo {
		get { return instance;}
	}


}
