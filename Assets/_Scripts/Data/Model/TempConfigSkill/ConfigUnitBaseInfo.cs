using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using bbproto;

public class ConfigUnitBaseInfo {

    public ConfigUnitBaseInfo() {
//        GenerateUnitBaseInfo();
//        Generate2();
//        Generate3();

//        GenerateUnitBaseExp();
//        GenerateUnitBaseAttack();
//		GenerateUnitBaseDefense();
//        GenerateUnitBaseHP();
        GenerateUserMaxCost();
        GenerateUserExp();
    }
    // exp attack defense hp
//    void GenerateUnitBaseExp() {
//        PowerTable pt = new PowerTable();
//        for (int i = 0; i < 99; i++) {
//            PowerValue pv = new PowerValue();
//            pv.level = i + 1;
//			pv.value = 100 * ((i+1)*(i+1)/3);
//			if (pv.value==0)
//				pv.value = 10;
//            pt.power.Add(pv);
//        }
////        TPowerTableInfo tbi = new TPowerTableInfo(pt);
////		if (DataCenter.Instance.UnitData.UnitValue.ContainsKey(TPowerTableInfo.UnitInfoExpType1)) {
////			DataCenter.Instance.UnitData.UnitValue[TPowerTableInfo.UnitInfoExpType1] = tbi;
////		}
////		else {
////			DataCenter.Instance.UnitData.UnitValue.Add(TPowerTableInfo.UnitInfoExpType1, tbi);
////		}
//        
//    }

//    void GenerateUnitBaseAttack() {
////        PowerTable pt = new PowerTable();
////        for (int i = 1; i < 100; i++) {
////            PowerValue pv = new PowerValue();
////            pv.level = i;
////			pv.value = 57 + (i - 1) * 4;
////            pt.power.Add(pv);
////        }
////        TPowerTableInfo tbi = new TPowerTableInfo(pt);
////
////		if (DataCenter.Instance.UnitData.UnitValue.ContainsKey(TPowerTableInfo.UnitInfoAttackType1)) {
////			DataCenter.Instance.UnitData.UnitValue[TPowerTableInfo.UnitInfoAttackType1] = tbi;
////		}
////		else {
////			DataCenter.Instance.UnitData.UnitValue.Add(TPowerTableInfo.UnitInfoAttackType1, tbi);
////		}
//
////        DataCenter.Instance.UnitData.UnitValue.Add(TPowerTableInfo.UnitInfoAttackType1, tbi); 
//    }

//    void GenerateUnitBaseHP() {
////        PowerTable pt = new PowerTable();
////        for (int i = 1; i < 100; i++) {
////            PowerValue pv = new PowerValue();
////            pv.level = i;
////			pv.value = 52 + (i - 1) * 20;
////            pt.power.Add(pv);
////        }
////        TPowerTableInfo tbi = new TPowerTableInfo(pt);
////
////		if (DataCenter.Instance.UnitData.UnitValue.ContainsKey (TPowerTableInfo.UnitInfoHPType1)) {
////			DataCenter.Instance.UnitData.UnitValue[TPowerTableInfo.UnitInfoHPType1] = tbi;
////		}
////		else {
////			DataCenter.Instance.UnitData.UnitValue.Add(TPowerTableInfo.UnitInfoHPType1, tbi);
////		}
//
//    }

    void GenerateUserMaxCost() {
        PowerTable pt = new PowerTable();
		int[] TableCostMax = new int[500]{
			20,22,22,22,24,24,24,26,26,26,28,28,28,30,30,30,32,32,32,37,37,37,39,39,39,41,41,41,43,43,
			43,45,45,45,47,47,47,49,49,49,51,51,51,53,53,53,55,55,55,60,60,60,62,62,62,64,64,64,66,66,
			66,68,68,68,70,70,70,72,72,77,77,79,79,81,81,83,83,85,85,90,90,92,92,94,94,96,96,98,98,103,
			103,105,105,107,107,109,109,111,111,116,116,118,118,120,120,122,122,124,124,126,126,128,128,
			130,130,132,132,134,134,136,136,138,138,140,140,142,142,144,144,146,146,148,148,150,150,152,
			152,154,154,156,156,158,158,160,160,162,162,164,164,166,166,168,168,170,170,172,172,174,174,
			176,176,178,178,180,180,182,182,184,184,186,186,188,188,190,190,192,192,194,194,196,196,198,
			198,200,200,202,202,204,204,206,206,208,208,210,210,212,212,214,214,216,216,218,218,220,220,
			222,222,224,224,226,226,228,228,230,230,232,232,234,234,236,236,238,238,240,240,242,242,244,
			244,246,246,248,248,250,250,252,252,254,254,256,256,258,258,260,260,262,262,264,264,266,266,
			268,268,270,270,272,272,274,274,276,276,278,278,280,280,282,282,284,284,286,286,288,288,290,
			290,292,292,294,294,296,296,298,298,300,300,302,302,304,304,306,306,308,308,310,310,312,312,314,314,316,316,318,318,
			320,320,322,322,324,324,326,326,328,328,330,330,332,332,334,334,336,336,338,338,340,340,342,342,344,344,346,346,348,
			348,350,350,352,352,354,354,356,356,358,358,360,360,362,362,364,364,366,366,368,368,370,370,372,372,374,374,376,376,
			378,378,380,380,382,382,384,384,386,386,388,388,390,390,392,392,394,394,396,396,398,398,400,400,402,402,404,404,406,
			406,408,408,410,410,412,412,414,414,416,416,418,418,420,420,422,422,424,424,426,426,428,428,430,430,432,432,434,434,
			436,436,438,438,440,440,442,442,444,444,446,446,448,448,450,450,452,452,454,454,456,456,458,458,460,460,462,462,464,
			464,466,466,468,468,470,470,472,472,474,474,476,476,478,478,480,480,482,482,484,484,486,486,488,488,490,490,492,492,
			494,494,496,496,498,498,500,500,502,502,504,504,506,506,508,508,510,510,512,512,514,514,516
		};//500 ranks

		for (int i = 0; i < 500; i++) {
            PowerValue pv = new PowerValue();
            pv.level = i+1;
			pv.value = TableCostMax[i];
            pt.power.Add(pv);
        }
		PowerTable tbi = pt;

		Dictionary<int,PowerTable> infoList = new Dictionary<int, PowerTable> ();
		if (infoList.ContainsKey (PowerTable.UserCostMax)) {
			infoList[PowerTable.UserCostMax] = tbi;
		}
		else {
			infoList.Add(PowerTable.UserCostMax, tbi);
		}

		DataCenter.Instance.SetData (ModelEnum.UnitValue, infoList);
    }

    void GenerateUserExp() {
        PowerTable pt = new PowerTable();
		int[] TableUserRankExp = new int[500] {
			0,12,17,28,28,48,67,86,115,172,185,264,288,333,367,393,435,478,526,548,602,651,680,728,772,815,869,890,937,985,1034,
			1084,1133,1184,1235,1287,1340,1393,1447,1501,1556,1611,1667,1724,1781,1838,1896,1955,2014,2074,2134,2194,2255,2317,
			2379,2441,2504,2568,2631,2696,2760,2826,2891,2957,3024,3090,3158,3225,3294,3362,3431,3500,3570,3640,3711,3781,3852,
			3924,3996,4068,4141,4214,4288,4362,4436,4510,4585,4661,4736,4812,4888,4965,5042,5119,5197,5275,5353,5432,5511,5590,
			5670,5750,5830,5910,5991,6072,6154,6236,6318,6400,6483,6566,6649,6733,6817,6901,6986,7070,7156,7241,7327,7413,7499,
			7585,7672,7759,7847,7934,8022,8111,8199,8288,8377,8466,8556,8646,8736,8826,8917,9008,9099,9190,9282,9374,9466,9559,
			9651,9744,9838,9931,10025,10119,10213,10308,10402,10497,10593,10688,10784,10880,10976,11072,11173,11262,11363,11461,
			11558,11656,11754,11853,11951,12050,12149,12248,12348,12448,12548,12648,12748,12849,12950,13051,13152,13254,13384,
			13429,13560,13662,13765,13868,13971,14074,14178,14282,14385,14490,14594,14699,14804,14909,15014,15119,15225,15331,
			15437,15543,15650,15757,15863,15970,16078,16185,16293,16401,16509,16618,16726,16835,16944,17053,17162,17272,17382,
			17492,17602,17712,17823,17934,18044,18155,18267,18378,18490,18602,18714,18826,18939,19052,19165,19278,19391,19504,
			19618,19732,19846,19960,20074,20189,20304,20419,20534,20649,20764,20880,20996,21113,21228,21345,21461,21578,21695,
			21812,21929,22047,22165,22282,22401,22519,22637,22756,22874,22993,23112,23232,23351,23471,23590,23710,23831,23951,
			24071,24192,24313,24434,24555,24676,24798,24919,25041,25163,25286,25408,25530,25653,25776,25899,26022,26146,26269,
			26393,26436,26517,26641,53654,27014,27139,27263,27388,27514,27639,27765,27890,28016,28142,28268,28394,28521,28647,
			28774,28901,29028,29156,29283,29411,29538,29666,29794,29923,30051,30180,30308,30437,30566,30695,30825,30954,31084,
			31214,31343,31473,31604,31734,31865,31995,32126,32257,32388,32520,32651,32783,32915,33047,33179,33311,33443,33576,
			33708,33841,33974,34107,34241,34374,34508,34641,34775,34909,35043,35178,35312,35447,35581,35716,35851,35986,36122,
			36218,36315,36411,36508,36604,36700,36797,36893,36990,37086,37183,37279,37375,37472,37568,37665,37761,37858,37954,
			38050,38147,38243,38340,38436,38533,38629,38725,38822,38918,39015,39111,39208,39304,39400,39497,39593,39690,39786,
			39883,39979,40075,40172,40268,40365,40461,40558,40654,40750,40847,40943,41040,41136,41233,41329,41425,41522,41618,
			41715,41811,41908,42004,42100,42197,42293,42390,42486,42583,42679,42775,42872,42968,43065,43161,43258,43354,43450,
			43547,43643,43740,43836,43933,44029,44125,44222,44318,44415,44511,44608,44704,44800,44897,44993,45090,45186,45283,
			45379,45475,45572,45668,45765,45861,45958,46054,46150,46247,46343,46440,46536,46633,46729,46825,46922,47018,47115,
			47211,47308,47404,47500,47597,47693,47790,47886,47983,48079,48175
		};

        for (int i = 0; i < 500; i++) {
            PowerValue pv = new PowerValue();
            pv.level = i + 1;
			pv.value = TableUserRankExp[i];
            pt.power.Add(pv);
        }
		PowerTable tbi = pt;

		if (DataCenter.Instance.UnitData.UnitValue.ContainsKey (PowerTable.UserExpType)) {
			DataCenter.Instance.UnitData.UnitValue[PowerTable.UserExpType] = tbi;
		}
		else {
			DataCenter.Instance.UnitData.UnitValue.Add(PowerTable.UserExpType, tbi);
		}

//        DataCenter.Instance.UnitData.UnitValue.Add(TPowerTableInfo.UserExpType, tbi);
    }

//    void GenerateUnitBaseInfo() {
//        UnitBaseInfo ubi = new UnitBaseInfo();
//        ubi.assetID = 181;
//        ubi.chineseName = "di lao ju ren s5";
//        ubi.englishName = "Dungeontitan";
//        ubi.spriteName = "16_1";
//
//        ubi.race = EUnitRace.MYTHIC;
//        ubi.starLevel = 3;
//        ubi.type = EUnitType.UFIRE;
//        ubi.hp = 52;
//        ubi.attack = 57;
//        ubi.cost = 2;
//        ubi.maxLv = 5;
//        ubi.maxExp = 632;
//        ubi.expType = 3;
//        ubi.strengthenExp = 100;
//        ubi.saleCoin = 50;
//        ubi.normalSkill1 = 0;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 111;
//        ubi.chineseName = "ju zhang yu s5";
//        ubi.englishName = "Kraken";
//        ubi.spriteName = "6_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 3;
//        ubi.type = EUnitType.UWATER;
//        ubi.hp = 62;
//        ubi.attack = 54;
//        ubi.cost = 2;
//        ubi.maxLv = 5;
//        ubi.maxExp = 632;
//        ubi.expType = 3;
//        ubi.strengthenExp = 100;
//        ubi.saleCoin = 50;
//        ubi.normalSkill1 = 2;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 185;
//        ubi.chineseName = "jiu tou se s5";
//        ubi.englishName = "Hydra";
//        ubi.spriteName = "17_1";
//        ubi.race = EUnitRace.MYTHIC;
//        ubi.starLevel = 3;
//        ubi.type = EUnitType.UWIND;
//        ubi.hp = 67;
//        ubi.attack = 32;
//        ubi.cost = 2;
//        ubi.maxLv = 5;
//        ubi.maxExp = 632;
//        ubi.expType = 3;
//        ubi.strengthenExp = 100;
//        ubi.saleCoin = 50;
//        ubi.normalSkill1 = 4;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//    }
//
//    void Generate2() {
//        UnitBaseInfo ubi = new UnitBaseInfo();
//        ubi.assetID = 85;
//        ubi.chineseName = "du yan guai s5";
//        ubi.englishName = "One-eyed warrior";
//        ubi.spriteName = "21_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UFIRE;
//        ubi.hp = 47;
//        ubi.attack = 43;
//        ubi.cost = 1;
//        ubi.maxLv = 10;
//        ubi.maxExp = 3195;
//        ubi.expType = 2;
//        ubi.strengthenExp = 50;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 0;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 80;
//        ubi.chineseName = "ku lou shi bing2";
//        ubi.englishName = "Skeleton warrior";
//        ubi.spriteName = "22_1";
//        ubi.race = EUnitRace.UNDEAD;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWATER;
//        ubi.hp = 45;
//        ubi.attack = 40;
//        ubi.cost = 1;
//        ubi.maxLv = 10;
//        ubi.maxExp = 3195;
//        ubi.expType = 2;
//        ubi.strengthenExp = 50;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 2;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 161;
//        ubi.chineseName = "shi lai mu s1";
//        ubi.englishName = "Slime";
//        ubi.spriteName = "11_1";
//        ubi.race = EUnitRace.MYTHIC;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWIND;
//        ubi.hp = 43;
//        ubi.attack = 39;
//        ubi.cost = 1;
//        ubi.maxLv = 10;
//        ubi.maxExp = 3195;
//        ubi.expType = 2;
//        ubi.strengthenExp = 50;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 4;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 87;
//        ubi.chineseName = "xue guai s1";
//        ubi.englishName = "Snowman";
//        ubi.spriteName = "2_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWATER;
//        ubi.hp = 43;
//        ubi.attack = 32;
//        ubi.cost = 2;
//        ubi.maxLv = 10;
//        ubi.maxExp = 3195;
//        ubi.expType = 2;
//        ubi.strengthenExp = 75;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 2;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//    }
//
//    void Generate3() {
//        UnitBaseInfo ubi = new UnitBaseInfo();
//        ubi.assetID = 101;
//        ubi.chineseName = "cang yin ren s3";
//        ubi.englishName = "Flyman";
//        ubi.spriteName = "5_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWIND;
//        ubi.hp = 43;
//        ubi.attack = 32;
//        ubi.cost = 2;
//        ubi.maxLv = 10;
//        ubi.maxExp = 3195;
//        ubi.expType = 2;
//        ubi.strengthenExp = 75;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 4;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 122;
//        ubi.chineseName = "yu ren s2";
//        ubi.englishName = "Gillman";
//        ubi.spriteName = "9_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWATER;
//        ubi.hp = 68;
//        ubi.attack = 65;
//        ubi.cost = 2;
//        ubi.maxLv = 10;
//        ubi.maxExp = 4793;
//        ubi.expType = 3;
//        ubi.strengthenExp = 75;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 2;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 195;
//        ubi.chineseName = "xiao gui s3";
//        ubi.englishName = "Gremlin";
//        ubi.spriteName = "10_1";
//        ubi.race = EUnitRace.MYTHIC;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWIND;
//        ubi.hp = 72;
//        ubi.attack = 67;
//        ubi.cost = 2;
//        ubi.maxLv = 10;
//        ubi.maxExp = 4793;
//        ubi.expType = 3;
//        ubi.strengthenExp = 75;
//        ubi.saleCoin = 10;
//        ubi.normalSkill1 = 4;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//
//        ubi = new UnitBaseInfo();
//        ubi.assetID = 89;
//        ubi.chineseName = "fei tian he ma s3";
//        ubi.englishName = "Flying hippo";
//        ubi.spriteName = "4_1";
//        ubi.race = EUnitRace.MONSTER;
//        ubi.starLevel = 1;
//        ubi.type = EUnitType.UWIND;
//        ubi.hp = 136;
//        ubi.attack = 105;
//        ubi.cost = 4;
//        ubi.maxLv = 25;
//        ubi.maxExp = 14463;
//        ubi.expType = 1;
//        ubi.strengthenExp = 150;
//        ubi.saleCoin = 60;
//        ubi.normalSkill1 = 30;
//        ubi.normalSkill2 = -1;
//        DataCenter.Instance.UnitBaseInfo.Add(ubi.assetID, ubi);
//    }

}

//public class TPowerTableInfo : ProtobufDataBase {
//	public TPowerTableInfo(object instance) : base (instance) {
//		pt = instance as PowerTable;
//	}
//	PowerTable pt;
//	public int GetValue (int level) {
//		PowerValue pv = pt.power.Find(a=>a.level == level);
//		if(pv == default(PowerValue)) {
//			return -1;
//		}
//		else{
//			return pv.value;
//		}
//	}
//}

public class UnitBaseInfo {
    private const string headPath = "Avatar/";
    private const string rolePath = "Profile/";
    public string GetHeadPath {
        get {
            return headPath + spriteName;
        }
    }
    public string GetRolePath {
        get {
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