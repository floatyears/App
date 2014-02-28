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
		List<EnemyItem> temp = new List<EnemyItem> ();
		for (int i = 0; i < enemy.Count; i++) {
			GameObject go = NGUITools.AddChild(gameObject,tempGameObject);
			go.SetActive(true);

			EnemyItem ei = go.AddComponent<EnemyItem>();
			ei.Init(enemy[i]);
//			CaculatePosition(i,enemy.Count,ei);
			temp.Add(ei);
			monster.Add(enemy[i].GetID(),ei);
		}
		SortEnemyItem (temp);
	}

	void Clear() {
		foreach (var item in monster) {
			if(item.Value != null) {
				item.Value.DestoryUI();
			}
		}
		monster.Clear();
	}

	void SortEnemyItem(List<EnemyItem> temp) {
		int count = temp.Count;
		if (count == 0) {	return;	}
		if (count == 1) { temp[0].transform.localPosition = Vector3.zero; }
		int centerIndex = 0;
		if (DGTools.IsOddNumber (count)) {
			centerIndex = ((count + 1) >> 1) - 1;		
//			Debug.LogError("centerIndex : " + centerIndex);
			temp[centerIndex].transform.localPosition = Vector3.zero;
			DisposeCenterLeft(centerIndex, temp);
			DisposeCenterRight(centerIndex,temp);
//			int tempIndex = centerIndex - 1;
//			Debug.LogError("centerIndex : " + centerIndex + " count ; " + count + " tempIndex : " + tempIndex);
//			while(tempIndex >= 0) {
//				Vector3 localPosition = temp[tempIndex + 1].transform.localPosition;
//				temp[tempIndex].transform.localPosition = new Vector3(localPosition.x - (temp[tempIndex].texture.width >> 1), 0f, 0f);
//				tempIndex--;
//			}
//			tempIndex = centerIndex;
//			while(tempIndex< count) {
//				Vector3 localPosition = temp[tempIndex - 1].transform.localPosition;
//				temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + (temp[tempIndex].texture.width >> 1), 0f, 0f);
//				tempIndex++;
//			}
		} else {
			centerIndex = count >> 1;
			int centerRightIndex = centerIndex + 1;
			temp[centerIndex].transform.localPosition = new Vector3(0f - (temp[centerIndex].texture.width >> 1),0f,0f);
			DisposeCenterLeft(centerIndex, temp);
			DisposeCenterLeft(centerRightIndex, temp);
		}

	}

	void DisposeCenterLeft(int centerIndex,List<EnemyItem> temp) {
		int tempIndex = centerIndex - 1;
		while(tempIndex >= 0) {
			Vector3 localPosition = temp[tempIndex + 1].transform.localPosition;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x - (temp[tempIndex].texture.width >> 1), 0f, 0f);
			tempIndex--;
		}
	}

	void DisposeCenterRight (int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex;
		while(tempIndex < temp.Count) {
			Vector3 localPosition = temp[tempIndex - 1].transform.localPosition;
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + (temp[tempIndex].texture.width >> 1), 0f, 0f);
			tempIndex++;
		}
	}

//	void CaculatePosition(int index,int max ,EnemyItem tex) {
//		if (max <= 0) {	return;	}
//		if(max == 1) { tex.transform.localPosition = Vector3.zero; }
//		int width = tex.texture.width;
//		float centerIndex = 1;
//		if (DGTools.IsOddNumber (max)) {
//			centerIndex = (max + 1) >> 1;
//			Debug.LogError ("centerIndex : " + centerIndex);
//		} else {
//			centerIndex = max >> 1 + 0.5f;
//		}
//
//
//	}
}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}



