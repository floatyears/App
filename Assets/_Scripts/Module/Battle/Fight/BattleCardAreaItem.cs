using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleCardAreaItem : MonoBehaviour {
	public static GameObject boostObject;

	private const int itemInterv = 17;

	private List<CardSprite> cardItemList = new List<CardSprite>();
	public List<CardSprite> CardItemList {
		get{ return cardItemList; }
	}

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
	[HideInInspector]
	private UISprite template;
	[HideInInspector]
	public List<int> haveCard = new List<int> ();

//	private BattleUseData battleUseData;
	private bool _isBoost = false;
	public bool isBoost {
		set { _isBoost = value; 
			SetBoost(value);
			if(_isBoost)  {
				boostObject = gameObject;
			}
		}
		get { return _isBoost; }
	}

	private UILabel boostLabel;

	public void Init (string name)
	{
//		base.Init(name);
		UISprite tex = GetComponent<UISprite>();
		utilityScale = new Vector3 ((float)tex.width / 4f, (float)tex.height / 4f, 1f);
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

	void SetBoost(bool show) {
		boostLabel.enabled = show;
	}
	
	void InitFightCard() {
		template = transform.FindChild("BattleCardTemplate").GetComponent<UISprite>();
		battleCardInitPos = template.transform.localPosition;
	}

	public void ShowUI () {
//		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.AddListener (CommandEnum.StartAttack, StartAttack);
//		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
//		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);
	}

	public void HideUI () {
//		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.RemoveListener (CommandEnum.StartAttack, StartAttack);
//		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
//		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);
	}

	void RecoverHP (object data) {
		Attack (data);
	}

	public int GenerateCard(List<CardItem> source) {
		Scale (true);
		int maxLimit = Config.cardCollectionCount - cardItemList.Count;
		if(maxLimit <= 0)
			return 0;
		maxLimit = maxLimit > source.Count ? source.Count : maxLimit;
		Vector3 pos = BattleManipulationView.ChangeCameraPosition() - ViewManager.Instance.ParentPanel.transform.localPosition;

		int preAttackCount = attackImage.Count;

		for (int i = 0; i < maxLimit; i++) {
			GameObject go = cardList[cardItemList.Count].gameObject;
			CardSprite ci = go.AddComponent<CardSprite>();
			ci.Init("aaa");
			DisposeTweenPosition(ci);
			DisposeTweenScale(ci);
			ci.ActorSprite.depth = GetDepth(cardItemList.Count);
			ci.SetTexture(source[i].colorType, source[i].canAttack);
			cardItemList.Add(ci);
			GenerateFightCardImmelity(source[i].colorType);
			haveCard.Add(source[i].colorType);
		}

		if (attackImage.Count > preAttackCount) {
//			Debug.LogError(" sound_title_success ");
			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_success);
		} else {
//			Debug.LogError(" sound_title_invalid ");
			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_invalid);
		}

		if (cardItemList.Count == Config.cardCollectionCount) {
			cardList[5].enabled = true;
		}

		return maxLimit;
	}

	List <AttackInfo> attackImage = new List<AttackInfo> ();

	void GenerateFightCardImmelity(int id) {

		attackImage = BattleUseData.Instance.CaculateFight (areaItemID, id, isBoost);


//		if (generateImage.Count > attackImage.Count) {
//			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_success);
//		} else {
//			AudioManager.Instance.PlayAudio (AudioEnum.sound_title_invalid);
//		}

//		attackImage = generateImage;

		InstnaceCard ();
	}

	void InstnaceCard() {
		int endNumber = battleCardTemplate.Count - attackImage.Count;
		for (int i = 0; i < endNumber; i++) {
			battleCardTemplate[attackImage.Count + i].enabled = false;
		}

		for (int i = 0; i < attackImage.Count; i++) {
			if(battleCardTemplate.Count == i) {
				CreatCard();
			}
			UISprite tex = battleCardTemplate[i];
			tex.enabled = true;
			tex.spriteName = DGTools.GetNormalSkillSpriteName(attackImage[i]); 
			attackImage[i].AttackSprite = tex;
		}
	}

	void CreatCard(){
		GameObject instance = NGUITools.AddChild (gameObject, template.gameObject);
		instance.transform.localPosition = battleCardInitPos + new Vector3 (0f, battleCardTemplate.Count * itemInterv, 0f);
		battleCardTemplate.Add(instance.GetComponent<UISprite>());
	}

//	void BattleEnd(object data) {
//		attackImage.Clear ();
//		battleCardTemplate.Clear ();
//	}

	void Attack(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}
		AttackInfo aiu = attackImage.Find (a => a.AttackID == ai.AttackID);
		attackImage.Remove (aiu);
		if (aiu != default(AttackInfo)) {
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

	void StartAttack(object data) {
		ClearCard ();
	}

	bool isScale = false;
	public void Scale(bool on) {
		if (!isScale) {
			isScale = true;
			iTween.ScaleFrom (gameObject, iTween.Hash ("scale", selfScale, "time", 0.3f, "easetype", "easeoutback","oncompletetarget",gameObject,"oncomplete","ScaleDown"));
		}
	}

	void ScaleDown() {
		isScale = false;
	}

	public void ClearCard() {
		for (int i = 0; i < cardItemList.Count; i++) {
			cardItemList[i].HideUI();
		}
		cardItemList.Clear();

		cardList [5].enabled = false; // cardlist[5] == full sprite
		haveCard.Clear ();
	}

	void DisposeTweenPosition(CardSprite ci) {
		ci.Move(GetPosition(cardItemList.Count),durationTime);
	}

	void DisposeTweenScale(CardSprite ci) {
		ci.Scale(scale,durationTime);
	}

	void DisposeTweenPosition(CardItem ci) {
		ci.Move(GetPosition(cardItemList.Count),durationTime);
	}

	void DisposeTweenScale(CardItem ci) {
		ci.Scale(scale,durationTime);
	}
	
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
		return tempPos - transform.localPosition;
	}

	int GetDepth(int sortID) {
		if(sortID == -1)
			return 0;
		return sortID == 4 ? 7 : 6;
	}
}