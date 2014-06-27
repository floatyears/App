using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private static Dictionary<uint, EnemyItem> monster = new Dictionary<uint, EnemyItem> ();
	public static Dictionary<uint, EnemyItem> Monster {
		get{
			return monster;
		}
	}

	private GameObject effectPanel;

	private GameObject effectParent;
	private GameObject effectItemPrefab;
	[HideInInspector]
	public Battle battle;
	private UISprite attackInfoLabel;
	private string[] attackInfo = new string[4] {"Nice!", "BEAUTY!", "great-!", "Excellent-!"};

	private BattleAttackInfo battleAttackInfo;

	public override void Init (string name) {
		base.Init (name);
		effectPanel = transform.Find ("Enemy/Effect").gameObject;
		effectParent = transform.Find ("Enemy").gameObject;
		effectItemPrefab = transform.Find ("Enemy/EnemyItem").gameObject;
		effectItemPrefab.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 6.5f, 0f);
		attackInfoLabel = FindChild<UISprite>("Enemy/Label");
		attackInfoLabel.spriteName = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		battleAttackInfo = FindChild<BattleAttackInfo>("Enemy/AttackInfo");
		battleAttackInfo.Init ();
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
//		Debug.LogError ("AttackEnemyEnd : " + data);
		int count = (int)data;
		DestoryEffect ();
		prevAttackInfo = null;
		if (count <= 0) {
			return;	
		}
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.spriteName = attackInfo [index];
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

		prevAttackInfo = ai;
		PlayerEffect (ei, ai);
	}

	void DestoryEffect() {
		if (prevEffect != null) {				
			Destroy(prevEffect);
		}
	}

	void End() {
		GameTimer.GetInstance ().AddCountDown (0.2f, HideAttackInfo);
	}

	void HideAttackInfo() {
		attackInfoLabel.spriteName = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		attackInfoLabel.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
	}
		                                 
	public void Refresh(List<TEnemyInfo> enemy) {
		Clear();
		List<EnemyItem> temp = new List<EnemyItem> ();
		for (int i = 0; i < enemy.Count; i++) {
			TEnemyInfo tei = enemy[i];
			tei.AddListener();
			GameObject go = NGUITools.AddChild(effectParent, effectItemPrefab);
			go.SetActive(true);
			EnemyItem ei = go.AddComponent<EnemyItem>();
			ei.battleEnemy = this;
			ei.Init(tei);
			temp.Add(ei);
			monster.Add(tei.EnemySymbol,ei);
		}
		SortEnemyItem (temp);
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
			float Difference = (centerRightWidth - centerWidth) * 0.5f;
			centerWidth += Difference;
			centerRightWidth -= Difference;
			enemys[centerIndex].transform.localPosition = new Vector3(0f - centerWidth, 0f, 0f);
			enemys[centerRightIndex].transform.localPosition = new Vector3(0f + centerRightWidth, 0f, 0f);
			DisposeCenterLeft(centerIndex--, enemys);
			centerRightIndex++;
			DisposeCenterRight(centerRightIndex , enemys);
		}
	}

	float probability;
	float allWidth;
	float screenWidth;

	void CompressTextureWidth (List<EnemyItem> enemys) {
		int count = enemys.Count;
		if (count == 1) {
			return;	
		}
		if (count == 2) {
			CompressTexture( GetProbability(Screen.width, enemys), enemys);
			return;
		}

		screenWidth = Screen.width * 0.5f;
		allWidth = 0;

		bool isOdd = DGTools.IsOddNumber (count);
		int centerIndex = count >> 1;
		if (isOdd) {
			probability = GetProbability (Screen.width, enemys);
			enemys [centerIndex].CompressTextureSize (probability);
			SetgmentationEnemys(enemys, centerIndex, screenWidth - enemys [centerIndex].texture.width * 0.5f);
		} else {
			SetgmentationEnemys(enemys, 0, screenWidth);
		}
	}

	void SetgmentationEnemys(List<EnemyItem> enemys, int centerIndex, float screenWidth) {
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
		if (lefrpro > rightpro) {
			CompressTexture (rightpro, enemys);
		} else {
			CompressTexture (lefrpro, enemys);	
		}
	}

	float GetProbability(float screenWidth, List<EnemyItem> enemys) {
		int width = 0;
		for (int i = 0; i < enemys.Count; i++) {	//Standardization texture size by rare config.
			UITexture tex = enemys [i].texture;
			width += tex.width;
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
			float leftWidth = leftItem.texture.width * 0.5f + currentEnemyItem.texture.width * 0.5f; ;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + leftWidth, 0f, 0f);
			tempIndex++;
		}
	}

	GameObject prevEffect;
	public void PlayerEffect(EnemyItem ei, AttackInfo ai) {
		EffectManager.Instance.GetSkillEffectObject (ai.SkillID, ai.UserUnitID, returnValue => {
			if(ei != null)
				ei.InjuredShake();
			if(returnValue == null) {
				return;
			}
			GameObject prefab = returnValue as GameObject;
			prevEffect = EffectManager.InstantiateEffect(effectPanel, prefab);
			if(ai.AttackRange == 0) {
				prevEffect.transform.localPosition = ei.transform.localPosition;
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