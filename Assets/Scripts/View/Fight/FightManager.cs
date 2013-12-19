using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour 
{
	public GameObject cardCreater;
	public static GameObject[ ] cardCreaterZones;
	private GameObject cardCreaterPrefab;
	public GameObject cardCollector;
	public GameObject[ ] cardCollectorZones; 
	private GameObject cardCollectorPrefab;
	
	private Vector3 CARD_CREATER_BASE_POS = new Vector3(-5, -2.5F, 0);
	private Vector3 CARD_CREATER_POS_OFFSET = new Vector3(2.5F, 0, 0);
	private Vector3 CARD_COLLECTOR_BASE_POS = new Vector3(-5, 0F, 0);
	private Vector3 CARD_COLLECTOR_POS_OFFSET = new Vector3(2.5F, 0, 0);
	
	private float timer = 0;
	private int frameRate = 0;

	public GUIText fps;

	void Start()
	{
		cardCreaterPrefab = Resources.Load("Prefabs/Fight/CardCreaterZone")as GameObject;
		cardCollectorPrefab = Resources.Load("Prefabs/Fight/CardCollectorZone")as GameObject;

		CardCreaterZoneInit();
		CardCollectorZoneInit();
	}

	void Update()
	{
		CheckFrameRate();
	}
	
	void CardCreaterZoneInit()
	{
		cardCreaterZones = new GameObject[ SceneConfig.CARD_CREATER_NUM ];

		for(int count = 0; count < cardCreaterZones.Length; count ++)
		{
			cardCreaterZones[ count ] = Instantiate(cardCreaterPrefab, CARD_CREATER_BASE_POS + count*CARD_CREATER_POS_OFFSET, Quaternion.identity) as GameObject;
			cardCreaterZones[ count ].GetComponent<CardCreaterZone>().ID = count;
			cardCreaterZones[ count ].transform.parent = cardCreater.transform;
		}
	}


	void CardCollectorZoneInit()
	{
		cardCollectorZones = new GameObject[ SceneConfig.CARD_COLLECTOR_NUM ];

		for(int count = 0; count < cardCollectorZones.Length; count ++)
		{
			cardCollectorZones[ count ] = Instantiate(cardCollectorPrefab, CARD_COLLECTOR_BASE_POS + count*CARD_COLLECTOR_POS_OFFSET, Quaternion.identity) as GameObject;
			cardCollectorZones[ count ].transform.parent = cardCollector.transform;
		}
	}
	#region Frame Rate
	void CheckFrameRate()
	{
		frameRate++;
		timer+=Time.deltaTime;
		
		if(timer>1.0f)
		{
			fps.text = frameRate.ToString();
			frameRate=0;
			timer=0f;
		}
	}
	#endregion 

	
}
