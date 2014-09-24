using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using bbproto;

public class DGTools {
//	public static bool DownloadComplete = false;

	private static float TWO_Sprite_Interv = 2f;
	public static void SortStateItem(Dictionary<StateEnum,GameObject> dic, Transform target, float width) {
		foreach (var item in dic.Values) {
			Vector3 localPosition = target.localPosition;
			float distance = Vector3.Distance(localPosition, item.transform.localPosition);
			if(distance < TWO_Sprite_Interv) {
				target.localPosition = new Vector3(localPosition.x + width, localPosition.y, localPosition.z);
				SortStateItem(dic,target,width);
				break;
			}
		}
	}

	static public GameObject AddChild (GameObject parent, GameObject prefab) {
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		
		if (go != null && parent != null) {
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}
		return go;
	}

	public static bool CheckFavorate(UserUnit tuu) {
		return tuu.isFavorite == 1 ? true : false;
	}


//	public static bool IsNoviceGuide () {
//		if (NoviceGuideStepEntityManager.CurrentNoviceGuideStage == NoviceGuideStage.NONE) {
//			return false;		
//		} else {
//			return true;	
//		}
//	}

	public static int GetEnemyWidthByRare(int rare) {
		switch (rare) {
		case 1:
			return 104; // height = 109;
		case 2:
			return 126; // height = 130;
		case 3:
			return 157; // height = 163;
		case 4:
			return 244; // height = 253;
		case 5:
			return 340; // height = 354;
		case 6:
			return 380; // height = 393;
		case 7:
			return 484; // height = 393;
		default:
			return 484;
		}
	}

	public static void ChangeToQuest() {
		DataCenter dataCenter = DataCenter.Instance;
//		UIManager manager = UIManager.Instance;
		MsgCenter msgCenter = MsgCenter.Instance;

		StageInfo tsi = BattleConfigData.Instance.currentStageInfo;
		QuestInfo tqi = BattleConfigData.Instance.currentQuestInfo;

		uint cityID = tsi.cityId;
		CityInfo tci = DataCenter.Instance.QuestData.GetCityInfo (cityID);

		if (tsi == null || tqi == null || tci == null) {
			ModuleManager.Instance.ShowModule (ModuleEnum.HomeModule);
			return;
		}

		StageState questStage = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryStageState (tsi.id);

		if (questStage == StageState.NEW) {

			ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityID);

			ModuleManager.Instance.ShowModule (ModuleEnum.QuestSelectModule,"data",tsi);

			MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
			return;
		}

		StageState stageClearStage = DataCenter.Instance.QuestData.QuestClearInfo.GetStoryCityState (cityID);

		if (questStage == StageState.CLEAR) { 	
			if (tsi.QuestInfo [tsi.QuestInfo.Count - 1].id != tqi.id) { // current quest not the last quest.
				ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityID);

				ModuleManager.Instance.ShowModule (ModuleEnum.QuestSelectModule,"data",tsi);

				MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
			} else {
				if (stageClearStage == StageState.CLEAR) {
					if (tci.stages [tci.stages.Count - 1].id != tsi.id) {	// current stage not the last stage
						ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityID);

						MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
					} else {
						ModuleManager.Instance.ShowModule (ModuleEnum.HomeModule);
					}
				} else {
					ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityID);

					MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
				}
			}
		} else {
			ModuleManager.Instance.ShowModule (ModuleEnum.StageSelectModule,"story",cityID);

			ModuleManager.Instance.ShowModule (ModuleEnum.QuestSelectModule,"data",tsi);

			MsgCenter.Instance.Invoke(CommandEnum.ShowHomeBgMask, true);
		}
	}

	public static int GetEnemyHeightByRare(int rare) {
		switch (rare) {
		case 1:
			return 109;
		case 2:
			return 130;
		case 3:
			return 163;
		case 4:
			return 253;
		case 5:
			return 354;
		case 6:
			return 393;
		case 7:
			return 393;
		default:
			return 484;
		}
	}
	/// <summary>
	/// 0:player, 1:enemy, 2:normal, 3:passive, 4:active.
	/// </summary>
//	public static string[] stateInfo = new string[] {"Player-Phase","Enemy-Phase","Normal-Skill","Passive-Skill","Active-Skill"};

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

	public static string GetItemBackgroundName (EUnitType unitType) {
		string suffixName = "avatar_bg_";
		switch (unitType) {
		case EUnitType.UFIRE:
			return suffixName += "fire";
		case EUnitType.UWATER:
			return suffixName += "water";
		case EUnitType.UWIND:
			return suffixName += "wind";
		case EUnitType.ULIGHT:
			return suffixName += "light";
		case EUnitType.UDARK:
			return suffixName += "dark";
		case EUnitType.UNONE:
			return suffixName += "none";
		default:
			return suffixName += "none";
			break;
		}
	}

	public static string GetItemBorderName (EUnitType unitType) {
		string suffixName = "avatar_border_";	
		switch (unitType) {
		case EUnitType.UFIRE:
			return suffixName += "fire";
		case EUnitType.UWATER:
			return suffixName += "water";
		case EUnitType.UWIND:
			return suffixName += "wind";
		case EUnitType.ULIGHT:
			return suffixName += "light";
		case EUnitType.UDARK:
			return suffixName += "dark";
		case EUnitType.UNONE:
			return suffixName += "none";
		default:
			return suffixName += "none";
			break;
		}
	}

	public static bool EqualCoordinate (Coordinate coorA, Coordinate coorB) {
		return coorA.x == coorB.x && coorA.y == coorB.y;
	}

	public static void ChangeToUnitDetail(uint unitID) {
		UserUnit userUnit = new UserUnit();
		userUnit.level = 1;
		userUnit.exp = 0;
		userUnit.unitId = (uint)unitID;

		ModuleManager.Instance.ShowModule(ModuleEnum.UnitDetailModule,"unit",userUnit);
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

	public static void ShowTexture(UITexture sprite, Texture2D tex) {
		if (sprite == null ) {
			return;	
		}
		sprite.mainTexture = tex;
		if (tex == null) {
			return;	
		}
		sprite.width = tex.width;
		sprite.height = tex.height;
	}
	public static string GetUnitDropSpriteName(int rare) {
		switch (rare) {
		case 1:
			return "a";
		case 2:
			return "b";
		case 3:
			return "c";
		case 4:
			return "d";
		case 5:
			return "e";
		case 6:
			return "f";
		default:
			return string.Empty;
		}
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

	public static bool RestraintType(int firstType, int secondType, bool beRestraint = false) {
		if (!beRestraint) {
			if(firstType == BeRestraintType(secondType)) {
				return true;
			}else{
				return false;
			}
		} else {
			if(firstType == RestraintType(secondType)) {
				return true;
			}
			else{
				return false;
			}
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
//		case 1:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_fire_attack);
//			break;
//		case 2:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_water_attack);
//			break;
//		case 3:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_wind_attack);
//			break;
//		case 4:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_light_attack);
//			break;
//		case 5:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_dark_attack);
//			break;
//		case 6:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_zero_attack);
//			break;
//		case 7:
//			AudioManager.Instance.PlayAudio(AudioEnum.sound_hp_recover);
//			break;
		}
	}

	public static bool IsTriggerSkill<T> (List<T> cardList, List<T> skillList) where T : struct {
		if (cardList.Count < skillList.Count) {
			return false;		
		}
		List<T> tempCard = new List<T> (cardList);
		List<T> tempSkillList = new List<T> (skillList);

		for (int i = 0; i < tempSkillList.Count; i++) {
			if(tempCard.Contains(tempSkillList[i])){
				tempCard.Remove(tempSkillList[i]);
			}else{
				return false;
			}
		}

		return true;
	}

	public static int NeedOneTriggerSkill (List<uint> cardList, List<uint> skillList) {
		int sCount = skillList.Count;
		int cCount = cardList.Count;

		if (sCount > cCount && (sCount - cCount) > 2) { //card count not match
			return -1;	
		}

		List<uint> tempCard = new List<uint> (cardList);
		List<uint> tempSkillList = new List<uint> (skillList);

		for (int i = 0; i < cCount; i++) {
			uint t = tempCard[i];
			if(tempSkillList.Contains(t)) {
				tempSkillList.Remove(t);
			}
		}

		if (tempSkillList.Count == 1) {
			return (int)tempSkillList [0];
		} else {
			return -1;
		}
	}

	public static bool IsFirstBoost<T>(IList<T> first, IList<T> second) {
		if (first.Count < second.Count) {
			return true;
		}
		else {
			return false;
		}
	}

	public static string GetNormalSkillSpriteName (AttackInfo ai) {
//		Debug.LogError ("ai.FixRecoverHP : " + ai.FixRecoverHP + " ai.AttackRange :" + ai.AttackRange);
		if (ai.FixRecoverHP || ai.AttackRange == 2) {
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

	public static float TypeMultiple (UserUnit baseUnit, UserUnit friend) {
		if (baseUnit.UnitInfo.type == friend.UnitInfo.type) {
			return 1.25f;	
		} else {
			return 1f;
		}
	}
	
	public static float RaceMultiple (UserUnit baseUnit, UserUnit friend) {
		if (baseUnit.UnitInfo.race == friend.UnitInfo.race) {
			return 1.25f;
		} else {
			return 1f;
		}
	}

	public static float OnlyTypeMultiple(UserUnit baseUnit, UserUnit material) {
		if (baseUnit.UnitInfo.type == material.UnitInfo.type) {
			return 1.5f;		
		} else {
			return 1f;		
		}
	}
	
	public static float AllMultiple (float type, float race) {
		return type + race - 1;
	}

	public static float AllMultiple (UserUnit baseUnit, UserUnit friend) {
		return AllMultiple (TypeMultiple (baseUnit, friend), RaceMultiple (baseUnit, friend));
	}

	//===========load skill=======================================================
	private const string skillPath = "Skill/";
	private static SkillJsonConfig skillJsonData;

	public static SkillBase LoadSkill (int id, SkillType type) {
		string reallyPath = path +  skillPath;
		if (skillJsonData == null) {
			TextAsset json = LoadTextAsset(reallyPath + "skills");
			skillJsonData = new SkillJsonConfig(json.text);
		}

		string className = skillJsonData.GetClassName (id);
		TextAsset ta = LoadTextAsset(reallyPath + "skill_" +id);
		if (ta == null) {
			Debug.LogError("skill path : " + reallyPath + " not exist paorobuf file" + " id : " + id);
			return null;
		}

		return DisposeByte(ta.bytes, className, type);
	}

	static SkillBase DisposeByte(byte[] data, string typeName,SkillType type) {
		switch (typeName) {
		case "NormalSkill" :
			return ProtobufSerializer.ParseFormBytes<NormalSkill>(data);
		case "SkillSingleAttack":
			SkillSingleAttack ssa = ProtobufSerializer.ParseFormBytes<SkillSingleAttack>(data);
			if(ssa.type == EValueType.PERCENT) {
				return ProtobufSerializer.ParseFormBytes<SkillSingleAttack>(data);
			}
			else{
				return ProtobufSerializer.ParseFormBytes<SkillSingleAttack>(data);
			}
		case "SkillBoost" : 
				return ProtobufSerializer.ParseFormBytes<SkillBoost>(data);
		case "SkillRecoverSP" : 
				return ProtobufSerializer.ParseFormBytes<SkillRecoverSP>(data);
		case "SkillPoison" : 
				return ProtobufSerializer.ParseFormBytes<SkillPoison>(data);
		case "SkillDodgeTrap" : 
				return ProtobufSerializer.ParseFormBytes<SkillDodgeTrap>(data);
		case "SkillAttackRecoverHP" :
				return ProtobufSerializer.ParseFormBytes<SkillAttackRecoverHP>(data);
		case "SkillSuicideAttack" :
				return ProtobufSerializer.ParseFormBytes<SkillSuicideAttack>(data);
		case "SkillTargetTypeAttack":
				return ProtobufSerializer.ParseFormBytes<SkillTargetTypeAttack>(data);
		case "SkillStrengthenAttack":
				return ProtobufSerializer.ParseFormBytes<SkillStrengthenAttack>(data);
		case "SkillKillHP" :
				return ProtobufSerializer.ParseFormBytes<SkillKillHP>(data);
		case "SkillRecoverHP":
			SkillRecoverHP recoverHP = ProtobufSerializer.ParseFormBytes<SkillRecoverHP>(data);
			if(recoverHP.period == EPeriod.EP_RIGHT_NOW) {
				return ProtobufSerializer.ParseFormBytes<SkillRecoverHP>(data);
			} else {
				return ProtobufSerializer.ParseFormBytes<SkillRecoverHP>(data);
			}
		case "SkillReduceHurt" :
			SkillReduceHurt reduceHurt = ProtobufSerializer.ParseFormBytes<SkillReduceHurt>(data);
			if(reduceHurt.baseInfo.skillCooling > 0){
				return ProtobufSerializer.ParseFormBytes<SkillReduceHurt>(data);
			} else{
				return ProtobufSerializer.ParseFormBytes<SkillReduceHurt>(data);
			}
		case "SkillReduceDefence":
			return ProtobufSerializer.ParseFormBytes<SkillReduceDefence>(data);
		case "SkillDeferAttackRound":
			return ProtobufSerializer.ParseFormBytes<SkillDeferAttackRound>(data);
		case "SkillDelayTime":
			SkillDelayTime delayTime = ProtobufSerializer.ParseFormBytes<SkillDelayTime>(data);
			if(delayTime.baseInfo.skillCooling > 0){
				return ProtobufSerializer.ParseFormBytes<SkillDelayTime>(data);
			}else{
				return ProtobufSerializer.ParseFormBytes<SkillDelayTime>(data);
			}
		case "SkillConvertUnitType":
			SkillConvertUnitType convertType = ProtobufSerializer.ParseFormBytes<SkillConvertUnitType>(data);
			if(convertType.baseInfo.skillCooling > 0) {
				return ProtobufSerializer.ParseFormBytes<SkillConvertUnitType>(data);
			} else{
				return ProtobufSerializer.ParseFormBytes<SkillConvertUnitType>(data);
			}
		case "SkillAntiAttack":
			return ProtobufSerializer.ParseFormBytes<SkillAntiAttack>(data);
		case "SkillExtraAttack":
			return ProtobufSerializer.ParseFormBytes<SkillExtraAttack>(data);
		case "SkillMultipleAttack":
			return ProtobufSerializer.ParseFormBytes<SkillMultipleAttack>(data);
		default:
			return null;
		}
	}

	//============load unitinfo====================================================
	private const string path = 
#if LANGUAGE_CN
	"Protobuf/";
#elif LANGUAGE_EN
	"ProtobufEn/";
#else
	"Protobuf/";
#endif
	private const string unitInfoPath = "Unit/unit_";
	public static UnitInfo LoadUnitInfoProtobuf(uint unitID) {
		string url = path +unitInfoPath + unitID;
		TextAsset ta = LoadTextAsset (url);
//		Debug.Log ("unitID: "+ unitID +" proto len: " +   ta.bytes.Length);
		if (ta == null) {
			Debug.LogError( "load unit info fail : " + " url : " + url + " unit id : " + unitID);	
			return null;
		}
		return ProtobufSerializer.ParseFormBytes<UnitInfo> (ta.bytes);
//		File.WriteAllBytes ("Assets/ResourceDownload/Output/" + unitID,ta.bytes);

	}

	//============load questinfo====================================================
	private const string CityPath = "City/";
	public static CityInfo LoadCityInfo (uint cityID) {
		string url = path + CityPath + "city_" + cityID;
//		Debug.LogError ("LoadCityInfo : " + url);
		TextAsset ta = LoadTextAsset (url);
		return ProtobufSerializer.ParseFormBytes<CityInfo> (ta.bytes);
	}
	
	public static List<CityInfo> LoadCityList(){
		List<CityInfo> cityList = new List<CityInfo>();
		string url = path + CityPath + "CityList";
		TextAsset ta = LoadTextAsset (url);
		WorldMapInfo wmi = ProtobufSerializer.ParseFormBytes<WorldMapInfo> (ta.bytes);
//		Debug.LogError("LoadCityList(), wmi.citylist count is : " + wmi.citylist.Count);
		for (int i = 0; i < wmi.citylist.Count; i++){
			cityList.Add(wmi.citylist[ i ]);
		}
		return cityList;
	}

	static TextAsset LoadTextAsset (string url) {
		return ResourceManager.Instance.LoadLocalAsset (url ,null) as TextAsset;
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

	/// <summary>
	/// Copies the transform. get target value to set source.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="target">Target.</param>
	public static void CopyTransform(Transform source, Transform target) {
		source.localPosition = target.localPosition;
		source.eulerAngles = target.eulerAngles;
		source.localScale = target.localScale;
	}

//	public static FriendSelectLevelUpView CreatFriendWindow() {
//		GameObject go = ResourceManager.Instance.LoadLocalAsset ("Prefabs/UI/Friend/FriendWindows",null) as GameObject;
//		GameObject instance = GameObject.Instantiate (go) as GameObject; // NGUITools.AddChild (ViewManager.Instance.CenterPanel, go);
//		Transform insTrans = instance.transform;
//		insTrans.parent = ViewManager.Instance.BottomPanel.transform;
//		insTrans.localPosition = Vector3.zero;
//		insTrans.localScale = Vector3.one;
//		FriendSelectLevelUpView fw = instance.GetComponent<FriendSelectLevelUpView>();
////		fw.Init (null, null);
//		return fw;
//	}
}

public class GameLayer {

	public static LayerMask Default = 0;
	
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

	public static LayerMask EffectLayer = 12;

	public static LayerMask blocker = 15;

	public static LayerMask NoviceGuide = 16;

	public static LayerMask BottomInfo = 29;

	public static LayerMask Bottom = 31;

	public static int LayerToInt(LayerMask layer)
	{

		return 1 << layer;
	}
}