using UnityEngine;
using System.Collections.Generic;

public class BattleCard : UIBaseUnity
{
	public event Callback CallBack;

	private Vector3[] cardPosition;

	public Vector3[] CardPosition
	{
		set{cardPosition = value;}
	}

	private UITexture templateItemCard;

	private CardItem[] cardItemArray;

	private List<CardItem> moveItem = new List<CardItem>();

	private float cardInterv = 0f;

	public override void Init (string name)
	{
		base.Init (name);

		InitParameter();
	}

	public override void ShowUI ()
	{
		LogHelper.Log("battle card ShowUI");
		base.ShowUI ();
		gameObject.SetActive(true);
	}

	public override void HideUI ()
	{
		LogHelper.Log("battle card HideUI");
		base.HideUI ();
		gameObject.SetActive(false);
	}

	void InitParameter()
	{
		templateItemCard = FindChild<UITexture>("Texture");

		templateItemCard.depth = 1;

		int count = cardPosition.Length;
		
		cardItemArray = new CardItem[count];
		
		for (int i = 0; i < count; i++) 
		{
			tempObject = NGUITools.AddChild(gameObject,templateItemCard.gameObject);
			
			tempObject.transform.localPosition = cardPosition[i];
			
			CardItem ci = tempObject.AddComponent<CardItem>();
			
			ci.location = i;
			
			ci.Init(i.ToString());

			cardItemArray[i] = ci;
		}

		templateItemCard.gameObject.SetActive(false);

		cardInterv = Mathf.Abs(cardPosition[1].x - cardPosition[0].x);
	}
	
	public void GenerateCard(int itemID,int locationID)
	{
		Texture2D tex = LoadAsset.Instance.LoadAssetFromResources(itemID) as Texture2D;

		cardItemArray[locationID].SetTexture(tex,itemID);
	}

	public void IgnoreCollider(bool isIgnore)
	{
		LayerMask layer;

		if(isIgnore)
			layer = GameLayer.ActorCard;
		else
			layer = GameLayer.IgnoreCard;

		for (int i = 0; i < cardItemArray.Length; i++)
		{
			cardItemArray[i].gameObject.layer = layer;
		}
	}

	public void DisposeDrag(int location,int itemID)
	{
		SetFront(location,itemID);
		SetBehind(location,itemID);
	}

	public void ResetDrag()
	{
		for (int i = 0; i < cardItemArray.Length; i++) 
		{
			cardItemArray[i].CanDrag = true;
		}
	}

	public bool SortCard(int sortID,List<CardItem> ci)
	{
		CardItem firstCard = ci[0];

		int index = ci.FindIndex(a =>a.location == sortID);

		if(index >= 0)
			return false;

		bool bigger = sortID > firstCard.location;

		CheckMove(sortID,ci,bigger);

		if(moveItem.Count == 0)
			return false;

		int moveCount = bigger ? -ci.Count : ci.Count;

		for (int i = 0; i < moveItem.Count; i++)
		{
			Vector3 position = moveItem[i].transform.localPosition;

			Vector3 to = new Vector3(position.x + moveCount * cardInterv,position.y,position.z);

			moveItem[i].Move(to);

			moveItem[i].location += moveCount;
		}

		for (int i = 0; i < ci.Count; i++) 
		{
			Vector3 pos = cardItemArray[sortID].transform.localPosition;
			Vector3 to;

			int plus = bigger ? -i : i;

			to = new Vector3(pos.x + plus * cardInterv,pos.y,pos.z);

			ci[i].location = sortID + plus;
			if(i == 0)
				ci[i].SetPos (to);	
			else
				ci[i].Move(to);
		}

		if (ci.Count > 1) {
			ci [ci.Count - 1].tweenCallback += HandletweenCallback;
		}
		else {
			moveItem[moveItem.Count - 1].tweenCallback += HandletweenCallback;;
		}

		moveItem.Clear();

		return true;
	}

	void HandletweenCallback (CardItem arg1)
	{
		arg1.tweenCallback -= HandletweenCallback;

		for (int i = 0; i < cardItemArray.Length - 1; i++) 
		{
			for (int j = i; j < cardItemArray.Length; j++) 
			{
				if(cardItemArray[i].location > cardItemArray[j].location)
				{
					CardItem temp = cardItemArray[i];
					cardItemArray[i] = cardItemArray[j];
					cardItemArray[j] = temp;
				}
			}
		}

		if(CallBack != null)
			CallBack();
	}
	
	void CheckMove(int startID, List<CardItem> firstCard,bool bigger)
	{
		if(firstCard.FindIndex(a => a.location == startID) == -1)
		{
			moveItem.Add(cardItemArray[startID]);

			if(bigger)
				startID --;
			else
				startID ++;

			CheckMove(startID,firstCard,bigger);
		}
		else
			return;
	}

	void SetFront(int sID,int itemID)
	{
		int countID = sID - 1;

		if(countID < 0)
			return;

		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetFront(countID,itemID);
		}
		else
		{
			while(countID > -1)
			{
				cardItemArray[countID].CanDrag = false;
				countID --;
			}
		}
	}

	void SetBehind(int sID,int itemID)
	{
		int countID = sID + 1;

		if(countID >= cardItemArray.Length)
			return;

		if(cardItemArray[countID].SetCanDrag(itemID))
		{
			SetBehind(countID,itemID);
		}
		else
		{
			while(countID < cardItemArray.Length)
			{
				cardItemArray[countID].CanDrag = false;
				countID ++;
			}
		}
	}
}
