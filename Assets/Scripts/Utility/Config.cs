using UnityEngine;
using System.Collections.Generic;

public class Config 
{
	private static Config instance;

	public static Config Instance
	{
		get
		{
			if(instance == null)
				instance = new Config();

			return instance;
		}
	}

	public const byte cardPoolSingle = 5;

	public const byte cardCollectionCount = 5;

	public const byte cardInterv = 3;

	public const byte cardDepth = 3;

	public const string battleCardName = "BattleCard";

	public static Vector3 cardPoolInitPosition = new Vector3(-255f,300f,0f);
	
	private Dictionary<int,ItemData> cardData = new Dictionary<int, ItemData>();

	public Dictionary<int,ItemData> CardData
	{
		get{return cardData;}
	}

	private Config()
	{
		ItemData cid;

		for (int i = 0; i < cardPoolSingle; i++) 
		{
			cid = new ItemData(i,"Card"+i,1);
			cardData.Add(cid.itemID,cid);
		}

		Generate();
	}

	void Generate()
	{
		for (int i = 0; i < 20; i++) 
		{
			int key = Random.Range(0, 5);
			cardSort.Enqueue(cardData[key]);
		}
	}
	
	private Queue<ItemData> cardSort = new Queue<ItemData>();

	public ItemData GetCard()
	{
		if(cardSort.Count == 0)
			Generate();

		return cardSort.Dequeue();
	}
}

public class ItemData
{
	public int itemID;

	public string itemName;

	public ResourceEuum resourceEnum;

	public bool isReadyToBattle = false;

	public ItemData(int ID,string name,byte type)
	{
		this.itemID = ID;
		this.itemName = name;

		resourceEnum = (ResourceEuum)type;
	}

}
