using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigViewData 
{
	public static List<UnitInfo> OwnedUnitInfoList = new List<UnitInfo>();
	public ConfigViewData(){
		ConfigOwnedUnitInfo();
	}

	void ConfigOwnedUnitInfo(){
		UnitInfo tempUnitInfo = new UnitInfo();

		tempUnitInfo.id = 1;
		tempUnitInfo.name = "Yurnero";
		tempUnitInfo.type = EUnitType.UFIRE;
		tempUnitInfo.maxLevel = 35;
		tempUnitInfo.cost = 7;
		OwnedUnitInfoList.Add( tempUnitInfo );

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 2;
		tempUnitInfo.name = "Zeus";
		tempUnitInfo.type = EUnitType.UWATER;
		tempUnitInfo.maxLevel = 50;
		tempUnitInfo.cost = 9;
		OwnedUnitInfoList.Add( tempUnitInfo );

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 3;
		tempUnitInfo.name = "Bradwarden";
		tempUnitInfo.type = EUnitType.UWIND;
		tempUnitInfo.maxLevel = 70;
		tempUnitInfo.cost = 12;
		OwnedUnitInfoList.Add( tempUnitInfo );

		tempUnitInfo = new UnitInfo();
		tempUnitInfo.id = 4;
		tempUnitInfo.name = "Coco";
		tempUnitInfo.type = EUnitType.ULIGHT;
		tempUnitInfo.maxLevel = 90;
		tempUnitInfo.cost = 26;
		OwnedUnitInfoList.Add( tempUnitInfo );

		Debug.Log(string.Format( "Finlish configging owned unit info, the Count of OwnedUnitInfoList is {0}", OwnedUnitInfoList.Count) );


	}

}

