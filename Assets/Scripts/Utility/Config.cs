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

	public const byte startCardID = 1;
	public const byte endCardID = 8;
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

		for (int i = startCardID; i < endCardID; i++) 
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
			int key = 1; //Random.Range(startCardID, endCardID);
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

	/// <summary>
	/// 0:red; 1:white; 2:blue; 3:green; 4:magenta
	/// </summary>
	public int propertyColor ;

	public ItemData(int ID,string name,byte type)
	{
		this.itemID = ID;
		this.itemName = name;
		propertyColor = ID;
		resourceEnum = (ResourceEuum)type;
	}

	public static Color GetColor(int color)
	{
		switch (color) {
		case 1:
			return Color.red;
		case 2:
			return Color.blue;
		case 3:
			return Color.green;
		case 4:
			return Color.yellow;
		case 5:
			return Color.magenta;
		case 6:
			return Color.white;
		default:
			return Color.white;
			break;
		}
	}
}
