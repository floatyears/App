using UnityEngine;
using System.Collections.Generic;

public class CardPoolSingleItem : UIComponentUnity 
{
	protected UITexture templateBackTexture ;



	protected UITexture templateItemTexture;

	protected UITexture itemTexture;
	
	//protected TweenPosition tweenPosition;
	
	private Vector3 initPosition = Vector3.zero;

	private int initDepth = 3;

	public float interv = 10f;

	protected int location = -1;

	private Vector3[] cardPosition;

	public Vector3[] CardPosition
	{
		get
		{
			return cardPosition;
		}
	}


	protected List<CardItem> itemCardList = new List<CardItem>();
//	private GameObject tempObject;
	private UITexture tempTexture;

	public override void Init (string name)
	{
		base.Init (name);

		cardPosition = new Vector3[Config.cardPoolSingle];

		UITexture templateTexture = FindChild<UITexture>("Back");

		interv += templateTexture.width;

		GameObject go = templateTexture.gameObject;

		initPosition = go.transform.localPosition;

		for (int i = 0; i < cardPosition.Length; i++) 
		{
			tempObject = NGUITools.AddChild(gameObject,go);
			tempObject.transform.localPosition = new Vector3(initPosition.x + i * interv,initPosition.y,initPosition.z);
			cardPosition[i] = tempObject.transform.localPosition;
		}

		templateItemTexture = FindChild<UITexture>("ItemTexture");

		templateItemTexture.depth = initDepth;

		templateTexture.enabled = false;

		templateItemTexture.enabled = false;
	}

	public virtual void SetInitPosition(int sort)
	{
		location = sort;
		
		transform.localPosition = new Vector3(initPosition.x + sort * interv,initPosition.y,initPosition.z);
	}

	public void ChangePosition(int sort)
	{
		location = sort;
		
		Vector3 pos = itemCardList[0].transform.localPosition;
		
		transform.localPosition = new Vector3(initPosition.x + sort * interv,initPosition.y,initPosition.z);
		
		itemCardList[0].transform.localPosition = pos;

		itemCardList[0].Move(pos,Vector3.zero);

		itemCardList[0].ActorTexture.depth = 1;
	}


	public virtual bool GenerateCard(object cardID)
	{
		int id = (int)cardID;

		if(!templateItemTexture.enabled)
			templateItemTexture.enabled = true;

		ItemData itemData = Config.Instance.CardData[id];

		//Texture2D image = LoadAsset.Instance.LoadAssetFromResources(itemData.itemID) as Texture2D;

		if(itemCardList.Count == 0)
		{
			CardItem cardItem = templateItemTexture.gameObject.AddComponent<CardItem>();

			itemCardList.Add(cardItem);
		}

		itemCardList[0].Init(itemData.itemName);

		//itemCardList[0].SetTexture(image,backTexture.width,backTexture.height,location,id);
//
//		return true;
		return true;
	}



	public void SetNotDrag(bool canDrag)
	{
		//itemCardList[0].CanDrag = canDrag;
	}

	public bool SetDrag(int itemID)
	{
		bool canDrag = true;

		if(itemCardList[0].itemID != itemID)
		{
			canDrag = false;
		}
		else
		{
			canDrag = true;
		}

		//itemCardList[0].CanDrag = canDrag;

		return canDrag;
		//return true;
	}

	public void Reset()
	{
		itemCardList.Clear();
	}

	public int GetCardID()
	{
		//return 0;
		return itemCardList[0].itemID;
	}
}
