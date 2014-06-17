using UnityEngine;
using System.Collections.Generic;

public class EffectConstValue {
	private static  EffectConstValue instance;
	public static EffectConstValue Instance {
		get {
			if(instance == null) {
				instance = new EffectConstValue();
			}
			return instance;
		}
	}




	private Dictionary<string,GameObject> effectAsset = new Dictionary<string, GameObject> ();
	public const string assetPath	= "Effect/";
	//---------------------effct asset name-------------------
	public const string Fire1		= "fire1";
	public const string Fire2		= "fire2";
	public const string Explosion	= "BOOM";
	public const string KnifeLight	= "daoguang";
	public const string MeetEnemy	= "Enconuterenemy";
	public const string PicksHit1	= "ice1";
	public const string PicksHit2	= "ice2";
	public const string WindHit1	= "wind1";
	public const string WindHit2	= "wind2";
	public const string PawHit		= "zhua";
	public const string MeetTrap	= "trap";
	public const string EatCard1	= "linhunqiu1";
	public const string EatCard2	= "linhunqiu2";
	public const string Upgrade		= "Upgrade";
	public const string fireRain	= "firerain";
	//---------------------end-------------------------------

	//---------------------effct excute name-----------------
	public const string NormalFire1		= "NormalFireAttack1";
	public const string NormalWind1		= "NormalWindAttack1";
	public const string NormalWater1	= "NormalWaterAttack1";
	public const string NormalKnife		= "NormalKnifeAttack1";
	public const string NormalMelee		= "NormalMeleeAttack1";
	public const string TriggerTrap		= "TriggerTrap";
	public const string FireAll			= "firerain";

	//---------------------end-------------------------------
	public List<GameObject> GetEffect(AttackInfo ai) {
		if (ai.AttackRange == 0) {
			return SingleAttackAsset (ai.AttackType);	
		} else if (ai.AttackRange == 1) {
			return AllAttackAsset (ai.AttackType);	
		} else {
			return null;
		}


	}

	List<GameObject> SingleAttackAsset (int attackType) {
		List<GameObject> tempList = new List<GameObject> ();
		GameObject tempObject = null;
		string path = "";
		switch (attackType) {
		case 1:
			path = assetPath + Fire1;
			tempList.Add(GetEffect(path));
			path = assetPath + Fire2;
			tempList.Add(GetEffect(path));
			break;
		case 2:
			path = assetPath + PicksHit1;
			tempList.Add(GetEffect(path));
			path = assetPath + PicksHit2;
			tempList.Add(GetEffect(path));
			break;
		case 3:
			path = assetPath + WindHit1;
			tempList.Add(GetEffect(path));
			path = assetPath + WindHit2;
			tempList.Add(GetEffect(path));
			break;
		case 4:

			break;
		case 5:

			break;
		default:

			break;
		}
		return tempList;
	}

	List<GameObject> AllAttackAsset (int attackType) {
		List<GameObject> tempList = new List<GameObject> ();
		GameObject tempObject = null;
		string path = "";
		switch (attackType) {
		case 1:
//			path = assetPath + fireRain;
//			tempList.Add(GetEffect(path));

			path = assetPath + Fire1;
			tempList.Add(GetEffect(path));
			path = assetPath + Fire2;
			tempList.Add(GetEffect(path));
			break;
		case 2:
			path = assetPath + PicksHit1;
			tempList.Add(GetEffect(path));
			path = assetPath + PicksHit2;
			tempList.Add(GetEffect(path));
			break;
		case 3:
			path = assetPath + WindHit1;
			tempList.Add(GetEffect(path));
			path = assetPath + WindHit2;
			tempList.Add(GetEffect(path));
			break;
		case 4:
			break;
		case 5:
			break;
		default:
			break;
		}
		return tempList;
	}

	GameObject GetEffect(string path) {
		GameObject data;
		if (effectAsset.TryGetValue (path, out data)) {
			return data;
		} else {
			data = ResourceManager.Instance.LoadLocalAsset(path, null) as GameObject;
			effectAsset.Add(path,data);
			return data;
		}
	}
}

