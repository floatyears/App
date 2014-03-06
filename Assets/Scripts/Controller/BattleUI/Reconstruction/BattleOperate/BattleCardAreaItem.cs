using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleCardAreaItem : UIBaseUnity {
	private List<CardSprite> cardItemList = new List<CardSprite>();
	public List<CardSprite> CardItemList {
		get{return cardItemList;}
	}

	private List<UISprite> cardList = new List<UISprite> ();
	private GameObject parentObject;
	private Vector3 scale = new Vector3 (0.5f, 0.5f, 1f);
	private Vector3 utilityScale = Vector3.zero;
	private Vector3 pos = Vector3.zero;
	private float durationTime = 0.1f;
	private Vector3 selfScale = new Vector3 (1.2f, 1.2f, 1f);
	private Vector3 battleCardInitPos ;
	private List<UITexture> battleCardTemplate = new List<UITexture>();

	private int areaItemID = -1;
	public int AreaItemID {
		get {return areaItemID;}
		set {areaItemID = value;}
	}

	private UITexture template;

	public override void Init(string name) {
		base.Init(name);
		UITexture tex = GetComponent<UITexture>();
		utilityScale = new Vector3 ((float)tex.width / 4f, (float)tex.height / 4f, 1f);
		pos = transform.localPosition;
		parentObject = transform.parent.gameObject;
		InitFightCard ();
		for (int i = 1; i < 6; i++) {
			UISprite sprite = FindChild<UISprite>(i.ToString());
			sprite.spriteName = "";
			cardList.Add(sprite);
		}
	}

	void InitFightCard() {
		template = FindChild<UITexture> ("BattleCardTemplate");
		battleCardTemplate.Add(template);
		battleCardInitPos = template.transform.localPosition;
	}

	public override void ShowUI () {
		base.ShowUI ();
		MsgCenter.Instance.AddListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.AddListener (CommandEnum.StartAttack, StartAttack);
		MsgCenter.Instance.AddListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.AddListener (CommandEnum.RecoverHP, RecoverHP);
	}

	public override void HideUI () {
		base.HideUI ();
		MsgCenter.Instance.RemoveListener (CommandEnum.AttackEnemy, Attack);
		MsgCenter.Instance.RemoveListener (CommandEnum.StartAttack, StartAttack);
		MsgCenter.Instance.RemoveListener (CommandEnum.BattleEnd, BattleEnd);
		MsgCenter.Instance.RemoveListener (CommandEnum.RecoverHP, RecoverHP);
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
		Vector3 pos = Battle.ChangeCameraPosition() - vManager.ParentPanel.transform.localPosition;
		for (int i = 0; i < maxLimit; i++) {

			GameObject go = cardList[cardItemList.Count].gameObject;

			CardSprite ci = go.AddComponent<CardSprite>();
			ci.Init("aaa");
			DisposeTweenPosition(ci);
			DisposeTweenScale(ci);
			ci.ActorSprite.depth = GetDepth(cardItemList.Count);
			ci.SetTexture(source[i].itemID);
			cardItemList.Add(ci);
			GenerateFightCardImmelity(source[i].itemID);
		}

		return maxLimit;
	}
	List <AttackImageUtility> attackImage = new List<AttackImageUtility> ();

	void GenerateFightCardImmelity(int id) {
		attackImage = BattleQuest.bud.CaculateFight (areaItemID,id);
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
			UITexture tex = battleCardTemplate[i];
			tex.enabled = true;
			tex.color = ItemData.GetColor (attackImage[i].attackProperty);
			attackImage[i].attackUI = tex;
		}
	}

	void CreatCard(){
		GameObject instance = Instantiate (template.gameObject) as GameObject;
		instance.transform.parent = transform;
		instance.transform.localScale = Vector3.one;
		instance.layer = gameObject.layer;
		instance.transform.localPosition = battleCardInitPos + new Vector3 (0f, battleCardTemplate.Count * 10f, 0f);
		battleCardTemplate.Add(instance.GetComponent<UITexture>());
	}

	void BattleEnd(object data) {
		for (int i = 0; i < attackImage.Count; i++) {
			attackImage[i].attackUI.enabled = false;
		}
	}

	void Attack(object data) {
		AttackInfo ai = data as AttackInfo;
		if (ai == null) {
			return;		
		}

		AttackImageUtility aiu = attackImage.Find (a => a.attackID == ai.AttackID);

		if (aiu != default(AttackImageUtility)) {
			aiu.attackUI.enabled = false;	
		}
 	}

	void StartAttack(object data) {
		ClearCard ();
	}

	bool isScale = false;
	public void Scale(bool on)
	{
//		if (on) {
		if (!isScale) {
			isScale = true;
			iTween.ScaleFrom (gameObject, iTween.Hash ("scale", selfScale, "time", 0.3f, "easetype", "easeoutback","oncompletetarget",gameObject,"oncomplete","ScaleDown"));
		}
//		} 
//		else {
//			iTween.ScaleTo(gameObject,iTween.Hash("scale",Vector3.one,"time",0.1f,"easetype","easeoutcubic"));
//		}
	}

	void ScaleDown() {
		isScale = false;
	}

	public void ClearCard()
	{
		for (int i = 0; i < cardItemList.Count; i++) {
			cardItemList[i].HideUI();
		}
		cardItemList.Clear();
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

	int GetDepth(int sortID)
	{
		if(sortID == -1)
			return 0;

		return sortID == 4 ? 2 : 1;
	}



}