using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TCityInfo : ProtobufDataBase {
	private CityInfo instance;
	private List<TStageInfo> stageInfo;
	public TCityInfo (CityInfo ci) : base (ci) {
		instance = ci;

		InitStageInfo (ci);
	}

	void InitStageInfo (CityInfo ci) {
		stageInfo = new List<TStageInfo> ();
		for (int i = 0; i < ci.stages.Count; i++) {
			TStageInfo tsi = new TStageInfo(instance.stages[i]);
			stageInfo.Add(tsi);
		}
	}

	public CityInfo cityInfo {
		get { return instance; }
	}

	public uint ID {
		get { return instance.id; }
	}

	public int State {
		get { return instance.state; }
	}

	public string CityName {
		get {
			return instance.cityName;
		}
	}

	public string Description {
		get {
			return instance.description;
		}
	}

	public Position Position {
		get {
			return instance.pos;
		}
	}

	public int PositionX {
		get {
			return Position.x;
		}
	}

	public int PositionY {
		get {
			return Position.y;
		}
	}

	public List<TStageInfo> Stages {
		get {
			return stageInfo;
		}
	}

	public TStageInfo GetStage(uint stageID) {

		if (stageInfo == null) {
			LogHelper.Log("City : {0} stage list is null ! ", instance.id);
			return null;
		}
		Debug.LogError (stageID + " stageInfo : " + stageInfo.Count);
		foreach (var item in stageInfo) {
			Debug.LogError(" GetStage : " + item.ID);
		}
		TStageInfo tsi = stageInfo.Find (a => a.ID == stageID);
		if (tsi == default(TStageInfo)) {
			LogHelper.Log("City : {0} stage list not containt this stage {1} ! ", instance.id, stageID);
			return null;
		}
		Debug.LogError (" return tsi : " + tsi.ID);
		return tsi;
	}
}
