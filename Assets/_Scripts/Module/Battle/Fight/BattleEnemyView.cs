using UnityEngine;
using System.Collections.Generic;
using bbproto;
using System.Text;
using System;

public class BattleEnemyView : ViewBase {
	private Dictionary<uint, BattleEnemyItem> enemyList;
	private Queue<BattleEnemyItem> enemyItemPool;
	private GameObject effectPanel;

	private GameObject enemyRoot;
	private GameObject enemyItemPrefab;
//	public BattleManipulationView battle;

	private UILabel attackInfoLabel;
//	private string[] attackInfo = new string[4] {"Nice!", "BEAUTY!", "great-!", "Excellent-!"};
	private string[] attackInfo = new string[4] {"Nice !", "Beauty !", "Great !", "Excellent !"};
	private BattleAttackInfo battleAttackInfo;
	private Vector3 defaultAtkInfoRotation = new Vector3 (0f, 0f, 10f);
	private Vector3 defaultAtkInfoPosition, moveAtkInfoPosition;
	private UITexture bgTexture;
	
	public const float SingleSkillDangerLevel = 2.3f;
	public const float AllSkillDangerLevel = 1.8f;

	public override void Init (UIConfigItem config, Dictionary<string, object> data = null)
	{
		base.Init (config, data);
//	}
//		transform.localPosition = new Vector3 (0f, 100f, 0f);
		enemyList = new Dictionary<uint, BattleEnemyItem> ();
		enemyItemPool = new Queue<BattleEnemyItem> ();

//		base.Init (name);
		effectPanel = transform.Find ("Effect").gameObject;
		enemyRoot = transform.Find ("EnemyRoot").gameObject;
		enemyItemPrefab = transform.Find ("EnemyRoot/EnemyItem").gameObject;
		enemyItemPrefab.SetActive (false);
//		transform.localPosition += new Vector3 (0f, battle.cardHeight * 6.5f, 0f);
		attackInfoLabel = FindChild<UILabel>("Label");
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		defaultAtkInfoPosition = attackInfoLabel.transform.localPosition;
		moveAtkInfoPosition = new Vector3(defaultAtkInfoPosition.x, defaultAtkInfoPosition.y+100f, defaultAtkInfoPosition.z);

		battleAttackInfo = FindChild<BattleAttackInfo>("AttackInfo");
		battleAttackInfo.Init ();
		bgTexture = FindChild<UITexture>("Texture");
		string path = "Texture/Map/fight_" + BattleConfigData.Instance.GetMapID ().ToString ();
//		Debug.LogError ("BattleEnemy path : " + path);
		ResourceManager.Instance.LoadLocalAsset (path, o => {
			bgTexture.mainTexture = o as Texture2D;
		});

	}
	
	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayAllEffect, PlayAllEffect);

		foreach (var item in enemyList.Values) {
			if(item != null) {
				item.gameObject.SetActive(false);
				enemyItemPool.Enqueue(item);
			}
		}
		enemyList.Clear ();
	}

	public override void ShowUI () {
		base.ShowUI ();


		NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.FIGHT);
		if(viewData != null && viewData.ContainsKey("enemy")){
			Refresh(viewData["enemy"] as List<EnemyInfo>);
			if((viewData["enemy"] as List<EnemyInfo>)[0].enemeyType == EEnemyType.BOSS){
				NoviceGuideStepManager.Instance.StartStep (NoviceGuideStartType.FIGHT_BOSS);
			}
		}
		MsgCenter.Instance.AddListener (CommandEnum.PlayAllEffect, PlayAllEffect);
	}

	public override void CallbackView (params object[] args)
	{
		switch(args[0].ToString()){
		case "refresh_enemy":
			foreach (var item in enemyList.Values) {
				item.EnemyRefresh(args[1]);
			}
			break;
		case "enemy_attack":
			foreach (var item in enemyList.Values) {
				item.EnemyAttack(args[1]);
			}
			break;
		case "enemy_dead":
			EnemyInfo en = args[1] as EnemyInfo;
			if (enemyList.ContainsKey (en.EnemySymbol) && enemyList[en.EnemySymbol].enemyInfo.IsDead) {
				bbproto.EnemyInfo ei = enemyList[en.EnemySymbol].enemyInfo;
				enemyList[en.EnemySymbol].EnemyDead();
			}

			BattleConfigData.Instance.storeBattleData.EnemyInfo.Remove(args[1] as EnemyInfo);
			BattleConfigData.Instance.StoreMapData();
			break;
		case "attack_enemy":
			foreach (var item in enemyList.Values) {
				item.AttackEnemy(args[1]);
				EnemyItemPlayEffect (item, args[1] as AttackInfoProto);
			}
			break;
		case "attack_enemy_end":
			AttackEnemyEnd(args[1]);
			break;
		case "skill_recover_sp":
			SkillRecoverSP(args[1]);
			break;
		case "reduce_info":
			foreach (var item in enemyList.Values) {
				item.ReduceDefense(args[1]);
			}
			break;
		}
	}

	void PlayAllEffect(object data) {
		AttackInfoProto ai = data as AttackInfoProto;
		if (ai == null) {
			return;	
		}

		PlayerEffect (null, ai);
	}

	void AttackEnemyEnd(object data) {
		int count = (int)data;
		prevAttackInfo = null;
		if (count <= 0) {
			return;	
		}

		GameTimer.GetInstance ().AddCountDown (0.2f, ShowAttackInfoAnimation);
	}

	void ShowAttackInfoAnimation() {
		//NICE!,GREAT!..动画
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.text = attackInfo [index];
		attackInfoLabel.transform.eulerAngles = defaultAtkInfoRotation;
		attackInfoLabel.gameObject.SetActive(true);
		iTween.MoveTo (attackInfoLabel.gameObject, iTween.Hash ("position", moveAtkInfoPosition, "time", 0.3f, "islocal", true, "easetype", iTween.EaseType.easeInQuad));
		
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.3f, "easetype", iTween.EaseType.easeInQuad));
		//旋转360度
		iTween.RotateBy (attackInfoLabel.gameObject, iTween.Hash ("amount", new Vector3 (0f, 0f, -1f), "time", 0.3f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "DisapperAttackInfo", "oncompletetarget", gameObject));
	}

	void DisapperAttackInfo() {
		//		GameTimer.GetInstance ().AddCountDown (0.3f, HideAttackInfo);
		iTween.RotateTo(attackInfoLabel.gameObject, iTween.Hash ("x", -90f, "y", -80f, "z",10f, "delay",0.1f, "time", 0.25f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "HideAttackInfo", "oncompletetarget", gameObject));
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (2.8f, 1.4f, 1f),"delay",0.1f, "time", 0.2f, "easetype", iTween.EaseType.easeInQuad));

	}
	
	void HideAttackInfo() {
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (4f, 4f, 4f);
		attackInfoLabel.transform.localPosition = defaultAtkInfoPosition;
		attackInfoLabel.transform.eulerAngles = defaultAtkInfoRotation;
		//		attackInfoLabel.transform.rotation = new Vector3 (0f, 0f, 0f);
	}
	
	List<AttackInfoProto> attackList= new List<AttackInfoProto>();

//	void AttackEnemy(object data) {
////		DestoryEffect ();
//	}

//	List<EnemyItem> enemyList = new List<EnemyItem>();

	AttackInfoProto prevAttackInfo = null;

	public void EnemyItemPlayEffect(BattleEnemyItem ei, AttackInfoProto ai) {
		// > 0. mean is all attack.
		if (prevAttackInfo != null &&  prevAttackInfo.isLink > 0 && prevAttackInfo.isLink == ai.isLink) {
			return;
		}
		if (ei.enemyInfo.EnemySymbol != ai.enemyID)
				return;
//		Debug.LogError ("EnemyItemPlayEffect ");
		prevAttackInfo = ai;
		PlayerEffect (ei, ai);
	}

	void End() {
//		GameTimer.GetInstance ().AddCountDown (0.2f, HideAttackInfo);
	}


//	private List<BattleEnemyItem> enemys = new List<BattleEnemyItem> ();	 

	public void Refresh(List<EnemyInfo> enemy) {
//
//		monster.Clear();
//		BattleEnemyItem[] ei = transform.GetComponentsInChildren<BattleEnemyItem> ();
//		foreach (var item in ei) {
//			item.DestoryUI();
//		}

		int sortCount = 0;
		enemyList.Clear ();
//		enemys.Clear ();
//		List<BattleEnemyItem> enemys = new List<BattleEnemyItem> ();
		if (enemy.Count == 0) {
			Debug.Log("no enemy");
//			sortCount++;
//			BeginSort ();
		} else {
			sortCount = enemy.Count;
			for (int i = 0; i < enemy.Count; i++) {
				EnemyInfo tei = enemy[i];
				tei.AddListener();
				GameObject go;
				BattleEnemyItem ei;
				if(enemyItemPool.Count > 0){
					go = enemyItemPool.Dequeue().gameObject;
					ei = go.GetComponent<BattleEnemyItem>();

					enemyList.Add(tei.EnemySymbol,ei);
					ei.RefreshData(tei,()=>{
						sortCount--;
						
						if (sortCount == 0) {
							SortEnemyItem (enemyList);
						} 
					});
				}else{
					go = NGUITools.AddChild(enemyRoot, enemyItemPrefab);
					ei = go.AddComponent<BattleEnemyItem>();
					enemyList.Add(tei.EnemySymbol,ei);
					//				enemys.Add(ei);
					ei.Init(tei, ()=>{
						sortCount--;
						
						if (sortCount == 0) {
							SortEnemyItem (enemyList);
						} 
					});
				}

//				go.SetActive(true);
			}
		}
	}

	void SortEnemyItem(Dictionary<uint,BattleEnemyItem> enemys) {

		int count = enemys.Count;
		float allWidth = 0f;
		float pos = 0f;
		float maxHeight = 0f;
		UITexture tex;
		float firstItemWidth = 0f;
		foreach (var item in enemys.Values) {
			tex = item.texture;
			if(pos == 0f){
				item.transform.localPosition = new Vector3(pos,0,0);
				pos += tex.width/2;

			}else{
				pos += tex.width/2;
				item.transform.localPosition = new Vector3(pos,0,0);
				pos += tex.width/2;
			}
			if(firstItemWidth == 0f){
				firstItemWidth = tex.width;
			}
			allWidth += tex.width;
			if(maxHeight < tex.height){
				maxHeight = tex.height;
			}
			item.gameObject.SetActive(true);
		}
		float scaleVal = 1f;
		if( ScreenWidth < allWidth ) {
			scaleVal = (float)ScreenWidth / allWidth;
		}
		enemyRoot.transform.localScale = new Vector3 (scaleVal, scaleVal, 0);
		effectPanel.transform.localPosition = enemyRoot.transform.localPosition = new Vector3((- allWidth + firstItemWidth)/2*scaleVal, maxHeight*scaleVal/2,0);// enemys[0].texture.width/2;
		Debug.Log ("enemy item sort: " + enemyRoot.transform.localPosition);

	}
	public const int ScreenWidth = 640;

//	GameObject prevEffect;
//	List<GameObject> extraEffect = new List<GameObject> ();

	void PlayerEffect(BattleEnemyItem ei, AttackInfoProto ai) {

		if (ai.skillID == 0) {
			//			Debug.LogError ("skillStoreID : " + skillID + " userUnitID : " + userUnitID);
			return; 
		}
		string skillStoreID = DataCenter.Instance.BattleData.GetSkillID(ai.userUnitID, ai.skillID);
		SkillBase sbi = DataCenter.Instance.BattleData.AllSkill[skillStoreID];
		string path = "";
		NormalSkill tns = sbi as NormalSkill;
		if (tns != null) {
			path = GetNormalSkillEffectName (tns);
		} else if (sbi is ActiveSkill) {
			StringBuilder sb = new StringBuilder ();
			sb.Append ("as-");
			Type type = sbi.GetType ();
			if (type == typeof(SkillSingleAttack)) {
				GetSingleAttackEffectName (sbi as SkillSingleAttack, sb);
			} else if (type == typeof(SkillTargetTypeAttack)) {
				GetAttackTargetType (sbi as SkillTargetTypeAttack, sb);
			} else if (type == typeof(SkillConvertUnitType)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_as_color_change);
				sb.Append ("color");
			} else if (type == typeof(SkillDeferAttackRound)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_slow);
				
				sb.Append ("low");
			} else if (type == typeof(SkillDelayTime)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_delay);
				
				sb.Append ("delay");
			} else if (type == typeof(SkillReduceDefence)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_def_down);
				
				sb.Append ("reduce-def");
			} else if (type == typeof(SkillReduceHurt)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_damage_down);
				
				sb.Append ("reduce-injure");
			} else if (type == typeof(SkillAttackRecoverHP)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_single1_blood);
				
				sb.Append ("single-blood-purple");
			} else if (type == typeof(SkillKillHP)) {
				sb.Append ("all-2-dark");
			} else if (type == typeof(SkillSingleAttack)) {
				sb.Append ("single-2-dark");
			} else if (type == typeof(SkillRecoverSP)) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_poison);
				sb.Append ("sp-recover");
			} else if (type == typeof(SkillPoison)) {
				sb.Append ("poison");
			} else if (type == typeof(SkillSuicideAttack)) {
				SkillSuicideAttack tsa = sbi as SkillSuicideAttack;
				sb.Append (GetAttackRanger (tsa.AttackRange));
				sb.Append (GetAttackDanger (1, 2)); //2 == second effect.
				sb.Append (GetSkillType (tsa.AttackUnitType));
			}
			path = sb.ToString ();
		} else if (sbi is SkillExtraAttack) {
			AudioManager.Instance.PlayAudio(AudioEnum.sound_ls_chase);
			path = "ls-claw-1-";
			SkillExtraAttack tsea = sbi as SkillExtraAttack;
			switch (tsea.unitType) {
			case bbproto.EUnitType.UFIRE:
				path += "fire";
				break;
			case bbproto.EUnitType.UWATER:
				path += "water";
				break;
			case bbproto.EUnitType.UWIND:
				path += "wind";
				break;
			case bbproto.EUnitType.ULIGHT:
				path += "light";
				break;
			case bbproto.EUnitType.UDARK:
				path += "dark";
				break;
			case bbproto.EUnitType.UNONE:
				path += "none";
				break;
			default:
				path += "fire";
				break;
			}
		} else if(sbi is TSkillAntiAttack) {
			string effectName = "ps-sword-1-";
			TSkillAntiAttack tsaa = sbi as TSkillAntiAttack;
			path = effectName + GetSkillType((int)tsaa.AttackSource);
			AudioManager.Instance.PlayAudio(AudioEnum.sound_ps_counter);
		}

		if(DataCenter.Instance.BattleData.AllSkill[skillStoreID].GetType() == typeof(SkillExtraAttack)) {
			foreach (var item in enemyList.Values) {
				if(item != null) {
					EffectManager.Instance.PlayEffect(path,effectPanel.transform, item.transform.localPosition, o=>{
						item.InjuredShake();
					});
				}
			}
		} else {

			if(ai.attackRange == 0) {
				UITexture tex = ei.texture;
				float x = ei.transform.localPosition.x*enemyRoot.transform.localScale.x;
				float y = (tex.transform.localPosition.y +  tex.height * 0.5f)*enemyRoot.transform.localScale.y;
				EffectManager.Instance.PlayEffect(path,effectPanel.transform,new Vector3(x,y,0),o=>{
					ei.InjuredShake();
				});
			}
		}
	}
	
	void GetAttackTargetType(SkillTargetTypeAttack aatt,StringBuilder sb) {
		sb.Append(GetAttackRanger(aatt.AttackRange));
		float hurtValue = aatt.value;
		if(aatt.type == bbproto.EValueType.FIXED) {
			sb.Append("1-");
		}
		else {
			sb.Append(GetAttackDanger(aatt.AttackRange ,hurtValue));
		}
		sb.Append (GetSkillType (aatt.AttackType));
		
		switch (aatt.AttackRange) {
		case 0:
			if(aatt.type == bbproto.EValueType.FIXED || hurtValue < SingleSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_single1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_single2);
			}
			break;
		case 1:
			if(aatt.type == bbproto.EValueType.FIXED || hurtValue < AllSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_all1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_all2);
			}
			break;
		}
	}
	
	void GetSingleAttackEffectName(SkillSingleAttack tssa,StringBuilder sb) {
		sb.Append(GetAttackRanger(tssa.AttackRange));
		
		if(tssa.type == bbproto.EValueType.FIXED) {
			sb.Append("1-");
		} else {
			sb.Append(GetAttackDanger(tssa.AttackRange, tssa.value));
		}
		
		sb.Append (GetSkillType (tssa.AttackType));
		
		switch (tssa.AttackRange) {
		case 0:
			if(tssa.type == bbproto.EValueType.FIXED || tssa.value < SingleSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_single1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_single2);
			}
			break;
		case 1:
			if(tssa.type == bbproto.EValueType.FIXED || tssa.value < AllSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_all1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_as_all2);
			}
			break;
		}
	}
	
	string GetNormalSkillEffectName(NormalSkill tns) {
		StringBuilder sb = new StringBuilder ();
		sb.Append("ns-");
		sb.Append (GetAttackRanger (tns.AttackRange));
		float hurtValue = tns.attackValue;
		sb.Append (GetAttackDanger (tns.AttackRange, hurtValue));
		sb.Append(GetSkillType(tns.AttackType));
		
		switch (tns.AttackRange) {
		case 0:
			if(hurtValue < SingleSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_ns_single1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_ns_single2);
			}
			break;
		case 1:
			if(hurtValue < AllSkillDangerLevel) {
				AudioManager.Instance.PlayAudio(AudioEnum.sound_ns_all1);
			}else{
				AudioManager.Instance.PlayAudio(AudioEnum.sound_ns_all2);
			}
			break;
		}
		
		return sb.ToString ();
	}
	
	string GetAttackRanger(int attackRange) {
		if(attackRange == 0) {
			return "single-";
		}
		else {
			return "all-";
		}
	}
	
	string GetAttackDanger(int attackRange, float attackValue) {
		if (attackRange == 0) {
			if (attackValue < SingleSkillDangerLevel) {	
				return "1-";
			} else {
				return "2-";
			}
		} else {
			if (attackValue < AllSkillDangerLevel) {
				return "1-";
			} else {
				return "2-";
			}
		}
		
	}
	
	string GetSkillType(int type) {
		switch (type) {
		case 0:
			return "all";
		case 1:
			return "fire";
		case 2:
			return "water";
		case 3:
			return "wind";
		case 4:
			return "light";
		case 5:
			return "dark";
		case 6:
			return "none";
		case 7:
			return "heart";
		}
		return "";
	}

	void SkillRecoverSP(object data) {
//		ResourceManager.Instance.LoadLocalAsset ("Effect/jiufeng", o => {
//			GameObject obj = o as GameObject;
//			if (obj != null) {
//				Transform trans = obj.transform;
//				prevEffect = NGUITools.AddChild (effectPanel, obj);
//				DGTools.CopyTransform (prevEffect.transform, trans);
//			}
//		});

	}
	
//	void ExcuteActiveSkillEnd(object data) {
//		bool b = (bool)data;
//		if (!b) {
//			DestoryEffect();
//		}
//	}

}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}