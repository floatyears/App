using UnityEngine;
using System.Collections.Generic;
using bbproto;

public class TUnitCatalog {
	private StageInfo instance;

	private List<byte> meetFlag;
	private List<byte> haveFlag;

	void ConvertFlag( byte[] srcflag, List<byte> targetflag) {
		if ( srcflag == null )
			return;
		targetflag.Clear();
		for(int i=0; i < srcflag.Length; i++){
//			Debug.LogError("b['"+i+"']: "+srcflag[i]);
			
			for (int b=0; b<8; b++){
				byte value = (byte)( (srcflag[i] >> b) & 1 );
//				if( srcflag[i] != 0)
//					Debug.LogError("["+(i*8+b)+"]  add bit:"+value);
				targetflag.Add(value);
			}
		}
//		Debug.LogError("   total targetflag.count: "+targetflag.Count);
	}
	
	//constructor
	public TUnitCatalog(byte[] meetflag, byte[]haveflag) {
		this.meetFlag = new List<byte>();
		this.haveFlag = new List<byte>();
		ConvertFlag(meetflag, this.meetFlag);
		ConvertFlag(haveflag, this.haveFlag);
//		Debug.Log("haveFlag count is : " + haveFlag.Count);
	}

	//
	public bool IsMeetNotHaveUnit(uint unitId) {
		return ( unitId-1 >= meetFlag.Count ) ? false : (meetFlag[(int)unitId-1]==1);
	}

	public bool IsHaveUnit(uint unitId) {
//		Debug.LogError ("ishaveunit : " + unitId + " haveFlag : " + haveFlag.Count);
		return ( unitId-1 >= haveFlag.Count ) ? false : (haveFlag[(int)unitId-1]==1);
	}


	public void AddHaveUnit(uint unitId) {
		while( unitId-1 >= haveFlag.Count ) {
			haveFlag.Add(0);
		}

		haveFlag[(int)unitId-1] = 1;
	}

	public void AddMeetNotHaveUnit(uint unitId) {
		while( unitId-1 >= meetFlag.Count ) {
			meetFlag.Add(0);
		}
		
		meetFlag[(int)unitId-1] = 1;
	}
}