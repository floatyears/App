using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private static Dictionary<uint, EnemyItem> monster = new Dictionary<uint, EnemyItem> ();
	public static Dictionary<uint, EnemyItem> Monster {
		get {
				return monster;
		}
	}

	private GameObject effectPanel;

	private GameObject effectParent;
	private GameObject effectItemPrefab;
	[HideInInspector]
	public Battle battle;

	private UILabel attackInfoLabel;
//	private string[] attackInfo = new string[4] {"Nice!", "BEAUTY!", "great-!", "Excellent-!"};
	private string[] attackInfo = new string[4] {"Nice !", "BEAUTY !", "great !", "Excellent !"};
	private BattleAttackInfo battleAttackInfo;
	private UITexture bgTexture;

	public override void Init (string name) {
		base.Init (name);
		effectPanel = transform.Find ("Enemy/Effect").gameObject;
		effectParent = transform.Find ("Enemy").gameObject;
		effectItemPrefab = transform.Find ("Enemy/EnemyItem").gameObject;
		effectItemPrefab.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 6.5f, 0f);
		attackInfoLabel = FindChild<UILabel>("Enemy/Label");
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		battleAttackInfo = FindChild<BattleAttackInfo>("Enemy/AttackInfo");
		battleAttackInfo.Init ();
		bgTexture = FindChild<UITexture>("Texture");
		string path = "Texture/Map/fight_" + ConfigBattleUseData.Instance.GetMapID ().ToString ();
		ResourceManager.Instance.LoadLocalAsset (path, o => {
						bgTexture.mainTexture = o as Texture2D;
		});
	}

	int count = 0;
	public override void HideUI () {
		base.HideUI ();
		Clear ();
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.DropItem, DropItem);
		MsgCenter.Instance.RemoveListener (CommandEnum.SkillRecoverSP, SkillRecoverSP);
		MsgCenter.Instance.RemoveListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.PlayAllEffect, PlayAllEffect);
		count --;
		battleAttackInfo.HideUI ();
		gameObject.SetActive (false);
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		count ++;
		battleAttackInfo.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.DropItem, DropItem);
		MsgCenter.Instance.AddListener (CommandEnum.SkillRecoverSP, SkillRecoverSP);
		MsgCenter.Instance.AddListener (CommandEnum.ExcuteActiveSkill, ExcuteActiveSkillEnd);
		MsgCenter.Instance.AddListener (CommandEnum.PlayAllEffect, PlayAllEffect);
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

	void AttackEnemy(object data) {
		DestoryEffect ();
	}

	List<EnemyItem> enemyList = new List<EnemyItem>();

	AttackInfo prevAttackInfo = null;

	public void EnemyItemPlayEffect(EnemyItem ei, AttackInfo ai) {
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

	private List<EnemyItem> enemys = new List<EnemyItem> ();	 

	public void Refresh(List<TEnemyInfo> enemy) {
		Clear();
		sortCount = 0;
		enemys.Clear ();
		if (enemy.Count == 0) {
			Debug.Log("no enemy");
			sortCount++;
			BeginSort ();
		} else {
			sortCount = enemy.Count;
			for (int i = 0; i < enemy.Count; i++) {
				TEnemyInfo tei = enemy[i];
				tei.AddListener();
				GameObject go = NGUITools.AddChild(effectParent, effectItemPrefab);
				go.SetActive(true);
				EnemyItem ei = go.AddComponent<EnemyItem>();
				enemys.Add(ei);
				monster.Add(tei.EnemySymbol,ei);
				ei.battleEnemy = this;
				ei.Init(tei, BeginSort);
			}
		}
	}

	int sortCount = 0;

	void BeginSort() {
		sortCount--;

		if (sortCount == 0) {
			SortEnemyItem (enemys);
		} 
	}

	void DropItem(object data) {
		int pos = (int)data;
		uint posSymbol = (uint)pos;

		if (monster.ContainsKey (posSymbol) && monster[posSymbol].enemyInfo.IsDead) {
			bbproto.EnemyInfo ei = monster[posSymbol].enemyInfo.EnemyInfo();
			ConfigBattleUseData.Instance.storeBattleData.RemoveEnemyInfo(ei);
			monster.Remove (posSymbol);	
		}
	}

	void Clear() {
		foreach (var item in monster) {
			if(item.Value != null) {
				item.Value.DestoryUI();
			}
		}
		monster.Clear();
		EnemyItem[] ei = transform.GetComponentsInChildren<EnemyItem> ();
		foreach (var item in ei) {
			item.DestoryUI();
		}
	}

	void SortEnemyItem(List<EnemyItem> enemys) {
		int count = enemys.Count;
		if (count == 0) {	return;	}
		CompressTextureWidth (enemys);
		if (count == 1) { 
			enemys[0].transform.localPosition = Vector3.zero; 
			return; 
		}
		int centerIndex = 0;
		if (DGTools.IsOddNumber (count)) {
			centerIndex = count >> 1;
			enemys[centerIndex].transform.localPosition = Vector3.zero;
			DisposeCenterLeft(centerIndex, enemys);
			DisposeCenterRight(centerIndex, enemys);
		} else {
			centerIndex = (count >> 1) - 1;
			int centerRightIndex = centerIndex + 1;
			float centerWidth = enemys[centerIndex].texture.width * 0.5f;
			float centerRightWidth =  enemys[centerRightIndex].texture.width * 0.5f;
			float Difference = (centerRightWidth - centerWidth);
			centerWidth += Difference;	
			centerRightWidth -= Difference;
			enemys[centerIndex].transform.localPosition = new Vector3(0f - centerWidth, 0f, 0f);
			enemys[centerRightIndex].transform.localPosition = new Vector3(0f + centerRightWidth, 0f, 0f);
			DisposeCenterLeft(centerIndex--, enemys);
			centerRightIndex++;
			DisposeCenterRight(centerRightIndex, enemys);
		}
	}

	float probability;
	float allWidth;
	float screenWidth;
	public const int ScreenWidth = 640;
	void CompressTextureWidth (List<EnemyItem> enemys) {
		int count = enemys.Count;
		if (count == 1) {
			return;	
		}
		if (count == 2) {
			CompressTexture( GetProbability(ScreenWidth, enemys), enemys);
			return;
		}

		screenWidth = ScreenWidth * 0.5f;
		allWidth = 0;

		bool isOdd = DGTools.IsOddNumber (count);
		int centerIndex = count >> 1;
		probability = 1.0f;
		if (isOdd) {
			float allPro = GetProbability (ScreenWidth, enemys);
			probability = SetgmentationEnemys(enemys, centerIndex, screenWidth - enemys [centerIndex].texture.width * 0.5f);
			if( probability > allPro ) {
				probability = allPro;
			}
		} else {
			probability = SetgmentationEnemys(enemys, 0, screenWidth);
		}

		CompressTexture (probability, enemys);
	}

	float SetgmentationEnemys(List<EnemyItem> enemys, int centerIndex, float screenWidth) {
		int leftEndIndex = centerIndex ;
		int rightStartIndex = centerIndex + 1;

		if (centerIndex == 0) {
			rightStartIndex = leftEndIndex = enemys.Count >> 1;
		}

		List<EnemyItem> leftEnemys = new List<EnemyItem> ();
		List<EnemyItem> rightEnemys = new List<EnemyItem> ();

		for (int i = 0; i < leftEndIndex; i++) {
			leftEnemys.Add (enemys [i]);
		}
		
		for (int i = rightStartIndex; i < enemys.Count; i++) {
			rightEnemys.Add (enemys [i]);
		}

		float lefrpro = GetProbability (screenWidth, leftEnemys);
		float rightpro = GetProbability (screenWidth, rightEnemys);

		return (lefrpro < rightpro ? lefrpro : rightpro);
	}

	float GetProbability(float screenWidth, List<EnemyItem> enemys) {
		int width = 0;
		for (int i = 0; i < enemys.Count; i++) {	//Standardization texture size by rare config.
			UITexture tex = enemys [i].texture;
			width += tex.width;
		}
		if( screenWidth >= width ) {
			return 1.0f;
		}
		return screenWidth / width;
	}

	void CompressTexture(float probability, List<EnemyItem> enemys) {
		for (int i = 0; i < enemys.Count; i++) {
			enemys[i].CompressTextureSize(probability);
		}
	}

	void DisposeCenterLeft(int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex - 1;
		while(tempIndex >= 0) {
			EnemyItem rightEnemyItem = temp[tempIndex + 1];
			EnemyItem currentEnemyItem = temp[tempIndex];
			Vector3 localPosition = rightEnemyItem.transform.localPosition;
			float rightWidth = rightEnemyItem.texture.width * 0.5f + currentEnemyItem.texture.width * 0.5f;
			currentEnemyItem.transform.localPosition = new Vector3(localPosition.x - rightWidth , 0f, 0f);
			tempIndex--;
		}
	}

	void DisposeCenterRight (int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex;
		while(tempIndex < temp.Count) {
			EnemyItem leftItem = temp[tempIndex - 1];
			EnemyItem currentEnemyItem = temp[tempIndex];
		
			Vector3 localPosition = leftItem.transform.localPosition;
			float leftWidth = leftItem.texture.width * 0.5f + currentEnemyItem.texture.width * 0.5f; 
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + leftWidth, 0f, 0f);
			tempIndex++;
		}
	}

	GameObject prevEffect;
	List<GameObject> extraEffect = new List<GameObject> ();

	public void PlayerEffect(EnemyItem ei, AttackInfo ai) {
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
				foreach (var item in monster.Values) {
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

	public void PlayAllEffect () {

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
	
	void ExcuteActiveSkillEnd(object data) {
		bool b = (bool)data;
		if (!b) {
			DestoryEffect();
		}
	}
}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}