using bbproto;
using UnityEngine;
using System.Collections.Generic;

public class TUnitInfo : ProtobufDataBase, INetBase  {
	private UnitInfo instance;
	public TUnitInfo (object instance) : base (instance) {
		this.instance = instance as UnitInfo;
	}
	
	public int unitBaseInfoID = 0;
	public UnitInfo Object{
		get{
			return instance;
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

	//	public string GetRace(){
	//		EUnitRace race = DeserializeData< UnitInfo >().race;
	//		switch ( race ){
	//			case EUnitRace.HUMAN : 
	//				return "Human";
	//				break;
	//			case EUnitRace.LEGEND : 
	//				return "Legend";
	//				break;
	//			case EUnitRace.MONSTER : 
	//				return "Monster";
	//				break;
	//			case EUnitRace.MYTHIC : 
	//				return "Mythic";
	//				break;
	//			case EUnitRace.BEAST : 
	//				return "Beast";
	//				break;
	//			case EUnitRace.UNDEAD : 
	//				return "Undead"  ;
	//				break;
	//			case EUnitRace.SCREAMCHEESE: 
	//				return "强化合成专用";
	//				break;
	//			default:
	//				return string.Empty;
	//				break;
	//		}
	//	}

	public string GetUnitType() {
		EUnitType unitType = DeserializeData< UnitInfo >().type;
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
	
	public Texture2D GetAsset(UnitAssetType uat) {
		string path = string.Empty;
		switch (uat) {
		case UnitAssetType.Avatar:
			path = string.Format("Avatar/{0}_1", ID) ;
			break;
		case UnitAssetType.Profile:
			path = string.Format("Profile/{0}_2", ID) ;
			break;
			
		}
		Texture2D tex2d = Resources.Load(path) as Texture2D;
		return tex2d;
	}
	
	public void Send () {
		
		HttpNetBase hnb = new HttpNetBase ();
		hnb.Url = "aaa";
		WWWForm wf = new WWWForm ();
		//		hnb.WwwInfo = new WWW (hnb.Url, wf);
		hnb.Send (this,wf);
	}
	
	public void Receive (IWWWPost post) {
		
	}
	
	
}