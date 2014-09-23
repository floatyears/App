using UnityEngine;
using System.Collections.Generic;
using bbproto;

namespace bbproto{
public partial class CityInfo : ProtobufDataBase {

	private List<StageInfo> stageInfo;

	public void InitStageInfo (CityInfo ci) {
		stageInfo = new List<StageInfo> ();
		stageInfo.AddRange (ci.stages);
	}

	public int PositionX {
		get {
			return pos.x;
		}
	}

	public int PositionY {
		get {
			return pos.y;
		}
	}

	public StageInfo GetStage(uint stageID) {
		if (stageInfo == null) {
			LogHelper.Log("City : {0} stage list is null ! ", id);
			return null;
		}
//		Debug.LogError (stageID + " stageInfo : " + stageInfo.Count);
		foreach (var item in stageInfo) {
//			Debug.LogError(" GetStage : " + item.ID);
		}
		StageInfo tsi = stageInfo.Find (a => a.id == stageID);
		if (tsi == default(StageInfo)) {
			LogHelper.Log("City : {0} stage list not containt this stage {1} ! ", id, stageID);
			return null;
		}
//		Debug.LogError (" return tsi : " + tsi.ID);
		return tsi;
	}
}
}
