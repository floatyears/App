using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleCardAreaItem : UIBaseUnity
{
	private List<CardItem> cardItemList = new List<CardItem>();
	public List<CardItem> CardItemList 
	{
		get{return cardItemList;}
	}

	private List<int> battleList = new List<int> ();
	/// <summary>
	/// this item's fight card
	/// </summary>
	/// <value>The battle list.</value>
	public List<int> BattleList{
		get{
			return battleList;
		}
	}

	private GameObject parentObject;

	private Vector3 scale = new Vector3 (0.5f, 0.5f, 1f);

	private Vector3 utilityScale = Vector3.zero;

	private Vector3 pos = Vector3.zero;

	private float durationTime = 0.1f;

	private Vector3 selfScale = new Vector3 (1.2f, 1.2f, 1f);

	private Vector3 battleCardInitPos ;

	private UITexture[] battleCardTemplate;

	public override void Init(string name)
	{
		base.Init(name);

		UITexture tex = GetComponent<UITexture>();

		utilityScale = new Vector3 ((float)tex.width / 4f, (float)tex.height / 4f, 1f);

		pos = transform.localPosition;

		parentObject = transform.parent.gameObject;

		InitFightCard ();
	}

	void InitFightCard()
	{
		battleCardTemplate = new UITexture[Config.cardCollectionCount];
		UITexture template = FindChild<UITexture> ("BattleCardTemplate");
		battleCardTemplate [0] = template;
		battleCardInitPos = template.transform.localPosition;
		for (int i = 1; i < Config.cardCollectionCount; i++) {
			GameObject instance = Instantiate (template.gameObject) as GameObject;
			instance.transform.parent = transform;
			instance.transform.localScale = Vector3.one;
			instance.layer = gameObject.layer;
			instance.transform.localPosition = battleCardInitPos + new Vector3 (0f, i * 10f, 0f);
			battleCardTemplate[i] = instance.GetComponent<UITexture>();
		}
	}

	public override void HideUI ()
	{
		base.HideUI ();

		ClearCard ();
	}

	public int GenerateCard(List<CardItem> source)
	{
		Scale (true);
		int maxLimit = Config.cardCollectionCount - cardItemList.Count;

		if(maxLimit <= 0)
			return 0;
	
		maxLimit = maxLimit > source.Count ? source.Count : maxLimit;

		Vector3 pos = Battle.ChangeCameraPosition() - vManager.ParentPanel.transform.localPosition;

		//float time = Time.realtimeSinceStartup;

//		for (int i = 0; i < maxLimit; i++) 
//		{
//			tempSource.Add(source[i]);
//		}
//
//		StartCoroutine (DisposeTexture(maxLimit));

		for (int i = 0; i < maxLimit; i++)
		{
			tempObject = BattleCardArea.GetCard();
			tempObject.layer = parentObject.layer;
			tempObject.transform.localPosition = pos;
			tempObject.transform.parent = parentObject.transform;
			CardItem ci = tempObject.GetComponent<CardItem>();
	
			ci.Init(tempObject.name);

			DisposeTweenPosition(ci);

			DisposeTweenScale(ci);

			ci.ActorTexture.depth = GetDepth(cardItemList.Count);
	
			ci.SetTexture( source[i].ActorTexture.mainTexture,source[i].itemID);

			cardItemList.Add(ci);

			StartCoroutine(GenerateFightCard(source[i].itemID));
		}

		return maxLimit;
	}

	IEnumerator GenerateFightCard(int id){
		yield return 1;
		int itemID = Config.Instance.CardData [id].itemID;

		MsgCenter.Instance.Invoke (CommandEnum.DragCardToBattleArea, itemID);
		int gID = BattleDataMode.GenerateCard (id, itemID);

		InstnaceCard (gID);
		battleList.Add(gID);
	}

	void InstnaceCard(int gID){
		if (battleList.Count > Config.cardCollectionCount)
			return;
		UITexture tex = battleCardTemplate[battleList.Count];
		tex.enabled = true;
		tex.color = ItemData.GetColor (gID);
	}

	public void Scale(bool on)
	{
		if (on) {
			iTween.ScaleFrom (gameObject, iTween.Hash ("x", selfScale.x, "y", selfScale.y, "time", 0.3f, "easetype", "easeoutback"));
		} 
		else {
			iTween.ScaleTo(gameObject,iTween.Hash("x",1f,"y",1f,"time",0.1f,"easetype","easeoutcubic"));
		}
	}

//	int startIndex = 0;
////	Texture tempTex;
//	List<CardItem> tempSource = new List<CardItem>();
//
//	IEnumerator DisposeTexture(int count)
//	{
//		tempObject = BattleCardArea.GetCard();
//		tempObject.layer = parentObject.layer;
//		tempObject.transform.localPosition = pos;
//		tempObject.transform.parent = parentObject.transform;
//		CardItem ci = tempObject.GetComponent<CardItem>();
//		ci.Init(tempObject.name);
//		DisposeTweenPosition(ci);
//		DisposeTweenScale(ci);
//		ci.ActorTexture.depth = GetDepth(cardItemList.Count);
//		ci.SetTexture( tempSource[0].ActorTexture.mainTexture,tempSource[0].itemID);
//		cardItemList.Add(ci);
//		tempSource.RemoveAt (0);
//
//		yield return 1;
//
//		if (tempSource.Count > 0)
//		{
//			StartCoroutine (DisposeTexture (count));
//		}
//	}

	public void ClearCard()
	{
		for (int i = 0; i < cardItemList.Count; i++)
		{
			cardItemList[i].HideUI();
		}

		cardItemList.Clear();

		for (int i = 0; i<battleCardTemplate.Length; i++) {
			if(battleCardTemplate[i].enabled){
				battleCardTemplate[i].enabled = false;
			}
			battleList.Clear();
		}
	}

	void DisposeTweenPosition(CardItem ci)
	{
		ci.Move(GetPosition(cardItemList.Count),durationTime);
	}

	void DisposeTweenScale(CardItem ci)
	{
		ci.Scale(scale,durationTime);
	}
	
	Vector3 GetPosition(int sortID)
	{	
		Vector3 tempPos = Vector3.zero;
		
		switch (sortID) 
		{
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

		return tempPos;
	}

	int GetDepth(int sortID)
	{
		if(sortID == -1)
			return 0;

		return sortID == 4 ? 2 : 1;
	}



}