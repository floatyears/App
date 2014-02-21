using UnityEngine;
using System.Collections;
using bbproto;

public class ConfigUnitBaseInfo {

	public ConfigUnitBaseInfo() {
		GenerateUnitBaseInfo ();
		Generate2 ();
		Generate3 ();

		GenerateUnitBaseExp();
		GenerateUnitBaseAttack();
//		GenerateUnitBaseDefense();
		GenerateUnitBaseHP();
	}
	// exp attack defense hp
	void GenerateUnitBaseExp () {
		PowerTable pt = new PowerTable();
		for (int i = 1; i < 100; i++) {
			PowerValue pv = new PowerValue();
			pv.level = i;
			pv.value = i * 100;
			pt.power.Add(pv);
		}
		GlobalData.unitValue .Add(1,new PowerTableInfo(pt)) ;
	}

	void GenerateUnitBaseAttack () {
		PowerTable pt = new PowerTable();
		for (int i = 1; i < 100; i++) {
			PowerValue pv = new PowerValue();
			pv.level = i;
			pv.value = i * 20;
			pt.power.Add(pv);
		}
		GlobalData.unitValue .Add(2,new PowerTableInfo(pt)) ; 
	}

//	void GenerateUnitBaseDefense () {
//		PowerTable pt = new PowerTable();
//		for (int i = 1; i < 100; i++) {
//			PowerValue pv = new PowerValue();
//			pv.level = i;
//			pv.value = i * 5;
//			pt.power.Add(pv);
//		}
//		GlobalData.unitValue .Add(3,new PowerTableInfo(pt)) ;
//	}

	void GenerateUnitBaseHP () {
		PowerTable pt = new PowerTable();
		for (int i = 1; i < 100; i++) {
			PowerValue pv = new PowerValue();
			pv.level = i;
			pv.value = i * 50;
			pt.power.Add(pv);
		}
		GlobalData.unitValue.Add(3,new PowerTableInfo(pt)) ;
	}

	void GenerateUnitBaseInfo() {
		UnitBaseInfo ubi = new UnitBaseInfo ();
		ubi.assetID = 181;
		ubi.chineseName = "di lao ju ren s5";
		ubi.englishName = "Dungeontitan";
		ubi.spriteName = "role016";

		ubi.race = EUnitRace.MYTHIC;
		ubi.starLevel = 3;
		ubi.type = EUnitType.UFIRE;
		ubi.hp = 52;
		ubi.attack = 57;
		ubi.cost = 2;
		ubi.maxLv = 5;
		ubi.maxExp = 632;
		ubi.expType = 3;
		ubi.strengthenExp = 100;
		ubi.saleCoin = 50;
		ubi.normalSkill1 = 0;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 111;
		ubi.chineseName = "ju zhang yu s5";
		ubi.englishName = "Kraken";
		ubi.spriteName = "role006";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 3;
		ubi.type = EUnitType.UWATER;
		ubi.hp = 62;
		ubi.attack = 54;
		ubi.cost = 2;
		ubi.maxLv = 5;
		ubi.maxExp = 632;
		ubi.expType = 3;
		ubi.strengthenExp = 100;
		ubi.saleCoin = 50;
		ubi.normalSkill1 = 2;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 185;
		ubi.chineseName = "jiu tou se s5";
		ubi.englishName = "Hydra";
		ubi.spriteName = "role017";
		ubi.race = EUnitRace.MYTHIC;
		ubi.starLevel = 3;
		ubi.type = EUnitType.UWIND;
		ubi.hp = 67;
		ubi.attack = 32;
		ubi.cost = 2;
		ubi.maxLv = 5;
		ubi.maxExp = 632;
		ubi.expType = 3;
		ubi.strengthenExp = 100;
		ubi.saleCoin = 50;
		ubi.normalSkill1 = 4;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

	}

	void Generate2 () {
		UnitBaseInfo ubi = new UnitBaseInfo ();
		ubi.assetID = 85;
		ubi.chineseName = "du yan guai s5";
		ubi.englishName = "One-eyed warrior";
		ubi.spriteName = "role027";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UFIRE;
		ubi.hp = 47;
		ubi.attack = 43;
		ubi.cost = 1;
		ubi.maxLv = 10;
		ubi.maxExp = 3195;
		ubi.expType = 2;
		ubi.strengthenExp = 50;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 0;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 80;
		ubi.chineseName = "ku lou shi bing2";
		ubi.englishName = "Skeleton warrior";
		ubi.spriteName = "role022";
		ubi.race = EUnitRace.UNDEAD;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWATER;
		ubi.hp = 45;
		ubi.attack = 40;
		ubi.cost = 1;
		ubi.maxLv = 10;
		ubi.maxExp = 3195;
		ubi.expType = 2;
		ubi.strengthenExp = 50;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 2;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 161;
		ubi.chineseName = "shi lai mu s1";
		ubi.englishName = "Slime";
		ubi.spriteName = "role011";
		ubi.race = EUnitRace.MYTHIC;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWIND;
		ubi.hp = 43;
		ubi.attack = 39;
		ubi.cost = 1;
		ubi.maxLv = 10;
		ubi.maxExp = 3195;
		ubi.expType = 2;
		ubi.strengthenExp = 50;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 4;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 87;
		ubi.chineseName = "xue guai s1";
		ubi.englishName = "Snowman";
		ubi.spriteName = "role002";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWATER;
		ubi.hp = 43;
		ubi.attack = 32;
		ubi.cost = 2;
		ubi.maxLv = 10;
		ubi.maxExp = 3195;
		ubi.expType = 2;
		ubi.strengthenExp = 75;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 2;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);
	}

	void Generate3 () {
		UnitBaseInfo ubi = new UnitBaseInfo ();
		ubi.assetID = 101;
		ubi.chineseName = "cang yin ren s3";
		ubi.englishName = "Flyman";
		ubi.spriteName = "role005";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWIND;
		ubi.hp = 43;
		ubi.attack = 32;
		ubi.cost = 2;
		ubi.maxLv = 10;
		ubi.maxExp = 3195;
		ubi.expType = 2;
		ubi.strengthenExp = 75;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 4;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 122;
		ubi.chineseName = "yu ren s2";
		ubi.englishName = "Gillman";
		ubi.spriteName = "role009";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWATER;
		ubi.hp = 68;
		ubi.attack = 65;
		ubi.cost = 2;
		ubi.maxLv = 10;
		ubi.maxExp = 4793;
		ubi.expType = 3;
		ubi.strengthenExp = 75;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 2;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 195;
		ubi.chineseName = "xiao gui s3";
		ubi.englishName = "Gremlin";
		ubi.spriteName = "role010";
		ubi.race = EUnitRace.MYTHIC;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWIND;
		ubi.hp = 72;
		ubi.attack = 67;
		ubi.cost = 2;
		ubi.maxLv = 10;
		ubi.maxExp = 4793;
		ubi.expType = 3;
		ubi.strengthenExp = 75;
		ubi.saleCoin = 10;
		ubi.normalSkill1 = 4;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);

		ubi = new UnitBaseInfo ();
		ubi.assetID = 89;
		ubi.chineseName = "fei tian he ma s3";
		ubi.englishName = "Flying hippo";
		ubi.spriteName = "role004";
		ubi.race = EUnitRace.MONSTER;
		ubi.starLevel = 1;
		ubi.type = EUnitType.UWIND;
		ubi.hp = 136;
		ubi.attack = 105;
		ubi.cost = 4;
		ubi.maxLv = 25;
		ubi.maxExp = 14463;
		ubi.expType = 1;
		ubi.strengthenExp = 150;
		ubi.saleCoin = 60;
		ubi.normalSkill1 = 30;
		ubi.normalSkill2 = -1;
		GlobalData.tempUnitBaseInfo.Add (ubi.assetID, ubi);
	}

}

public class PowerTableInfo : ProtobufDataBase {
	public PowerTableInfo(object instance) : base (instance) {
		pt = instance as PowerTable;
	}
	PowerTable pt;
	public int GetValue (int level) {
		PowerValue pv = pt.power.Find(a=>a.level == level);
		if(pv == default(PowerValue)) {
			return -1;
		}
		else{
			return pv.value;
		}
	}
}

public class UnitBaseInfo {
	private const string headPath= "Avatar/";
	private const string rolePath= "Role/";
	public string GetHeadPath {
		get{
			return headPath + spriteName;
		}
	}
	public string GetRolePath {
		get{
			return rolePath + spriteName;
		}
	}

	public int id;
	public int assetID ;
	public string chineseName;
	public string englishName;
	public string spriteName;
	public string spriteHeadName;
	public EUnitRace race;
	public int starLevel ;
	public EUnitType type;
	public int hp;
	public int attack;
	public int cost;
	public int maxLv ;
	public int maxExp;
	public int expType ;
	public int strengthenExp;
	public int saleCoin;
	public int normalSkill1;
	public int normalSkill2;
}