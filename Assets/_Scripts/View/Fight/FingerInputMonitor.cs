using UnityEngine;
using System.Collections;

public class FingerInputMonitor : MonoBehaviour
{	
	private ArrayList pickedCards;
	private Vector3 cardOffset;
	private string lastPickedCardStyle = string.Empty;
	private int lastPickedCardID = 0;
	private const int LEFT = -1;
	private const int NONE = 0;
	private const int RIGHT = 1;
	private const int ERROR = -1;
	private Vector3 cardLocalPos;
	private Camera mainCamera;

	void Start()
	{	
		pickedCards = new ArrayList();
		cardOffset = new Vector3(0.3f, -0.3f, 1f);
		cardLocalPos = new Vector3(0, 0, -1);
		mainCamera = Camera.main;
	}

	void Update()
	{
		CheckFingerInput();
	}

	 public void CheckFingerInput()
	{
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began )
		{
			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnpickedCard" );
			Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				lastPickedCardStyle = hit.collider.gameObject.GetComponent<Card>().style.ToString();
				lastPickedCardID = hit.collider.gameObject.transform.parent.gameObject.GetComponent<CardCreaterZone>().ID;
				hit.collider.gameObject.layer = SceneConfig.CARD_PICKED_LAYER;
				
				pickedCards.Add(hit.collider.gameObject);	
			}
			
			if(pickedCards.Count == 0)
				return;
			
			int count = 0;
			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + count*cardOffset;
				count++;
			}
		}

		if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved )
		{
			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnpickedCard" );
			
			Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;			
			
			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				if( lastPickedCardStyle == string.Empty)
				{
					pickedCards.Add(hit.collider.gameObject);
					lastPickedCardStyle = hit.collider.gameObject.GetComponent<Card>().style.ToString();
					lastPickedCardID = hit.collider.gameObject.transform.parent.gameObject.GetComponent<CardCreaterZone>().ID;
				}

				if( lastPickedCardStyle == hit.collider.gameObject.GetComponent<Card>().style.ToString() )
				{
					int startID = lastPickedCardID;	
					int endID = hit.collider.gameObject.transform.parent.gameObject.GetComponent<CardCreaterZone>().ID;

					bool canPick = true;

					if( startID < endID )
					{
						for(int idCount = startID; idCount <= endID; idCount++)
						{
							if( FightManager.cardCreaterZones[ idCount ].GetComponentInChildren<Card>().style.ToString() != lastPickedCardStyle )
							{
								canPick = false;
								break;
							}
						}	
					}
					else
					{
						for(int idCount = endID; idCount <= startID; idCount++)
						{
							if( FightManager.cardCreaterZones[ idCount ].GetComponentInChildren<Card>().style.ToString() != lastPickedCardStyle )
							{
								canPick = false;
								break;
							}
						}	
					}

					if(canPick)
					{
						hit.collider.gameObject.layer = SceneConfig.CARD_PICKED_LAYER;
						pickedCards.Add(hit.collider.gameObject);
						lastPickedCardID = hit.collider.gameObject.transform.parent.gameObject.GetComponent<CardCreaterZone>().ID;
					}
				}

			}
			
			if( pickedCards.Count == 0 )
				return;
			
			int count = 0;
			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + count*cardOffset;
				count++;
			}
		}

		if( Input.touchCount == 1 &&Input.GetTouch(0).phase == TouchPhase.Ended )
		{
			int collectorLayer = 1 << LayerMask.NameToLayer ( "Collector" );
			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnpickedCard" );

			Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				//SwapCards(hit.collider.gameObject);

				foreach(GameObject pickedCard in pickedCards)
				{
					pickedCard.transform.localPosition = Vector3.back*5;
					pickedCard.layer = SceneConfig.CARD_UNPICKED_LAYER;
				}
			}
			
			else if(Physics.Raycast(ray, out hit, 100, collectorLayer))
			{
				foreach(GameObject pickedCard in pickedCards)
				{
					if(hit.collider.gameObject.GetComponent<CardCollectorZone>().currentState == CardCollectorZone.States.Full)
						hit.collider.gameObject.GetComponent<CardCollectorZone>().RefuseCard(pickedCard);

					else
						hit.collider.gameObject.GetComponent<CardCollectorZone>().AcceptCard(pickedCard);
				}
			}
			else
			{
				foreach(GameObject pickedCard in pickedCards)
				{
					pickedCard.transform.localPosition = Vector3.back*5;	
					pickedCard.layer = SceneConfig.CARD_UNPICKED_LAYER;
				}
			}
			pickedCards.Clear();
			lastPickedCardStyle = string.Empty;
			lastPickedCardID = 0;
		}

		if(Input.GetKey(KeyCode.Escape)||Input.GetKey(KeyCode.Home))
		{
			Application.Quit();
		}
	}

	void SwapCards(GameObject hit)
	{
		int moveDirection = NONE;
		int hitIDInUnpickedCards = FindHitIDInUnpickedCards(hit);
		int swapDirection = GetSwapDirection(hitIDInUnpickedCards);
		int pickedCardCount = pickedCards.Count;
	
		if( swapDirection == LEFT)
		{
			moveDirection = RIGHT;
			int minIDInPickedCards = FindMinIDInPickedCards();
			MoveCardsByOrder(hitIDInUnpickedCards, minIDInPickedCards, moveDirection, pickedCardCount);
			PutDownPickedCardIntoZone(hitIDInUnpickedCards,moveDirection);
		}
		else if( swapDirection == RIGHT)
		{
			moveDirection = LEFT;
			int maxIDInPickedCards = FindMaxIDInPickedCards();
			MoveCardsByOrder(hitIDInUnpickedCards, maxIDInPickedCards, moveDirection, pickedCardCount);
			PutDownPickedCardIntoZone(hitIDInUnpickedCards,moveDirection);
		}

	}

	int FindHitIDInUnpickedCards(GameObject hitCard)
	{
		return hitCard.transform.parent.gameObject.GetComponent< CardCreaterZone >().ID;
	}

	int GetSwapDirection(int hitID)
	{
		int originID = lastPickedCardID;
		int endID = hitID;

		if( originID > endID )
			return LEFT;

		if( originID < endID)
			return RIGHT;
		else
			return NONE;
	}

	int GetZoneID(GameObject zone)
	{
		return zone.GetComponent<CardCreaterZone>().ID;
	}
	
	int FindMinIDInPickedCards()
	{
		int[ ] allIDs = new int[ pickedCards.Count ];
		
		int count = 0;
		foreach(GameObject pickedCard in pickedCards )
			allIDs[ count ] = pickedCard.transform.parent.GetComponent< CardCreaterZone >().ID;

		return GetMinID(allIDs);
	}

	int FindMaxIDInPickedCards()
	{
		int[ ] allIDs = new int[ pickedCards.Count ];

		int count = 0;
		foreach(GameObject pickedCard in pickedCards )
			allIDs[ count ] = pickedCard.transform.parent.GetComponent< CardCreaterZone >().ID;

		return GetMaxID(allIDs);
	}

	int GetMaxID( int[ ] ids )
	{
		int maxID= ids[0];

		for(int count = 1; count < ids.Length; count++)
		{
			if( ids[ count ] > maxID )
				maxID = ids[ count ];
		}
		return maxID;
	}

	int GetMinID( int[ ] ids )
	{
		int minID= ids[0];
		
		for(int count = 1; count < ids.Length; count++)
		{
			if( ids[ count ] < minID )
				minID = ids[ count ];
		}
		return minID;
	}

	void MoveCardsByOrder(int hitID, int boundryID, int movDir, int  pickCardCount)
	{
		int moveStep = pickCardCount * movDir;

		if( movDir == RIGHT)
		{
			for(int count = hitID; count < boundryID; count++)
			{
				GameObject currentToMoveCard = FightManager.cardCreaterZones[ count ].GetComponentInChildren<CardCreaterZone>().currentCard;
				currentToMoveCard.transform.parent = FightManager.cardCreaterZones[ count + moveStep].transform;
				currentToMoveCard.transform.localPosition = new Vector3(0,0,-1);
			}
		}
		else if( movDir == LEFT)
		{
			for(int count = boundryID + 1; count <= hitID; count++)
			{
				GameObject currentToMoveCard = FightManager.cardCreaterZones[ count ].GetComponentInChildren<CardCreaterZone>().currentCard;
				
				currentToMoveCard.transform.parent = FightManager.cardCreaterZones[ count + moveStep].transform;
		
				currentToMoveCard.transform.localPosition = new Vector3(0,0,-1);
			}
		}
	}

	void PutDownPickedCardIntoZone(int hitID, int movDir )
	{
		int count = 0;
		if( movDir == RIGHT)
		{
			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.parent = FightManager.cardCreaterZones[ hitID + count*movDir ].transform;
				pickedCard.transform.localPosition = cardLocalPos;
				pickedCard.layer = SceneConfig.CARD_UNPICKED_LAYER;
				count++;
			}
		}
		else if( movDir == LEFT)
		{
			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.parent = FightManager.cardCreaterZones[ hitID + count*movDir ].transform;
				pickedCard.transform.localPosition = cardLocalPos;
				pickedCard.layer = SceneConfig.CARD_UNPICKED_LAYER;
				count++;
			}
		}

	}	
}
