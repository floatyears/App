using UnityEngine;
using System.Collections;

public class ReadyPool : MonoBehaviour 
{
	public enum States
	{
		empty,
		full,
		disabled
	}
	private GameObject fullMask;

	public States currentState;

	public int nextArea = 1;

	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 pos3;
	private Vector3 pos4;
	private Vector3 pos5;

	private Vector3 maskOffset;

	public void SetPositionInReadyPool(GameObject obj)
	{
		if(nextArea == 1)
		{
			obj.transform.parent.gameObject.GetComponent<CardPoolZone>().currentState = CardPoolZone.States.empty;

			obj.transform.parent = this.gameObject.transform;//set hit collect pool as the current card's parent

			obj.transform.localPosition = pos1;

			obj.transform.localScale = 0.5f*Vector3.one;

			obj.GetComponent<Card>().canMove = false;

			LogHelper.Log("after set: world: "+obj.transform.position+" local: "+obj.transform.localPosition);

			nextArea++;
		}
		else if(nextArea == 2)
		{
			obj.transform.parent.gameObject.GetComponent<CardPoolZone>().currentState = CardPoolZone.States.empty;

			obj.transform.parent = this.gameObject.transform;//set hit collect pool as the current card's parent

			obj.transform.localPosition = pos2;

			obj.transform.localScale = 0.5f*Vector3.one;

			obj.GetComponent<Card>().canMove = false;

			LogHelper.Log("after set: world: "+obj.transform.position+" local: "+obj.transform.localPosition);

			nextArea++;
		}
		else if(nextArea == 3)
		{
			obj.transform.parent.gameObject.GetComponent<CardPoolZone>().currentState = CardPoolZone.States.empty;

			obj.transform.parent = this.gameObject.transform;//set hit collect pool as the current card's parent

			obj.transform.localPosition = pos3;

			obj.transform.localScale = 0.5f*Vector3.one;

			obj.GetComponent<Card>().canMove = false;

			LogHelper.Log("after set: world: "+obj.transform.position+" local: "+obj.transform.localPosition);

			nextArea++;
		}
		else if(nextArea == 4)
		{
			obj.transform.parent.gameObject.GetComponent<CardPoolZone>().currentState = CardPoolZone.States.empty;

			obj.transform.parent = this.gameObject.transform;//set hit collect pool as the current card's parent

			obj.transform.localPosition = pos4;

			obj.transform.localScale = 0.5f*Vector3.one;

			obj.GetComponent<Card>().canMove = false;

			LogHelper.Log("after set: world: "+obj.transform.position+" local: "+obj.transform.localPosition);

			nextArea++;
		}
		else if(nextArea == 5)
		{
			obj.transform.parent.gameObject.GetComponent<CardPoolZone>().currentState = CardPoolZone.States.empty;

			obj.transform.parent = this.gameObject.transform;//set hit collect pool as the current card's parent

			obj.transform.localPosition = pos5;

			obj.transform.localScale = 0.5f*Vector3.one;

			obj.GetComponent<Card>().canMove = false;

			LogHelper.Log("after set: world: "+obj.transform.position+" local: "+obj.transform.localPosition);

			currentState = States.full;

			nextArea++;
		}
		else if(nextArea == 6)
		{
			obj.transform.localPosition = Vector3.back*5;//current card back to pre position
			
			obj.layer = MoveCard.UNPICKEDLAYER;

			LogHelper.Log("Fight: Full Back to Layer: " + obj.layer.ToString());

			LogHelper.Log("Fight: Full Back to Pre Pos: " + obj.transform.position);

			LogHelper.Log("Fight: Full Back to Pre Parent: " + obj.transform.parent.gameObject.name);

			LogHelper.Log("Fight: Full Back to canMove: " + obj.GetComponent<Card>().canMove);
		}

	}

	void Start()
	{
		currentState = States.empty;

		pos1 = new Vector3( -0.5f, 0.5f, -1);

		pos2 = new Vector3( -0.5f, -0.5f, -1);

		pos3 = new Vector3( 0.5f, 0.5f, -1);

		pos4 = new Vector3( 0.5f, -0.5f, -1);

		pos5 = new Vector3( 0, 0, -2);

		maskOffset = new Vector3(0, 0, -3);

		fullMask = ResourceManager.Instance.LoadLocalAsset("Prefabs/Cards/FullMask")as GameObject;

	}

	void Update()
	{
		CheckFull();
	}

	void CheckFull()
	{
		if( currentState == States.full )
		{
			GameObject mask = Instantiate( fullMask, Vector3.zero, Quaternion.identity ) as GameObject;

			mask.transform.parent = this.gameObject.transform;

			mask.transform.localPosition = maskOffset;

			currentState = States.disabled;
		}
	}
}
