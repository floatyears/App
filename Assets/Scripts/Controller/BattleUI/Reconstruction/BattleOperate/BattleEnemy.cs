using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private static Dictionary<uint, EnemyItem> monster = new Dictionary<uint, EnemyItem> ();
	public static Dictionary<uint, EnemyItem> Monster {
		get{
			return monster;
		}
	}

//	public List<TEnemyInfo> monsterList = new List<TEnemyInfo> ();

	private GameObject effectPanel;

	private GameObject tempGameObject;
	[HideInInspector]
	public Battle battle;
	private UISprite attackInfoLabel;
	private string[] attackInfo = new string[4] {"Nice!", "BEAUTY!", "great-!", "Excellent-!"};

	private BattleAttackInfo battleAttackInfo;

	public override void Init (string name) {
		base.Init (name);
		effectPanel = transform.Find ("Effect").gameObject;
		tempGameObject = transform.Find ("EnemyItem").gameObject;
		tempGameObject.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 6.5f, 0f);
		attackInfoLabel = FindChild<UISprite>("Label");
		attackInfoLabel.spriteName = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
		battleAttackInfo = FindChild<BattleAttackInfo>("AttackInfo");
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


	}

	void AttackEnemyEnd(object data) {
		DestoryEffect ();
		prevAttackInfo = null;
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.spriteName = attackInfo [index];
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.3f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "End", "oncompletetarget", gameObject));
	}
	
	List<AttackInfo> attackList=  new List<AttackInfo>();

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
//			Debug.LogError(tei.EnemyID + " hp : " + tei.initBlood);
			GameObject go = NGUITools.AddChild(gameObject,tempGameObject);
			go.SetActive(true);
			EnemyItem ei = go.AddComponent<EnemyItem>();
			ei.battleEnemy = this;
			ei.Init(tei);
			temp.Add(ei);
			monster.Add(tei.EnemySymbol,ei);
			if(width < temp[i].texture.width) {
				width = temp[i].texture.width;
			}
		}
		SortEnemyItem (temp);
	}
	float width = 0;
	void DropItem(object data) {
		int pos = (int)data;
		uint posSymbol = (uint)pos;

		if (monster.ContainsKey (posSymbol) && monster[posSymbol].enemyInfo.IsDead) {
			bbproto.EnemyInfo ei = monster[posSymbol].enemyInfo.EnemyInfo();
			ConfigBattleUseData.Instance.storeBattleData.RemoveEnemyInfo(ei);
//			monsterList.Remove(tei);
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

//	float interv = 0f;

	void SortEnemyItem(List<EnemyItem> temp) {
//		interv = 0f;
		int count = temp.Count;
		if (count == 0) {	return;	}
		CompressTextureWidth (temp);
//		Debug.LogError ("width : " + width);
		if (count == 1) { 
			temp[0].transform.localPosition = Vector3.zero; 
			return; 
		}
		int centerIndex = 0;
		if (DGTools.IsOddNumber (count)) {
			centerIndex = ((count + 1) >> 1) - 1;
			temp[centerIndex].transform.localPosition = Vector3.zero;
			DisposeCenterLeft(centerIndex, temp);
			DisposeCenterRight(centerIndex,temp);
		} else {
			centerIndex = (count >> 1) - 1;
			int centerRightIndex = centerIndex + 1;
			float tempinterv = width * 0.5f;
			temp[centerIndex].transform.localPosition = new Vector3(0f - tempinterv, 0f, 0f);
			temp[centerRightIndex].transform.localPosition = new Vector3(0f + tempinterv, 0f, 0f);
			DisposeCenterLeft(centerIndex--, temp);
			centerRightIndex++;

			DisposeCenterRight(centerRightIndex , temp);
		}
	}
	float probability;
	float allWidth;
	int screenWidth;
	void CompressTextureWidth (List<EnemyItem> temp) {
		screenWidth = Screen.width;
		int count = temp.Count;
		if (!DGTools.IsOddNumber (count)) {
			count ++;
		}
		allWidth =  count * width ;
		probability = screenWidth / allWidth;
		if (probability <= 1f) { //screewidth <= allWidth
			width *= probability;
			for (int i = 0; i < temp.Count; i++) {
				UITexture tex = temp [i].texture;
				float tempWidth = tex.width * probability;
				float tempHeight = tex.height * probability;
				tex.width = (int)tempWidth;
				tex.height = (int)tempHeight;
			}
		}	
	}

	void DisposeCenterLeft(int centerIndex,List<EnemyItem> temp) {
		int tempIndex = centerIndex - 1;
		while(tempIndex >= 0) {
			Vector3 localPosition = temp[tempIndex + 1].transform.localPosition;
			float rightWidth = width ;

			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x - rightWidth , 0f, 0f);
			tempIndex--;
		}
	}

	void DisposeCenterRight (int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex;
		while(tempIndex < temp.Count) {
			Vector3 localPosition = temp[tempIndex - 1].transform.localPosition;
			float rightWidth = width ;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + rightWidth, 0f, 0f);
			tempIndex++;
		}
	}
	GameObject prevEffect;
	public void PlayerEffect(EnemyItem ei,AttackInfo ai) {
		GameObject obj = EffectManager.Instance.GetEffectObject (ai.SkillID); //DataCenter.Instance.GetEffect(ai) as GameObject;
//		DGTools.PlayAttackSound(ai.AttackType);
		ei.InjuredShake();
		if (obj != null) {
			Vector3 localScale = obj.transform.localScale;
			prevEffect = NGUITools.AddChild(effectPanel, obj);
			prevEffect.transform.localScale = localScale;
			if(ai.AttackRange == 0) {
				prevEffect.transform.localPosition = ei.transform.localPosition;
			}
		}
	}

	public void PlayAllEffect() {

	}

	void SkillRecoverSP(object data) {
		GameObject obj = Resources.Load("Effect/jiufeng") as GameObject;
		if (obj != null) {
			Transform trans = obj.transform;
			prevEffect = NGUITools.AddChild(effectPanel, obj);
			DGTools.CopyTransform(prevEffect.transform, trans);
		}
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



