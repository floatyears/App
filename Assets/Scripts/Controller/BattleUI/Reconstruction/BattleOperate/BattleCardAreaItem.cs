using UnityEngine;
using System.Collections.Generic;

public class BattleCardAreaItem : UIBaseUnity
{
	private List<CardItem> cardItemList = new List<CardItem>();
	public List<CardItem> CardItemList 
	{
		get{return cardItemList;}
	}

	private GameObject parentObject;

	private Vector3 scale = Vector3.one;

	private Vector3 utilityScale = Vector3.zero;

	private Vector3 pos = Vector3.zero;

	private float durationTime = 0.2f;

	public override void Init(string name)
	{
		base.Init(name);

		UITexture tex = GetComponent<UITexture>();

		scale.x = ((float)tex.width) / 2f;
		scale.y = ((float)tex.height) / 2f;

		utilityScale = scale / 2f;

		pos = transform.localPosition;

		parentObject = transform.parent.gameObject;
	}

	public int GenerateCard(List<CardItem> source)
	{
		int maxLimit = Config.cardCollectionCount - cardItemList.Count;

		if(maxLimit <= 0)
			return 0;

		maxLimit = maxLimit > source.Count ? source.Count : maxLimit;

		for (int i = 0; i < maxLimit; i++)
		{
			tempObject = NGUITools.AddChild(parentObject,source[i].gameObject);

			tempObject.transform.localPosition = source[i].transform.localPosition;

			tempObject.transform.parent = parentObject.transform;

			CardItem ci = tempObject.GetComponent<CardItem>();

			ci.Init(tempObject.name);

			DisposeTweenPosition(ci);

			DisposeTweenScale(ci);

			ci.ActorTexture.depth = GetDepth(cardItemList.Count);

			cardItemList.Add(ci);
		}

		return maxLimit;
	}

	public void ClearCard()
	{
		for (int i = 0; i < cardItemList.Count; i++)
		{
			cardItemList[i].HideUI();
		}

		cardItemList.Clear();
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
