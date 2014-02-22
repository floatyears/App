using bbproto;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigViewData 
{
	public static List<UserUnit> OwnedUnitInfoList = new List<UserUnit>();
	public ConfigViewData(){
		ConfigOwnedUnitInfo();
	}

	void ConfigOwnedUnitInfo(){
		UserUnit tempUserUnit;

		tempUserUnit= new UserUnit();
		tempUserUnit.uniqueId = 1001;
		tempUserUnit.unitId = 1;
		tempUserUnit.exp = 134;
		tempUserUnit.level = 9;
		tempUserUnit.addAttack = 3;
		tempUserUnit.addHp = 4;
		OwnedUnitInfoList.Add( tempUserUnit );

		tempUserUnit= new UserUnit();
		tempUserUnit.uniqueId = 1002;
		tempUserUnit.unitId = 2;
		tempUserUnit.exp = 786;
		tempUserUnit.level = 54;
		tempUserUnit.addAttack = 8;
		tempUserUnit.addHp = 14;
		OwnedUnitInfoList.Add( tempUserUnit );

		tempUserUnit= new UserUnit();
		tempUserUnit.uniqueId = 1003;
		tempUserUnit.unitId = 3;
		tempUserUnit.exp = 134;
		tempUserUnit.level = 48;
		tempUserUnit.addAttack = 3;
		tempUserUnit.addHp = 4;
		OwnedUnitInfoList.Add( tempUserUnit );

		tempUserUnit= new UserUnit();
		tempUserUnit.uniqueId = 1004;
		tempUserUnit.unitId = 4;
		tempUserUnit.exp = 291;
		tempUserUnit.level = 107;
		tempUserUnit.addAttack = 4;
		tempUserUnit.addHp = 15;
		OwnedUnitInfoList.Add( tempUserUnit );

		tempUserUnit= new UserUnit();
		tempUserUnit.uniqueId = 1005;
		tempUserUnit.unitId = 5;
		tempUserUnit.exp = 291;
		tempUserUnit.level = 92;
		tempUserUnit.addAttack = 13;
		tempUserUnit.addHp = 24;
		OwnedUnitInfoList.Add( tempUserUnit );

//		Debug.Log(string.Format( "Finlish configging owned unit info, the Count of OwnedUnitInfoList is {0}", OwnedUnitInfoList.Count) );


	}

}

