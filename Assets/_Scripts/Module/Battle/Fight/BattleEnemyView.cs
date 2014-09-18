using UnityEngine;
using System.Collections.Generic;

public class BattleEnemyView : ViewBase {
	private Dictionary<uint, BattleEnemyItem> enemyList;
	private Queue<BattleEnemyItem> enemyItemPool;
	private GameObject effectPanel;

	private GameObject enemyRoot;
	private GameObject enemyItemPrefab;
//	public BattleManipulationView battle;

	private UILabel attackInfoLabel;
//	private string[] attackInfo = new string[4] {"Nice!", "BEAUTY!", "great-!", "Excellent-!"};
	private string[] attackInfo = new string[4] {"Nice !", "BEAUTY !", "great !", "Excellent !"};
	private BattleAttackInfo battleAttackInfo;
	private UITexture bgTexture;

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
//		Clear ();
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
//		MsgCenter.Instance.RemoveListener (CommandEnum.DropItem, DropItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillRecoverSP, SkillRecoverSP);
//		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayAllEffect, PlayAllEffect);
//		battleAttackInfo.HideUI ();

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

		if(viewData != null && viewData.ContainsKey("enemy")){
			Refresh(viewData["enemy"] as List<TEnemyInfo>);
		}
//		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
//		battleAttackInfo.ShowUI ();
//		MsgCenter.Instance.AddListener (CommandEnum.DropItem, DropItem);
		MsgCenter.Instance.AddListener (CommandEnum.SkillRecoverSP, SkillRecoverSP);
//		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillEnd);
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
			foreach (var item in enemyList.Values) {
				item.EnemyDead(args[1]);
			}
			break;
		case "attack_enemy":
			foreach (var item in enemyList.Values) {
				item.AttackEnemy(args[1]);
				EnemyItemPlayEffect (item, args[1] as AttackInfo);
			}
			break;
		case "attack_enemy_end":
			AttackEnemyEnd(args[1]);
			break;

		case "drop_item":
			DropItem(args[1]);
			break;
		}
	}

	void PlayAllEffect(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;	
		}

		PlayerEffect (null, ai);
	}

	void AttackEnemyEnd(object data) {
		int count = (int)data;
		DestoryEffect ();
		prevAttackInfo = null;
		if (count <= 0) {
			return;	
		}
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.text = attackInfo [index];
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.3f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "End", "oncompletetarget", gameObject));
	}
	
	List<AttackInfo> attackList= new List<AttackInfo>();

//	void AttackEnemy(object data) {
////		DestoryEffect ();
//	}

//	List<EnemyItem> enemyList = new List<EnemyItem>();

	AttackInfo prevAttackInfo = null;

	public void EnemyItemPlayEffect(BattleEnemyItem ei, AttackInfo ai) {
		// > 0. mean is all attack.
		if (prevAttackInfo != null &&  prevAttackInfo.IsLink > 0 && prevAttackInfo.IsLink == ai.IsLink) {
			return;
		}
//		Debug.LogError ("EnemyItemPlayEffect ");
		prevAttackInfo = ai;
		PlayerEffect (ei, ai);
	}

	void DestoryEffect() {
		if (prevEffect != null) {				
			Destroy(prevEffect);
		}
		if (extraEffect.Count > 0) {
			for (int i = extraEffect.Count - 1; i >= 0; i--) {
				Destroy(extraEffect[i]);
			}	
			extraEffect.Clear();
		}
	}

	void End() {
		GameTimer.GetInstance ().AddCountDown (0.2f, HideAttackInfo);
	}

	void HideAttackInfo() {
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		attackInfoLabel.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
	}

//	private List<BattleEnemyItem> enemys = new List<BattleEnemyItem> ();	 

	public void Refresh(List<TEnemyInfo> enemy) {
//
//		monster.Clear();
//		BattleEnemyItem[] ei = transform.GetComponentsInChildren<BattleEnemyItem> ();
//		foreach (var item in ei) {
//			item.DestoryUI();
//		}

		int sortCount = 0;
//		enemys.Clear ();
//		List<BattleEnemyItem> enemys = new List<BattleEnemyItem> ();
		if (enemy.Count == 0) {
			Debug.Log("no enemy");
//			sortCount++;
//			BeginSort ();
		} else {
			sortCount = enemy.Count;
			for (int i = 0; i < enemy.Count; i++) {
				TEnemyInfo tei = enemy[i];
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

				go.SetActive(true);
			}
		}
	}

	void DropItem(object data) {
		uint posSymbol = (uint)(int)data;

		if (enemyList.ContainsKey (posSymbol) && enemyList[posSymbol].enemyInfo.IsDead) {
			bbproto.EnemyInfo ei = enemyList[posSymbol].enemyInfo.EnemyInfo();
			BattleConfigData.Instance.storeBattleData.RemoveEnemyInfo(ei);
//			enemyList.Remove (posSymbol);	
			enemyList[posSymbol].DropItem();
		}
//		foreach (var item in enemyList.Values) {
//			item.DropItem(data);
//		}
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

	GameObject prevEffect;
	List<GameObject> extraEffect = new List<GameObject> ();

	void PlayerEffect(BattleEnemyItem ei, AttackInfo ai) {
		EffectManager.Instance.GetSkillEffectObject (ai.SkillID, ai.UserUnitID, returnValue => {
			if(ei != null)
				ei.InjuredShake();

			if(returnValue == null) {
				return;
			}

			GameObject prefab = returnValue as GameObject;

			string skillStoreID = DataCenter.Instance.GetSkillID(ai.UserUnitID, ai.SkillID);

			ProtobufDataBase pdb = DataCenter.Instance.AllSkill[skillStoreID];

			System.Type t = pdb.GetType();

			if(t == typeof(TSkillExtraAttack)) {
				foreach (var item in enemyList.Values) {
					if(item != null) {
						GameObject go = EffectManager.InstantiateEffect(effectPanel, prefab);
						go.transform.localPosition = item.transform.localPosition;
						extraEffect.Add(go);
					}
				}
			} else {
				Vector3 pos = prefab.transform.localPosition;
				prevEffect = EffectManager.InstantiateEffect(effectPanel, prefab);
				prevEffect.transform.localPosition = pos;
				if(ai.AttackRange == 0) {
					UITexture tex = ei.texture;
					float x = ei.transform.localPosition.x;
					float y = tex.transform.localPosition.y +  tex.height * 0.5f;

					prevEffect.transform.localPosition =  new Vector3(x, y, 0f);
				}
			}
		});
	}

	void SkillRecoverSP(object data) {
		ResourceManager.Instance.LoadLocalAsset ("Effect/jiufeng", o => {
			GameObject obj = o as GameObject;
			if (obj != null) {
				Transform trans = obj.transform;
				prevEffect = NGUITools.AddChild (effectPanel, obj);
				DGTools.CopyTransform (prevEffect.transform, trans);
			}
		});
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