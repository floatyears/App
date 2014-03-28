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
		transform.localPosition += new Vector3 (0f, battle.cardHeight * 6.5f, 0f);
		attackInfoLabel = FindChild<UILabel>("Label");
		attackInfoLabel.transform.localScale = new Vector3 (2f, 2f, 2f);
	}

	int count = 0;
	public override void HideUI () {
		base.HideUI ();
		Clear ();
		gameObject.SetActive (false);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, AttackEnemy);
		MsgCenter.Instance.RemoveListener (CommandEnum.DropItem, DropItem);
		count --;
	}

	public override void ShowUI () {
		base.ShowUI ();
		gameObject.SetActive (true);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemyEnd, AttackEnemyEnd);
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, AttackEnemy);
		count ++;
		MsgCenter.Instance.AddListener (CommandEnum.DropItem, DropItem);
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
			temp.Add(ei);
			monster.Add(enemy[i].EnemySymbol,ei);
			if(width < temp[i].texture.width) {
				width = temp[i].texture.width;
			}
//			LogHelper.LogError("width:{0}, texture[{1}].width:{2} x H:{3}", width, i, temp[i].texture.width, temp[i].texture.height);
		}
		SortEnemyItem (temp);
	}
	float width = 0;
	void DropItem(object data) {
		int pos = (int)data;
		uint posSymbol = (uint)pos;

		if (monster.ContainsKey (posSymbol) && monster[posSymbol].enemyInfo.IsDead) {
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
	}

	float interv = 0f;

	void SortEnemyItem(List<EnemyItem> temp) {
		interv = 0f;
		int count = temp.Count;
		if (count == 0) {	return;	}
		CompressTextureWidth (temp);
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

	void CompressTextureWidth (List<EnemyItem> temp) {
		int screenWidth = Screen.width;
		int count = temp.Count;

		float allWidth =  count * width + (count - 1) * interv ;
//		LogHelper.LogError("screenWidth:{0}, allWidth:{1} width:{2}", screenWidth, allWidth, width);
		float probability = screenWidth / allWidth;
		if (probability <= 1f) { //screewidth <= allWidth
			interv = 0;
			width = (width + interv) * probability;
			for (int i = 0; i < temp.Count; i++) {
				UITexture tex = temp [i].texture;
				float tempWidth = tex.width * probability;
				float tempHeight = tex.height * probability;
				tex.width = (int)tempWidth;
				tex.height = (int)tempHeight;
			}	
		} else { 
			if( temp.Count > 1)
				interv = (screenWidth - allWidth) * 1f / (temp.Count-1);
			else
				interv = 0;
		}
	}

	void DisposeCenterLeft(int centerIndex,List<EnemyItem> temp) {
		int tempIndex = centerIndex - 1;
		while(tempIndex >= 0) {
			Vector3 localPosition = temp[tempIndex + 1].transform.localPosition;
			float rightWidth = width ;
//			LogHelper.LogError("DisposeCenterLeft :: index=[{0}] localPosition.x:{1} -  width: {2} ", tempIndex, localPosition.x, width );
			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x - rightWidth , 0f, 0f);
			tempIndex--;
		}
	}

	void DisposeCenterRight (int centerIndex, List<EnemyItem> temp) {
		int tempIndex = centerIndex;
		while(tempIndex < temp.Count) {
			Vector3 localPosition = temp[tempIndex - 1].transform.localPosition;
			float rightWidth = width ;
//			LogHelper.LogError("DisposeCenterRight :: index=[{0}] localPosition.x:{1} +  width: {2} ", tempIndex, localPosition.x, width );

			temp[tempIndex].transform.localPosition = new Vector3(localPosition.x + rightWidth, 0f, 0f);
			tempIndex++;
		}
	}
}

public class ShowEnemyUtility {
	public uint enemyID;
	public int enemyBlood;
	public int attackRound;
}



