using UnityEngine;
using System.Collections;

public class CardCreaterZone : MonoBehaviour 
{
	private GameObject cardFire;
	private GameObject cardWater;
	private GameObject cardWind;
	private GameObject cardBlood;

	public int ID;

	public enum States
	{ 
		Empty,
		Full 
	}
	public States currentState;
	private Vector3 cardOffset;
	public GameObject currentCard;

	void Start()
	{
		cardFire = Resources.Load("Prefabs/Fight/Card1")as GameObject;
		cardWater = Resources.Load("Prefabs/Fight/Card2")as GameObject;
		cardWind = Resources.Load("Prefabs/Fight/Card3")as GameObject;
		cardBlood = Resources.Load("Prefabs/Fight/Card4")as GameObject;

		cardOffset = new Vector3(0, 0, -1);
		currentState = States.Empty;

		CheckEmpty();
	}

	void Update()
	{
		CheckEmpty();
	}

	void CheckEmpty()
	{
		if(currentState == States.Empty)
		{
			CreteCardOnRule();
			currentState=States.Full;
		}
	}

	void SetCard(GameObject card, Card.Styles style)
	{
		card.transform.parent = this.gameObject.transform;
		card.transform.localPosition = cardOffset;
		card.GetComponent<Card>().style = style;
		currentCard = card;
	}

	void CreteCardOnRule()
	{
		int cardID=RuleOfCardCreation.OnRule();

		switch(cardID)
		{
		case 1:
			GameObject card1=Instantiate(cardFire, Vector3.zero, Quaternion.identity) as GameObject;
			SetCard(card1, Card.Styles.fire);
			break;
		case 2:
			GameObject card2=Instantiate(cardWater, Vector3.zero, Quaternion.identity) as GameObject;
			SetCard(card2, Card.Styles.water);
			break;
		case 3:
			GameObject card3 = Instantiate(cardWind, Vector3.zero, Quaternion.identity) as GameObject;
			SetCard(card3, Card.Styles.wind);
			break;
		case 4:
			GameObject card4 = Instantiate(cardBlood, Vector3.zero, Quaternion.identity) as GameObject;
			SetCard(card4, Card.Styles.blood);
			break;
		default:
			break;
		}
	}
}
