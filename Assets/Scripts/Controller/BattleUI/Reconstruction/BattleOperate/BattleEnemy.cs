using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private static Dictionary<uint, EnemyItem> monster = new Dictionary<uint, EnemyItem> ();
	public static Dictionary<uint, EnemyItem> Monster {
		get{
			return monster;
		}
	}
	private GameObject tempGameObject;
	[HideInInspector]
	public Battle battle;

	private UILabel attackInfoLabel;

	private string[] attackInfo = new string[4] {"Nice", "Good", "Great", "Excellent"};

	public override void Init (string name) {
		base.Init (name);
		tempGameObject = transform.Find ("EnemyItem").gameObject;
		tempGameObject.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 5.5f, 0f);
		attackInfoLabel = FindChild<UILabel>("Label");
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
	}

	public override void HideUI () {
		base.HideUI ();
		Clear ();
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
	}

	void AttackEnemyEnd(object data) {
		int index = DGTools.RandomToInt (0, 4);
		attackInfoLabel.text = attackInfo [index];
		iTween.ScaleTo (attackInfoLabel.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "time", 0.5f, "easetype", iTween.EaseType.easeInQuart, "oncomplete", "End", "oncompletetarget", gameObject));
	}

	void AttackEnemy(object data) {

	}

	void End() {
		attackInfoLabel.text = "";
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
	}

	public void Refresh(List<TEnemyInfo> enemy) {
		Clear();
		for (int i = 0; i < enemy.Count; i++) {
			GameObject go = NGUITools.AddChild(gameObject,tempGameObject);
			go.SetActive(true);
 			CaculatePosition(i,go);
			EnemyItem ei = go.AddComponent<EnemyItem>();
			ei.Init(enemy[i]);
			monster.Add(enemy[i].GetID(),ei);
		}
	}

	void Clear() {
		foreach (var item in monster) {
			if(item.Value != null) {
				item.Value.DestoryUI();
			}
		}
		monster.Clear();
	}

	void CaculatePosition(int index,GameObject target) {
		target.transform.localPosition = new Vector3(index * 180, 0f, 0f) ;
	}
}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}



