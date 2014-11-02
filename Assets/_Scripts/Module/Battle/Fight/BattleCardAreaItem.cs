using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using bbproto;

public class BattleCardAreaItem : MonoBehaviour {

	private const int itemInterv = 17;

	private List<CardSprite> smallCardItemList = new List<CardSprite>();

	private List<UISprite> cardList = new List<UISprite> ();
	private GameObject parentObject;
	private Vector3 scale = new Vector3 (0.5f, 0.5f, 1f); 
	private Vector3 utilityScale = Vector3.zero;
	private Vector3 pos = Vector3.zero;
	private float durationTime = 0.1f;
	private Vector3 selfScale = new Vector3 (1.2f, 1.2f, 1f);
	private Vector3 battleCardInitPos;
	private List<UISprite> battleCardTemplate = new List<UISprite>();
	private int areaItemID = -1;
	public int AreaItemID {
		get {return areaItemID;}
		set {areaItemID = value;}
	}
	private UISprite template;
//	private List<int> haveCard = new List<int> ();

//	private BattleUseData battleUseData;
	private bool _isBoost = false;
	public bool isBoost {
		set { 
			boostLabel.enabled = _isBoost = value; 
		}
		get { return _isBoost; }
	}

	private UILabel boostLabel;

	public void Init (string name)
	{
//		base.Init(name);
		UISprite tex = GetComponent<UISprite>();
		utilityScale = new Vector3 ((float)tex.width / 4f, (float)tex.height / 4f, 1f);
		utilityScale.x = utilityScale.x * 0.89f; //修正Back的图标阴影大小offset
		utilityScale.y = utilityScale.y * 0.89f; //修正Back的图标阴影大小offset
		scale.x = scale.x * 0.9f; //修正Back的图标阴影大小offset
		scale.y = scale.y * 0.9f; //修正Back的图标阴影大小offset
		pos = transform.localPosition;
		parentObject = transform.parent.gameObject;
		InitFightCard ();

		boostLabel = transform.FindChild ("Label").GetComponent<UILabel>();

		for (int i = 1; i < 7; i++) {
			UISprite sprite = transform.FindChild(i.ToString()).GetComponent<UISprite>();
			if(i != 6)
				sprite.spriteName = "";

			cardList.Add(sprite);
		}
	}
	
	void InitFightCard() {
		template = transform.FindChild("BattleCardTemplate").GetComponent<UISprite>();
		battleCardInitPos = template.transform.localPosition;
	}

	public int UpdateCardAndAttackInfo(List<CardItem> source) {
//		Scale (true);
		int maxLimit = BattleConfigData.cardCollectionCount - smallCardItemList.Count;
		if(maxLimit <= 0)
			return 0;
		maxLimit = maxLimit > source.Count ? source.Count : maxLimit;
		Vector3 pos = BattleManipulationView.ChangeCameraPosition() - ViewManager.Instance.ParentPanel.transform.localPosition;

		int preAttackCount = attackInfos.Count;

		for (int i = 0; i < maxLimit; i++) {
			GameObject go = cardList[smallCardItemList.Count].gameObject;
			CardSprite ci = go.AddComponent<CardSprite>();
			ci.Init("aaa");
			ci.Move(GetPosition(smallCardItemList.Count),durationTime);
			ci.Scale(scale,durationTime);
			ci.ActorSprite.depth = GetDepth(smallCardItemList.Count);
			ci.SetTexture(source[i].colorType, source[i].canAttack);
			smallCardItemList.Add(ci);
			attackInfos = BattleAttackManager.Instance.CaculateFight (areaItemID, source[i].colorType, isBoost);
			
			int endNumber = battleCardTemplate.Count - attackInfos.Count;
			for (int j = 0; j < endNumber; j++) {
				battleCardTemplate[attackInfos.Count + j].enabled = false;
			}
			
			for (int m = 0; m < attackInfos.Count; m++) {
				if(battleCardTemplate.Count == m) {
					GameObject instance = NGUITools.AddChild (gameObject, template.gameObject);
					instance.transform.localPosition = battleCardInitPos + new Vector3 (0f, battleCardTemplate.Count * itemInterv, 0f);
					battleCardTemplate.Add(instance.GetComponent<UISprite>());
				}
				UISprite tex = battleCardTemplate[m];
				tex.enabled = true;
				tex.spriteName = DGTools.GetNormalSkillSpriteName(attackInfos[m]); 
				attackInfos[m].AttackSprite = tex;
			}

//			haveCard.Add(source[i].colorType);
		}

		if (attackInfos.Count > preAttackCount) {
//			Debug.LogError(" sound_title_success ");
			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_success);
		} else {
//			Debug.LogError(" sound_title_invalid ");
			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_invalid);
		}

		if (smallCardItemList.Count == BattleConfigData.cardCollectionCount) {
			cardList[5].enabled = true;
		}

		return maxLimit;
	}

	List <AttackInfoProto> attackInfos = new List<AttackInfoProto> ();

	public void AttackEnemy(object data) {
		AttackInfoProto ai = data as AttackInfoProto;
		if (ai == null) {
			return;		
		}

		AttackInfoProto aiu = attackInfos.Find (a => a.attackID == ai.attackID);
		attackInfos.Remove (aiu);
		if (aiu != default(AttackInfoProto)) {
			UISprite sprite = aiu.AttackSprite;
			int index = battleCardTemplate.IndexOf(sprite);
			battleCardTemplate.Remove(sprite);
			Destroy(sprite.gameObject);
			if(index < battleCardTemplate.Count && index > -1) {				//check index is vaild data
				for (int i = index; i < battleCardTemplate.Count; i++) {
//					Debug.LogError("index : " + i + " battleCardTemplate.Count : " + battleCardTemplate.Count);
					float y = battleCardTemplate[i].transform.localPosition.y - itemInterv;
					iTween.MoveTo(battleCardTemplate[i].gameObject,iTween.Hash("y",y,"time",0.2f,"easetype",iTween.EaseType.easeInCubic,"islocal",true));
				}
			}
		}
 	}

//	bool isScale = false;
//	public void Scale(bool on) {
//		if (!isScale) {
//			isScale = true;
//			iTween.ScaleFrom (gameObject, iTween.Hash ("scale", selfScale, "time", 0.3f, "easetype", "easeoutback","oncompletetarget",gameObject,"oncomplete","ScaleDown"));
//		}
//	}

//	void ScaleDown() {
//		isScale = false;
//	}

	public void ClearCard() {
		for (int i = 0; i < smallCardItemList.Count; i++) {
			smallCardItemList[i].HideUI();
		}
		smallCardItemList.Clear();

		cardList [5].enabled = false; // cardlist[5] == full sprite

		attackInfos.Clear ();
		foreach (var item in battleCardTemplate) {
			Destroy(item.gameObject);
		}
		battleCardTemplate.Clear ();
//		Debug.Log ("card area clear: " + areaItemID);
//		haveCard.Clear ();
	}
	
//	void DisposeTweenPosition(CardItem ci) {
//		ci.Move(GetPosition(cardItemList.Count),durationTime);
//	}
//
//	void DisposeTweenScale(CardItem ci) {
//		ci.Scale(scale,durationTime);
//	}
//	
	Vector3 GetPosition(int sortID) {	
		Vector3 tempPos = Vector3.zero;
		switch (sortID) {
		case 0:
			tempPos = new Vector3(pos.x - utilityScale.x,pos.y + utilityScale.y,pos.z);
			break;
		case 1:
			tempPos = new Vector3(pos.x - utilityScale.x,pos.y - utilityScale.y,pos.z);
			break;
		case 2:
			tempPos = new Vector3(pos.x + utilityScale.x,pos.y + utilityScale.y,pos.z);
			break;
		case 3:
			tempPos = new Vector3(pos.x + utilityScale.x,pos.y - utilityScale.y,pos.z);
			break;
		case 4:
			tempPos = new Vector3(pos.x ,pos.y,pos.z);
			break;
		default:
			break;
		}

		tempPos.x -= utilityScale.x*0.2f; //修正Back的图标阴影大小offset
		tempPos.y += utilityScale.y*0.15f;

		return tempPos - transform.localPosition;
	}

	int GetDepth(int sortID) {
		if(sortID == -1)
			return 0;
		return sortID == 4 ? 7 : 6;
	}

	public void DestroyUI(){

	}
}