using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : UIBaseUnity {
	private List<EnemyItem> monster = new List<EnemyItem> ();
	private GameObject tempGameObject;
	[HideInInspector]
	public Battle battle;

	public override void Init (string name)
	{
		base.Init (name);
		tempGameObject = transform.Find ("EnemyItem").gameObject;
		tempGameObject.SetActive (false);
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 5f, 0f);
	}

	public override void HideUI ()
	{
		base.HideUI ();
		for (int i = 0; i < monster.Count; i++) {
			if(monster[i] == null) {
				monster.RemoveAt(i);
				continue;
			}
			monster[i].DestoryUI();
		}
		monster.Clear ();
		gameObject.SetActive (false);
	}

	public override void ShowUI ()
	{
		base.ShowUI ();
		gameObject.SetActive (true);
	}

	void ShowEnemy (object enemyList) {
		List<ShowEnemyUtility> enemy = (List<ShowEnemyUtility>)enemyList;
	}

	public void Refresh(List<ShowEnemyUtility> enemy) {
		Clear();

		for (int i = 0; i < enemy.Count; i++) {
			GameObject go = NGUITools.AddChild(gameObject,tempGameObject);
			go.SetActive(true);
			CaculatePosition(i,go);
			EnemyItem ei = go.AddComponent<EnemyItem>();
		
			ei.Init(enemy[i]);
			monster.Add(ei);
		}
	}

	void Clear() {
		for (int i = 0; i < monster.Count; i++) {
			if(monster[i] == null ){
				monster.RemoveAt(i);
			}
			Destroy(monster[i].gameObject);
		}
		monster.Clear();
	}

	void CaculatePosition(int index,GameObject target) {
		target.transform.localPosition = new Vector3(index * 180, 0f, 0f) ;
	}
}

public class ShowEnemyUtility {
	public int enemyID;
	public int enemyBlood;
	public int attackRound;
}



