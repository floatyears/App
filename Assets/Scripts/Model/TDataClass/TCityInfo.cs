using UnityEngine;
using System.Collections;
using bbproto;

public class TCityInfo : ProtobufDataBase {
	private CityInfo instance;

	public TCityInfo (CityInfo ci) : base (ci) {
		instance = ci;
	}
	public CityInfo cityInfo {
		get { return instance; }
	}


}
