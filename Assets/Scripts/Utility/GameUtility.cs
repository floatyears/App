using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using bbproto;

public class DGTools {
	public static string[] stateInfo = new string[] {"PlayerPhase","EnemyPhase","NormalSkill","PassiveSkill","FirstAttack","BackAttack" };

	public static int RandomToInt(int min,int max) {
		return UnityEngine.Random.Range(min,max);
	}
	/// <summary>
	/// return a float number between 0 and 1.
	/// </summary>
	/// <returns>The to float.</returns>
	public static float RandomToFloat() {
		return UnityEngine.Random.Range (0f, 1f);
	}

	public static bool CheckCooling(SkillBase sb) {
		if (sb.skillCooling == 0) {
			return true;
		}
		sb.skillCooling --;
		if (sb.skillCooling == 0) {
			return true;
		} 
		else {
			return false;
		}
	}

	/// <summary>
	/// show sprite. and set the size to texture size;
	/// </summary>
	/// <param name="sprite">Sprite.</param>
	/// <param name="name">Name.</param>
	public static void ShowSprite(UISprite sprite, string name) {
		sprite.spriteName = name;
		UIAtlas atlas = sprite.atlas;
		UISpriteData sd = sprite.GetAtlasSprite ();
		sprite.width = sd.width;
		sprite.height = sd.height;
		sprite.spriteName = name;
	}

	public static string SwitchUnitType (int unitType) {
		string type = "";
		switch (unitType) {
		case 1:
			type = "UFire";
			break;
		case 2:
			type = "UWATER";
			break;
		case 3:
			type = "UWIND";
			break;
		case 4:
			type = "ULIGHT";
			break;
		case 5:
			type = "UDARK";
			break;
		case 6:
			type = "UNONE";
			break;
		}

		return type;
	}

	public static int RestraintType (int unitType) {
		switch (unitType) {
		case 1:
			return 3;
		case 2:
			return 1;
		case 3:
			return 2;
		case 4:
			return 5;
		case 5:
			return 4;
		default :
			return -1;
		}
	}

	public static int BeRestraintType (int unitType) {
		switch (unitType) {
		case 1:
			return 2;
		case 2:
			return 3;
		case 3:
			return 1;
		case 4:
			return 5;
		case 5:
			return 4;
		default :
			return -1;
		}
	}

	public static bool ListContains<T>(List<T> big, List<T> small) {
		if (big.Count < small.Count) {
			return false;
		}
						
		for (int i = 0; i < small.Count; i++) {
			if(!big.Contains(small[i])) {
				return false;
			}
		}
		return true;
	}

	public static void PlayAttackSound(int attackType) {
//		Debug.LogError ("PlayAttackSound : " + attackType);
		switch (attackType) {
		case 1:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_fire_attack);
			break;
		case 2:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_water_attack);
			break;
		case 3:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_wind_attack);
			break;
		case 4:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_light_attack);
			break;
		case 5:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_dark_attack);
			break;
		case 6:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_zero_attack);
			break;
		case 7:
			AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
			break;
		}
	}

	public static bool IsTriggerSkill<T> (List<T> cardList, List<T> skillList) where T : struct {
		if (cardList.Count < skillList.Count) {
			return false;		
		}
		List<T> tempCard = new List<T> (cardList);
		List<T> tempSkillList = new List<T> (skillList);

		for (int i = 0; i < tempSkillList.Count; i++) {
			if(tempCard.Contains(tempSkillList[i])) {
				T value = tempSkillList[i];
				tempCard.Remove(value);
			}
			else  {
				return false;
			}
		}

		return true;
	}

	public static bool IsFirstBoost<T>(IList<T> first, IList<T> second) {
		if (first.Count < second.Count) {
			return true;
		}
		else {
			return false;
		}
	}

	public static int CaculateAddBlood (int addHP,UserUnit uu, UnitInfo ui) {
		return addHP * 10 + GetValue (uu, ui);
	}

	public static int CaculateAddAttack (int addAttack, UserUnit uu, UnitInfo ui) {
		return addAttack * 5 + GetValue (uu, ui);
	}

	public static int CaculateAddDefense (int add, UserUnit uu, UnitInfo ui) {
		return add * 10 + GetValue (uu, ui);
	}

	static int GetValue (UserUnit uu, UnitInfo ui) {
		return DataCenter.Instance.GetUnitValue(ui.powerType.attackType,uu.level);
	}

	public static string GetNormalSkillSpriteName (AttackInfo ai) {
		if (ai.FixRecoverHP) {
			return "7_1";
		}
		string name1 = ai.AttackType.ToString ();
		string name2 = ai.AttackRange.ToString ();
		return name1 + "_" + name2;
	}



	public static float IntegerSubtriction(int firstInterger,int secondInterger) {
		return (float)firstInterger / (float)secondInterger;
	}

	/// <summary>
	/// Inserts the sort.
	/// </summary>
	/// <param name="target">Target collections.</param>
	/// <param name="sort">Sort is bool. True is Descending and False is Ascending.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void InsertSort<T,T1> (IList<T> target, T1 compareObject, bool sort = true) where T1 :  IComparer {
		if (target == null) {
			return;
		}
		int length = target.Count;
		for (int i = 1; i < length; i++) {
			//T temp = target[i];
			for (int j = 0; j < i; j++) {
				int compare = compareObject.Compare(target[i], target[j]);
				if(sort && compare > 0) {
					T temp = target[i];
					target[i] = target[j];
					target[j] = temp;

					continue;
				}
		
				if(!sort && compare < 0) {
					T temp = target[i];
					target[i] = target[j];
					target[j] = temp;
				}
			}
		}
	}

	public static void InsertSortBySequence<T,T1> (IList<T> target, T1 compareObject, bool sort = true) where T1 :  IComparer {
		if (target == null) {
			return;
		}
		int length = target.Count;
		for (int i = 1; i < length; i++) {
			for (int j = 0; j < i; j++) {
				int compare = compareObject.Compare(target[i], target[j]);
				if(sort && compare > 0) {
					T temp = target[j];
					target[j] = target[i];
					int k = j + 1;
					T temp1 ;
					while(k <= i) {
						temp1 = target[k];
						target[k] = temp;
						temp = temp1;
						k++;
					}
					continue;
				}
				
				if(!sort && compare < 0) {
					T temp = target[i];
					target[i] = target[j];
					target[j] = temp;
				}
			}
		}
	}

	public static void SwitchObject<T>(ref T arg1,ref T arg2) {
		T temp = arg1;
		arg1 = arg2;
		arg2 = temp;
	}

	public static bool IsOddNumber(int number) {
		return System.Convert.ToBoolean (number & 1);
	}

	public static float TypeMultiple (TUserUnit baseUnit, TUserUnit friend) {
		if (baseUnit.UnitInfo.Type == friend.UnitInfo.Type) {
			return 1.25f;	
		} else {
			return 1f;
		}
	}
	
	public static float RaceMultiple (TUserUnit baseUnit, TUserUnit friend) {
		if (baseUnit.UnitInfo.Race == friend.UnitInfo.Race) {
			return 1.25f;
		} else {
			return 1f;
		}
	}
	
	public static float AllMultiple (float type, float race) {
		return type + race - 1;
	}

	public static float AllMultiple (TUserUnit baseUnit, TUserUnit friend) {
		return AllMultiple (TypeMultiple (baseUnit, friend), RaceMultiple (baseUnit, friend));
	}

	private const string path = "Protobuf/";
	private const string unitInfoPath = "Unit/";
	public static TUnitInfo LoadUnitInfoProtobuf(uint unitID) {
		string url = path +unitInfoPath + unitID;
		TextAsset ta = LoadTextAsset (url);
		UnitInfo ui = ProtobufSerializer.ParseFormBytes<UnitInfo> (ta.bytes);
		TUnitInfo tui = new TUnitInfo (ui);
		return tui;
	}
	private const string CityPath = "City/";
	public static TCityInfo LoadCityInfo (uint cityID) {
		string url = path + CityPath + cityID;
		TextAsset ta = LoadTextAsset (url);
		CityInfo ci = ProtobufSerializer.ParseFormBytes<CityInfo> (ta.bytes);
		TCityInfo tci = new TCityInfo(ci);
		return tci;
	}

	static TextAsset LoadTextAsset (string url) {
		return Resources.Load (url, typeof(TextAsset)) as TextAsset;
	}

	private static Dictionary<EUnitType, Color> typeColor = new Dictionary<EUnitType, Color> ();
	public static Color TypeToColor (EUnitType type) {
		Color value = new Color();
		if (!typeColor.TryGetValue (type, out value)) {
			value = Generate(type);
			typeColor.Add(type, value);
		}
		return value;
	}

	private static Color Generate(EUnitType type) {
		switch (type) {
		case EUnitType.UFIRE:
			return new Color(171.0f/ 255, 73.0f / 255, 82.0f / 255, 1f);
		case EUnitType.UWATER:
			return new Color(73.0f / 255, 152.0f / 255, 171.0f / 255, 1f);
		case EUnitType.UWIND:
			return new Color(77.0f / 255, 167.0f / 255,120.0f / 255, 1f);
		case EUnitType.UHeart:
			return new Color(77.0f / 255,167.0f / 255, 120.0f / 255, 1f);
		case EUnitType.ULIGHT:
			return new Color(214.0f / 255, 213.0f / 255, 159.0f / 255, 1f);
		case EUnitType.UDARK:
			return new Color(161.0f / 255, 73.0f / 255, 171.0f / 255, 1f);
		case EUnitType.UNONE:
			return new Color(122.0f / 255, 122.0f / 255, 122.0f / 255,1f);
		case EUnitType.UALL:
		default:
			return new Color(0,0,0,1f);
		}
	}
}

public class GameLayer
{

//	private static LayerMask Default = 0;
	
	public static LayerMask TransparentFX = 1;
	
	public static LayerMask IgnoreRaycast = 2;
	
	//null = 3,
	
	public static LayerMask Water = 4;
	
	//null = 5,
	
	//null = 6,
	
	//null = 7,
	
	public static LayerMask ActorCard = 8;

	public static LayerMask BattleCard = 9;

	public static LayerMask IgnoreCard = 10;

	public static LayerMask EnemyCard = 11;

	public static LayerMask Bottom = 31;

	public static int LayerToInt(LayerMask layer)
	{
		return 1 << layer;
	}
}