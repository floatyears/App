using UnityEngine;
using System.Collections;

public class CardCollectorZone : MonoBehaviour 
{
	public enum States
	{
		Empty,
		Full,
		Spilled
	}

	private GameObject fullMask;
	public States currentState;
	public int unusedAreaCount = 5;

	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 pos3;
	private Vector3 pos4;
	private Vector3 pos5;

	void Start()
	{
		currentState = States.Empty;
		
		pos1 = new Vector3( -0.5F, 0.5F, -1);
		pos2 = new Vector3( -0.5F, -0.5F, -1);
		pos3 = new Vector3( 0.5F, 0.5F, -1);
		pos4 = new Vector3( 0.5F, -0.5F, -1);
		pos5 = new Vector3( 0F, 0F, -2F);

		this.gameObject.layer = SceneConfig.CARD_COLLECTOR_LAYER;	
	}

	void Update()
	{
		CheckFull();
	}

	public Vector3 GetUnusedArea()
	{
		if(unusedAreaCount == 5 )
			return pos1;

		else if(unusedAreaCount == 4 )
			return pos2;

		else if(unusedAreaCount == 3 )
			return pos3;

		else if(unusedAreaCount == 2 )
			return pos4;

		else if( unusedAreaCount == 1 )
			return pos5;

		else
			return Vector3.zero;
	}

	public void AcceptCard( GameObject card )
	{
		card.transform.parent.gameObject.GetComponent<CardCreaterZone>().currentState = CardCreaterZone.States.Empty;
		card.transform.parent = this.gameObject.transform;
		card.transform.localPosition = GetUnusedArea();
		card.transform.localScale = SceneConfig.CARD_SCALE_WHEN_COLLECTED*Vector3.one;
		card.GetComponent<Card>().canMove = false;
		card.layer = SceneConfig.CARD_COLLECTOR_LAYER;
		unusedAreaCount--;
		Debug.Log("End: Accept card");
	}

	public void RefuseCard(GameObject card)
	{
		card.transform.localPosition = Vector3.back*5;
		card.layer = SceneConfig.CARD_UNPICKED_LAYER;
		Debug.Log("End: Refuse card");
	}

	void CheckFull()
	{
		if( unusedAreaCount == 0 )
			currentState = States.Full;
	}
}
