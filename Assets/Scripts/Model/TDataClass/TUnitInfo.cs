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
	
	public int MaxLevel {
		get {
			return instance.maxLevel;
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
					return "Human";
					break;
				case EUnitRace.LEGEND : 
					return "Legend";
					break;
				case EUnitRace.MONSTER : 
					return "Monster";
					break;
				case EUnitRace.MYTHIC : 
					return "Mythic";
					break;
				case EUnitRace.BEAST : 
					return "Beast";
					break;
				case EUnitRace.UNDEAD : 
					return "Undead"  ;
					break;
				case EUnitRace.SCREAMCHEESE: 
					return "强化合成专用";
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
	
	public int HPType {
		get{
			return instance.powerType.hpType;
		}
	}
	
	public int AttackType {
		get{
			return instance.powerType.attackType;
		}
	}
	
	public int ExpType {
		get{
			return instance.powerType.expType;
		}
	}

	public int DevourExp {
		get {
			return instance.devourValue;
		}
	}

	private Texture2D avatarTexture;
	private Texture2D profileTexture;
	
	public Texture2D GetAsset(UnitAssetType uat) {
		string path = string.Empty;

		if (uat == UnitAssetType.Avatar) {
			if (avatarTexture == null) {
				path = string.Format ("Avatar/{0}", ID);
				avatarTexture = Resources.Load (path) as Texture2D;
			}
			return avatarTexture;
		} 
		else  {
			if(profileTexture == null) {
				path = string.Format("Profile/{0}", ID) ;
				profileTexture =  Resources.Load(path) as Texture2D;
				
			}
			return profileTexture;
		}
	}
	
	public void Send () {
	}
	
	public void Receive (IWWWPost post) {
		
	}

	public void SerialToFile () {
	}
}