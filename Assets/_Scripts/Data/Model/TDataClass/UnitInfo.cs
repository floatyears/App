 using bbproto;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace bbproto{
public partial class UnitInfo : ProtoBuf.IExtensible {
		
	public string UnitRace{
		get{
			switch ( race ){
			case EUnitRace.HUMAN : 
				return TextCenter.GetText("RACE_Human");
				break;
			case EUnitRace.LEGEND : 
				return TextCenter.GetText("RACE_Legend");
				break;
			case EUnitRace.MONSTER : 
				return TextCenter.GetText("RACE_Monster");
				break;
			case EUnitRace.MYTHIC : 
				return TextCenter.GetText("RACE_Mythic");
				break;
			case EUnitRace.BEAST : 
				return TextCenter.GetText("RACE_Beast");
				break;
			case EUnitRace.UNDEAD : 
				return TextCenter.GetText("RACE_Undead");
				break;
			case EUnitRace.SCREAMCHEESE: 
				return TextCenter.GetText("RACE_Screamcheese");
				break;
			case EUnitRace.DRAGON:
				return TextCenter.GetText("RACE_Dragon");
				break;
			case EUnitRace.EVOLVEPARTS:
				return TextCenter.GetText("RACE_Evolveparts");
				break;
			default:
				return string.Empty;
				break;
			}
		}
	}
	
	public string UnitType {
		get{
			switch ( type ){
			case EUnitType.UFIRE : 
				return "Fire";
				break;
			case EUnitType.ULIGHT : 
				return "Light";
				break;
			case EUnitType.UDARK : 
				return "Dark";
				break;
			case EUnitType.UWATER : 
				return "Water" ;
				break;
			case EUnitType.UNONE : 
				return "None";
				break;
			case EUnitType.UWIND : 
				return "Wind"	;
				break;
			default:
				return string.Empty;
				break;
			}
		}
	}

	public string UnitTypeText {
		get{
			switch ( type ){
			case EUnitType.UFIRE : 
				return TextCenter.GetText("Type_Fire");
				break;
			case EUnitType.ULIGHT : 
				return TextCenter.GetText("Type_Light");
				break;
			case EUnitType.UDARK : 
				return TextCenter.GetText("Type_Dark");
				break;
			case EUnitType.UWATER : 
				return TextCenter.GetText("Type_Water") ;
				break;
			case EUnitType.UNONE : 
				return TextCenter.GetText("Type_None");
				break;
			case EUnitType.UWIND : 
				return TextCenter.GetText("Type_Wind");
				break;
			default:
				return string.Empty;
				break;
			}
		}
	}

	//GetCurveValue() Return Total value at level
	public int GetCurveValue(int level, PowerInfo pi) {

		if (maxLevel <= 1) {
			return pi.max;
		}

		if (level <= 0) {
			return pi.min;
		}

		// growCurve = 0.7, 1.0, 1.5
		float result = pi.min + (pi.max-pi.min) * Mathf.Pow( (float)(level-1)/(maxLevel-1) , (pi.growCurve) );
		return Mathf.FloorToInt(result);
	}

	//return (level+1)'s exp - level's exp
	public int GetLevelExp(int level) {
		int result = 0;
		if (level == 1) {
			result = GetCurveValue(level+1, powerType.expType);
		} else {
			result = GetCurveValue(level+1, powerType.expType) - GetCurveValue(level, powerType.expType);
		}

		return result;
	}

	//return Hp at level
	public int GetHpByLevel(int level) {
		return GetCurveValue(level, powerType.hpType);
	}

	//return Atk at level
	public int GetAtkByLevel(int level) {
		return GetCurveValue(level, powerType.attackType);
	}

	//return the unit level at totalExp
	public int GetLevelByExp(int totalExp) {
		int level = 1;
		while (level < maxLevel) {
			int exp = GetCurveValue(level+1, powerType.expType);
			if ( totalExp < exp ){
				break;
			}
			level += 1;
		}
		return level;
	}

	public bbproto.PowerType PowerType {
		get{
			return powerType;
		}
	}
	
	public int Hp {
		get{
			return GetCurveValue(1, powerType.hpType);
		}
	}
	
	public int Attack {
		get{
			return GetCurveValue(1, powerType.attackType);
		}
	}

	public int Exp {
		get{
			return GetCurveValue(1, powerType.expType);
		}
	}

	public int DevourExp {
		get {
			return devourValue;
		}
	}

//	private Texture2D avatarTexture;
//	private Texture2D profileTexture;



	public string GetUnitBorderSprName(){
		switch (type){
			case EUnitType.UFIRE :
				return UIConfig.SPR_NAME_BORDER_FIRE;
				break;
			case EUnitType.UWATER :
				return UIConfig.SPR_NAME_BORDER_WATER;
				break;
			case EUnitType.UWIND :
				return UIConfig.SPR_NAME_BORDER_WIND;
				break;
			case EUnitType.ULIGHT :
				return UIConfig.SPR_NAME_BORDER_LIGHT;
				break;
			case EUnitType.UDARK :
				return UIConfig.SPR_NAME_BORDER_DARK;
				break;
			case EUnitType.UNONE :
				return UIConfig.SPR_NAME_BORDER_NONE;
				break;
			default:
				return UIConfig.SPR_NAME_BORDER_NONE;
				break;
		}
	}

	public string GetUnitBackgroundName(){
		switch (type){
			case EUnitType.UFIRE :
				return UIConfig.SPR_NAME_BG_FIRE;
				break;
			case EUnitType.UWATER :
				return UIConfig.SPR_NAME_BG_WATER;
				break;
			case EUnitType.UWIND :
				return UIConfig.SPR_NAME_BG_WIND;
				break;
			case EUnitType.ULIGHT :
				return UIConfig.SPR_NAME_BG_LIGHT;
				break;
			case EUnitType.UDARK :
				return UIConfig.SPR_NAME_BG_DARK;
				break;
			case EUnitType.UNONE :
				return UIConfig.SPR_NAME_BG_NONE;
				break;
			default:
				return UIConfig.SPR_NAME_BG_NONE;
				break;
		}
	}

	public List<bbproto.UnitGetWay> UnitGetWay{
		get{
			return getWay;
		}
	}

}
}