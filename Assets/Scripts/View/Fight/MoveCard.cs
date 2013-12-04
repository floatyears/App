using UnityEngine;
using System.Collections;

public class MoveCard : MonoBehaviour 
{
	public GUIText fps;
	private GameObject touchedObj;
	private ArrayList pickedCards = new ArrayList();
	private Vector3 touchedWorldPos;
	private Vector3 touchPos;
	private float timer = 0;
	private int frameRate = 0;
	private Vector3 cardOffset;
	private string currentCardStyle = "";


	public const int UNPICKEDLAYER = 8;
	public const int PICKEDLAYER = 9;
	public const int COLLECTEDLAYER = 10;

	private enum FingerSlideDirection
	{
		Left,
		Right,
		Other
	}
	
	void Start()
	{
		cardOffset = new Vector3(0.3f, -0.3f, 1f);
	}
	void Update()
	{
		CheckFrameRate();

		CheckExitGame();

		CheckFingerStates();
	}

	void CheckFingerStates()
	{
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began )
		{
			Debug.Log("[Fight]:  ******Touch Began******");

			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnpickedLayer" );

			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				Debug.Log("[Fight]:  Began hit card name: "+hit.collider.gameObject.name);

				hit.collider.gameObject.layer = PICKEDLAYER;

				pickedCards.Add(hit.collider.gameObject);

				currentCardStyle = hit.collider.gameObject.GetComponent<Card>().style.ToString();

				Debug.Log("[Fight]:  Began hit card style: "+hit.collider.gameObject.GetComponent<Card>().style.ToString());
			}
			if(pickedCards.Count == 0)
			{
				Debug.Log("[Fight]:  Picked Card Nums: "+pickedCards.Count);

				return;
			}
			Debug.Log("[Fight]:  Picked Card Nums: "+pickedCards.Count);

			int count = 0;

			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + count*cardOffset;//move with finger

				count++;

				Debug.Log("[Fight]:  Picked "+pickedCard.name+" Pos: "+pickedCard.transform.position);
			}
		}
		
		if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved )
		{
			Debug.Log("[Fight]:  ******Touch Moved******");

			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnpickedLayer" );

			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

			RaycastHit hit;			

			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				Debug.Log("[Fight]:  Moved hit card name: "+hit.collider.gameObject.name);

				hit.collider.gameObject.layer = PICKEDLAYER;//modify the current picked card's layer as PickedLayer

				if(hit.collider.gameObject.GetComponent<Card>().style.ToString() != currentCardStyle)
					return;

				pickedCards.Add(hit.collider.gameObject);//add new picked card to CardCollector

				currentCardStyle = hit.collider.gameObject.GetComponent<Card>().style.ToString();

				Debug.Log("[Fight]:  Moved pickedCard Num: "+pickedCards.Count);
			}
			if( pickedCards.Count == 0 )
			{
				Debug.Log("[Fight]:  Moved pickedCard Num: "+pickedCards.Count);

				return;
			}

			Debug.Log("[Fight]:  Moved pickedCard Num: "+pickedCards.Count);

			int count = 0;

			foreach(GameObject pickedCard in pickedCards)
			{
				pickedCard.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + count*cardOffset;//set the current picked card's position

				Debug.Log("[Fight]:  Moved picked Card Name "+pickedCard.name);

				Debug.Log("[Fight]:  Moved picked Card Pos "+pickedCard.transform.position);

				count++;
			}
		}
		
		if( Input.touchCount == 1 &&Input.GetTouch(0).phase == TouchPhase.Ended )
		{
			Debug.Log("[Fight]:  ******Touch Ended******");

			int collectLayer = 1 << LayerMask.NameToLayer ( "CollectedLayer" );

			int unPickedLayer = 1 << LayerMask.NameToLayer ( "UnPickedLayer" );
			
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 100, unPickedLayer))
			{
				Debug.Log("[Fight]:  Ended hit UnpickedCard Name: "+hit.collider.gameObject.name);






			}

			else if(Physics.Raycast(ray, out hit, 100, collectLayer))
			{
				Debug.Log("[Fight]:  Ended hit Collect Pool name: "+hit.collider.gameObject.name);

				Debug.Log("[Fight]:  Ended picked Cards Num: "+pickedCards.Count);

				foreach(GameObject pickedCard in pickedCards)
				{
					if(hit.collider.gameObject.GetComponent<ReadyPool>().currentState == ReadyPool.States.full)
					{

					}

					Debug.Log("[Fight]:  Collect Pool is Not FULL");

					Debug.Log("[Fight]:  Hit Collect Pool name:" + hit.collider.gameObject);

					Debug.Log("[Fight]:  Picked Card's Parent: " + pickedCard.transform.parent.name);
					
					hit.collider.gameObject.GetComponent<ReadyPool>().SetPositionInReadyPool(pickedCard);//set current card's position in order

					pickedCard.layer = COLLECTEDLAYER;//mark current card's layer as collected layer

					Debug.Log("[Fight]:  Hit Collect Pool: " + hit.collider.gameObject.name + "'s NextArea: " + hit.collider.gameObject.GetComponent<ReadyPool>().nextArea);

				}
			}

			else
			{
				Debug.Log("[Fight]:  Touch Ended State: DON'T hit Collect Pool");

				foreach(GameObject pickedCard in pickedCards)
				{
					pickedCard.transform.localPosition = Vector3.back*5;//current card back to pre position

					pickedCard.layer = UNPICKEDLAYER;

					Debug.Log("[Fight]: Current Card's canMove: " + pickedCard.GetComponent<Card>().canMove);

					Debug.Log("[Fight]: Current Card's Layer: " + pickedCard.layer);
				}
			}
			pickedCards.Clear();
		}
	}

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

	void CheckExitGame()
	{
		if(Input.GetKey(KeyCode.Escape)||Input.GetKey(KeyCode.Home))
		{
			Application.Quit();
		}
	}
	
}
