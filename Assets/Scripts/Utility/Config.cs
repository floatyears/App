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

	public const byte startCardID = 0;
	public const byte endCardID = 4;
	public const byte cardPoolSingle = 5;

	public const byte cardCollectionCount = 5;

	public const byte cardInterv = 3;

	public const byte cardDepth = 3;

	public const string battleCardName = "BattleCard";
	public int[] cardTypeID = new int[4] {1,2,3,7};

	public static Vector3 cardPoolInitPosition = new Vector3(-255f,300f,0f);
	
	private Dictionary<int,ItemData> cardData = new Dictionary<int, ItemData>();

	public Dictionary<int,ItemData> CardData
	{
		get{return cardData;}
	}

	private Config() {
		ItemData cid;
		for (int i = 1; i < 8; i++) {
			cid = new ItemData(i,"Card"+i,1);
			cardData.Add(cid.itemID,cid);
		}
	}

	void Generate() {
		for (int i = 0; i < 20; i++) {
			int key = Random.Range(0, cardTypeID.Length);
			int id = cardTypeID[key];
			if(ISwitchCard != null){
				id = ISwitchCard.SwitchCard(id);
			}
			cardSort.Enqueue(cardData[id]);
		}
	}

	private Queue<ItemData> cardSort = new Queue<ItemData>();

	public ItemData GetCard() {
		if(cardSort.Count == 0)
			Generate();
		return cardSort.Dequeue();
	}

	ILeaderSkillSwitchCard ISwitchCard;
	public void SwitchCard (ILeaderSkillSwitchCard ilssc) {
		ISwitchCard = ilssc;
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
		if (color == -1) {
			return Color.black;
		}

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
