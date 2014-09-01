using UnityEngine;
using System.Collections.Generic;

public class CardPoolMutilItem : CardPoolSingleItem 
{
//	public event UICallback Callback;

	bool isFull = false;

	Vector3 pos ;
	
	Vector3 scale;

	public override void Init (UIInsConfig config)
	{
		base.Init (config);

		//backTexture.depth = GetDepth(-1);

		pos = templateBackTexture.transform.localPosition;

		Destroy(templateItemTexture.gameObject);
		 
		scale =  new Vector3((float)templateBackTexture.width / 2f,(float)templateBackTexture.height / 2f,0f);
	}

	public override bool GenerateCard(object data)
	{
		List<CardItem> item = (List<CardItem>)data;

		for (int i = 0; i < item.Count; i++) 
		{
			if(!isFull)
				AddToList(item[i]);
			else
				return true;
		}

		return true;
	}

	public override void SetInitPosition (int sort)
	{
		base.SetInitPosition (sort);
	}

	void AddToList(CardItem ci)
	{
		GameObject go = NGUITools.AddChild(gameObject,ci.gameObject);

		go.transform.parent = transform;

		go.GetComponent<BoxCollider>().enabled =false;

		CardItem card = go.GetComponent<CardItem>();

//		card.Init(go.name);

		card.ActorTexture.enabled = true;

		TweenPosition tp = go.GetComponent<TweenPosition>();

		TweenScaleExtend ts = go.GetComponent<TweenScaleExtend>();

		tp.enabled = true;

		tp.duration = 0.2f;

		tp.from = ci.transform.localPosition;

		tp.to = GetPosition(itemCardList.Count);

		ts.enabled = true;

		ts.from =new Vector3(card.ActorTexture.width,card.ActorTexture.height,0f);

		ts.to = scale;
	
		card.ActorTexture.depth = GetDepth(itemCardList.Count);

		itemCardList.Add(go.GetComponent<CardItem>());

		if(itemCardList.Count == 5)
			isFull = true;
	}

	Vector3 GetPosition(int sortID)
	{
		Vector3 tempscale = scale / 2f;

		Vector3 tempPos = Vector3.zero;

		switch (sortID) 
		{
		case 0:
			tempPos = new Vector3(pos.x - tempscale.x,pos.y + tempscale.y,pos.z);
			break;
		case 1:
			tempPos = new Vector3(pos.x - tempscale.x,pos.y - tempscale.y,pos.z);
			break;
		case 2:
			tempPos = new Vector3(pos.x + tempscale.x,pos.y + tempscale.y,pos.z);
			break;
		case 3:
			tempPos = new Vector3(pos.x + tempscale.x,pos.y - tempscale.y,pos.z);
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

		if(sortID == 4)
			return 2;
		else
			return 1;
	}

	public int GetCount()
	{
		//return 0;
		return itemCardList.Count;
	}
}
