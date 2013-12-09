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

	private Dictionary<int,ItemData> cardData = new Dictionary<int, ItemData>();

	public Dictionary<int,ItemData> CardData
	{
		get{return cardData;}
	}

	private Config()
	{
		ItemData cid;
		for (int i = 0; i < 4; i++) 
		{
			int j = i +1;
			cid = new ItemData(j,"Card"+j,1);
			cardData.Add(cid.itemID,cid);
		}
	}
}

public class ItemData
{
	public int itemID;

	public string itemName;

	public ResourceEuum resourceEnum;

	public ItemData(int ID,string name,byte type)
	{
		this.itemID = ID;
		this.itemName = name;

		resourceEnum = (ResourceEuum)type;
	}

}
