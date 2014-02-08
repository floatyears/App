using UnityEngine;
using System.Collections;

public class ShowUnitInfo {
	public ShowUnitInfo() {
		MsgCenter.Instance.AddListener (CommandEnum.EnterUnitInfo, EnterUnitInfo);
	}

	~ShowUnitInfo () {
		MsgCenter.Instance.RemoveListener (CommandEnum.EnterUnitInfo, EnterUnitInfo);
	}
	private static UnitBaseInfo currentDetail;

	public static string roleSpriteName = "";
	public static UserUnitInfo uui;
	public static int unitID = -1;
	public static string unitName = "";
	public static int level = -1;
	public static string unitType = "";
	public static string race = string.Empty;
	public static int hp = -1;
	public static int cost = -1;
	private static int rare = -1;
	public static string Rare{
		get {
			return rare + " star";
		}
	}
	public static int attack = -1;
	public static float experenceProgress = 0f; 
	public static string normalSkill_1_Name = " ";
	public static string normalSkill_2_Name = " ";

	void EnterUnitInfo (object data) {
		currentDetail = data as UnitBaseInfo;
		if (currentDetail == null) {
			DisposeUnitInfo (data);
		} 
		else {
			DisposeBaseInfo (currentDetail);
		}
		UIManager.Instance.ChangeScene (SceneEnum.UnitDetail);
	}

	void DisposeBaseInfo (UnitBaseInfo ubi) {
		roleSpriteName = ubi.GetRolePath;
		unitID = ubi.assetID;
		unitName = ubi.englishName;
		level = 1;
		unitType = ubi.type.ToString ();
		race = ubi.race.ToString ();
		hp = ubi.hp;
		cost = ubi.cost;
		rare = ubi.starLevel;
		attack = ubi.attack;
		experenceProgress = 0f;
	}

	void DisposeUnitInfo (object data) {
		UserUnitInfo uui = data as UserUnitInfo;
		if (uui == null) {
			return;	
		}
		UnitBaseInfo ubi = GlobalData.tempUnitBaseInfo [uui.unitBaseInfo];
		roleSpriteName = ubi.GetRolePath;
		unitID = ubi.assetID;
		unitName = ubi.englishName;
		level = uui.GetLevel;
		unitType = ubi.type.ToString ();
		race = ubi.race.ToString ();
		hp = uui.GetBlood ();
		cost = ubi.cost;
		rare = ubi.starLevel;
		attack = uui.GetAttack;
		experenceProgress = 0f;

//		TempNormalSkill tns1 = GlobalData.tempNormalSkill[ uui. ] as TempNormalSkill;
//		normalSkill_1_Name = tns1.GetName();
//	
//		TempNormalSkill tns2 = GlobalData.tempNormalSkill[ ubi.normalSkill2 ] as TempNormalSkill;
//		normalSkill_2_Name = tns2.GetName();

	}
}
