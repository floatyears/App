using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TUnitCatalog {
	private StageInfo instance;

	private List<byte> meetFlag;
	private List<byte> haveFlag;

	void ConvertFlag( byte[] srcflag, List<byte> targetflag) {
		targetflag.Clear();
		for(int i=0; i < srcflag.Length; i++){
			Debug.LogError("b['"+i+"']: "+srcflag[i]);
			
			for (int b=0; b<8; b++){
				byte value = (byte)( (srcflag[i] >> b) & 1 );
				Debug.LogError("   add bit:"+value);
				targetflag.Add(value);
			}
		}
	}
	
	//constructor
	public TUnitCatalog(byte[] meetflag, byte[]haveflag) {
		ConvertFlag(meetflag, this.meetFlag);
		ConvertFlag(haveflag, this.haveFlag);
	}

	//
	public bool IsMeetNotHaveUnit(uint unitId) {
		return ( unitId >= meetFlag.Count ) ? false : (meetFlag[(int)unitId-1]==1);
	}

	public bool IsHaveUnit(uint unitId) {
		return ( unitId >= haveFlag.Count ) ? false : (haveFlag[(int)unitId-1]==1);
	}


	public void AddHaveUnit(uint unitId) {
		while( unitId >= haveFlag.Count ) {
			haveFlag.Add(0);
		}

		haveFlag[(int)unitId-1] = 1;
	}

	public void AddMeetNotHaveUnit(uint unitId) {
		while( unitId >= meetFlag.Count ) {
			meetFlag.Add(0);
		}
		
		meetFlag[(int)unitId-1] = 1;
	}
}