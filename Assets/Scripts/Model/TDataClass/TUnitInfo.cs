 using bbproto;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TUnitInfo : ProtobufDataBase {
	private UnitInfo instance;
	public TUnitInfo (object instance) : base (instance) {
		this.instance = instance as UnitInfo;
	}
	
//	public int unitBaseInfoID = 0;
	public UnitInfo Object{
		get{
			return instance;
		}
	}

	public EvolveInfo evolveInfo {
		get {
			return instance.evolveInfo;
		}
	}

	public uint ID  {
		get {
			return instance.id;
		}
	}
	public string Name {
		get{
			return instance.name;
		}
	}
	
	public int Cost {
		get{
			return instance.cost;
		}
	}

	public int SaleValue{
		get{
			return instance.saleValue;
		}
	}

	public int Rare {
		get{
			return instance.rare;
		}
	}

	public int MaxRare {
		get{
			return instance.maxStar;
		}
	}

	public int MaxLevel {
		get {
			return instance.maxLevel;
		}
	}

	public Position ShowPos {
		get {
			return instance.showPos;
		}
	}

	public int NormalSkill1 {
		get{
			return instance.skill1;
		}
	}

	public int NormalSkill2 {
		get{
			return instance.skill2;
		}
	}

	public int ActiveSkill {
		get{
			return instance.activeSkill;
		}
	}

	public int PassiveSkill {
		get{
			return instance.passiveSkill;
		}
	}

	public int LeaderSkill {
		get{
			return instance.leaderSkill;
		}
	}

/// <summary>
/// SSS
/// </summary>
/// <returns>The profile.</returns>

	public string Profile {
		get{
			return instance.profile;
		}
	}

	public EUnitRace Race {
		get { return instance.race; }
	}

	public EUnitType Type {
		get { return instance.type; }
	}
		
	public string UnitRace{
			get{
				EUnitRace race = instance.race;
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
			EUnitType unitType =  instance.type;
			switch ( unitType ){
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
			EUnitType unitType =  instance.type;
			switch ( unitType ){
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

		if (MaxLevel <= 1) {
			return pi.max;
		}

		if (level <= 0) {
			return pi.min;
		}

		// growCurve = 0.7, 1.0, 1.5
		float result = pi.min + (pi.max-pi.min) * Mathf.Pow( (float)(level-1)/(MaxLevel-1) , (pi.growCurve) );

//		Debug.LogError("id : " + ID +"Unit.GetCurveValue("+level+") min:"+pi.min+" max:"+pi.max+" maxLv:"+MaxLevel+" grow:"+pi.growCurve+" result:"+ Mathf.FloorToInt(result));
		return Mathf.FloorToInt(result);
	}

	public int GetLevelExp(int level) {
		int result = 0;
		if (level == 1) {
			result = GetCurveValue(level+1, instance.powerType.expType);
		} else {
			result = GetCurveValue(level+1, instance.powerType.expType) - GetCurveValue(level, instance.powerType.expType);
		}

		return result;
	}

	public bbproto.PowerType PowerType {
		get{
			return instance.powerType;
		}
	}
	
	public int Hp {
		get{
			return GetCurveValue(1, instance.powerType.hpType);
		}
	}
	
	public int Attack {
		get{
			return GetCurveValue(1, instance.powerType.attackType);
		}
	}

	public int Exp {
		get{
			return GetCurveValue(1, instance.powerType.expType);
		}
	}

//	public int HPType {
//		get{
//			return TPowerTableInfo.UnitInfoHPType;
//		}
//	}
//	
//	public int AttackType {
//		get{
//			return TPowerTableInfo.UnitInfoAttackType;
//		}
//	}
//	
//	public int ExpType {
//		get{
//			return TPowerTableInfo.UnitInfoExpType;
//		}
//	}

	public int DevourExp {
		get {
			return instance.devourValue;
		}
	}

	private Texture2D avatarTexture;
	private Texture2D profileTexture;

	public void GetAsset(UnitAssetType uat,ResourceCallback callback) {
		string path = string.Empty;

		if (uat == UnitAssetType.Avatar) {
			if (avatarTexture == null) {
				path = string.Format ("Avatar/{0}", ID);
				ResourceManager.Instance.LoadLocalAsset (path,o=>{
					avatarTexture = o as Texture2D;
					callback(o);
				});
			}else{
				callback(avatarTexture);
			}

		} 
		else  {
			if(profileTexture == null) {
				path = string.Format("Profile/{0}", ID) ;
				ResourceManager.Instance.LoadLocalAsset(path,o=>{
					profileTexture = o as Texture2D;
					callback(o);
				});
			}else{
				callback(profileTexture);
			}

		}
	}

	public string GetUnitBorderSprName(){
		switch (Type){
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
		switch (Type){
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

	public void Send () {}
	public void Receive (IWWWPost post) {}
	public void SerialToFile () {}
}